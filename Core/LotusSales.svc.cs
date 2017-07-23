using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.ServiceModel;
using System.ServiceModel.Channels;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Web.Script.Serialization;
using Core.DataBase;
using Core.Resources;
using ViewModels;
using ViewModels.General;
using ViewModels.Request;
using LotusViewModels.Response;
using System.Runtime.Caching;
using LotusViewModels.General;
using LotusViewModels.Request;
using System.Data.Entity.Core.Objects;

namespace Core
{
    public class LotusSales : ILotusSales
    {
        private readonly GeneralDb _db = new GeneralDb();
        private readonly Encryption _encrypt = new Encryption();
        private readonly Authorizations _auth = new Authorizations();

        /*GetAccesibleKeys*/
        private List<DataBase.Excepciones> _exceptions = new List<DataBase.Excepciones>();
        private const string CacheKey = "LotusCacheOptions";
        private string _language;
        private string _logProcess;
        private string _currentProcess;
        private string _loggedUser;
        private string _url;
        private bool _brazilian;

        private readonly string _stripeKey = Convert.ToBoolean(ConfigurationManager.AppSettings["IsProduction"])
            ? ConfigurationManager.AppSettings["StripeKeyProd"]
            : ConfigurationManager.AppSettings["StripeKeyTest"];

        /*Public Methods*/

        public async Task<SalesOptionsViewModel> GetSaleOptions(AuthorizeModel request)
        {
            _currentProcess += "GetPlans";
            await Task.Factory.StartNew(() => ParseAndStore(request));
            LanguageDefinition(request);
            var rtn = new SalesOptionsViewModel();
            if (!_auth.Authorized(request))
            {
                rtn = new SalesOptionsViewModel
                {
                    Error = ErrorDefinition("1", "AS")
                };
                _loggedUser = "None";
            }
            else
            {
                try
                {
                    var result = await Task.Factory.StartNew(() => GetSaleOptionsSrv()).Result;
                    if (result != null) rtn = result;
                }
                catch (Exception)
                {
                    rtn = new SalesOptionsViewModel
                    {
                        Error = ErrorDefinition("1", "AS")
                    };
                }
                _loggedUser = request.User;
            }
            ParseAndStore(rtn);
            await Task.Factory.StartNew(LogProcess);
            return rtn;
        }

        public async Task<ProductsRsViewModel> GetProducts(GetProductsViewModel request)
        {
            _currentProcess += "GetPlans";
            await Task.Factory.StartNew(() => ParseAndStore(request));
            LanguageDefinition(request.Authorization);
            var rtn = new ProductsRsViewModel();
            if (!_auth.Authorized(request.Authorization))
            {
                rtn = new ProductsRsViewModel
                {
                    Error = ErrorDefinition("1", "AS")
                };
                _loggedUser = "None";
            }
            else
            {
                try
                {
                    var result = await Task.Factory.StartNew(() => GetProductsSrv(request.ValidityTime, request.Currency,
                        request.paymentPeriod, request.InfoContractProduct, request.SellerCode)).Result;
                    if (result != null) rtn = result;
                }
                catch (Exception)
                {
                    rtn = new ProductsRsViewModel
                    {
                        Error = ErrorDefinition("1", "AS")
                    };
                }
                _loggedUser = request.Authorization.User;
            }
            ParseAndStore(rtn);
            await Task.Factory.StartNew(LogProcess);
            return rtn;
        }

