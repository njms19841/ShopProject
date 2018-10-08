using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NFine.Domain._03_Entity.APPManage
{
    public class sysUserPushDeviceEntity : IEntity<sysUserPushDeviceEntity>
    {
        public string id { get; set; }
        public string userId { get; set; }
        public string pushToken { get; set; }
        public string deviceType { get; set; }

    }
}
