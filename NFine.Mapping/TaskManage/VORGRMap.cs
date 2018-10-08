using NFine.Domain._03_Entity.APPManage;
using NFine.Domain._03_Entity.TaskManage;
using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NFine.Mapping.TaskManage
{
    public class VORGRMap : EntityTypeConfiguration<VORGEntity>
    {
        public VORGRMap()
        {
            this.ToTable("view_Org");
        }
    }
}
