using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NFine.Domain._03_Entity.APPManage
{
    public class marketSampleActEntity : IEntity<marketSampleActEntity>
    {
        public string id { get; set; }
        public string SAMPLE_UP_NO { get; set; }
        public string SHOP_CODE { get; set; }
        public string SHOP_NAME { get; set; }
        public string PRODUCT_TYPE_CODE { get; set; }
        
        public string PRODUCT_TYPE_NAME { get; set; }
        public string SN_NO { get; set; }
        public int? isSync { get; set; }
        public DateTime? Created_Time { get; set; }
        public DateTime? Modify_Time { get; set; }
        public DateTime? SAMPLE_DATE { get; set; }
        public int? isDeleted { get; set; }

        public string file_ids { get; set; }

        public string  sales_No { get; set; }
        public string MACHINE_MODEL_NO { get; set; }

        public string UP_TYPE_CODE { get; set; }
        public string UP_TYPE_NAME { get; set; }
        public string SAMPLE_TYPE_CODE { get; set; }
        public string SAMPLE_TYPE_NAME { get; set; }
        public string SOURCE_SAMPLE_UP_NO { get; set; }
        public string PRODUCT_STATUS_CODE { get; set; }

        public string  REMARK { get; set; }
    }
}
