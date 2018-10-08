
using Newtonsoft.Json.Linq;
using NFine.Application.APPManage;
using NFine.Application.SystemSecurity;
using NFine.Domain._02_ViewModel;
using NFine.Domain._03_Entity.APPManage;
using NFine.Domain._03_Entity.TaskManage;
using NFine.Domain._04_IRepository.APPManage;
using NFine.Domain._04_IRepository.TaskManage;
using NFine.Domain.Entity.SystemSecurity;
using NFine.Repository.APPManage;
using NFine.Repository.TaskManage;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UMengPush.Net.Core;

namespace NFine.Application.TaskManage
{
    public class TaskMemberApp
    {
        ITaskMemberDataRepository rep = new TaskMemberDataRepository();
        ITaskMastRepository taskRep = new TaskMastRepository();
        public List<TaskMemberDataEntity> getData(string bid, string shopCode)
        {
            return rep.IQueryable().Where(p => p.beachId.Equals(bid) && p.shopCode.Equals(shopCode)).ToList();
        }
        public List<TaskMemberDataEntity> getData(string shopCode)
        {
            return rep.IQueryable().Where(p => p.shopCode.Equals(shopCode)).OrderByDescending(p=>p.InTime).Take(20).ToList();
        }
        public List<TaskMemberDataEntity> getData(DateTime startTime, DateTime endTime,  string shopCode)
        {
            return rep.IQueryable().Where(p => p.InTime>= startTime && p.InTime<= endTime && p.shopCode.Equals(shopCode)).OrderByDescending(p=>p.InTime).ToList();
        }
        public int getReadCount(DateTime startTime, DateTime endTime, string shopCode)
        {
          var rest=  rep.IQueryable().Where(p => p.InTime >= startTime && p.InTime <= endTime && p.shopCode.Equals(shopCode) && p.IsRead==1).GroupBy(g => new 
            {
                key = g.mfMemberId,
                
            });
            return rest.Count();
        }
        public int getReadCount(DateTime startTime, DateTime endTime, string shopCode,string userId)
        {
            var rest = rep.IQueryable().Where(p => p.InTime >= startTime && p.InTime <= endTime && p.shopCode.Equals(shopCode) && p.IsRead == 1 && p.readUserId.Equals(userId)).GroupBy(g => new
            {
                key = g.mfMemberId,

            });
            return rest.Count();
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
        public void readPushMessage(string mfid, string bid,string userId)
        {
            var list = rep.IQueryable().Where(p => p.beachId.Equals(bid) && p.mfMemberId.Equals(mfid)).ToList();
            foreach (var ent in list)
            {
                ent.IsRead = 1;
                ent.readUserId = userId;
                rep.Update(ent);
            }
        }
       
        public void insertTaskMember(List<TaskMemberDataEntity> ents,string bid)
        {
            LogApp logApp = new LogApp();
            foreach (var ent in ents)
            {
                rep.Insert(ent);
            }
            
            /*logApp.WriteDbLog(new LogEntity()
            {
                F_Account = "",
                F_CreatorTime = System.DateTime.Now,
                F_Date = System.DateTime.Now,
                F_CreatorUserId = "",
                F_Description = "StartSend",
                F_Id = System.Guid.NewGuid().ToString(),
                F_ModuleId = "SendMessage",
                F_ModuleName = "SendMessage",


            });
            */
            try
            {

                var shops = ents.GroupBy(p => p.shopCode);
                foreach (var shop in shops)
                {
                    string ShopCode = shop.Key;


                    var salesApp = new marketSalesApp();
                    var users = salesApp.getGuidManByShop(ShopCode);
                    foreach (var user in users)
                    {
                        var userinfo = salesApp.getUserInfoBySalesNo(user.sales_No);
                        string taskId = System.Guid.NewGuid().ToString();
                        TaskMastEntity ent = new TaskMastEntity()
                        {
                            id = taskId,
                            alertType = 1,
                            createdTime = System.DateTime.Now,
                            createdUserId = userinfo.id,
                            MESSAGE_BILL_NO = bid,
                            MESSAGE_SUB_NO = 0,
                            MESSAGE_REPLY_TYPE_CODE = "001",
                            MESSAGE_REPLY_TYPE_NAME = "不需回复",
                            RECEIVE_EMPLOYEE_CODE = user.sales_No,
                            RECEIVE_EMPLOYEE_NAME = user.sales_Name,
                            desc = "有会员进店，请接待",
                            freqType = 1,
                            taskName = "有会员进店，请接待",
                            starTime = System.DateTime.Now,
                            endTime = System.DateTime.Now.AddMinutes(5),
                            importantType = 1,
                            isAll = 1,
                            isRead = 0,
                            isReply = 0,

                            taskType = "009",
                            taskTypeName = "会员到店通知",
                            URGENCY_TYPE_CODE = "002",
                            URGENCY_TYPE_NAME = "紧急",
                            taskSource = "系统",
                            isDelete = 0,
                            taskUrl = "{'shopCode':'" + ShopCode + "','bid':'" + bid + "'}"
                        };
                        taskRep.Insert(ent);

                        TaskPopApp popApp = new TaskPopApp();
                        popApp.createTaskPop(new TaskPopEntity() { id = System.Guid.NewGuid().ToString(), status = 2, taskId = taskId, UserId = userinfo.id, userType = 1 });


                        string AndroIdDevice = getDeviceTokens(userinfo.id, "Android");
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
                            foreach (var memberEnt in ents)
                            {
                                postJson = new AndroidPostJson();
                                payload = new AndroidPayload();
                                postJson.type = CastType.unicast;
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
                                dic = new Dictionary<string, string>();
                                dic.Add("messageId", System.Guid.NewGuid().ToString());
                                dic.Add("mfMemberId", memberEnt.mfMemberId);
                                dic.Add("bid", bid);

                                payload.extra = dic;
                                postJson.payload = payload;
                                postJson.policy = new AndroidPolicy();
                                postJson.policy.expire_time = DateTime.Now.AddMinutes(5).ToString("yyyy-MM-dd HH:mm:ss");
                                UMengMessagePush<AndroidPostJson> uMAndroidPush2 = new UMengMessagePush<AndroidPostJson>("5b3ae2eaf43e4808f6000112", "gljoyn3tebkcchaeyvksvp0itjagynqr");
                                ReturnJsonClass resu2 = uMAndroidPush2.SendMessage(postJson);
                               
                                /*logApp.WriteDbLog(new LogEntity()
                                {
                                    F_Account = user.sales_No,
                                    F_CreatorTime = System.DateTime.Now,
                                    F_Date = System.DateTime.Now,
                                    F_CreatorUserId = user.sales_No,
                                    F_Description = resu2.ret,
                                    F_Id = System.Guid.NewGuid().ToString(),
                                    F_ModuleId = "SendMessage",
                                    F_ModuleName = "SendMessage",

                                });*/


                            }

                            System.Console.WriteLine(resu.ret);
                        }
                        string IOSDevice = getDeviceTokens(userinfo.id, "IOS");
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

                }
            }
            catch (Exception ex)
            {
                logApp.WriteDbLog(new LogEntity()
                {
                    F_Account = "",
                    F_CreatorTime = System.DateTime.Now,
                    F_Date = System.DateTime.Now,
                    F_CreatorUserId = "",
                    F_Description = ex.Message,
                    F_Id = System.Guid.NewGuid().ToString(),
                    F_ModuleId = "SendMessage",
                    F_ModuleName = "SendMessage",

                });
            }

        }

    }
}
