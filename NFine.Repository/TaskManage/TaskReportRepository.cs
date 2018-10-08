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
using DataSynchronizationLib.SCMTableAdapters;
using DataSynchronizationLib.DataSetPopTableAdapters;
using NFine.Repository.APPManage;

namespace NFine.Repository.TaskManage
{
    public class TaskReportRepository : RepositoryBase<TaskReportEntity>, ITaskReportRepository
    {
        public void AllowReport(string id)
        {
            var oldent = this.FindEntity(id);
            oldent.state = 2;
            this.Update(oldent);
        }

        public void CreateReport(TaskReportEntity ent)
        {
           
                ent.reportTime = System.DateTime.Now;
                this.Insert(ent);
            try
            {
                uploadFile(ent, "Normal", "正常");
            } catch (Exception ex)
            {
                this.Delete(ent);
                throw ex;
            }


        }
        public void uploadFile(TaskReportEntity ent,string UP_TYPE,string UP_TYPE_NAME)
        {
            JS5_S12_CRM_FILE_INFO_UPLOADTableAdapter ad = new JS5_S12_CRM_FILE_INFO_UPLOADTableAdapter();
            string id = System.Guid.NewGuid().ToString();
            IPsiSalesEmpOrgRepository orgRep = new PsiSalesEmpOrgRepository();
          
            var orglist=orgRep.FindList("select a.* from psi_salesemporg a join aspnetusers b on a.EMPLOYEE_CODE=b.UserName where b.Id='"+ ent.userId+ "'");
            string orgId = "";
            if (orglist.Count() > 0)
            {
                orgId = orglist.First().ORG_ID;
            }
            else
            {
                ImarketSalesShopRepository shopRepository = new marketSalesShopRepository();
                var shop = shopRepository.getShopByUserId(ent.userId);
                if (shop.Count>0)
                {
                    orgId = shop.First().ORG_ID;
                }
            }
             

             
           
            ad.Insert(id, 1, ent.userId, ent.reportTime.Value.ToString("yyyyMMddHHmmss"), System.DateTime.Now.ToString("yyyyMMddHHmmss"), "PSIadmin_APP@" + ent.reportTime.Value.ToString("yyyyMMddHHmmss") + "@APP",
               "PSIadmin_APP@" + System.DateTime.Now.ToString("yyyyMMddHHmmss") + "@APP", "FLNET", 1,ent.fileName,"099", "其他", "002", "工作報告", orgId, "002", "APP檔案管理","无",ent.id, UP_TYPE, UP_TYPE_NAME,
               1, "PSIadmin_APP", System.DateTime.Now, System.DateTime.Now
               );
            DataSynchronizationLib.DataSetPopTableAdapters.JS5_S12_SALES_FILE_UPLOADTableAdapter fileAd = new JS5_S12_SALES_FILE_UPLOADTableAdapter();
            string physical_path =ent.reportTime.Value.ToString("yyyyMM");
            fileAd.InsertQuery(System.Guid.NewGuid().ToString(),ent.fileId+"."+ent.fileExt, "PSIadmin_APP@" + System.DateTime.Now.ToString("yyyyMMddHHmmss") + "@APP", System.DateTime.Now, "PSIadmin_APP@" + System.DateTime.Now.ToString("yyyyMMddHHmmss") + "@APP", System.DateTime.Now, 
                1, ent.fileExt, 0, "", ent.fileName, physical_path, "", "P13049", id, "CDG1294", "002", "工作報告", id,
                               "Normal", "正常", 1, "PSIadmin_APP", System.DateTime.Now, System.DateTime.Now);
            SCMQueriesTableAdapter spAd = new SCMQueriesTableAdapter();
            String outMessage = "";

            spAd.SP_CRM_FILE_INFO_UPLOAD("FLNET", id, out outMessage);
            if (!outMessage.Equals("OK"))
            {
                throw new Exception(outMessage);
            }
        }

        public void DeleteReport(string id)
        {
            var oldent = this.FindEntity(id);
            this.Delete(oldent);
        }

        public void UpdateReport(TaskReportEntity ent)
        {
            var oldent = this.FindEntity(ent.id);
            string title = oldent.title;
            string context = oldent.context;
            string fileId = oldent.fileId;
            int state = oldent.state.Value;
            oldent.title = ent.title;
            oldent.context = ent.context;
            oldent.fileId = ent.fileId;
            oldent.state = 1;

            this.Update(oldent);
            try
            {
                uploadFile(oldent, "Change", "調整");
            }
            catch (Exception ex)
            {
                oldent.title = title;
                oldent.context = context;
                oldent.fileId = fileId;
                oldent.state = state;
                this.Update(oldent);
                throw ex;
            }
        }
    }
}
