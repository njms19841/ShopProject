using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NFine.Domain._03_Entity.APPManage
{
   public class marketMessageListEntity : IEntity<marketMessageListEntity>
    {
        public string id { get; set; }
        public string Message_Url { get; set; }
        public string Message_Title { get; set; }
        public string CUSTOMER_CODE { get; set; }
        public string DEALERE_CODE { get; set; }
        public string MACHINE_MODEL_NO { get; set; }
        public string T_TVSIZEID { get; set; }
        public string CUSTOMER_NAME { get; set; }
        public string DEALERE_NAME { get; set; }
        public int? TVSIZE { get; set; }
        public int? ALL_POST_FLAG { get; set; }
        public string POST_PAGE_MEMO { get; set; }
        public string STATUS_CODE { get; set; }

        public byte[] POST_PAGE_CONTENT { get; set; }
        public string POSTED_FLAG { get; set; }
        public DateTime? POSTED_TIME { get; set; }
        public DateTime? CREATE_DATE { get; set; }
        public int? isSend { get; set; }

    }
}
