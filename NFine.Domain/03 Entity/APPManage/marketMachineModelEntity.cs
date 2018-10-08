using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NFine.Domain._03_Entity.APPManage
{
   public  class marketMachineModelEntity : IEntity<marketMachineModelEntity>
    {
        public string id { get; set; }
        public string COMPANY_CODE { get; set; }
        public string MACHINE_MODEL_NO { get; set; }
        public string DESCP { get; set; }
        public string T_TVSIZEID { get; set; }
        public double? SALE_PRICE { get; set; }
        public double? TVSIZE { get; set; }
        public DateTime? Modify_Date { get; set; }
        public int? isActive { get; set; }
        public string BANDCODE { get; set; }
        public string BANDNAME { get; set; }

    }
}
