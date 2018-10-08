using NFine.Data;
using NFine.Domain._02_ViewModel;
using NFine.Domain._03_Entity.APPManage;
using NFine.Domain._03_Entity.TaskManage;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NFine.Domain._04_IRepository.TaskManage
{
   public interface IPsiSalesEmpOrgRepository : IRepositoryBase<PsiSalesEmpOrgEntity>
   {
        UserInfoResultModel GetUserInfoByOrgId(string OrgId);
   }
}
