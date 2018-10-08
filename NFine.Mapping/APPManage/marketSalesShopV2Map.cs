using NFine.Domain._03_Entity.APPManage;
using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NFine.Mapping.APPManage
{
    public class marketSalesShopV2Map : EntityTypeConfiguration<marketSalesShopV2Entity>
    {
        public marketSalesShopV2Map()
        {
            this.ToTable("market_sales_shopv2");
            this.HasKey(t => t.id);
        }
    }
}
