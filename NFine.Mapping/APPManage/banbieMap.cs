﻿using NFine.Domain._03_Entity.APPManage;
using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NFine.Mapping.APPManage
{
    public class banbieMap : EntityTypeConfiguration<BanbieEntity>
    {
        public banbieMap()
        {
            this.ToTable("user_banbie");
            this.HasKey(t => t.id);
        }
    }
}