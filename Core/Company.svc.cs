using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.Entity.Core.Objects;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Threading.Tasks;
using System.Web;
using Core.DataBase;
using LotusViewModels.General;
using ViewModels.ILotusAssistance;
using ViewModels.General;
using Carnets = LotusViewModels.General.Carnets;
using System.Data.SqlClient;
using Core.Resources;
using LotusViewModels.Request;
using LotusViewModels.Response;
using ViewModels.Request;
using System.Net.Http;

namespace Core
{
    public class Company : ICompany
    {
        private readonly GeneralDb _db = new GeneralDb();
        private readonly Mailing _mail = new Mailing();
        private readonly Resources.Languages _lang = new Resources.Languages();

        public async Task<SaleValidation> CheckExistingSale(string phone, string id, string session)
        {
            return await Task<SaleValidation>.Factory.StartNew(() =>
            {
                var rtn = new SaleValidation
                {
                    Error = new Error
                    {
                        IsError = true,
                        Message = "No existe una venta que corresponda con los parámetros ingresados."
                    }
                };
                try
                {
                    var query = @"exec sp_GetSalesMainCustomer '" + id + "','" + phone + "','1'";
                    var sale = _db.Database.SqlQuery<AllSalesMainCustomer>(query).FirstOrDefault();
                    if (sale == null)
                    {
                        query = @"exec sp_GetSalesMainCustomer '" + id + "','" + phone + "','2'";
                        sale = _db.Database.SqlQuery<AllSalesMainCustomer>(query).FirstOrDefault();
                    }
                    if (sale == null)
                        return rtn;
                    rtn.Error = new Error
                    {
                        IsError = false
                    };
                    var saleDetails = new Sale
                    {
                        Address = "",
                        AffiliationDate = Convert.ToDateTime(sale.BeginingDate),
                        Birthday = Convert.ToDateTime(sale.CustomerBirthday),
                        SaleDate = Convert.ToDateTime(sale.SaleDate),
                        Currency = "USD",
                        Email = sale.CustomerEmail,
                        LastName = sale.CustomerLastName,
                        Name = sale.CustomerFirstName,
                        MobilePhone = sale.CustomerCellphone,
                        SellerId = sale.SellerCode,
                        SellerCode = sale.SellerCode,
                        Session = session,
                        SaleId = sale.IdSale
                    };
                    rtn.Sale = saleDetails;
                    return rtn;
                }
                catch (Exception)
                {
                    return rtn;
                }
            });
        }

        public void UpdateAndSendData(SaleVal model)
        {
            try
            {
                var query = "exec sp_UpdateUserInformationBus '" + model.SaleId + "', '" + model.Name +
                    "', '" + model.LastName + "', '" + model.Icard + "', '" + model.MobilePhone +
                    "', '" + model.Address + "', '" + null + "', '" + model.Email + "'";
                var rtn = _db.Database.SqlQuery<object>(query);
            }
            catch (Exception e)
            {
                //
            }
        }

        public void SendPaymentEmail(string name, string id, string phone, string email,
     string address, string birthday, string plan, string country, string value, string buyerEmail,
     string buyerName, string bid)
        {
            string body;
            using (var client = new WebClient())
            {
                body = client.DownloadString("https://portal.smilego.com/content/html/p125.html");
            }
            try
            {
                body = body.Replace("@name", name);
                body = body.Replace("@id", id);
                body = body.Replace("@phone", phone);
                body = body.Replace("@mail", email);
                body = body.Replace("@address", address);
                body = body.Replace("@birthday", birthday);
                body = body.Replace("@plan", plan);
                body = body.Replace("@country", country);
                body = body.Replace("@value", value);
                body = body.Replace("@pmnt", "https://portal.smilego.com/Sales/BuyPlan?bid=" + bid);
                var mail = new MailMessage();
                var client = new SmtpClient
                {
                    Port = Convert.ToInt32(ConfigurationManager.AppSettings["port"]),
                    DeliveryMethod = SmtpDeliveryMethod.Network,
                    UseDefaultCredentials = false,
                    Host = ConfigurationManager.AppSettings["host"],
                    EnableSsl = true,
                    Credentials = new NetworkCredential(ConfigurationManager.AppSettings["email"], ConfigurationManager.AppSettings["password"])
                };
                mail.Subject = "Compra YA tu seguro";
                mail.Body = body;
                mail.IsBodyHtml = true;
                mail.To.Add(new MailAddress(buyerEmail, buyerName));
                mail.From = new MailAddress(ConfigurationManager.AppSettings["email"], ConfigurationManager.AppSettings["name"]);
                client.Send(mail);
            }
            catch (Exception e)
            {
                //
            }
        }

