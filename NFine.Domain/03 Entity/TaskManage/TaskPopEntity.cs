using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NFine.Domain._03_Entity.TaskManage
{
    public class TaskPopEntity : IEntity<TaskPopEntity>
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
        /// 状态 1：负责人 2：参与人
        /// </summary>
        public int? userType { get; set; }

        /// <summary>
        /// 状态 1：待接受 2：处理中 3：拒绝
        /// </summary>
        public int? status { get; set; }

        public int? isDelete { get; set; }

    }
}
