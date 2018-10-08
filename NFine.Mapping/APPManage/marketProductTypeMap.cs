using NFine.Domain._03_Entity.APPManage;
using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NFine.Mapping.APPManage
{
    public class marketProductTypeMap : EntityTypeConfiguration<marketProductTypeEntity>
    {
        public marketProductTypeMap()
        {
            this.ToTable("market_product_type");
            this.HasKey(t => t.CODE);
        }
    }
}
