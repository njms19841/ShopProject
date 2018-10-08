using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NFine.Domain._03_Entity.APPManage
{
    public class marketSalesShopEntity : IEntity<marketSalesShopEntity>
    {
        public string ID { get; set; }
        public string T_BUID { get; set; }
        public string T_BUNAME { get; set; }
        public string AREA { get; set; }
        public string COMMUNITY { get; set; }
        public string MANAGE_CENTER { get; set; }
        public string CITY { get; set; }
        public string CITY_LEVEL { get; set; }
        public string DEALERE_NAME { get; set; }
        public string DEALERE_CODE { get; set; }
        public string DEALERE_CODE_SAMPLE { get; set; }
        public string CHANNEL { get; set; }
        public string DEALERE_TYPE { get; set; }
        public string SHOP_NAME { get; set; }
        public string SHOP_CODE { get; set; }
        public string SHOP_PANEL_LEVEL { get; set; }
        public string SHOP_LEVEL { get; set; }
        public string BIG_SHOP { get; set; }
        public string SHOP_ADDR { get; set; }
        public string SHOP_PRODUCT_TYPE { get; set; }
        public string SHOP_POINT_CUSTOMER { get; set; }
        public string SHOP_ATTR { get; set; }
        public string SHOP_TYPE { get; set; }
        public string SHOP_PRODUCTS { get; set; }
        public string SHOP_OLD_NAME { get; set; }
        public string SHOP_PROMOTER_NAME { get; set; }
        public string SHOP_PROMOTER_CODE { get; set; }
        public string SHOP_PROMOTER_CELL { get; set; }
        public string SHOP_SALESOFYEAR { get; set; }
        public DateTime? SHOP_BUILD_DATE { get; set; }
        public DateTime? SHOP_LEAVE_DATE { get; set; }
        public DateTime? SHOP_REBUILD_DATE { get; set; }
        public string PIC { get; set; }
        public string MEMO { get; set; }
        public string T_TYPEID { get; set; }
        public string T_TYPENAME { get; set; }
        public string CUSTOMER_CODE { get; set; }
        public string STATUS_CODE { get; set; }
        public string CUSTOMER_NAME { get; set; }
        public int? HAVEPEOPLE_FLAG { get; set; }
        public double? FLOOR_AREA { get; set; }
        public int? ACTIVE_FLAG { get; set; }
        public DateTime? Modify_Date { get; set; }
        public double? LONGITUDE { get; set; }
        public double? LATITUDE { get; set; }
        public string ORG_ID { get; set; }
    }
}
