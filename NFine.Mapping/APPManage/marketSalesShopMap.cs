using NFine.Domain._03_Entity.APPManage;
using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NFine.Mapping.APPManage
{
    public class marketSalesShopMap : EntityTypeConfiguration<marketSalesShopEntity>
    {
        public marketSalesShopMap()
        {
            this.ToTable("market_sales_shop");
            this.HasKey(t => t.ID);
        }
    }
}