        public async Task<List<Countries>> GetCountriesAlone()
        {
            return await Task<List<Countries>>.Factory.StartNew(() =>
            {
                try
                {
                    var query = "exec sp_GetCountries";
                    return _db.Database.SqlQuery<Countries>(query).ToList();
                }
                catch (Exception e)
                {
                    return new List<Countries>();
                }
            });
        }

        public async Task<List<EntityType>> GetEntityTypes()
        {
            return await Task<List<EntityType>>.Factory.StartNew(() =>
            {
                try
                {
                    var query = "exec sp_GetEntityType";
                    return _db.Database.SqlQuery<EntityType>(query).ToList();
                }
                catch (Exception e)
                {
                    return new List<EntityType>();
                }
            });
        }

        public async Task<List<Roles>> GetRoles()
        {
            return await Task<List<Roles>>.Factory.StartNew(() =>
            {
                var query = "exec sp_GetRoles '" + null + "', '2'";
                return _db.Database.SqlQuery<Roles>(query).ToList();
            });
        }

        public async Task<List<LotusViewModels.General.Languages>> GetLanguages()
        {
            return await Task<List<LotusViewModels.General.Languages>>.Factory.StartNew(() =>
            {
                var query = "exec sp_GetLanguages";
                return _db.Database.SqlQuery<LotusViewModels.General.Languages>(query).ToList();
            });
        }

        public string InsertOrUpdateGroup(string entityId, string descGroup)
        {
            try
            {
                var query = "exec sp_createGroupNew '" + Convert.ToInt32(entityId) + "', '" + descGroup.ToUpper() + " '";
                return _db.Database.SqlQuery<string>(query).SingleOrDefault();
            }
            catch (Exception e)
            {
                return "Fallo: " + e.Message;
            }
        }

        public async Task<string> CreateEntity(CreateEntity createEntity)
        {
            return await Task<string>.Factory.StartNew(() =>
            {
                try
                {
                    var query = "exec sp_NewEntity '" + createEntity.Flag + "', '" + createEntity.Name.ToUpper() + "', '" + createEntity.Nit + "', '" + createEntity.Latitud
                    + "', '" + createEntity.Longitud + "', '" + createEntity.EntityType + "', '" + createEntity.Neig + "', '" + createEntity.Country + "', '" + createEntity.Status + "', '" + createEntity.FatherId
                    + "', '" + createEntity.Description + "', '" + createEntity.Logo + "', '" + createEntity.Background + "', '" + createEntity.MainColor
                    + "', '" + createEntity.SecondaryColor + "', '" + createEntity.Url + "', '" + createEntity.Favicon
                    + "', '" + createEntity.User.Flag + "', '" + createEntity.User.UserIcard
                    + "', '" + createEntity.User.Name + "', '" + createEntity.User.UserLastname
                    + "', '" + createEntity.User.UserIcardtitular + "', '" + createEntity.User.UserEmail + "', '" + createEntity.User.Password
                    + "', '" + createEntity.User.LanguageId + "', '" + createEntity.User.SellerCode
                    + "', '" + createEntity.User.CellPhone + "', '" + createEntity.User.CountryId + "', '" + createEntity.User.RoleId + "'";
                    var rtn = _db.Database.SqlQuery<string>(query).SingleOrDefault();
                    try
                    {
                        var fReturn = rtn.ToUpper().Split(new[] { "AND" }, StringSplitOptions.None);
                        if (!fReturn[1].ToUpper().Equals(" SUCCESSFULLY REGISTERED USER"))
                            return rtn;
                        query = "exec sp_GetUser '" + createEntity.User.UserEmail + "', '" + null + "', '2'";
                        var usr = _db.Database.SqlQuery<User>(query).FirstOrDefault();
                        createEntity.User.UserId = usr.UserId;
                        Task.Factory.StartNew(() => _mail.SendCreationEmail(createEntity.User));
                    }
                    catch (Exception e)
                    {
                        //
                    }
                    return rtn;
                }
                catch (Exception e)
                {
                    return "Fallo: " + e.Message;
                }
            });
        }

