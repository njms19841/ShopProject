using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NFine.Domain._03_Entity.APPManage
{
    public class marketFilesEntity : IEntity<marketFilesEntity>
    {
        public string id { get; set; }
        public byte[] fileContext { get; set; }
        public string ContextType { get; set; }
    }
}
