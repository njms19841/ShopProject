
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
    public class TaskReportApp
    {
        ITaskReportRepository rep = new TaskReportRepository();
        ITaskReportUserRepository UserRep = new TaskReportUserRepository();

        public void CreateReport(TaskReportEntity ent)
        {
            rep.CreateReport(ent);
        }
        public void UpdateReport(TaskReportEntity ent)
        {
            rep.UpdateReport(ent);
        }
        public void CreateReportUser(TaskReportUserEntity ent)
        {
            UserRep.Insert(ent);
        }
        public List<TaskReportEntity> getReports(string Userid)
        {
            return rep.FindList("SELECT * FROM task_report where userId='" + Userid + "' or id in ( select reportId from task_reportusers where userId='" + Userid + "') order by reportTime desc");
        }
        public List<TaskReportUserEntity> getReportUsers(string reportId)
        {
            return UserRep.IQueryable().Where(p => p.reportId.Equals(reportId)).ToList();
        }
        public void DeleteReportUser(string reportId)
        {
            var userEnts = UserRep.IQueryable().Where(p => p.reportId.Equals(reportId)).ToList();
            foreach (var ent in userEnts)
            {
                UserRep.Delete(ent);
            }
        }
        public void DeleteReport(string id)
        {
            rep.DeleteReport(id);
        }
        public void AllowReport(string id)
        {
            rep.AllowReport(id);
        }
    }
}
