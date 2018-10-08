using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NFine.Domain._03_Entity.TaskManage
{
    public class VORGEntity : IEntity<VORGEntity>
    {
        /// <summary>
        /// id
        /// </summary>
        public string id { get; set; }
        public string EMPLOYEE_CODE { get; set; }
        public string EMPLOYEE_NAME { get; set; }

        public string ORG_ID { get; set; }
        public string Job_NAME { get; set; }
        


    }
}
