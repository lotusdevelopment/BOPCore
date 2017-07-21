using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using ViewModels.Response;
using System.Runtime.Caching;
using System.Collections.Specialized;
using System.Web.Configuration;
using ViewModels.General;
using System.Device.Location;
using SaintModeCaching;
using System.IO;
using System.Text;
using System.Globalization;

namespace Core.InnerLogic
{
    public class Tem
    {
        private Uri _bA = new Uri("http://private-amnesiac-c0198-tem1.apiary-proxy.com/");
        private readonly Connections _cnn = new Connections();
        private readonly LocationsServices _lcn = new LocationsServices();

        public async Task<List<GeneralIdName>> GetSpecialties()
        {
            return await Task<List<GeneralIdName>>.Factory.StartNew(() =>
            {
                try
                {
                    var rtn = _cnn.GetTemResponseGetAsync(_bA, "health-units-specialties");
                    return JsonConvert.DeserializeObject<List<GeneralIdName>>(rtn);
                }
                catch (Exception ex)
                {
                    return new List<GeneralIdName>();
                }
            });
        }

        public async Task<List<TemState>> GetStates()
        {
            return await Task<List<TemState>>.Factory.StartNew(() =>
            {
                try
                {
                    var rtn = _cnn.GetTemResponseGetAsync(_bA, "health-units-states");
                    return JsonConvert.DeserializeObject<List<TemState>>(rtn);
                }
                catch (Exception ex)
                {
                    return new List<TemState>();
                }
            });
        }

        public async Task<List<TemCity>> GetCities(string state)
        {
            return await Task<List<TemCity>>.Factory.StartNew(() =>
            {
                try
                {
                    var lst = new List<string>
                    {
                        state
                    };
                    var rtn = _cnn.GetTemResponseGetAsync(_bA, "health-units-cities", lst);
                    return JsonConvert.DeserializeObject<List<TemCity>>(rtn);
                }
                catch (Exception ex)
                {
                    return new List<TemCity>();
                }
            });
        }

        public async Task<List<TemNeighborhood>> GetNeighborhoods(string state, string city)
        {
            return await Task<List<TemNeighborhood>>.Factory.StartNew(() =>
            {
                try
                {
                    var lst = new List<string>
                    {
                        state,
                        city
                    };
                    var rtn = _cnn.GetTemResponseGetAsync(_bA, "health-units-neighborhoods", lst);
                    return JsonConvert.DeserializeObject<List<TemNeighborhood>>(rtn);
                }
                catch (Exception ex)
                {
                    return new List<TemNeighborhood>();
                }
            });
        }

        public async Task<List<TemAccredited>> GetAccredited(string latitude, string longitude, string range)
        {
            return await Task<List<TemAccredited>>.Factory.StartNew(() =>
            {
                try
                {
                    var spc = "specialties=";
                    var searchFactor = SearchPlaces(Convert.ToDouble(latitude, CultureInfo.InvariantCulture),
                        Convert.ToDouble(longitude, CultureInfo.InvariantCulture),
                        float.Parse(range));
                    var rtn = new List<TemAccredited>();
                    var url = string.Concat("health-units/search-units-filter/100/50?", spc, '&');
                    var limit = Convert.ToInt32(WebConfigurationManager.AppSettings["MaxResults"]);
                    foreach (var state in searchFactor)
                    {
                        foreach (var city in state.Cities)
                        {
                            foreach (var neig in city.Neighborhoods)
                            {
                                if (rtn.Count >= limit) return rtn;
                                var nUrl = url + "state=" + state.state + "&city=" + city.city + "&neighborhood=" + neig.neighborhood;
                                var query = _cnn.GetTemResponseGetAsync(_bA, nUrl);
                                var deve = JsonConvert.DeserializeObject<TemAccreditedResult>(query);
                                foreach (var item in deve.data)
                                {
                                    if (rtn.Count >= limit) return rtn;
                                    var cUrl = "health-unit/" + item.DT_RowId;
                                    query = _cnn.GetTemResponseGetAsync(_bA, cUrl);
                                    var nDeve = JsonConvert.DeserializeObject<TemAccredited>(query);
                                    item.address = nDeve.address;
                                    item.email = nDeve.email;
                                    item.type = nDeve.type;
                                    item.address_type = nDeve.address_type;
                                    item.postal_code = nDeve.postal_code;
                                    item.number = nDeve.number;
                                    item.complement = nDeve.complement;
                                    var positioning = _lcn.GetLatLanFromAddress(item.address_type, item.address,
                                        item.number, item.complement, "BR");
                                    if (!string.IsNullOrEmpty(positioning))
                                    {
                                        var positioning2 = positioning.Split(',');
                                        item.Latitude = positioning2[0];
                                        item.Longitude = positioning2[1];
                                    }
                                    rtn.Add(item);
                                }
                            }
                        }
                    }
                    return rtn;
                }
                catch (Exception ex)
                {
                    return new List<TemAccredited>();
                }
            });
        }

