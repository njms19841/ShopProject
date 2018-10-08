using NFine.Domain._03_Entity.APPManage;
using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NFine.Mapping.APPManage
{
    public class user_leaveMap : EntityTypeConfiguration<user_leaveEntity>
    {
        public user_leaveMap()
        {
            this.ToTable("user_leave");
            this.HasKey(t => t.id);
        }
    }
}
