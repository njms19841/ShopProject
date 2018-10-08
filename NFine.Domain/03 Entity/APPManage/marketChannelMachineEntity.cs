using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NFine.Domain._03_Entity.APPManage
{
    public class marketChannelMachineEntity : IEntity<marketChannelMachineEntity>
    {
        public string ID { get; set; }
        public int? ACTIVE_FLAG { get; set; }
        public string T_TYPEID { get; set; }
        public string MACHINE_MODEL_NO { get; set; }
        public int? ALLOW_FLAG { get; set; }
        public DateTime? Modify_Date { get; set; }
    }
}
