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
using NFine.Domain._03_Entity.TaskManage;

namespace NFine.Repository.APPManage
{
    public class marketSalesRepository : RepositoryBase<marketSalesEntity>, ImarketSalesRepository
    {
        public UserInfoResultModel GetUserInfo(String id)
        {

            var ent = dbcontext.Set<aspnetusersEntity>().Find(id);
            if (ent != null)
            {
                UserInfoResultModel model = new UserInfoResultModel() {
                    id = ent.Id,
                    Name = ent.EMPLOYEE_NAME,
                    No = ent.EMPLOYEE_CODE,
                    PhoneNumber = ent.PhoneNumber,
                    POP_TYPE_CODE = "",
                     SalesNo=ent.EMPLOYEE_CODE
                };
                var ent2 = this.IQueryable().Where(p => p.sales_No.Equals(ent.EMPLOYEE_CODE) && p.Active == 1);
                if (ent2.Count() > 0)
                {
                    model.POP_TYPE_CODE = ent2.First().POP_TYPE_CODE;
                    model.POP_TYPE_NAME = ent2.First().POP_TYPE_CODE.Equals("SalesMan") ? "业务员" : "导购员";
                    return model;
                }
                var ent3 = dbcontext.Set<PsiSalesEmpOrgEntity>().Where(p => p.EMPLOYEE_CODE.Equals(ent.EMPLOYEE_CODE) && p.ACTIVE_FLAG == 1);
                if (ent3.Count() > 0)
                {
                    model.POP_TYPE_CODE = ent3.First().JOB_CODE;
                    model.POP_TYPE_NAME = ent3.First().JOB_NAME;
                    return model;
                }
                return model;

            }
            return null;
            /*
            var t = from p in dbcontext.Set<marketSalesEntity>()  join 
                    c in dbcontext.Set<aspnetusersEntity>() on new { PhoneNumber = p.sales_PhoneNumber } equals new { PhoneNumber = c.PhoneNumber }
                    where c.Id.Equals(id) && p.Active==1 
                    select new UserInfoResultModel
                    {
                        Name = p.sales_Name, No=p.sales_No, PhoneNumber=p.sales_PhoneNumber, SalesNo=p.sales_No, POP_TYPE_CODE=p.POP_TYPE_CODE, id=c.Id
                        
                    } ;
            if (t != null && t.Count() > 0)
            {
                return t.First();
            }
            else
            {
                var x = from p in dbcontext.Set<PsiSalesEmpOrgEntity>()
                        join c in dbcontext.Set<aspnetusersEntity>() on new { UserName = p.EMPLOYEE_CODE } equals new { UserName = c.UserName }
                        where c.Id.Equals(id) && p.ACTIVE_FLAG == 1
                        select new UserInfoResultModel
                        {
                            Name = p.EMPLOYEE_NAME,
                            No = p.EMPLOYEE_CODE,
                            PhoneNumber = p.EMPLOYEE_CODE,
                            SalesNo = p.EMPLOYEE_CODE,
                            POP_TYPE_CODE = p.JOB_CODE,id=c.Id

                        };
                if (x != null && x.Count() > 0)
                {
                    return x.First();
                }
                else
                {
                    return null;
                }
                
            }
            */
            
            //dbcontext.Set<marketSalesEntity>().Join(dbcontext.Entry<AreaEntity>.Set<AreaEntity>(),p=> p.test= )
        }
       
        public UserInfoResultModel GetUserInfoBySalesNo(string salesNo)
        {
            var ents = dbcontext.Set<aspnetusersEntity>().Where(p=>p.EMPLOYEE_CODE.Equals(salesNo) && p.active==1);
            if (ents.Count()>0)
            {
                var ent = ents.First();
                UserInfoResultModel model = new UserInfoResultModel()
                {
                    id = ent.Id,
                    Name = ent.EMPLOYEE_NAME,
                    No = ent.EMPLOYEE_CODE,
                    PhoneNumber = ent.PhoneNumber,
                    POP_TYPE_CODE = "",
                    SalesNo = ent.EMPLOYEE_CODE
                };
                var ent2 = this.IQueryable().Where(p => p.sales_No.Equals(ent.EMPLOYEE_CODE) && p.Active==1);
                if (ent2.Count() > 0)
                {
                    model.POP_TYPE_CODE = ent2.First().POP_TYPE_CODE;
                    return model;
                }
                var ent3 = dbcontext.Set<PsiSalesEmpOrgEntity>().Where(p => p.EMPLOYEE_CODE.Equals(ent.EMPLOYEE_CODE) && p.ACTIVE_FLAG==1) ;
                if (ent3.Count() > 0)
                {
                    model.POP_TYPE_CODE = ent3.First().JOB_CODE;
                    return model;
                }
                return model;

            }
            return null;
            /*
            var t = from p in dbcontext.Set<marketSalesEntity>()
                    join c in dbcontext.Set<aspnetusersEntity>() on new { PhoneNumber = p.sales_PhoneNumber } equals new { PhoneNumber = c.PhoneNumber }
                    where p.sales_No.Equals(salesNo) && p.Active == 1
                    select new UserInfoResultModel
                    {
                        Name = p.sales_Name,
                        No = p.sales_No,
                        PhoneNumber = p.sales_PhoneNumber,
                        SalesNo = p.sales_No,
                        POP_TYPE_CODE = p.POP_TYPE_CODE
                        , id=c.Id

                    };
            if (t != null && t.Count() > 0)
            {
                return t.First();
            }
            else
            {
                var x = from p in dbcontext.Set<PsiSalesEmpOrgEntity>()
                        join c in dbcontext.Set<aspnetusersEntity>() on new  { UserName = p.EMPLOYEE_CODE } equals new { UserName = c.UserName }
                        where p.EMPLOYEE_CODE.Equals(salesNo) && p.ACTIVE_FLAG == 1
                        select new UserInfoResultModel
                        {
                            Name = p.EMPLOYEE_NAME,
                            No = p.EMPLOYEE_CODE,
                            PhoneNumber = p.EMPLOYEE_CODE,
                            SalesNo = p.EMPLOYEE_CODE,
                            POP_TYPE_CODE = p.JOB_CODE,
                            id = c.Id

                        };
                if (x != null && x.Count() > 0)
                {
                    return x.First();
                }
                else
                {
                    return null;
                }
            }
            **/
            //dbcontext.Set<marketSalesEntity>().Join(dbcontext.Entry<AreaEntity>.Set<AreaEntity>(),p=> p.test= )
        }

    }
}
