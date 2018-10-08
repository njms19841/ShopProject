
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

namespace NFine.Application.APPManage
{
    public class marketSalesApp
    {
        private ImarketSalesRepository service = new marketSalesRepository();
        
        private IPsiSalesEmpOrgRepository empService = new PsiSalesEmpOrgRepository();
        private IPsiSalesOrgRepository orgService = new PsiSalesOrgRepository();
        private IaspnetusersRepository userService = new aspnetusersRepository();
        public int getPopCount(String shopCode)
        {
            return service.IQueryable().Count(p => p.sales_ShopNo.Equals(shopCode) && p.Active == 1);
        }
        public List<marketSalesEntity> getPop(String shopCode)
        {
            return service.IQueryable().Where(p => p.sales_ShopNo.Equals(shopCode) && p.Active == 1).ToList();
        }
        public UserInfoResultModel GetUserInfo(string id)
        {
            return service.GetUserInfo(id);
        }
        public UserInfoResultModel GetUserInfo()
        {
            marketSalesEntity ent= service.IQueryable().First();
            return new UserInfoResultModel()
            { No=ent.sales_No };

        }
        
        public List<PsiSalesEmpOrgEntity> getPOrgUserInfo(string salesNo)
        {
            return empService.FindList("select * from psi_salesemporg where EMPLOYEE_CODE<>'" + salesNo + "' and org_id in(select PARENT_ID from psi_salesorg where  id in (select org_id from psi_salesemporg where EMPLOYEE_CODE='" + salesNo + "' ))").ToList();

        }
        public List<PsiSalesEmpOrgEntity> getSubOrgUserInfo(string salesNo)
        {
           return empService.FindList("select * from psi_salesemporg where org_id in(select id from psi_salesorg where  PARENT_ID in (select org_id from psi_salesemporg where EMPLOYEE_CODE='" + salesNo + "' ))").ToList();

        }
        public string findUserNameBy(string no)
        {
            var list = userService.IQueryable().Where(p =>p.UserName.Equals(no)||  p.EMPLOYEE_CODE.Equals(no) || p.PhoneNumber.Equals(no) || p.Email.Equals(no)).Where(p=>p.active==1);
            if (list.Count() > 0)
            {
                return list.First().UserName;
            }
            return null;
            
        }
        public string findUserNameByEmpCode(string emp_Code)
        {
            var list = userService.IQueryable().Where(p =>  p.EMPLOYEE_CODE.Equals(emp_Code));
            if (list.Count() > 0)
            {
                return list.First().UserName;
            }
            return null;

        }
        public string findNameByEmpCode(string emp_Code)
        {
            var list = userService.IQueryable().Where(p => p.EMPLOYEE_CODE.Equals(emp_Code));
            if (list.Count() > 0)
            {
                return list.First().EMPLOYEE_NAME;
            }
            return null;

        }
        public List<marketSalesEntity> getShopUserInfo(string salesNo)
        {
            return service.FindList("select * from market_sales where POP_TYPE_CODE= 'GuideMan' and  sales_ShopNo in( select sales_ShopNo from market_sales where sales_No='" + salesNo + "' and Active=1)   and Active=1").ToList();

        }
        public List<marketSalesEntity> getGuidManByShop(string shopCode)
        {
            return service.FindList("select * from market_sales where POP_TYPE_CODE= 'GuideMan' and  sales_ShopNo='"+ shopCode + "'   and Active=1").ToList();

        }
        public UserInfoResultModel getUserInfoBySalesNo(string salesNo)
        {
            return service.GetUserInfoBySalesNo(salesNo);


        }
        public UserInfoResultModel getUserInfoByOrgId(string OrgId)
        {
            return empService.GetUserInfoByOrgId(OrgId);


        }
        public List<PsiSalesEmpOrgEntity> getEmpOrgBySalesNo(string salesNo)
        {
            return empService.IQueryable().Where(p => p.EMPLOYEE_CODE.Equals(salesNo) && p.ACTIVE_FLAG == 1).ToList();


        }
        
        public PsiSalesEmpOrgEntity getAdminExecutiveById(string id)
        {
           
            var ent = orgService.FindEntity(id);
            var userEnts = empService.IQueryable().Where(p => p.ORG_ID.Equals(id) && p.JOB_CODE.Equals("AdminExecutive") && p.ACTIVE_FLAG==1);
            
            if (userEnts.Count()>0 )
            {
                return userEnts.First();
            }
            else
            {
                return getAdminExecutiveById(ent.PARENT_ID);
            }
        }

