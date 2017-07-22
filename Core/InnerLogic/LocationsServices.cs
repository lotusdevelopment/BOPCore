using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.Configuration;
using ViewModels.General;

namespace Core.InnerLogic
{
    public class LocationsServices
    {
        private readonly Connections _cnn = new Connections();

        public async Task<string> GetLatLanFromService(string state, string city, string neigh, string country, string language)
        {
            return await Task<string>.Factory.StartNew(() =>
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
            });

        }

        public string GetLatLanFromAddress(string address, string number, string type, string complement, string language)
        {
            try
            {
                var gUrl = string.Concat("https://maps.googleapis.com/maps/api/geocode/json?address=", type,
                    "+", address, "+", number, "+", complement,
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

        public double Rad(double x)
        {
            return x * Math.PI / 180;
        }

        public string GetPaths(int type, string name = "")
        {
            var pathR = "";
            var path = AppDomain.CurrentDomain.BaseDirectory;
            var dir = Path.GetDirectoryName(path);
            try
            {
                switch (type)
                {
                    case 1:
                        dir += "\\Helpers";
                        if (!Directory.Exists(dir)) Directory.CreateDirectory(dir);
                        pathR = dir + "\\Tem_places.json";
                        break;
                    case 2:
                        dir += "\\Helpers";
                        if (!Directory.Exists(dir)) Directory.CreateDirectory(dir);
                        pathR = dir + "\\Funcional_places.json";
                        break;
                    case 3:
                        dir += "\\Logs";
                        if (!string.IsNullOrEmpty(name))
                            pathR = dir + "\\LogProcess" + name + " - " +
                                DateTime.Now.ToString("yyyy-MM-dd hh-mm-ss.fff") + ".txt";
                        else
                            pathR = dir + "\\LogProcess" + DateTime.Now.ToString("yyyy-MM-dd hh-mm-ss.fff") + ".txt";
                        break;
                }
            }
            catch (Exception ex)
            {
                SaveProcessLog(ex.Source + "  ----  " + ex.Message);
                return "";
            }
            return pathR;
        }

        public void SaveProcessLog(string msg = "", string name = "")
        {
            try
            {
                var filename = GetPaths(3, name);
                using (var fs = File.Create(filename))
                {
                    var info = new UTF8Encoding(true).GetBytes(msg);
                    fs.Write(info, 0, info.Length);
                }
            }
            catch (Exception ex)
            {
                new Mailing().Sendmail(ex);
            }

        }
    }
}