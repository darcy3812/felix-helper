namespace FelixHelper.Models
{
    public class PromotionModel
    {
        public PromotionModel(string month, string days)
        {
            Month = month;
            Days = days;
        }
        public string Month { get; set; }
        public string Days { get; set; }

        
    }
}
