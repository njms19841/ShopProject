using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NFine.Domain._03_Entity.TaskManage
{
    public class TaskReportEntity : IEntity<TaskReportEntity>
    {
        /// <summary>
        /// id
        /// </summary>
        public string id { get; set; }

        /// <summary>
        /// userId
        /// </summary>
        public string userId { get; set; }
        public DateTime? reportTime { get; set; }
        /// <summary>
        /// 状态 1:未审阅 2:已审阅
        /// </summary>
        public int? state { get; set; }

        public string title { get; set; }
        public string context { get; set; }
        public string fileId { get; set; }
        public string fileExt { get; set; }
        public string fileName { get; set; }
        
        /// <summary>
        /// 1:日报 2:周报 3:月报 4:其他
        /// </summary>
        public int? reportType { get; set; }
        /// <summary>
        /// 语音文件ID
        /// </summary>
        public string audoFileId { get; set; }
        

    }
}
