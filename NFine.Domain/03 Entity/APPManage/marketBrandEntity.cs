using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NFine.Domain._03_Entity.APPManage
{
    public class marketBrandEntity : IEntity<marketBrandEntity>
    {
        public string id { get; set; }
        public string BRAND_CODE { get; set; }
        public string BRAND_NAME { get; set; }
    }
}
