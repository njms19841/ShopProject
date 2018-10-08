using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NFine.Domain._03_Entity.APPManage
{
    public class user_leaveEntity : IEntity<user_leaveEntity>
    {
        
        public string id { get; set; }
        public string leave_type { get; set; }
        public string day_type { get; set; }
        public string desc { get; set; }
        public DateTime? day { get; set; }
        public DateTime? StartDateTime { get; set; }
        public DateTime? EndDateTime { get; set; }
        public string userid { get; set; }
        public int? is_Sync { get; set; }
        public int? state { get; set; }
        public string auser { get; set; }
        public string adesc { get; set; }

        
        public DateTime? atime { get; set; }
        public string file_id { get; set; }
        

    }
}