        public async Task<List<Groups>> GetGroupsEntity(string entityId)
        {
            return await Task<List<Groups>>.Factory.StartNew(() =>
            {
                var cn = Convert.ToInt32(entityId);
                try
                {
                    var query = "exec sp_GetGroupxEntity";
                    return _db.Database.SqlQuery<Groups>(query).ToList();
                }
                catch (Exception)
                {
                    return new List<Groups>();
                }
            });
        }

        public async Task<List<GetApps>> GetApps()
        {
            return await Task<List<GetApps>>.Factory.StartNew(() =>
            {
                try
                {
                    var query = "exec sp_GetApps '" + null + "', '2'";
                    return _db.Database.SqlQuery<GetApps>(query).ToList();
                }
                catch (Exception)
                {
                    return new List<GetApps>();
                }
            });
        }

        public async Task<List<Treatments>> GetTreatments()
        {
            return await Task<List<Treatments>>.Factory.StartNew(() =>
            {
                try
                {
                    var query = "exec sp_GetTreatments";
                    return _db.Database.SqlQuery<Treatments>(query).ToList();
                }
                catch (Exception)
                {
                    return new List<Treatments>();
                }
            });
        }

        public async Task<List<Products>> GetProducts()
        {
            return await Task<List<Products>>.Factory.StartNew(() =>
            {
                try
                {
                    var query = "exec sp_GetProducts";
                    return _db.Database.SqlQuery<Products>(query).ToList();
                }
                catch (Exception)
                {
                    return new List<Products>();
                }
            });
        }

        public async Task<List<Entity>> GetEntities()
        {
            return await Task<List<Entity>>.Factory.StartNew(() =>
            {
                try
                {
                    var query = "exec sp_GetEntities";
                    return _db.Database.SqlQuery<Entity>(query).ToList();
                }
                catch (Exception)
                {
                    return new List<Entity>();
                }
            });
        }

        public async Task<List<Contract>> GetContracts()
        {
            return await Task<List<Contract>>.Factory.StartNew(() =>
            {
                try
                {
                    var query = "exec sp_GetContracts";
                    return _db.Database.SqlQuery<Contract>(query).ToList();
                }
                catch (Exception)
                {
                    return new List<Contract>();
                }
            });
        }

        public async Task<List<GetApps>> GetValidityTime()
        {
            return await Task<List<GetApps>>.Factory.StartNew(() =>
            {
                try
                {
                    //return (from a in _db.apps
                    //        select new GetApps
                    //        {
                    //            AppId = a.app_id,
                    //            AppName = a.app_name
                    //        }).ToList();
                    return null;
                }
                catch (Exception)
                {
                    return new List<GetApps>();
                }
            });
        }

        public async Task<List<GetApps>> GetPaymentPeriod()
        {
            return await Task<List<GetApps>>.Factory.StartNew(() =>
            {
                try
                {
                    //return (from a in _db.apps
                    //        select new GetApps
                    //        {
                    //            AppId = a.app_id,
                    //            AppName = a.app_name
                    //        }).ToList();
                    return null;
                }
                catch (Exception)
                {
                    return new List<GetApps>();
                }
            });
        }

