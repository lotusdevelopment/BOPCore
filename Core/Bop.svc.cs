using System;
using System.Linq;
using System.Threading.Tasks;
using Core.DataBase;
using Core.InnerLogic;
using ViewModels.General;

namespace Core
{
    public class Bop : IBop
    {
        private readonly BopLogic _bop = new BopLogic();
        private readonly Funcional _fnc = new Funcional();

        /*Bop Methods*/

        public async Task<bool> LogIn(string userName, string password)
        {
            return await _bop.LogIn(userName, password);
        }

        public async Task<GeneralUser> GetUser(string userName)
        {
            return await _bop.GetUser(userName);
        }

        public async void ForgotPassword(string userName)
        {
            await _bop.ForgotPassword(userName);
        }

        public async Task<bool> UpdateUser(GeneralUser user)
        {
            return await _bop.UpdateUser(user);
        }        
    }
}
