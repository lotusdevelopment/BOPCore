using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Configuration;
using ViewModels.General;

namespace Core.InnerLogic
{
    public class LocationsServices
    {
        private readonly Connections _cnn = new Connections();

        public string GetLatLanFromService(string state, string city, string neigh, string country, string language)
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

        public string GetPaths(int type)
        {
            var pathR = "";
            var path = AppDomain.CurrentDomain.BaseDirectory;
            var dir = Path.GetDirectoryName(path);
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
            }
            return pathR;
        }
    }
}