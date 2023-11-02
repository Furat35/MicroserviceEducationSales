using Web.Models.Catalogs;

namespace Web.Services.Interfaces
{
    public interface ICatalogService
    {
        Task<List<CourseViewModel>> GetAllCourseAsync();
        Task<List<CategoryViewModel>> GetAllCategoriesAsync();
        Task<List<CourseViewModel>> GetAllCourseByUserIdAsync(string userId);
        Task<CourseViewModel> GetByCourseIdAsync(string userId);
        Task<bool> CreateCourseAsync(CourseCreateInput input);
        Task<bool> UpdateCourseAsync(CourseUpdateInput input);
        Task<bool> DeleteCourseAsync(string courseId);
    }
}
