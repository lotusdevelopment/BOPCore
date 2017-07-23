using ViewModels.General;

namespace ViewModels
{
    public class SaleRequest
    {
        public AuthorizeModel Credentials { get; set; }
        public string CustomerId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Gender { get; set; }
        public string Birthday { get; set; }
        public string Email { get; set; }
        public string Cellphone { get; set; }
        public string MotherName { get; set; }
        public string City { get; set; }
        public string State { get; set; }
        public string ZipCode { get; set; }
        public string Street { get; set; }
        public string PlateNumber { get; set; }
        public string Complement { get; set; }
        public string Neighborhood { get; set; }
        public string Plan { get; set; }
        public int PaymentType { get; set; }
        public string Url { get; set; }
        public string BuyerName { get; set; }
        public string BuyerCellPhone { get; set; }
        public string BuyerEmail { get; set; }
        public string SellerCode { get; set; }
        public string CountryId { get; set; }
        public string CompanySaleId { get; set; }
        public ClientCard CardData { get; set; }
        public string CountryName { get; set; }
        public string SaleDateTime { get; set; }
    }
}
