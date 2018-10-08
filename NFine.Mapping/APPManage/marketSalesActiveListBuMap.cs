using NFine.Domain._03_Entity.APPManage;
using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NFine.Mapping.APPManage
{
    public class marketSalesActiveListBuMap : EntityTypeConfiguration<marketSalesActiveListBuEntity>
    {
        public marketSalesActiveListBuMap()
        {
            this.ToTable("market_sales_activelist_bu");
            this.HasKey(t => t.id);
        }
    }
}
