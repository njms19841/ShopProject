using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NFine.Domain._03_Entity.APPManage
{
    public class marketSalesActiveActEntity : IEntity<marketSalesActiveActEntity>
    {
        public string id { get; set; }
        public string ACT_NO { get; set; }
        public string ACT_NAME { get; set; }
        public string SHOP_CODE { get; set; }
        public string SHOP_NAME { get; set; }
        
        public string file_id_Type002 { get; set; }
        public string file_id_Type003 { get; set; }
        public string file_id_Type004 { get; set; }
        public int? isSync { get; set; }
        public DateTime? Created_Time { get; set; }
        public DateTime? Modify_Time { get; set; }
        public int? is_Delete { get; set; }
        public string sales_No { get; set; }
        public string ACT_REPORT_NO { get; set; }

        

    }
}
