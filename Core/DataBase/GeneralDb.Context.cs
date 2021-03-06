﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace Core.DataBase
{
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Infrastructure;
    using System.Data.Entity.Core.Objects;
    using System.Linq;
    
    public partial class GeneralDb : DbContext
    {
        public GeneralDb()
            : base("name=GeneralDb")
        {
        }
    
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            throw new UnintentionalCodeFirstException();
        }
    
        public virtual DbSet<Excepciones> Excepciones { get; set; }
        public virtual DbSet<LogWs> LogWs { get; set; }
    
        public virtual ObjectResult<string> SaveAllTheSales(string customerId, string customerFirstName, string customerLastName, Nullable<System.DateTime> customerBirthday, string customerEmail, string customerCellphone, string customerGender, Nullable<int> countryId, string statesId, string cityId, string customerZipCode, string customerStreet, string customerPlateNumber, string customerComplement, string customerNameMother, Nullable<int> planId, Nullable<int> paymentType, string sellerCode, string buyerName, string buyerCellPhone, string buyerEmail, string companySaleId, Nullable<int> mainly, string mainlyId, string url, Nullable<int> bus, Nullable<System.DateTime> beginingDate, Nullable<System.DateTime> endDate, Nullable<System.DateTime> dateSaleReal)
        {
            var customerIdParameter = customerId != null ?
                new ObjectParameter("CustomerId", customerId) :
                new ObjectParameter("CustomerId", typeof(string));
    
            var customerFirstNameParameter = customerFirstName != null ?
                new ObjectParameter("CustomerFirstName", customerFirstName) :
                new ObjectParameter("CustomerFirstName", typeof(string));
    
            var customerLastNameParameter = customerLastName != null ?
                new ObjectParameter("CustomerLastName", customerLastName) :
                new ObjectParameter("CustomerLastName", typeof(string));
    
            var customerBirthdayParameter = customerBirthday.HasValue ?
                new ObjectParameter("CustomerBirthday", customerBirthday) :
                new ObjectParameter("CustomerBirthday", typeof(System.DateTime));
    
            var customerEmailParameter = customerEmail != null ?
                new ObjectParameter("CustomerEmail", customerEmail) :
                new ObjectParameter("CustomerEmail", typeof(string));
    
            var customerCellphoneParameter = customerCellphone != null ?
                new ObjectParameter("CustomerCellphone", customerCellphone) :
                new ObjectParameter("CustomerCellphone", typeof(string));
    
            var customerGenderParameter = customerGender != null ?
                new ObjectParameter("CustomerGender", customerGender) :
                new ObjectParameter("CustomerGender", typeof(string));
    
            var countryIdParameter = countryId.HasValue ?
                new ObjectParameter("CountryId", countryId) :
                new ObjectParameter("CountryId", typeof(int));
    
            var statesIdParameter = statesId != null ?
                new ObjectParameter("StatesId", statesId) :
                new ObjectParameter("StatesId", typeof(string));
    
            var cityIdParameter = cityId != null ?
                new ObjectParameter("CityId", cityId) :
                new ObjectParameter("CityId", typeof(string));
    
            var customerZipCodeParameter = customerZipCode != null ?
                new ObjectParameter("CustomerZipCode", customerZipCode) :
                new ObjectParameter("CustomerZipCode", typeof(string));
    
            var customerStreetParameter = customerStreet != null ?
                new ObjectParameter("CustomerStreet", customerStreet) :
                new ObjectParameter("CustomerStreet", typeof(string));
    
            var customerPlateNumberParameter = customerPlateNumber != null ?
                new ObjectParameter("CustomerPlateNumber", customerPlateNumber) :
                new ObjectParameter("CustomerPlateNumber", typeof(string));
    
            var customerComplementParameter = customerComplement != null ?
                new ObjectParameter("CustomerComplement", customerComplement) :
                new ObjectParameter("CustomerComplement", typeof(string));
    
            var customerNameMotherParameter = customerNameMother != null ?
                new ObjectParameter("CustomerNameMother", customerNameMother) :
                new ObjectParameter("CustomerNameMother", typeof(string));
    
            var planIdParameter = planId.HasValue ?
                new ObjectParameter("PlanId", planId) :
                new ObjectParameter("PlanId", typeof(int));
    
            var paymentTypeParameter = paymentType.HasValue ?
                new ObjectParameter("PaymentType", paymentType) :
                new ObjectParameter("PaymentType", typeof(int));
    
            var sellerCodeParameter = sellerCode != null ?
                new ObjectParameter("SellerCode", sellerCode) :
                new ObjectParameter("SellerCode", typeof(string));
    
            var buyerNameParameter = buyerName != null ?
                new ObjectParameter("BuyerName", buyerName) :
                new ObjectParameter("BuyerName", typeof(string));
    
            var buyerCellPhoneParameter = buyerCellPhone != null ?
                new ObjectParameter("BuyerCellPhone", buyerCellPhone) :
                new ObjectParameter("BuyerCellPhone", typeof(string));
    
            var buyerEmailParameter = buyerEmail != null ?
                new ObjectParameter("BuyerEmail", buyerEmail) :
                new ObjectParameter("BuyerEmail", typeof(string));
    
            var companySaleIdParameter = companySaleId != null ?
                new ObjectParameter("CompanySaleId", companySaleId) :
                new ObjectParameter("CompanySaleId", typeof(string));
    
            var mainlyParameter = mainly.HasValue ?
                new ObjectParameter("Mainly", mainly) :
                new ObjectParameter("Mainly", typeof(int));
    
            var mainlyIdParameter = mainlyId != null ?
                new ObjectParameter("MainlyId", mainlyId) :
                new ObjectParameter("MainlyId", typeof(string));
    
            var urlParameter = url != null ?
                new ObjectParameter("Url", url) :
                new ObjectParameter("Url", typeof(string));
    
            var busParameter = bus.HasValue ?
                new ObjectParameter("Bus", bus) :
                new ObjectParameter("Bus", typeof(int));
    
            var beginingDateParameter = beginingDate.HasValue ?
                new ObjectParameter("BeginingDate", beginingDate) :
                new ObjectParameter("BeginingDate", typeof(System.DateTime));
    
            var endDateParameter = endDate.HasValue ?
                new ObjectParameter("EndDate", endDate) :
                new ObjectParameter("EndDate", typeof(System.DateTime));
    
            var dateSaleRealParameter = dateSaleReal.HasValue ?
                new ObjectParameter("DateSaleReal", dateSaleReal) :
                new ObjectParameter("DateSaleReal", typeof(System.DateTime));
    
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction<string>("SaveAllTheSales", customerIdParameter, customerFirstNameParameter, customerLastNameParameter, customerBirthdayParameter, customerEmailParameter, customerCellphoneParameter, customerGenderParameter, countryIdParameter, statesIdParameter, cityIdParameter, customerZipCodeParameter, customerStreetParameter, customerPlateNumberParameter, customerComplementParameter, customerNameMotherParameter, planIdParameter, paymentTypeParameter, sellerCodeParameter, buyerNameParameter, buyerCellPhoneParameter, buyerEmailParameter, companySaleIdParameter, mainlyParameter, mainlyIdParameter, urlParameter, busParameter, beginingDateParameter, endDateParameter, dateSaleRealParameter);
        }
    }
}
