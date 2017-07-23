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
    // NOTA: puede usar el comando "Rename" del menú "Refactorizar" para cambiar el nombre de clase "Reports" en el código, en svc y en el archivo de configuración a la vez.
    // NOTA: para iniciar el Cliente de prueba WCF para probar este servicio, seleccione Reports.svc o Reports.svc.cs en el Explorador de soluciones e inicie la depuración.
    public class Reports : IReports
    {
        private readonly GeneralDb _db = new GeneralDb();

        public async Task<string> GetDate(string name, bool validate, int id, DateTime date)
        {
            return string.Concat("welcome ", name, " usted es valido: ", validate.ToString(), " su id es: ", id, " con fecha: ", date);
        }
    }
}
