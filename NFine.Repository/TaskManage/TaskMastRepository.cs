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

namespace NFine.Repository.TaskManage
{
    public class TaskMastRepository : RepositoryBase<TaskMastEntity>, ITaskMastRepository
    {
        public void readTask(string taskId, string userId)
        {
            var ent = this.FindEntity(taskId);
            if (ent.RECEIVE_EMPLOYEE_CODE == null || ent.RECEIVE_EMPLOYEE_CODE.Equals(""))
            {
                return;
            }
            if (!ent.isRead.HasValue || ent.isRead.Value == 0)
            {
                ent.isRead = 1;
                ent.ReadTime = DateTime.Now;
                JS5_S12_CRM_MESSAGE_READ_LOGTableAdapter ad = new JS5_S12_CRM_MESSAGE_READ_LOGTableAdapter();

                ad.Insert(System.Guid.NewGuid().ToString(), 1, ent.RECEIVE_EMPLOYEE_CODE, System.DateTime.Now.ToString("yyyyMMddHHmmss"), System.DateTime.Now.ToString("yyyyMMddHHmmss"),
                    "APP@" + System.DateTime.Now.ToString("yyyyMMddHHmmss"), "APP@" + System.DateTime.Now.ToString("yyyyMMddHHmmss"), "FLNET", 1, ent.MESSAGE_BILL_NO,(decimal)ent.MESSAGE_SUB_NO,
                    ent.RECEIVE_EMPLOYEE_CODE,ent.RECEIVE_EMPLOYEE_NAME,ent.RECEIVE_MANAGE_ORG_ID,ent.RECEIVE_JOB_CODE,"002","已读","APP","APP读取","无");
                this.Update(ent);
                

            }
        }
        public void ReplyTask(string taskId, string userId, string title, string context, string fileId, string fileExt)
        {
            var ent = this.FindEntity(taskId);
            if (ent.RECEIVE_EMPLOYEE_CODE == null || ent.RECEIVE_EMPLOYEE_CODE.Equals(""))
            {
                return;
            }
            if (!ent.isReply.HasValue || ent.isReply.Value == 0)
            {
                ent.isReply = 1;
                ent.REPLY_SUBJECT = title;
                ent.REPLY_CONTENT = context;
                ent.REPLY_FileId = fileId;
                ent.REPLY_fileExt = fileExt;
                JS5_S12_CRM_MESSAGE_REPLY_UPTableAdapter ReplyAd = new JS5_S12_CRM_MESSAGE_REPLY_UPTableAdapter();
                string id = System.Guid.NewGuid().ToString();
                ReplyAd.Insert(id, 1, ent.RECEIVE_EMPLOYEE_CODE, System.DateTime.Now.ToString("yyyyMMddHHmmss"), System.DateTime.Now.ToString("yyyyMMddHHmmss"), "APP@" + System.DateTime.Now.ToString("yyyyMMddHHmmss")
                    , "APP@" + System.DateTime.Now.ToString("yyyyMMddHHmmss"), "FLNET", 1, ent.MESSAGE_BILL_NO, (decimal)ent.MESSAGE_SUB_NO, title, context,
                    ent.RECEIVE_EMPLOYEE_CODE,ent.RECEIVE_EMPLOYEE_NAME,ent.RECEIVE_MANAGE_ORG_ID,ent.RECEIVE_JOB_CODE,"001", "一般回复","无",id,
                    "Normal","正常",1, "PSIadmin_APP", System.DateTime.Now,System.DateTime.Now,0,"",null,"");
                DataSynchronizationLib.DataSetPopTableAdapters.JS5_S12_SALES_FILE_UPLOADTableAdapter fileAd = new JS5_S12_SALES_FILE_UPLOADTableAdapter();
                if (fileId != null && !fileId.Equals(""))
                {
                    string[] fileids = fileId.Split(",".ToCharArray());
                    string physical_path = System.DateTime.Now.ToString("yyyyMM");

                    foreach (string fileid in fileids)
                    {
                        //写入报告信息
                        fileAd.InsertQuery(System.Guid.NewGuid().ToString(), fileid + ".jpg", ent.RECEIVE_EMPLOYEE_CODE, System.DateTime.Now, ent.RECEIVE_EMPLOYEE_CODE, 
                            System.DateTime.Now, 1, fileExt, 0, "", fileid + "."+ fileExt, physical_path, "", "P13047", id, "PDG1367", "001", "回复报告", id,
                            "Normal", "正常", 1, "PSIadmin_APP", System.DateTime.Now, System.DateTime.Now);
                    }

                }
               SCMQueriesTableAdapter spAd = new DataSynchronizationLib.SCMTableAdapters.SCMQueriesTableAdapter();
                String outMessage = "";
                spAd.SP_CRM_MESSAGE_REPLY_UP("FLNET", id, out outMessage);
                if (!outMessage.Equals("OK"))
                {
                    throw new Exception(outMessage);
                }
                
                this.Update(ent);
            }
        }
    }
}
