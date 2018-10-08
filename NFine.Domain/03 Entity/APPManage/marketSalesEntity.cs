using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NFine.Domain._03_Entity.APPManage
{
    public class marketSalesEntity : IEntity<marketSalesEntity>
    {
        public DateTime? Modify_Date { get; set; }
        public string sales_No { get; set; }
        public string id { get; set; }
        public string sales_Name { get; set; }
        public string sales_PhoneNumber { get; set; }
        public string sales_IdNo { get; set; }
        //public DateTime? sales_Birthday { get; set; }
        public string sales_six { get; set; }
        //public DateTime? sales_joinDate { get; set; }
        //public DateTime? sales_ContractStart { get; set; }
        //public DateTime? sales_ContractEnd { get; set; }
        public string sales_ShopNo { get; set; }
        public int? Active { get; set; }

        public string app_password { get; set; }
        public string POP_TYPE_CODE { get; set; }




    }
}
