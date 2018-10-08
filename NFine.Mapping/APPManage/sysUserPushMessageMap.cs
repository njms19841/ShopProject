﻿using NFine.Domain._03_Entity.APPManage;
using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NFine.Mapping.APPManage
{
    public class sysUserPushMessageMap : EntityTypeConfiguration<sysUserPushMessageEntity>
    {
        public sysUserPushMessageMap()
        {
            this.ToTable("sys_user_pushmessage");
            this.HasKey(t => t.id);
        }
    }
}