        public async Task<List<GeneralIdName>> GetAccreditedDetails()
        {
            return await Task<List<GeneralIdName>>.Factory.StartNew(() =>
            {
                try
                {
                    var rtn = _cnn.GetTemResponseGetAsync(_bA, "health-units-specialties");
                    return JsonConvert.DeserializeObject<List<GeneralIdName>>(rtn);
                }
                catch (Exception ex)
                {
                    return new List<GeneralIdName>();
                }
            });
        }

        private List<CustomState> SearchPlaces(double lat, double lng, float range)
        {
            try
            {
                var pc = File.ReadAllText(_lcn.GetPaths(1));
                if (string.IsNullOrEmpty(pc)) throw new Exception();
                var places = JsonConvert.DeserializeObject<List<CustomState>>(pc);
                var lst = new List<CustomState>();
                var r = 6371;
                foreach (var place in places)
                {
                    var ctc = place.Cities;
                    foreach (var city in ctc)
                    {
                        var ngb = new List<CustomNeighborhood>();
                        foreach (var neigh in city.Neighborhoods)
                        {
                            var mlat = Convert.ToDouble(neigh.Latitude, CultureInfo.InvariantCulture);
                            var mlng = Convert.ToDouble(neigh.Longitude, CultureInfo.InvariantCulture);
                            var dLat = _lcn.Rad(mlat - lat);
                            var dLong = _lcn.Rad(mlng - lng);
                            var a = Math.Sin(dLat / 2) * Math.Sin(dLat / 2) +
                                Math.Cos(_lcn.Rad(lat)) * Math.Cos(_lcn.Rad(lat)) * Math.Sin(dLong / 2) * Math.Sin(dLong / 2);
                            var c = 2 * Math.Atan2(Math.Sqrt(a), Math.Sqrt(1 - a));
                            var d = r * c;
                            if (d <= range) ngb.Add(neigh);
                        }
                        city.Neighborhoods = ngb;
                    }
                    place.Cities = ctc.Where(x => x.Neighborhoods.Count > 0).ToList();
                }
                var lst2 = places.Where(x => x.Cities.Count > 0).ToList();
                return lst2;
            }
            catch (Exception e)
            {
                return new List<CustomState>();
            }
        }

        public void CheckAndSaveTemDirections(string filename)
        {
            try
            {
                _lcn.SaveProcessLog("", "---Start Update Tem---");
                var places = new BrazilianPlaces().GetBrazilianPlaces();
                foreach (var place in places)
                {
                    var innerCities = GetCities(place.state).Result;
                    var citties = new List<CustomCity>();
                    foreach (var city in innerCities)
                    {
                        var innerCity = new CustomCity()
                        {
                            city = city.city
                        };
                        var neighs = new List<CustomNeighborhood>();
                        var innerNeigh = GetNeighborhoods(place.state, city.city).Result;
                        foreach (var ngh in innerNeigh)
                        {
                            var ng = new CustomNeighborhood()
                            {
                                neighborhood = ngh.neighborhood
                            };
                            var positioning = _lcn.GetLatLanFromService(place.StateName, city.city, ngh.neighborhood, "BR", "PT");
                            if (!string.IsNullOrEmpty(positioning))
                            {
                                var positioning2 = positioning.Split(',');
                                ng.Latitude = positioning2[0];
                                ng.Longitude = positioning2[1];
                            }
                            neighs.Add(ng);
                        }
                        innerCity.Neighborhoods = neighs;
                        citties.Add(innerCity);
                    }
                    place.Cities = citties;
                }

                using (var fs = File.Create(filename))
                {
                    var dataasstring = JsonConvert.SerializeObject(places);
                    var info = new UTF8Encoding(true).GetBytes(dataasstring);
                    fs.Write(info, 0, info.Length);
                }
            }
            catch (Exception)
            {

                throw;
            }
        }
    }
}