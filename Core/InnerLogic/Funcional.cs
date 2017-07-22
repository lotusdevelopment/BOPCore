using Core.FuncionalWS;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.Configuration;
using ViewModels.Response;

namespace Core.InnerLogic
{
    public class Funcional
    {
        private readonly FuncionalNovoServiceClient _fun = new FuncionalNovoServiceClient();
        private readonly LocationsServices _lcn = new LocationsServices();

        public async Task<List<FuncionalState>> GetStates()
        {
            return await Task<List<FuncionalState>>.Factory.StartNew(() =>
            {
                try
                {
                    var rtn = new List<FuncionalState>();
                    var uf = new UF
                    {
                        Token = GetToken().Result,
                        CodigoCliente = 123
                    };
                    var states = _fun.ListarUF(uf);
                    foreach (var item in states)
                    {
                        rtn.Add(new FuncionalState
                        {
                            state = item.UF,
                            StateName = item.Descricao
                        });
                    }
                    return rtn;
                }
                catch (Exception ex)
                {
                    return new List<FuncionalState>();
                }
            });
        }

        public async Task<List<FuncionalCity>> GetCities(string state)
        {
            return await Task<List<FuncionalCity>>.Factory.StartNew(() =>
            {
                try
                {
                    var rtn = new List<FuncionalCity>();
                    var ciudade = new Cidade
                    {
                        Token = GetToken().Result,
                        UF = state,
                        CodigoCliente = 123
                    };
                    var cities = _fun.ListarCidade(ciudade);
                    foreach (var item in cities)
                    {
                        rtn.Add(new FuncionalCity
                        {
                            city = item.Descricao,
                            localityCode = item.CodigoLocalidade
                        });
                    }
                    return rtn;
                }
                catch (Exception ex)
                {
                    return new List<FuncionalCity>();
                }
            });
        }

        public async Task<List<FuncionalNeighborhood>> GetNeighborhoods(int cityCode)
        {
            return await Task<List<FuncionalNeighborhood>>.Factory.StartNew(() =>
            {
                try
                {
                    var rtn = new List<FuncionalNeighborhood>();
                    var neighborhood = new Bairro
                    {
                        Token = GetToken().Result,
                        CodigoCidade = cityCode,
                        CodigoCliente = 123
                    };
                    var neighborhoods = _fun.ListarBairro(neighborhood);
                    foreach (var item in neighborhoods)
                    {
                        rtn.Add(new FuncionalNeighborhood
                        {
                            neighborhood = item.Descricao,
                            NeighborhoodCode = item.CodigoBairro
                        });
                    }
                    return rtn;
                }
                catch (Exception ex)
                {
                    return new List<FuncionalNeighborhood>();
                }
            });
        }

        public async Task<List<FuncionalAccredited>> GetDrugStores(string lat, string lng, string range)
        {
            return await Task<List<FuncionalAccredited>>.Factory.StartNew(() =>
            {
                try
                {
                    var searchFactor = SearchPlaces(Convert.ToDouble(lat, CultureInfo.InvariantCulture),
                        Convert.ToDouble(lng, CultureInfo.InvariantCulture),
                        float.Parse(range));
                    var rtn = new List<FuncionalAccredited>();
                    var limit = Convert.ToInt32(WebConfigurationManager.AppSettings["MaxResults"]);
                    foreach (var state in searchFactor)
                    {
                        if (rtn.Count >= limit) return rtn;
                        foreach (var city in state.FCities)
                        {
                            foreach (var neig in city.Neighborhoods)
                            {
                                if (rtn.Count >= limit) return rtn;
                                var drg = new RedeCredenciada
                                {
                                    Token = GetToken().Result,
                                    UF = state.state,
                                    CodigoBairro = neig.NeighborhoodCode,
                                    CodigoCidade = city.localityCode,
                                    CodigoCliente = 123
                                };
                                var drgs = _fun.ListarRedeCredenciada(drg);
                                foreach (var item in drgs)
                                {
                                    if (rtn.Count >= limit) return rtn;
                                    rtn.Add(new FuncionalAccredited
                                    {
                                        AccreditedCode = item.CodigoRedeCredenciada,
                                        AccreditedName = item.Nome,
                                        Attendant = item.Responsavel,
                                        Cep = item.Cep,
                                        City = item.NomeCidade,
                                        Email = item.Email,
                                        Latitude = item.Latitude,
                                        Longitude = item.Longitude,
                                        Neighborhood = item.Bairro,
                                        Phone = item.Telefone,
                                        RegionCode = item.CodigoRegiao
                                    });
                                }
                            }
                        }
                    }
                    return rtn; ;
                }
                catch (Exception ex)
                {
                    return new List<FuncionalAccredited>();
                }
            });
        }

        private async Task<string> GetToken()
        {
            try
            {
                return await Task<string>.Factory.StartNew(() =>
                {
                    var token = _fun.AcessarSistema(WebConfigurationManager.AppSettings["FuncionalUser"],
                    WebConfigurationManager.AppSettings["FuncionalPassword"]);
                    return token.Token;
                });
            }
            catch (Exception)
            {
                return null;
            }
        }

        public void CheckAndSaveFuncionalDirections(string filename)
        {
            try
            {
                _lcn.SaveProcessLog("", "---Start Update Funcional---");
                var placesT = new BrazilianPlaces().GetBrazilianPlaces();
                var places = new List<FuncionalState>();
                foreach (var item in placesT)
                {
                    places.Add(new FuncionalState
                    {
                        state = item.state,
                        StateName = item.StateName
                    });
                }
                foreach (var place in places)
                {
                    var innerCities = GetCities(place.state).Result;
                    var citties = new List<FuncionalCity>();
                    foreach (var city in innerCities)
                    {
                        var innerCity = new FuncionalCity()
                        {
                            city = city.city,
                            localityCode = city.localityCode
                        };
                        var neighs = new List<FuncionalNeighborhood>();
                        var innerNeigh = GetNeighborhoods(city.localityCode).Result;
                        foreach (var ngh in innerNeigh)
                        {
                            var ng = new FuncionalNeighborhood()
                            {
                                neighborhood = ngh.neighborhood,
                                NeighborhoodCode = ngh.NeighborhoodCode
                            };
                            //var positioning = _lcn.GetLatLanFromService(place.StateName, city.city, ngh.neighborhood, "BR", "PT");
                            var positioning = "";
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
                    place.FCities = citties;
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

        private List<FuncionalState> SearchPlaces(double lat, double lng, float range)
        {
            try
            {
                var pc = File.ReadAllText(_lcn.GetPaths(2));
                if (string.IsNullOrEmpty(pc)) throw new Exception();
                var places = JsonConvert.DeserializeObject<List<FuncionalState>>(pc);
                var lst = new List<FuncionalState>();
                var r = 6371;
                foreach (var place in places)
                {
                    var ctc = place.FCities;
                    foreach (var city in ctc)
                    {
                        var ngb = new List<FuncionalNeighborhood>();
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
                    place.FCities = ctc.Where(x => x.Neighborhoods.Count > 0).ToList();
                }
                var lst2 = places.Where(x => x.FCities.Count > 0).ToList();
                return lst2;
            }
            catch (Exception e)
            {
                return new List<FuncionalState>();
            }
        }
    }
}