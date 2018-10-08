using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NFine.Domain._03_Entity.APPManage
{
    public class marketProductTypeEntity : IEntity<marketProductTypeEntity>
    {
        public string CODE { get; set; }
        public string NAME { get; set; }



    }
}
