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
    public class seriesModel
    {
        public string name { get; set; }
        public List<int> data { get; set; }
        public string type { get; set; }
        public int yAxisIndex { get; set; }

    }
    public class popModel2
    {
        public string empName { get; set; }
        public string empCode { get; set; }
        public int data1 { get; set; }
        public int data2 { get; set; }
        public int data3 { get; set; }
        public int data4 { get; set; }
    }
    public class orderMode
    {
        public string orderNo { get; set; }
        public string orderDate { get; set; }
        public int pCount { get; set; }
        public double amount { get; set; }
        public string landingCode { get; set; }
        public string name { get; set; }
        public string phoneNumber { get; set; }
       public string status { get; set; }
        public List<orderModelDetail> orderModelDetail { get; set; }

    }
    public class orderModelDetail
    {
        public string pNmae { get; set; }
        public int qty { get; set; }
        public double price { get; set; }
        public double amount { get; set; }
    }

        public class HKPIController : Controller
    {
        public ActionResult HKPIOrderSubIndex(string order)
        {
            ViewData["order"] = order;
            
            
                return View("HKPIOrder_subView");

        }
        public ActionResult HKPISalesStateSubIndex(string empCode, string shopCode)
        {
            ViewData["empCode"] = empCode;
            ViewData["shopCode"] = shopCode;
           
                return View("HKPISalesStateView_sub");
           
        }
        public ActionResult HKPIIndex(string userId,string shopCode,string kpi)
        {
            ViewData["userId"] = userId;
            ViewData["shopCode"] = shopCode;
            if (kpi == null)
            {
                return View("HKPIView");
            }
            else if (kpi.Equals("1"))
            {
                return View("HKPIView");
            }
            else if (kpi.Equals("2"))
            {
                return View("HKPISalesView");
            }
            else if (kpi.Equals("3"))
            {
                return View("HKPIMemberView");
            }
            else if (kpi.Equals("4"))
            {
                return View("HKPIShopView");
            }
            else if (kpi.Equals("5"))
            {
                return View("HKPISalesStateView");
            }
            else if (kpi.Equals("6"))
            {
                return View("HKPISubSalesView");
            }
            else if (kpi.Equals("7"))
            {
                return View("HKPIBackMemberView");
            }
            else if (kpi.Equals("8"))
            {
                return View("HKPIOrderView");
            }
            else if (kpi.Equals("9"))
            {
                return View("HKPIOrderReport");
            }
            else
            {
                return View("HKPIView");
            }
        }
        public PartialViewResult DateSelectView()
        {
           
            return PartialView("_HKPIDateSelect", null);
        }
        public JsonResult SeachOrderData(string shopCode, string phone)
        {
            try
            {

                Hashtable data = new Hashtable();
                List<orderMode> orders = new List<orderMode>();
                V_SHOP_YHJ_SALES_HEADTableAdapter ad = new V_SHOP_YHJ_SALES_HEADTableAdapter();
                V_SHOP_YHJ_SALES_DETAILTableAdapter ad_sub = new V_SHOP_YHJ_SALES_DETAILTableAdapter();
                var mTable = ad.GetDataByPhone(shopCode, phone);
                if (mTable.Rows.Count > 0)
                {
                    foreach (var row in mTable)
                    {
                        orderMode order = new orderMode()
                        {
                            landingCode = row.LADING_CODE,
                            name = row.CUSTOMER_NAME,
                            orderDate = row.ORDER_DATE.ToString("yy/MM/dd"),
                            orderNo = row.ORDER_NO,
                            phoneNumber = row.CUSTOMER_MOBILE,
                            amount = (double)row.ORDER_AMT,
                            pCount = (int)row.SALES_QTY, status=row.ORDER_STATUS_NAME
                        };
                        orders.Add(order);
                        /*var s_table = ad_sub.GetDataByOrderNo(row.ORDER_NO);
                        if (s_table.Rows.Count > 0)
                        {
                            order.pCount = (int)s_table.Sum(p => p.SALES_QTY);
                        }
                        else
                        {
                            order.pCount = 0;
                         }
                         */
                    }
                }

                data.Add("orders", orders);



                return Json(data, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(ex.Message, JsonRequestBehavior.AllowGet);
            }
        }
        public JsonResult GetOrderDetail(string orderNo)
        {
            try
            {

                Hashtable data = new Hashtable();
                orderMode order = new orderMode();
                V_SHOP_YHJ_SALES_HEADTableAdapter ad = new V_SHOP_YHJ_SALES_HEADTableAdapter();
                V_SHOP_YHJ_SALES_DETAILTableAdapter ad_sub = new V_SHOP_YHJ_SALES_DETAILTableAdapter();
                var mTable = ad.GetDataByOrder(orderNo);
                if (mTable.Rows.Count > 0)
                {
                    var row = mTable.First();
                   
                         order = new orderMode()
                        {
                            landingCode = row.LADING_CODE,
                            name = row.CUSTOMER_NAME,
                            orderDate = row.ORDER_DATE.ToString("yy/MM/dd"),
                            orderNo = row.ORDER_NO,
                            phoneNumber = row.CUSTOMER_MOBILE,
                            amount = (double)row.ORDER_AMT,
                            pCount = (int)row.SALES_QTY,
                             status =row.ORDER_STATUS_NAME
                            
                        };
                    order.orderModelDetail = new List<orderModelDetail>();
                    var s_table = ad_sub.GetDataByOrderNo(row.ORDER_NO);
                    foreach (var dRow in s_table)
                    {
                        order.orderModelDetail.Add(new orderModelDetail() {
                             pNmae=dRow.PROD_NAME, qty=(int)dRow.SALES_QTY, price=(double)dRow.SALES_PRICE, amount=(double)(dRow.SALES_QTY*dRow.SALES_PRICE)
                        });
                    }
                    /*var s_table = ad_sub.GetDataByOrderNo(row.ORDER_NO);
                    if (s_table.Rows.Count > 0)
                    {
                        order.pCount = (int)s_table.Sum(p => p.SALES_QTY);
                    }
                    else
                    {
                        order.pCount = 0;
                     }
                     */

                }

                data.Add("order", order);



                return Json(data, JsonRequestBehavior.AllowGet);
            
            }
            catch (Exception ex)
            {
                return Json(ex.Message, JsonRequestBehavior.AllowGet);
    }
}
public JsonResult GetOrderData(string shopCode,int page)
        {
            try
            {
                
                Hashtable data = new Hashtable();
                List<orderMode> orders = new List<orderMode>();
                V_SHOP_YHJ_SALES_HEADTableAdapter ad = new V_SHOP_YHJ_SALES_HEADTableAdapter();
                V_SHOP_YHJ_SALES_DETAILTableAdapter ad_sub = new V_SHOP_YHJ_SALES_DETAILTableAdapter();
                var mTable = ad.GetDataShop(shopCode, page);
                //var mTable = ad.GetData();
                if (mTable.Rows.Count > 0)
                {
                    foreach (var row in mTable)
                    {
                        orderMode order = new orderMode() { landingCode = row.IsLADING_CODENull()?"":row.LADING_CODE, name = row.IsCUSTOMER_NAMENull()?"":row.CUSTOMER_NAME,
                            orderDate = row.ORDER_DATE.ToString("yy/MM/dd"), orderNo = row.ORDER_NO,
                            phoneNumber = row.IsCUSTOMER_MOBILENull()?"": row.CUSTOMER_MOBILE, amount=(double)row.ORDER_AMT, pCount=(int)row.SALES_QTY,
                            status = row.ORDER_STATUS_NAME
                        };
                        orders.Add(order);
                        /*var s_table = ad_sub.GetDataByOrderNo(row.ORDER_NO);
                        if (s_table.Rows.Count > 0)
                        {
                            order.pCount = (int)s_table.Sum(p => p.SALES_QTY);
                        }
                        else
                        {
                            order.pCount = 0;
                         }
                         */
                    }
                }

                data.Add("orders", orders);
                


                return Json(data, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(ex.Message, JsonRequestBehavior.AllowGet);
            }
        }

        public JsonResult GetShopQueryData(int model,string shopCode)
        {
            try
            {
                DateTime startDay = DateTime.ParseExact(DateTime.Now.ToString("yyyy-MM-dd") + " 00:00", "yyyy-MM-dd HH:mm", CultureInfo.CurrentCulture);
                DateTime endDay = DateTime.ParseExact(DateTime.Now.ToString("yyyy-MM-dd") + " 23:59", "yyyy-MM-dd HH:mm", CultureInfo.CurrentCulture);
                if (model == 0)
                { }
                else if (model == 1)//昨天
                {
                    startDay = startDay.AddDays(-1);
                    endDay = endDay.AddDays(-1);
                }
                else if (model == 2)//本周
                {
                    startDay = DateTime.ParseExact(this.getWeekStartDay(startDay).ToString("yyyy-MM-dd") + " 00:00", "yyyy-MM-dd HH:mm", CultureInfo.CurrentCulture);
                    endDay = DateTime.ParseExact(this.getWeekEndDay(startDay).ToString("yyyy-MM-dd") + " 23:59", "yyyy-MM-dd HH:mm", CultureInfo.CurrentCulture);
                }
                else if (model == 3)//本月
                {
                    startDay = DateTime.ParseExact(startDay.ToString("yyyy-MM") + "-01 00:00", "yyyy-MM-dd HH:mm", CultureInfo.CurrentCulture);
                    endDay = DateTime.ParseExact(startDay.AddMonths(1).AddDays(-1).ToString("yyyy-MM-dd") + " 23:59", "yyyy-MM-dd HH:mm", CultureInfo.CurrentCulture);
                }
                else if (model == 4)//本季度
                {
                    int q= startDay.Month / 3 + (startDay.Month % 3 > 0 ? 1 : 0);
                    if (q == 1)
                    {
                        startDay = DateTime.ParseExact(startDay.ToString("yyyy") + "-01-01 00:00", "yyyy-MM-dd HH:mm", CultureInfo.CurrentCulture);
                        endDay = DateTime.ParseExact(startDay.ToString("yyyy") + "-03-31 23:59", "yyyy-MM-dd HH:mm", CultureInfo.CurrentCulture);

                    }
                    if (q == 2)
                    {
                        startDay = DateTime.ParseExact(startDay.ToString("yyyy") + "-04-01 00:00", "yyyy-MM-dd HH:mm", CultureInfo.CurrentCulture);
                        endDay = DateTime.ParseExact(startDay.ToString("yyyy") + "-06-30 23:59", "yyyy-MM-dd HH:mm", CultureInfo.CurrentCulture);

                    }
                    if (q == 3)
                    {
                        startDay = DateTime.ParseExact(startDay.ToString("yyyy") + "-07-01 00:00", "yyyy-MM-dd HH:mm", CultureInfo.CurrentCulture);
                        endDay = DateTime.ParseExact(startDay.ToString("yyyy") + "-09-30 23:59", "yyyy-MM-dd HH:mm", CultureInfo.CurrentCulture);

                    }
                    if (q == 4)
                    {
                        startDay = DateTime.ParseExact(startDay.ToString("yyyy") + "-10-01 00:00", "yyyy-MM-dd HH:mm", CultureInfo.CurrentCulture);
                        endDay = DateTime.ParseExact(startDay.ToString("yyyy") + "-12-31 23:59", "yyyy-MM-dd HH:mm", CultureInfo.CurrentCulture);

                    }
                }
                else if (model == 5)//7天
                {
                    startDay = startDay.AddDays(-7);
                   
                }
                else if (model == 6)//30天
                {
                    startDay = startDay.AddDays(-30);
                   
                }
                else if (model == 7)//90天
                {
                    startDay = startDay.AddDays(-90);

                }
                Hashtable data = new Hashtable();

                //本店人数
                marketSalesApp salesApp = new marketSalesApp();
                int popCount = salesApp.getPopCount(shopCode);
                data.Add("data1", popCount);

                //本店人效
                QuerySalesTotalTableAdapter ad = new QuerySalesTotalTableAdapter();
                var SalesTotal = ad.GetData(startDay, endDay, shopCode);
                if (popCount == 0)
                {
                    data.Add("data2", 0);
                }
                else
                {
                    data.Add("data2", SalesTotal.Sum(p => p.AMT)/ popCount);
                }
                //本店坪数
                shopADTableTableAdapter ad2 = new shopADTableTableAdapter();
                var table2 = ad2.GetData(shopCode);
                decimal ADQty = 0;
                
                if (table2.Rows.Count > 0)
                {
                    ADQty = table2.First().AD;
                }
                data.Add("data3", ADQty);
                if (ADQty == 0)
                {
                    data.Add("data4", 0);
                }
                else
                {
                    data.Add("data4", SalesTotal.Sum(p => p.AMT) / ADQty);
                }
                //客流量
                DataTableQueryMemberVisitAdapter ad3 = new DataTableQueryMemberVisitAdapter();
                var MemberCount = ad3.GetData(startDay, endDay, shopCode);
                data.Add("data5", MemberCount.Sum(p=>p.CT));

                //订单数
                QuerySalesCountTableAdapter ad4 = new QuerySalesCountTableAdapter();
                var SalesCount = ad4.GetData(startDay, endDay, shopCode);
                data.Add("data6", SalesCount.Sum(p=>p.CT));
                //商品数
                // marketMachineChannelApp macApp = new marketMachineChannelApp();
               // QuerySalesModelListTableAdapter macAd = new QuerySalesModelListTableAdapter();
                DataTableQuerySalesAdapter macAd = new DataTableQuerySalesAdapter();
                int macCount = 0;
                var macCtTable=    macAd.GetData(startDay, endDay, shopCode);
                if (macCtTable.Rows.Count > 0)
                {
                    macCount = (int)macCtTable.Sum(p => p.QTY);
                }
                data.Add("data7", macCount);
                //交易额
                data.Add("data8", SalesTotal.Sum(p=>p.AMT));
                //成交率
                if (MemberCount.Sum(p => p.CT) == 0)
                {
                    data.Add("data9", 1);
                }
                else
                {
                    data.Add("data9", SalesCount.Sum(p => p.CT)/ MemberCount.Sum(p => p.CT));
                }
                //连带率
                if (SalesCount.Sum(p => p.CT) == 0)
                {
                    data.Add("data10", 0);
                }
                else
                {
                    data.Add("data10", macCount/ SalesCount.Sum(p => p.CT));
                }
                //件单价
                if (macCount == 0)
                {
                    data.Add("data11", 0);
                }
                else
                {
                    data.Add("data11", SalesTotal.Sum(p => p.AMT) / macCount);
                }
                //客单价
                if (SalesCount.Sum(p => p.CT) == 0)
                {
                    data.Add("data12", 0);
                }
                else
                {
                    data.Add("data12", SalesTotal.Sum(p => p.AMT) / SalesCount.Sum(p => p.CT));
                }
               



                return Json(data, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(ex.Message, JsonRequestBehavior.AllowGet);
            }
        }
        public JsonResult GetSubSalesQueryData(DateTime startTime, DateTime endTime, string shopCode, int model)
        {
            try
            {
                List<String> timeGroup = new List<string>();
                timeGroup.Add("00:00");
                timeGroup.Add("01:00");
                timeGroup.Add("02:00");
                timeGroup.Add("03:00");
                timeGroup.Add("04:00");
                timeGroup.Add("05:00");
                timeGroup.Add("06:00");
                timeGroup.Add("07:00");
                timeGroup.Add("08:00");
                timeGroup.Add("09:00");
                timeGroup.Add("10:00");
                timeGroup.Add("11:00");
                timeGroup.Add("12:00");
                timeGroup.Add("13:00");
                timeGroup.Add("14:00");
                timeGroup.Add("15:00");
                timeGroup.Add("16:00");
                timeGroup.Add("17:00");
                timeGroup.Add("18:00");
                timeGroup.Add("19:00");
                timeGroup.Add("20:00");
                timeGroup.Add("21:00");
                timeGroup.Add("22:00");
                timeGroup.Add("23:00");
                List<String> timeGroupName = new List<string>();
                timeGroupName.Add("0点");
                timeGroupName.Add("1点");
                timeGroupName.Add("2点");
                timeGroupName.Add("3点");
                timeGroupName.Add("4点");
                timeGroupName.Add("5点");
                timeGroupName.Add("6点");
                timeGroupName.Add("7点");
                timeGroupName.Add("8点");
                timeGroupName.Add("9点");
                timeGroupName.Add("10点");
                timeGroupName.Add("11点");
                timeGroupName.Add("12点");
                timeGroupName.Add("13点");
                timeGroupName.Add("14点");
                timeGroupName.Add("15点");
                timeGroupName.Add("16点");
                timeGroupName.Add("17点");
                timeGroupName.Add("18点");
                timeGroupName.Add("19点");
                timeGroupName.Add("20点");
                timeGroupName.Add("21点");
                timeGroupName.Add("22点");
                timeGroupName.Add("23点");

                Random rd = new Random();

                DateTime startDay = DateTime.ParseExact(startTime.ToString("yyyy-MM-dd") + " 00:00", "yyyy-MM-dd HH:mm", CultureInfo.CurrentCulture);
                DateTime endDay = DateTime.ParseExact(endTime.ToString("yyyy-MM-dd") + " 23:59", "yyyy-MM-dd HH:mm", CultureInfo.CurrentCulture);
                QuerySubSalesTotalTableAdapter ad = new QuerySubSalesTotalTableAdapter();
                List<string> legend = new List<string>();
                List<seriesModel> seriesQty = new List<seriesModel>();
                List<seriesModel> seriesAmount = new List<seriesModel>();
                marketSalesApp salesApp = new marketSalesApp();
                var table = ad.GetData(startDay, endDay, shopCode);
                
                var pop = salesApp.getPop(shopCode);

                
                    

                    
                    foreach (var row in pop)
                    {

                        string name = row.sales_Name;
                        legend.Add(name);
                        var dataA = new seriesModel() { name = name, type = "bar", yAxisIndex = 0 };
                        dataA.data = new List<int>();

                        var dataQ = new seriesModel() { name = name, type = "bar", yAxisIndex = 0 };
                        dataQ.data = new List<int>();

                    if (table.Rows.Count > 0)
                    {
                        foreach (var item in timeGroup)
                        {
                            var subRows = table.Where(p => p.EMPLOYEE_ID.Equals(row.sales_No) && p.SALESDATE.Equals(item.Substring(0, 2)));
                            if (subRows.Count() > 0)
                            {
                                dataA.data.Add((int)subRows.First().AMT);
                                dataQ.data.Add((int)subRows.First().QTY);
                            }
                            else
                            {
                                dataA.data.Add(0);
                                dataQ.data.Add(0);
                            }
                        }
                    }
                       
                        seriesQty.Add(dataQ);
                        seriesAmount.Add(dataA);

                    }
                

                Hashtable data = new Hashtable();
                data.Add("timeGroupName", timeGroupName);
                data.Add("timeGroup", timeGroup);
                data.Add("legend", legend);
                data.Add("seriesAmount", seriesAmount);
                data.Add("seriesQty", seriesQty);

                return Json(data, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(ex.Message, JsonRequestBehavior.AllowGet);
            }
        }
        public JsonResult GetSalesStateSubQueryData(string shopCode,string empCode)
        {
            try
            {
                DateTime startDay = DateTime.ParseExact(DateTime.Now.ToString("yyyy-MM-dd") + " 00:00", "yyyy-MM-dd HH:mm", CultureInfo.CurrentCulture);
            DateTime endDay = DateTime.ParseExact(DateTime.Now.ToString("yyyy-MM-dd") + " 23:59", "yyyy-MM-dd HH:mm", CultureInfo.CurrentCulture);
            Hashtable data = new Hashtable();
            //姓名
            marketSalesApp salesApp = new marketSalesApp();
            var userinfo = salesApp.getUserInfoBySalesNo(empCode);
            if (userinfo != null)
            {
                data.Add("data1", userinfo.Name);
            }
            else
            {
                data.Add("data1", "未知");
            }
                QuerySubSalesTotalTableAdapter salesTotalAd = new QuerySubSalesTotalTableAdapter();
                //昨日销售额
                var salesEmp = salesTotalAd.GetDataByEmpCode(startDay.AddDays(-1), endDay.AddDays(-1), shopCode,empCode);
            if (salesEmp.Rows.Count > 0)
            {
                data.Add("data3", salesEmp.Sum(p => p.AMT));
            }
            else
            {
                data.Add("data3", 0);
            }
            //本月销售额
           var startDay_month = DateTime.ParseExact(startDay.ToString("yyyy-MM") + "-01 00:00", "yyyy-MM-dd HH:mm", CultureInfo.CurrentCulture);
           var endDay_month = DateTime.ParseExact(startDay.AddMonths(1).AddDays(-1).ToString("yyyy-MM-dd") + " 23:59", "yyyy-MM-dd HH:mm", CultureInfo.CurrentCulture);
            
             salesEmp = salesTotalAd.GetDataByEmpCode(startDay_month, endDay_month, shopCode, empCode);
            if (salesEmp.Rows.Count > 0)
            {
                data.Add("data4", salesEmp.Sum(p => p.AMT));
                data.Add("data6", salesEmp.Sum(p => p.AMT));
            }
            else
            {
                data.Add("data4", 0);
                data.Add("data6", 0);
            }
                //排名
               
                var salesTotal = salesTotalAd.GetData(startDay_month, endDay_month, shopCode);
                int reck = 0;
                if (salesTotal.Rows.Count > 0)
                {
                    int tempreck = 0;
                    foreach (var salesItem in salesTotal.OrderByDescending(p => p.AMT))
                    {
                        tempreck = tempreck + 1;
                        if (salesItem.EMPLOYEE_ID.Equals(empCode))
                        {
                            reck = tempreck;
                            break;
                        }
                    }
                }

                if (reck > 0)
                {
                    data.Add("data2", "本店销售额 第 " + reck + " 名");
                }
                else
                {
                    data.Add("data2", "暂无名次");
                }
                //本月月均订单数
                QuerySalesCountTableAdapter countAd = new QuerySalesCountTableAdapter();
            var salesOrderCount= countAd.GetDataByEmpCode(startDay_month, endDay_month, shopCode, empCode);
            if (salesOrderCount.Rows.Count > 0)
            {
                data.Add("data7", salesOrderCount.Sum(p => p.CT)/30);
                    //客单价
                    if (salesOrderCount.Sum(p => p.CT) != 0)
                    {
                        data.Add("data8", salesEmp.Sum(p => p.AMT) / salesOrderCount.Sum(p => p.CT));
                    }
                    else
                    {
                        data.Add("data8", 0);
                    }
               

            }
            else
            {
                data.Add("data7", 0);
                
            }
            

            //上月销售额
            startDay_month = DateTime.ParseExact(startDay.AddMonths(-1).ToString("yyyy-MM") + "-01 00:00", "yyyy-MM-dd HH:mm", CultureInfo.CurrentCulture);
             endDay_month = DateTime.ParseExact(startDay.AddMonths(-1).AddMonths(1).AddDays(-1).ToString("yyyy-MM-dd") + " 23:59", "yyyy-MM-dd HH:mm", CultureInfo.CurrentCulture);
            
            salesEmp = salesTotalAd.GetDataByEmpCode(startDay_month, endDay_month, shopCode, empCode);
            if (salesEmp.Rows.Count > 0)
            {
                data.Add("data5", salesEmp.Sum(p => p.AMT));

            }
            else
            {
                data.Add("data5", 0);
            }
            data.Add("data9", 1);
                return Json(data, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(ex.Message, JsonRequestBehavior.AllowGet);
            }
        }
        public JsonResult GetSalesStateQueryData(string shopCode)
        {
            try
            {
                DateTime startDay = DateTime.ParseExact(DateTime.Now.ToString("yyyy-MM-dd") + " 00:00", "yyyy-MM-dd HH:mm", CultureInfo.CurrentCulture);
                DateTime endDay = DateTime.ParseExact(DateTime.Now.ToString("yyyy-MM-dd") + " 23:59", "yyyy-MM-dd HH:mm", CultureInfo.CurrentCulture);
                //到访人数
                MEMBER_CT_2TableAdapter ad = new MEMBER_CT_2TableAdapter();
                var table1 = ad.GetAllData(startDay, endDay, shopCode);
                Hashtable data = new Hashtable();
                int vistCount = 0;
                if (table1.Rows.Count > 0)
                {
                    vistCount = (int)table1.Sum(p => p.CT);
               
                }
                data.Add("data1", vistCount);
                //接待人数
                TaskMemberApp memberApp = new TaskMemberApp();
                int readCuont = memberApp.getReadCount(startDay, endDay, shopCode);
                data.Add("data2", readCuont);
                //接待比率
                if (vistCount == 0)
                {
                    data.Add("data3", 0);
                }
                else
                {
                    data.Add("data3", readCuont/ vistCount);
                }
                marketSalesApp salesApp = new marketSalesApp();
              

                var pops = salesApp.getPop(shopCode);
                List<popModel2> popModles = new List<popModel2>();
                QuerySubSalesTotalTableAdapter salesTotalAd = new QuerySubSalesTotalTableAdapter();
                QuerySalesCountTableAdapter salesCountAd = new QuerySalesCountTableAdapter();
                var salesTotal = salesTotalAd.GetData(startDay, endDay, shopCode);
                foreach (var pop in pops)
                {
                    popModel2 model = new popModel2() { empCode=pop.sales_No, empName=pop.sales_Name };
                    model.data1= memberApp.getReadCount(startDay, endDay, shopCode, salesApp.getUserInfoBySalesNo(pop.sales_No).id);
                    model.data2 = 0;
                    model.data3 = 0;
                    if (salesTotal.Rows.Count > 0)
                    {
                        model.data3 = (int)salesTotal.Where(p => p.EMPLOYEE_ID.Equals(pop.sales_No)).Sum(p => p.AMT);
                    }
                    model.data4 = 0;
                    var salesCountTable = salesCountAd.GetDataByEmpCode(startDay, endDay, shopCode, pop.sales_No);
                    if (salesCountTable.Rows.Count > 0)
                    {
                        model.data4 = (int)salesCountTable.Sum(p=>p.CT);
                    }
                    popModles.Add(model);
                }

                data.Add("data4", popModles);
                return Json(data, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(ex.Message, JsonRequestBehavior.AllowGet);
            }
        }
        private JsonResult GetSalesQueryData_Model1(DateTime startTime, DateTime endTime, string shopCode, int model)
        {
            try
            {
                List<String> timeGroup = new List<string>();
                timeGroup.Add("00:00");
                timeGroup.Add("01:00");
                timeGroup.Add("02:00");
                timeGroup.Add("03:00");
                timeGroup.Add("04:00");
                timeGroup.Add("05:00");
                timeGroup.Add("06:00");
                timeGroup.Add("07:00");
                timeGroup.Add("08:00");
                timeGroup.Add("09:00");
                timeGroup.Add("10:00");
                timeGroup.Add("11:00");
                timeGroup.Add("12:00");
                timeGroup.Add("13:00");
                timeGroup.Add("14:00");
                timeGroup.Add("15:00");
                timeGroup.Add("16:00");
                timeGroup.Add("17:00");
                timeGroup.Add("18:00");
                timeGroup.Add("19:00");
                timeGroup.Add("20:00");
                timeGroup.Add("21:00");
                timeGroup.Add("22:00");
                timeGroup.Add("23:00");
                List<String> timeGroupName = new List<string>();
                timeGroupName.Add("0点");
                timeGroupName.Add("1点");
                timeGroupName.Add("2点");
                timeGroupName.Add("3点");
                timeGroupName.Add("4点");
                timeGroupName.Add("5点");
                timeGroupName.Add("6点");
                timeGroupName.Add("7点");
                timeGroupName.Add("8点");
                timeGroupName.Add("9点");
                timeGroupName.Add("10点");
                timeGroupName.Add("11点");
                timeGroupName.Add("12点");
                timeGroupName.Add("13点");
                timeGroupName.Add("14点");
                timeGroupName.Add("15点");
                timeGroupName.Add("16点");
                timeGroupName.Add("17点");
                timeGroupName.Add("18点");
                timeGroupName.Add("19点");
                timeGroupName.Add("20点");
                timeGroupName.Add("21点");
                timeGroupName.Add("22点");
                timeGroupName.Add("23点");

                Random rd = new Random();

                DateTime startDay = DateTime.ParseExact(startTime.ToString("yyyy-MM-dd") + " 00:00", "yyyy-MM-dd HH:mm", CultureInfo.CurrentCulture);
                DateTime endDay = DateTime.ParseExact(endTime.ToString("yyyy-MM-dd") + " 23:59", "yyyy-MM-dd HH:mm", CultureInfo.CurrentCulture);
                DateTime startDay2 = startDay;
                DateTime endDay2 = endDay;
                if (model == 0)
                {
                    startDay2 = startDay.AddDays(-1);
                    endDay2 = endDay.AddDays(-1);
                }
                else if (model == 2)
                {
                    startDay2 = startDay.AddMonths(-1);
                    endDay2 = endDay.AddMonths(-1);
                    timeGroup = new List<string>();
                    timeGroupName = new List<string>();
                    var tempDay = startDay.AddDays(-1);
                    while (!endDay.ToString("yyyy-MM-dd").Equals(tempDay.ToString("yyyy-MM-dd")))
                    {
                        tempDay = tempDay.AddDays(1);
                        timeGroup.Add(tempDay.ToString("yyyyMMdd"));
                        timeGroupName.Add(tempDay.Day + "号");

                    }
                }
                else if (model == 1)
                {
                    startDay2 = startDay.AddDays(-7);
                    endDay2 = endDay.AddDays(-7);
                    timeGroup = new List<string>();
                    timeGroupName = new List<string>();
                    var tempDay = startDay.AddDays(-1);
                    int wd = -1;
                    while (!endDay.ToString("yyyy-MM-dd").Equals(tempDay.ToString("yyyy-MM-dd")))
                    {
                        wd = wd + 1;
                        tempDay = tempDay.AddDays(1);
                        if (wd == 0)
                        {
                            timeGroupName.Add("周日");
                        }
                        else if (wd == 1)
                        {
                            timeGroupName.Add("周一");
                        }
                        else if (wd == 2)
                        {
                            timeGroupName.Add("周二");
                        }
                        else if (wd == 3)
                        {
                            timeGroupName.Add("周三");
                        }
                        else if (wd == 4)
                        {
                            timeGroupName.Add("周四");
                        }
                        else if (wd == 5)
                        {
                            timeGroupName.Add("周五");
                        }
                        else if (wd == 6)
                        {
                            timeGroupName.Add("周六");
                        }
                        timeGroup.Add(tempDay.ToString("yyyyMMdd"));



                    }

                }
                List<int> numbers = new List<int>();
                List<int> pre_numbers = new List<int>();
                DataTableQuerySalesAdapter ad = new DataTableQuerySalesAdapter();
                var table = new DataSynchronizationStanbyLib.KPIDataSet.DataTableQuerySalesDataTable();
                var table2 = new DataSynchronizationStanbyLib.KPIDataSet.DataTableQuerySalesDataTable();
                if (model == 0)
                {
                    table = ad.GetData(startDay, endDay, shopCode);
                    table2 = ad.GetData(startDay2, endDay2, shopCode);
                }
                else
                {
                    table = ad.GetDataByDay(startDay, endDay, shopCode);
                    table2 = ad.GetDataByDay(startDay2, endDay2, shopCode);
                }
                foreach (var item in timeGroup)
                {

                    if (table.Rows.Count > 0)
                    {
                        if (model == 0)
                        {
                            var tableItem = table.Where(p => p.SALESDATE.Equals(item.Substring(0, 2)));
                            if (tableItem.Count() > 0)
                            {
                                numbers.Add((int)tableItem.First().QTY);
                            }
                            else
                            {
                                numbers.Add(0);
                            }
                        }
                        else
                        {
                            var tableItem = table.Where(p => p.SALESDATE.Equals(item));
                            if (tableItem.Count() > 0)
                            {
                                numbers.Add((int)tableItem.First().QTY);
                            }
                            else
                            {
                                numbers.Add(0);
                            }
                        }
                    }
                    else
                    {
                        numbers.Add(0);
                    }
                    if (table2.Rows.Count > 0)
                    {
                        if (model == 0)
                        {
                            var table2Item = table2.Where(p => p.SALESDATE.Equals(item.Substring(0, 2)));
                            if (table2Item.Count() > 0)
                            {
                                pre_numbers.Add((int)table2Item.First().QTY);
                            }
                            else
                            {
                                pre_numbers.Add(0);
                            }
                        }
                        else
                        {
                            string tempDay = "";
                            if (model == 1)
                            {
                                tempDay = DateTime.ParseExact(item, "yyyyMMdd", CultureInfo.CurrentCulture).AddDays(-7).ToString("yyyyMMdd");
                            }
                            else if (model == 2)
                            {
                                tempDay = endDay2.ToString("yyyyMM") + item.Substring(6);
                            }

                            var table2Item = table2.Where(p => p.SALESDATE.Equals(tempDay));
                            if (table2Item.Count() > 0)
                            {
                                pre_numbers.Add((int)table2Item.First().QTY);
                            }
                            else
                            {
                                pre_numbers.Add(0);
                            }

                        }
                    }
                    else
                    {
                        pre_numbers.Add(0);
                    }

                }
                List<double> pre_numbers_ct = new List<double>();
                for (int i = 0; i < timeGroup.Count; i++)
                {
                    if (pre_numbers[i] == 0)
                    {
                        if (numbers[i] > 0)
                        {
                            pre_numbers_ct.Add(1);
                        }
                        else
                        {
                            pre_numbers_ct.Add(0);
                        }
                    }
                    else if (numbers[i] == 0)
                    {
                        if (pre_numbers[i] > 0)
                        {
                            pre_numbers_ct.Add(-1);
                        }
                        else
                        {
                            pre_numbers_ct.Add(0);
                        }
                    }
                    else
                    {
                        double temp = (double)(numbers[i] - pre_numbers[i]) / (double)pre_numbers[i];
                        pre_numbers_ct.Add(temp);
                    }
                }
                Hashtable data = new Hashtable();
                data.Add("totalNumber", numbers.Sum());
                data.Add("totalPreNumber", pre_numbers.Sum());
                if ((double)pre_numbers.Sum() == 0)
                {
                    data.Add("totalNumberCt", (double)0);
                }
                else
                {
                    data.Add("totalNumberCt", (double)(numbers.Sum() - pre_numbers.Sum()) / (double)pre_numbers.Sum());
                }

                data.Add("numbers", numbers);
                data.Add("preNumbers", pre_numbers);
                data.Add("preNumbersCt", pre_numbers_ct);
                data.Add("timeGroupName", timeGroupName);
                data.Add("timeGroup", timeGroup);

                return Json(data, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(ex.Message, JsonRequestBehavior.AllowGet);
            }
        }
        private JsonResult GetSalesQueryData_Model2(DateTime startTime, DateTime endTime, string shopCode, int model)
        {
            try
            {
                List<String> timeGroup = new List<string>();
                timeGroup.Add("00:00");
                timeGroup.Add("01:00");
                timeGroup.Add("02:00");
                timeGroup.Add("03:00");
                timeGroup.Add("04:00");
                timeGroup.Add("05:00");
                timeGroup.Add("06:00");
                timeGroup.Add("07:00");
                timeGroup.Add("08:00");
                timeGroup.Add("09:00");
                timeGroup.Add("10:00");
                timeGroup.Add("11:00");
                timeGroup.Add("12:00");
                timeGroup.Add("13:00");
                timeGroup.Add("14:00");
                timeGroup.Add("15:00");
                timeGroup.Add("16:00");
                timeGroup.Add("17:00");
                timeGroup.Add("18:00");
                timeGroup.Add("19:00");
                timeGroup.Add("20:00");
                timeGroup.Add("21:00");
                timeGroup.Add("22:00");
                timeGroup.Add("23:00");
                List<String> timeGroupName = new List<string>();
                timeGroupName.Add("0点");
                timeGroupName.Add("1点");
                timeGroupName.Add("2点");
                timeGroupName.Add("3点");
                timeGroupName.Add("4点");
                timeGroupName.Add("5点");
                timeGroupName.Add("6点");
                timeGroupName.Add("7点");
                timeGroupName.Add("8点");
                timeGroupName.Add("9点");
                timeGroupName.Add("10点");
                timeGroupName.Add("11点");
                timeGroupName.Add("12点");
                timeGroupName.Add("13点");
                timeGroupName.Add("14点");
                timeGroupName.Add("15点");
                timeGroupName.Add("16点");
                timeGroupName.Add("17点");
                timeGroupName.Add("18点");
                timeGroupName.Add("19点");
                timeGroupName.Add("20点");
                timeGroupName.Add("21点");
                timeGroupName.Add("22点");
                timeGroupName.Add("23点");

                Random rd = new Random();

                DateTime startDay = DateTime.ParseExact(startTime.ToString("yyyy-MM-dd") + " 00:00", "yyyy-MM-dd HH:mm", CultureInfo.CurrentCulture);
                DateTime endDay = DateTime.ParseExact(endTime.ToString("yyyy-MM-dd") + " 23:59", "yyyy-MM-dd HH:mm", CultureInfo.CurrentCulture);
                DateTime startDay2 = startDay;
                DateTime endDay2 = endDay;
                if (model == 0)
                {
                    startDay2 = startDay.AddDays(-1);
                    endDay2 = endDay.AddDays(-1);
                }
                else if (model == 2)
                {
                    startDay2 = startDay.AddMonths(-1);
                    endDay2 = endDay.AddMonths(-1);
                    timeGroup = new List<string>();
                    timeGroupName = new List<string>();
                    var tempDay = startDay.AddDays(-1);
                    while (!endDay.ToString("yyyy-MM-dd").Equals(tempDay.ToString("yyyy-MM-dd")))
                    {
                        tempDay = tempDay.AddDays(1);
                        timeGroup.Add(tempDay.ToString("yyyyMMdd"));
                        timeGroupName.Add(tempDay.Day + "号");

                    }
                }
                else if (model == 1)
                {
                    startDay2 = startDay.AddDays(-7);
                    endDay2 = endDay.AddDays(-7);
                    timeGroup = new List<string>();
                    timeGroupName = new List<string>();
                    var tempDay = startDay.AddDays(-1);
                    int wd = -1;
                    while (!endDay.ToString("yyyy-MM-dd").Equals(tempDay.ToString("yyyy-MM-dd")))
                    {
                        wd = wd + 1;
                        tempDay = tempDay.AddDays(1);
                        if (wd == 0)
                        {
                            timeGroupName.Add("周日");
                        }
                        else if (wd == 1)
                        {
                            timeGroupName.Add("周一");
                        }
                        else if (wd == 2)
                        {
                            timeGroupName.Add("周二");
                        }
                        else if (wd == 3)
                        {
                            timeGroupName.Add("周三");
                        }
                        else if (wd == 4)
                        {
                            timeGroupName.Add("周四");
                        }
                        else if (wd == 5)
                        {
                            timeGroupName.Add("周五");
                        }
                        else if (wd == 6)
                        {
                            timeGroupName.Add("周六");
                        }
                        timeGroup.Add(tempDay.ToString("yyyyMMdd"));



                    }

                }
                List<int> numbers = new List<int>();
                List<int> pre_numbers = new List<int>();
                DataTableQuerySalesAMTTableAdapter ad = new DataTableQuerySalesAMTTableAdapter();
                var table = new DataSynchronizationStanbyLib.KPIDataSet.DataTableQuerySalesAMTDataTable();
                var table2 = new DataSynchronizationStanbyLib.KPIDataSet.DataTableQuerySalesAMTDataTable();
                if (model == 0)
                {
                    table = ad.GetData(startDay, endDay, shopCode);
                    table2 = ad.GetData(startDay2, endDay2, shopCode);
                }
                else
                {
                    table = ad.GetDataByDay(startDay, endDay, shopCode);
                    table2 = ad.GetDataByDay(startDay2, endDay2, shopCode);
                }
                foreach (var item in timeGroup)
                {

                    if (table.Rows.Count > 0)
                    {
                        if (model == 0)
                        {
                            var tableItem = table.Where(p => p.SALESDATE.Equals(item.Substring(0, 2)));
                            if (tableItem.Count() > 0)
                            {
                                numbers.Add((int)tableItem.First().AMT);
                            }
                            else
                            {
                                numbers.Add(0);
                            }
                        }
                        else
                        {
                            var tableItem = table.Where(p => p.SALESDATE.Equals(item));
                            if (tableItem.Count() > 0)
                            {
                                numbers.Add((int)tableItem.First().AMT);
                            }
                            else
                            {
                                numbers.Add(0);
                            }
                        }
                    }
                    else
                    {
                        numbers.Add(0);
                    }
                    if (table2.Rows.Count > 0)
                    {
                        if (model == 0)
                        {
                            var table2Item = table2.Where(p => p.SALESDATE.Equals(item.Substring(0, 2)));
                            if (table2Item.Count() > 0)
                            {
                                pre_numbers.Add((int)table2Item.First().AMT);
                            }
                            else
                            {
                                pre_numbers.Add(0);
                            }
                        }
                        else
                        {
                            string tempDay = "";
                            if (model == 1)
                            {
                                tempDay = DateTime.ParseExact(item, "yyyyMMdd", CultureInfo.CurrentCulture).AddDays(-7).ToString("yyyyMMdd");
                            }
                            else if (model == 2)
                            {
                                tempDay = endDay2.ToString("yyyyMM") + item.Substring(6);
                            }

                            var table2Item = table2.Where(p => p.SALESDATE.Equals(tempDay));
                            if (table2Item.Count() > 0)
                            {
                                pre_numbers.Add((int)table2Item.First().AMT);
                            }
                            else
                            {
                                pre_numbers.Add(0);
                            }

                        }
                    }
                    else
                    {
                        pre_numbers.Add(0);
                    }

                }
                List<double> pre_numbers_ct = new List<double>();
                for (int i = 0; i < timeGroup.Count; i++)
                {
                    if (pre_numbers[i] == 0)
                    {
                        if (numbers[i] > 0)
                        {
                            pre_numbers_ct.Add(1);
                        }
                        else
                        {
                            pre_numbers_ct.Add(0);
                        }
                    }
                    else if (numbers[i] == 0)
                    {
                        if (pre_numbers[i] > 0)
                        {
                            pre_numbers_ct.Add(-1);
                        }
                        else
                        {
                            pre_numbers_ct.Add(0);
                        }
                    }
                    else
                    {
                        double temp = (double)(numbers[i] - pre_numbers[i]) / (double)pre_numbers[i];
                        pre_numbers_ct.Add(temp);
                    }
                }
                Hashtable data = new Hashtable();
                data.Add("totalNumber", numbers.Sum());
                data.Add("totalPreNumber", pre_numbers.Sum());
                if ((double)pre_numbers.Sum() == 0)
                {
                    data.Add("totalNumberCt", (double)0);
                }
                else
                {
                    data.Add("totalNumberCt", (double)(numbers.Sum() - pre_numbers.Sum()) / (double)pre_numbers.Sum());
                }

                data.Add("numbers", numbers);
                data.Add("preNumbers", pre_numbers);
                data.Add("preNumbersCt", pre_numbers_ct);
                data.Add("timeGroupName", timeGroupName);
                data.Add("timeGroup", timeGroup);

                return Json(data, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(ex.Message, JsonRequestBehavior.AllowGet);
            }
        }
        private JsonResult GetSalesQueryData_Model3(DateTime startTime, DateTime endTime, string shopCode, int model)
        {
            try
            {
                List<String> timeGroup = new List<string>();
                timeGroup.Add("00:00");
                timeGroup.Add("01:00");
                timeGroup.Add("02:00");
                timeGroup.Add("03:00");
                timeGroup.Add("04:00");
                timeGroup.Add("05:00");
                timeGroup.Add("06:00");
                timeGroup.Add("07:00");
                timeGroup.Add("08:00");
                timeGroup.Add("09:00");
                timeGroup.Add("10:00");
                timeGroup.Add("11:00");
                timeGroup.Add("12:00");
                timeGroup.Add("13:00");
                timeGroup.Add("14:00");
                timeGroup.Add("15:00");
                timeGroup.Add("16:00");
                timeGroup.Add("17:00");
                timeGroup.Add("18:00");
                timeGroup.Add("19:00");
                timeGroup.Add("20:00");
                timeGroup.Add("21:00");
                timeGroup.Add("22:00");
                timeGroup.Add("23:00");
                List<String> timeGroupName = new List<string>();
                timeGroupName.Add("0点");
                timeGroupName.Add("1点");
                timeGroupName.Add("2点");
                timeGroupName.Add("3点");
                timeGroupName.Add("4点");
                timeGroupName.Add("5点");
                timeGroupName.Add("6点");
                timeGroupName.Add("7点");
                timeGroupName.Add("8点");
                timeGroupName.Add("9点");
                timeGroupName.Add("10点");
                timeGroupName.Add("11点");
                timeGroupName.Add("12点");
                timeGroupName.Add("13点");
                timeGroupName.Add("14点");
                timeGroupName.Add("15点");
                timeGroupName.Add("16点");
                timeGroupName.Add("17点");
                timeGroupName.Add("18点");
                timeGroupName.Add("19点");
                timeGroupName.Add("20点");
                timeGroupName.Add("21点");
                timeGroupName.Add("22点");
                timeGroupName.Add("23点");

                Random rd = new Random();

                DateTime startDay = DateTime.ParseExact(startTime.ToString("yyyy-MM-dd") + " 00:00", "yyyy-MM-dd HH:mm", CultureInfo.CurrentCulture);
                DateTime endDay = DateTime.ParseExact(endTime.ToString("yyyy-MM-dd") + " 23:59", "yyyy-MM-dd HH:mm", CultureInfo.CurrentCulture);
                DateTime startDay2 = startDay;
                DateTime endDay2 = endDay;
                if (model == 0)
                {
                    startDay2 = startDay.AddDays(-1);
                    endDay2 = endDay.AddDays(-1);
                }
                else if (model == 2)
                {
                    startDay2 = startDay.AddMonths(-1);
                    endDay2 = endDay.AddMonths(-1);
                    timeGroup = new List<string>();
                    timeGroupName = new List<string>();
                    var tempDay = startDay.AddDays(-1);
                    while (!endDay.ToString("yyyy-MM-dd").Equals(tempDay.ToString("yyyy-MM-dd")))
                    {
                        tempDay = tempDay.AddDays(1);
                        timeGroup.Add(tempDay.ToString("yyyyMMdd"));
                        timeGroupName.Add(tempDay.Day + "号");

                    }
                }
                else if (model == 1)
                {
                    startDay2 = startDay.AddDays(-7);
                    endDay2 = endDay.AddDays(-7);
                    timeGroup = new List<string>();
                    timeGroupName = new List<string>();
                    var tempDay = startDay.AddDays(-1);
                    int wd = -1;
                    while (!endDay.ToString("yyyy-MM-dd").Equals(tempDay.ToString("yyyy-MM-dd")))
                    {
                        wd = wd + 1;
                        tempDay = tempDay.AddDays(1);
                        if (wd == 0)
                        {
                            timeGroupName.Add("周日");
                        }
                        else if (wd == 1)
                        {
                            timeGroupName.Add("周一");
                        }
                        else if (wd == 2)
                        {
                            timeGroupName.Add("周二");
                        }
                        else if (wd == 3)
                        {
                            timeGroupName.Add("周三");
                        }
                        else if (wd == 4)
                        {
                            timeGroupName.Add("周四");
                        }
                        else if (wd == 5)
                        {
                            timeGroupName.Add("周五");
                        }
                        else if (wd == 6)
                        {
                            timeGroupName.Add("周六");
                        }
                        timeGroup.Add(tempDay.ToString("yyyyMMdd"));



                    }

                }
                List<double> numbers = new List<double>();
                List<double> pre_numbers = new List<double>();
                DataTableQuerySalesCountTableAdapter ad = new DataTableQuerySalesCountTableAdapter();
                DataTableQueryMemberVisitAdapter ad_m = new DataTableQueryMemberVisitAdapter();
                var table = new DataSynchronizationStanbyLib.KPIDataSet.DataTableQuerySalesCountDataTable();
                var table2 = new DataSynchronizationStanbyLib.KPIDataSet.DataTableQuerySalesCountDataTable();
                var table_m = new DataSynchronizationStanbyLib.KPIDataSet.DataTableQueryMemberVisitDataTable();
                var table2_m = new DataSynchronizationStanbyLib.KPIDataSet.DataTableQueryMemberVisitDataTable();
                if (model == 0)
                {
                    table = ad.GetData(startDay, endDay, shopCode);
                    table2 = ad.GetData(startDay2, endDay2, shopCode);
                    table_m = ad_m.GetData(startDay, endDay, shopCode);
                    table2_m = ad_m.GetData(startDay2, endDay2, shopCode);
                }
                else
                {
                    table = ad.GetDataByDay(startDay, endDay, shopCode);
                    table2 = ad.GetDataByDay(startDay2, endDay2, shopCode);
                    table_m = ad_m.GetDataBydDay(startDay, endDay, shopCode);
                    table2_m = ad_m.GetDataBydDay(startDay2, endDay2, shopCode);
                }
                foreach (var item in timeGroup)
                {

                    if (table.Rows.Count > 0)
                    {
                        if (model == 0)
                        {
                            if (table_m.Rows.Count > 0)
                            {
                                var tableItem = table.Where(p => p.SALESDATE.Equals(item.Substring(0, 2)));
                                var tableMItem = table_m.Where(p => p.VISITTIME.Equals(item.Substring(0, 2)));
                                if (tableItem.Count() > 0 && tableMItem.Count() > 0)
                                {
                                    numbers.Add((double)(tableItem.First().CT / tableMItem.First().CT));
                                }
                                else
                                {
                                    numbers.Add(0);
                                }
                            }
                            else
                            {
                                numbers.Add(0);
                            }
                            
                        }
                        else
                        {
                            if (table_m.Rows.Count > 0)
                            {
                                var tableItem = table.Where(p => p.SALESDATE.Equals(item));
                                var tableMItem = table_m.Where(p => p.VISITTIME.Equals(item));
                                if (tableItem.Count() > 0 && tableMItem.Count() > 0)
                                {
                                    numbers.Add((double)(tableItem.First().CT / tableMItem.First().CT));
                                }
                                else 
                                {
                                    numbers.Add(0);
                                }
                               
                            }
                            else
                            {
                                numbers.Add(0);
                            }
                        }
                    }
                    else
                    {
                        numbers.Add(0);
                    }



                    if (table2.Rows.Count > 0)
                    {
                        if (model == 0)
                        {
                            if (table2_m.Rows.Count > 0)
                            {
                                var table2Item = table2.Where(p => p.SALESDATE.Equals(item.Substring(0, 2)));
                                var table2MItem = table2_m.Where(p => p.VISITTIME.Equals(item.Substring(0, 2)));
                                if (table2Item.Count() > 0 && table2MItem.Count()>0)
                                {
                                    pre_numbers.Add((double)(table2Item.First().CT / table2MItem.First().CT));
                                }
                                else
                                {
                                    pre_numbers.Add(0);
                                }
                            }
                            else
                            {
                                pre_numbers.Add(0);
                            }
                        }
                        else
                        {
                            string tempDay = "";
                            if (model == 1)
                            {
                                tempDay = DateTime.ParseExact(item, "yyyyMMdd", CultureInfo.CurrentCulture).AddDays(-7).ToString("yyyyMMdd");
                            }
                            else if (model == 2)
                            {
                                tempDay = endDay2.ToString("yyyyMM") + item.Substring(6);
                            }
                            if (table2_m.Rows.Count > 0)
                            {
                                var table2Item = table2.Where(p => p.SALESDATE.Equals(tempDay));
                                var table2MItem = table2_m.Where(p => p.VISITTIME.Equals(tempDay));
                                if (table2Item.Count() > 0 && table2MItem.Count() > 0)
                                {
                                    pre_numbers.Add((double)(table2Item.First().CT / table2MItem.First().CT));
                                }
                                else
                                {
                                    pre_numbers.Add(0);
                                }
                            }
                            else
                            {
                                pre_numbers.Add(0);
                            }

                        }
                    }
                    else
                    {
                        pre_numbers.Add(0);
                    }

                }
                List<double> pre_numbers_ct = new List<double>();
                for (int i = 0; i < timeGroup.Count; i++)
                {
                    if (pre_numbers[i] == 0)
                    {
                        if (numbers[i] > 0)
                        {
                            pre_numbers_ct.Add(1);
                        }
                        else
                        {
                            pre_numbers_ct.Add(0);
                        }
                    }
                    else if (numbers[i] == 0)
                    {
                        if (pre_numbers[i] > 0)
                        {
                            pre_numbers_ct.Add(-1);
                        }
                        else
                        {
                            pre_numbers_ct.Add(0);
                        }
                    }
                    else
                    {
                        double temp = (double)(numbers[i] - pre_numbers[i]) / (double)pre_numbers[i];
                        pre_numbers_ct.Add(temp);
                    }
                }
                Hashtable data = new Hashtable();
                double totalNumber = 0;
                double totalPreNumber = 0;
                if (table.Rows.Count > 0 && table_m.Rows.Count > 0)
                {
                    totalNumber = (double)(table.Sum(p => p.CT) / table_m.Sum(p => p.CT));
                    data.Add("totalNumber", totalNumber);
                }
                else
                {
                    data.Add("totalNumber", 0);
                }

                if (table2.Rows.Count > 0 && table2_m.Rows.Count > 0)
                {
                    totalPreNumber = (double)(table2.Sum(p => p.CT) / table2_m.Sum(p => p.CT));
                    data.Add("totalPreNumber", totalPreNumber);
                }
                else
                {
                    data.Add("totalPreNumber", 0);
                }




                if ((double)totalPreNumber == 0)
                {
                    data.Add("totalNumberCt", (double)0);
                }
                else
                {
                    data.Add("totalNumberCt", (double)(totalNumber - totalPreNumber) / (double)totalPreNumber);
                }

                data.Add("numbers", numbers);
                data.Add("preNumbers", pre_numbers);
                data.Add("preNumbersCt", pre_numbers_ct);
                data.Add("timeGroupName", timeGroupName);
                data.Add("timeGroup", timeGroup);

                return Json(data, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(ex.Message, JsonRequestBehavior.AllowGet);
            }
        }

        public JsonResult GetSalesQueryData(DateTime startTime, DateTime endTime, string shopCode, int model,int model2)
        {
            if (model2 == 1)
            {
                return GetSalesQueryData_Model1(startTime, endTime, shopCode, model);
            }
            else if (model2 == 2)
            {
                return GetSalesQueryData_Model2(startTime, endTime, shopCode, model);
            }
            else if (model2 == 3)
            {
                return GetSalesQueryData_Model3(startTime, endTime, shopCode, model);
            }
            else
            {
                return GetSalesQueryData_Model1(startTime, endTime, shopCode, model);
            }

        }
        private JsonResult GetOrderReportQueryData_Model1(DateTime startTime, DateTime endTime, string shopCode, int model)
        {
            try
            {
                Hashtable data = new Hashtable();
                List<String> timeGroup = new List<string>();
                //timeGroup.Add("云货架");
                //timeGroup.Add("现金");
                //timeGroup.Add("POS");
                
                List<String> timeGroupName = new List<string>();
                //timeGroupName.Add("云货架");
                //timeGroupName.Add("现金");
                //timeGroupName.Add("POS");
                

                Random rd = new Random();

                DateTime startDay = DateTime.ParseExact(startTime.ToString("yyyy-MM-dd") + " 00:00", "yyyy-MM-dd HH:mm", CultureInfo.CurrentCulture);
                DateTime endDay = DateTime.ParseExact(endTime.ToString("yyyy-MM-dd") + " 23:59", "yyyy-MM-dd HH:mm", CultureInfo.CurrentCulture);
                DateTime startDay2 = startDay;
                DateTime endDay2 = endDay;
                if (model == 0)
                {
                    startDay2 = startDay.AddDays(-1);
                    endDay2 = endDay.AddDays(-1);
                }
                else if (model == 2)
                {
                    startDay2 = startDay.AddMonths(-1);
                    endDay2 = endDay.AddMonths(-1);
                   
                    
                }
                else if (model == 1)
                {
                    startDay2 = startDay.AddDays(-7);
                    endDay2 = endDay.AddDays(-7);

                }
                KPI9TableAdapter kpi9Ad = new KPI9TableAdapter();
                var kpiTotalTable = kpi9Ad.GetData(startDay, endDay, shopCode);

                if (kpiTotalTable.Rows.Count > 0)
                {
                    data.Add("totalData1", kpiTotalTable.Sum(p => p.CT));
                    
                    data.Add("totalData2", kpiTotalTable.Sum(p => p.SALES_QTY));
                    if (kpiTotalTable.Sum(p => p.CT) > 0)
                    {
                        data.Add("totalData3", kpiTotalTable.Sum(p => p.SALES_QTY) / kpiTotalTable.Sum(p => p.CT));
                        data.Add("totalData5", kpiTotalTable.Sum(p => p.PAY_AMT) / kpiTotalTable.Sum(p => p.CT));
                    }
                    else
                    {
                        data.Add("totalData3", 0);
                        data.Add("totalData5", 0);
                    }
                    data.Add("totalData4", kpiTotalTable.Sum(p => p.PAY_AMT));
                    
                    if (kpiTotalTable.Sum(p => p.SALES_QTY) > 0)
                        data.Add("totalData6", kpiTotalTable.Sum(p => p.PAY_AMT) / kpiTotalTable.Sum(p => p.SALES_QTY));
                    else
                        data.Add("totalData6", 0);
                }



                List<int> qtys = new List<int>();
                List<int> amts = new List<int>();
                List<double> aegs  = new List<double>();
                KPI9_1Adapter ad = new KPI9_1Adapter();
                var table = ad.GetData(startDay, endDay, shopCode);
                if (table.Rows.Count > 0)
                {
                    foreach (var row in table)
                    {
                        timeGroup.Add(row.PAY_MONTHED_NAME);
                        timeGroupName.Add(row.PAY_MONTHED_NAME);
                        qtys.Add((int)row.SALES_QTY);
                        amts.Add((int)row.PAY_AMT);
                        if (table.Sum(p => p.PAY_AMT) > 0)
                        {
                            aegs.Add((double)(row.PAY_AMT / table.Sum(p => p.PAY_AMT)));
                        }
                    }
                }

                 
               
                /*
                data.Add("totalNumber", numbers.Sum());
                data.Add("totalPreNumber", pre_numbers.Sum());
                if ((double)pre_numbers.Sum() == 0)
                {
                    data.Add("totalNumberCt", (double)0);
                }
                else
                {
                    data.Add("totalNumberCt", (double)(numbers.Sum() - pre_numbers.Sum()) / (double)pre_numbers.Sum());
                }**/

                data.Add("qtys", qtys);
                data.Add("amts", amts);
                data.Add("aegs", aegs);
                data.Add("timeGroupName", timeGroupName);
                data.Add("timeGroup", timeGroup);

                return Json(data, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(ex.Message, JsonRequestBehavior.AllowGet);
            }
        }
        private JsonResult GetOrderReportQueryData_Model2(DateTime startTime, DateTime endTime, string shopCode, int model)
        {
            try
            {
                Hashtable data = new Hashtable();
                List<String> timeGroup = new List<string>();
                //timeGroup.Add("云货架");
                //timeGroup.Add("现金");
                //timeGroup.Add("POS");

                List<String> timeGroupName = new List<string>();
                //timeGroupName.Add("云货架");
                //timeGroupName.Add("现金");
                //timeGroupName.Add("POS");


                Random rd = new Random();

                DateTime startDay = DateTime.ParseExact(startTime.ToString("yyyy-MM-dd") + " 00:00", "yyyy-MM-dd HH:mm", CultureInfo.CurrentCulture);
                DateTime endDay = DateTime.ParseExact(endTime.ToString("yyyy-MM-dd") + " 23:59", "yyyy-MM-dd HH:mm", CultureInfo.CurrentCulture);
                DateTime startDay2 = startDay;
                DateTime endDay2 = endDay;
                if (model == 0)
                {
                    startDay2 = startDay.AddDays(-1);
                    endDay2 = endDay.AddDays(-1);
                }
                else if (model == 2)
                {
                    startDay2 = startDay.AddMonths(-1);
                    endDay2 = endDay.AddMonths(-1);


                }
                else if (model == 1)
                {
                    startDay2 = startDay.AddDays(-7);
                    endDay2 = endDay.AddDays(-7);

                }
                KPI9TableAdapter kpi9Ad = new KPI9TableAdapter();
                var kpiTotalTable = kpi9Ad.GetData(startDay, endDay, shopCode);

                if (kpiTotalTable.Rows.Count > 0)
                {
                    data.Add("totalData1", kpiTotalTable.Sum(p => p.CT));
                    data.Add("totalData2", kpiTotalTable.Sum(p => p.SALES_QTY));

                    if (kpiTotalTable.Sum(p => p.CT) > 0)
                    {
                        data.Add("totalData3", kpiTotalTable.Sum(p => p.SALES_QTY) / kpiTotalTable.Sum(p => p.CT));
                        data.Add("totalData5", kpiTotalTable.Sum(p => p.PAY_AMT) / kpiTotalTable.Sum(p => p.CT));
                    }
                    else
                    {
                        data.Add("totalData3", 0);
                        data.Add("totalData5", 0);
                    }
                    //data.Add("totalData3", kpiTotalTable.Sum(p => p.SALES_QTY) / kpiTotalTable.Sum(p => p.CT));
                    data.Add("totalData4", kpiTotalTable.Sum(p => p.PAY_AMT));
                    //data.Add("totalData5", kpiTotalTable.Sum(p => p.PAY_AMT) / kpiTotalTable.Sum(p => p.CT));
                    if (kpiTotalTable.Sum(p => p.SALES_QTY) > 0)
                        data.Add("totalData6", kpiTotalTable.Sum(p => p.PAY_AMT) / kpiTotalTable.Sum(p => p.SALES_QTY));
                    else
                        data.Add("totalData6", 0);
                }



                List<int> qtys = new List<int>();
                List<int> amts = new List<int>();
                List<double> aegs = new List<double>();
                KPI9_2Adapter ad = new KPI9_2Adapter();
                var table = ad.GetData(startDay, endDay, shopCode);
                if (table.Rows.Count > 0)
                {
                    foreach (var row in table)
                    {
                        timeGroup.Add(row.PROD_TYPE);
                        timeGroupName.Add(row.PROD_TYPE);
                        qtys.Add((int)row.SALES_QTY);
                        amts.Add((int)row.SALES_AMT);
                        if (table.Sum(p => p.SALES_AMT) > 0)
                        {
                            aegs.Add((double)(row.SALES_AMT / table.Sum(p => p.SALES_AMT)));
                        }
                    }
                }



                /*
                data.Add("totalNumber", numbers.Sum());
                data.Add("totalPreNumber", pre_numbers.Sum());
                if ((double)pre_numbers.Sum() == 0)
                {
                    data.Add("totalNumberCt", (double)0);
                }
                else
                {
                    data.Add("totalNumberCt", (double)(numbers.Sum() - pre_numbers.Sum()) / (double)pre_numbers.Sum());
                }**/

                data.Add("qtys", qtys);
                data.Add("amts", amts);
                data.Add("aegs", aegs);
                data.Add("timeGroupName", timeGroupName);
                data.Add("timeGroup", timeGroup);

                return Json(data, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(ex.Message, JsonRequestBehavior.AllowGet);
            }
        }
        private JsonResult GetOrderReportQueryData_Model3(DateTime startTime, DateTime endTime, string shopCode, int model)
        {
            try
            {
                Hashtable data = new Hashtable();
                List<String> timeGroup = new List<string>();
                //timeGroup.Add("云货架");
                //timeGroup.Add("现金");
                //timeGroup.Add("POS");

                List<String> timeGroupName = new List<string>();
                //timeGroupName.Add("云货架");
                //timeGroupName.Add("现金");
                //timeGroupName.Add("POS");


                Random rd = new Random();

                DateTime startDay = DateTime.ParseExact(startTime.ToString("yyyy-MM-dd") + " 00:00", "yyyy-MM-dd HH:mm", CultureInfo.CurrentCulture);
                DateTime endDay = DateTime.ParseExact(endTime.ToString("yyyy-MM-dd") + " 23:59", "yyyy-MM-dd HH:mm", CultureInfo.CurrentCulture);
                DateTime startDay2 = startDay;
                DateTime endDay2 = endDay;
                if (model == 0)
                {
                    startDay2 = startDay.AddDays(-1);
                    endDay2 = endDay.AddDays(-1);
                }
                else if (model == 2)
                {
                    startDay2 = startDay.AddMonths(-1);
                    endDay2 = endDay.AddMonths(-1);


                }
                else if (model == 1)
                {
                    startDay2 = startDay.AddDays(-7);
                    endDay2 = endDay.AddDays(-7);

                }
                KPI9TableAdapter kpi9Ad = new KPI9TableAdapter();
                var kpiTotalTable = kpi9Ad.GetData(startDay, endDay, shopCode);

                if (kpiTotalTable.Rows.Count > 0)
                {
                    data.Add("totalData1", kpiTotalTable.Sum(p => p.CT));
                    data.Add("totalData2", kpiTotalTable.Sum(p => p.SALES_QTY));
                    if (kpiTotalTable.Sum(p => p.CT) > 0)
                    {
                        data.Add("totalData3", kpiTotalTable.Sum(p => p.SALES_QTY) / kpiTotalTable.Sum(p => p.CT));
                        data.Add("totalData5", kpiTotalTable.Sum(p => p.PAY_AMT) / kpiTotalTable.Sum(p => p.CT));
                    }
                    else
                    {
                        data.Add("totalData3", 0);
                        data.Add("totalData5", 0);
                    }

                    //data.Add("totalData3", kpiTotalTable.Sum(p => p.SALES_QTY) / kpiTotalTable.Sum(p => p.CT));
                    data.Add("totalData4", kpiTotalTable.Sum(p => p.PAY_AMT));
                    //data.Add("totalData5", kpiTotalTable.Sum(p => p.PAY_AMT) / kpiTotalTable.Sum(p => p.CT));
                    if (kpiTotalTable.Sum(p => p.SALES_QTY) > 0)
                        data.Add("totalData6", kpiTotalTable.Sum(p => p.PAY_AMT) / kpiTotalTable.Sum(p => p.SALES_QTY));
                    else
                        data.Add("totalData6", 0);
                }



                List<int> qtys = new List<int>();
                List<int> amts = new List<int>();
                List<double> aegs = new List<double>();
                KPI9_3Adapter ad = new KPI9_3Adapter();
                var table = ad.GetData(startDay, endDay, shopCode);
                if (table.Rows.Count > 0)
                {
                    foreach (var row in table)
                    {
                        timeGroup.Add(row.PROD_BRAND);
                        timeGroupName.Add(row.PROD_BRAND);
                        qtys.Add((int)row.SALES_QTY);
                        amts.Add((int)row.SALES_AMT);
                        if (table.Sum(p => p.SALES_AMT) > 0)
                        {
                            aegs.Add((double)(row.SALES_AMT / table.Sum(p => p.SALES_AMT)));
                        }
                    }
                }



                /*
                data.Add("totalNumber", numbers.Sum());
                data.Add("totalPreNumber", pre_numbers.Sum());
                if ((double)pre_numbers.Sum() == 0)
                {
                    data.Add("totalNumberCt", (double)0);
                }
                else
                {
                    data.Add("totalNumberCt", (double)(numbers.Sum() - pre_numbers.Sum()) / (double)pre_numbers.Sum());
                }**/

                data.Add("qtys", qtys);
                data.Add("amts", amts);
                data.Add("aegs", aegs);
                data.Add("timeGroupName", timeGroupName);
                data.Add("timeGroup", timeGroup);

                return Json(data, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(ex.Message, JsonRequestBehavior.AllowGet);
            }
        }
        private JsonResult GetOrderReportQueryData_Model4(DateTime startTime, DateTime endTime, string shopCode, int model)
        {
            try
            {
                Hashtable data = new Hashtable();
                List<String> timeGroup = new List<string>();
                //timeGroup.Add("云货架");
                //timeGroup.Add("现金");
                //timeGroup.Add("POS");

                List<String> timeGroupName = new List<string>();
                //timeGroupName.Add("云货架");
                //timeGroupName.Add("现金");
                //timeGroupName.Add("POS");


                Random rd = new Random();

                DateTime startDay = DateTime.ParseExact(startTime.ToString("yyyy-MM-dd") + " 00:00", "yyyy-MM-dd HH:mm", CultureInfo.CurrentCulture);
                DateTime endDay = DateTime.ParseExact(endTime.ToString("yyyy-MM-dd") + " 23:59", "yyyy-MM-dd HH:mm", CultureInfo.CurrentCulture);
                DateTime startDay2 = startDay;
                DateTime endDay2 = endDay;
                if (model == 0)
                {
                    startDay2 = startDay.AddDays(-1);
                    endDay2 = endDay.AddDays(-1);
                }
                else if (model == 2)
                {
                    startDay2 = startDay.AddMonths(-1);
                    endDay2 = endDay.AddMonths(-1);


                }
                else if (model == 1)
                {
                    startDay2 = startDay.AddDays(-7);
                    endDay2 = endDay.AddDays(-7);

                }
                KPI9TableAdapter kpi9Ad = new KPI9TableAdapter();
                var kpiTotalTable = kpi9Ad.GetData(startDay, endDay, shopCode);

                if (kpiTotalTable.Rows.Count > 0)
                {
                    data.Add("totalData1", kpiTotalTable.Sum(p => p.CT));
                    data.Add("totalData2", kpiTotalTable.Sum(p => p.SALES_QTY));

                    if (kpiTotalTable.Sum(p => p.CT) > 0)
                    {
                        data.Add("totalData3", kpiTotalTable.Sum(p => p.SALES_QTY) / kpiTotalTable.Sum(p => p.CT));
                        data.Add("totalData5", kpiTotalTable.Sum(p => p.PAY_AMT) / kpiTotalTable.Sum(p => p.CT));
                    }
                    else
                    {
                        data.Add("totalData3", 0);
                        data.Add("totalData5", 0);
                    }

                    //data.Add("totalData3", kpiTotalTable.Sum(p => p.SALES_QTY) / kpiTotalTable.Sum(p => p.CT));
                    data.Add("totalData4", kpiTotalTable.Sum(p => p.PAY_AMT));
                    //data.Add("totalData5", kpiTotalTable.Sum(p => p.PAY_AMT) / kpiTotalTable.Sum(p => p.CT));
                    if (kpiTotalTable.Sum(p => p.SALES_QTY) > 0)
                        data.Add("totalData6", kpiTotalTable.Sum(p => p.PAY_AMT) / kpiTotalTable.Sum(p => p.SALES_QTY));
                    else
                        data.Add("totalData6", 0);
                }



                List<int> qtys = new List<int>();
                List<int> amts = new List<int>();
                List<double> aegs = new List<double>();
                KPI9_4Adapter ad = new KPI9_4Adapter();
                var table = ad.GetData(startDay, endDay, shopCode);
                if (table.Rows.Count > 0)
                {
                    foreach (var row in table)
                    {
                        timeGroup.Add(row.PRICE_RANGE);
                        timeGroupName.Add(row.PRICE_RANGE);
                        qtys.Add((int)row.SALES_QTY);
                        amts.Add((int)row.SALES_AMT);
                        if (table.Sum(p => p.SALES_AMT) > 0)
                        {
                            aegs.Add((double)(row.SALES_AMT / table.Sum(p => p.SALES_AMT)));
                        }
                    }
                }



                /*
                data.Add("totalNumber", numbers.Sum());
                data.Add("totalPreNumber", pre_numbers.Sum());
                if ((double)pre_numbers.Sum() == 0)
                {
                    data.Add("totalNumberCt", (double)0);
                }
                else
                {
                    data.Add("totalNumberCt", (double)(numbers.Sum() - pre_numbers.Sum()) / (double)pre_numbers.Sum());
                }**/

                data.Add("qtys", qtys);
                data.Add("amts", amts);
                data.Add("aegs", aegs);
                data.Add("timeGroupName", timeGroupName);
                data.Add("timeGroup", timeGroup);

                return Json(data, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(ex.Message, JsonRequestBehavior.AllowGet);
            }
        }
        private JsonResult GetOrderReportQueryData_Model5(DateTime startTime, DateTime endTime, string shopCode, int model)
        {
            try
            {
                Hashtable data = new Hashtable();
                List<String> timeGroup = new List<string>();
                //timeGroup.Add("云货架");
                //timeGroup.Add("现金");
                //timeGroup.Add("POS");

                List<String> timeGroupName = new List<string>();
                //timeGroupName.Add("云货架");
                //timeGroupName.Add("现金");
                //timeGroupName.Add("POS");


                Random rd = new Random();

                DateTime startDay = DateTime.ParseExact(startTime.ToString("yyyy-MM-dd") + " 00:00", "yyyy-MM-dd HH:mm", CultureInfo.CurrentCulture);
                DateTime endDay = DateTime.ParseExact(endTime.ToString("yyyy-MM-dd") + " 23:59", "yyyy-MM-dd HH:mm", CultureInfo.CurrentCulture);
                DateTime startDay2 = startDay;
                DateTime endDay2 = endDay;
                if (model == 0)
                {
                    startDay2 = startDay.AddDays(-1);
                    endDay2 = endDay.AddDays(-1);
                }
                else if (model == 2)
                {
                    startDay2 = startDay.AddMonths(-1);
                    endDay2 = endDay.AddMonths(-1);


                }
                else if (model == 1)
                {
                    startDay2 = startDay.AddDays(-7);
                    endDay2 = endDay.AddDays(-7);

                }
                KPI9TableAdapter kpi9Ad = new KPI9TableAdapter();
                var kpiTotalTable = kpi9Ad.GetData(startDay, endDay, shopCode);

                if (kpiTotalTable.Rows.Count > 0)
                {
                    data.Add("totalData1", kpiTotalTable.Sum(p => p.CT));
                    data.Add("totalData2", kpiTotalTable.Sum(p => p.SALES_QTY));

                    if (kpiTotalTable.Sum(p => p.CT) > 0)
                    {
                        data.Add("totalData3", kpiTotalTable.Sum(p => p.SALES_QTY) / kpiTotalTable.Sum(p => p.CT));
                        data.Add("totalData5", kpiTotalTable.Sum(p => p.PAY_AMT) / kpiTotalTable.Sum(p => p.CT));
                    }
                    else
                    {
                        data.Add("totalData3", 0);
                        data.Add("totalData5", 0);
                    }

                    //data.Add("totalData3", kpiTotalTable.Sum(p => p.SALES_QTY) / kpiTotalTable.Sum(p => p.CT));
                    data.Add("totalData4", kpiTotalTable.Sum(p => p.PAY_AMT));
                    //data.Add("totalData5", kpiTotalTable.Sum(p => p.PAY_AMT) / kpiTotalTable.Sum(p => p.CT));
                    if (kpiTotalTable.Sum(p => p.SALES_QTY) > 0)
                        data.Add("totalData6", kpiTotalTable.Sum(p => p.PAY_AMT) / kpiTotalTable.Sum(p => p.SALES_QTY));
                    else
                        data.Add("totalData6", 0);
                }



                List<int> qtys = new List<int>();
                List<int> amts = new List<int>();
                List<double> aegs = new List<double>();
                KPI9_5Adapter ad = new KPI9_5Adapter();
                var table = ad.GetData(startDay, endDay, shopCode);
                if (table.Rows.Count > 0)
                {
                    foreach (var row in table)
                    {
                        timeGroup.Add(row.TYPE);
                        timeGroupName.Add(row.TYPE);
                        qtys.Add((int)row.SALES_QTY);
                        amts.Add((int)row.PAY_AMT);
                        if (table.Sum(p => p.PAY_AMT) > 0)
                        {
                            aegs.Add((double)(row.PAY_AMT / table.Sum(p => p.PAY_AMT)));
                        }
                    }
                }



                /*
                data.Add("totalNumber", numbers.Sum());
                data.Add("totalPreNumber", pre_numbers.Sum());
                if ((double)pre_numbers.Sum() == 0)
                {
                    data.Add("totalNumberCt", (double)0);
                }
                else
                {
                    data.Add("totalNumberCt", (double)(numbers.Sum() - pre_numbers.Sum()) / (double)pre_numbers.Sum());
                }**/

                data.Add("qtys", qtys);
                data.Add("amts", amts);
                data.Add("aegs", aegs);
                data.Add("timeGroupName", timeGroupName);
                data.Add("timeGroup", timeGroup);

                return Json(data, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(ex.Message, JsonRequestBehavior.AllowGet);
            }
        }
        public JsonResult GetOrderReportQueryData(DateTime startTime, DateTime endTime, string shopCode, int model, int model2)
        {
            if (model2 == 1)
            {
                return GetOrderReportQueryData_Model1(startTime, endTime, shopCode, model);
            }
            else if (model2 == 2)
            {
                return GetOrderReportQueryData_Model2(startTime, endTime, shopCode, model);
            }
            else if (model2 == 3)
            {
                return GetOrderReportQueryData_Model3(startTime, endTime, shopCode, model);
            }
            else if (model2 == 4)
            {
                return GetOrderReportQueryData_Model4(startTime, endTime, shopCode, model);
            }
            else if (model2 == 5)
            {
                return GetOrderReportQueryData_Model5(startTime, endTime, shopCode, model);
            }
            else
            {
                return GetOrderReportQueryData_Model1(startTime, endTime, shopCode, model);
            }

        }

        public JsonResult GetQueryData(DateTime startTime,DateTime endTime,string shopCode,int model)
        {
            try
            {
                List<String> timeGroup = new List<string>();
                timeGroup.Add("00:00");
                timeGroup.Add("01:00");
                timeGroup.Add("02:00");
                timeGroup.Add("03:00");
                timeGroup.Add("04:00");
                timeGroup.Add("05:00");
                timeGroup.Add("06:00");
                timeGroup.Add("07:00");
                timeGroup.Add("08:00");
                timeGroup.Add("09:00");
                timeGroup.Add("10:00");
                timeGroup.Add("11:00");
                timeGroup.Add("12:00");
                timeGroup.Add("13:00");
                timeGroup.Add("14:00");
                timeGroup.Add("15:00");
                timeGroup.Add("16:00");
                timeGroup.Add("17:00");
                timeGroup.Add("18:00");
                timeGroup.Add("19:00");
                timeGroup.Add("20:00");
                timeGroup.Add("21:00");
                timeGroup.Add("22:00");
                timeGroup.Add("23:00");
                List<String> timeGroupName = new List<string>();
                timeGroupName.Add("0点");
                timeGroupName.Add("1点");
                timeGroupName.Add("2点");
                timeGroupName.Add("3点");
                timeGroupName.Add("4点");
                timeGroupName.Add("5点");
                timeGroupName.Add("6点");
                timeGroupName.Add("7点");
                timeGroupName.Add("8点");
                timeGroupName.Add("9点");
                timeGroupName.Add("10点");
                timeGroupName.Add("11点");
                timeGroupName.Add("12点");
                timeGroupName.Add("13点");
                timeGroupName.Add("14点");
                timeGroupName.Add("15点");
                timeGroupName.Add("16点");
                timeGroupName.Add("17点");
                timeGroupName.Add("18点");
                timeGroupName.Add("19点");
                timeGroupName.Add("20点");
                timeGroupName.Add("21点");
                timeGroupName.Add("22点");
                timeGroupName.Add("23点");
                Random rd = new Random();

                DateTime startDay = DateTime.ParseExact(startTime.ToString("yyyy-MM-dd") + " 00:00", "yyyy-MM-dd HH:mm", CultureInfo.CurrentCulture);
                DateTime endDay = DateTime.ParseExact(endTime.ToString("yyyy-MM-dd") + " 23:59", "yyyy-MM-dd HH:mm", CultureInfo.CurrentCulture);
                DateTime startDay2 = startDay;
                DateTime endDay2 = endDay;
                if (model == 0)
                {
                    startDay2 = startDay.AddDays(-1);
                    endDay2 = endDay.AddDays(-1);
                }
                else if (model == 2)
                {
                    startDay2 = startDay.AddMonths(-1);
                    endDay2 = endDay.AddMonths(-1);
                    timeGroup = new List<string>();
                    timeGroupName = new List<string>();
                    var tempDay = startDay.AddDays(-1);
                    while (!endDay.ToString("yyyy-MM-dd").Equals(tempDay.ToString("yyyy-MM-dd")))
                    {
                        tempDay = tempDay.AddDays(1);
                        timeGroup.Add(tempDay.ToString("yyyyMMdd"));
                        timeGroupName.Add(tempDay.Day + "号");

                    }
                }
                else if (model == 1)
                {
                    startDay2 = startDay.AddDays(-7);
                    endDay2 = endDay.AddDays(-7);
                    timeGroup = new List<string>();
                    timeGroupName = new List<string>();
                    var tempDay = startDay.AddDays(-1);
                    int wd = -1;
                    while (!endDay.ToString("yyyy-MM-dd").Equals(tempDay.ToString("yyyy-MM-dd")))
                    {
                        wd = wd + 1;
                        tempDay = tempDay.AddDays(1);
                        if (wd == 0)
                        {
                            timeGroupName.Add("周日");
                        }
                        else if (wd == 1)
                        {
                            timeGroupName.Add("周一");
                        }
                        else if (wd == 2)
                        {
                            timeGroupName.Add("周二");
                        }
                        else if (wd == 3)
                        {
                            timeGroupName.Add("周三");
                        }
                        else if (wd == 4)
                        {
                            timeGroupName.Add("周四");
                        }
                        else if (wd == 5)
                        {
                            timeGroupName.Add("周五");
                        }
                        else if (wd == 6)
                        {
                            timeGroupName.Add("周六");
                        }
                        timeGroup.Add(tempDay.ToString("yyyyMMdd"));



                    }

                }
                List<int> numbers = new List<int>();
                List<int> pre_numbers = new List<int>();
              
                DataTableQueryMemberVisitAdapter ad = new DataTableQueryMemberVisitAdapter();
                var table = new DataSynchronizationStanbyLib.KPIDataSet.DataTableQueryMemberVisitDataTable();
                var table2 = new DataSynchronizationStanbyLib.KPIDataSet.DataTableQueryMemberVisitDataTable();
                if (model == 0)
                {
                    table = ad.GetData(startDay, endDay, shopCode);
                    table2 = ad.GetData(startDay2, endDay2, shopCode);
                }
                else
                {
                    table = ad.GetDataBydDay(startDay, endDay, shopCode);
                    table2 = ad.GetDataBydDay(startDay2, endDay2, shopCode);
                }
                
                
                //var table = ad.GetDataBy1(startDay, endDay);
                //var table2 = ad.GetDataBy1(startDay2, endDay2);
                foreach (var item in timeGroup)
                {
                    
                    if (table.Rows.Count > 0)
                    {
                        if (model == 0)
                        {
                            var tableItem = table.Where(p => p.VISITTIME.Equals(item.Substring(0, 2)));
                            if (tableItem.Count() > 0)
                            {
                                numbers.Add((int)tableItem.First().CT);
                            }
                            else
                            {
                                numbers.Add(0);
                            }
                        }
                        else {
                            var tableItem = table.Where(p => p.VISITTIME.Equals(item));
                            if (tableItem.Count() > 0)
                            {
                                numbers.Add((int)tableItem.First().CT);
                            }
                            else
                            {
                                numbers.Add(0);
                            }
                        }

                       
                    }
                    else
                    {
                        numbers.Add(0);
                    }
                    if (table2.Rows.Count > 0)
                    {
                        if (model == 0)
                        {
                            var table2Item = table2.Where(p => p.VISITTIME.Equals(item.Substring(0, 2)));
                            if (table2Item.Count() > 0)
                            {
                                pre_numbers.Add((int)table2Item.First().CT);
                            }
                            else
                            {
                                pre_numbers.Add(0);
                            }
                        }
                        else
                        {
                            string tempDay = "";
                            if (model == 1)
                            {
                                tempDay = DateTime.ParseExact(item, "yyyyMMdd", CultureInfo.CurrentCulture).AddDays(-7).ToString("yyyyMMdd");
                            }
                            else if (model == 2)
                            {
                                tempDay = endDay2.ToString("yyyyMM") + item.Substring(6);
                            }

                            var table2Item = table2.Where(p => p.VISITTIME.Equals(tempDay));
                            if (table2Item.Count() > 0)
                            {
                                pre_numbers.Add((int)table2Item.First().CT);
                            }
                            else
                            {
                                pre_numbers.Add(0);
                            }
                        }
                    }
                    else
                    {
                        pre_numbers.Add(0);
                    }
                }
                List<double> pre_numbers_ct = new List<double>();
                for (int i = 0; i < timeGroup.Count; i++)
                {
                    if (pre_numbers[i] == 0)
                    {
                        if (numbers[i] > 0)
                        {
                            pre_numbers_ct.Add(1);
                        }
                        else
                        {
                            pre_numbers_ct.Add(0);
                        }
                    }
                    else if (numbers[i] == 0)
                    {
                        if (pre_numbers[i] > 0)
                        {
                            pre_numbers_ct.Add(-1);
                        }
                        else
                        {
                            pre_numbers_ct.Add(0);
                        }
                    }
                    else
                    {
                        double temp = (double)(numbers[i] - pre_numbers[i]) / (double)pre_numbers[i];
                        pre_numbers_ct.Add(temp);
                    }
                }
                Hashtable data = new Hashtable();
                data.Add("totalNumber", numbers.Sum());
                data.Add("totalPreNumber", pre_numbers.Sum());
                if ((double)pre_numbers.Sum() == 0)
                {
                    data.Add("totalNumberCt", (double)0);
                }
                else
                {
                    data.Add("totalNumberCt", (double)(numbers.Sum() - pre_numbers.Sum()) / (double)pre_numbers.Sum());
                }
                data.Add("numbers", numbers);
                data.Add("preNumbers", pre_numbers);
                data.Add("preNumbersCt", pre_numbers_ct);
                data.Add("timeGroupName", timeGroupName);
                data.Add("timeGroup", timeGroup);

                return Json(data, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(ex.Message, JsonRequestBehavior.AllowGet);
            }
        }
        public JsonResult GetQueryBackData(DateTime startTime, DateTime endTime, string shopCode, int model)
        {
            try
            {
                List<String> timeGroup = new List<string>();
                timeGroup.Add("00:00");
                timeGroup.Add("01:00");
                timeGroup.Add("02:00");
                timeGroup.Add("03:00");
                timeGroup.Add("04:00");
                timeGroup.Add("05:00");
                timeGroup.Add("06:00");
                timeGroup.Add("07:00");
                timeGroup.Add("08:00");
                timeGroup.Add("09:00");
                timeGroup.Add("10:00");
                timeGroup.Add("11:00");
                timeGroup.Add("12:00");
                timeGroup.Add("13:00");
                timeGroup.Add("14:00");
                timeGroup.Add("15:00");
                timeGroup.Add("16:00");
                timeGroup.Add("17:00");
                timeGroup.Add("18:00");
                timeGroup.Add("19:00");
                timeGroup.Add("20:00");
                timeGroup.Add("21:00");
                timeGroup.Add("22:00");
                timeGroup.Add("23:00");
                List<String> timeGroupName = new List<string>();
                timeGroupName.Add("0点");
                timeGroupName.Add("1点");
                timeGroupName.Add("2点");
                timeGroupName.Add("3点");
                timeGroupName.Add("4点");
                timeGroupName.Add("5点");
                timeGroupName.Add("6点");
                timeGroupName.Add("7点");
                timeGroupName.Add("8点");
                timeGroupName.Add("9点");
                timeGroupName.Add("10点");
                timeGroupName.Add("11点");
                timeGroupName.Add("12点");
                timeGroupName.Add("13点");
                timeGroupName.Add("14点");
                timeGroupName.Add("15点");
                timeGroupName.Add("16点");
                timeGroupName.Add("17点");
                timeGroupName.Add("18点");
                timeGroupName.Add("19点");
                timeGroupName.Add("20点");
                timeGroupName.Add("21点");
                timeGroupName.Add("22点");
                timeGroupName.Add("23点");
                Random rd = new Random();

                DateTime startDay = DateTime.ParseExact(startTime.ToString("yyyy-MM-dd") + " 00:00", "yyyy-MM-dd HH:mm", CultureInfo.CurrentCulture);
                DateTime endDay = DateTime.ParseExact(endTime.ToString("yyyy-MM-dd") + " 23:59", "yyyy-MM-dd HH:mm", CultureInfo.CurrentCulture);
                DateTime startDay2 = startDay;
                DateTime endDay2 = endDay;
                if (model == 0)
                {
                    startDay2 = startDay.AddDays(-1);
                    endDay2 = endDay.AddDays(-1);
                }
                else if (model == 2)
                {
                    startDay2 = startDay.AddMonths(-1);
                    endDay2 = endDay.AddMonths(-1);
                }
                else if (model == 1)
                {
                    startDay2 = startDay.AddDays(-7);
                    endDay2 = endDay.AddDays(-7);

                }
                List<int> numbers = new List<int>();
                List<int> pre_numbers = new List<int>();

                DataTableQueryMemberVisitAdapter ad = new DataTableQueryMemberVisitAdapter();
                var table = ad.GetDataByVistCount(startDay, endDay, shopCode);
                var table2 = ad.GetDataByVistCount(startDay2, endDay2, shopCode);
                //var table = ad.GetDataBy1(startDay, endDay);
                //var table2 = ad.GetDataBy1(startDay2, endDay2);
                foreach (var item in timeGroup)
                {
                    if (table.Rows.Count > 0)
                    {
                        var tableItem = table.Where(p => p.VISITTIME.Equals(item.Substring(0, 2)));
                        if (tableItem.Count() > 0)
                        {
                            numbers.Add((int)tableItem.First().CT);
                        }
                        else
                        {
                            numbers.Add(0);
                        }
                    }
                    else
                    {
                        numbers.Add(0);
                    }
                    if (table2.Rows.Count > 0)
                    {
                        var table2Item = table2.Where(p => p.VISITTIME.Equals(item.Substring(0, 2)));
                        if (table2Item.Count() > 0)
                        {
                            pre_numbers.Add((int)table2Item.First().CT);
                        }
                        else
                        {
                            pre_numbers.Add(0);
                        }
                    }
                    else
                    {
                        pre_numbers.Add(0);
                    }
                }
                List<double> pre_numbers_ct = new List<double>();
                for (int i = 0; i < timeGroup.Count; i++)
                {
                    if (pre_numbers[i] == 0)
                    {
                        if (numbers[i] > 0)
                        {
                            pre_numbers_ct.Add(1);
                        }
                        else
                        {
                            pre_numbers_ct.Add(0);
                        }
                    }
                    else if (numbers[i] == 0)
                    {
                        if (pre_numbers[i] > 0)
                        {
                            pre_numbers_ct.Add(-1);
                        }
                        else
                        {
                            pre_numbers_ct.Add(0);
                        }
                    }
                    else
                    {
                        double temp = (double)(numbers[i] - pre_numbers[i]) / (double)pre_numbers[i];
                        pre_numbers_ct.Add(temp);
                    }
                }
                Hashtable data = new Hashtable();
                data.Add("totalNumber", numbers.Sum());
                data.Add("totalPreNumber", pre_numbers.Sum());
                if ((double)pre_numbers.Sum() == 0)
                {
                    data.Add("totalNumberCt", (double)0);
                }
                else
                {
                    data.Add("totalNumberCt", (double)(numbers.Sum() - pre_numbers.Sum()) / (double)pre_numbers.Sum());
                }
                data.Add("numbers", numbers);
                data.Add("preNumbers", pre_numbers);
                data.Add("preNumbersCt", pre_numbers_ct);
                data.Add("timeGroupName", timeGroupName);
                data.Add("timeGroup", timeGroup);

                return Json(data, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(ex.Message, JsonRequestBehavior.AllowGet);
            }
        }

        public JsonResult GetMemberQueryData(DateTime startTime, DateTime endTime, string shopCode, int model,string model2)
        {
            try { 

            DateTime startDay = DateTime.ParseExact(startTime.ToString("yyyy-MM-dd") + " 00:00", "yyyy-MM-dd HH:mm", CultureInfo.CurrentCulture);
            DateTime endDay = DateTime.ParseExact(endTime.ToString("yyyy-MM-dd") + " 23:59", "yyyy-MM-dd HH:mm", CultureInfo.CurrentCulture);
            DateTime startDay2 = startDay;
            DateTime endDay2 = endDay;
            if (model == 0)
            {
                startDay2 = startDay.AddDays(-1);
                endDay2 = endDay.AddDays(-1);
            }
            else if (model == 2)
            {
                startDay2 = startDay.AddMonths(-1);
                endDay2 = endDay.AddMonths(-1);
            }
            else if (model == 1)
            {
                startDay2 = startDay.AddDays(-7);
                endDay2 = endDay.AddDays(-7);

            }
            DataTableQueryMemberVisitAdapter ad = new DataTableQueryMemberVisitAdapter();
            var table = ad.GetData(startDay, endDay, shopCode);
            var table2 = ad.GetData(startDay2, endDay2, shopCode);
            List<String> char1Group = new List<string>();
            char1Group.Add("1次");
            char1Group.Add("2次");
            char1Group.Add("3次");
            char1Group.Add("3次以上");
            //粘性分析
            List<int> char1Data = new List<int>();
            MEMBER_CTAdapter ad1 = new MEMBER_CTAdapter();
            var table3= ad1.GetData(startDay, endDay, shopCode);
            var time1ct = table3.Where(p => p.CT == 1);
            char1Data.Add(time1ct.Count());
            time1ct = table3.Where(p => p.CT == 2);
            char1Data.Add(time1ct.Count());
            time1ct = table3.Where(p => p.CT == 3);
            char1Data.Add(time1ct.Count());
            time1ct = table3.Where(p => p.CT >3);
            char1Data.Add(time1ct.Count());
                //会员占比
                // MEMBER_CT_2TableAdapter ad3 = new MEMBER_CT_2TableAdapter();
                //var table4 = ad3.GetMemberData(startDay, endDay, shopCode);
                //var table5 = ad3.GetNoMemberData(startDay, endDay, shopCode);
                //会员粘性
                //MEMBER_CT_3TableAdapter ad4 = new MEMBER_CT_3TableAdapter();
                //List<String> char3Group = new List<string>();
                //char3Group.Add("3个月内");
                //char3Group.Add("6个月内");
                //char3Group.Add("12个月内");
                //List<int> char3Data = new List<int>();
                //var table6= ad4.GetData(shopCode, DateTime.Now.AddMonths(3));
                //char3Data.Add((int)table6.Sum(p=>p.CT));
                //table6 = ad4.GetData(shopCode, DateTime.Now.AddMonths(6));
                //char3Data.Add((int)table6.Sum(p => p.CT));
                //table6 = ad4.GetData(shopCode, DateTime.Now.AddMonths(12));
                //char3Data.Add((int)table6.Sum(p => p.CT));
                //性别分析
                MEMBER_CT_5TableAdapter ad6 = new MEMBER_CT_5TableAdapter();
                List<String> char6Group = new List<string>();
                char6Group.Add("20岁以下");
                char6Group.Add("21-30岁");
                char6Group.Add("31-40岁");
                char6Group.Add("41-50岁");
                char6Group.Add("51-60岁");
                char6Group.Add("50岁以上");
                List<int> char6Data = new List<int>();
                var table9 = ad6.GetData(startDay, endDay, shopCode);
                char6Data.Add((int)table9.Where(p => p.AGE <= 20).Sum(p => p.CT));
                char6Data.Add((int)table9.Where(p => p.AGE >= 21 && p.AGE<=30 ).Sum(p => p.CT));
                char6Data.Add((int)table9.Where(p => p.AGE >= 31 && p.AGE <= 40).Sum(p => p.CT));
                char6Data.Add((int)table9.Where(p => p.AGE >= 41 && p.AGE <= 50).Sum(p => p.CT));
                char6Data.Add((int)table9.Where(p => p.AGE >= 51 && p.AGE <= 60).Sum(p => p.CT));
                char6Data.Add((int)table9.Where(p => p.AGE > 60).Sum(p => p.CT));

                MEMBER_CT_4TableAdapter ad5 = new MEMBER_CT_4TableAdapter();
            Hashtable data = new Hashtable();
            if (model2.Equals("1"))
            {
               var table7= ad5.GetMemberData(startDay, endDay, shopCode);
               
                     data.Add("char4_M_Data", table7.Where(p => p.SEX.Equals("M")).Sum(p => p.CT));
                data.Add("char4_S_Data", table7.Where(p => !p.SEX .Equals("M")).Sum(p => p.CT));
            }
            else if (model2.Equals("2"))
            {
                var table7 = ad5.GetNoMemberData(startDay, endDay, shopCode);

                data.Add("char4_M_Data", table7.Where(p => p.SEX.Equals("M")).Sum(p => p.CT));
                data.Add("char4_S_Data", table7.Where(p => !p.SEX.Equals("M")).Sum(p => p.CT));
            }
            else if (model2.Equals("3"))
            {
                var table7 = ad5.GetAllData(startDay, endDay, shopCode);

                data.Add("char4_M_Data", table7.Where(p => p.SEX.Equals("M")).Sum(p => p.CT));
                data.Add("char4_S_Data", table7.Where(p => !p.SEX.Equals("M")).Sum(p => p.CT));
            }


            

            data.Add("totalNumber", table.Sum(p=>p.CT));
            data.Add("totalPreNumber", table2.Sum(p => p.CT));
            if ((double)table2.Sum(p => p.CT) == 0)
            {
                data.Add("totalNumberCt", (double)0);
            }
            else
            {
                data.Add("totalNumberCt", (double)(table.Sum(p => p.CT) - table2.Sum(p => p.CT)) / (double)table2.Sum(p => p.CT));
            }
                MEMBER_CT_6TableAdapter ad7 = new MEMBER_CT_6TableAdapter();
                var table10=ad7.GetData(startDay, endDay, shopCode);


                data.Add("char1Group", char1Group);
            data.Add("char1Data", char1Data);
            
            data.Add("char7_001", table10.Where(p=>p.CG_TYPE_CODE.Equals("001")).Sum(p=>p.CT));
            data.Add("char7_002", table10.Where(p => p.CG_TYPE_CODE.Equals("002")).Sum(p => p.CT));
                data.Add("char7_003", table10.Where(p => p.CG_TYPE_CODE.Equals("003")).Sum(p => p.CT));

                data.Add("char6Group", char6Group);
            data.Add("char6Data", char6Data);
           

            return Json(data, JsonRequestBehavior.AllowGet);
        }
            catch (Exception ex)
            {
                return Json(ex.Message, JsonRequestBehavior.AllowGet);
    }
}

        public JsonResult GetWeekNumber(DateTime dt)
        {
          var ci= CultureInfo.CurrentCulture;
            Hashtable data = new Hashtable();
            data.Add("weekNumber", ci.Calendar.GetWeekOfYear(dt, ci.DateTimeFormat.CalendarWeekRule, ci.DateTimeFormat.FirstDayOfWeek));
            return Json(data, JsonRequestBehavior.AllowGet);
        }
        public DateTime getWeekStartDay(DateTime dt)
        {
            var ci = CultureInfo.CurrentCulture;
            var weeks = ci.Calendar.GetWeekOfYear(dt, ci.DateTimeFormat.CalendarWeekRule, ci.DateTimeFormat.FirstDayOfWeek);
            var year = dt.Year;
            DateTime start = new DateTime(year, 1, 1);
            List<DateTime> days = new List<DateTime>();
            
            for (int i = 0; i < 364; i++)
            {
                start = start.AddDays(1);
                if (ci.Calendar.GetWeekOfYear(start, ci.DateTimeFormat.CalendarWeekRule, ci.DateTimeFormat.FirstDayOfWeek) == weeks)
                {
                    days.Add(start);
                }
            }
           return days.First();
        }
        public DateTime getWeekEndDay(DateTime dt)
        {
            var ci = CultureInfo.CurrentCulture;
            var weeks = ci.Calendar.GetWeekOfYear(dt, ci.DateTimeFormat.CalendarWeekRule, ci.DateTimeFormat.FirstDayOfWeek);
            var year = dt.Year;
            DateTime start = new DateTime(year, 1, 1);
            List<DateTime> days = new List<DateTime>();

            for (int i = 0; i < 364; i++)
            {
                start = start.AddDays(1);
                if (ci.Calendar.GetWeekOfYear(start, ci.DateTimeFormat.CalendarWeekRule, ci.DateTimeFormat.FirstDayOfWeek) == weeks)
                {
                    days.Add(start);
                }
            }
            return days.Last();
        }
        public JsonResult GetWeekDays(int year,int weeks)
        {
            DateTime start=new DateTime(year,1,1);
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
            return Json(data, JsonRequestBehavior.AllowGet);
        }
        public JsonResult GetMonthDays(int year, int month)
        {
            DateTime start = new DateTime(year, month, 1);
           
            var ci = CultureInfo.CurrentCulture;
            Hashtable data = new Hashtable();
           


            data.Add("firstDay", start.ToString("yyyy-MM-dd"));
            data.Add("lastDay", start.AddMonths(1).AddDays(-1).ToString("yyyy-MM-dd"));
            return Json(data, JsonRequestBehavior.AllowGet);
        }

    }
}