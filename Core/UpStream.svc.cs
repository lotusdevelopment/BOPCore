using System;
using System.Linq;
using System.Threading.Tasks;
using ViewModels.Response;
using ViewModels.General;
using System.Text.RegularExpressions;
using Core.DataBase;

namespace Core
{
    public class UpStream : IUpStream
    {
        private readonly BopDb _db = new BopDb();

        public async Task<SaleMade> UserActivation(string phoneNumber, string phoneOp, string plan, string sellerCode)
        {
            return await Task<SaleMade>.Factory.StartNew(() =>
            {
                var rtn = new SaleMade();
                try
                {
                    var rgx = new Regex("[^0-9 +]");
                    var celu = rgx.Replace(phoneNumber, "");
                    if (celu.Length < 6)
                    {
                        rtn.Error = new GenericError
                        {
                            IsError = true,
                            Description = "The phone number is invalid",
                            Message = "Invalid phone number",
                            Source = "PHVAL"
                        };
                        return rtn;
                    }

                    var query = "exec sp_UserActivation '" + phoneNumber + "', '" + phoneOp + "', '" + plan + "', '" + sellerCode + "', '" + System.Web.Security.Membership.GeneratePassword(10, 0) + "'";
                    var sr = _db.Database.SqlQuery<UserActivationResponse>(query).FirstOrDefault();

                    rtn = new SaleMade
                    {
                        Error = new GenericError
                        {
                            IsError = false
                        },
                        Password = sr.UserPassword,
                        SaleDate = Convert.ToDateTime(sr.SaleDate).ToString("yyyy/MM/dd"),
                        ValidUntil = Convert.ToDateTime(sr.ValidUntil).ToString("yyyy/MM/dd"),
                        User = sr.UserCellPhone
                    };
                    return rtn;
                }
                catch (Exception ex)
                {
                    rtn.Error = new GenericError
                    {
                        IsError = true,
                        Description = "Error until the sale process",
                        Message = "Sales Issue",
                        Source = "GenMstk"
                    };
                    return rtn;
                }
            });
        }

        public async Task<SaleUpdate> ChargeNotification(string phoneNumber, string phoneOp, string plan, string sellerCode)
        {
            return await Task<SaleUpdate>.Factory.StartNew(() =>
            {
                var rtn = new SaleUpdate
                {
                    UpdateStatus = false
                };
                try
                {
                    var rgx = new Regex("[^0-9 +]");
                    var celu = rgx.Replace(phoneNumber, "");
                    if (celu.Length < 6)
                    {
                        rtn.Error = new GenericError
                        {
                            IsError = true,
                            Description = "The phone number is invalid",
                            Message = "Invalid phone number",
                            Source = "PHVAL"
                        };
                        return rtn;
                    }

                    var query = "exec sp_ChargeNotification '" + phoneNumber + "', '" + phoneOp + "', '" + plan + "', '" + sellerCode + "'";
                    var sr = _db.Database.SqlQuery<GenericUpStreamResponse>(query).FirstOrDefault();

                    rtn = new SaleUpdate
                    {
                        Error = new GenericError
                        {
                            IsError = false
                        },
                        UpdateStatus = true,
                        SaleDate = Convert.ToDateTime(sr.SaleDate).ToString("yyyy/MM/dd"),
                        ValidUntil = Convert.ToDateTime(sr.ValidUntil).ToString("yyyy/MM/dd")
                    };
                    return rtn;
                }
                catch (Exception ex)
                {
                    rtn.Error = new GenericError
                    {
                        IsError = true,
                        Description = "Error until the sale process",
                        Message = "Sales Issue",
                        Source = "GenMstk"
                    };
                    return rtn;
                }
            });
        }

        public async Task<SaleUpdate> Suspension(string phoneNumber, string sellerCode)
        {
            return await Task<SaleUpdate>.Factory.StartNew(() =>
            {
                var rtn = new SaleUpdate
                {
                    UpdateStatus = false
                };
                try
                {
                    var rgx = new Regex("[^0-9 +]");
                    var celu = rgx.Replace(phoneNumber, "");
                    if (celu.Length < 6)
                    {
                        rtn.Error = new GenericError
                        {
                            IsError = true,
                            Description = "The phone number is invalid",
                            Message = "Invalid phone number",
                            Source = "PHVAL"
                        };
                        return rtn;
                    }

                    var query = "exec sp_Suspension '" + phoneNumber + "', '" + "1" + "'";
                    var sr = _db.Database.SqlQuery<string>(query).FirstOrDefault();

                    rtn = new SaleUpdate
                    {
                        Error = new GenericError
                        {
                            IsError = false
                        },
                        UpdateStatus = Convert.ToBoolean(sr)
                    };
                    return rtn;
                }
                catch (Exception ex)
                {
                    rtn.Error = new GenericError
                    {
                        IsError = true,
                        Description = "Error until the sale process",
                        Message = "Sales Issue",
                        Source = "GenMstk"
                    };
                    return rtn;
                }
            });
        }

        public async Task<SaleUpdate> Deactivation(string phoneNumber, string sellerCode, string plan)
        {
            return await Task<SaleUpdate>.Factory.StartNew(() =>
            {
                var rtn = new SaleUpdate
                {
                    UpdateStatus = false
                };
                try
                {
                    var rgx = new Regex("[^0-9 +]");
                    var celu = rgx.Replace(phoneNumber, "");
                    if (celu.Length < 6)
                    {
                        rtn.Error = new GenericError
                        {
                            IsError = true,
                            Description = "The phone number is invalid",
                            Message = "Invalid phone number",
                            Source = "PHVAL"
                        };
                        return rtn;
                    }

                    var query = "exec sp_SuspensionDesactivation '" + phoneNumber + "',  '" + plan + "', '" + "2" + "'";
                    var sr = _db.Database.SqlQuery<string>(query).FirstOrDefault();

                    rtn = new SaleUpdate
                    {
                        Error = new GenericError
                        {
                            IsError = false
                        },
                        UpdateStatus = Convert.ToBoolean(sr)
                    };
                    return rtn;
                }
                catch (Exception ex)
                {
                    rtn.Error = new GenericError
                    {
                        IsError = true,
                        Description = "Error until the sale process",
                        Message = "Sales Issue",
                        Source = "GenMstk"
                    };
                    return rtn;
                }
            });
        }
    }
}
