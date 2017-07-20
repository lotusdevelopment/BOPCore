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

namespace Core.InnerLogic
{
    public class Tem
    {
        private Uri _bA = new Uri("http://private-amnesiac-c0198-tem1.apiary-proxy.com/");
        private readonly Connections _cnn = new Connections();

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

        public async Task<List<TemAccredited>> GetAccredited(string latitude, string longitude, string specialties)
        {
            return await Task<List<TemAccredited>>.Factory.StartNew(() =>
            {
                try
                {
                    var spc = "specialties=";
                    if (!string.IsNullOrEmpty(specialties)) spc += specialties;
                    var searchFactor = SearchPlaces(Convert.ToDouble(latitude), Convert.ToDouble(longitude), 20);
                    var rtn = new List<TemAccredited>();
                    foreach (var item in searchFactor)
                    {
                        var url = string.Concat("health-units/search-units-filter/100/50?", spc, '&');
                        var query = _cnn.GetTemResponseGetAsync(_bA, url);
                        var deve = JsonConvert.DeserializeObject<TemAccreditedResult>(query);
                        foreach (var item2 in deve.data) rtn.Add(item2);
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
                var pc = File.ReadAllText(GetPaths(1));
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
                            var mlat = neigh.Latitude;
                            var mlng = neigh.Longitude;
                            var dLat = Rad(Convert.ToDouble(mlat) - lat);
                            var dLong = Rad(Convert.ToDouble(mlng) - lng);
                            var a = Math.Sin(dLat / 2) * Math.Sin(dLat / 2) +
                                Math.Cos(Rad(lat)) * Math.Cos(Rad(lat)) * Math.Sin(dLong / 2) * Math.Sin(dLong / 2);
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
                ///*Separate keys in order to get fun*/
                //var states = GetStates().Result;
                //var cities = new List<Dictionary<string, List<TemCity>>>();
                //var neigh = new List<Dictionary<string, List<TemNeighborhood>>>();
                //foreach (var state in states)
                //{
                //    var dct = new Dictionary<string, List<TemCity>>
                //{
                //    { state.state, GetCities(state.state).Result }
                //};
                //    cities.Add(dct);
                //}
                //foreach (var city in cities)
                //{
                //    foreach (var cit in city.Values.FirstOrDefault())
                //    {
                //        var dct = new Dictionary<string, List<TemNeighborhood>>
                //    {
                //        { cit.city, GetNeighborhoods(city.Keys.FirstOrDefault(), cit.city).Result }
                //    };
                //        neigh.Add(dct);
                //    }
                //}

                /*All in one place for specific searching*/

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
                            var positioning = GetLatLanFromService(place.StateName, city.city, ngh.neighborhood, "BR", "PT");
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

        private string GetLatLanFromService(string state, string city, string neigh, string country, string language)
        {
            try
            {
                var gUrl = string.Concat("https://maps.googleapis.com/maps/api/geocode/json?address=", neigh,
                    "&components=country:", country, "|administrative_area:", state,
                    "&key=", WebConfigurationManager.AppSettings["GoogleTkey"]);
                var response = _cnn.GetResponseGet(gUrl);
                var obj = JsonConvert.DeserializeObject<GMapsResponse>(response);
                return string.Concat(obj.results.FirstOrDefault().geometry.location.lat,
                    ',', obj.results.FirstOrDefault().geometry.location.lng);
            }
            catch (Exception e)
            {
                return "";
            }
        }

        private double Rad(double x)
        {
            return x * Math.PI / 180;
        }

        public string GetPaths(int type)
        {
            var pathR = "";
            switch (type)
            {
                case 1:
                    var path = AppDomain.CurrentDomain.BaseDirectory;
                    var dir = Path.GetDirectoryName(path);
                    dir += "\\Helpers";
                    if (!Directory.Exists(dir)) Directory.CreateDirectory(dir);
                    pathR = dir + "\\Tem_places.json";
                    break;
            }
            return pathR;
        }
    }
}