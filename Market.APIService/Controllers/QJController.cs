using DataSynchronizationLib.SCMTableAdapters;
using DataSynchronizationStanbyLib;
using Market.APIService.Models;
using Newtonsoft.Json.Linq;
using NFine.Application.APPManage;
using NFine.Application.TaskManage;
using NFine.Domain._02_ViewModel;
using NFine.Domain._03_Entity.APPManage;
using NFine.Domain._04_IRepository.TaskManage;
using NFine.Repository.TaskManage;
using RestSharp;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace Market.APIService.Controllers
{
    public class QJCreatedModel
    {
        public string userId { get; set; }
    }
    public class QJQueryModel
    {
        public string userId { get; set; }
        public List<QJDataModel> data { get; set; }
    }
    public class KaoQinQueryModel
    {
        public string userName { get; set; }
        public string OrgName { get; set; }
        public string MonthQJDays { get; set; }
        public string YearQJDays { get; set; }
        public string YearQJDays_1 { get; set; }
        public string YearQJDays_2 { get; set; }
        public string YearQJDays_3 { get; set; }
        public string YearQJDays_4 { get; set; }
        public string YearQJDays_5 { get; set; }
        public string YearQJDays_6 { get; set; }
        public string YearQJDays_7 { get; set; }
        public string YearQJDays_8 { get; set; }
        public  List<QJDataModel> LeveData { get; set; }
        public List<ClockDataModel> ClockData { get; set; }
        public List<KaoQinDataModel> data { get; set; }
    }
    public class PaibanModel
    {
       
        public string empCode { get; set; }
        public string empName { get; set; }
        public string type1 { get; set; }
        public string type2 { get; set; }
        public string type3 { get; set; }
        public string type4 { get; set; }
        public string type5{ get; set; }
        public string type6 { get; set; }
        public string type7 { get; set; }
        public string todayType { get; set; }
        public int sumTypeA { get; set; }
        public int sumTypeB { get; set; }
        public int sumTypeC { get; set; }
        public int sumTypeD { get; set; }
        public int nextSumTypeA { get; set; }
        public int nextSumTypeB { get; set; }
        public int nextSumTypeC { get; set; }
        public int nextSumTypeD { get; set; }

    }
    public class KaoQinDataModel
    {
        public string day { get; set; }
        public string workIn { get; set; }
        public string workOut { get; set; }
        public string leave { get; set; }
        public string state { get; set; }
        public string tempStr { get; set; }
    }
    public class ClockDataModel
    {
        public string type2 { get; set; }
        public string datetime { get; set; }
        public string type { get; set; }
        
    }
    public class clockTime
    {
        public string inTime { get; set; }
        public string outTime { get; set; }
    }
    public class clockRest
    {
        public bool isOk { get; set; }
        public string message { get; set; }
    }
    public class popModel
    {
        public string userId { get; set; }
        public string salesNo { get; set; }
        public string name { get; set; }
        public string orgName { get; set; }
    }
    public class QJDataModel
    {
        public string day { get; set; }
        public string state { get; set; }
        public string leaveType { get; set; }
    }
    public class MemberLeveDataModel
    {
        public string id { get; set; }
        public string startDay { get; set; }
        public string endDay { get; set; }
        public string state { get; set; }
        public string leaveType { get; set; }
       
        public string leaveDays { get; set; }
        public string desc { get; set; }
        public string adesc { get; set; }
        public string userName { get; set; }
        public string userId { get; set; }
        
        public List<imageModel> imgs { get; set; }
    }
    public class PaibanSelectOptionModel
    {
        public string code { get; set; }
        public string name { get; set; }
    }
    public class QJAllowModel
    {
        public string id { get; set; }
        public string startDay { get; set; }
        public string endDay { get; set; }
        public string state { get; set; }
        public string leaveType { get; set; }
        public string name { get; set; }
        public string workNumber { get; set; }
        public string desc { get; set; }
    }
    public class QJController : Controller
    {

        public ActionResult QJAllowIndex(string id,string userId)
        {
            user_leaveAPP app = new user_leaveAPP();
            var ent= app.getLeaveDataById(id);
            marketSalesApp salesApp = new marketSalesApp();
            var userInfo = salesApp.GetUserInfo(ent.userid);
            QJAllowModel model = new QJAllowModel() {
                id = id, desc = ent.desc, startDay = ent.StartDateTime.Value.ToString("yyyy-MM-dd"),
                endDay = ent.EndDateTime.Value.ToString("yyyy-MM-dd"), leaveType = ent.leave_type.ToString(),
                state = ent.state.HasValue ? ent.state.Value.ToString() : "1", name= userInfo.Name, workNumber= userInfo.SalesNo
            };
            ViewData["model"] = model;
            ViewData["userId"] = userId;
            return View("QJAllowView");
        }
        
        // GET: PSI
        public ActionResult KaoQingIndex(string userId)
        {
            PModel pModel = new PModel();
            pModel.Month = new List<KeyValueModel>();
            DateTime now = DateTime.Now;
            pModel.Month.Add(new KeyValueModel() { key = now.AddMonths(-2).ToString("yyyyMM"), keyValue = now.AddMonths(-2).ToString("yyyy/MM"), isSelected = false });
            pModel.Month.Add(new KeyValueModel() { key = now.AddMonths(-1).ToString("yyyyMM"), keyValue = now.AddMonths(-1).ToString("yyyy/MM"), isSelected = false });
            pModel.Month.Add(new KeyValueModel() { key = now.ToString("yyyyMM"), keyValue = now.ToString("yyyy/MM"), isSelected = true });
            pModel.UserId = userId;
            
            ViewData["PModel"] = pModel;
            List<popModel> popModels = new List<popModel>();
            marketSalesApp salesApp = new marketSalesApp();
            var userInfo = salesApp.GetUserInfo(userId);
            marketShopApp shopApp = new marketShopApp();
            popModels.Add(new popModel() {
                 name= userInfo.Name+"("+ userInfo .SalesNo+ ")",
                  userId= userInfo.id,
                   orgName= shopApp.getShopByUserId(userInfo.id).First().SHOP_NAME, salesNo=userInfo.SalesNo
            });
            
            //
            var userList= salesApp.getShopUserInfo(userInfo.SalesNo);
            
            foreach (var user in userList)
            {
                
                if (popModels.Find(p => p.salesNo.Equals(user.sales_No))==null)
                {
                    string s_name = user.sales_Name + "(" + user.sales_No + ")";
                    var tempuserInfo = salesApp.getUserInfoBySalesNo(user.sales_No);
                    if (tempuserInfo != null)
                    {
                        string s_userId = salesApp.getUserInfoBySalesNo(user.sales_No).id;
                        string s_orgName = shopApp.getShopName(user.sales_ShopNo);
                        popModels.Add(new popModel()
                        {
                            name = s_name,
                            userId = s_userId,
                            orgName = s_orgName,
                            salesNo = user.sales_No
                        });
                    }
                }
            }
            ViewData["VModel"] = popModels;
            return View("KaoQinView");
        }
        public PartialViewResult KaoQinQueryView(string userId,string month)
        {
            return PartialView("_KaoQinPartialPage", getKaoQinData(userId, month));
        }
        public PartialViewResult KaoQinPopView(string userId)
        {
            List<popModel> popModels = new List<popModel>();
            marketSalesApp salesApp = new marketSalesApp();
            var userInfo = salesApp.GetUserInfo(userId);
            marketShopApp shopApp = new marketShopApp();
            popModels.Add(new popModel()
            {
                name = userInfo.Name + "(" + userInfo.SalesNo + ")",
                userId = userInfo.id,
                orgName = shopApp.getShopByUserId(userInfo.id).First().SHOP_NAME,
                  salesNo = userInfo.SalesNo
            });

            var userList = salesApp.getShopUserInfo(userInfo.SalesNo);
           
            foreach (var user in userList)
            {

                if (popModels.Find(p => p.salesNo.Equals(user.sales_No)) == null)
                {
                    string s_name = user.sales_Name + "(" + user.sales_No + ")";
                    var tempuserInfo = salesApp.getUserInfoBySalesNo(user.sales_No);
                    if (tempuserInfo != null)
                    {
                        string s_userId = salesApp.getUserInfoBySalesNo(user.sales_No).id;
                        string s_orgName = shopApp.getShopName(user.sales_ShopNo);
                        popModels.Add(new popModel()
                        {
                            name = s_name,
                            userId = s_userId,
                            orgName = s_orgName,
                            salesNo = user.sales_No
                        });
                    }
                }
            }

            return PartialView("_KaoQinPopPartialPage", popModels);
        }
        

        private KaoQinQueryModel getKaoQinData(string userId,string month)
        {
            DateTime monthDay = DateTime.ParseExact(month, "yyyyMM", CultureInfo.CurrentCulture);
            DateTime startDay_Month = monthDay;
            DateTime endDay_Month = monthDay.AddMonths(1).AddDays(-1);
            DateTime yearDay = DateTime.ParseExact(month.Substring(0, 4), "yyyy", CultureInfo.CurrentCulture);
            DateTime startDay_Year = yearDay;
            DateTime endDay_Year = yearDay.AddYears(1).AddDays(-1);

           
            
            marketSalesApp salesApp = new marketSalesApp();
            var userInfo = salesApp.GetUserInfo(userId);
            marketShopApp shopApp = new marketShopApp();
            List<marketSalesShopEntity> shops = shopApp.getShopByUserId(userId);

            KaoQinQueryModel VModel = new KaoQinQueryModel();
            if (shops.Count > 0)
            {
                VModel.OrgName = shops[0].SHOP_NAME;
            }
            else
            {
                VModel.OrgName = salesApp.getOrgName(userInfo.SalesNo);
            }
            VModel.userName = userInfo.Name + "(" + userInfo.SalesNo + ")";
            user_leaveAPP leaveApp = new user_leaveAPP();


            var monthList = leaveApp.getLeaveByDate(userId, startDay_Month.ToString("yyyy-MM-dd"), endDay_Month.ToString("yyyy-MM-dd"));
            
            var yearList = leaveApp.getLeaveByDate(userId, startDay_Year.ToString("yyyy-MM-dd"), endDay_Year.ToString("yyyy-MM-dd"));

            VModel.MonthQJDays = getdays(startDay_Month, endDay_Month, monthList).ToString() + "天";

           // monthList = leaveApp.getLeaveByDate(userId, startDay_Month.ToString("yyyy-MM-dd"), endDay_Month.ToString("yyyy-MM-dd"));

            VModel.YearQJDays = getdays(startDay_Year, endDay_Year, yearList).ToString() + "天";
            VModel.YearQJDays_1 = getdays(startDay_Year, endDay_Year, yearList.Where(p=>p.leave_type.Equals("1")).ToList()).ToString() + "天";
            VModel.YearQJDays_2 = getdays(startDay_Year, endDay_Year, yearList.Where(p => p.leave_type.Equals("2")).ToList()).ToString() + "天";
            VModel.YearQJDays_3 = getdays(startDay_Year, endDay_Year, yearList.Where(p => p.leave_type.Equals("3")).ToList()).ToString() + "天";
            VModel.YearQJDays_4 = getdays(startDay_Year, endDay_Year, yearList.Where(p => p.leave_type.Equals("4")).ToList()).ToString() + "天";
            VModel.YearQJDays_5 = getdays(startDay_Year, endDay_Year, yearList.Where(p => p.leave_type.Equals("5")).ToList()).ToString() + "天";
            VModel.YearQJDays_6 = getdays(startDay_Year, endDay_Year, yearList.Where(p => p.leave_type.Equals("6")).ToList()).ToString() + "天";
            VModel.YearQJDays_7 = getdays(startDay_Year, endDay_Year, yearList.Where(p => p.leave_type.Equals("7")).ToList()).ToString() + "天";
            VModel.YearQJDays_8 = getdays(startDay_Year, endDay_Year, yearList.Where(p => p.leave_type.Equals("8")).ToList()).ToString() + "天";

            VModel.LeveData = new List<QJDataModel>();
            VModel.ClockData = new List<ClockDataModel>();
            foreach (var ent in monthList)
            {
                VModel.LeveData.Add(new QJDataModel()
                {
                    day = ent.StartDateTime.Value.ToString("MM/dd-") + ent.EndDateTime.Value.ToString("MM/dd")
                     ,
                    leaveType = ent.leave_type.ToString(),
                    state = ent.state.HasValue ? ent.state.Value.ToString() : "1"
                });
            }

            VModel.data = new List<KaoQinDataModel>();
            userKaoqinAPP KqoqinApp = new userKaoqinAPP();
            paibanApp pApp = new paibanApp();
            var KaoqinList= KqoqinApp.GetClock(userId, startDay_Month.ToString("yyyy-MM-dd"), endDay_Month.ToString("yyyy-MM-dd"));
            var paiBanList = pApp.getPaibanEnts(userInfo.SalesNo, startDay_Month.ToString("yyyy-MM-dd"), endDay_Month.ToString("yyyy-MM-dd"));
            foreach (var row in KaoqinList)
            {
                if (row.kaoqin_Type.Equals("1")||row.kaoqin_Type.Equals("9"))
                {
                    VModel.ClockData.Add(new ClockDataModel { datetime = row.checkTime.Value.ToString("yyyy-MM-dd HH:mm:ss"), type = "上/下班", type2 = "签到" });
                }
                else if (row.kaoqin_Type.Equals("2")|| row.kaoqin_Type.Equals("10"))
                {
                    VModel.ClockData.Add(new ClockDataModel { datetime = row.checkTime.Value.ToString("yyyy-MM-dd HH:mm:ss"), type = "上/下班", type2 = "签退" });
                }
                else if (row.kaoqin_Type.Equals("5"))
                {
                    VModel.ClockData.Add(new ClockDataModel { datetime = row.checkTime.Value.ToString("yyyy-MM-dd HH:mm:ss"), type = "会议", type2 = "签到" });
                }
                else if (row.kaoqin_Type.Equals("6"))
                {
                    VModel.ClockData.Add(new ClockDataModel { datetime = row.checkTime.Value.ToString("yyyy-MM-dd HH:mm:ss"), type = "会议", type2 = "签退" });
                }
                else if (row.kaoqin_Type.Equals("7"))
                {
                    VModel.ClockData.Add(new ClockDataModel { datetime = row.checkTime.Value.ToString("yyyy-MM-dd HH:mm:ss"), type = "外出宣传", type2 = "签到" });
                }
                else if (row.kaoqin_Type.Equals("8"))
                {
                    VModel.ClockData.Add(new ClockDataModel { datetime = row.checkTime.Value.ToString("yyyy-MM-dd HH:mm:ss"), type = "外出宣传", type2 = "签退" });
                }
            }
            VModel.ClockData = VModel.ClockData.OrderBy(p => p.datetime).ToList();
            TimeSpan ts = endDay_Month.AddDays(1) - startDay_Month;
            for (int i = 1; i <= ts.Days; i++)
            {
                /*
                 * :
1,签到  2签退
:
5，6会议签到签退
:
7，8外出宣传签到签退
:
9，10 签到 签退
                 * */
                bool isQingjia = false;//表示请假时间范围是否覆盖了排班时间
                if (int.Parse(DateTime.Now.ToString("yyyyMMdd"))< int.Parse(startDay_Month.AddDays(i - 1).ToString("yyyyMMdd")))
                {
                    break;
                }
                var model = new KaoQinDataModel() { day = startDay_Month.AddDays(i - 1).ToString("MM/dd"), workIn="N", workOut="N", leave="N", state="0"};

               var kaoqinModels= KaoqinList.Where(p => p.checkTime.Value.ToString("yyyyMMdd").Equals(startDay_Month.AddDays(i - 1).ToString("yyyyMMdd")) && (p.kaoqin_Type.Equals("1")|| p.kaoqin_Type.Equals("5") || p.kaoqin_Type.Equals("7") || p.kaoqin_Type.Equals("9")));
                var paibanModel = paiBanList.Find(p => p.Day.Equals(startDay_Month.AddDays(i - 1).ToString("yyyy-MM-dd")));
                if (paibanModel != null)
                {
                   
                    var startTimes = paibanModel.StartTime.Split(":".ToCharArray());
                    var endTimes = paibanModel.EndTime.Split(":".ToCharArray());

                    DateTime BanBieberStartTime = DateTime.ParseExact(paibanModel.Day, "yyyy-MM-dd", CultureInfo.CurrentCulture);
                    BanBieberStartTime = BanBieberStartTime.AddHours(int.Parse(startTimes[0])).AddMinutes(int.Parse(startTimes[1]));

                    DateTime BanBieberEndTime = DateTime.ParseExact(paibanModel.Day, "yyyy-MM-dd", CultureInfo.CurrentCulture);
                    BanBieberEndTime = BanBieberEndTime.AddHours(int.Parse(endTimes[0])).AddMinutes(int.Parse(endTimes[1]));

                    if (kaoqinModels.Count()>0)
                    {

                        var kaoqinModel = kaoqinModels.OrderBy(p => p.checkTime).First();//取最早一笔刷卡记录

                        if (kaoqinModel.checkTime.Value <= BanBieberStartTime)
                        {
                            model.workIn = "Y";
                        }
                    }

                    kaoqinModels = KaoqinList.Where(p => p.checkTime.Value.ToString("yyyyMMdd").Equals(startDay_Month.AddDays(i - 1).ToString("yyyyMMdd")) && (p.kaoqin_Type.Equals("2") || p.kaoqin_Type.Equals("6") || p.kaoqin_Type.Equals("8") || p.kaoqin_Type.Equals("10")));
                    if (kaoqinModels.Count() > 0)
                    {
                        var kaoqinModel = kaoqinModels.OrderByDescending(p => p.checkTime).First();//取最晚一笔刷卡记录

                        if (kaoqinModel.checkTime.Value >= BanBieberEndTime)
                        {
                            model.workOut = "Y";
                        }
                    }
                    var qingjiaModels = monthList.Where(p => int.Parse(p.StartDateTime.Value.ToString("yyyyMMdd")) <= int.Parse(startDay_Month.AddDays(i - 1).ToString("yyyyMMdd"))
                 && int.Parse(p.EndDateTime.Value.ToString("yyyyMMdd")) >= int.Parse(startDay_Month.AddDays(i - 1).ToString("yyyyMMdd"))
                );
                    
                    if (qingjiaModels.Count()>0)
                    {
                        model.leave = "Y";
                        var qingjiaModel = qingjiaModels.First();
                        DateTime leveStartTime;
                        DateTime leveEndTime;
                        //请假开始日期是否是今天
                        if (int.Parse(qingjiaModel.StartDateTime.Value.ToString("yyyyMMdd")) == int.Parse(startDay_Month.AddDays(i - 1).ToString("yyyyMMdd")))
                        {
                            leveStartTime = DateTime.ParseExact(startDay_Month.AddDays(i - 1).ToString("yyyy-MM-dd"), "yyyy-MM-dd", CultureInfo.CurrentCulture);
                            leveStartTime = leveStartTime.AddHours(qingjiaModel.StartDateTime.Value.Hour);
                            leveStartTime= leveStartTime.AddMinutes(qingjiaModel.StartDateTime.Value.Minute);

                            //model.tempStr = qingjiaModel.StartDateTime.Value.ToString("yyyy-MM-dd HH:mm");

                            //leveStartTime = DateTime.ParseExact(startDay_Month.AddDays(i - 1).ToString("yyyy-MM-dd") + " 13:50", "yyyy-MM-dd HH:mm", CultureInfo.CurrentCulture);
                        }
                        else
                        {
                            leveStartTime = DateTime.ParseExact(startDay_Month.AddDays(i - 1).ToString("yyyy-MM-dd"), "yyyy-MM-dd", CultureInfo.CurrentCulture);
                          

                        }

                       
                        //请假结束日期是否是今天
                        if (int.Parse(qingjiaModel.EndDateTime.Value.ToString("yyyyMMdd")) == int.Parse(startDay_Month.AddDays(i - 1).ToString("yyyyMMdd")))
                        {
                            leveEndTime = DateTime.ParseExact(startDay_Month.AddDays(i - 1).ToString("yyyy-MM-dd"), "yyyy-MM-dd", CultureInfo.CurrentCulture);
                            leveEndTime = leveEndTime.AddHours(qingjiaModel.EndDateTime.Value.Hour==0?23: qingjiaModel.EndDateTime.Value.Hour);
                            leveEndTime = leveEndTime.AddMinutes(qingjiaModel.EndDateTime.Value.Hour == 0 ? 59 : qingjiaModel.EndDateTime.Value.Minute);
                            //leveEndTime = DateTime.ParseExact(startDay_Month.AddDays(i - 1).ToString("yyyy-MM-dd"), "yyyy-MM-dd", CultureInfo.CurrentCulture)
                            //.AddHours(qingjiaModel.EndDateTime.Value.Hour).AddMinutes(qingjiaModel.EndDateTime.Value.Minute);
                        }
                        else
                        {
                            leveEndTime = DateTime.ParseExact(startDay_Month.AddDays(i - 1).ToString("yyyy-MM-dd")+ " 23:59", "yyyy-MM-dd HH:mm", CultureInfo.CurrentCulture);

                        }

                        if (BanBieberStartTime >= leveStartTime && BanBieberEndTime <= leveEndTime)//请假时间覆盖班别
                        {
                            isQingjia = true;
                        }

                        if ((model.workIn.Equals("N") || model.workOut.Equals("N")) && model.leave.Equals("Y"))
                        {
                            if (isQingjia)
                            {
                                model.state = "1";
                            }
                        }
                        else if (model.workIn.Equals("Y") && model.workOut.Equals("Y"))
                        {
                            model.state = "1";
                        }




                    }
                    

                }
                else
                {

                    if (kaoqinModels.Count() > 0)
                    {
                        model.workIn = "Y";
                    }
                    kaoqinModels = KaoqinList.Where(p => p.checkTime.Value.ToString("yyyyMMdd").Equals(startDay_Month.AddDays(i - 1).ToString("yyyyMMdd")) && (p.kaoqin_Type.Equals("2") || p.kaoqin_Type.Equals("6") || p.kaoqin_Type.Equals("8") || p.kaoqin_Type.Equals("10")));
                    if (kaoqinModels.Count() > 0)
                    {
                        model.workOut = "Y";
                    }
                    var qingjiaModel = monthList.Find(p => int.Parse(p.StartDateTime.Value.ToString("yyyyMMdd")) <= int.Parse(startDay_Month.AddDays(i - 1).ToString("yyyyMMdd"))
                 && int.Parse(p.EndDateTime.Value.ToString("yyyyMMdd")) >= int.Parse(startDay_Month.AddDays(i - 1).ToString("yyyyMMdd"))
                );
                    if (qingjiaModel != null)
                    {
                        model.leave = "Y";
                    }
                    if ((model.workIn.Equals("N") || model.workOut.Equals("N")) && model.leave.Equals("Y"))
                    {
                        model.state = "1";
                    }
                    else if (model.workIn.Equals("Y") && model.workOut.Equals("Y"))
                    {
                        model.state = "1";
                    }

                }

                
                VModel.data.Add(model);
            }

            //VModel.data.Add(new KaoQinDataModel() { day = "02/01", leave = "N", state = "1", workIn = "Y", workOut = "Y" });
            //VModel.data.Add(new KaoQinDataModel() { day = "02/02", leave = "N", state = "0", workIn = "Y", workOut = "N" });
            //VModel.data.Add(new KaoQinDataModel() { day = "02/03", leave = "Y", state = "1", workIn = "Y", workOut = "N" });
            return VModel;
        }
        
        private  int getdays(DateTime startDay, DateTime endDay, List<user_leaveEntity> dataList)
        {
            int monthDays = 0;
            foreach (var item in dataList)
            {
                var StartDateTime = DateTime.ParseExact(item.StartDateTime.Value.ToString("yyyy-MM-dd 00:00:00"), "yyyy-MM-dd HH:mm:ss", CultureInfo.CurrentCulture);
                var EndDateTime = DateTime.ParseExact(item.EndDateTime.Value.ToString("yyyy-MM-dd 00:00:00"), "yyyy-MM-dd HH:mm:ss", CultureInfo.CurrentCulture);
                //请假在查询范围内的
                if (DateTime.Compare(StartDateTime, startDay) >= 0 && DateTime.Compare(endDay, EndDateTime) >= 0)
                {
                    TimeSpan ts = EndDateTime.AddDays(1) - StartDateTime;
                    monthDays = monthDays + ts.Days;
                }
                //请假覆盖查询范围
                if (DateTime.Compare(startDay, StartDateTime) > 0 && DateTime.Compare(EndDateTime, endDay) > 0)
                {
                    TimeSpan ts = endDay.AddDays(1) - startDay;
                    monthDays = monthDays + ts.Days;
                }
                //请假开始日期在查询范围之前&结束日期在查询范围之内
                if (DateTime.Compare(startDay, StartDateTime) >0 && DateTime.Compare(endDay, EndDateTime) >= 0)
                {
                    TimeSpan ts = EndDateTime.AddDays(1) - startDay;
                    monthDays = monthDays + ts.Days;
                }
                //请假开始日期在查询范围内&结束日期在查询范围之后
                if (DateTime.Compare(StartDateTime, startDay) >= 0 && DateTime.Compare(EndDateTime, endDay) > 0)
                {
                    TimeSpan ts = endDay.AddDays(1) - StartDateTime;
                    monthDays = monthDays + ts.Days;
                }
            }

            return monthDays;
        }

        public ActionResult KaoQinIndex(string userId)
        {
            //ViewData["userId"] = userId;
            //ViewData["userName"] = userId;
            
            ViewData["userId"] = userId;
            marketSalesApp app = new marketSalesApp();
            UserInfoResultModel t = app.GetUserInfo(userId);
            if (t != null)
            {
                ViewData["userName"] = t.Name + " " + t.POP_TYPE_NAME;
                
            }
            else
            {
                return null;
            }
            return View("ClockView");

        }
       
        public ActionResult KaoQinQueryIndex(string userId)
        {
            //ViewData["userId"] = userId;
            //ViewData["userName"] = userId;
            ViewData["userId"] = userId;
            return View("KaoQinViewV2");

        }
        public ActionResult ALLMemberLeveIndex(string userId)
        {
            //ViewData["userId"] = userId;
            //ViewData["userName"] = userId;
            ViewData["userId"] = userId;
            return View("ALLMemberLeveView");
        }
        public ActionResult MemberNewLeveIndex(string userId)
        {
            //ViewData["userId"] = userId;
            //ViewData["userName"] = userId;
            ViewData["userId"] = userId;
            return View("MemberNewLeveView");

        }
        public ActionResult MemberLeveIndex(string userId)
        {
            //ViewData["userId"] = userId;
            //ViewData["userName"] = userId;
            ViewData["userId"] = userId;
            return View("MemberLeveView");

        }
        public ActionResult PaiBanIndex(string userId)
        {
            //ViewData["userId"] = userId;
            //ViewData["userName"] = userId;
            marketShopApp shopApp = new marketShopApp();
            //banbieApp app = new banbieApp();
            
            ViewData["userId"] = userId;
            ViewData["shopCode"] = "SZ1709004";
           /* var ents = app.getBanbie("SZ1709004");
            if (ents.Count > 0)
            {
                ViewData["isSetBanBie"] = 1;
            }
            else
            {
                ViewData["isSetBanBie"] = 0;
            }
            */
                return View("MainPaibanView");

        }
        public void savePaibanToDb(string type,List<BanbieEntity> banBieEnts,DateTime tempTime,string pops)
        {
            paibanApp app = new paibanApp();
            var banBieEnt = banBieEnts.Where(p => p.type.Equals(type)).First();
            foreach (var pop in pops.Split(",".ToCharArray()))
            {
                if (!pop.Equals(""))
                {
                    var ent = new paibanEntity()
                    {
                        Day = tempTime.ToString("yyyy-MM-dd"),
                        StartTime = banBieEnt.startH + ":" + banBieEnt.startM,
                        EndTime = banBieEnt.endH + ":" + banBieEnt.endM,
                        Type = type,
                        userId = pop
                    };
                    app.saveEnt(ent);
                }
            }
        }
        public JsonResult getAllPaiban(string shopCode, string startDay, string endDay)
        {
            Hashtable data = new Hashtable();
            paibanApp app = new paibanApp();
            List<PaibanSelectOptionModel> list = new List<PaibanSelectOptionModel>();
            marketSalesApp salesApp = new marketSalesApp();
            List<PaibanModel> paibanList = new List<PaibanModel>();
            DateTime startTime = DateTime.ParseExact(startDay, "yyyy-MM-dd", CultureInfo.CurrentCulture);
            DateTime endTime = DateTime.ParseExact(endDay, "yyyy-MM-dd", CultureInfo.CurrentCulture);
            

            var ci = CultureInfo.CurrentCulture;

            var weekday = getWeeekDay(DateTime.Now.Year, ci.Calendar.GetWeekOfYear(DateTime.Now, ci.DateTimeFormat.CalendarWeekRule, ci.DateTimeFormat.FirstDayOfWeek));
            var nextWeekday = getWeeekDay(DateTime.Now.AddDays(7).Year, ci.Calendar.GetWeekOfYear(DateTime.Now.AddDays(7), ci.DateTimeFormat.CalendarWeekRule, ci.DateTimeFormat.FirstDayOfWeek));

            int noPanbanCount = 0;
            var pops = salesApp.getPop(shopCode);
            foreach (var item in pops)
            {
                startTime = DateTime.ParseExact(startDay, "yyyy-MM-dd", CultureInfo.CurrentCulture);
                endTime = DateTime.ParseExact(endDay, "yyyy-MM-dd", CultureInfo.CurrentCulture);
                PaibanModel model = new PaibanModel();
                model.empCode = item.sales_No;
                model.empName = item.sales_Name;
                var paiban = app.getPaibanEnts(item.sales_No, startDay, endDay);
                if (paiban.Count <= 0)
                {
                    noPanbanCount = noPanbanCount + 1;
                }
               
                model.type7 = "";
                model.type1 = "";
                model.type2 = "";
                model.type3 = "";
                model.type4 = "";
                model.type5 = "";
                model.type6 = "";
                model.todayType = "";
                // data.Add("firstDay", days.First());
                //data.Add("lastDay", days.Last());
                var currentWeekData = app.getPaibanEnts(item.sales_No, weekday["firstDay"].ToString(), weekday["lastDay"].ToString());
                var nextWeekData = app.getPaibanEnts(item.sales_No, nextWeekday["firstDay"].ToString(), nextWeekday["lastDay"].ToString());
                model.sumTypeA = currentWeekData.Count(p => p.Type.Equals("A"));
                model.sumTypeB = currentWeekData.Count(p => p.Type.Equals("B"));
                model.sumTypeC = currentWeekData.Count(p => p.Type.Equals("C"));
                model.sumTypeD = currentWeekData.Count(p => p.Type.Equals("D"));
                model.nextSumTypeA = nextWeekData.Count(p => p.Type.Equals("A"));
                model.nextSumTypeB = nextWeekData.Count(p => p.Type.Equals("B"));
                model.nextSumTypeC = nextWeekData.Count(p => p.Type.Equals("C"));
                model.nextSumTypeD = nextWeekData.Count(p => p.Type.Equals("D"));
                if (currentWeekData.Where(p => p.Day.Equals(DateTime.Now.ToString("yyyy-MM-dd"))).Count()>0)
                {
                    model.todayType = currentWeekData.Where(p => p.Day.Equals(DateTime.Now.ToString("yyyy-MM-dd"))).First().Type;
                }
                int i = 0;
                while (int.Parse(startTime.ToString("yyyyMMdd")) <= int.Parse(endTime.ToString("yyyyMMdd")))
                {
                    var sub = paiban.Where(p => p.Day.Equals(startTime.ToString("yyyy-MM-dd")));
                    if (i == 0)
                    {
                        if (sub.Count() > 0)
                        {
                            model.type7 = sub.First().Type;
                        }

                    }
                    else if (i == 1)
                    {
                        if (sub.Count() > 0)
                        {
                            model.type1 = sub.First().Type;
                        }
                    }
                    else if (i == 2)
                    {
                        if (sub.Count() > 0)
                        {
                            model.type2 = sub.First().Type;
                        }
                    }
                    else if (i == 3)
                    {
                        if (sub.Count() > 0)
                        {
                            model.type3 = sub.First().Type;
                        }
                    }
                    else if (i == 4)
                    {
                        if (sub.Count() > 0)
                        {
                            model.type4 = sub.First().Type;
                        }
                    }
                    else if (i == 5)
                    {
                        if (sub.Count() > 0)
                        {
                            model.type5 = sub.First().Type;
                        }
                    }
                    else if (i == 6)
                    {
                        if (sub.Count() > 0)
                        {
                            model.type6 = sub.First().Type;
                        }
                    }
                    i = i + 1;
                    startTime = startTime.AddDays(1);
                }
                paibanList.Add(model);
                

            }
            data.Add("list", paibanList);
            data.Add("noPanbanCount", noPanbanCount);
            data.Add("panbanCount", pops.Count-noPanbanCount);
            

            return Json(data, JsonRequestBehavior.AllowGet);
        }
        public  JsonResult savePaiban(string shopCode,string pops,string startDay,string endDay,string paiban_Type_1, string paiban_Type_2, string paiban_Type_3, string paiban_Type_4, string paiban_Type_5, string paiban_Type_6, string paiban_Type_7)
        {
            Hashtable data = new Hashtable();
            if (int.Parse(DateTime.ParseExact(startDay, "yyyy-MM-dd", CultureInfo.CurrentCulture).ToString("yyyyMMdd")) > int.Parse(DateTime.ParseExact(endDay, "yyyy-MM-dd", CultureInfo.CurrentCulture).ToString("yyyyMMdd")))

            {
                data.Add("isOk", 0);
                data.Add("message", "排班失败,开始日期不能大于结束日期!");
                return Json(data, JsonRequestBehavior.AllowGet);

            }
            if (int.Parse(DateTime.ParseExact(startDay, "yyyy-MM-dd", CultureInfo.CurrentCulture).ToString("yyyyMMdd")) <= int.Parse(DateTime.Now.ToString("yyyyMMdd")))

            {
                data.Add("isOk", 0);
                data.Add("message", "排班失败,只能排今天往后的日期!");
                return Json(data, JsonRequestBehavior.AllowGet);

            }
            var startTime = DateTime.ParseExact(startDay, "yyyy-MM-dd", CultureInfo.CurrentCulture);
            var endTime= DateTime.ParseExact(endDay, "yyyy-MM-dd", CultureInfo.CurrentCulture);
            var tempTime = startTime;
            banbieApp app = new banbieApp();
            var banBieEnts = app.getBanbie(shopCode);
            while (tempTime <= endTime)
            {
                var weekDay = tempTime.DayOfWeek;
                 
                if (weekDay == DayOfWeek.Sunday)//星期天
                {
                    savePaibanToDb(paiban_Type_7, banBieEnts, tempTime, pops);


                }
                else if (weekDay == DayOfWeek.Monday)
                {
                    savePaibanToDb(paiban_Type_1, banBieEnts, tempTime, pops);


                }
                else if (weekDay == DayOfWeek.Tuesday)
                {
                    savePaibanToDb(paiban_Type_2, banBieEnts, tempTime, pops);


                }
                else if (weekDay == DayOfWeek.Wednesday)
                {
                    savePaibanToDb(paiban_Type_3, banBieEnts, tempTime, pops);


                }
                else if (weekDay == DayOfWeek.Thursday)
                {
                    savePaibanToDb(paiban_Type_4, banBieEnts, tempTime, pops);


                }
                else if (weekDay == DayOfWeek.Friday)
                {
                    savePaibanToDb(paiban_Type_5, banBieEnts, tempTime, pops);


                }
                else if (weekDay == DayOfWeek.Saturday)
                {
                    savePaibanToDb(paiban_Type_6, banBieEnts, tempTime, pops);


                }

                tempTime = tempTime.AddDays(1);
            }

           

            data.Add("isOk", 1);
            return Json(data, JsonRequestBehavior.AllowGet);
        }
        public JsonResult getPopByCode(string code)
        {
            try
            {
                Hashtable data = new Hashtable();
                List<PaibanSelectOptionModel> list = new List<PaibanSelectOptionModel>();
                marketSalesApp app = new marketSalesApp();
                var userInfo = app.getUserInfoBySalesNo(code);
                if (userInfo != null)
                {
                    list.Add(new PaibanSelectOptionModel() { code = code, name = userInfo.Name });
                }
                else {
                    list.Add(new PaibanSelectOptionModel() { code = code, name = code });
                }
                data.Add("pop", list);
                return Json(data, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(ex.Message, JsonRequestBehavior.AllowGet);
            }
        }
        public JsonResult getShopPops(string shopCode)
        {
            Hashtable data = new Hashtable();
            List<PaibanSelectOptionModel> list = new List<PaibanSelectOptionModel>();
            marketSalesApp app = new marketSalesApp();
            foreach (var item in app.getPop(shopCode))
            {
                list.Add(new PaibanSelectOptionModel() { code = item.sales_No, name = item.sales_Name });
            }
            data.Add("pop", list);
            return Json(data, JsonRequestBehavior.AllowGet);
        }
        public JsonResult isBanBieSet(string shopCode)
        {
            Hashtable data = new Hashtable();
            banbieApp app = new banbieApp();
            var ents = app.getBanbie(shopCode);
            if (ents.Count > 0)
            {
                data.Add("isSetBanBie", 1);
               
            }
            else
            {
                data.Add("isSetBanBie", 0);
                
            }
            
            return Json(data, JsonRequestBehavior.AllowGet);
        }
        public JsonResult saveBanBie(string shopCode,int A_StartH,int A_StartM,int A_EndH,int A_EndM, int B_StartH, int B_StartM, int B_EndH, int B_EndM
            , int C_StartH, int C_StartM, int C_EndH, int C_EndM, int D_StartH, int D_StartM, int D_EndH, int D_EndM)
        {
            Hashtable data = new Hashtable();
            banbieApp app = new banbieApp();
            app.saveEnts(shopCode, A_StartH, A_StartM, A_EndH, A_EndM, B_StartH, B_StartM, B_EndH, B_EndM
            , C_StartH, C_StartM, C_EndH, C_EndM, D_StartH, D_StartM, D_EndH, D_EndM);
            data.Add("state", 1);
            return Json(data, JsonRequestBehavior.AllowGet);
        }
        public JsonResult GetBanBie(string shopCode)
        {
            Hashtable data = new Hashtable();
            banbieApp app = new banbieApp();
            var ents = app.getBanbie(shopCode);
            if (ents.Where(p => p.type.Equals("A")).Count() > 0)
            {
                data.Add("A_StartH", int.Parse( ents.Where(p => p.type.Equals("A")).First().startH));
                data.Add("A_StartM", int.Parse(ents.Where(p => p.type.Equals("A")).First().startM));
                data.Add("A_EndH", int.Parse(ents.Where(p => p.type.Equals("A")).First().endH));
                data.Add("A_EndM", int.Parse(ents.Where(p => p.type.Equals("A")).First().endM));
            }
            else
            {
                data.Add("A_StartH", 8);
                data.Add("A_StartM", 0);
                data.Add("A_EndH", 12);
                data.Add("A_EndM", 0);
            }
            if (ents.Where(p => p.type.Equals("B")).Count() > 0)
            {
                data.Add("B_StartH", int.Parse(ents.Where(p => p.type.Equals("B")).First().startH));
                data.Add("B_StartM", int.Parse(ents.Where(p => p.type.Equals("B")).First().startM));
                data.Add("B_EndH", int.Parse(ents.Where(p => p.type.Equals("B")).First().endH));
                data.Add("B_EndM", int.Parse(ents.Where(p => p.type.Equals("B")).First().endM));
            }
            else
            {
                data.Add("B_StartH", 12);
                data.Add("B_StartM", 0);
                data.Add("B_EndH", 18);
                data.Add("B_EndM", 0);
            }
            if (ents.Where(p => p.type.Equals("C")).Count() > 0)
            {
                data.Add("C_StartH", int.Parse(ents.Where(p => p.type.Equals("C")).First().startH));
                data.Add("C_StartM", int.Parse(ents.Where(p => p.type.Equals("C")).First().startM));
                data.Add("C_EndH", int.Parse(ents.Where(p => p.type.Equals("C")).First().endH));
                data.Add("C_EndM", int.Parse(ents.Where(p => p.type.Equals("C")).First().endM));
            }
            else
            {
                data.Add("C_StartH", 18);
                data.Add("C_StartM", 0);
                data.Add("C_EndH", 23);
                data.Add("C_EndM", 0);
            }
            if (ents.Where(p => p.type.Equals("D")).Count() > 0)
            {
                data.Add("D_StartH", int.Parse(ents.Where(p => p.type.Equals("D")).First().startH));
                data.Add("D_StartM", int.Parse(ents.Where(p => p.type.Equals("D")).First().startM));
                data.Add("D_EndH", int.Parse(ents.Where(p => p.type.Equals("D")).First().endH));
                data.Add("D_EndM", int.Parse(ents.Where(p => p.type.Equals("D")).First().endM));
            }
            else
            {
                data.Add("D_StartH", 19);
                data.Add("D_StartM", 0);
                data.Add("D_EndH", 23);
                data.Add("D_EndM", 59);
            }

            return Json(data, JsonRequestBehavior.AllowGet);
        }
        public Hashtable getWeeekDay(int year, int weeks)
        {
            DateTime start = new DateTime(year, 1, 1);
            List<string> days = new List<string>();
            var ci = CultureInfo.CurrentCulture;
            Hashtable data = new Hashtable();
            for (int i = 0; i < 364; i++)
            {
                start = start.AddDays(1);
                if (ci.Calendar.GetWeekOfYear(start, ci.DateTimeFormat.CalendarWeekRule, ci.DateTimeFormat.FirstDayOfWeek) == weeks)
                {
                    days.Add(start.ToString("yyyy-MM-dd"));
                }
            }


            data.Add("firstDay", days.First());
            data.Add("lastDay", days.Last());
            return data;
        }
        public JsonResult GetWeekDays(int year, int weeks)
        {
          
           
            return Json(getWeeekDay(year,weeks), JsonRequestBehavior.AllowGet);
        }
        public JsonResult GetWeekNumber(DateTime dt)
        {
            var ci = CultureInfo.CurrentCulture;
            Hashtable data = new Hashtable();
            data.Add("weekNumber", ci.Calendar.GetWeekOfYear(dt, ci.DateTimeFormat.CalendarWeekRule, ci.DateTimeFormat.FirstDayOfWeek));
            return Json(data, JsonRequestBehavior.AllowGet);
        }
        public ActionResult MemberLeveOrderIndex(string userId,string id,string aUserId)
        {
            //ViewData["userId"] = userId;
            //ViewData["userName"] = userId;
            marketSalesApp salesApp = new marketSalesApp();
            var userInfo = salesApp.GetUserInfo(userId);
            MemberLeveDataModel model = new MemberLeveDataModel();
            user_leaveAPP leveApp = new user_leaveAPP();
            var ent = leveApp.getLeaveDataById(id);
            if (ent != null)
            {
                model.id = ent.id;
               
                if (ent.leave_type.Equals("1"))
                {
                    model.leaveType = "产假";
                }
                else if (ent.leave_type.Equals("2"))
                {
                    model.leaveType  = "病假";
                }
                else if (ent.leave_type.Equals("3"))
                {
                    model.leaveType = "事假";
                }
                else if (ent.leave_type.Equals("4"))
                {
                    model.leaveType  = "婚假";
                }
                else if (ent.leave_type.Equals("5"))
                {
                    model.leaveType = "丧假";
                }
                else if (ent.leave_type.Equals("6"))
                {
                    model.leaveType = "护理假";
                }
                else if (ent.leave_type.Equals("7"))
                {
                    model.leaveType = "年休假";
                }
                else if (ent.leave_type.Equals("8"))
                {
                    model.leaveType = "休息";
                }

                if (!ent.state.HasValue)
                {
                    model.state = "1";
                }
                else
                {
                    model.state = ent.state.Value.ToString();
                }
                model.startDay = ent.StartDateTime.Value.ToString("yyyy-MM-dd");
                model.endDay = ent.EndDateTime.Value.ToString("yyyy-MM-dd");
                model.desc = ent.desc;
                model.adesc = ent.adesc;
                model.imgs = new List<imageModel>();

                if (ent.file_id != null && !ent.file_id.Equals(""))
                {
                    string[] ids = ent.file_id.Split(",".ToCharArray());
                    //marketSalesActualApp app = new marketSalesActualApp();
                    foreach (string fid in ids)
                    {
                        if (fid != null && !fid.Equals(""))
                        {
                            var fileModel = new imageModel() { id = fid };
                            fileModel.url = "https://iretailerapp.flnet.com/Messages/APPUploadFile/" + ent.day.Value.ToString("yyyyMM") + "/" + fid + ".jpg";
                            model.imgs.Add(fileModel);
                        }
                    }
                }
                TimeSpan sp =ent.EndDateTime.Value.Subtract(ent.StartDateTime.Value);
                int days = sp.Days;

                if (days < 0)
                {
                    days = 0;
                }
                else
                {
                    days = days + 1;
                }

                model.leaveDays = days + "天";



            }
            ViewData["userId"] = userId;
            ViewData["userName"] = userInfo.Name;
            ViewData["empCode"] = userInfo.SalesNo;
            ViewData["aUserId"] = aUserId;
            ViewData["data"] = model;
            return View("MemberLeveOrder");

        }
        public JsonResult ALLMemberLeveDataQuery(string userId)
        {
            Hashtable data = new Hashtable();

            user_leaveAPP leaveApp = new user_leaveAPP();
            var yearList = leaveApp.getAllLeaveByDate(userId, DateTime.Now.Year + "-01-01", DateTime.Now.Year + "-12-31");

           
            /*
              <option value="1">产假</option>
    <option value="2">病假</option>
    <option value="3" selected="selected">事假</option>
    <option value="4">婚假</option>
    <option value="5">丧假</option>
    <option value="6">护理假</option>
    <option value="7">年休假</option>
    <option value="8">休息</option>
             */
            List<MemberLeveDataModel> type1List = new List<MemberLeveDataModel>();
            marketSalesApp salesApp = new marketSalesApp();
            foreach (var item in yearList.Where(p => !p.state.HasValue || p.state.Value == 1).OrderByDescending(p=>p.StartDateTime))
            {
                
                var userInfo = salesApp.GetUserInfo(item.userid);
                string type = "";
                if (item.leave_type.Equals("1"))
                {
                    type = "产假";
                }
                else if (item.leave_type.Equals("2"))
                {
                    type = "病假";
                }
                else if (item.leave_type.Equals("3"))
                {
                    type = "事假";
                }
                else if (item.leave_type.Equals("4"))
                {
                    type = "婚假";
                }
                else if (item.leave_type.Equals("5"))
                {
                    type = "丧假";
                }
                else if (item.leave_type.Equals("6"))
                {
                    type = "护理假";
                }
                else if (item.leave_type.Equals("7"))
                {
                    type = "年休假";
                }
                else if (item.leave_type.Equals("8"))
                {
                    type = "休息";
                }

                type1List.Add(new MemberLeveDataModel() { userId=item.userid, userName= userInfo.Name, id = item.id, startDay = item.StartDateTime.Value.ToString("yyyy-MM-dd"), endDay = item.EndDateTime.Value.ToString("yyyy-MM-dd"), leaveType = type, state = "签核中" });
            }
            List<MemberLeveDataModel> type1List2 = new List<MemberLeveDataModel>();
            foreach (var item in yearList.Where(p => p.state==2 || p.state==3).OrderByDescending(p => p.StartDateTime))
            {
                var userInfo = salesApp.GetUserInfo(item.userid);
                string type = "";
                if (item.leave_type.Equals("1"))
                {
                    type = "产假";
                }
                else if (item.leave_type.Equals("2"))
                {
                    type = "病假";
                }
                else if (item.leave_type.Equals("3"))
                {
                    type = "事假";
                }
                else if (item.leave_type.Equals("4"))
                {
                    type = "婚假";
                }
                else if (item.leave_type.Equals("5"))
                {
                    type = "丧假";
                }
                else if (item.leave_type.Equals("6"))
                {
                    type = "护理假";
                }
                else if (item.leave_type.Equals("7"))
                {
                    type = "年休假";
                }
                else if (item.leave_type.Equals("8"))
                {
                    type = "休息";
                }

                type1List2.Add(new MemberLeveDataModel() { userId = item.userid, userName = userInfo.Name, id = item.id, startDay = item.StartDateTime.Value.ToString("yyyy-MM-dd"), endDay = item.EndDateTime.Value.ToString("yyyy-MM-dd"), leaveType = type, state = item.state.Value.ToString() });
            }

            data.Add("type1List", type1List);
            data.Add("type2List", type1List2);
            return Json(data, JsonRequestBehavior.AllowGet);
        }
        public JsonResult MemberLeveDataQuery(string userId)
        {
            Hashtable data = new Hashtable();

            user_leaveAPP leaveApp = new user_leaveAPP();
            var yearList = leaveApp.getLeaveByDate3(userId, DateTime.Now.Year + "-01-01", DateTime.Now.Year + "-12-31");

            data.Add("leveCount", yearList.Where(p=>p.state==2|| p.state == 3).Count());
            data.Add("backCount", 0);
            data.Add("inCount", yearList.Where(p => !p.state.HasValue || p.state.Value == 1).Count());
            /*
              <option value="1">产假</option>
    <option value="2">病假</option>
    <option value="3" selected="selected">事假</option>
    <option value="4">婚假</option>
    <option value="5">丧假</option>
    <option value="6">护理假</option>
    <option value="7">年休假</option>
    <option value="8">休息</option>
             */
            List<MemberLeveDataModel> type1List = new List<MemberLeveDataModel>();
            foreach (var item in yearList.Where(p => !p.state.HasValue || p.state.Value==1))
            {
                string type = "";
                if (item.leave_type.Equals("1"))
                {
                    type = "产假";
                }else if (item.leave_type.Equals("2"))
                {
                    type = "病假";
                }
                else if (item.leave_type.Equals("3"))
                {
                    type = "事假";
                }
                else if (item.leave_type.Equals("4"))
                {
                    type = "婚假";
                }
                else if (item.leave_type.Equals("5"))
                {
                    type = "丧假";
                }
                else if (item.leave_type.Equals("6"))
                {
                    type = "护理假";
                }
                else if (item.leave_type.Equals("7"))
                {
                    type = "年休假";
                }
                else if (item.leave_type.Equals("8"))
                {
                    type = "休息";
                }
                
                type1List.Add(new MemberLeveDataModel() { id=item.id, startDay=item.StartDateTime.Value.ToString("yyyy-MM-dd"),endDay=item.EndDateTime.Value.ToString("yyyy-MM-dd"), leaveType=type, state="签核中" });
            }
            List<MemberLeveDataModel> type1List2 = new List<MemberLeveDataModel>();
            foreach (var item in yearList.Where(p => p.state.HasValue))
            {
                string type = "";
                if (item.leave_type.Equals("1"))
                {
                    type = "产假";
                }
                else if (item.leave_type.Equals("2"))
                {
                    type = "病假";
                }
                else if (item.leave_type.Equals("3"))
                {
                    type = "事假";
                }
                else if (item.leave_type.Equals("4"))
                {
                    type = "婚假";
                }
                else if (item.leave_type.Equals("5"))
                {
                    type = "丧假";
                }
                else if (item.leave_type.Equals("6"))
                {
                    type = "护理假";
                }
                else if (item.leave_type.Equals("7"))
                {
                    type = "年休假";
                }
                else if (item.leave_type.Equals("8"))
                {
                    type = "休息";
                }

                type1List2.Add(new MemberLeveDataModel() { id = item.id, startDay = item.StartDateTime.Value.ToString("yyyy-MM-dd"), endDay = item.EndDateTime.Value.ToString("yyyy-MM-dd"), leaveType = type, state = item.state.Value.ToString() });
            }

            data.Add("type1List", type1List);
            data.Add("type2List", type1List2);
            return Json(data, JsonRequestBehavior.AllowGet);
        }
        public JsonResult Clock(string userId,string type,double lng,double lat)
        {
            clockRest data = new clockRest();
            data.isOk = true;
            userKaoqinAPP app = new userKaoqinAPP();
            string adder = "";
            try
            {
                var client = new RestClient("https://restapi.amap.com");
                var request = new RestRequest("/v3/geocode/regeo?location=" + lng + "," + lat + "&key=2daf078d66ac9f5a8aa661906a618710&radius=1000&extensions=base&output=JSON ", Method.GET);
                IRestResponse response3 = client.Execute(request);

                if (response3.StatusCode == System.Net.HttpStatusCode.OK)
                {
                    JObject obj = JObject.Parse(response3.Content);
                    adder = ((JObject)obj.GetValue("regeocode")).GetValue("formatted_address").ToString();

                }
            }
            catch { }
            userKaoqinEntity ent = new userKaoqinEntity
            {
                checkTime = System.DateTime.Now,
                kaoqin_Type = type,
                userId = userId,
                LATITUDE = lat,
                LONGITUDE = lng, adder= adder
            };

            salesActualChangeRes res = app.Submit2(ent);
            
            if (!res.isOk)
            {
                data.isOk = false;
                data.message = res.errorMessage;
            }
            return Json(data, JsonRequestBehavior.AllowGet);
        }
        public JsonResult getClockDetail(string userId, string month)
        {
            return Json(this.getKaoQinData(userId, month), JsonRequestBehavior.AllowGet);
        }
        public JsonResult getClock(string userId)
        {
            try { 
            userKaoqinAPP app = new userKaoqinAPP();
            List<userKaoqinEntity> ents = app.GetClock(userId);
            clockTime models = new clockTime();
                models.inTime = "上班卡: 未刷卡";
                models.outTime = "下班卡: 未刷卡";
            foreach (userKaoqinEntity ent in ents)
            {
                if (ent.kaoqin_Type.Equals("1"))
                {
                    models.inTime ="上班卡: "+ ent.checkTime.Value.ToString("yyyy-MM-dd HH:mm");
                }
                if (ent.kaoqin_Type.Equals("2"))
                {
                    models.outTime = "下班卡: " + ent.checkTime.Value.ToString("yyyy-MM-dd HH:mm");
                }
            }
            return Json(models, JsonRequestBehavior.AllowGet);
        }
            catch (Exception ex)
            {
                return Json(ex.Message, JsonRequestBehavior.AllowGet);
    }

}

        public ActionResult QJIndex(string userId)
        {

            QJQueryModel model = new QJQueryModel();
            model.userId = userId;
            model.data = new List<QJDataModel>();
            user_leaveAPP app = new user_leaveAPP();
            var ents=app.getLeaveData(userId);
            foreach (var ent in ents)
            {
                model.data.Add(new QJDataModel() {
                     day= ent.StartDateTime.Value.ToString("MM/dd-") + ent.EndDateTime.Value.ToString("MM/dd")
                     , leaveType=ent.leave_type.ToString(), state=ent.state.HasValue?ent.state.Value.ToString():"1"
                });
            }
            ViewData["model"] = model;
                return View("QJView");
           
        }
        [HttpPost]
        public ActionResult AllowLeave(string id, string state,string userId)
        {
            user_leaveAPP app = new user_leaveAPP();
            app.AllowLeave(id, int.Parse(state),userId);
            return Content("操作成功！");
        }
        [HttpPost]
        public ActionResult AllowLeaveV2(string id, string state, string userId,string desc)
        {
            user_leaveAPP app = new user_leaveAPP();
            app.AllowLeave(id, int.Parse(state), userId, desc);
            return Content("1");
        }
        public PartialViewResult QJQueryView(string userId)
        {
            QJQueryModel model = new QJQueryModel();
            model.userId = userId;
            model.data = new List<QJDataModel>();
            user_leaveAPP app = new user_leaveAPP();
            var ents = app.getLeaveData(userId);
            foreach (var ent in ents)
            {
                model.data.Add(new QJDataModel()
                {
                    day = ent.StartDateTime.Value.ToString("MM/dd-") + ent.EndDateTime.Value.ToString("MM/dd")
                     ,
                    leaveType = ent.leave_type.ToString(),
                    state = ent.state.HasValue ? ent.state.Value.ToString() : "1"
                });
            }
            return PartialView("_QJQueryPage", model);
        }
        public PartialViewResult QJView(string userId)
        {
            QJCreatedModel model = new QJCreatedModel();
            model.userId = userId;
            return PartialView("_QJPartialPage", model);
        }
        public JsonResult GetDays(string startDay, string endDay)
        {
           
            Hashtable data = new Hashtable();

            DateTime startTime = DateTime.ParseExact(startDay, "yyyy-MM-dd", CultureInfo.CurrentCulture);
            DateTime endTime = DateTime.ParseExact(endDay, "yyyy-MM-dd", CultureInfo.CurrentCulture);

            TimeSpan sp = endTime.Subtract(startTime);
            int days = sp.Days;
            
            if (days < 0)
            {
                days = 0;
            }
            else
            {
                days = days + 1;
            }

            data.Add("day", days + "天");
            return Json(data, JsonRequestBehavior.AllowGet);
        }
        public JsonResult GetLeveDays(string userId)
        {
            Hashtable data = new Hashtable();

            user_leaveAPP leaveApp = new user_leaveAPP();


           
            var yearList = leaveApp.getLeaveByDate(userId, DateTime.Now.Year+"-01-01", DateTime.Now.Year + "-12-31");

            

            string yearDay = getdays(DateTime.ParseExact(DateTime.Now.Year+"-01-01", "yyyy-MM-dd", CultureInfo.CurrentCulture), DateTime.ParseExact(DateTime.Now.Year + "-12-31", "yyyy-MM-dd", CultureInfo.CurrentCulture), yearList).ToString() ;


            data.Add("leveDay", yearDay);
            data.Add("yearDay", 0 );
            return Json(data, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public ActionResult Leave(string userId, string startDay, string endDay, string desc, string leaveType)
        {
            try
            {
                if (int.Parse(DateTime.ParseExact(startDay, "yyyy-MM-dd", CultureInfo.CurrentCulture).ToString("yyyyMMdd")) > int.Parse(DateTime.ParseExact(endDay, "yyyy-MM-dd", CultureInfo.CurrentCulture).ToString("yyyyMMdd")))

                {
                    return Content("请假失败,日期止不能大于日期起!");
                }
                user_leaveAPP app = new user_leaveAPP();

                user_leaveEntity ent = new user_leaveEntity
                {
                    id = System.Guid.NewGuid().ToString(),
                    day = System.DateTime.Now,
                    day_type = "1",
                    desc = desc,
                    leave_type = leaveType,
                    StartDateTime = DateTime.ParseExact(startDay, "yyyy-MM-dd", CultureInfo.CurrentCulture),
                    EndDateTime = DateTime.ParseExact(endDay, "yyyy-MM-dd", CultureInfo.CurrentCulture),
                    userid = userId
                };
                salesActualChangeRes res = app.leave(ent);
                if (!res.isOk)
                {
                    return Content(res.errorMessage);
                    //return Content("操作成功！");
                }

                return Content("1");
            }
            catch (Exception ex) {
                return Content(ex.Message);
            }
        }
        [HttpPost]
        public ActionResult LeaveV2(string userId, string startDay, string endDay, string desc, string leaveType,string fileId)
        {
            try
            {
                if (int.Parse(DateTime.ParseExact(startDay, "yyyy-MM-dd", CultureInfo.CurrentCulture).ToString("yyyyMMdd")) > int.Parse(DateTime.ParseExact(endDay, "yyyy-MM-dd", CultureInfo.CurrentCulture).ToString("yyyyMMdd")))

                {
                    return Content("请假失败,日期止不能大于日期起!");
                }
                user_leaveAPP app = new user_leaveAPP();

                user_leaveEntity ent = new user_leaveEntity
                {
                    id = System.Guid.NewGuid().ToString(),
                    day = System.DateTime.Now,
                    day_type = "1",
                    desc = desc,
                    leave_type = leaveType,
                    StartDateTime = DateTime.ParseExact(startDay, "yyyy-MM-dd", CultureInfo.CurrentCulture),
                    EndDateTime = DateTime.ParseExact(endDay, "yyyy-MM-dd", CultureInfo.CurrentCulture),
                    userid = userId, file_id=fileId
                };
                salesActualChangeRes res = app.leave(ent);
                if (!res.isOk)
                {
                    return Content(res.errorMessage);
                    //return Content("操作成功！");
                }

                return Content("1");
            }
            catch (Exception ex)
            {
                return Content(ex.Message);
            }
        }
    }
}