        public async Task<SaleResponse> SaleRequest(NewSaleFormat sale)
        {
            var saleInfo = sale.SaleInfo;
            var lotusPayment = sale.LotusPayment;
            var process = sale.Process;
            _currentProcess += process;
            await Task.Factory.StartNew(() => ParseAndStore(saleInfo));
            LanguageDefinition(saleInfo.Credentials);
            if (saleInfo.Beneficiaries == null)
                saleInfo.Beneficiaries = new List<Beneficiary>();
            var val = GeneralVerifications(saleInfo);
            SaleResponse rtn;
            if (val.Error.IsError)
            {
                rtn = new SaleResponse
                {
                    ProcessSucess = false,
                    Error = val.Error
                };
                return FinalStatement(rtn);
            }

            InnerCard clientCard = null;
            string loggeduser;

            #region paymentInfo

            if (saleInfo.PaymentType == 0)
            {
                if (lotusPayment)
                {
                    if (saleInfo.CardData == null)
                    {
                        rtn = new SaleResponse
                        {
                            ProcessSucess = false,
                            Error = ErrorDefinition("18", "L")
                        };
                        return FinalStatement(rtn);
                    }

                    clientCard = GetCardInfo(saleInfo.CardData);

                    if (clientCard.Error.IsError)
                    {
                        rtn = new SaleResponse
                        {
                            ProcessSucess = false,
                            Error = clientCard.Error
                        };
                        return FinalStatement(rtn);
                    }

                    if (string.IsNullOrEmpty(saleInfo.BuyerEmail) || string.IsNullOrEmpty(saleInfo.BuyerName))
                    {
                        rtn = new SaleResponse
                        {
                            ProcessSucess = false,
                            Error = ErrorDefinition("21", "M")
                        };
                        return FinalStatement(rtn);
                    }
                }
                loggeduser = saleInfo.Credentials.IsEncrypted
                    ? _encrypt.Decrypt(saleInfo.Credentials.User)
                    : saleInfo.Credentials.User;
            }
            else
            {
                loggeduser = saleInfo.Credentials.IsEncrypted
                    ? _encrypt.Decrypt(saleInfo.Credentials.User)
                    : saleInfo.Credentials.User;
            }

            #endregion                        

            #region defineValues

            var birthdayDateTime = string.IsNullOrEmpty(saleInfo.Birthday)
                ? DateTime.Now
                : Convert.ToDateTime(saleInfo.Birthday);

            #region ValidateAndDefinePhone

            var rgx = new Regex("[^0-9 +]");
            var celu = rgx.Replace(saleInfo.Cellphone, "");
            celu = celu.Substring(celu.Length - (celu.Length - 1));
            var rgx2 = new Regex("[^0-9]");
            celu = rgx2.Replace(celu, "");

            #endregion

            if (celu.Length < 5)
            {
                rtn = new SaleResponse
                {
                    ProcessSucess = false,
                    Error = ErrorDefinition("3", "P")
                };
                return FinalStatement(rtn);
            }

            if (string.IsNullOrEmpty(saleInfo.Email))
            {
                if (string.IsNullOrEmpty(saleInfo.CustomerId))
                {
                    var rnd = new Random();
                    saleInfo.CustomerId =
                        (rnd.Next(Convert.ToInt32(celu.Substring(celu.Length - 4)), 2147483646)).ToString();
                    saleInfo.Email = saleInfo.CustomerId + "@lotus.com";
                }
                else
                    saleInfo.Email = saleInfo.CustomerId + "@lotus.com";
            }

            if (string.IsNullOrEmpty(saleInfo.CustomerId))
            {
                var rnd = new Random();
                saleInfo.CustomerId =
                    (rnd.Next(Convert.ToInt32(celu.Substring(celu.Length - 4)), 2147483646)).ToString();
            }
            else
            {
                var rgxA = new Regex("[^a-zA-Z0-9]");
                saleInfo.CustomerId = rgxA.Replace(saleInfo.CustomerId, "");
            }

            if (string.IsNullOrEmpty(saleInfo.FirstName))
                saleInfo.FirstName = saleInfo.Email;
            if (string.IsNullOrEmpty(saleInfo.LastName))
                saleInfo.LastName = saleInfo.Email;
            if (string.IsNullOrEmpty(saleInfo.BuyerName))
                saleInfo.BuyerName = saleInfo.FirstName;
            if (string.IsNullOrEmpty(saleInfo.BuyerCellPhone))
                saleInfo.BuyerCellPhone = saleInfo.Cellphone;

            #endregion

            var payment = SetSaleSvc(saleInfo.CustomerId, birthdayDateTime, saleInfo.ZipCode, saleInfo.Street,
                saleInfo.PlateNumber, saleInfo.Complement, saleInfo.Neighborhood, saleInfo.City, saleInfo.State,
                saleInfo.Email, saleInfo.Cellphone, saleInfo.LastName, saleInfo.FirstName, saleInfo.MotherName,
                saleInfo.Cellphone, saleInfo.Gender, saleInfo.SellerCode, Convert.ToInt32(sale.SaleInfo.CountryId),
                saleInfo.PaymentType, Convert.ToInt32(sale.SaleInfo.Plan),
                Convert.ToDouble(saleInfo.SaleValue), saleInfo.BuyerName,
                saleInfo.BuyerCellPhone, saleInfo.BuyerEmail, clientCard, lotusPayment, sale.SaleInfo.CompanySaleId,
                saleInfo.Beneficiaries, saleInfo.SaleDateTime, saleInfo.CountryName);

            if (payment.Contains("Error"))
                rtn = new SaleResponse
                {
                    ProcessSucess = false,
                    Error = ErrorDefinition("30", payment)
                };
            else
                rtn = new SaleResponse
                {
                    ProcessSucess = true,
                    Annotations = payment,
                    Error = new GenericError
                    {
                        IsError = false
                    }
                };
            return FinalStatement(rtn);
        }

        /*Private Methods*/

        private async Task<SalesOptionsViewModel> GetSaleOptionsSrv()
        {
            var cache = MemoryCache.Default;
            if (cache.Contains(CacheKey))
                return (SalesOptionsViewModel)cache.Get(CacheKey);
            else
            {
                var rtn = new SalesOptionsViewModel
                {
                    Error = new GenericError
                    {
                        IsError = false
                    },
                    CurrencyProd = await Task.Factory.StartNew(() => GetCurrencies()).Result,
                    InfoContractProd = await Task.Factory.StartNew(() => GetInfoContractProd()).Result,
                    PaymentPeriod = await Task.Factory.StartNew(() => GetPaymentPeriod()).Result,
                    ValidityTime = await Task.Factory.StartNew(() => GetValidityTime()).Result,
                    Countries = await Task.Factory.StartNew(() => GetCountries()).Result
                };
                if (rtn.CurrencyProd == null || rtn.InfoContractProd == null || rtn.PaymentPeriod == null ||
                    rtn.ValidityTime == null) return null;
                var cacheItemPolicy = new CacheItemPolicy()
                {
                    AbsoluteExpiration = DateTime.Now.AddHours(1.0)
                };
                cache.Add(CacheKey, rtn, cacheItemPolicy);
                return rtn;
            }
        }

