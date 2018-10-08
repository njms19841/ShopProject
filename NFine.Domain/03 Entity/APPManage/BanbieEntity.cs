using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NFine.Domain._03_Entity.APPManage
{
    public class BanbieEntity : IEntity<BanbieEntity>
    {
        public string id { get; set; }
        public string type { get; set; }
        public string startH { get; set; }
        public string startM { get; set; }
        public string endH { get; set; }
        public string endM { get; set; }
        public string shopCode { get; set; }
        

    }
}
