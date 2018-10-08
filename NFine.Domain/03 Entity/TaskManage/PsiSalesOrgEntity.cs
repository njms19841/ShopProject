using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NFine.Domain._03_Entity.TaskManage
{
    public class PsiSalesOrgEntity : IEntity<PsiSalesOrgEntity>
    {
        /// <summary>
        /// id
        /// </summary>
        public string id { get; set; }
        /// <summary>
        /// 用户ID
        /// </summary>
      
        public DateTime? CREATE_DATE { get; set; }
        public DateTime? MODIFY_DATE { get; set; }


        public int? ACTIVE_FLAG { get; set; }
        public string MANAGE_ORG_CODE { get; set; }
        public string MANAGE_ORG_NAME { get; set; }

        public string PARENT_ID { get; set; }
        public int? SORTBY { get; set; }


        public int? TREE_LEVEL { get; set; }
        public string ORG_TREE_CODE { get; set; }
        public string ORG_TREE_NAME { get; set; }

        public string MANAGE_ORG_TYPE_CODE { get; set; }
        public string MANAGE_ORG_TYPE_NAME { get; set; }


    }
}
