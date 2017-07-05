using System;
using System.Linq;
using System.Threading.Tasks;
using Core.DataBase;
using Core.InnerLogic;

namespace Core
{
    public class Bop : IBop
    {
        private readonly BopDb _db = new BopDb();
        private readonly Serializers _srz = new Serializers();

        public async Task<bool> LogIn(string userName, string password)
        {
            return await Task<bool>.Factory.StartNew(() =>
            {
                try
                {
                    var pass = _srz.CalculateMD5Hash(password);
                    var query = "select count(*) from dbo.users where user_cellphone = '" + userName + "' and user_password = '" + pass + "'";
                    var sr = _db.Database.SqlQuery<int>(query).FirstOrDefault();
                    return (sr > 0);
                }
                catch (Exception ex)
                {
                    return false;
                }
            });
        }
    }
}
