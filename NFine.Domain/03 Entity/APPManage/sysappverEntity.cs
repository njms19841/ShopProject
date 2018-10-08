using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NFine.Domain._03_Entity.APPManage
{
   public class sysappverEntity : IEntity<sysappverEntity>
    {
        public string id { get; set; }
        public string Ver { get; set; }
        public string Type { get; set; }
        public string DowloandUrl { get; set; }
    }
}