        private async Task<ProductsRsViewModel> GetProductsSrv(int validityTime, int currency, int paymentPeriod,
            int infoContractProduct, string sellerCode)
        {
            return await Task<ProductsRsViewModel>.Factory.StartNew(() =>
            {
                try
                {
                    var rtn = new ProductsRsViewModel
                    {
                        Error = new GenericError
                        {
                            IsError = false
                        },
                    };
                    var qr = _db.Database.SqlQuery<RsProductsInner>(@"select
                                                            cxcxp.confcontractprod_id as ProductId,
                                                            pr.product_description as ProductName,
                                                            cu.curren_description as Currency,
                                                            vt.vtime_description as ValidityTime,
                                                            pp.ppay_description as PaymentPeriod,
                                                            ixcxp.info_description as InfoContractProduct,
                                                            cxcxp.confcontractprod_value as ProductPrice
                                                            from users u
                                                            inner
                                                            join usersXgroup uxg on uxg.user_id = u.user_id
                                                            inner
                                                            join groupXentity gxe on gxe.groupenti_id = uxg.groupenti_id
                                                            inner
                                                            join entity e on e.enti_id = gxe.enti_id
                                                            inner
                                                            join entitycontract ec on ec.enti_id = e.enti_id
                                                            inner
                                                            join contract co on co.contract_id = ec.contract_id
                                                            inner
                                                            join contractXproducts cxp on cxp.contract_id = co.contract_id
                                                            inner
                                                            join configurationXcontractXproducts cxcxp on cxcxp.contractprod_id = cxp.contractprod_id
                                                            inner
                                                            join currency cu on cu.curren_id = cxcxp.curren_id
                                                            inner
                                                            join infoXcontractXprod ixcxp on ixcxp.info_id = cxcxp.info_id
                                                            inner
                                                            join periodpayment pp on pp.ppay_id = cxcxp.ppay_id
                                                            inner
                                                            join validitytime vt on vt.vtime_id = cxcxp.vtime_id
                                                            inner
                                                            join products pr on pr.product_id = cxp.product_id
                                                            where u.user_sellerCode = '" + sellerCode + @"' 
                                                            and cxcxp.vtime_id = " + validityTime + @" 
                                                            and cxcxp.curren_id = " + currency + @" 
                                                            and cxcxp.ppay_id = " + paymentPeriod + @" 
                                                            and cxcxp.info_id = " + infoContractProduct).ToList();
                    rtn.Products = qr;
                    return rtn;
                }
                catch (Exception e)
                {
                    return null;
                }
            });
        }

        private async Task<List<CurrencyProdViewModel>> GetCurrencies()
        {
            return await Task<List<CurrencyProdViewModel>>.Factory.StartNew(() =>
            {
                try
                {
                    var query = "exec sp_GetCurrencies";
                    return _db.Database.SqlQuery<CurrencyProdViewModel>(query).ToList();
                }
                catch (Exception)
                {
                    return null;
                }
            });
        }

        private async Task<List<InfoContractProdViewModel>> GetInfoContractProd()
        {
            return await Task<List<InfoContractProdViewModel>>.Factory.StartNew(() =>
            {
                try
                {
                    var query = "exec sp_GetInfoxContractxProd";
                    return _db.Database.SqlQuery<InfoContractProdViewModel>(query).ToList();
                }
                catch (Exception ex)
                {
                    return null;
                }
            });
        }

        private async Task<List<PaymentPeriodViewModel>> GetPaymentPeriod()
        {
            return await Task<List<PaymentPeriodViewModel>>.Factory.StartNew(() =>
            {
                try
                {
                    var query = "exec sp_GetPaymentPeriod";
                    return _db.Database.SqlQuery<PaymentPeriodViewModel>(query).ToList();
                }
                catch (Exception)
                {
                    return null;
                }
            });
        }

        private async Task<List<ValidityTimeViewModel>> GetValidityTime()
        {
            return await Task<List<ValidityTimeViewModel>>.Factory.StartNew(() =>
            {
                try
                {
                    var query = "exec sp_GetValidityTime";
                    return _db.Database.SqlQuery<ValidityTimeViewModel>(query).ToList();
                }
                catch (Exception)
                {
                    return null;
                }
            });
        }

        private async Task<List<CountriesViewModel>> GetCountries()
        {
            return await Task<List<CountriesViewModel>>.Factory.StartNew(() =>
            {
                try
                {
                    var query = "exec sp_GetCountries";
                    return _db.Database.SqlQuery<CountriesViewModel>(query).ToList();
                }
                catch (Exception)
                {
                    return null;
                }
            });
        }

        private GenericError ErrorDefinition(string errorCode, string msg)
        {
            var rtn = new GenericError
            {
                IsError = true
            };

            if (_exceptions.Count < 1)
            {
                _exceptions = GetAllExceptions();
            }

            var qr = _exceptions.FirstOrDefault(x => x.ErrorId.Equals(errorCode) && x.Language.Equals(_language));
            if (qr == null)
            {
                rtn.Description = "General App Error:";
                rtn.Message = "General Error";
                rtn.Source = errorCode;
                return rtn;
            }
            if (string.IsNullOrEmpty(msg))
                rtn.Description = qr.Description;
            else
                rtn.Description = qr.Description + " - Specific Error: " + msg;
            rtn.Message = qr.Message;
            rtn.Source = qr.Source + "-" + qr.IdExcepciones.ToString() + "-" + qr.ErrorId.ToString();
            return rtn;
        }

        private List<DataBase.Excepciones> GetAllExceptions()
        {
            try
            {
                return _db.Excepciones.ToList();
            }
            catch (Exception)
            {
                return new List<DataBase.Excepciones>();
            }
        }

        private void ParseAndStore(object obj)
        {
            var json = new JavaScriptSerializer().Serialize(obj);
            _logProcess += json;
        }

        private void LanguageDefinition(AuthorizeModel user)
        {
            try
            {
                _url = OperationContext.Current.RequestContext.RequestMessage.Headers.To.ToString();
            }
            catch (Exception e)
            {
                _url = "UrlMistaken";
            }
            if (user == null)
                _language = "ES";
            else
            {
                var usu = user.IsEncrypted ? _encrypt.Decrypt(user.User) : user.User;
                var cu = GetCurrencyUser(usu);
                if (!string.IsNullOrEmpty(cu))
                {
                    if (cu.Equals("USD"))
                        _language = "EN";
                    else if (cu.Equals("BRL"))
                        _language = "BR";
                    else
                        _language = "ES";
                }
                else
                    _language = "ES";
            }
        }

        private string GetCurrencyUser(string user)
        {
            try
            {
                var query = "exec sp_GetCurrencies";
                return _db.Database.SqlQuery<CurrencyProdViewModel>(query).FirstOrDefault().Description;
            }
            catch (Exception e)
            {
                return null;
            }
        }

        private void LogProcess()
        {
            try
            {
                _db.LogWs.Add(new LogWs
                {
                    DescripcionProceso = _logProcess,
                    Fecha = DateTime.Now,
                    Ip = GetIp(),
                    Proceso = _currentProcess,
                    Url = _url,
                    Usuario = (!string.IsNullOrEmpty(_loggedUser)) ? _loggedUser : "None"
                });
                _db.SaveChanges();
            }
            catch (Exception e)
            {
                _db.LogWs.Add(new LogWs
                {
                    DescripcionProceso = (!string.IsNullOrEmpty(_logProcess)) ? _logProcess + e.Message : e.Message,
                    Fecha = DateTime.Now,
                    Ip = "0.0.0.0",
                    Proceso = (!string.IsNullOrEmpty(_currentProcess)) ? _currentProcess : "Empty Process",
                    Url = "urlMistakenC",
                    Usuario = (!string.IsNullOrEmpty(_loggedUser)) ? _loggedUser : "None"
                });
                _db.SaveChanges();
            }
        }

        private string GetIp()
        {
            try
            {
                var context = System.ServiceModel.OperationContext.Current;
                var prop = context.IncomingMessageProperties;
                var endpoint = prop[RemoteEndpointMessageProperty.Name] as RemoteEndpointMessageProperty;
                return endpoint.Address;
            }
            catch (Exception)
            {
                return "0.0.0.0";
            }
        }

        private SaleResponse FinalStatement(SaleResponse rtn)
        {
            ParseAndStore(rtn);
            Task.Factory.StartNew(LogProcess);
            return rtn;
        }

        private SaleResponse GeneralVerifications(SaleRequestBeneficiaries request)
        {
            if (!_auth.Authorized(request.Credentials))
            {
                _loggedUser = "None";
                return new SaleResponse
                {
                    ProcessSucess = false,
                    Error = ErrorDefinition("1", "AR")
                };
            }
            _loggedUser = request.Credentials.User;
            var validations = CheckSpecificInfo(request.Email, request.Birthday, request.Cellphone,
                request.Plan, request.SellerCode, request.PaymentType, request.BuyerEmail);
            if (validations.IsError)
                return new SaleResponse
                {
                    ProcessSucess = false,
                    Error = validations
                };
            return new SaleResponse
            {
                Error = validations
            };
        }

        private GenericError CheckSpecificInfo(string email, string birthday, string cellphone, string plan,
            string sellerCode, int paymentType, string buyerEmail)
        {
            if (string.IsNullOrEmpty(cellphone) || string.IsNullOrEmpty(sellerCode) || string.IsNullOrEmpty(plan))
            {
                return ErrorDefinition("2", "W");
            }

            if (cellphone.Length < 5)
            {
                return ErrorDefinition("3", "X");
            }

            if (!string.IsNullOrEmpty(email))
            {
                if (
                    !Regex.IsMatch(email,
                        @"\A(?:[a-z0-9!#$%&'*+/=?^_`{|}~-]+(?:\.[a-z0-9!#$%&'*+/=?^_`{|}~-]+)*@(?:[a-z0-9](?:[a-z0-9-]*[a-z0-9])?\.)+[a-z0-9](?:[a-z0-9-]*[a-z0-9])?)\Z",
                        RegexOptions.IgnoreCase))
                {
                    return ErrorDefinition("9", "AD");
                }
            }

            if (!string.IsNullOrEmpty(buyerEmail))
            {
                if (
                    !Regex.IsMatch(buyerEmail,
                        @"\A(?:[a-z0-9!#$%&'*+/=?^_`{|}~-]+(?:\.[a-z0-9!#$%&'*+/=?^_`{|}~-]+)*@(?:[a-z0-9](?:[a-z0-9-]*[a-z0-9])?\.)+[a-z0-9](?:[a-z0-9-]*[a-z0-9])?)\Z",
                        RegexOptions.IgnoreCase))
                {
                    return ErrorDefinition("9", "AD");
                }
            }

            if (!string.IsNullOrEmpty(birthday))
            {
                DateTime temp;
                if (!DateTime.TryParse(birthday, out temp))
                {
                    return ErrorDefinition("10", "AF");
                }
            }

            return new GenericError
            {
                IsError = false
            };
        }

        private GenericError CheckLifes(int count, int plan)
        {
            /*var pln = (from a in _ilotus.ConfigPlanesPaises
                       join b in _ilotus.ConfiguracionPlanes on a.IdConfigPlan equals b.IdConfigPlan
                       join c in _ilotus.PlanesInfo on b.IdPlanesInfo equals c.IdPlanesInfo
                       where a.IdConfigPlanPais.Equals(plan)
                       select c.CantidadVidas).FirstOrDefault();*/
            var pln = 1;
            if ((count + 1) <= Convert.ToInt32(pln))
                return new GenericError
                {
                    IsError = false
                };
            return ErrorDefinition("12", "AO");
        }

        private bool IsBrazil(int plan)
        {
            /*var firstOrDefault = (from c in _ilotus.ConfigPlanesPaises
                                  join p in _ilotus.Paises on c.IdPais equals p.IdPais
                                  join z in _ilotus.ZonaHoraria on p.IdZonaHoraria equals z.IdZonaHoraria
                                  where c.IdConfigPlanPais.Equals(plan)
                                  select z.Descripcion).FirstOrDefault();*/
            string firstOrDefault = null;
            if (firstOrDefault == null)
            {
                _brazilian = false;
                return false;
            }
            var rtn = firstOrDefault;
            if (rtn.ToUpper().Equals("BRAZIL") || rtn.ToUpper().Equals("BRASIL"))
            {
                _brazilian = true;
                return _brazilian;
            }
            _brazilian = false;
            return _brazilian;
        }

        private GenericError CheckBrazilInfo(SaleRequest request)
        {
            if (!string.IsNullOrEmpty(request.Birthday) && !string.IsNullOrEmpty(request.City) &&
                !string.IsNullOrEmpty(request.Complement) && !string.IsNullOrEmpty(request.CustomerId) &&
                !string.IsNullOrEmpty(request.Email) && !string.IsNullOrEmpty(request.FirstName) &&
                !string.IsNullOrEmpty(request.Gender) && !string.IsNullOrEmpty(request.LastName) &&
                !string.IsNullOrEmpty(request.MotherName) && !string.IsNullOrEmpty(request.Neighborhood) &&
                !string.IsNullOrEmpty(request.PlateNumber) && !string.IsNullOrEmpty(request.State) &&
                !string.IsNullOrEmpty(request.Street) && !string.IsNullOrEmpty(request.ZipCode))
                return new GenericError
                {
                    IsError = false
                };
            return ErrorDefinition("11", "AI");
        }

        private GenericError CheckBrazilInfo(SaleRequestBeneficiaries request)
        {
            if (!string.IsNullOrEmpty(request.Birthday) && !string.IsNullOrEmpty(request.City) &&
                !string.IsNullOrEmpty(request.Complement) && !string.IsNullOrEmpty(request.CustomerId) &&
                !string.IsNullOrEmpty(request.Email) && !string.IsNullOrEmpty(request.FirstName) &&
                !string.IsNullOrEmpty(request.Gender) && !string.IsNullOrEmpty(request.LastName) &&
                !string.IsNullOrEmpty(request.MotherName) && !string.IsNullOrEmpty(request.Neighborhood) &&
                !string.IsNullOrEmpty(request.PlateNumber) && !string.IsNullOrEmpty(request.State) &&
                !string.IsNullOrEmpty(request.Street) && !string.IsNullOrEmpty(request.ZipCode))
                return new GenericError
                {
                    IsError = false
                };
            return ErrorDefinition("11", "AJ");
        }

        private GenericError CheckBeneficiaries(List<Beneficiary> beneficiaries)
        {
            if (beneficiaries.Any(ben => string.IsNullOrEmpty(ben.Id) || string.IsNullOrEmpty(ben.Lastname) || string.IsNullOrEmpty(ben.Name)))
            {
                return ErrorDefinition("13", "AK");
            }
            return new GenericError
            {
                IsError = false
            };
        }

        private GenericError CheckBeneficiariesBrazil(List<Beneficiary> beneficiaries)
        {
            if (beneficiaries.Any(ben => string.IsNullOrEmpty(ben.Birthday) || string.IsNullOrEmpty(ben.Gender) || string.IsNullOrEmpty(ben.Id) || string.IsNullOrEmpty(ben.Lastname) || string.IsNullOrEmpty(ben.Mother) || string.IsNullOrEmpty(ben.Name) || string.IsNullOrEmpty(ben.Parent)))
            {
                return ErrorDefinition("13", "AL");
            }
            return new GenericError
            {
                IsError = false
            };
        }

        private InnerCard GetCardInfo(ClientCard cardInfo)
        {
            if (string.IsNullOrEmpty(cardInfo.CardCvc) || string.IsNullOrEmpty(cardInfo.CardExpirationMonth) ||
                string.IsNullOrEmpty(cardInfo.CardExpirationYear) || string.IsNullOrEmpty(cardInfo.CardName) ||
                string.IsNullOrEmpty(cardInfo.CardNumber))
            {
                return new InnerCard
                {
                    Error = ErrorDefinition("19", "AN")
                };
            }
            try
            {
                var card = new InnerCard
                {
                    Error = new GenericError
                    {
                        IsError = false
                    },
                    CardCvc = _encrypt.Decrypt(cardInfo.CardCvc),
                    CardExpirationMonth = _encrypt.Decrypt(cardInfo.CardExpirationMonth),
                    CardExpirationYear = _encrypt.Decrypt(cardInfo.CardExpirationYear),
                    CardName = _encrypt.Decrypt(cardInfo.CardName),
                    CardNumber = _encrypt.Decrypt(cardInfo.CardNumber)
                };
                if (string.IsNullOrEmpty(card.CardCvc)) return CardDataError();
                if (string.IsNullOrEmpty(card.CardExpirationMonth)) return CardDataError();
                if (string.IsNullOrEmpty(card.CardExpirationYear)) return CardDataError();
                if (string.IsNullOrEmpty(card.CardName)) return CardDataError();
                if (string.IsNullOrEmpty(card.CardNumber)) return CardDataError();
                if (!string.IsNullOrEmpty(card.CardAddressCity))
                    card.CardAddressCity = _encrypt.Decrypt(cardInfo.CardAddressCity);
                if (!string.IsNullOrEmpty(card.CardAddressLine1))
                    card.CardAddressLine1 = _encrypt.Decrypt(cardInfo.CardAddressLine1);
                if (!string.IsNullOrEmpty(card.CardAddressZip))
                    card.CardAddressZip = _encrypt.Decrypt(cardInfo.CardAddressZip);
                return card;
            }
            catch (Exception)
            {
                return new InnerCard
                {
                    Error = ErrorDefinition("20", "AM")
                };
            }
        }

        private InnerCard CardDataError()
        {
            return new InnerCard
            {
                Error = ErrorDefinition("20", "AP")
            };
        }

        private string SetSaleSvc(string id, DateTime birthday, string zipcode,
            string street, string number, string complement, string neighborhood, string city, string state,
            string email, string cellphone, string lastname, string name, string mother, string landline,
            string gender, string sellerCode, int countryId, int paymentType, int plan, double value, string buyerName,
            string buyerCellphone, string buyerEmail, InnerCard clientCard, bool lotusPayment, string externalSaleId,
            List<Beneficiary> bens, string saleDateTime, string countryName)
        {
            var dataCard = new ClientCardModel();
            var planInfo = GetNewPlanInfo(plan);
            if (planInfo == null)
                return "Error";
            if (paymentType == 0)
            {
                if (clientCard == null)
                    clientCard = new InnerCard();
                dataCard.CardName = (!string.IsNullOrEmpty(clientCard.CardName))
                    ? clientCard.CardName
                    : "No Card Entered";
                dataCard.CardNumber = (!string.IsNullOrEmpty(clientCard.CardNumber))
                    ? clientCard.CardNumber
                    : "1234123412341234";
                dataCard.CardExpirationYear = (!string.IsNullOrEmpty(clientCard.CardExpirationYear))
                    ? clientCard.CardExpirationYear
                    : "2099";
                dataCard.CardExpirationMonth = (!string.IsNullOrEmpty(clientCard.CardExpirationMonth))
                    ? clientCard.CardExpirationMonth
                    : "13";
                dataCard.CardCvc = (!string.IsNullOrEmpty(clientCard.CardCvc))
                    ? clientCard.CardCvc
                    : "000";
                dataCard.CardAddressCountry = (!string.IsNullOrEmpty(clientCard.CardAddressCountry))
                    ? clientCard.CardAddressCountry
                    : string.Empty;
                dataCard.CardAddressLine1 = (!string.IsNullOrEmpty(clientCard.CardAddressLine1))
                    ? clientCard.CardAddressLine1
                    : string.Empty;
                dataCard.CardAddressLine2 = (!string.IsNullOrEmpty(clientCard.CardAddressLine2))
                    ? clientCard.CardAddressLine2
                    : string.Empty;
                dataCard.CardAddressCity = (!string.IsNullOrEmpty(clientCard.CardAddressCity))
                    ? clientCard.CardAddressCity
                    : string.Empty;
                dataCard.CardAddressZip = (!string.IsNullOrEmpty(clientCard.CardAddressZip))
                    ? clientCard.CardAddressZip
                    : string.Empty;
                dataCard.Amount = planInfo.Price;
            }
            var numero = new Regex("[^0-9.]");
            // Genera Codigo Unico
            var ticks = DateTime.Now.Ticks;
            var bytes = BitConverter.GetBytes(ticks);
            var idUnique = Convert.ToBase64String(bytes)
                .Replace("+", "")
                .Replace("/", "")
                .TrimEnd('=');
            string ret;
            // PaymentType == 1 --> Efectivo
            // PaymentType == 0 --> Tarjeta
            if (lotusPayment)
                ret = paymentType == 1
                    ? idUnique + "|true|.|."
                    : StripeProcess(dataCard, buyerEmail, buyerName, planInfo);
            else
                ret = idUnique + "|true|.|.";

            if (ret.Split('|').Length == 1 || ret.Contains("Error"))
                return ret;

            var respuesta = ret.Split('|')[1] + "," + ret.Split('|')[0] + ", Refunded: " + ret.Split('|')[2] +
                            "Message: " + ret.Split('|')[3];

            if (!bool.Parse(ret.Split('|')[1])) return null;
            var response = "";
            if (paymentType == 0)
                paymentType = 2;
            else if (paymentType == 1)
                paymentType = 1;
            else if (paymentType == 2)
                paymentType = 3;
            else if (paymentType == 3)
                paymentType = 4;
            var dateFrom = DateTime.Now.AddDays(1);
            var dateTo = dateFrom.AddDays(planInfo.Vdays);
            var realSaleDate = DateTime.Now;
            if (!string.IsNullOrEmpty(saleDateTime))
            {
                DateTime temp;
                var dt = !DateTime.TryParse(saleDateTime, out temp);
                // DateTime.TryParseExact(saleDateTime,
                //"yyyy-MM-dd hh:mm:ss",
                //CultureInfo.InvariantCulture,
                //DateTimeStyles.None,
                //out DateTime dt);
                if (temp != null)
                    realSaleDate = temp;
            }
            realSaleDate = realSaleDate.AddTicks(-(realSaleDate.Ticks % TimeSpan.TicksPerSecond));
            try
            {
                birthday = new DateTime(birthday.Year, birthday.Month, birthday.Day, 0, 0, 0);
                dateFrom = new DateTime(dateFrom.Year, dateFrom.Month, dateFrom.Day, 0, 0, 0);
                dateTo = new DateTime(dateTo.Year, dateTo.Month, dateTo.Day, 0, 0, 0);
                realSaleDate = new DateTime(realSaleDate.Year, realSaleDate.Month, realSaleDate.Day, 0, 0, 0);
                ObjectResult<string> resp = _db.SaveAllTheSales(id, name, lastname, birthday, email, cellphone,
                                gender, countryId, state, city, zipcode, street, number, complement, mother,
                                plan, paymentType, sellerCode, buyerName, buyerCellphone,
                                buyerEmail, externalSaleId, 0, "0", null, 2, dateFrom, dateTo, realSaleDate);
                response = resp.FirstOrDefault();
            }
            catch (Exception ex)
            {
                respuesta += "Error";
                _logProcess += string.Concat("{Bens - ", ex.Message, " - ", ex.Source, " - ", ex.StackTrace,
                    " - ", ex.InnerException, " - ", ex.TargetSite, " - ", respuesta, "}");
                return string.Concat("Bens - ", ex.Message, " - ", ex.Source, " - ", ex.StackTrace,
                    " - ", ex.InnerException, " - ", ex.TargetSite, " - ", respuesta);
            }
            respuesta = response + "&" + dateFrom.ToShortDateString() + "&" + dateTo.ToShortDateString();
            Task.Factory.StartNew(() => UpdateSaleBeneficiaries(id, birthday, zipcode, street, number, complement,
                neighborhood, city, state, email, cellphone, lastname, name, mother, landline, gender, sellerCode,
                countryId, paymentType, plan, value, buyerName, buyerCellphone, buyerEmail, externalSaleId, bens,
                dateFrom, dateTo, countryName, realSaleDate));
            return respuesta;
        }

        private string StripeProcess(ClientCardModel model, string email, string fullName, InnerPaymentProducts planInfo)
        {
            //General Stripe Process
            //1. Get plan Info By Stripe Plan Id
            //2. Get or create customer
            //3. Add card to customer
            //4. Create Subscription
            //5. Process Payment
            /*try
            {
                var planService = new StripePlanService(_stripeKey);
                var customerService = new StripeCustomerService(_stripeKey);
                var chargeService = new StripeChargeService(_stripeKey);
                var subscriptionService = new StripeSubscriptionService(_stripeKey);
                //1. Get plan Info By Stripe Plan Id
                var stripePlan = planService.Get(planInfo.StripeDescription);
                //2. Get or create customer
                var allCustomers = customerService.List().ToList();
                var stripeCustomer = allCustomers.FirstOrDefault(x => x.Email.Equals(email));
                //3. Add card to customer
                if (stripeCustomer == null)
                {
                    var myCustomer = new StripeCustomerCreateOptions
                    {
                        Email = email,
                        Description = string.Concat(fullName, " (", email, ")"),
                        SourceCard = new SourceCard()
                        {
                            Number = model.CardNumber,
                            ExpirationYear = model.CardExpirationYear,
                            ExpirationMonth = model.CardExpirationMonth,
                            AddressCountry = model.CardAddressCountry, // optional
                            AddressLine1 = model.CardAddressLine1, // optional
                            AddressLine2 = model.CardAddressLine2, // optional
                            AddressCity = model.CardAddressCity, // optional
                            AddressZip = model.CardAddressZip, // optional
                            Name = model.CardName,
                            Cvc = model.CardCvc
                        },
                        PlanId = stripePlan.Id,
                        Quantity = 1
                    };
                    //4. Create Subscription && customer
                    stripeCustomer = customerService.Create(myCustomer);
                }
                else
                {
                    var subsresponses =
                        subscriptionService.List(stripeCustomer.Id).Where(x => x.StripePlan.Id.Equals(stripePlan.Id));
                    if (!subsresponses.Any())
                    {
                        //4. Create Subscription
                        subscriptionService.Create(stripeCustomer.Id, stripePlan.Id);
                    }
                    else
                    {
                        return "Error : El usuario ya tiene una subscripción de pago a ese plan";
                    }
                }
                //5. Process Payment
                var value = Convert.ToInt32(planInfo.Valor) * 100;
                var card = new SourceCard
                {
                    Number = model.CardNumber,
                    ExpirationYear = model.CardExpirationYear,
                    ExpirationMonth = model.CardExpirationMonth,
                    Name = model.CardName,
                    Cvc = model.CardCvc
                };
                var myCharge = new StripeChargeCreateOptions
                {
                    Amount = value,
                    Currency = planInfo.Moneda,
                    Description = planInfo.Titulo1,
                    CustomerId = stripeCustomer.Id,
                    Capture = true
                };
                var stripeCharge = chargeService.Create(myCharge);
                return $"{stripeCharge.Id}|{stripeCharge.Paid}|{stripeCharge.Refunded}|{stripeCharge.FailureMessage}";
            }
            catch (StripeException e)
            {
                return "Error procesando el pago: " + e.StripeError.Message.Replace(",", ".");
            }*/
            return "|true|.|.";
        }

        private InnerPaymentProducts GetNewPlanInfo(int planId)
        {
            try
            {
                var rtn = _db.Database.SqlQuery<InnerPaymentProducts>(@"select
                                                                  cxcxp.confcontractprod_id as ProductId,
                                                                  pr.product_description as ProductName,
                                                                  cu.curren_description as Currency,
                                                                  pp.ppay_time as Ptime,
                                                                  pp.ppay_description as Pdescription,
                                                                  vt.vtime_description as Vtime,
                                                                  vt.vtime_days as Vdays,
                                                                  cxcxp.confcontractprod_value as Price
                                                                  from users u
                                                                  inner join usersXgroup uxg on uxg.user_id = u.user_id
                                                                  inner join groupXentity gxe on gxe.groupenti_id = uxg.groupenti_id
                                                                  inner join entity e on e.enti_id = gxe.enti_id
                                                                  inner join entitycontract ec on ec.enti_id = e.enti_id
                                                                  inner join contract co on co.contract_id = ec.contract_id
                                                                  inner join contractXproducts cxp on cxp.contract_id = co.contract_id
                                                                  inner join configurationXcontractXproducts cxcxp on cxcxp.contractprod_id = cxp.contractprod_id
                                                                  inner join currency cu on cu.curren_id = cxcxp.curren_id
                                                                  inner join infoXcontractXprod ixcxp on ixcxp.info_id = cxcxp.info_id
                                                                  inner join periodpayment pp on pp.ppay_id = cxcxp.ppay_id
                                                                  inner join validitytime vt on vt.vtime_id = cxcxp.vtime_id
                                                                  inner join products pr on pr.product_id = cxp.product_id
                                                                  where cxcxp.confcontractprod_id = " + planId).FirstOrDefault();
                return rtn;
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        private void UpdateSaleBeneficiaries(string id, DateTime birthday, string zipcode,
            string street, string number, string complement, string neighborhood, string city, string state,
            string email, string cellphone, string lastname, string name, string mother, string landline,
            string gender, string sellerCode, int countryId, int paymentType, int plan, double value, string buyerName,
            string buyerCellphone, string buyerEmail, string externalSaleId, List<Beneficiary> bens, DateTime df, DateTime dt,
            string countryName, DateTime realSaleDate)
        {
            try
            {
                foreach (var ben in bens)
                {
                    var bday = DateTime.Now.AddYears(-25);
                    if (!string.IsNullOrEmpty(ben.Birthday))
                        try
                        {
                            bday = Convert.ToDateTime(ben.Birthday);
                        }
                        catch (Exception)
                        {
                        }
                    ObjectResult<string> resp = _db.SaveAllTheSales(ben.Id, ben.Name, ben.Lastname, bday, email, cellphone,
                                ben.Gender, countryId, state, city, zipcode, street, number, complement, ben.Mother,
                                plan, paymentType, sellerCode, buyerName, buyerCellphone,
                                buyerEmail, externalSaleId, 1, id, null, 2, df, dt, realSaleDate);
                }
            }
            catch (Exception bEx)
            {
            }
        }
    }
}