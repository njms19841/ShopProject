using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NFine.Domain._03_Entity.TaskManage
{
    public class TaskReportUserEntity : IEntity<TaskReportUserEntity>
    {
        /// <summary>
        /// id
        /// </summary>
        public string id { get; set; }

        /// <summary>
        /// userId
        /// </summary>
        public string userId { get; set; }
        public string reportId { get; set; }
        
        /// <summary>
        /// 1:审阅人 2:抄送人
        /// </summary>
        public int? userType { get; set; }
       

    }
}
