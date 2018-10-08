using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NFine.Domain._03_Entity.APPManage
{
    public class sysUserPushMessageEntity : IEntity<sysUserPushMessageEntity>
    {
        public string id { get; set; }
        public string sourceId { get; set; }
        public string Message_Url { get; set; }
        public string Message_Title { get; set; }
        public int? isSend { get; set; }
        public int? isRead { get; set; }
        public int? isDeleted { get; set; }
        public string MessageType { get; set; }
        public string userId { get; set; }
        public DateTime? CREATE_DATE { get; set; }


    }
}
