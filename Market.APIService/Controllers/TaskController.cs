using DataSynchronizationLib.SCMTableAdapters;
using Market.APIService.Models;
using Microsoft.AspNet.Identity;
using NFine.Application.APPManage;
using NFine.Application.TaskManage;
using NFine.Domain._02_ViewModel;
using NFine.Domain._03_Entity.APPManage;
using NFine.Domain._03_Entity.TaskManage;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;
using Tools;

namespace Market.APIService.Controllers
{
    [Authorize]
    [RoutePrefix("api/Task")]
    public class TaskController : ApiController
    {

        /// <summary>
        /// 通过multipart/form-data方式上传文件
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [Route("UploadFile")]
        [AllowAnonymous]
        public async Task<fileViewModel> PostFile(string ext)
        {
            try
            {
                // 是否请求包含multipart/form-data。
                if (!Request.Content.IsMimeMultipartContent())
                {

                    throw new HttpResponseException(HttpStatusCode.UnsupportedMediaType);
                }

                string root = HttpContext.Current.Server.MapPath("/UploadFiles/");
                if (!Directory.Exists(HttpContext.Current.Server.MapPath("~/UploadFiles/")))
                {
                    Directory.CreateDirectory(HttpContext.Current.Server.MapPath("~/UploadFiles/"));
                }

                var provider = new MultipartFormDataStreamProvider(root);

                StringBuilder sb = new StringBuilder(); // Holds the response body

                // 阅读表格数据并返回一个异步任务.
                await Request.Content.ReadAsMultipartAsync(provider);

                // 如何上传文件到文件名.
                foreach (var file in provider.FileData)
                {
                    string orfilename = file.Headers.ContentDisposition.FileName.TrimStart('"').TrimEnd('"');
                    FileInfo fileinfo = new FileInfo(file.LocalFileName);
                    //sb.Append(string.Format("Uploaded file: {0} ({1} bytes)\n", fileInfo.Name, fileInfo.Length));
                    //最大文件大小 
                    //int maxSize = Convert.ToInt32(SettingConfig.MaxSize);
                    marketreportInfoApp app = new marketreportInfoApp();
                    marketFilesEntity ent = new marketFilesEntity()
                    {
                        id = System.Guid.NewGuid().ToString(),
                        ContextType = "image/png",
                    };
                    app.InsertFile(ent);
                    using (new Impersonator("administrator", "", "!QAZ2wsx"))
                    {
                        if (Directory.Exists(System.Configuration.ConfigurationManager.AppSettings["filePatch"] + "\\" + System.DateTime.Now.ToString("yyyyMM")) == false)//如果不存在就创建file文件夹
                        {
                            Directory.CreateDirectory(System.Configuration.ConfigurationManager.AppSettings["filePatch"] + "\\" + System.DateTime.Now.ToString("yyyyMM"));
                        }
                        fileinfo.CopyTo(System.Configuration.ConfigurationManager.AppSettings["filePatch"] + "\\" + System.DateTime.Now.ToString("yyyyMM") + "\\" + ent.id + "." + ext, true);

                        // System.IO.File.Copy(fileDir + "\\" + ent.id + ".jpg", System.Configuration.ConfigurationManager.AppSettings["filePatch"] + "\\" + System.DateTime.Now.ToString("yyyyMM") + "\\" + ent.id + ".jpg", true);
                    }
                    fileinfo.Delete();//删除原文件
                    return new fileViewModel() { fileId = ent.id };
                    //
                }
            }
            catch (System.Exception e)
            {
                return new fileViewModel() { fileId = e.Message };
            }
            return new fileViewModel() { fileId = "" };
        }
        private QueryTaskModel markTasks(int mastStatus,TaskMastEntity mastEnt,List<SubTaskStatusEntity> statusEnts,string patch,DateTime day)
        {
            QueryTaskModel model = new QueryTaskModel()
            {
                id = mastEnt.id,
                alertType = mastEnt.alertType.Value,
                createdUserId = mastEnt.createdUserId,
                CustomerCode = mastEnt.CustomerCode,
                CustomerName = mastEnt.CustomerName,
                day = day,
                starTime = mastEnt.starTime.Value,
                dayString = day.ToString("yyyyMMdd"),
                desc = mastEnt.desc,
                endTime = mastEnt.endTime.Value
              
                , fileName=mastEnt.fileName,
                freqType = mastEnt.freqType.Value,
                importantType = mastEnt.importantType.Value,
               
                taskName = mastEnt.taskName,
                taskType = mastEnt.taskType,
                taskTypeName = mastEnt.taskTypeName,
                 isAll= mastEnt.isAll.HasValue?mastEnt.isAll.Value:0,
                  taskSource=mastEnt.taskSource,
                   MESSAGE_REPLY_TYPE_CODE= mastEnt.MESSAGE_REPLY_TYPE_CODE,
                    MESSAGE_REPLY_TYPE_NAME= mastEnt.MESSAGE_REPLY_TYPE_NAME,
                     isRead= mastEnt.isRead.HasValue? mastEnt.isRead.Value:1,
                       URGENCY_TYPE_CODE= mastEnt.URGENCY_TYPE_CODE,
                        URGENCY_TYPE_NAME= mastEnt.URGENCY_TYPE_NAME,
                         REPLY_SUBJECT= mastEnt.REPLY_SUBJECT,
                          REPLY_CONTENT= mastEnt.REPLY_CONTENT,  isReply=mastEnt.isReply.HasValue?mastEnt.isReply.Value:0
                          , isMessage=(mastEnt.RECEIVE_EMPLOYEE_CODE==null|| mastEnt.RECEIVE_EMPLOYEE_CODE.Equals(""))?0:1
                           , taskUrl= mastEnt.taskUrl, address=mastEnt.address, Location=mastEnt.Location


            };
            /*
            if (mastEnt.taskName.Equals("顾客信息推送"))
            {
                model.taskUrl = "https://iretailerapp.flnet.com/APPQC/PSI/popinfo?billNo="+mastEnt.MESSAGE_BILL_NO;
            }
            */
            if (mastEnt.REPLY_FileId != null && !mastEnt.REPLY_FileId.Equals(""))
            {
                model.Reply_fileUrl = "https://iretailerapp.flnet.com/Messages/APPUploadFile/" + patch + "/" + mastEnt.REPLY_FileId + "."+mastEnt.REPLY_fileExt;
                model.REPLY_FileId = mastEnt.REPLY_FileId;
            }
            if (mastEnt.fileId != null && !mastEnt.fileId.Equals(""))
            {
                model.fileUrl = "https://iretailerapp.flnet.com/Messages/APPUploadFile/" + patch + "/" + mastEnt.fileId +"."+ mastEnt.fileExt;
                model.fileId = mastEnt.fileId;
                model.fileExt = mastEnt.fileExt;
              }
            if (mastEnt.isDelete.HasValue)
            {
                model.isDelete = mastEnt.isDelete.Value;
            }
            if (mastStatus == 1) //待接收
            {

                
                if (mastEnt.isDelete != null && mastEnt.isDelete == 1)
                {
                    model.status = 5;
                }
                else
                {
                    /**
                    DateTime t1 = Convert.ToDateTime(mastEnt.starTime.Value.ToString("yyyy-MM-dd 00:00:00"));
                    DateTime t2 = Convert.ToDateTime(System.DateTime.Now.ToString("yyyy-MM-dd 00:00:00"));
                    if (DateTime.Compare(t1, t2) > 0)//表示任务过期
                    {
                        model.status = 3;
                    }
                    else
                    {
                        model.status = 1;
                    }
                    */
                    model.status = 1;
                }
               

            }
            else if (mastStatus == 2)// 待处理
            {
            
                
                if (mastEnt.isDelete != null && mastEnt.isDelete == 1)
                {
                    model.status = 5;
                }
                else
                {
                    var statusEnt = statusEnts.Find(p => p.day.Equals(day.ToString("yyyyMMdd")) && p.taskId.Equals(mastEnt.id));
                    if (statusEnt != null)//判断是否有子任务状态
                    {
                        if (statusEnt.status == 1)
                        {
                            DateTime t1 = Convert.ToDateTime(model.day.ToString("yyyy-MM-dd 00:00:00"));
                            DateTime t2 = Convert.ToDateTime(System.DateTime.Now.ToString("yyyy-MM-dd 00:00:00"));
                            if (DateTime.Compare(t2, t1) > 0)//表示任务过期
                            {
                                model.status = 3;
                            }
                            else
                            {
                                model.status = 2;
                            }

                        }
                        else if (statusEnt.status == 2)
                        {
                            model.status = 4;
                        }
                    }
                    else
                    {
                        DateTime t1 = Convert.ToDateTime(model.day.ToString("yyyy-MM-dd 00:00:00"));
                        DateTime t2 = Convert.ToDateTime(System.DateTime.Now.ToString("yyyy-MM-dd 00:00:00"));
                        if (DateTime.Compare(t2, t1) > 0)//表示任务过期
                        {
                            model.status = 3;
                        }
                        else
                        {
                            model.status = 2;
                        }
                    }
                }
            }
            return model;
        }
        private string getOrgDesc(string orgId, List<PsiSalesOrgEntity> orgs)
        {
            var ent = orgs.Find(p => p.id.Equals(orgId));
            if (ent != null)
            {
                string OrgName = ent.MANAGE_ORG_NAME;
                if (ent.PARENT_ID!=null)
                {
                    OrgName = getOrgDesc(ent.PARENT_ID, orgs) + ">" + OrgName;
                }
                return OrgName;
            }
            return "";
        }

