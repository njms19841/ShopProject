using NFine.Domain._03_Entity.APPManage;
using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NFine.Mapping.APPManage
{
    public class marketBrandMap : EntityTypeConfiguration<marketBrandEntity>
    {
        public marketBrandMap()
        {
            this.ToTable("market_brand");
            this.HasKey(t => t.id);
        }
    }
}
