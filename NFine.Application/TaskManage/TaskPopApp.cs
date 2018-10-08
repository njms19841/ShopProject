
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
    public class TaskPopApp
    {
        ITaskPopRepository repository = new TaskPopRepository();
        IUserInfoRepository UserInforepository = new UserInfoRepository();
        IPsiSalesEmpOrgRepository psiSalesRep = new PsiSalesEmpOrgRepository();
        IPsiSalesOrgRepository orgRep = new PsiSalesOrgRepository();
        IVORGRepository orgEmpRep = new VORGRepository();
        public List<VORGEntity> getAllOrgUser()
        {
            return orgEmpRep.IQueryable().ToList();
        }
        public List<PsiSalesOrgEntity> getAllOrg()
        {
            return orgRep.IQueryable().ToList();
        }
        public List<UserInfoEntity> getUsers(int page, string queryString)
        {
           return UserInforepository.IQueryable().Where(p => p.CUSTOMER_NAME.Contains(queryString) || p.sales_Name.Contains(queryString) || p.SHOP_NAME.Contains(queryString) || p.MANAGE_CENTER.Contains(queryString)).OrderBy(p=>p.MANAGE_CENTER).ThenBy(p=>p.sales_Name).Take(20 * page).Skip(20 * (page - 1)).ToList();
        }
        public List<TaskPopEntity> getTaskPop(string taskId)
        {
            return repository.IQueryable().Where(p => p.taskId.Equals(taskId) && p.status!=3 && p.isDelete!=1).ToList();
        }
        public int getTaskStatus(string taskId,string userId)
        {
            return repository.IQueryable().Where(p => p.taskId.Equals(taskId) && p.UserId.Equals(userId)).First().status.Value;
        }
        public List<TaskPopEntity> getTaskStatusList(string startTime, string endTime, string userId)
        {
            return repository.FindList("select distinct b.* from task_Mast a join task_Pop b on a.id=b.taskId and b.status <>3  where ((a.starTime>='" + startTime + " 00:00:00' and  a.starTime <='" + endTime + " 23:59:59') or (a.endTime>='" + startTime + " 00:00:00' and  a.endTime <='" + endTime + " 23:59:59') or (a.starTime<='" + startTime + " 23:59:59' and  a.endTime >='" + startTime + " 00:00:00') or (a.starTime<='" + endTime + " 23:59:59' and  a.endTime >='" + endTime + " 00:00:00')) and b.UserId='" + userId + "' and a.taskType<>'task' and (b.isDelete<>1 or b.isDelete is null ) and (a.isDelete<>1 or a.isDelete is null)");
        }
        public void createTaskPop(TaskPopEntity ent)
        {
            repository.Insert(ent);
        }
        public void deletePop(string taskId)
        {
           
                var ents = repository.IQueryable().Where(p => p.isDelete != 1 && p.taskId.Equals(taskId)).ToList();
                foreach (var ent in ents)
                {
                    ent.isDelete = 1;
                    repository.Update(ent);
                 }
            
        }
        public void AllowTask(string taskId, string userId, int userType,int allowType)
        {
            var ents = repository.IQueryable().Where(p => p.isDelete != 1 && p.taskId.Equals(taskId) && p.UserId.Equals(userId)).ToList();
            foreach (var ent in ents)
            {
                ent.status = allowType;
                repository.Update(ent);
            }
        }

    }
}
