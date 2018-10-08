using Market.APIService.Models;
using Microsoft.AspNet.Identity;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using NFine.Application.APPManage;
using NFine.Domain._02_ViewModel;
using NFine.Domain._03_Entity.APPManage;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;


namespace Market.APIService.Controllers
{
    [Authorize]
    [RoutePrefix("api/KaoQin")]
    public class KaoQinController : ApiController
    {


        /// <summary>
        /// 打卡方法
        /// </summary>
        /// <param name="model">打开类型</param>
        /// <returns></returns>
        [Route("clock")]
        public async Task<IHttpActionResult> SetClock(KaoQinModel model)
        {
            return BadRequest("该版本已停用，请使用新版！");
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            userKaoqinAPP app = new userKaoqinAPP();
            userKaoqinEntity ent = new userKaoqinEntity {
                 checkTime=System.DateTime.Now, kaoqin_Type=model.KaoQinType, userId= User.Identity.GetUserId(),
                LATITUDE = model.LATITUDE,
                LONGITUDE = model.LONGITUDE
            };
            app.Submit(ent);
            return Ok();
        }
        [Route("clockV2")]
        
        public async Task<IHttpActionResult> SetClockV2(KaoQinModel model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            string adder = "";
            try
            {
                if (model.LONGITUDE > 0)
                {
                    var client = new RestClient("https://restapi.amap.com");
                    var request = new RestRequest("/v3/geocode/regeo?location=" + model.LONGITUDE + "," + model.LATITUDE + "&key=2daf078d66ac9f5a8aa661906a618710&radius=1000&extensions=base&output=JSON ", Method.GET);
                    IRestResponse response3 = client.Execute(request);

                    if (response3.StatusCode == System.Net.HttpStatusCode.OK)
                    {
                        JObject obj = JObject.Parse(response3.Content);
                        adder = ((JObject)obj.GetValue("regeocode")).GetValue("formatted_address").ToString();

                    }
                }
            }
            catch { }

            userKaoqinAPP app = new userKaoqinAPP();
            userKaoqinEntity ent = new userKaoqinEntity
            {
                checkTime = System.DateTime.Now,
                kaoqin_Type = model.KaoQinType,
                userId = User.Identity.GetUserId(),
                LATITUDE = model.LATITUDE,
                LONGITUDE = model.LONGITUDE,
                 file_id=model.file_id,
                  adder= adder
            };

            salesActualChangeRes res =  app.Submit2(ent);
            if (!res.isOk)
            {
                return BadRequest(res.errorMessage);
            }
            return Ok();
        }
        [Route("GetClock")]
        public List<KaoQinModel> GetClock()
        {
            userKaoqinAPP app = new userKaoqinAPP();
            List < userKaoqinEntity > ents = app.GetClock(User.Identity.GetUserId());
            List<KaoQinModel> models = new List<KaoQinModel>();
            foreach (userKaoqinEntity ent in ents)
            {
                models.Add(new KaoQinModel() { clockTime = ent.checkTime.Value, KaoQinType = ent.kaoqin_Type, LATITUDE = ent.LATITUDE.Value, LONGITUDE = ent.LONGITUDE.Value });
            }
            return models;

        }

        ///// <summary>
        ///// 打卡方法
        ///// </summary>
        ///// <param name="model">打开类型</param>
        ///// <returns></returns>
        //[Route("clockV2")]
        //public async Task<IHttpActionResult> SetClockV2(KaoQinModel model)
        //{
        //    if (!ModelState.IsValid)
        //    {
        //        return BadRequest(ModelState);
        //    }
        //    userKaoqinAPP app = new userKaoqinAPP();
        //    userKaoqinEntity ent = new userKaoqinEntity
        //    {
        //        checkTime = System.DateTime.Now,
        //        kaoqin_Type = model.KaoQinType,
               
        //    };
        //    app.Submit(ent);
        //    return Ok();
        //}

        /// <summary>
        /// 请假方法
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [Route("leave")]
        public async Task<IHttpActionResult> SetLeave(LeaveModel model)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }
                user_leaveAPP app = new user_leaveAPP();
                user_leaveEntity ent = new user_leaveEntity
                {
                    id = System.Guid.NewGuid().ToString(),
                    day = model.Day,
                    day_type = model.DayType,
                    desc = model.Desc,
                    leave_type = model.LeaveType,
                    StartDateTime = model.StartDateTime,
                    EndDateTime = model.EndDateTime,
                    userid = User.Identity.GetUserId()
                };
                salesActualChangeRes res = app.leave(ent);
                if (!res.isOk)
                {
                    return BadRequest(res.errorMessage);
                }
                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
           
        }
        

    }
}