        public async Task<List<GetApps>> GetInactivationPeriod()
        {
            return await Task<List<GetApps>>.Factory.StartNew(() =>
            {
                try
                {
                    //return (from a in _db.apps
                    //        select new GetApps
                    //        {
                    //            AppId = a.app_id,
                    //            AppName = a.app_name
                    //        }).ToList();
                    return null;
                }
                catch (Exception)
                {
                    return new List<GetApps>();
                }
            });
        }

        public async Task<string> CreateProduct(string NomProduct, string CodesTreat, string ValueCoverage, string Quantity)
        {
            return await Task<string>.Factory.StartNew(() =>
            {
                try
                {
                    var query = "exec sp_createProducts '" + NomProduct + "','" + CodesTreat + "','" + ValueCoverage + "','" + Convert.ToInt32(Quantity) + "'";
                    return _db.Database.SqlQuery<string>(query).SingleOrDefault();
                }
                catch (Exception e)
                {
                    return "Fallo: " + e.Message;
                }
            });
        }

        public async Task<string> CreateMenuRole(MenusRole CreateMenuRole)
        {
            return await Task<string>.Factory.StartNew(() =>
            {
                try
                {
                    var query = "exec sp_createMenus '" + CreateMenuRole.NameMenu + "','" + CreateMenuRole.AppId
                    + "','" + CreateMenuRole.RoleId + "','" + CreateMenuRole.IsParent + "', '" + CreateMenuRole.ParentId
                    + "','" + CreateMenuRole.Action + "','" + CreateMenuRole.Controller + "','" + CreateMenuRole.Icon + "'";
                    return _db.Database.SqlQuery<string>(query).SingleOrDefault();
                }
                catch (Exception e)
                {
                    return "Fallo: " + e.Message;
                }
            });
        }

        public List<ContractsSpecificationsRs> GetAllContracts(string gId)
        {
            try
            {
                var groupId = Convert.ToInt32(gId);
                var query = "exec sp_GetAllContracts '" + groupId + "'";
                var contracts = _db.Database.SqlQuery<GeneralContractsFilll>(query).ToList().GroupBy(x => x.ContractId).ToList();
                var rtn = new List<ContractsSpecificationsRs>();
                foreach (var contract in contracts)
                {
                    rtn.Add(new ContractsSpecificationsRs
                    {
                        Contract = contract.FirstOrDefault().Contract,
                        ContractId = contract.FirstOrDefault().ContractId
                    });
                    var csp = new List<ContractsSpecifications>();
                    foreach (var cr in contract)
                    {
                        if (!csp.Any(x => x.ContractProductId == cr.ContractProductId))
                        {
                            var c = new ContractsSpecifications
                            {
                                ComissionType = cr.ComissionType,
                                ComissionTypeId = cr.ComissionTypeId,
                                ContractProductId = cr.ContractProductId,
                                ContractProductRecurrent = cr.ContractProductRecurrent,
                                ContractProductWp = cr.ContractProductWp,
                                MaxComission = cr.MaxComission,
                                ProductName = cr.ProductName,
                                Comissions = new List<ProductSpecifications>()
                            };
                            csp.Add(c);
                        }
                        var cico = new ProductSpecifications
                        {
                            ComiId = cr.ComiId,
                            Comission = cr.Comission,
                            GroupId = cr.GroupId,
                            Role = cr.Role,
                            RoleId = cr.RoleId
                        };
                        csp.FirstOrDefault(x => x.ContractProductId == cr.ContractProductId).Comissions.Add(cico);
                    }
                    rtn.FirstOrDefault(x => x.ContractId == contract.FirstOrDefault().ContractId).ContractsSpecifications = csp;
                }
                return rtn;
            }
            catch (Exception e)
            {
                return new List<ContractsSpecificationsRs>();
            }
        }

