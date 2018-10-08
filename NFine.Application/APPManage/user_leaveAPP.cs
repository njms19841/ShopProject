using Newtonsoft.Json.Linq;
using NFine.Application.TaskManage;
using NFine.Code;
using NFine.Domain._02_ViewModel;
using NFine.Domain._03_Entity.APPManage;
using NFine.Domain._03_Entity.TaskManage;
using NFine.Domain._04_IRepository.APPManage;
using NFine.Domain._04_IRepository.TaskManage;
using NFine.Repository.APPManage;
using NFine.Repository.TaskManage;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UMengPush.Net.Core;

namespace NFine.Application.APPManage
{
    public class user_leaveAPP
    {
        private Iuser_leaveRepository service = new user_leaveRepository();
        ITaskMastRepository taskRep = new TaskMastRepository();
        public List<user_leaveEntity> getLeaveData(string UserId)
        {
           return service.IQueryable().Where(p => p.userid.Equals(UserId)).OrderByDescending(p=>p.StartDateTime).Take(30).ToList(); 
        }
        public void AllowLeave(string id, int state,string userId)
        {
            var ent = service.FindEntity(id);
            ent.state = state;
            ent.auser = userId;
            ent.atime = System.DateTime.Now;
            service.Update(ent);
        }
        public void AllowLeave(string id, int state, string userId,string desc)
        {
            var ent = service.FindEntity(id);
            ent.state = state;
            ent.auser = userId;
            ent.atime = System.DateTime.Now;
            ent.adesc = desc;
            service.Update(ent);
        }
        public user_leaveEntity getLeaveDataById(string id)
        {
            return service.FindEntity(id);
        }
        public List<user_leaveEntity> getLeaveByDate(string UserId, string startDate, string endDate)
        {
            
            DateTime startTime = DateTime.ParseExact(startDate + " 00:00", "yyyy-MM-dd HH:mm", CultureInfo.CurrentCulture);
            DateTime startTime2 = DateTime.ParseExact(startDate + " 23:59", "yyyy-MM-dd HH:mm", CultureInfo.CurrentCulture);
            DateTime endTime = DateTime.ParseExact(endDate + " 23:59", "yyyy-MM-dd HH:mm", CultureInfo.CurrentCulture);
            DateTime endTime2 = DateTime.ParseExact(endDate + " 00:00", "yyyy-MM-dd HH:mm", CultureInfo.CurrentCulture);

            return service.IQueryable().Where(p => p.userid.Equals(UserId) && p.state == 2 &&
            ((p.StartDateTime >= startTime && p.StartDateTime <= endTime) ||
            (p.EndDateTime <= startTime && p.EndDateTime <= endTime) ||
            (p.StartDateTime <= startTime2 && p.EndDateTime >= startTime)||
            (p.StartDateTime<=endTime && p.EndDateTime>= endTime2))).ToList();

          // return service.FindList("select * from user_leave where userid='"+ UserId + "' and state=2 and ((StartDateTime>='" + startDate + " 00:00:00' and  StartDateTime <='" + endDate + " 23:59:59') or (EndDateTime>='" + startDate + " 00:00:00' and  EndDateTime <='" + endDate + " 23:59:59')or (StartDateTime<='" + startDate + " 23:59:59' and  EndDateTime >='" + startDate + " 00:00:00') or (StartDateTime<='" + endDate + " 23:59:59' and  EndDateTime >='" + endDate + " 00:00:00')) ").ToList();
        }
        public List<user_leaveEntity> getLeaveByDate2(string UserId, string startDate, string endDate)
        {
            return service.FindList("select * from user_leave where userid='" + UserId + "' and (state=1 or state=2 or state is null)  and ((StartDateTime>='" + startDate + " 00:00:00' and  StartDateTime <='" + endDate + " 23:59:59') or (EndDateTime>='" + startDate + " 00:00:00' and  EndDateTime <='" + endDate + " 23:59:59') or (StartDateTime<='" + startDate + " 23:59:59' and  EndDateTime >='" + startDate + " 00:00:00') or (StartDateTime<='" + endDate + " 23:59:59' and  EndDateTime >='" + endDate + " 00:00:00')) ").ToList();
        }
        public List<user_leaveEntity> getLeaveByDate3(string UserId, string startDate, string endDate)
        {
            return service.FindList("select * from user_leave where userid='" + UserId + "' and (state=1 or state=2 or state=3 or state is null)  and ((StartDateTime>='" + startDate + " 00:00:00' and  StartDateTime <='" + endDate + " 23:59:59') or (EndDateTime>='" + startDate + " 00:00:00' and  EndDateTime <='" + endDate + " 23:59:59') or (StartDateTime<='" + startDate + " 23:59:59' and  EndDateTime >='" + startDate + " 00:00:00') or (StartDateTime<='" + endDate + " 23:59:59' and  EndDateTime >='" + endDate + " 00:00:00')) ").ToList();
        }
        public List<user_leaveEntity> getAllLeaveByDate(string UserId, string startDate, string endDate)
        {
            return service.FindList("select * from user_leave where auser='" + UserId + "' and (state=1 or state=2 or state=3 or state is null)  and ((StartDateTime>='" + startDate + " 00:00:00' and  StartDateTime <='" + endDate + " 23:59:59') or (EndDateTime>='" + startDate + " 00:00:00' and  EndDateTime <='" + endDate + " 23:59:59') or (StartDateTime<='" + startDate + " 23:59:59' and  EndDateTime >='" + startDate + " 00:00:00') or (StartDateTime<='" + endDate + " 23:59:59' and  EndDateTime >='" + endDate + " 00:00:00')) ").ToList();
        }
        private string getDeviceTokens(string userId, string type)
        {
            IsysUserPushDeviceRepository deviceRep = new sysUserPushDeviceRepository();
            var devices = deviceRep.IQueryable().Where(p => p.userId.Equals(userId) && p.deviceType.Equals(type));
            string strDevices = "";
            foreach (var device in devices)
            {
                if (strDevices.Equals(""))
                {
                    strDevices = device.pushToken;
                }
                else
                {
                    strDevices = strDevices + "," + device.pushToken;
                }

            }
            return strDevices;

        }

