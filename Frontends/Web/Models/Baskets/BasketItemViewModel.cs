﻿namespace Web.Models.Baskets
{
    public class BasketItemViewModel
    {
        public int Quantity { get; } = 1;
        public string CourseId { get; set; }
        public string CourseName { get; set; }
        public decimal Price { get; set; }
        private decimal? DiscountAppliedPrice;
        public decimal GetCurrentPrice { get => DiscountAppliedPrice ?? Price; }
        public void AppliedDiscount(decimal discountAppliedPrice)
        {
            DiscountAppliedPrice = discountAppliedPrice;
        }
    }
}
