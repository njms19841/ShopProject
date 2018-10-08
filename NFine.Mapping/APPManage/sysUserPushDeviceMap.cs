using NFine.Domain._03_Entity.APPManage;
using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NFine.Mapping.APPManage
{
    public class sysUserPushDeviceMap : EntityTypeConfiguration<sysUserPushDeviceEntity>
    {
        public sysUserPushDeviceMap()
        {
            this.ToTable("sys_user_pushdevice");
            this.HasKey(t => t.id);
        }
    }
}