        public salesActualChangeRes leave(user_leaveEntity entity)
        {
            if (getLeaveByDate2(entity.userid, entity.StartDateTime.Value.ToString("yyyy-MM-dd"), entity.EndDateTime.Value.ToString("yyyy-MM-dd")).Count>0)
            {
                return new salesActualChangeRes() { errorCode = "101", errorMessage = "请假失败，日期范围内请假记录已经存在！", isOk = false };
            }
           var res= service.leave(entity);
            string auser = null;
            if (res.isOk)
            {
                var shopApp = new marketShopApp();
                var shops=shopApp.getShopByUserId(entity.userid);
                foreach(var shop in shops)
                {
                   
                   
                    //发送签核消息
                    var salesapp = new marketSalesApp();
                    var user = salesapp.getAdminExecutiveById(shop.ORG_ID);
                   var userinfo = salesapp.GetUserInfo(entity.userid);



                    var ApUsers = salesapp.getPOrgUserInfo(userinfo.SalesNo);
                    
                        var ApUserinfo = salesapp.getUserInfoBySalesNo(user.EMPLOYEE_CODE);

                    auser = ApUserinfo.id;

                        string taskId = System.Guid.NewGuid().ToString();
                        TaskMastEntity ent = new TaskMastEntity()
                        {
                            id = taskId,
                            alertType = 1,
                            createdTime = System.DateTime.Now,
                            createdUserId = ApUserinfo.id,
                            MESSAGE_BILL_NO = entity.id,
                            MESSAGE_SUB_NO = 0,
                            MESSAGE_REPLY_TYPE_CODE = "001",
                            MESSAGE_REPLY_TYPE_NAME = "不需回复",
                            RECEIVE_EMPLOYEE_CODE = ApUserinfo.SalesNo,
                            RECEIVE_EMPLOYEE_NAME = ApUserinfo.Name,
                            desc = userinfo.Name + "的请假签核",
                            freqType = 1,
                            taskName = userinfo.Name + "的请假签核",
                            starTime = System.DateTime.Now,
                            endTime = System.DateTime.Now.AddMinutes(5),
                            importantType = 1,
                            isAll = 1,
                            isRead = 0,
                            isReply = 0,

                            taskType = "001",
                            taskTypeName = "一般通知",
                            URGENCY_TYPE_CODE = "002",
                            URGENCY_TYPE_NAME = "紧急",
                            taskSource = "系统",
                            isDelete = 0,
                            taskUrl = "https://iretailerapp.flnet.com/QJ/QJAllowIndex?id=" + entity.id + "&userId=" + ApUserinfo.id
                        };
                        taskRep.Insert(ent);
                        TaskPopApp popApp = new TaskPopApp();
                        popApp.createTaskPop(new TaskPopEntity() { id = System.Guid.NewGuid().ToString(), status = 2, taskId = taskId, UserId = ApUserinfo.id, userType = 1 });
                        string AndroIdDevice = getDeviceTokens(ApUserinfo.id, "Android");
                        if (AndroIdDevice.Length > 0)
                        {
                            AndroidPostJson postJson = new AndroidPostJson();
                            var payload = new AndroidPayload();
                            postJson.type = CastType.listcast;
                            postJson.device_tokens = AndroIdDevice;

                            payload.display_type = "notification";
                            payload.body = new ContentBody();
                            payload.body.ticker = ent.taskName;
                            payload.body.title = ent.taskName;
                            payload.body.icon = "appicon";
                            payload.body.play_lights = "true";
                            payload.body.play_sound = "true";
                            payload.body.play_vibrate = "true";
                            payload.body.text = ent.taskName;
                            payload.body.after_open = AfterOpenAction.go_app;
                            //payload.body.custom = "comment-notify";
                            var dic = new Dictionary<string, string>();
                            dic.Add("messageId", System.Guid.NewGuid().ToString());
                            payload.extra = dic;
                            postJson.payload = payload;
                            postJson.description = ent.taskName;
                            UMengMessagePush<AndroidPostJson> uMAndroidPush = new UMengMessagePush<AndroidPostJson>("59550725677baa17ce0003fe", "grpqx0ayqc1ovn45iqczlrovqrdtvujf");
                            ReturnJsonClass resu = uMAndroidPush.SendMessage(postJson);
                            System.Console.WriteLine(resu.ret);
                        }
                        string IOSDevice = getDeviceTokens(ApUserinfo.id, "IOS");
                        if (IOSDevice.Length > 0)
                        {

                            IOSPostJson postJson = new IOSPostJson();
                            postJson.type = CastType.unicast;
                            var aps = new Aps()
                            {
                                alert = "msg",
                                sound = "default"
                            };
                            var payload = new IOSPayload(aps);
                            JObject jo = JObject.FromObject(payload);
                            var extra = new Dictionary<string, string>();
                            //用户自定义内容，"d","p"为友盟保留字段，key不可以是"d","p"
                            extra.Add("open", "list");
                            extra.ToList().ForEach(x => jo.Add(x.Key, x.Value));

                            postJson.payload = jo;
                            postJson.description = ent.taskName;
                            postJson.device_tokens = IOSDevice;
                            postJson.production_mode = "true";

                        UMengMessagePush<IOSPostJson> uMAndroidPush = new UMengMessagePush<IOSPostJson>("596791cbb27b0a673700001f", "siy2v7u9uzishzimgnslzdukyqkeofhp");
                        ReturnJsonClass resu = uMAndroidPush.SendMessage(postJson);
                            System.Console.WriteLine(resu.ret);
                        }
                    
                

                }
                //var ApUserinfo = salesapp.GetUserInfo(entity.userid);
               
            }
           
            var ent2 = service.FindEntity(entity.id);
            ent2.auser = auser;
            service.Update(ent2);
            return res;
            

            //DateTime startTime = System.Convert.ToDateTime(entity.day.Value.ToString("yyyy-MM-dd 00:00:00"));
            //DateTime EndTime = System.Convert.ToDateTime(entity.day.Value.ToString("yyyy-MM-dd 23:59:59"));

            //entity.id = Common.GuId();
            //service.Insert(entity);
            //if (entity.day_type.Equals("1"))
            //{
            //    if (service.FindEntity(p => p.day >= startTime && p.day <= EndTime && (p.day_type.Equals("1") || p.day_type.Equals("2") || p.day_type.Equals("3"))) == null)
            //    {
            //        entity.id = Common.GuId();
            //        service.Insert(entity);
            //    }
            //    else
            //    {
            //        throw new Exception("请假记录已存在！");
            //    }
            //}
            //else if (entity.day_type.Equals("2"))
            //{
            //    if (service.FindEntity(p => p.day >= startTime && p.day <= EndTime && (p.day_type.Equals("1") || p.day_type.Equals("2"))) == null)
            //    {
            //        entity.id = Common.GuId();
            //        service.Insert(entity);
            //    }
            //    else
            //    {
            //        throw new Exception("请假记录已存在！");
            //    }
            //}
            //else if (entity.day_type.Equals("3"))
            //{
            //    if (service.FindEntity(p => p.day >= startTime && p.day <= EndTime && (p.day_type.Equals("1") || p.day_type.Equals("3"))) == null)
            //    {
            //        entity.id = Common.GuId();
            //        service.Insert(entity);
            //    }
            //    else
            //    {
            //        throw new Exception("请假记录已存在！");
            //    }
            //}
            //else
            //{
            //    throw new Exception("时间范围错误！");
            //}


        }
    }
}
