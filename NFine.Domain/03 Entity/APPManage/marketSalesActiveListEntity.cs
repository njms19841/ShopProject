using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NFine.Domain._03_Entity.APPManage
{
    public class marketSalesActiveListEntity : IEntity<marketSalesActiveListEntity>
    {
        public string id { get; set; }
        public string ACT_NO { get; set; }
        public string ACT_NAME { get; set; }
        public string ACT_TYPE_CODE { get; set; }
        public string ACT_TYPE_NAME { get; set; }
        public string T_TYPEID { get; set; }
        public string T_TYPENAME { get; set; }
        public string CUSTOMER_CODE { get; set; }
        public string CUSTOMER_NAME { get; set; }
        public DateTime? ACT_START_DATE { get; set; }
        public DateTime? ACT_END_DATE { get; set; }
        public DateTime? Created_Time { get; set; }
        public DateTime? Modify_Time { get; set; }
        public int? isSync { get; set; }




    }
}
