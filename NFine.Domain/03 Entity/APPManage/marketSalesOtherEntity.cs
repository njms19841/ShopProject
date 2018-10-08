using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NFine.Domain._03_Entity.APPManage
{
    public class marketSalesOtherEntity : IEntity<marketSalesOtherEntity>
    {
        public string id { get; set; }
        public string userid { get; set; }
        public string shopCode { get; set; }
        
        public string BRAND_CODE { get; set; }
        public string KEY_MODEL_CODE { get; set; }
        public double? SALES_PRICE { get; set; }
        public string T_TVIZEID { get; set; }
        public int? IS_CURVED_FLAG { get; set; }
        public int? IS_OLED_FLAG { get; set; }
        public int? IS_QUANTUM_DOT_FLAG { get; set; }
        public string DESCP { get; set; }
        public DateTime? Created_Time { get; set; }
        public DateTime? Modify_Time { get; set; }
        public int? isSync { get; set; }
        public int? IS_SMART_TV_FLAG { get; set; }
        public string prdFileIds { get; set; }
        public string ACT_NO { get; set; }
        /// <summary>
        /// 活动名称
        /// </summary>
        public string ACT_NAME { get; set; }

    }
}
