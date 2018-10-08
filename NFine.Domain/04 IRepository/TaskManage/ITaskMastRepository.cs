using NFine.Data;
using NFine.Domain._03_Entity.APPManage;
using NFine.Domain._03_Entity.TaskManage;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NFine.Domain._04_IRepository.TaskManage
{
   public interface ITaskMastRepository : IRepositoryBase<TaskMastEntity>
   {
        void readTask(string taskId, string userId);
        void ReplyTask(string taskId, string userId, string title, string context, string fileId, string fileExt);
   }
}
