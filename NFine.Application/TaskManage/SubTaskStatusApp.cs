
using NFine.Domain._02_ViewModel;
using NFine.Domain._03_Entity.APPManage;
using NFine.Domain._03_Entity.TaskManage;
using NFine.Domain._04_IRepository.APPManage;
using NFine.Domain._04_IRepository.TaskManage;
using NFine.Repository.APPManage;
using NFine.Repository.TaskManage;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NFine.Application.TaskManage
{
    public class SubTaskStatusApp
    {
        ISubTaskStatusRepository repository = new SubTaskStatusRepository();
        public void changeTaskStatus(string taskId, int status, string day, string userId)
        {
            var ents = repository.IQueryable().Where(p => p.taskId.Equals(taskId) && p.day.Equals(day) && p.UserId.Equals(userId)).ToList();
            if (ents.Count() > 0)
            {
                foreach (var ent in ents)
                {
                    ent.status = status;
                    ent.dayTime = DateTime.ParseExact(ent.day,"yyyyMMdd", CultureInfo.CurrentCulture);
                    repository.Update(ent);
                }
            }
            else
            {
                SubTaskStatusEntity ent = new SubTaskStatusEntity() { id=System.Guid.NewGuid().ToString(), day=day, status=status, taskId=taskId, UserId=userId ,
                 dayTime= DateTime.ParseExact(day, "yyyyMMdd", CultureInfo.CurrentCulture)
                };
                repository.Insert(ent);
            }

        }
        public List<SubTaskStatusEntity> getTaskStatus(string startDay, string EndDay, string userId)
        {
           return repository.FindList("select * from task_SubTaskStatus where UserId='" + userId + "' and dayTime>='" + startDay + "' and dayTime<='" + EndDay + "'");
        }
    }
}
