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

        public async Task<List<TemAccredited>> GetTemAccredited(string latitude, string longitude, string specialties)
        {
            return await _tem.GetAccredited(latitude, longitude, specialties);
        }

        public async Task<List<GeneralIdName>> GetTemAccreditedDetails()
        {
            return await _tem.GetAccreditedDetails();
        }
    }
}
