namespace Web.Models.Baskets
{
    public class BasketViewModel
    {
        public BasketViewModel()
        {
            _basketItems = new List<BasketItemViewModel>();
        }
        public string UserId { get; set; }
        public string DiscountCode { get; set; }
        public int? DiscountRate { get; set; }
        private List<BasketItemViewModel> _basketItems;
        public decimal TotalPrice { get => _basketItems.Sum(_ => _.GetCurrentPrice); }

        public bool HasDiscount
        {
            get => !string.IsNullOrEmpty(DiscountCode) && DiscountRate.HasValue;
        }

        public List<BasketItemViewModel> BasketItems
        {
            get
            {
                if (HasDiscount)
                    _basketItems.ForEach(_ =>
                    {
                        var discountPrice = _.Price * ((decimal)DiscountRate.Value / 100);
                        _.AppliedDiscount(Math.Round(_.Price - discountPrice, 2));
                    });
                return _basketItems;
            }
            set
            {
                _basketItems = value;
            }
        }

        public void CancelDiscount()
        {
            DiscountCode = null;
            DiscountRate = null;
        }

        public void ApplyDiscount(string code, int rate)
        {
            DiscountCode = code;
            DiscountRate = rate;
        }
    }
}
