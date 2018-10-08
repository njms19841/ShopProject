using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NFine.Domain._03_Entity.APPManage
{
    public class marketSalesShopV2Entity : IEntity<marketSalesShopV2Entity>
    {
        public string id { get; set; }
        public string EMPLOYEE_CODE { get; set; }
        public string SHOP_ID { get; set; }
        public string SHOP_CODE { get; set; }
        public string SHOP_NAME { get; set; }
        public string T_TYPEID { get; set; }
        public string T_TYPENAME { get; set; }
        public string CUSTOMER_CODE { get; set; }
        public string CUSTOMER_NAME { get; set; }
        public string T_BUID { get; set; }
        public string T_BUNAME { get; set; }

        public int? ACTIVE_FLAG { get; set; }
        public int? ALLOW_FLAG { get; set; }

    }
}
