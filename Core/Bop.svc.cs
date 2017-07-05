using Core.DataBase;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;

namespace Core
{
    public class Bop : IBop
    {
        private readonly BopDb _db = new BopDb();

        public async Task<bool> LogIn(string userName, string password)
        {
            return await Task<bool>.Factory.StartNew(() =>
            {
                try
                {
                    var query = "select count(*) from dbo.users where user_cellphone = '" + userName + "'and user_password = '" + password + "'";
                    var sr = _db.Database.SqlQuery<int>(query).FirstOrDefault();
                    return (sr > 1) ? true : false;
                }
                catch (Exception ex)
                {
                    return false;
                }
            });
        }
    }
}
