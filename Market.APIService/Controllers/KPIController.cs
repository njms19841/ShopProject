using DataSynchronizationLib.SCMTableAdapters;
using DataSynchronizationStanbyLib.KPIDataSetTableAdapters;
using Market.APIService.Models;
using NFine.Application.APPManage;
using NFine.Application.TaskManage;
using NFine.Domain._02_ViewModel;
using NFine.Domain._03_Entity.APPManage;
using NFine.Domain._04_IRepository.TaskManage;
using NFine.Repository.TaskManage;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Market.APIService.Controllers
{
    public class View1_3PModel
    {
        public string minDay { get; set; }
        public string MaxDay { get; set; }
        public string userId { get; set; }
        public List<KeyValueModel> MonthList { get; set; }
        public List<KeyValueModel> TBuList { get; set; }
        public List<KeyValueModel> TBuList2 { get; set; }
        public List<KeyValueModel> TSBuList2 { get; set; }
        public List<KeyValueModel> TSBuList { get; set; }

    }
    public class View1_3_3PModel2
    {
        public string text1 { get; set; }
        public string text2 { get; set; }
        public double qty1 { get; set; }
        public double qty2 { get; set; }
        public double qty3 { get; set; }
    }
    public class View1_3_3PModel
    {
        public string text1 { get; set; }
        public string text2 { get; set; }
        public double qty1 { get; set; }
        public double qty2 { get; set; }
        public double qty3 { get; set; }
    }
    public class View1_1PModel
    {
        public string minDay { get; set; }
        public string MaxDay { get; set; }
        public List<KeyValueModel> MonthList { get; set; }
        public List<KeyValueModel> TBuList { get; set; }

    }
    public class descModel
    {
        public string key { get; set; }
        public double value1 { get; set; }
        public double value2 { get; set; }
        public double value3 { get; set; }
        public double value4 { get; set; }
    }
    public class KPiTotalModel
    {
        public string type { get; set; }
        public double TARGET_QTY { get; set; }
        public double TARGET_AMOUNT { get; set; }
        public double TOTAL_QTY { get; set; }
        public double TOTAL_AMOUNT { get; set; }
        public double TARGET_AMOUNT_RATE { get; set; }
        public double TARGET_QTY_RATE { get; set; }

    }
    
    public class KPIController : Controller
    {
        public ActionResult KPITotalIndex(string userId)
        {
            ViewData["userId"] = userId;
            return View("KPIView_Total");
        }
        public ActionResult KPISellOutIndex(string userId)
        {
            ViewData["userId"] = userId;
            return View("KPIView_Detail");
        }
        public ActionResult KPISellInIndex(string userId,string tbu)
        {
            ViewData["userId"] = userId;
            ViewData["tbu"] = tbu;
            ViewData["month"]=getMonths();
            return View("KPIView_1_1");
        }
        public JsonResult getKpiData(string day)
        {
            try
            {
                DateTime time = DateTime.ParseExact(day + " 00:00", "yyyy-MM-dd HH:mm", CultureInfo.CurrentCulture);
                Hashtable data = new Hashtable();
                V_SALES_TARGETTableAdapter ad = new V_SALES_TARGETTableAdapter();
                var table = ad.GetData(time);
                List<KPiTotalModel> list = new List<KPiTotalModel>();
                // data: ['天猫', '国美', '京东', '苏宁线上', '苏宁', '国美线上', '富连网', '战区', '推广员'],
                double[] arr = new double[9];
                foreach (var row in table)
                {
                    string type = row.KIND;
                    if (row.KIND_CODE.Equals("1GM"))
                    {
                        type = "国美";
                        arr[1] = (double)row.TOTAL_AMOUNT1;
                    }
                    else if (row.KIND_CODE.Equals("4"))
                    {
                        type = "天猫";
                        arr[0] = (double)row.TOTAL_AMOUNT1;
                    }
                    else if (row.KIND_CODE.Equals("FLW"))
                    {
                        type = "富连网";
                        arr[6] = (double)row.TOTAL_AMOUNT1;
                    }
                    else if (row.KIND_CODE.Equals("4TGY"))
                    {
                        type = "推广员";
                        arr[8] = (double)row.TOTAL_AMOUNT1;
                    }
                    else if (row.KIND_CODE.Equals("2"))
                    {
                        type = "苏宁线上";
                        arr[3] = (double)row.TOTAL_AMOUNT1;
                    }
                    else if (row.KIND_CODE.Equals("1"))
                    {
                        type = "京东";
                        arr[2] = (double)row.TOTAL_AMOUNT1;
                    }
                    else if (row.KIND_CODE.Equals("2SN"))
                    {
                        type = "苏宁";
                        arr[4] = (double)row.TOTAL_AMOUNT1;
                    }
                    else if (row.KIND_CODE.Equals("3BU"))
                    {
                        type = "战区";
                        arr[7] = (double)row.TOTAL_AMOUNT1;
                    }
                    else if (row.KIND_CODE.Equals("3"))
                    {
                        type = "国美线上";
                        arr[5] = (double)row.TOTAL_AMOUNT1;
                    }
                    var kpi = new KPiTotalModel()
                    {
                        type = type,
                        TOTAL_QTY = (double)row.TOTAL_QTY,
                        TOTAL_AMOUNT = (double)row.TOTAL_AMOUNT1,
                        TARGET_AMOUNT = (double)row.TARGET_AMOUNT,
                        TARGET_QTY = (double)row.TARGET_QTY
                    };
                    if (kpi.TARGET_QTY > 0)
                    {
                        kpi.TARGET_QTY_RATE = kpi.TOTAL_QTY / kpi.TARGET_QTY;

                    }
                    else
                    {
                        kpi.TARGET_QTY_RATE = 0;
                    }
                    if (kpi.TARGET_AMOUNT > 0)
                    {
                        kpi.TARGET_AMOUNT_RATE = kpi.TOTAL_AMOUNT / kpi.TARGET_AMOUNT;
                    }
                    else
                    {
                        kpi.TARGET_AMOUNT_RATE = 0;
                    }
                    list.Add(kpi);
                }
                data.Add("list", list);
                data.Add("arr", arr);
                
                data.Add("totalQty", list.Sum(p=>p.TOTAL_QTY));
                data.Add("totalTargetQty", list.Sum(p => p.TARGET_QTY));
                if (list.Sum(p => p.TARGET_QTY) > 0)
                {
                    data.Add("totalQty_Rate", list.Sum(p => p.TOTAL_QTY) / list.Sum(p => p.TARGET_QTY));
                }
                else
                {
                    data.Add("totalQty_Rate", 0);
                }
                data.Add("totalAmt", list.Sum(p => p.TOTAL_AMOUNT));
                data.Add("totalTargetAmt", list.Sum(p => p.TARGET_AMOUNT));

                if (list.Sum(p => p.TARGET_AMOUNT) > 0)
                {
                    data.Add("totalAmt_Rate", list.Sum(p => p.TOTAL_AMOUNT) / list.Sum(p => p.TARGET_AMOUNT));
                }
                else
                {
                    data.Add("totalAmt_Rate", 0);
                }




                return Json(data, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(ex.Message, JsonRequestBehavior.AllowGet);
            }
        }
        public ActionResult KPIIndex(string userId)
        {
            ViewData["userId"] = userId;
            return View("KPIView");
        }
        public PartialViewResult KPIMemuView()
        {
            return PartialView("_KPIMemuPartialPage", null);
        }

        public PartialViewResult KPIView1_2(string userId)
        {
            View1_1PModel model = new View1_1PModel();
            model.minDay = System.DateTime.Now.AddDays(-60).ToString("yyyy-MM-dd");
            model.MaxDay = System.DateTime.Now.ToString("yyyy-MM-dd");
            model.MonthList = getMonths();

            model.TBuList = getUserBu(userId, "3BU");
            return PartialView("_KPIPartialPage1_2", model);
        }
        private List<KeyValueModel> getUserBu(string userId,string def)
        {
            List<KeyValueModel> data = new List<KeyValueModel>();

            ORG_INFOTableAdapter ad = new ORG_INFOTableAdapter();
            var table = ad.GetKINDData();
            foreach (var row in table)
            {
                if (row.MANAGE_ORG_CODE.Equals(def))
                {
                    data.Add(new KeyValueModel() { key = row.MANAGE_ORG_CODE, keyValue = row.MANAGE_ORG_NAME, isSelected = true });
                }
                else
                {
                    data.Add(new KeyValueModel() { key = row.MANAGE_ORG_CODE, keyValue = row.MANAGE_ORG_NAME});
                }
            } 
            return data;
        }
        private List<KeyValueModel> getUserTBu(string userId,string kind)
        {
            ORG_INFOTableAdapter ad = new ORG_INFOTableAdapter();
           
            var ents = getTbus(kind);
            if (ents.Count > 0)
            {
                ents[0].isSelected = true;
            }
            return ents;
        }
        private List<KeyValueModel> getMonths()
        {
            List<KeyValueModel> data = new List<KeyValueModel>();
            data.Add(new KeyValueModel() { key = System.DateTime.Now.AddMonths(-2).ToString("yyyyMM"), keyValue = System.DateTime.Now.AddMonths(-2).ToString("yyyy/MM") });
            data.Add(new KeyValueModel() { key = System.DateTime.Now.AddMonths(-1).ToString("yyyyMM"), keyValue = System.DateTime.Now.AddMonths(-1).ToString("yyyy/MM") });
            data.Add(new KeyValueModel() { key = System.DateTime.Now.AddMonths(-0).ToString("yyyyMM"), keyValue = System.DateTime.Now.AddMonths(-0).ToString("yyyy/MM"), isSelected = true });
            return data;
        }
        public PartialViewResult KPIView1_3(string userId)
        {
            View1_3PModel model = new View1_3PModel();
            model.minDay = System.DateTime.Now.AddDays(-60).ToString("yyyy-MM-dd");
            model.MaxDay = System.DateTime.Now.ToString("yyyy-MM-dd");
            model.TBuList = getUserBu(userId, "1GM");
            model.TBuList2 =  getUserBu(userId, "3BU");
            model.TSBuList = getUserTBu(userId, "1GM");
            model.TSBuList2 = getUserTBu(userId, "3BU");
            model.userId = userId;


            return PartialView("_KPIPartialPage1_3", model);
        }
        public PartialViewResult KPIView2_1(string userId)
        {
            View1_3PModel model = new View1_3PModel();
            model.minDay = System.DateTime.Now.AddDays(-60).ToString("yyyy-MM-dd");
            model.MaxDay = System.DateTime.Now.ToString("yyyy-MM-dd");
            model.TBuList = getUserBu(userId, "3BU");



            return PartialView("_KPIPartialPage2_1", model);
        }
        public PartialViewResult KPIView3_1(string userId)
        {
            View1_1PModel model = new View1_1PModel();
            model.minDay = System.DateTime.Now.AddDays(-60).ToString("yyyy-MM-dd");
            model.MaxDay = System.DateTime.Now.ToString("yyyy-MM-dd");
            model.MonthList = getMonths();

            model.TBuList = getUserBu(userId, "3BU");



            return PartialView("_KPIPartialPage3_1", model);
        }
        public PartialViewResult KPIView1_1(string userId)
        {
            View1_1PModel model = new View1_1PModel();
            model.minDay = System.DateTime.Now.AddDays(-60).ToString("yyyy-MM-dd");
            model.MaxDay= System.DateTime.Now.ToString("yyyy-MM-dd");
            model.MonthList = getMonths();

                  model.TBuList = getUserBu(userId, "1GM");
            return PartialView("_KPIPartialPage1_1", model);
        }
        public PartialViewResult GetView13SubView(string bu, string tbu, string day,string type)
        {

            string startDay = System.DateTime.ParseExact(day, "yyyy-MM-dd", CultureInfo.CurrentCulture).ToString("yyyy-MM-01");
            string endDay = System.DateTime.ParseExact(System.DateTime.ParseExact(day, "yyyy-MM-dd", CultureInfo.CurrentCulture).AddMonths(1).ToString("yyyy-MM-01"),
                "yyyy-MM-dd", CultureInfo.CurrentCulture).AddDays(-1).ToString("yyyy-MM-dd");
            List<View1_3_3PModel> model = new List<View1_3_3PModel>();
            if (type.Equals("3"))
            {
                
                KPI133DataAdapter ad = new KPI133DataAdapter();
                var table = ad.GetDataByday(bu, tbu, startDay, endDay, day);
                foreach (var row in table)
                {
                    try
                    {
                        model.Add(new View1_3_3PModel()
                        {
                            text1 = row.CUSTOMER_NAME,
                            qty1 = row.IsSHOP_NUMNull() ? 0 : (double)row.SHOP_NUM,
                            qty2 = row.IsTOTAL_QTYNull() ? 0 : (double)row.TOTAL_QTY,
                            qty3 = row.IsMONTH_QTYNull() ? 0 : (double)row.MONTH_QTY,
                        }
                            );
                    }
                    catch { }
                       
                }
                return PartialView("_KPIPartialPage1_3_3", model.OrderByDescending(p => p.qty3).ToList());
            }
            else if (type.Equals("4"))
            {
                KPI134DataAdapter ad = new KPI134DataAdapter();
                var table = ad.GetDataByDay(startDay, endDay, bu, tbu, day);
                foreach (var row in table)
                {
                    model.Add(new View1_3_3PModel()
                    {
                        text1 = row.CH_SHOP,
                        qty1 = row.IsGUIDE_NUMNull() ? 0 : (double)row.GUIDE_NUM,
                        qty2 = row.IsTOTAL_QTYNull() ? 0 : (double)row.TOTAL_QTY,
                        qty3 = row.IsMONTH_QTYNull() ? 0 : (double)row.MONTH_QTY,
                    }
                        );
                }
                
                return PartialView("_KPIPartialPage1_3_4", model.OrderByDescending(p=>p.qty3).ToList());
            }
            else if (type.Equals("5"))
            {
                KPI135DataAdapter ad = new KPI135DataAdapter();
                var table = ad.GetDataByDay(startDay, endDay, bu, tbu, day);
                foreach (var row in table)
                {
                    model.Add(new View1_3_3PModel()
                    {
                        text1 = row.MACHINE_MODEL_NO,
                        qty2 = row.IsTOTAL_QTYNull() ? 0 : (double)row.TOTAL_QTY,
                        qty3 = row.IsMONTH_QTYNull() ? 0 : (double)row.MONTH_QTY,
                    }
                        );
                }
                return PartialView("_KPIPartialPage1_3_5", model.OrderByDescending(p => p.qty3).ToList());
            }
            return PartialView("_KPIPartialPage1_3_3", model);
        }
        public JsonResult GetView21DataJson(string bu)
        {
            try
            {
                KPI21DataAdapter ad = new KPI21DataAdapter();
                var table = ad.GetDataByDay(bu);
                List<String> tbus = new List<string>();
                List<Double> rValue = new List<Double>();
                List<Double> tValue = new List<Double>();
                List<descModel> rows = new List<descModel>();
                foreach (var row in table)
                {
                    double value = 0;
                    double t = 0;
                    //Type.Add(row.T_BUNAME);
                    if (!row.IsSHOP_TARGETNull() && row.SHOP_TARGET > 0)
                    {
                        value = (double)((row.IsSHOP_NUMNull()?0:row.SHOP_NUM) / row.SHOP_TARGET) * 100;
                        t = (double)row.SHOP_NUM;
                        //Value.Add((double)(row.TOTAL_QTY/row.TARGET_QTY) *100);
                    }
                    rows.Add(new descModel() { key = row.T_BUNAME, value1 = double.Parse(value.ToString("0.0")), value2 = double.Parse(t.ToString("0")) });
                    //tbus.Add(row.T_BUNAME);
                    //rValue.Add(double.Parse(value.ToString("0.0")));
                    //tValue.Add(double.Parse(t.ToString("0")));
                }
                foreach (var item in rows.OrderBy(p=>p.value1))
                {
                    tbus.Add(item.key);
                    rValue.Add(item.value1);
                    tValue.Add(item.value2);
                }
                Hashtable data = new Hashtable();
                data.Add("tbus", tbus);
                data.Add("rvalue", rValue);
                data.Add("tvalue", tValue);

                return Json(data, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json("{message:'" + ex.Message + "'}", JsonRequestBehavior.AllowGet);
            }
        }
        public JsonResult GetView132DataJson(string bu, string tbu,string day)
        {
            try
            {
                string startDay = System.DateTime.ParseExact(day,"yyyy-MM-dd", CultureInfo.CurrentCulture).ToString("yyyy-MM-01");
                string endDay = System.DateTime.ParseExact(System.DateTime.ParseExact(day, "yyyy-MM-dd", CultureInfo.CurrentCulture).AddMonths(1).ToString("yyyy-MM-01"),
                    "yyyy-MM-dd", CultureInfo.CurrentCulture).AddDays(-1).ToString("yyyy-MM-dd");
                KPI132DataAdapter ad = new KPI132DataAdapter();
               var table= ad.GetDataByDay(startDay, endDay, bu, tbu, day);
                List<String> ttype = new List<string>();
                List<Double> dvalue = new List<Double>();
                List<Double> mvalue = new List<Double>();
                foreach (var row in table)
                {
                    ttype.Add(row.T_TYPENAME);
                    dvalue.Add(row.IsTOTAL_QTYNull()?0:double.Parse(row.TOTAL_QTY.ToString("0.0")));
                    mvalue.Add(row.IsMONTH_QTYNull() ? 0 : double.Parse(row.MONTH_QTY.ToString("0.0")));
                }
                Hashtable data = new Hashtable();
                data.Add("ttype", ttype);
                data.Add("dvalue", dvalue);
                data.Add("mvalue", mvalue);

                return Json(data, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json("{message:'" + ex.Message + "'}", JsonRequestBehavior.AllowGet);
            }
        }
        public JsonResult GetTBuData(string kind,string userId)
        {
            ORG_INFOTableAdapter ad = new ORG_INFOTableAdapter();
            var ents = getTbus(kind);
            if (ents.Count > 0)
            {
                ents[0].isSelected = true;
            }
            return Json(ents, JsonRequestBehavior.AllowGet);
        }
        public List<KeyValueModel> getTbus(string kind)
        {
            List<KeyValueModel> ents = new List<KeyValueModel>();
            V_SALES_KIND_BUTableAdapter ad = new V_SALES_KIND_BUTableAdapter();
            var table =ad.GetData(kind);
            foreach (var row in table)
            {
                
                    ents.Add(new KeyValueModel() { key = row.BU_CODE, keyValue = row.BU_NAME });
                
            }
            return ents;
        }


        public JsonResult GetView131DataJson(string bu, string tbu)
        {
            try
            {
                DateTime startDay = System.DateTime.Now.AddDays(-6);
            DateTime endDay = System.DateTime.Now;
            KPI13DataAdapter ad = new KPI13DataAdapter();
            var table = ad.GetDataByDate(startDay.ToString("yyyy-MM-dd"), endDay.ToString("yyyy-MM-dd"), bu, tbu);

               // var table = ad.GetDataBy(startDay.ToString("2018-03-"), endDay.ToString("yyyy-MM-dd"));
                List<String> days = new List<string>();
            List<Double> rValue = new List<Double>();
            List<Double> tValue = new List<Double>();
                //return Json("{rows:'"+ table .Count+ "'}", JsonRequestBehavior.AllowGet);
                foreach (var row in table)
            {
                double value = 0;
                double t = 0;
                //Type.Add(row.T_BUNAME);
                if (!row.IsTARGET_QTYNull() && row.TARGET_QTY > 0)
                {
                    value = (double)(row.TOTAL_QTY / row.TARGET_QTY) * 100;
                    t = (double)row.TARGET_QTY;
                    //Value.Add((double)(row.TOTAL_QTY/row.TARGET_QTY) *100);
                }
                days.Add(row.SALES_DATE.ToString("M/d"));
                rValue.Add(double.Parse(value.ToString("0.0")));
                    tValue.Add(double.Parse(t.ToString("0")));
            }
            Hashtable data = new Hashtable();
            data.Add("day", days);
            data.Add("rvalue", rValue);
            data.Add("tvalue", tValue);

            return Json(data, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json("{message:'" + ex.Message + "'}", JsonRequestBehavior.AllowGet);
            }
        }
        

        public JsonResult GetView2DataJson(string month, string day, string bu, string type)
        {
            try
            {
                string startDay = day;
                string endDay = day;
                if (type.Equals("2"))
                {
                    DateTime monthDay = DateTime.ParseExact(month, "yyyyMM", CultureInfo.CurrentCulture);
                    startDay = monthDay.ToString("yyyy-MM-01");
                    endDay = DateTime.ParseExact(monthDay.AddMonths(1).ToString("yyyy-MM-01"), "yyyy-MM-dd", CultureInfo.CurrentCulture).AddDays(-1).ToString("yyyy-MM-dd");
                }

                KPI1DataTableAdapter ad = new KPI1DataTableAdapter();
                var table = ad.GetDataByDate(startDay, endDay, bu);
                Hashtable data = new Hashtable();
                List<String> Type = new List<string>();
                List<Double> Value = new List<Double>();
                List<Double> TValue = new List<Double>();
                List<descModel> rows = new List<descModel>();
                foreach (var row in table)
                {


                    if (!row.IsT_BUIDNull() && !row.T_BUID.Equals(""))
                    {
                        double value = 0;
                        //Type.Add(row.T_BUNAME);
                        if (!row.IsTARGET_QTYNull() && row.TARGET_QTY > 0)
                        {
                            value = (double)(row.TOTAL_QTY / row.TARGET_QTY) * 100;
                            //Value.Add((double)(row.TOTAL_QTY/row.TARGET_QTY) *100);
                        }

                        //eyValuePair<string, double> temp = new KeyValuePair<string, double>(row.T_BUNAME, value);


                        rows.Add(new descModel() { key = row.T_BUNAME, value1 = (double)row.TOTAL_QTY, value2 = double.Parse(value.ToString("0.0")) });
                    }

                }
                foreach (var item in rows.OrderBy(p => p.value2))
                {
                    Type.Add(item.key);
                    Value.Add(item.value2);
                    TValue.Add(item.value1);
                }
                data.Add("type", Type);
                data.Add("value", Value);
                data.Add("tvalue", TValue);
                return Json(data, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json("{message:'" + ex.Message + "'}", JsonRequestBehavior.AllowGet);
            }


        }

        public JsonResult GetView31DataJson(string month, string day, string bu, string type)
        {
            try
            {
                string startDay = day;

                if (type.Equals("2"))
                {
                    DateTime monthDay = DateTime.ParseExact(month, "yyyyMM", CultureInfo.CurrentCulture);
                    startDay = monthDay.ToString("yyyy/MM/01");
                    //endDay = DateTime.ParseExact(monthDay.AddMonths(1).ToString("yyyy-MM-01"), "yyyy-MM-dd", CultureInfo.CurrentCulture).AddDays(-1).ToString("yyyy-MM-dd");
                }
                else
                {
                    DateTime monthDay = DateTime.ParseExact(day, "yyyy-MM-dd", CultureInfo.CurrentCulture);
                    startDay = monthDay.ToString("yyyy/MM/dd");
                }

                SP_SALES_KPI31TableAdapter ad = new SP_SALES_KPI31TableAdapter();
                Object obj = new object();
                var table = ad.GetData(startDay, bu, out obj);
                Hashtable data = new Hashtable();
                List<String> Type = new List<string>();
                List<Double> Value = new List<Double>();
                List<Double> TValue = new List<Double>();
                List<descModel> rows = new List<descModel>();
                foreach (var row in table)
                {




                    //Type.Add(row.T_BUNAME);
                    //Value.Add(double.Parse(value.ToString("0.0")));
                    //TValue.Add((double)row.TOTAL_QTY);
                    //KeyValuePair<string, double> temp = new KeyValuePair<string, double>(row.T_BUNAME, value);

                    if (type.Equals("2"))
                    {
                        rows.Add(new descModel() { key = row.T_BUNAME, value1 = (double)row.MONTH_CARDQTY, value2 = double.Parse(row.MONTH_CARDRATE.ToString("0.0")) });
                    }
                    else
                    {
                        rows.Add(new descModel() { key = row.T_BUNAME, value1 = (double)row.TODAY_CARDQTY, value2 = double.Parse(row.TODAY_CARDRATE.ToString("0.0")) });
                    }


                }

                foreach (var item in rows.OrderBy(p => p.value2))
                {
                    Type.Add(item.key);
                    Value.Add(item.value2);
                    TValue.Add(item.value1);
                }

                data.Add("type", Type);
                data.Add("value", Value);
                data.Add("tvalue", TValue);

                return Json(data, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json("{message:'" + ex.Message + "'}", JsonRequestBehavior.AllowGet);
            }
        }

        public JsonResult GetView1DataJson(string month,string day,string bu,string type)
        {
            try
            {
                string startDay = day;
                string endDay = day;
                if (type.Equals("2"))
                {
                    DateTime monthDay = DateTime.ParseExact(month, "yyyyMM", CultureInfo.CurrentCulture);
                    startDay = monthDay.ToString("yyyy-MM-01");
                    endDay = DateTime.ParseExact(monthDay.AddMonths(1).ToString("yyyy-MM-01"), "yyyy-MM-dd", CultureInfo.CurrentCulture).AddDays(-1).ToString("yyyy-MM-dd");
                }

                KPI1DataTableAdapter ad = new KPI1DataTableAdapter();
                var table = ad.GetDataByDate(startDay, endDay, bu);
                Hashtable data = new Hashtable();
                List<String> Type = new List<string>();
                List<Double> Value = new List<Double>();
                List<Double> TValue = new List<Double>();
                List<Double> Value3 = new List<Double>();
                List<Double> Value4 = new List<Double>();
                List<descModel> rows = new List<descModel>();
                foreach (var row in table)
                {

                    
                    if (!row.IsT_BUIDNull() && !row.T_BUID.Equals(""))
                    {
                        double value = 0;
                        double value2 = 0;
                        //Type.Add(row.T_BUNAME);
                        if (!row.IsTARGET_QTYNull() && row.TARGET_QTY > 0)
                        {
                            value = (double)(row.TOTAL_QTY / row.TARGET_QTY) * 100;
                            //Value.Add((double)(row.TOTAL_QTY/row.TARGET_QTY) *100);
                        }
                        if (!row.IsTARGET_AMOUNTNull() && row.TARGET_AMOUNT > 0)
                        {
                            value2 = (double)(row.TOTAL_AMOUNT / row.TARGET_AMOUNT) * 100;
                            //Value.Add((double)(row.TOTAL_QTY/row.TARGET_QTY) *100);
                        }
                        //Type.Add(row.T_BUNAME);
                        //Value.Add(double.Parse(value.ToString("0.0")));
                        //TValue.Add((double)row.TOTAL_QTY);
                        //KeyValuePair<string, double> temp = new KeyValuePair<string, double>(row.T_BUNAME, value);


                        rows.Add(new descModel() { key= row.T_BUNAME,value1= (double)row.TOTAL_QTY,value2= double.Parse(value.ToString("0.0")),
                         value3= (double)row.TOTAL_AMOUNT, value4= double.Parse(value2.ToString("0.0"))
                        });
                    }
                    
                }
                
                foreach (var item in rows.OrderBy(p => p.value2))
                {
                    Type.Add(item.key);
                    Value.Add(item.value2);
                    TValue.Add(item.value1);
                    Value3.Add(item.value3);
                    Value4.Add(item.value4);
                }
               
                data.Add("type", Type);
                data.Add("value", Value);
                data.Add("tvalue", TValue);
                data.Add("value3", Value3);
                data.Add("value4", Value4);

                return Json(data, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json("{message:'"+ ex .Message+ "'}", JsonRequestBehavior.AllowGet);
            }
            

        }
        

    }
}