        public async Task<string> UpdateCommission(string listIds, string ListParti, string Quantity)
        {
            return await Task<string>.Factory.StartNew(() =>
            {
                try
                {
                    var query = "exec sp_updateCommission '" + listIds + "','" + ListParti + "','" + Convert.ToInt32(Quantity) + "'";
                    return _db.Database.SqlQuery<string>(query).SingleOrDefault();
                }
                catch (Exception e)
                {
                    return "Fallo: " + e.Message;
                }
            });
        }

        public async Task<string> Translate(string phrase, string langF, string lanfT)
        {
            return await Task<string>.Factory.StartNew(() =>
            {
                return _lang.Translate(phrase, langF, lanfT);
            });
        }

        public async Task<List<ListChildrenResult>> GetUsersPerParent(string uId)
        {
            return await Task<List<ListChildrenResult>>.Factory.StartNew(() =>
            {
                try
                {
                    var query = "exec sp_ListChildren '" + uId + "'";
                    var sr = _db.Database.SqlQuery<ListChildrenResult>(query);
                    return sr.ToList();
                }
                catch (Exception ex)
                {
                    return new List<ListChildrenResult>();
                }
            });
        }

        public async Task<DashboardReportsStatistics> SalesReport(string id, string type)
        {
            return await Task<DashboardReportsStatistics>.Factory.StartNew(() =>
            {
                try
                {
                    var query = "exec sp_SalesReport '" + id + "', " + Convert.ToInt32(type);
                    var sr = _db.Database.SqlQuery<DashboardReportsStatistics>(query);
                    return sr.FirstOrDefault();
                }
                catch (Exception e)
                {
                    return null;
                }
            });
        }

        public async Task<List<GroupMetricsReport>> GroupMetricsReport(string companyId)
        {
            return await Task<List<GroupMetricsReport>>.Factory.StartNew(() =>
            {
                try
                {
                    var query = "exec sp_SalesReportGroup " + companyId;
                    var sr = _db.Database.SqlQuery<GroupMetricsReport>(query);
                    return sr.ToList();
                }
                catch (Exception e)
                {
                    return new List<GroupMetricsReport>();
                }
            });
        }

        public async Task<List<GroupMetricsReport>> ProductsMetricsReport(string companyId)
        {
            return await Task<List<GroupMetricsReport>>.Factory.StartNew(() =>
            {
                try
                {
                    var query = "exec sp_ReportSaleProduct " + companyId;
                    var sr = _db.Database.SqlQuery<GroupMetricsReport>(query);
                    return sr.ToList();
                }
                catch (Exception e)
                {
                    return new List<GroupMetricsReport>();
                }
            });
        }

        public async Task<DashboardReportsStatistics> SalesReportSeller(string id)
        {
            return await Task<DashboardReportsStatistics>.Factory.StartNew(() =>
            {
                try
                {
                    var query = "exec sp_ReportSaleSeller '" + id + "'";
                    var sr = _db.Database.SqlQuery<DashboardReportsStatistics>(query).FirstOrDefault();
                    query = "exec sp_InfoReportSaleSeller '" + id + "'";
                    var lst = _db.Database.SqlQuery<DashboardReportsSellerStatistics>(query).ToList();
                    sr.DaySales = lst;
                    return sr;
                }
                catch (Exception e)
                {
                    return null;
                }
            });
        }

        public async Task<Dictionary<string, string>> GetFullDictionary(string lang)
        {
            return await Task<Dictionary<string, string>>.Factory.StartNew(() =>
            {
                try
                {
                    var query = "select top 500 Lang_Key as word, Lang_Translation as translation from " + lang + "";
                    var sr = _db.Database.SqlQuery<LanguagesDictionary>(query).ToList();
                    var dct = new Dictionary<string, string>();
                    foreach (var item in sr) dct.Add(item.Word, item.Translation);
                    return dct;
                }
                catch (Exception e)
                {
                    return new Dictionary<string, string>();
                }
            });
        }
    }
}