using NFine.Domain._04_IRepository.APPManage;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NFine.Code;
using NFine.Domain._03_Entity.APPManage;
using System.Data.Common;
using System.Linq.Expressions;
using NFine.Data;
using NFine.Domain.Entity.SystemManage;
using NFine.Domain._02_ViewModel;
using NFine.Domain._04_IRepository.TaskManage;
using NFine.Domain._03_Entity.TaskManage;

namespace NFine.Repository.TaskManage
{
    public class PsiSalesEmpOrgRepository : RepositoryBase<PsiSalesEmpOrgEntity>, IPsiSalesEmpOrgRepository
    {
        public UserInfoResultModel GetUserInfoByOrgId(string OrgId)
        {
            var x = from p in dbcontext.Set<PsiSalesEmpOrgEntity>()
                    join c in dbcontext.Set<aspnetusersEntity>() on new { UserName = p.EMPLOYEE_CODE } equals new { UserName = c.EMPLOYEE_CODE }
                    where p.ORG_ID.Equals(OrgId) && p.ACTIVE_FLAG == 1
                    select new UserInfoResultModel
                    {
                        Name = p.EMPLOYEE_NAME,
                        No = p.EMPLOYEE_CODE,
                        PhoneNumber = p.EMPLOYEE_CODE,
                        SalesNo = p.EMPLOYEE_CODE,
                        POP_TYPE_CODE = p.JOB_CODE,
                        id = c.Id

                    };
            if(x.Count()>0)
            return x.First();
            return null;
        }
    }
}
