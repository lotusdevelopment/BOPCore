using System.Collections.Generic;
using ViewModels;
using ViewModels.General;

namespace LotusViewModels.Request
{
    public class SaleViewModel
    {
        public AuthorizeModel Authorization { get; set; }
        public CustomerInformation CustomerInformation { get; set; }
        public SaleInformation SaleInformation { get; set; }
    }
    public class CustomerInformation
    {
        public MainCustomer MainCustomer { get; set; }
        public List<Beneficiary> Beneficiaries { get; set; }
        public BuyerInfo BuyerInfo { get; set; }
    }
    public class SaleInformation
    {
        public int ProductId { get; set; }
        public int PaymentType { get; set; }
        public string SellerCode { get; set; }
        public int CountryId { get; set; }
        public string CompanySaleId { get; set; }
        public ClientCard CardData { get; set; }
    }
    public class MainCustomer
    {
        public string CustomerId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Gender { get; set; }
        public string Birthday { get; set; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; }
        public int CountryCode { get; set; }
        public MobileProperties MobileProperties { get; set; }
        public string MotherName { get; set; }
        public string City { get; set; }
        public string State { get; set; }
        public string ZipCode { get; set; }
        public string Street { get; set; }
        public string PlateNumber { get; set; }
        public string Complement { get; set; }
        public string Neighborhood { get; set; }
    }
    public class BuyerInfo
    {
        public string BuyerName { get; set; }
        public string BuyerCellPhone { get; set; }
        public string BuyerEmail { get; set; }
    }
    public class MobileProperties
    {
        public string Mcc { get; set; }
        public string Mnc { get; set; }
        public string NetworkName { get; set; }
    }
}
