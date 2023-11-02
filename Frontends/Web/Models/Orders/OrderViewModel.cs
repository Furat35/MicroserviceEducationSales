namespace Web.Models.Orders
{
    public class OrderViewModel
    {
        public int Id { get; set; }

        public DateTime CreatedDate { get; set; }

        //Ödeme geçmişinde adres alanına ihtiyaç duyulmadığından alınmadı.
        //public Address Address { get; set; }
        public string BuyerId { get; set; }

        public List<OrderItemViewModel> OrderItems { get; set; }
    }
}
