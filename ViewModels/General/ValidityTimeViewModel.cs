namespace LotusViewModels.General
{
    public class ValidityTimeViewModel
    {
        public int Id { get; set; }
        public string Description { get; set; }
        public int Days { get; set; }
    }
    public class PaymentPeriodViewModel
    {
        public int Id { get; set; }
        public string Description { get; set; }
        public int Time { get; set; }
    }
    public class InfoContractProdViewModel
    {
        public int Id { get; set; }
        public string Description { get; set; }
        public int LifesQuantity { get; set; }
    }
    public class CurrencyProdViewModel
    {
        public int Id { get; set; }
        public string Description { get; set; }
    }
}