        /// <summary>
        /// 获取用户列表
        /// </summary>
        /// <param name="page">分页的页数</param>
        /// <param name="queryString">查询的关键字</param>
        /// <returns></returns>
        [HttpGet]
        [Route("GetUsers")]
        public List<TaskUser> GetUsers(int page, string queryString)
        {
            try
            {
                List<TaskUser> users = new List<TaskUser>();
                TaskPopApp popApp = new TaskPopApp();
                if (queryString == null)
                {
                    queryString = "";
                }
                var orgEnts = popApp.getAllOrg();

                var ents = popApp.getAllOrgUser();
                foreach (var ent in ents)
                {
                    users.Add(new TaskUser() { GroupName = "ALL", userId = ent.id, userName = ent.EMPLOYEE_NAME, JobName = ent.Job_NAME, Desc = getOrgDesc(ent.ORG_ID, orgEnts) });
                }
                return users;
            }
            catch (Exception ex)
            {
                var resp = new HttpResponseMessage(HttpStatusCode.InternalServerError)
                {
                    Content = new StringContent(ex.ToString()),
                    ReasonPhrase = "error"
                };
                throw new HttpResponseException(resp);

            }
        }
        /// <summary>
        /// 抓取任务的参与用户和责任用户
        /// </summary>
        /// <param name="taskId"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("GetTaskUsers")]
        public TaskUsers GetTaskUsers(string taskId)
        {
            try
            {
                TaskUsers users = new TaskUsers();
                users.participateUser = new List<TaskUser>();
                users.responsibilityUser = new List<TaskUser>();
                marketSalesApp salesApp = new marketSalesApp();
                TaskPopApp popApp = new TaskPopApp();
                var ents= popApp.getTaskPop(taskId);
                foreach (var ent in ents)
                {
                    if (ent.userType == 1)
                    {
                        users.responsibilityUser.Add(new TaskUser() { userId = ent.UserId, userName = salesApp.GetUserInfo(ent.UserId).Name });
                    }
                    else 
                        {
                            users.participateUser.Add(new TaskUser() { userId = ent.UserId, userName = salesApp.GetUserInfo(ent.UserId).Name });
                        }
                }
                return users;
            }
            catch (Exception ex)
            {
                var resp = new HttpResponseMessage(HttpStatusCode.InternalServerError)
                {
                    Content = new StringContent(ex.ToString()),
                    ReasonPhrase = "error"
                };
                throw new HttpResponseException(resp);

            }
        }
        /// <summary>
        /// 抓取日期范围内的任务
        /// </summary>
        /// <param name="queryType">查询类型，1:日程，2:任务,3:行事历</param>
        /// <param name="startDay">20171201</param>
        /// <param name="endDay">20171230</param>
        /// <returns></returns>
        [HttpGet]
        [Route("GetTasks")]
        public List<QueryTaskModel> GetTasks(int queryType, string startDay, string endDay)
        {
            try
            {
                DateTime startTime = DateTime.ParseExact(startDay + " 00:00:00", "yyyyMMdd HH:mm:ss", CultureInfo.CurrentCulture);
                DateTime endTime = DateTime.ParseExact(endDay + " 23:59:59", "yyyyMMdd HH:mm:ss", CultureInfo.CurrentCulture);
                String userid = User.Identity.GetUserId();
                List<QueryTaskModel> models = new List<QueryTaskModel>();
                TaskMastApp mastApp = new TaskMastApp();
                var tempMastEnt = mastApp.getTask(userid, startTime.ToString("yyyy-MM-dd"),
                   endTime.ToString("yyyy-MM-dd"),queryType);
               var mastEnts = tempMastEnt.Distinct();
                SubTaskStatusApp statusApp = new SubTaskStatusApp();
                var statusEnts= statusApp.getTaskStatus(startTime.ToString("yyyy-MM-dd HH:mm:ss"), endTime.ToString("yyyy-MM-dd HH:mm:ss"), userid);
                TaskPopApp popApp = new TaskPopApp();
                var mastStatusList = popApp.getTaskStatusList(startTime.ToString("yyyy-MM-dd"),
                   endTime.ToString("yyyy-MM-dd"), userid);


                foreach (var mastEnt in mastEnts)
                {
                    int mastStatus = 2;
                    var list = mastStatusList.Where(p => p.taskId.Equals(mastEnt.id));
                    if (list != null && list.Count() > 0)
                    {
                        mastStatus = list.First().status.Value;
                    }
                   //int mastStatus= mastStatusList.Where(p=>p.taskId.Equals(mastEnt.id))   //popApp.getTaskStatus(mastEnt.id, userid);
                    string patch = mastEnt.createdTime.Value.ToString("yyyyMM");
                    // 1 = 一次
                    if (mastEnt.freqType == 1)
                    {
                        models.Add(this.markTasks(mastStatus, mastEnt, statusEnts, patch, mastEnt.starTime.Value));
                    }
                    //4 = 每日
                    else if (mastEnt.freqType == 4)
                    {
                        
                        TimeSpan ts = endTime - startTime;
                        DateTime temptime = DateTime.ParseExact( startTime.ToString("yyyy-MM-dd")+ mastEnt.starTime.Value.ToString(" HH:mm:ss"),"yyyy-MM-dd HH:mm:ss", CultureInfo.CurrentCulture);
                        DateTime temptime2 = DateTime.ParseExact(startTime.ToString("yyyy-MM-dd") + mastEnt.endTime.Value.ToString(" HH:mm:ss"), "yyyy-MM-dd HH:mm:ss", CultureInfo.CurrentCulture);
                        for (int i = 0; i <= ts.Days; i++)
                        {
                           
                            if (DateTime.Compare(temptime, mastEnt.starTime.Value) >= 0 && DateTime.Compare(mastEnt.endTime.Value, temptime2) >= 0)
                            {
                                models.Add(this.markTasks(mastStatus, mastEnt, statusEnts, patch, temptime));
                            }
                            temptime=temptime.AddDays(1);
                            temptime2 = temptime2.AddDays(1);

                        }
                        
                    }
                    //8 = 每周
                    else if (mastEnt.freqType == 8)
                    {
                        TimeSpan ts = endTime - startTime;
                        DateTime temptime = DateTime.ParseExact(startTime.ToString("yyyy-MM-dd") + mastEnt.starTime.Value.ToString(" HH:mm:ss"), "yyyy-MM-dd HH:mm:ss", CultureInfo.CurrentCulture);
                        DateTime temptime2 = DateTime.ParseExact(startTime.ToString("yyyy-MM-dd") + mastEnt.endTime.Value.ToString(" HH:mm:ss"), "yyyy-MM-dd HH:mm:ss", CultureInfo.CurrentCulture);

                        for (int i = 0; i <= ts.Days; i++)
                        {
                            //DateTime tempDate = mastEnt.starTime.Value.AddDays(i);
                            if (DateTime.Compare(temptime, mastEnt.starTime.Value) >= 0 && DateTime.Compare(mastEnt.endTime.Value, temptime2) >= 0 && temptime.DayOfWeek == mastEnt.starTime.Value.DayOfWeek)
                            {
                                models.Add(this.markTasks(mastStatus, mastEnt, statusEnts, patch, temptime));
                            }
                            temptime = temptime.AddDays(1);
                            temptime2 = temptime2.AddDays(1);
                        }
                    }
                    //16 = 每月
                    else if (mastEnt.freqType == 16)
                    {
                        TimeSpan ts = endTime - startTime;
                        DateTime temptime = DateTime.ParseExact(startTime.ToString("yyyy-MM-dd") + mastEnt.starTime.Value.ToString(" HH:mm:ss"), "yyyy-MM-dd HH:mm:ss", CultureInfo.CurrentCulture);
                        DateTime temptime2 = DateTime.ParseExact(startTime.ToString("yyyy-MM-dd") + mastEnt.endTime.Value.ToString(" HH:mm:ss"), "yyyy-MM-dd HH:mm:ss", CultureInfo.CurrentCulture);

                        for (int i = 0; i <= ts.Days; i++)
                        {
                            //DateTime tempDate = mastEnt.starTime.Value.AddDays(i);
                            if (DateTime.Compare(temptime, mastEnt.starTime.Value) >= 0 && DateTime.Compare(mastEnt.endTime.Value, temptime2) >= 0 && temptime.Day== mastEnt.starTime.Value.Day)
                            {
                                models.Add(this.markTasks(mastStatus, mastEnt, statusEnts, patch, temptime));
                            }
                            temptime = temptime.AddDays(1);
                            temptime2 = temptime2.AddDays(1);
                        }
                    }
                }

                return models.OrderByDescending(p=>p.starTime).ToList();
            }
            catch (Exception ex)
            {
                var resp = new HttpResponseMessage(HttpStatusCode.InternalServerError)
                {
                    Content = new StringContent(ex.ToString()),
                    ReasonPhrase = "error"
                };
                throw new HttpResponseException(resp);

            }
        }
        /// <summary>
        /// 查询工作报告
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Route("GetReports")]

