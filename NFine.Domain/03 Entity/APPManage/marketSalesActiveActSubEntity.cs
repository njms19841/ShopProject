using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NFine.Domain._03_Entity.APPManage
{
    public class marketSalesActiveActSubEntity : IEntity<marketSalesActiveActSubEntity>
    {
        public string id { get; set; }
       
        public string BRAND_CODE { get; set; }
        public string BRAND_NAME { get; set; }
        public string MACHINE_MODEL_NO { get; set; }
        public double? SALES_PRICE { get; set; }
        public string T_TVSIZEID { get; set; }
        public int? TVSIZE { get; set; }
        public int? IS_NEW_PRD_FLAG { get; set; }
        public string file_id_Type001 { get; set; }
        public string m_id { get; set; }
        public int? isDelete { get; set; }
        public int? ACT_REPORT_ITEM_NO { get; set; }




    }
}
