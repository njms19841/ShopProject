
using NFine.Domain._02_ViewModel;
using NFine.Domain._03_Entity.APPManage;
using NFine.Domain._03_Entity.TaskManage;
using NFine.Domain._04_IRepository.APPManage;
using NFine.Domain._04_IRepository.TaskManage;
using NFine.Repository.APPManage;
using NFine.Repository.TaskManage;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NFine.Application.TaskManage
{
    public class TaskMastApp
    {
        ITaskMastRepository repository = new TaskMastRepository();
        public void ReplyTask(string taskId, string userId, string title, string context, string fileId, string fileExt)
        {
            repository.ReplyTask(taskId, userId, title, context, fileId, fileExt);
        }
        public void readTask(string taskId, string userId)
        {
            repository.readTask(taskId, userId);
        }
        public List<TaskMastEntity> getTask(string userId, string startTime, string endTime, int queryType)
        {
            if (queryType == 1)
            {
                var ents = repository.FindList("select distinct a.* from task_Mast a join task_Pop b on a.id=b.taskId and b.status <>3  where ((a.starTime>='" + startTime + " 00:00:00' and  a.starTime <='" + endTime + " 23:59:59') or (a.endTime>='" + startTime + " 00:00:00' and  a.endTime <='" + endTime + " 23:59:59') or (a.starTime<='" + startTime + " 23:59:59' and  a.endTime >='" + startTime + " 00:00:00') or (a.starTime<='" + endTime + " 23:59:59' and  a.endTime >='" + endTime + " 00:00:00')) and b.UserId='" + userId + "' and a.taskType<>'task' and (b.isDelete<>1 or b.isDelete is null ) and (a.isDelete<>1 or a.isDelete is null)");
                return ents.ToList();
            }
            else if (queryType == 2)
            {
                var ents = repository.FindList("select distinct a.* from task_Mast a join task_Pop b on a.id=b.taskId and b.status <>3    where b.UserId='" + userId + "' and a.taskType='task' and (b.isDelete<>1 or b.isDelete is null ) and (a.isDelete<>1 or a.isDelete is null)");
                return ents.ToList();
            }
            else
            {
                var ents = repository.FindList("select distinct a.* from task_Mast a join task_Pop b on a.id=b.taskId and b.status <>3  where ((a.starTime>='" + startTime + " 00:00:00' and  a.starTime <='" + endTime + " 23:59:59') or (a.endTime>='" + startTime + " 00:00:00' and  a.endTime <='" + endTime + " 23:59:59') or (a.starTime<='" + startTime + " 23:59:59' and  a.endTime >='" + startTime + " 00:00:00') or (a.starTime<='" + endTime + " 23:59:59' and  a.endTime >='" + endTime + " 00:00:00')) and b.UserId='" + userId + "' and  (b.isDelete<>1 or b.isDelete is null ) and (a.isDelete<>1 or a.isDelete is null)");
                return ents.ToList();
            }
        
        }
        public void createTask(TaskMastEntity ent)
        {
            repository.Insert(ent);
        }
        public void modifyTask(TaskMastEntity ent)
        {
            var oldEnt = repository.FindEntity(ent.id);

            oldEnt.alertType = ent.alertType;
            oldEnt.modifyUserId = ent.createdUserId;
            oldEnt.CustomerCode = ent.CustomerCode;
            oldEnt.CustomerName = ent.CustomerName;
            oldEnt.desc = ent.desc;
            oldEnt.endTime = ent.endTime;
            oldEnt.freqType = ent.freqType;
            oldEnt.importantType = ent.importantType;
            oldEnt.starTime = ent.starTime;
            oldEnt.taskName = ent.taskName;
            oldEnt.taskType = ent.taskType;
            oldEnt.taskTypeName = ent.taskTypeName;
            oldEnt.fileId = ent.fileId;
            oldEnt.modifyTime = System.DateTime.Now;
           
            repository.Update(oldEnt);

        }
        public void deleteTask(string id)
        {
           var ent= repository.FindEntity(id);
            ent.isDelete = 1;
            ent.modifyTime = System.DateTime.Now;
            repository.Update(ent);
        }
    }
}
