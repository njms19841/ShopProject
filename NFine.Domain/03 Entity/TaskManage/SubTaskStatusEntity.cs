using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NFine.Domain._03_Entity.TaskManage
{
    public class SubTaskStatusEntity : IEntity<SubTaskStatusEntity>
    {

        /// <summary>
        /// id
        /// </summary>
        public string id { get; set; }
        /// <summary>
        /// TaskId
        /// </summary>
        public string taskId { get; set; }
        /// <summary>
        /// 用户ID
        /// </summary>
        public string UserId { get; set; }
        /// <summary>
        /// 任务日期 YYYYMMDD
        /// </summary>
        public string day { get; set; }
        /// <summary>
        /// 任务日期
        /// </summary>
        public DateTime? dayTime { get; set; }

        /// <summary>
        /// 状态 1:进行中 2：完成
        /// </summary>
        public int? status { get; set; }


    }
}
