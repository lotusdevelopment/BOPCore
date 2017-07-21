using Core.InnerLogic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;
using ViewModels.Response;

namespace Core
{
    public class External : IExternal
    {
        private readonly Tem _tem = new Tem();
        private readonly Funcional _fun = new Funcional();

        public async Task<List<GeneralIdName>> GetTemSpecialties()
        {
            return await _tem.GetSpecialties();
        }

        public async Task<List<TemState>> GetTemStates()
        {
            return await _tem.GetStates();
        }

        public async Task<List<TemCity>> GetTemCities(string state)
        {
            return await _tem.GetCities(state);
        }

        public async Task<List<TemNeighborhood>> GetTemNeighborhoods(string state, string city)
        {
            return await _tem.GetNeighborhoods(state, city);
        }

        public async Task<List<TemAccredited>> GetTemAccredited(string latitude, string longitude,  string range)
        {
            return await _tem.GetAccredited(latitude, longitude, range);
        }

        public async Task<List<GeneralIdName>> GetTemAccreditedDetails()
        {
            return await _tem.GetAccreditedDetails();
        }

        public async Task<List<FuncionalState>> GetFuncionalStates()
        {
            return await _fun.GetStates();
        }

        public async Task<List<FuncionalCity>> GetFuncionalCities(string state)
        {
            return await _fun.GetCities(state);
        }

        public async Task<List<FuncionalAccredited>> GetFuncionalDrugStores(string latitude, string longitude, string range)
        {
            return await _fun.GetDrugStores(latitude, longitude, range);
        }

        public async Task<List<FuncionalNeighborhood>> GetFuncionalNeighborhoods(string cityCode)
        {
            return await _fun.GetNeighborhoods(Convert.ToInt32(cityCode));
        }
    }
}
