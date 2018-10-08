using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NFine.Domain._03_Entity.APPManage
{
    public class marketTvsizeinfoEntity : IEntity<marketMachineModelEntity>
    {
        public string T_TVSIZEID { get; set; }

        public string T_TVSIZENAME { get; set; }
        public double? TVSIZE { get; set; }
        public DateTime? Modify_Date { get; set; }
        public int? isActive { get; set; }
    }
}
