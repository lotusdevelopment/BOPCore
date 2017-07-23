using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LotusViewModels.General
{
    public class User
    {
        public string UserId { get; set; }
        public string UserIcard { get; set; }
        public string Name { get; set; }
        public string UserLastname { get; set; }
        public string UserIcardtitular { get; set; }
        public int? UserUc { get; set; }
        public int StatusId { get; set; }
        public string UserEmail { get; set; }
        public string Password { get; set; }
        public int LanguageId { get; set; }
        public string UserName { get; set; }
        public string SellerCode { get; set; }
        public string UserLanguage { get; set; }
        public string UserLanguageDescription { get; set; }
        public bool PasswordChanged { get; set; }
        public DateTime Birthdate { get; set; }
        public string Gender { get; set; }
        public string CellPhone { get; set; }
        public string Address { get; set; }
        public int CountryId { get; set; }
        public string Country { get; set; }
        public int GroupId { get; set; }
        public string Group { get; set; }
        public int RoleId { get; set; }
        public int Flag { get; set; }
        public DateTime LastActivity { get; set; }
        public string ProfilePicture { get; set; }
    }
}
