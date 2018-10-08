using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NFine.Domain._03_Entity.TaskManage
{
    public class PsiSalesEmpOrgEntity : IEntity<PsiSalesEmpOrgEntity>
    {
        /// <summary>
        /// id
        /// </summary>
        public string id { get; set; }

        public string EMPLOYEE_ID { get; set; }
        
        /// <summary>
        /// 用户ID
        /// </summary>
        public string EMPLOYEE_CODE { get; set; }
        public string EMPLOYEE_NAME { get; set; }
        public string ORG_ID { get; set; }

        public string COMPANY_CODE { get; set; }
        
        public DateTime? CREATE_DATE { get; set; }
        public DateTime? LAST_MODIFY_DATE { get; set; }
        public int? ACTIVE_FLAG { get; set; }
        public string JOB_CODE { get; set; }
        public string JOB_NAME { get; set; }

        public string LOGIN_PASSWORD { get; set; }


    }
}
