using System;
using System.Security.Principal;
using System.Text;
using Apps.App_Start;
using Apps.Resources;
using LotusViewModels.General;
using LotusViewModels.Request;
using LotusViewModels.Response;
using Newtonsoft.Json;

namespace Apps.Logic.Authorize
{
    interface ICustomPrincipal : IPrincipal
    {
        string UserId { get; set; }
        string UserIcard { get; set; }
        string Name { get; set; }
        string UserLastname { get; set; }
        string UserIcardtitular { get; set; }
        int UserUc { get; set; }
        int StatusId { get; set; }
        string UserEmail { get; set; }
        string Password { get; set; }
        int LanguageId { get; set; }
        string UserName { get; set; }
        string SellerCode { get; set; }
        string UserLanguage { get; set; }
        bool PasswordChanged { get; set; }
        DateTime? Birthdate { get; set; }
        string Gender { get; set; }
        string CellPhone { get; set; }
        string Address { get; set; }
        int CountryId { get; set; }
        string Country { get; set; }
        int GroupId { get; set; }
        string Group { get; set; }
        int RoleId { get; set; }
        int Flag { get; set; }
        DateTime LastActivity { get; set; }
    }
}
