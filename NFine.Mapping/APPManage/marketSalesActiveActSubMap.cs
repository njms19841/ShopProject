﻿using NFine.Domain._03_Entity.APPManage;
using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NFine.Mapping.APPManage
{
    public class marketSalesActiveActMap : EntityTypeConfiguration<marketSalesActiveActEntity>
    {
        public marketSalesActiveActMap()
        {
            this.ToTable("market_sales_active_act");
            this.HasKey(t => t.id);
        }
    }
}