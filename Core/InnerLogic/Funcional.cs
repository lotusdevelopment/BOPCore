using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;

namespace Core.InnerLogic
{
    public class Funcional
    {
        public async Task<bool> GetDrugStores(string latitude, string longitude)
        {
            return await Task<bool>.Factory.StartNew(() =>
            {
                try
                {

                    return true;
                }
                catch (Exception ex)
                {
                    return false;
                }
            });
        }

        private string GetToken()
        {
            try
            {
                return "";
            }
            catch (Exception)
            {
                return null;
            }
        }
    }
}