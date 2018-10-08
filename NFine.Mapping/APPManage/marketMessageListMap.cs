using NFine.Domain._03_Entity.APPManage;
using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NFine.Mapping.APPManage
{
    public class marketMessageListMap : EntityTypeConfiguration<marketMessageListEntity>
    {
        public marketMessageListMap()
        {
            this.ToTable("market_message_list");
            this.HasKey(t => t.id);
        }
    }
}
