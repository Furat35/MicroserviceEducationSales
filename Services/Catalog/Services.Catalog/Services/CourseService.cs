using AutoMapper;
using Mass = MassTransit;
using MongoDB.Driver;
using Services.Catalog.Dtos;
using Services.Catalog.Models;
using Services.Catalog.Services.Interfaces;
using Services.Catalog.Services.Settings;
using Shared.Dtos;
using Shared.Messages;

namespace Services.Catalog.Services
{
    public class CourseService : ICourseService
    {
        private readonly IMongoCollection<Course> _courseCollection;
        private readonly IMongoCollection<Category> _categoryCollection;
        private readonly IMapper _mapper;
        private readonly Mass.IPublishEndpoint _publishEndpoint;
        public CourseService(IDatabaseSettings databaseSettings, IMapper mapper, Mass.IPublishEndpoint publishEndpoint)
        {
            var client = new MongoClient(databaseSettings.ConnectionString);
            var database = client.GetDatabase(databaseSettings.DatabaseName);
            _courseCollection = database.GetCollection<Course>(databaseSettings.CourseCollectionName);
            _categoryCollection = database.GetCollection<Category>(databaseSettings.CategoryCollectionName);
            _mapper = mapper;
            _publishEndpoint = publishEndpoint;
        }

        public async Task<Response<List<CourseDto>>> GetAllAsync()
        {
            var courses = await _courseCollection.Find(course => true).ToListAsync();
            if (courses.Any())
                foreach (var course in courses)
                    course.Category = await _categoryCollection.Find(_ => _.Id == course.CategoryId).FirstOrDefaultAsync();
            else
                courses = new List<Course>();
            return Response<List<CourseDto>>.Success(_mapper.Map<List<CourseDto>>(courses), 200);
        }

        public async Task<Response<CourseDto>> GetByIdAsync(string id)
        {
            var course = await _courseCollection.Find(_ => _.Id == id).FirstOrDefaultAsync();
            if (course is null)
                Response<CourseDto>.Fail("Category not found", 404);
            course.Category = await _categoryCollection.Find(_ => _.Id == course.CategoryId).FirstAsync();
            return Response<CourseDto>.Success(_mapper.Map<CourseDto>(course), 200);
        }

        public async Task<Response<List<CourseDto>>> GetAllByUserId(string userId)
        {
            var courses = await _courseCollection.Find(course => course.UserId == userId).ToListAsync();
            if (courses.Any())
                foreach (var course in courses)
                    course.Category = await _categoryCollection.Find(_ => _.Id == course.CategoryId).FirstAsync();
            else
                courses = new List<Course>();
            return Response<List<CourseDto>>.Success(_mapper.Map<List<CourseDto>>(courses), 200);
        }

        public async Task<Response<CourseDto>> CreateAsync(CourseCreateDto courseCreateDto)
        {
            var newCourse = _mapper.Map<Course>(courseCreateDto);
            newCourse.CreatedTime = DateTime.Now;
            await _courseCollection.InsertOneAsync(newCourse);
            return Response<CourseDto>.Success(_mapper.Map<CourseDto>(newCourse), 200);
        }

        public async Task<Response<NoContent>> UpdateAsync(CourseUpdateDto courseUpdateDto)
        {
            var updateCourse = _mapper.Map<Course>(courseUpdateDto);
            var result = await _courseCollection.FindOneAndReplaceAsync(_ => _.Id == courseUpdateDto.Id, updateCourse);
            await _publishEndpoint.Publish(new CourseNameChangedEvent { CourseId = updateCourse.Id, UpdatedName = updateCourse.Name });
            return result is null
                 ? Response<NoContent>.Fail("Course not found", 404)
                 : Response<NoContent>.Success(204);
        }

        public async Task<Response<NoContent>> DeleteAsync(string id)
        {
            var result = await _courseCollection.DeleteOneAsync(_ => _.Id == id);
            return result.DeletedCount > 0
                ? Response<NoContent>.Success(204)
                : Response<NoContent>.Fail("Course not found", 404);
        }
    }
}