        /// <summary>
        /// 查询固定的父级类型组织
        /// </summary>
        /// <param name="subId"></param>
        /// <param name="typeCode"></param>
        /// <returns></returns>
        public PsiSalesOrgEntity getOrginfoBySubId(string subId, string typeCode)
        {
            PsiSalesOrgEntity OrgEntity = null;
            var ent = orgService.FindEntity(subId);
            if (ent.MANAGE_ORG_TYPE_CODE.Equals(typeCode))
            {
                return ent;
            }
            else
            {
                return getOrginfoBySubId(ent.PARENT_ID, typeCode);
            }
        }
        /// <summary>
        /// 查询子级固定的类型组织
        /// </summary>
        /// <param name="pId"></param>
        /// <param name="typeCode"></param>
        /// <returns></returns>
        public List<PsiSalesOrgEntity> getOrginfoByPCode(string pId,string typeCode)
        {
            List<PsiSalesOrgEntity> entlist = new List<PsiSalesOrgEntity>();
            var list = orgService.IQueryable().Where(p => p.PARENT_ID.Equals(pId));
            foreach (var ent in list)
            {
                if (ent.MANAGE_ORG_TYPE_CODE.Equals(typeCode))
                {
                    entlist.Add(ent);
                }
                else
                {
                   var tempList=  getOrginfoByPCode(ent.id, typeCode);
                    if (tempList.Count > 0)
                    {
                        entlist.AddRange(tempList);
                    }
                }
            }
            return entlist;
        }

        /// <summary>
        /// 查询子级组织和人员
        /// </summary>
        /// <param name="pId"></param>
        /// <param name="typeCode"></param>
        /// <returns></returns>
        public List<PsiSalesOrgEntity> getOrginfoByPCode(string pId, out List<PsiSalesEmpOrgEntity> PopList)
        {
            PopList = new List<PsiSalesEmpOrgEntity>();
            List<PsiSalesOrgEntity> entlist = new List<PsiSalesOrgEntity>();
            var list = orgService.IQueryable().Where(p => p.PARENT_ID.Equals(pId));
            foreach (var ent in list)
            {
                
                    entlist.Add(ent);
                PopList.AddRange(empService.IQueryable().Where(p => p.ORG_ID.Equals(ent.id) && p.ACTIVE_FLAG==1).ToList());
                var PopListTemp = new List<PsiSalesEmpOrgEntity>();
                var tempList = getOrginfoByPCode(ent.id, out PopListTemp);
                    PopList.AddRange(PopListTemp);
                    if (tempList.Count > 0)
                    {
                        entlist.AddRange(tempList);
                    }
                
            }
            return entlist;
        }


        public PsiSalesOrgEntity getOrgInfo(string orgId)
        {
            return orgService.FindEntity(orgId);
        }
        public List<PsiSalesEmpOrgEntity> getEmpOrgBySalesNo(string salesNo,string jobCode)
        {
            return empService.IQueryable().Where(p => p.EMPLOYEE_CODE.Equals(salesNo) && p.ACTIVE_FLAG == 1 && p.JOB_CODE.Equals(jobCode)).ToList();


        }
        public List<PsiSalesEmpOrgEntity> getEmpOrgBySalesNoTow(string salesNo)
        {
            return empService.IQueryable().Where(p => p.EMPLOYEE_CODE.Equals(salesNo) && p.ACTIVE_FLAG == 1).ToList();


        }
        public string getOrgName(string salesNo)
        {
            var orgs = getEmpOrgBySalesNoTow(salesNo).ToList();
            if (orgs.Count > 0)
            {
                var org = orgs.First();
                var temp = orgService.IQueryable().Where(p => p.id.Equals(org.ORG_ID));
                if (temp!=null && temp.Count()> 0)
                {
                    return temp.First().MANAGE_ORG_NAME;
                }
                else
                {
                    return "";
                }
            }
            else
            {
                return "";
            }
        }
        public string getUserName(string sales_No)
        {
            var temp = service.IQueryable().Where(p => p.sales_No.Equals(sales_No));
            if (temp != null && temp.Count() > 0)
            {
                return temp.First().sales_Name;
            }
            else
            {
                var temp2 = empService.IQueryable().Where(p => p.EMPLOYEE_CODE.Equals(sales_No));
                if (temp2 != null && temp2.Count() > 0)
                {
                    return temp2.First().EMPLOYEE_NAME;
                }
                else
                {
                    return sales_No;
                }
            }
        }
    }
}
