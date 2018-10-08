using NFine.Domain._03_Entity.APPManage;
using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NFine.Mapping.APPManage
{
    public class marketChannelMachineMap : EntityTypeConfiguration<marketChannelMachineEntity>
    {
        public marketChannelMachineMap()
        {
            this.ToTable("market_channel_machine");
            this.HasKey(t => t.ID);
        }
    }
}
