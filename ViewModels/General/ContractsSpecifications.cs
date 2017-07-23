using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LotusViewModels.General
{
    public class ContractsSpecificationsRs
    {
        public int ContractId { get; set; }
        public string Contract { get; set; }
        public List<ContractsSpecifications> ContractsSpecifications { get; set; }
    }

    public class ContractsSpecifications
    {
        public int ContractProductId { get; set; }        
        public int ContractProductWp { get; set; }
        public int ContractProductRecurrent { get; set; }
        public int ComissionTypeId { get; set; }
        public string ComissionType { get; set; }
        public string ProductName { get; set; }
        public double MaxComission { get; set; }
        public List<ProductSpecifications> Comissions { get; set; }
    }

    public class ProductSpecifications
    {
        public int ComiId { get; set; }
        public double Comission { get; set; }
        public string Role { get; set; }
        public int RoleId { get; set; }
        public int GroupId { get; set; }
    }

    public class GeneralContractsFilll
    {
        public int ContractId { get; set; }
        public string Contract { get; set; }
        public int ContractProductId { get; set; }
        public double ContractProductValue { get; set; }
        public int ContractProductWp { get; set; }
        public int ContractProductRecurrent { get; set; }
        public int ComissionTypeId { get; set; }
        public string ComissionType { get; set; }
        public double MaxComission { get; set; }
        public int ComiId { get; set; }
        public double Comission { get; set; }
        public string Role { get; set; }
        public int RoleId { get; set; }
        public int GroupId { get; set; }
        public string ProductName { get; set; }
    }
}
