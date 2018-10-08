using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NFine.Domain._03_Entity.APPManage
{
    public class marketSalesActiveListShopEntity : IEntity<marketSalesActiveListShopEntity>
    {
        public string id { get; set; }
        public string SHOP_CODE { get; set; }
        public string SHOP_NAME { get; set; }
        public string ACT_ID { get; set; }
       




    }
}