        public List<TaskReport> GetReports()
        {
            try
            {
                marketSalesApp salesApp = new marketSalesApp();
                List<TaskReport> models = new List<TaskReport>();
                TaskReportApp app = new TaskReportApp();
                var ents = app.getReports(User.Identity.GetUserId());
                foreach (var ent in ents)
                {
                   var model = new TaskReport() {
                        id = ent.id,
                        context = ent.context,
                        reportTime = ent.reportTime.Value,
                        reportType = ent.reportType,
                        state = ent.state,
                        title = ent.title,
                        userId = ent.userId,
                       userName= salesApp.GetUserInfo(ent.userId).Name,
                       
                        users = new TaskReportUsers() { AllowUser=new List<TaskUser>(), ReadUser= new List<TaskUser>() }
                         
                    };
                    if (ent.fileId != null)
                    {
                        string patch = ent.reportTime.Value.ToString("yyyyMM");
                        model.fileId = ent.fileId;
                        model.fileExt = ent.fileExt;
                        model.fileName = ent.fileName;
                        model.fileUrl= "https://iretailerapp.flnet.com/Messages/APPUploadFile/" + patch + "/" + ent.fileId + "." + ent.fileExt;
                    }
                    if (ent.audoFileId != null)
                    {
                        string patch = ent.reportTime.Value.ToString("yyyyMM");
                        model.audoFileId = ent.audoFileId;
                        model.audoFileUrl= "https://iretailerapp.flnet.com/Messages/APPUploadFile/" + patch + "/" + ent.audoFileId + ".aac";

                    }
                    var users = app.getReportUsers(ent.id);
                   
                    foreach (var user in users)
                    {
                        if (user.userType == 1)
                        {
                            model.users.AllowUser.Add(new TaskUser() { userId = user.userId, userName= salesApp.GetUserInfo(user.userId).Name });
                        }
                        else
                        {
                            model.users.ReadUser.Add(new TaskUser() { userId = user.userId, userName = salesApp.GetUserInfo(user.userId).Name });
                        }
                    }
                    models.Add(model);


                }
                return models;
            }
            catch (Exception ex)
            {
                var resp = new HttpResponseMessage(HttpStatusCode.InternalServerError)
                {
                    Content = new StringContent(ex.ToString()),
                    ReasonPhrase = "error"
                };
                throw new HttpResponseException(resp);

            }
        }
        /// <summary>
        /// 删除报告
        /// </summary>
        /// <param name="ReportId"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("DeleteReport")]
        public async Task<IHttpActionResult> DeleteReport(string ReportId)
        {
            try
            {
                TaskReportApp app = new TaskReportApp();
                app.DeleteReport(ReportId);
                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        /// <summary>
        /// 审阅工作报告
        /// </summary>
        /// <param name="ReportId"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("AllowReport")]
        public async Task<IHttpActionResult> AllowReport(string ReportId)
        {
            try
            {
                TaskReportApp app = new TaskReportApp();
                app.AllowReport(ReportId);
                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        /// <summary>
        /// 上报工作报告
        /// </summary>
        /// <param name="taskReport"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("WorkReport")]
        public async Task<IHttpActionResult> WorkReport(TaskReport taskReport)
        {
            try
            {
                TaskReportApp app = new TaskReportApp();
                if (taskReport.id != null)
                {
                    app.UpdateReport(new TaskReportEntity() {
                         id= taskReport.id,
                          context= taskReport.context,
                           fileId= taskReport.fileId,
                            reportType= taskReport.reportType,
                             state= 1,
                              title= taskReport.title,
                               userId= User.Identity.GetUserId(),
                                fileExt=taskReport.fileExt, fileName=taskReport.fileName
                                 ,audoFileId=taskReport.audoFileId
                    } );
                    app.DeleteReportUser(taskReport.id);
                    foreach (var user in taskReport.AllowUser)
                    {
                        app.CreateReportUser(new TaskReportUserEntity() { id=System.Guid.NewGuid().ToString(), reportId=taskReport.id, userId=user,
                         userType=1});
                    }
                    foreach (var user in taskReport.ReadUser)
                    {
                        app.CreateReportUser(new TaskReportUserEntity()
                        {
                            id = System.Guid.NewGuid().ToString(),
                            reportId = taskReport.id,
                            userId = user,
                            userType = 2
                        });
                    }
                }
                else
                {
                    string id = System.Guid.NewGuid().ToString();
                    app.CreateReport(new TaskReportEntity()
                    {
                        id = id,
                        context = taskReport.context,
                        fileId = taskReport.fileId,
                        reportType = taskReport.reportType,
                        state = 1,
                        title = taskReport.title,
                        userId = User.Identity.GetUserId(),
                        fileExt = taskReport.fileExt,
                        fileName = taskReport.fileName,
                         audoFileId=taskReport.audoFileId
                    });
                    
                    foreach (var user in taskReport.AllowUser)
                    {
                        app.CreateReportUser(new TaskReportUserEntity()
                        {
                            id = System.Guid.NewGuid().ToString(),
                            reportId = id,
                            userId = user,
                            userType = 1
                        });
                    }
                    foreach (var user in taskReport.ReadUser)
                    {
                        app.CreateReportUser(new TaskReportUserEntity()
                        {
                            id = System.Guid.NewGuid().ToString(),
                            reportId = id,
                            userId = user,
                            userType = 2
                        });
                    }
                }

                return Ok();

            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        /// <summary>
        /// 改变任务状态
        /// </summary>
        /// <param name="taskId">任务ID</param>
        /// <param name="status">1:进行中 2：完成</param>
        /// <param name="day">任务的日期 格式为:20180105</param>
        /// <returns></returns>
        [HttpPost]
        [Route("ChangeTaskStatus")]
        public async Task<IHttpActionResult> ChangeTaskStatus(string taskId, int status,string day)
        {
            try
            {
                String userid = User.Identity.GetUserId();
                SubTaskStatusApp statusApp = new SubTaskStatusApp();
                statusApp.changeTaskStatus(taskId, status, day, userid);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }



            return Ok();
        }

        /// <summary>
        /// 接受任务
        /// </summary>
        /// <param name="taskId">任务ID</param>
        /// <param name="allowType">接受类型 2：接受 3：拒绝</param>
        /// <returns></returns>
        [HttpPost]
        [Route("AllowTask")]
        public async Task<IHttpActionResult> AllowTask(string taskId,int allowType)
        {
            try
            {
                String userid = User.Identity.GetUserId();
                TaskPopApp popApp = new TaskPopApp();
                popApp.AllowTask(taskId, userid, 0, allowType);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }



            return Ok();
        }
        /// <summary>
        /// 设置日程状态为已读
        /// </summary>
        /// <param name="taskId"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("ReadTask")]
        public async Task<IHttpActionResult> ReadTask(string taskId)
        {
            try
            {
                TaskMastApp app = new TaskMastApp();
                app.readTask(taskId, User.Identity.GetUserId());
                return Ok();
            
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
    }

}
        /// <summary>
        /// 回复工作报告
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("ReplyTask")]
        public async Task<IHttpActionResult> ReplyTask(ReplyTaskModel model)
        {
            try
            {
                TaskMastApp app = new TaskMastApp();
                app.ReplyTask(model.taskId, User.Identity.GetUserId(), model.title, model.context, model.fileId, model.fileExt);
                return Ok();

            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        /// <summary>
        /// 删除任务
        /// </summary>
        /// <param name="taskId"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("DeleteTask")]
        public async Task<IHttpActionResult> DeleteTask(string taskId)
        {
            try
            {
                TaskMastApp MastApp = new TaskMastApp();
                MastApp.deleteTask(taskId);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }



            return Ok();
        }
        /// <summary>
        /// 修改任务
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("ModifyTask")]
        public async Task<IHttpActionResult> ModifyTask(TaskModel model)
        {
            try
            {
                String userid = User.Identity.GetUserId();
                marketSalesApp salesApp = new marketSalesApp();
                TaskMastApp MastApp = new TaskMastApp();
                MastApp.modifyTask(new TaskMastEntity()
                {
                    id = model.id,
                    alertType = model.alertType,
                    createdUserId = userid,
                    CustomerCode = model.CustomerCode,
                    CustomerName = model.CustomerName,
                    desc = model.desc,
                    endTime = model.endTime,
                    freqType = model.freqType,
                    importantType = model.importantType,
                    starTime = model.starTime,
                    taskName = model.taskName,
                    taskType = model.taskType,
                    taskTypeName = model.taskTypeName,
                    fileId = model.fileId,
                     isAll=model.isAll,
                    modifyTime = System.DateTime.Now, fileName=model.fileName, fileExt=model.fileExt,
                    taskSource=salesApp.GetUserInfo(userid).Name, Location=model.Location, address=model.address

                });
                TaskPopApp popApp = new TaskPopApp();
                popApp.deletePop(model.id);
                popApp.createTaskPop(new TaskPopEntity() { id = System.Guid.NewGuid().ToString(), status = 2, taskId = model.id, UserId = userid, userType = 1 });
                foreach (string pUserid in model.responsibilityUser)
                {
                    if (pUserid.Equals(userid))
                    {
                        popApp.createTaskPop(new TaskPopEntity() { id = System.Guid.NewGuid().ToString(), status = 2, taskId = model.id, UserId = pUserid, userType = 1 });
                    }
                    else
                    {
                        popApp.createTaskPop(new TaskPopEntity() { id = System.Guid.NewGuid().ToString(), status = 1, taskId = model.id, UserId = pUserid, userType = 1 });

                    }
                }
                foreach (string pUserid in model.participateUser)
                {
                    if (pUserid.Equals(userid))
                    {
                        popApp.createTaskPop(new TaskPopEntity() { id = System.Guid.NewGuid().ToString(), status = 2, taskId = model.id, UserId = pUserid, userType = 2 });
                    }
                    else
                    {
                        popApp.createTaskPop(new TaskPopEntity() { id = System.Guid.NewGuid().ToString(), status = 1, taskId = model.id, UserId = pUserid, userType = 2 });

                    }
                }
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }



            return Ok();
        }
        /// <summary>
        /// 发送会员到店通知
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        [AllowAnonymous]
        [Route("sendMember")]
        public List<MemberMessageOutModel> sendMember(List<MemberMessageInModel> model)
        {
            try
            {
                TaskMemberApp app = new TaskMemberApp();
                
                var returnModel = new List<MemberMessageOutModel>();
                List<TaskMemberDataEntity> ents = new List<TaskMemberDataEntity>();
                V_CRM_MEMBER_LIST_APPTableAdapter memberAd = new V_CRM_MEMBER_LIST_APPTableAdapter();
                string bid = System.Guid.NewGuid().ToString();
                foreach (var item in model)
                {
                    var member = memberAd.GetDataByNo(item.mfMemberId);
                    //if (member.Rows.Count > 0  && member.First().VISIT_COUNT>1)
                    //{
                        ents.Add(new TaskMemberDataEntity()
                        {
                            id = Guid.NewGuid().ToString(),
                            beachId = bid,
                            InTime = item.InTime,
                            memberId = item.memberId,
                            MemberTypeCode = item.MemberTypeCode,
                            MemberTypeName = item.MemberTypeName,
                            mfMemberId = item.mfMemberId,
                            picUrl = item.picUrl,
                            shopCode = item.shopCode,
                            IsRead = 0
                        });
                        returnModel.Add(new MemberMessageOutModel() { hasError = false, mfMemberId = item.mfMemberId, message = "", shopCode = item.shopCode });
                    //}
                    //else
                    //{
                      //  returnModel.Add(new MemberMessageOutModel() { hasError = false, mfMemberId = item.mfMemberId, message = "访问次数未达到2次，无需推送", shopCode = item.shopCode });
                    //}
                }
                try
                {
                    app.insertTaskMember(ents, bid);
                }
                catch (Exception ex)
                {
                    for (int i = 0; i < returnModel.Count; i++)
                    {
                        returnModel[i].hasError = true;
                        returnModel[i].message = ex.Message;
                    }
                }
                return returnModel;
            }
            catch (Exception ex)
            {
                var resp = new HttpResponseMessage(HttpStatusCode.InternalServerError)
                {
                    Content = new StringContent(ex.ToString()),
                    ReasonPhrase = "error"
                };
                throw new HttpResponseException(resp);
            }
        }
        /// <summary>
        /// 创建任务
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("CreateTask")]
        public TaskModel CreateTask(TaskModel model)
        {
           
            try
            {
                String userid = User.Identity.GetUserId();
                String taskId = System.Guid.NewGuid().ToString();
                TaskMastApp MastApp = new TaskMastApp();
                marketSalesApp salesApp = new marketSalesApp();
                MastApp.createTask(new TaskMastEntity()
                {
                    id = taskId,
                    alertType = model.alertType,
                    createdUserId = userid,
                    CustomerCode = model.CustomerCode,
                    CustomerName = model.CustomerName,
                    desc = model.desc,
                    endTime = model.endTime,
                    freqType = model.freqType,
                    importantType = model.importantType,
                    starTime = model.starTime,
                    taskName = model.taskName,
                    taskType = model.taskType,
                     isAll=model.isAll,
                    taskTypeName = model.taskTypeName,
                     fileId=model.fileId, createdTime=System.DateTime.Now,modifyTime=System.DateTime.Now,
                    taskSource =  salesApp.GetUserInfo(userid).Name,
                     fileName=model.fileName,
                      fileExt=model.fileExt, address=model.address, Location=model.Location
                      

                });
                model.id = taskId;
                TaskPopApp popApp = new TaskPopApp();
                popApp.createTaskPop(new TaskPopEntity() { id = System.Guid.NewGuid().ToString(), status = 2, taskId = taskId, UserId = userid, userType = 1 });
                foreach (string pUserid in model.responsibilityUser)
                {
                    if (pUserid.Equals(userid))
                    {
                        popApp.createTaskPop(new TaskPopEntity() { id = System.Guid.NewGuid().ToString(), status = 2, taskId = taskId, UserId = pUserid, userType = 1 });
                    }
                    else
                    {
                        popApp.createTaskPop(new TaskPopEntity() { id = System.Guid.NewGuid().ToString(), status = 1, taskId = taskId, UserId = pUserid, userType = 1 });

                    }
                }
                foreach (string pUserid in model.participateUser)
                {
                    if (pUserid.Equals(userid))
                    {
                        popApp.createTaskPop(new TaskPopEntity() { id = System.Guid.NewGuid().ToString(), status = 2, taskId = taskId, UserId = pUserid, userType = 2 });
                    }
                    else
                    {
                        popApp.createTaskPop(new TaskPopEntity() { id = System.Guid.NewGuid().ToString(), status = 1, taskId = taskId, UserId = pUserid, userType = 2 });

                    }
                }
                return model;
            }
            catch (Exception ex)
            {
                var resp = new HttpResponseMessage(HttpStatusCode.InternalServerError)
                {
                    Content = new StringContent(ex.ToString()),
                    ReasonPhrase = "error"
                };
                throw new HttpResponseException(resp);

            }



            //return Ok();
        }


    }
}