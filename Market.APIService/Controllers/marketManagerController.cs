using DataSynchronizationLib.DataSetPopTableAdapters;
using DataSynchronizationLib.MemberDSTableAdapters;
using DataSynchronizationLib.SCMTableAdapters;
using DataSynchronizationStanbyLib;
using DataSynchronizationStanbyLib.DataSetStanbyTableAdapters;
using DataSynchronizationStanbyLib.KPIDataSetTableAdapters;
using Market.APIService.Models;
using Microsoft.AspNet.Identity;
using NFine.Application.APPManage;
using NFine.Application.SystemSecurity;
using NFine.Application.TaskManage;
using NFine.Domain._02_ViewModel;
using NFine.Domain._03_Entity.APPManage;
using NFine.Domain._04_IRepository.APPManage;
using NFine.Domain.Entity.SystemSecurity;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Runtime.InteropServices;
using System.Security.Principal;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;
using Tools;

namespace Market.APIService.Controllers
{
    [Authorize]
    [RoutePrefix("api/marketApi")]
    public class marketManagerController : ApiController
    {
      

        [HttpGet]
        [Route("GetCustomer")]
        public List<CustomerModel> GetCustomer()
        {
            marketShopApp shopApp = new marketShopApp();
            List<marketSalesShopEntity> shops = new List<marketSalesShopEntity>();
            shops.AddRange( shopApp.getShopByUserId(User.Identity.GetUserId()));
            marketSalesApp userapp = new marketSalesApp();
            UserInfoResultModel userinfo = userapp.GetUserInfo(User.Identity.GetUserId());
            shops.AddRange(shopApp.getAllShopByEmpCode(userinfo.SalesNo));

            var t = from e in shops
                    group e by new { Code = e.CUSTOMER_CODE, Name = e.CUSTOMER_NAME } into g
                    select new CustomerModel { Code = g.Key.Code, Name = g.Key.Name };
            return t.OrderBy(p=>p.Name).ToList();
        }



        /// <summary>
        /// 获取战区列表
        /// </summary>
        /// <returns></returns>
        [AllowAnonymous]
        [HttpGet]
        [Route("GetTbus")]
        public List<KeyValueModel> getTbus()
        {
            List<KeyValueModel> data = new List<KeyValueModel>();

            ORG_INFOTableAdapter ad = new ORG_INFOTableAdapter();
            var table = ad.GetKINDData();
            foreach (var row in table)
            {
               
                    data.Add(new KeyValueModel() { key = row.MANAGE_ORG_CODE, keyValue = row.MANAGE_ORG_NAME, isSelected = false });
               
            }
            return data;
           // return ents;
        }

        /// <summary>
        /// 年龄段列表
        /// </summary>
        /// <returns></returns>
        [AllowAnonymous]
        [HttpGet]
        [Route("GetAgeList")]
        public List<AgeModel> GetAgeList()
        {


            JS5_S12_SALES_AGE_PERIODTableAdapter ad = new JS5_S12_SALES_AGE_PERIODTableAdapter();
            var ages = ad.GetData();
            var model = new List<AgeModel>();
            foreach (var age in ages)
            {
                model.Add(new AgeModel() { Code = age.AGE_PERIOD_CODE, Name = age.AGE_PERIOD_NAME });
            }
            return model;
        }
        /// <summary>
        /// 获取地区列表
        /// </summary>
        /// <param name="type">类型 0:省份,1:市</param>
        /// <param name="pCode">查询省份时传ALL，查询市时传省份代码</param>
        /// <returns></returns>
        [AllowAnonymous]
        [HttpGet]
        [Route("GetAreaList")]
        public List<AgeModel> GetAreaList(string type,string pCode)
        {
            string leve = "province";

            if (type.Equals("0"))
            {
                leve = "province";
                pCode = "ALL";
            }
            else
            {
                leve = "city";
            }

            JS5_AREA_CODETableAdapter ad = new JS5_AREA_CODETableAdapter();
            var ages = ad.GetDataBy(leve, pCode);
            var model = new List<AgeModel>();
            foreach (var age in ages)
            {
                model.Add(new AgeModel() { Code = age.AREA_CODE_ID, Name = age.AREA_NAME });
            }
            return model;
        }



        /// <summary>
        /// 查询订单列表
        /// </summary>
        /// <param name="customerCode">客户编号</param>
        /// <param name="status">订单状态 1:未接單 2:已接單 3:已發貨 4:已到貨 5:已簽收
        /// </param>
        /// <returns></returns>
        [HttpGet]
        [AllowAnonymous]
        [Route("GetOrderList")]
        public List<OrderModel> GetOrderList(string customerCode,int status)
        {
            try { 
            List<OrderModel> models = new List<OrderModel>();
            marketSalesActualApp app = new marketSalesActualApp();
            var table= app.GetOrderList(customerCode, status);
            foreach (DataSynchronizationLib.DataSetPop.V_SALES_ORDER_APP_QUERYRow row in table.Rows)
            {
                models.Add(new OrderModel() { ARRIVAL_DATE=row.ARRIVAL_DATE.ToString("yyyy/MM/dd"), CUSTOMER_CODE=row.CUSTOMER_CODE, CUSTOMER_NAME=row.CUSTOMER_NAME,
                 DC_NAME=row.DC_NAME, FACTORY_CITY=row.FACTORY_CITY, LOGIST_CAR_CODE=row.LOGIST_CAR_CODE, LOGIST_DATE=row.LOGIST_DATE.ToString("yyyy/MM/dd")
                , MACHINE_MODEL_NO=row.MACHINE_MODEL_NO, ORDER_CODE=row.ORDER_CODE, ORDER_DATE=row.ORDER_DATE.ToString("yyyy/MM/dd")
                , ORDER_SALE_QTY=(int)row.ORDER_SALE_QTY, PROCESS_FLAG=(int)row.PROCESS_FLAG, PROCESS_FLAG_NAME=row.PROCESS_FLAG_NAME, QTY=(int)row.QTY, RECEIVE_CITY=row.RECEIVE_CITY,
                 RECEIVE_PROVINCE=row.RECEIVE_PROVINCE, SIGNUP_QTY=(int)row.SIGNUP_QTY , SIGNUP_UPLOAD_DATE=row.SIGNUP_UPLOAD_DATE.ToString("yyyy/MM/dd")
                });
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


        [HttpGet]
        [Route("GetVer")]
        [AllowAnonymous]
        public VerModel GetVer()
        {
            sysappverApp app = new sysappverApp();
            sysappverEntity ents = app.getVer();
            return new VerModel() { id=ents.id, DowloandUrl=ents.DowloandUrl, Type=ents.Type, Ver=ents.Ver};
        }

        [HttpGet]
        [Route("GetMessage")]
        public List<MessageModel> GetMessage()
        {

            marketMessageListApp app = new marketMessageListApp();
            List<marketMessageListEntity> ents = app.getMessageList();
            List<MessageModel> messages = new List<MessageModel>();
            foreach (marketMessageListEntity ent in ents)
            {
                messages.Add(new MessageModel()
                {
                    ContextUrl = ent.Message_Url,
                    Desc = ent.POST_PAGE_MEMO,
                    Title = ent.Message_Title,
                    MessageId = ent.id,
                    MessageTime = ent.CREATE_DATE.Value
                });
            }

            

            return messages;
        }
        [HttpGet]
        [Route("GetPostMessage")]
        public List<MessageModel> GetPostMessage()
        {

            marketMessageListApp app = new marketMessageListApp();
            marketSalesApp userapp = new marketSalesApp();

            UserInfoResultModel t = userapp.GetUserInfo(User.Identity.GetUserId());
            var ents = app.getMessageList(t.SalesNo);
            List<MessageModel> messages = new List<MessageModel>();
            foreach (var ent in ents)
            {
                messages.Add(new MessageModel()
                {
                    ContextUrl = ent.Message_Url,
                    Desc = ent.Message_Title,
                    Title = ent.Message_Title,
                    MessageId = ent.id,
                    MessageTime = ent.CREATE_DATE.Value, MessageType=ent.MessageType
                });
            }



            return messages;
        }
        [HttpPost]
        [Route("DeleteMessage")]
        public async Task<IHttpActionResult> DeleteMessage(string id)
        {
            marketMessageListApp app = new marketMessageListApp();
            var res = app.DeleteMessage(id);
            if (!res.isOk)
            {
                return BadRequest(res.errorMessage);
            }
            return Ok();
        }
        /// <summary>
        /// 接待会员
        /// </summary>
        /// <param name="mfid">mfMemberId</param>
        /// <param name="bid">bid</param>
        /// <returns></returns>
        [HttpPost]
        [Route("readPushMember")]
        public async Task<IHttpActionResult> readPushMember(string mfid,string bid)
        {
            try
            {
                TaskMemberApp app = new TaskMemberApp();
                app.readPushMessage(mfid, bid, User.Identity.GetUserId());
                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        /// <summary>
        /// 抓取推送会员消息
        /// </summary>
        /// <param name="bid"></param>
        /// <param name="shopCode"></param>
        /// <returns></returns>
        [HttpGet]
        [AllowAnonymous]
        [Route("GetPushMemberListV2")]
        public List<MemberListModel> GetPushMemberListV2(string shopCode)
        {
            string no = "";
            try
            { 
            TaskMemberApp app = new TaskMemberApp();
            marketShopApp shopApp = new marketShopApp();
            var items = app.getData(shopCode);
            List<MemberListModel> list = new List<MemberListModel>();
            V_CRM_MEMBER_LIST_APPTableAdapter ad = new V_CRM_MEMBER_LIST_APPTableAdapter();
               
            foreach (var item in items)
            {
                //item.beachId
                var temp = new MemberListModel()
                {
                    memberNo = item.memberId,
                    mf_memberNo = item.mfMemberId,
                    shop = shopApp.getShopName(item.shopCode),
                    pic_Url = item.picUrl == null ? "" : item.picUrl.Replace("/data/upload", "https://iretailerapp.flnet.com/Messages"),
                    time = item.InTime.Value.ToString("yyyy-MM-dd HH:mm"),
                    name = "",
                    IsRead = item.IsRead
                    , beachId=item.beachId
                };
                    no = item.mfMemberId;
                var table = ad.GetDataByNo2(item.mfMemberId);
                    if (table.Count > 0)
                    {
                        temp.name = table[0].IsMEMBER_NAMENull() ? "" : table[0].MEMBER_NAME;
                        temp.lookMacs = table[0].IsFOLLOW_PRODUCTNull() ? "" : table[0].FOLLOW_PRODUCT;
                        temp.vistaCount = table[0].IsVISIT_COUNTNull() ? 0 : (int)table[0].VISIT_COUNT;
                        temp.CG_TYPE_NAME = table[0].CG_TYPE_NAME;
                        temp.MEMBER_ADDR = table[0].MEMBER_ADDR;
                        temp.MEMBER_LEVEL_CODE = table[0].MEMBER_LEVEL_CODE;
                        temp.MEMBER_LEVEL_NAME = table[0].MEMBER_LEVEL_NAME;
                        temp.SCAN_AREA = table[0].SCAN_AREA;
                        temp.phoneNumber = table[0].MOBILE;

                    }
                    else
                    {
                        temp.name ="";
                        temp.lookMacs = "";
                        temp.vistaCount = 0;
                        temp.CG_TYPE_NAME = "";
                        temp.MEMBER_ADDR = "";
                        temp.MEMBER_LEVEL_CODE ="";
                        temp.MEMBER_LEVEL_NAME = "";
                        temp.SCAN_AREA = "";
                        temp.phoneNumber = "";
                    }
                if (temp.lookMacs == null)
                {
                    temp.lookMacs = "无";
                }
                list.Add(temp);

            }
            return list;
            }
            catch (Exception ex)
            {
                var resp = new HttpResponseMessage(HttpStatusCode.InternalServerError)
                {
                    Content = new StringContent(no),
                    ReasonPhrase = "error"
                };
                throw new HttpResponseException(resp);

            }
        }
        /// <summary>
        /// 抓取推送会员消息
        /// </summary>
        /// <param name="bid"></param>
        /// <param name="shopCode"></param>
        /// <returns></returns>
        [HttpGet]
        [AllowAnonymous]
        [Route("GetPushMemberList")]
        public List<MemberListModel> GetPushMemberList(string bid, string shopCode)
        {
            TaskMemberApp app = new TaskMemberApp();
            marketShopApp shopApp = new marketShopApp();
            var items = app.getData(bid, shopCode);
            List<MemberListModel> list = new List<MemberListModel>();
            V_CRM_MEMBER_LIST_APPTableAdapter ad = new V_CRM_MEMBER_LIST_APPTableAdapter();

            foreach (var item in items)
            {

                var temp = new MemberListModel()
                {
                    memberNo = item.memberId,
                    mf_memberNo = item.mfMemberId,
                    shop = shopApp.getShopName(item.shopCode),
                    pic_Url = item.picUrl == null ? "" : item.picUrl.Replace("/data/upload", "https://iretailerapp.flnet.com/Messages"),
                    time = item.InTime.Value.ToString("yyyy-MM-dd HH:mm"),
                    name = "", IsRead=item.IsRead
                };
                var table = ad.GetDataByNo(item.mfMemberId);
                if (table.Count > 0)
                {
                    temp.name = table[0].IsMEMBER_NAMENull() ? "" : table[0].MEMBER_NAME;
                    temp.lookMacs = table[0].IsFOLLOW_PRODUCTNull() ? "" : table[0].FOLLOW_PRODUCT;
                    temp.vistaCount = table[0].IsVISIT_COUNTNull() ? 0 : (int)table[0].VISIT_COUNT;
                }
                if (temp.lookMacs == null)
                {
                    temp.lookMacs = "无";
                }
                list.Add(temp);

            }
            return list;
        }
        /// <summary>
        /// 抓取抓拍列表
        /// </summary>
        /// <param name="shopCode"></param>
        /// <param name="day"></param>
        /// <returns></returns>
        [HttpGet]
        [AllowAnonymous]
        [Route("GetNoMemberList")]
        public List<NoMemberModel> GetNoMemberList(string shopCode,string day)
        {
            try { 
            List<NoMemberModel> model = new List<NoMemberModel>();
            V_CRM_NO_MEMBERTableAdapter ad = new V_CRM_NO_MEMBERTableAdapter();
            var table = ad.GetData(day,shopCode);
            foreach ( var row in table) {
                model.Add(new NoMemberModel() { MEMBER_NO=row.MEMBER_NO, MF_MEMBER_ID=row.MF_MEMBER_ID, VISIT_TIME=row.VISIT_TIME
                , picUrl= row.IsIMGFACETIMENull()?"":row.IMGFACETIME.Replace("/data/upload", "https://iretailerapp.flnet.com/Messages"), MEMBER_TYPE=row.CG_TYPE_CODE, MEMBER_TYPENAME=row.CG_TYPE_NAME
                });
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
}
        /// <summary>
        /// 查询会员明细
        /// </summary>
        /// <param name="memberNumber"></param>
        /// <returns></returns>
        [HttpGet]
        [AllowAnonymous]
        [Route("getMember")]
        public MemberMode getMember(string memberNumber)
        {
            try
            {
                marketShopApp shopApp = new marketShopApp();

                V_CRM_MEMBER_LIST_APPTableAdapter ad = new V_CRM_MEMBER_LIST_APPTableAdapter();
                V_SHOP_YHJ_SALES_HEADTableAdapter adOrder = new V_SHOP_YHJ_SALES_HEADTableAdapter();
                V_CRM_MEMBER_IOSHOP_LOG_APPTableAdapter IOAD = new V_CRM_MEMBER_IOSHOP_LOG_APPTableAdapter();
                V_SHOP_YHJ_SALES_DETAILTableAdapter adOrderDetail = new V_SHOP_YHJ_SALES_DETAILTableAdapter();
                V_SALES_TRANS_APP_QUERYTableAdapter adSalesTrans = new V_SALES_TRANS_APP_QUERYTableAdapter();
                var table = ad.GetDataByNo(memberNumber);
                var faceTimeAd = new FactTimeImageAdapter();
                if (table.Count > 0)
                {
                    var row = table[0];
                    var faceTime = faceTimeAd.GetData(row.ID);
                    string faceUrl = "";
                    if (faceTime.Rows.Count > 0)
                    {
                        faceUrl = faceTime[0].IsIMGFACETIMENull() ? "" : faceTime[0].IMGFACETIME.Replace("/data/upload", "https://iretailerapp.flnet.com/Messages");
                    }

                    MemberMode model = new MemberMode() { adder = row.MEMBER_ADDR, LAST_BUY_MODEL = row.LAST_BUY_MODEL,
                        LAST_INSHOP = shopApp.getShopName(row.LAST_INSHOP_CODE), LAST_INSHOP_TIME = row.LAST_INSHOP_TIME,
                        MEMBER_NO = row.MEMBER_NO, MF_MEMBER_ID = row.MF_MEMBER_ID, MEMO = row.MEMO, name = row.MEMBER_NAME, phoneNumber = row.MOBILE, weiChat = row.WECHAT_NO,
                        VISIT_COUNT = (int)row.VISIT_COUNT,
                        picRUL = faceUrl
                        ,
                        age = (int)row.AGE,
                        MemberType = row.CG_TYPE_NAME,
                        regShop = shopApp.getShopName(row.REG_SHOP_CODE), MEMBER_CARDNO = row.MEMBER_CARDNO, MEMBER_POINT = (int)row.MEMBER_POINT
                         , CG_TYPE_NAME = row.CG_TYPE_NAME,
                        MEMBER_ADDR = row.MEMBER_ADDR,
                        MEMBER_LEVEL_CODE = row.MEMBER_LEVEL_CODE,
                        MEMBER_LEVEL_NAME = row.MEMBER_LEVEL_NAME,
                        SCAN_AREA = row.SCAN_AREA,
                        AVGMONTH_VISITCOUNT = (int)row.AVGMONTH_VISITCOUNT,
                        FAST_TIME = row.FISRT_INSHOP_TIME,six=row.SEX
                    };
                    model.IOList = new List<Member_IOSHOPMode>();
                    model.picRULOthers = new List<string>();
                   
                    var iologs = IOAD.GetDataByMFID(row.MF_MEMBER_ID);
                    int i = 1;
                    foreach (var item in iologs.Take(10))
                    {
                        model.IOList.Add(new Member_IOSHOPMode() { SHOP_NAME = item.SHOP_NAME, Time = item.VISIT_TIME });
                        if (i < 5)
                        {
                            //string[] ss = item.IMG_FACE_FRAME.Split("/".ToCharArray(), StringSplitOptions.RemoveEmptyEntries);
                            //string month = DateTime.ParseExact(ss[2],"yyyy-MM-dd", CultureInfo.CurrentCulture).ToString("yyyyMM");
                            //string fileNmae = ss[4];
                            model.picRULOthers.Add(item.IMGFACETIME.Replace("/data/upload", "https://iretailerapp.flnet.com/Messages"));
                            i = i + 1;
                        }
                    }
                    model.ByList = new List<Member_ByMode>();
                    model.ByList2 = new List<Member_ByModeMast>();
                    
                    if (!row.IsMEMBER_IDNull())
                    {
                        var tempByList = new List<Member_ByModeMast>();
                       
                        var orders = adOrder.GetDataByMemberId(row.MEMBER_ID);
                        if (orders.Rows.Count > 0)
                        {
                            foreach (var order in orders)
                            {
                                Member_ByModeMast mast = new Member_ByModeMast() { byTime = order.ORDER_DATE, amt=(double)order.PAY_AMT };
                                mast.detail = new List<Member_ByMode>();
                                var details = adOrderDetail.GetDataByOrderNo(order.ORDER_NO);
                                if (details.Rows.Count > 0)
                                {
                                    foreach (var detail in details)
                                    {
                                        mast.detail.Add(new Member_ByMode() { byTime = order.ORDER_DATE, MAC = detail.PROD_CODE, MACName = detail.PROD_NAME, price = (double)detail.SALES_PRICE, qty = (int)detail.SALES_QTY });
                                    }
                                }
                                model.ByList2.Add(mast);
                            }
                        }
                        var salesOrders = adSalesTrans.GetDataByMemberId(row.MEMBER_ID);
                        if (salesOrders.Rows.Count > 0)
                        {
                            foreach (var salesOrder in salesOrders)
                            {
                                Member_ByModeMast mast = new Member_ByModeMast() { byTime = salesOrder.SALES_DATE,amt= (double)salesOrder .SALES_AMOUNT};
                                mast.detail = new List<Member_ByMode>() ;
                                mast.detail.Add(new Member_ByMode() { byTime = salesOrder.SALES_DATE, MAC = salesOrder.MACHINE_MODEL_NO, MACName = salesOrder.MACHINE_MODEL_NO, price = (double)salesOrder.SALES_PRICE, qty = (int)salesOrder.SALES_QTY });
                                model.ByList2.Add(mast);
                            }
                        }
                    }
                    ISSSP_SMART_PRODNM_COMMENDTableAdapter smartAd = new ISSSP_SMART_PRODNM_COMMENDTableAdapter();
                    var sMartTable = smartAd.GetDataByAge(row.AGE.ToString());

                    model.SmartProd = new List<Member_Smart>();
                    model.SmartType = new List<Member_Smart>();
                    model.SmartBrod = new List<Member_Smart>();
                    model.SmartList = new List<Member_SmartPrd>();
                    if (sMartTable.Rows.Count>0)
                    {
                        var prd = sMartTable.First();
                        model.SmartList.Add(new Member_SmartPrd() { type = prd.PROD_TYPE, bord = prd.BRAND, name = prd.PRODUCT_NAME+"("+prd.PROD_ID+")" });
                        
                    }
                    model.SmartBrod.AddRange( sMartTable.GroupBy(x => new { x.BRAND }).Select(p => new Member_Smart { name = p.Key.BRAND, isS = false, rick = 0 }).ToList());
                    model.SmartType.AddRange(sMartTable.GroupBy(x => new { x.PROD_TYPE }).Select(p => new Member_Smart { name = p.Key.PROD_TYPE, isS = false, rick = 0 }).ToList());

                    model.SmartProd.AddRange(sMartTable.GroupBy(x => new { x.PRODUCT_NAME }).Select(p => new Member_Smart { name = p.Key.PRODUCT_NAME, isS = false, rick = 0 }).ToList());

                    // V_CRM_MEMBER_HISSALES_APPTableAdapter byAD = new V_CRM_MEMBER_HISSALES_APPTableAdapter();
                    //var bylogs = byAD.GetData(row.MOBILE);
                    //foreach (var item in bylogs)
                    //{
                    //  model.ByList.Add(new Member_ByMode() { byTime = item.BUY_DATE, MAC = item.BUY_MODEL, qty = (int)item.BUY_QTY });
                    // }

                    model.lookMacsList = new List<string>();
                    if (!row.IsFOLLOW_PRODUCTNull())
                    {
                        model.lookMacsList.AddRange(row.FOLLOW_PRODUCT.Split(",".ToCharArray()));
                            }
                    return model;
                }
                return new MemberMode();
               
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
        /// 到访记录
        /// </summary>
        /// <param name="day">yyyy-MM-dd</param>
        /// <param name="shopCode"></param>
        /// <returns></returns>
        [HttpGet]
        [AllowAnonymous]
        [Route("GetVISITLog")]
        public List<MemberListModel> GetVISITLog(string day, string shopCode)
        {
            try
            {
                TaskMemberApp app = new TaskMemberApp();
            marketShopApp shopApp = new marketShopApp();
            
            var items = app.getData(DateTime.ParseExact(day + " 00:00:00","yyyy-MM-dd HH:mm:ss", CultureInfo.CurrentCulture), DateTime.ParseExact(day + " 23:59:59", "yyyy-MM-dd HH:mm:ss", CultureInfo.CurrentCulture), shopCode);
            List<MemberListModel> list = new List<MemberListModel>();
            V_CRM_MEMBER_APPTableAdapter ad = new V_CRM_MEMBER_APPTableAdapter();

            foreach (var item in items)
            {

                var temp = new MemberListModel()
                {
                    memberNo = item.memberId,
                    mf_memberNo = item.mfMemberId,
                    shop = shopApp.getShopName(item.shopCode),
                    pic_Url = item.picUrl == null ? "" : item.picUrl.Replace("/data/upload", "https://iretailerapp.flnet.com/Messages"),
                    time = item.InTime.Value.ToString("yyyy-MM-dd HH:mm"),
                    name = "",
                    IsRead = item.IsRead
                };
                var table = ad.GetDataByNo(item.mfMemberId);
                if (table.Count > 0)
                {
                    temp.name = table[0].IsMEMBER_NAMENull() ? "" : table[0].MEMBER_NAME;
                    temp.lookMacs = table[0].IsFOLLOW_PRODUCTNull() ? "" : table[0].FOLLOW_PRODUCT;
                    temp.vistaCount = table[0].IsVISIT_COUNTNull() ? 0 : (int)table[0].VISIT_COUNT;
                }
                list.Add(temp);

            }
            return list;
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
        /// 查询会员列表
        /// </summary>
        /// <param name="phoneNumber">电话号码</param>
        /// <param name="memberNumber">会员编号</param>
        /// <param name="shopCode">门店</param>
        /// <returns></returns>
        [HttpGet]
        [AllowAnonymous]
        [Route("GetMemberListV2")]
        public List<MemberQueryMode> GetMemberListV2(string phoneNumber, string memberNumber,string shopCode,int page=1)
        {
            try
            {
                List<MemberQueryMode> model = new List<MemberQueryMode>();
                V_CRM_MEMBER_LIST_APPTableAdapter ad = new V_CRM_MEMBER_LIST_APPTableAdapter();
                // DataSynchronizationLib.SCM.V_CRM_MEMBER_APPDataTable table;
                var faceTimeAd = new FactTimeImageAdapter();
                //if (phoneNumber == null) phoneNumber = "";
                //if (memberNumber == null) memberNumber = "";

                JS5_S12_CRM_MEMBER_MTableAdapter tdAd = new JS5_S12_CRM_MEMBER_MTableAdapter();
                object refObje = new object();
                var table = new DataSynchronizationLib.MemberDS.JS5_S12_CRM_MEMBER_MDataTable();
                if (phoneNumber != null && memberNumber != null)
                {
                    table = tdAd.GetData(phoneNumber, memberNumber);
                }
                else
                {
                    table = tdAd.GetAllData(page);
                }
                /*if (memberNumber==null ||memberNumber.Equals(""))
                {
                    table = tdAd.GetAllData(page, shopCode);
                }
                else
                {
                    table = tdAd.GetAllData(page, shopCode);
                    //table = tdAd.GetDataByNumber(page,memberNumber,phoneNumber);
                }*/


                foreach (var item in table)
                {
                    /*
                    model.Add(new MemberQueryMode()
                    {
                        MEMBER_NO = item.ID,
                        
                    });
                    */
                   
                    var table2 = ad.GetDataByMemberId(item.ID);
                    if (table2.Rows.Count > 0)
                    {
                        var row = table2.First();
                        var faceTime = faceTimeAd.GetData(row.ID);
                        string faceUrl = "";
                        if (faceTime.Rows.Count > 0)
                        {
                            faceUrl = faceTime[0].IsIMGFACETIMENull() ? "" : faceTime[0].IMGFACETIME.Replace("/data/upload", "https://iretailerapp.flnet.com/Messages");
                        }
                        model.Add(new MemberQueryMode()
                        {
                            MEMBER_NO = row.MEMBER_NO,
                            MF_MEMBER_ID = row.MF_MEMBER_ID,
                            name = row.MEMBER_NAME,
                            picUrl = faceUrl,
                            RegeditTime = row.FISRT_INSHOP_TIME,
                            type = row.CG_TYPE_NAME,
                            phoneNumber = row.MOBILE,
                            vistaCount = (int)row.VISIT_COUNT,
                            CG_TYPE_NAME = row.CG_TYPE_NAME,
                            MEMBER_ADDR = row.MEMBER_ADDR,
                            MEMBER_LEVEL_CODE = row.MEMBER_LEVEL_CODE,
                            MEMBER_LEVEL_NAME = row.MEMBER_LEVEL_NAME,
                            SCAN_AREA = row.SCAN_AREA,
                        });
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
        }
        /// <summary>
        /// 查询会员列表
        /// </summary>
        /// <param name="phoneNumber">电话号码</param>
        /// <param name="memberNumber">会员编号</param>
        /// <param name="page">页数</param>
        /// <returns></returns>
        [HttpGet]
        [AllowAnonymous]
        [Route("GetMemberList")]
        public List<MemberQueryMode> GetMemberList(string phoneNumber, string memberNumber,int page=1)
        {
            try
            {
                List<MemberQueryMode> model = new List<MemberQueryMode>();
                V_CRM_MEMBER_LIST_APPTableAdapter ad = new V_CRM_MEMBER_LIST_APPTableAdapter();
                // DataSynchronizationLib.SCM.V_CRM_MEMBER_APPDataTable table;
                var faceTimeAd = new FactTimeImageAdapter();
                //if (phoneNumber == null) phoneNumber = "";
                //if (memberNumber == null) memberNumber = "";

                JS5_S12_CRM_MEMBER_MTableAdapter tdAd = new JS5_S12_CRM_MEMBER_MTableAdapter();
                object refObje = new object();
                var table = new DataSynchronizationLib.MemberDS.JS5_S12_CRM_MEMBER_MDataTable();
                if (phoneNumber != null && memberNumber != null)
                {
                    table = tdAd.GetData(phoneNumber, memberNumber);
                }
                else
                {
                    table = tdAd.GetAllData(page);
                }
                /*if (memberNumber==null ||memberNumber.Equals(""))
                {
                    table = tdAd.GetAllData(page, shopCode);
                }
                else
                {
                    table = tdAd.GetAllData(page, shopCode);
                    //table = tdAd.GetDataByNumber(page,memberNumber,phoneNumber);
                }*/


                foreach (var item in table)
                {
                    /*
                    model.Add(new MemberQueryMode()
                    {
                        MEMBER_NO = item.ID,
                        
                    });
                    */

                    var table2 = ad.GetDataByMemberId(item.ID);
                    if (table2.Rows.Count > 0)
                    {
                        var row = table2.First();
                        var faceTime = faceTimeAd.GetData(row.ID);
                        string faceUrl = "";
                        if (faceTime.Rows.Count > 0)
                        {
                            faceUrl = faceTime[0].IsIMGFACETIMENull() ? "" : faceTime[0].IMGFACETIME.Replace("/data/upload", "https://iretailerapp.flnet.com/Messages");
                        }
                        model.Add(new MemberQueryMode()
                        {
                            MEMBER_NO = row.MEMBER_NO,
                            MF_MEMBER_ID = row.MF_MEMBER_ID,
                            name = row.MEMBER_NAME,
                            picUrl = faceUrl,
                            RegeditTime = row.FISRT_INSHOP_TIME,
                            type = row.CG_TYPE_NAME,
                            phoneNumber = row.MOBILE,
                            vistaCount = (int)row.VISIT_COUNT,
                            CG_TYPE_NAME = row.CG_TYPE_NAME,
                            MEMBER_ADDR = row.MEMBER_ADDR,
                            MEMBER_LEVEL_CODE = row.MEMBER_LEVEL_CODE,
                            MEMBER_LEVEL_NAME = row.MEMBER_LEVEL_NAME,
                            SCAN_AREA = row.SCAN_AREA,
                        });
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
        }

        /// <summary>
        /// 会员注册
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("RegesitMember")]
        public async Task<IHttpActionResult> RegesitMember(MemberRegestMode model)
        {
            try
            {
                JS5_S12_CRM_MEMBER_UPLOADTableAdapter ad = new JS5_S12_CRM_MEMBER_UPLOADTableAdapter();
                string id = Guid.NewGuid().ToString();
                marketSalesApp app = new marketSalesApp();

                V_CRM_MEMBER_APPTableAdapter queryAd = new V_CRM_MEMBER_APPTableAdapter();
                var table = queryAd.GetDataByPhone(model.phoneNumber);
                if (table.Count > 0)
                {
                    if (!table[0].MF_MEMBER_ID.Equals(model.MF_MEMBER_ID))
                    {
                        return BadRequest("电话号码已经被注册，请确定电话号码正确性！");
                    }
                }


                //string shopCode = "";
                //marketShopApp shopApp = new marketShopApp();
                //List<marketSalesShopEntity> shops = shopApp.getShopByUserId(User.Identity.GetUserId());
                //if (shops.Count > 0)
                //{
                //  shopCode = shops[0].SHOP_CODE;
                ///}
                UserInfoResultModel t = app.GetUserInfo(User.Identity.GetUserId());
                ad.InsertQuery(id, 1, t.SalesNo, System.DateTime.Now.ToString("yyyyMMddHHmmss"), System.DateTime.Now.ToString("yyyyMMddHHmmss"), "APP@" + System.DateTime.Now.ToString("yyyyMMddHHmmss"),
                    "APP@" + System.DateTime.Now.ToString("yyyyMMddHHmmss"), 1, "FLNET", model.MF_MEMBER_ID, model.SHOP_CODE, t.SalesNo, model.MEMBER_NO, model.name, "", "", model.phoneNumber,
                    "", model.weiChat, null, 0, model.six, "", model.adder, "", "", "", "", "", "",model.MEMO, model.bayMac, model.number, System.DateTime.Now.ToString("yyyyMMddHHmmss"), model.lookMacs, 1, t.SalesNo, DateTime.Now, DateTime.Now);
                SCMQueriesTableAdapter spad = new SCMQueriesTableAdapter();
                String outMessage = "";
                spad.SP_CRM_MEMBER_UPLOAD_APP(model.MF_MEMBER_ID, id, out outMessage);
                if (!outMessage.Equals("OK"))
                {
                    throw new Exception(outMessage);
                }
                return Ok(model);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        /// <summary>
        /// 注册推送
        /// </summary>
        /// <param name="pushId">友盟客户端推送ID</param>
        /// <param name="dviceType">IOS/Android</param>
        /// <returns></returns>
        [HttpPost]
        [Route("RegestPush")]
        public async Task<IHttpActionResult> RegestPush(string pushId, string dviceType)
        {
            marketMessageListApp app = new marketMessageListApp();
            var res = app.regDevice(User.Identity.GetUserId(), dviceType, pushId);
            if (!res.isOk)
            {
                return BadRequest(res.errorMessage);
            }
            return Ok();
        }

        [HttpGet]
        [Route("GetBrandInfo")]
        public List<BandModel> GetBrandInfo()
        {
            List<BandModel> models = new List<BandModel>();
            marketBrandApp brandApp = new marketBrandApp();
            var brands = brandApp.GetBrnadInfo();
            foreach (marketBrandEntity ent in brands)
            {
                models.Add(new BandModel() { BRAND_CODE = ent.BRAND_CODE, BRAND_NAME = ent.BRAND_NAME });
            }
            return models;

        }
       /// <summary>
       /// 获取品牌
       /// </summary>
       /// <returns></returns>
        [HttpGet]
        [AllowAnonymous]
        [Route("GetOtherBrandInfo")]
        public List<BandModel> GetOtherBrandInfo()
        {
            try { 
            List<BandModel> models = new List<BandModel>();
            marketBrandApp brandApp = new marketBrandApp();
            var brands = brandApp.GetBrnadOtherInfo();
            foreach ( DataSynchronizationLib.DataSetPop.V_COMPETITOR_MACHINERow  ent in brands)
            {
                if (models.Find(p => p.BRAND_CODE.Equals(ent.BRAND_ID))==null)
                {
                    models.Add(new BandModel() { BRAND_CODE = ent.BRAND_ID, BRAND_NAME = ent.BRAND });
                }
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
        /// 获取尺寸信息
        /// </summary>
        /// <param name="bandCode">品牌</param>
        /// <returns></returns>
        [HttpGet]
        [AllowAnonymous]
        [Route("GetOtherTvSizeInfo")]
        public List<tvSizeViewModel> GetOtherTvSizeInfo(string bandCode)
        {
            marketTvSizeApp app = new marketTvSizeApp();
            List<marketTvsizeinfoEntity> ents = app.getTvSize();
            List<tvSizeViewModel> model = new List<tvSizeViewModel>();

            marketBrandApp brandApp = new marketBrandApp();
            
            var brands = brandApp.GetBrnadOtherInfo();
            var b2 = brands.Where(p=>p.BRAND_ID.Equals(bandCode)).OrderBy(p => p.TVSIZE);
            foreach (DataSynchronizationLib.DataSetPop.V_COMPETITOR_MACHINERow ent in b2)
            {
                if (model.Find(p => p.T_TVSIZEID.Equals(ent.TVSIZE.ToString())) == null)
                {
                    model.Add(new tvSizeViewModel() { T_TVSIZEID = ent.TVSIZE.ToString(), T_TVSIZENAME = ent.TVSIZE.ToString() });
                }
            }
            return model;
        }
        /// <summary>
        /// 取得型号
        /// </summary>
        /// <param name="bandCode">品牌</param>
        /// <param name="tvSize">尺寸</param>
        /// <returns></returns>
        [HttpGet]
        [AllowAnonymous]
        [Route("GetOtherMachineInfo")]
        public List<machineViewModel> GetOtherMachineInfo(string bandCode,string tvSize)
        {
           
            List<machineViewModel> Machines = new List<machineViewModel>();

            marketTvSizeApp app = new marketTvSizeApp();
            List<marketTvsizeinfoEntity> ents = app.getTvSize();
            

            marketBrandApp brandApp = new marketBrandApp();

            var brands = brandApp.GetBrnadOtherInfo(bandCode,int.Parse(tvSize));
            var b2 = brands.OrderBy(p => p.TVSIZE);
            foreach (DataSynchronizationLib.DataSetPop.V_COMPETITOR_MACHINERow ent in b2)
            {
                if (Machines.Find(p => p.MACHINE_MODEL_NO.Equals(ent.MACHINE_MODEL_NO)) == null)
                {
                    Machines.Add(new machineViewModel()
                    {
                        
                        MACHINE_MODEL_NO = ent.MACHINE_MODEL_NO,
                        T_TVSIZEID = tvSize,
                        BANDCODE = bandCode,
                        BANDNAME = bandCode
                    });
                }
            }
            return Machines;
        }

        /// <summary>
        /// 抓取活动列表
        /// </summary>
        /// <param name="salesNo"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("GetActiveListByDate")]
        public List<Models.ActiveModel> GetActiveListByDate(String salesNo,String day,String shopCode)
        {
            List<Models.ActiveModel> models = new List<Models.ActiveModel>();
            marketSalesActiveListApp app = new marketSalesActiveListApp();
            List<ActiveModel2> tempModel = app.getList(salesNo, DateTime.ParseExact(day,"yyyyMMdd", CultureInfo.CurrentCulture));
            marketSalesActiveActApp actApp = new marketSalesActiveActApp();
            marketSalesOtherApp otherApp = new marketSalesOtherApp();
            foreach (var temp in tempModel)
            {

                // model.Shops = new List<Models.UserShopInfoModel>();
                foreach (var shop in temp.Shops)
                {
                    if (shop.ShopCode.Equals(shopCode))
                    {
                        Models.ActiveModel model = new Models.ActiveModel()
                        {
                            ACT_END_DATE = temp.ACT_END_DATE,
                            ACT_NAME = temp.ACT_NAME,
                            ACT_NO = temp.ACT_NO,
                            ACT_START_DATE = temp.ACT_START_DATE,
                            ACT_TYPE_CODE = temp.ACT_TYPE_CODE,
                            ACT_TYPE_NAME = temp.ACT_TYPE_NAME,
                            CUSTOMER_CODE = temp.CUSTOMER_CODE,
                            CUSTOMER_NAME = temp.CUSTOMER_NAME,
                            T_TYPEID = temp.T_TYPEID,
                            T_TYPENAME = temp.T_TYPENAME,
                            SHOP_CODE = shop.ShopCode,
                            SHOP_NAME = shop.ShopName,

                        };
                        if (actApp.getActCount(salesNo, temp.ACT_NO, shop.ShopCode) > 0)
                        {
                            model.is_New = true;
                        }
                        else
                        {
                            model.is_New = false;
                        }
                        if (otherApp.getActCount(salesNo, temp.ACT_NO, shop.ShopCode) > 0)
                        {
                            model.is_OtherNew = false;
                        }
                        else
                        {
                            model.is_OtherNew = true;
                        }
                        models.Add(model);
                    }
                    // model.Shops.Add(new Models.UserShopInfoModel() { ShopCode = shop.ShopCode, ShopName = shop.ShopName });
                }
                //var actList = actApp.getAct(salesNo, temp.ACT_NO);
                // model.ActiveSub = new List<ActiveActModelSub>();

                //foreach (var act in actList)
                //{
                //    ActiveActModelSub modelsub = new ActiveActModelSub() { BRAND_CODE = act.BRAND_CODE, id = act.id, BRAND_NAME = act.BRAND_NAME, IS_NEW_PRD_FLAG = act.IS_NEW_PRD_FLAG.Value, MACHINE_MODEL_NO = act.MACHINE_MODEL_NO,
                //        SALES_PRICE = act.SALES_PRICE.Value, SHOP_CODE = act.SHOP_CODE, SHOP_NAME = act.SHOP_NAME, TVSIZE = act.TVSIZE.Value, T_TVSIZEID = act.T_TVSIZEID };
                //    modelsub.file_Model_Type001 = new List<imageModel>();
                //    modelsub.file_Model_Type002 = new List<imageModel>();
                //    modelsub.file_Model_Type003 = new List<imageModel>();
                //    modelsub.file_Model_Type004 = new List<imageModel>();
                //    string patch = act.Created_Time.Value.ToString("yyyyMM");
                //    if (act.file_id_Type001 != null && !act.file_id_Type001.Equals(""))
                //    {
                //        string[] files = act.file_id_Type001.Split(",".ToArray());
                //        foreach (var file in files)
                //        {
                //            modelsub.file_Model_Type001.Add(new imageModel() { id = file, url = "https://iretailerapp.flnet.com/Messages/" + patch+"/"+ file+".jpg" });
                //        }
                //    }
                //    if (act.file_id_Type002 != null && !act.file_id_Type002.Equals(""))
                //    {
                //        string[] files = act.file_id_Type002.Split(",".ToArray());
                //        foreach (var file in files)
                //        {
                //            modelsub.file_Model_Type002.Add(new imageModel() { id = file, url = "https://iretailerapp.flnet.com/Messages/" + patch + "/" + file + ".jpg" });
                //        }
                //    }
                //    if (act.file_id_Type003 != null && !act.file_id_Type003.Equals(""))
                //    {
                //        string[] files = act.file_id_Type003.Split(",".ToArray());
                //        foreach (var file in files)
                //        {
                //            modelsub.file_Model_Type003.Add(new imageModel() { id = file, url = "https://iretailerapp.flnet.com/Messages/" + patch + "/" + file + ".jpg" });
                //        }
                //    }
                //    if (act.file_id_Type004 != null && !act.file_id_Type004.Equals(""))
                //    {
                //        string[] files = act.file_id_Type004.Split(",".ToArray());
                //        foreach (var file in files)
                //        {
                //            modelsub.file_Model_Type004.Add(new imageModel() { id = file, url = "https://iretailerapp.flnet.com/Messages/" + patch + "/" + file + ".jpg" });
                //        }
                //    }

                //    model.ActiveSub.Add(modelsub);
                //}

            }
            return models;

        }


        /// <summary>
        /// 抓取产品类型
        /// </summary>
        /// <param name="salesNo"></param>
        /// <returns></returns>
        [HttpGet]
        [AllowAnonymous]
        [Route("GetProdBigTypeList")]
        public List<Models.ProdModel> GetProdBigTypeList()
        {
            List<Models.ProdModel> models = new List<ProdModel>();
            DataSynchronizationLib.DataSetPopTableAdapters.JS5_S12_PRODUCT_TYPE_INFOTableAdapter app = new DataSynchronizationLib.DataSetPopTableAdapters.JS5_S12_PRODUCT_TYPE_INFOTableAdapter();
            var types = app.GetBigTypeData();
            foreach (var type in types)
            {
                models.Add(new ProdModel() { CODE = type.ID, NAME = type.TREE_NODE_NAME });
            }
            return models;
        }
        /// <summary>
        /// 抓取产品类型
        /// </summary>
        /// <param name="BigId">产品大类ID</param>
        /// <returns></returns>
        [HttpGet]
        [AllowAnonymous]
        [Route("GetProdSamllTypeList")]
        public List<Models.ProdModel> GetProdSamllTypeList(string BigId)
        {
            List<Models.ProdModel> models = new List<ProdModel>();
            marketProductTypeApp app = new marketProductTypeApp();
            var types = app.getSmailTypes(BigId);
            foreach (var type in types)
            {
                models.Add(new ProdModel() { CODE = type.ID, NAME = type.TREE_NODE_NAME });
            }
            return models;
        }

        /// <summary>
        /// 抓取产品类型
        /// </summary>
        /// <param name="salesNo"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("GetProdTypeList")]
        public List<Models.ProdModel> GetProdTypeList()
        {
            List<Models.ProdModel> models = new List<ProdModel>();
            marketProductTypeApp app = new marketProductTypeApp();
            var types= app.getTypes();
            foreach (var type in types)
            {
                models.Add(new ProdModel() { CODE = type.CODE, NAME = type.NAME });
            }
            return models;
        }


        /// <summary>
        /// 抓取活动列表
        /// </summary>
        /// <param name="salesNo"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("GetActiveListByShop")]
        public List<Models.ActiveModel> GetActiveListByShop(String salesNo,String shopCode)
        {
            List<Models.ActiveModel> models = new List<Models.ActiveModel>();
            marketSalesActiveListApp app = new marketSalesActiveListApp();
            List<ActiveModel2> tempModel = app.getList(salesNo);
            marketSalesActiveActApp actApp = new marketSalesActiveActApp();
            marketSalesOtherApp otherApp = new marketSalesOtherApp();
            foreach (var temp in tempModel)
            {

                // model.Shops = new List<Models.UserShopInfoModel>();
                foreach (var shop in temp.Shops)
                {
                    if (shop.ShopCode.Equals(shopCode))
                    {
                        Models.ActiveModel model = new Models.ActiveModel()
                        {
                            ACT_END_DATE = temp.ACT_END_DATE,
                            ACT_NAME = temp.ACT_NAME,
                            ACT_NO = temp.ACT_NO,
                            ACT_START_DATE = temp.ACT_START_DATE,
                            ACT_TYPE_CODE = temp.ACT_TYPE_CODE,
                            ACT_TYPE_NAME = temp.ACT_TYPE_NAME,
                            CUSTOMER_CODE = temp.CUSTOMER_CODE,
                            CUSTOMER_NAME = temp.CUSTOMER_NAME,
                            T_TYPEID = temp.T_TYPEID,
                            T_TYPENAME = temp.T_TYPENAME,
                            SHOP_CODE = shop.ShopCode,
                            SHOP_NAME = shop.ShopName,

                        };
                        if (actApp.getActCount(salesNo, temp.ACT_NO, shop.ShopCode) > 0)
                        {
                            model.is_New = false;
                        }
                        else
                        {
                            model.is_New = true;
                        }
                        if (otherApp.getActCount(salesNo, temp.ACT_NO, shop.ShopCode) > 0)
                        {
                            model.is_OtherNew = false;
                        }
                        else
                        {
                            model.is_OtherNew = true;
                        }
                        models.Add(model);
                    }
                    // model.Shops.Add(new Models.UserShopInfoModel() { ShopCode = shop.ShopCode, ShopName = shop.ShopName });
                }
                //var actList = actApp.getAct(salesNo, temp.ACT_NO);
                // model.ActiveSub = new List<ActiveActModelSub>();

                //foreach (var act in actList)
                //{
                //    ActiveActModelSub modelsub = new ActiveActModelSub() { BRAND_CODE = act.BRAND_CODE, id = act.id, BRAND_NAME = act.BRAND_NAME, IS_NEW_PRD_FLAG = act.IS_NEW_PRD_FLAG.Value, MACHINE_MODEL_NO = act.MACHINE_MODEL_NO,
                //        SALES_PRICE = act.SALES_PRICE.Value, SHOP_CODE = act.SHOP_CODE, SHOP_NAME = act.SHOP_NAME, TVSIZE = act.TVSIZE.Value, T_TVSIZEID = act.T_TVSIZEID };
                //    modelsub.file_Model_Type001 = new List<imageModel>();
                //    modelsub.file_Model_Type002 = new List<imageModel>();
                //    modelsub.file_Model_Type003 = new List<imageModel>();
                //    modelsub.file_Model_Type004 = new List<imageModel>();
                //    string patch = act.Created_Time.Value.ToString("yyyyMM");
                //    if (act.file_id_Type001 != null && !act.file_id_Type001.Equals(""))
                //    {
                //        string[] files = act.file_id_Type001.Split(",".ToArray());
                //        foreach (var file in files)
                //        {
                //            modelsub.file_Model_Type001.Add(new imageModel() { id = file, url = "https://iretailerapp.flnet.com/Messages/" + patch+"/"+ file+".jpg" });
                //        }
                //    }
                //    if (act.file_id_Type002 != null && !act.file_id_Type002.Equals(""))
                //    {
                //        string[] files = act.file_id_Type002.Split(",".ToArray());
                //        foreach (var file in files)
                //        {
                //            modelsub.file_Model_Type002.Add(new imageModel() { id = file, url = "https://iretailerapp.flnet.com/Messages/" + patch + "/" + file + ".jpg" });
                //        }
                //    }
                //    if (act.file_id_Type003 != null && !act.file_id_Type003.Equals(""))
                //    {
                //        string[] files = act.file_id_Type003.Split(",".ToArray());
                //        foreach (var file in files)
                //        {
                //            modelsub.file_Model_Type003.Add(new imageModel() { id = file, url = "https://iretailerapp.flnet.com/Messages/" + patch + "/" + file + ".jpg" });
                //        }
                //    }
                //    if (act.file_id_Type004 != null && !act.file_id_Type004.Equals(""))
                //    {
                //        string[] files = act.file_id_Type004.Split(",".ToArray());
                //        foreach (var file in files)
                //        {
                //            modelsub.file_Model_Type004.Add(new imageModel() { id = file, url = "https://iretailerapp.flnet.com/Messages/" + patch + "/" + file + ".jpg" });
                //        }
                //    }

                //    model.ActiveSub.Add(modelsub);
                //}

            }
            return models;

        }

        /// <summary>
        /// 抓取活动列表
        /// </summary>
        /// <param name="salesNo"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("GetActiveList")]
        public List<Models.ActiveModel> GetActiveList(String salesNo)
        {
            List<Models.ActiveModel> models = new List<Models.ActiveModel>();
            marketSalesActiveListApp app = new marketSalesActiveListApp();
            List<ActiveModel2> tempModel = app.getList(salesNo);
            marketSalesActiveActApp actApp = new marketSalesActiveActApp();
            marketSalesOtherApp otherApp = new marketSalesOtherApp();
            foreach (var temp in tempModel)
            {
                
               // model.Shops = new List<Models.UserShopInfoModel>();
                foreach (var shop in temp.Shops)
                {
                    Models.ActiveModel model = new Models.ActiveModel()
                    {
                        ACT_END_DATE = temp.ACT_END_DATE,
                        ACT_NAME = temp.ACT_NAME,
                        ACT_NO = temp.ACT_NO,
                        ACT_START_DATE = temp.ACT_START_DATE,
                        ACT_TYPE_CODE = temp.ACT_TYPE_CODE,
                        ACT_TYPE_NAME = temp.ACT_TYPE_NAME,
                        CUSTOMER_CODE = temp.CUSTOMER_CODE,
                        CUSTOMER_NAME = temp.CUSTOMER_NAME,
                        T_TYPEID = temp.T_TYPEID,
                        T_TYPENAME = temp.T_TYPENAME,
                          SHOP_CODE=shop.ShopCode,
                           SHOP_NAME=shop.ShopName,
                            
                    };
                    if (actApp.getActCount(salesNo, temp.ACT_NO, shop.ShopCode) > 0)
                    {
                        model.is_New = false;
                    }
                    else
                    {
                        model.is_New = true;
                    }
                    if (otherApp.getActCount(salesNo, temp.ACT_NO, shop.ShopCode) > 0)
                    {
                        model.is_OtherNew = false;
                    }
                    else
                    {
                        model.is_OtherNew = true;
                    }
                    models.Add(model);
                    // model.Shops.Add(new Models.UserShopInfoModel() { ShopCode = shop.ShopCode, ShopName = shop.ShopName });
                }
                //var actList = actApp.getAct(salesNo, temp.ACT_NO);
               // model.ActiveSub = new List<ActiveActModelSub>();
                
                //foreach (var act in actList)
                //{
                //    ActiveActModelSub modelsub = new ActiveActModelSub() { BRAND_CODE = act.BRAND_CODE, id = act.id, BRAND_NAME = act.BRAND_NAME, IS_NEW_PRD_FLAG = act.IS_NEW_PRD_FLAG.Value, MACHINE_MODEL_NO = act.MACHINE_MODEL_NO,
                //        SALES_PRICE = act.SALES_PRICE.Value, SHOP_CODE = act.SHOP_CODE, SHOP_NAME = act.SHOP_NAME, TVSIZE = act.TVSIZE.Value, T_TVSIZEID = act.T_TVSIZEID };
                //    modelsub.file_Model_Type001 = new List<imageModel>();
                //    modelsub.file_Model_Type002 = new List<imageModel>();
                //    modelsub.file_Model_Type003 = new List<imageModel>();
                //    modelsub.file_Model_Type004 = new List<imageModel>();
                //    string patch = act.Created_Time.Value.ToString("yyyyMM");
                //    if (act.file_id_Type001 != null && !act.file_id_Type001.Equals(""))
                //    {
                //        string[] files = act.file_id_Type001.Split(",".ToArray());
                //        foreach (var file in files)
                //        {
                //            modelsub.file_Model_Type001.Add(new imageModel() { id = file, url = "https://iretailerapp.flnet.com/Messages/" + patch+"/"+ file+".jpg" });
                //        }
                //    }
                //    if (act.file_id_Type002 != null && !act.file_id_Type002.Equals(""))
                //    {
                //        string[] files = act.file_id_Type002.Split(",".ToArray());
                //        foreach (var file in files)
                //        {
                //            modelsub.file_Model_Type002.Add(new imageModel() { id = file, url = "https://iretailerapp.flnet.com/Messages/" + patch + "/" + file + ".jpg" });
                //        }
                //    }
                //    if (act.file_id_Type003 != null && !act.file_id_Type003.Equals(""))
                //    {
                //        string[] files = act.file_id_Type003.Split(",".ToArray());
                //        foreach (var file in files)
                //        {
                //            modelsub.file_Model_Type003.Add(new imageModel() { id = file, url = "https://iretailerapp.flnet.com/Messages/" + patch + "/" + file + ".jpg" });
                //        }
                //    }
                //    if (act.file_id_Type004 != null && !act.file_id_Type004.Equals(""))
                //    {
                //        string[] files = act.file_id_Type004.Split(",".ToArray());
                //        foreach (var file in files)
                //        {
                //            modelsub.file_Model_Type004.Add(new imageModel() { id = file, url = "https://iretailerapp.flnet.com/Messages/" + patch + "/" + file + ".jpg" });
                //        }
                //    }

                //    model.ActiveSub.Add(modelsub);
                //}
                
            }
            return models;

        }

        [HttpGet]
        [Route("GetSelfBrandInfo")]
        public List<BandModel> GetSelfBrandInfo()
        {
            List<BandModel> models = new List<BandModel>();
            models.Add(new BandModel() { BRAND_CODE = "001", BRAND_NAME = "夏普(SHARP)" });
            models.Add(new BandModel() { BRAND_CODE = "010", BRAND_NAME = "富可视" });
            /*marketMachineChannelApp machineApp = new marketMachineChannelApp();
            marketShopApp shopApp = new marketShopApp();
            List<marketSalesShopEntity> shops = shopApp.getShopByUserId(User.Identity.GetUserId());
            List<machineViewModel> Machines = new List<machineViewModel>();
            foreach (marketSalesShopEntity shop in shops)
            {
                List<marketMachineModelEntity> macs = machineApp.getMachineModelByShop(shop.SHOP_CODE);
                foreach (marketMachineModelEntity mac in macs)
                {
                    if (!Machines.Exists(p => p.MACHINE_MODEL_NO.Equals(mac.MACHINE_MODEL_NO)))
                    {
                        if (!models.Exists(p => p.BRAND_CODE.Equals(mac.BANDCODE)))
                        {
                            models.Add(new BandModel() {  BRAND_CODE=mac.BANDCODE,BRAND_NAME=mac.BANDNAME});
                        }
                        //Machines.Add(new machineViewModel()
                        //{
                        //    ShopCode = shop.SHOP_CODE,
                        //    MACHINE_MODEL_NO = mac.MACHINE_MODEL_NO,
                        //    T_TVSIZEID = mac.T_TVSIZEID
                        //    ,
                        //    DESCP = mac.DESCP,
                        //    BANDCODE = mac.BANDCODE,
                        //    BANDNAME = mac.BANDNAME
                        //});
                    }
                }

            }
            **/

            return models;

        }
        /// <summary>
        /// 型号列表
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Route("GetMachineInfoV2")]
        public List<machineViewModel> GetMachineInfoV2(string bandCode)
        {
            try
            { 
            marketMachineChannelApp machineApp = new marketMachineChannelApp();
            marketShopApp shopApp = new marketShopApp();
            List<marketSalesShopEntity> shops = shopApp.getShopByUserId(User.Identity.GetUserId());
            List<machineViewModel> Machines = new List<machineViewModel>();
            foreach (marketSalesShopEntity shop in shops)
            {
                List<marketMachineModelEntity> macs = machineApp.getMachineModelByShop(shop.SHOP_CODE);
                foreach (marketMachineModelEntity mac in macs)
                {
                    if (!Machines.Exists(p => p.MACHINE_MODEL_NO.Equals(mac.MACHINE_MODEL_NO)) && mac.BANDCODE !=null && mac.BANDCODE.Equals(bandCode) )
                    {
                        Machines.Add(new machineViewModel()
                        {
                            ShopCode = shop.SHOP_CODE,
                            MACHINE_MODEL_NO = mac.MACHINE_MODEL_NO,
                            T_TVSIZEID = mac.T_TVSIZEID
                            ,
                            DESCP = mac.DESCP,
                            BANDCODE = mac.BANDCODE,
                            BANDNAME = mac.BANDNAME
                        });
                    }
                }

            }
            return Machines;
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
        /// 型号列表
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Route("GetMachineInfoV4")]
        public List<machineViewModel> GetMachineInfoV4(string bandCode,string shopCode)
        {
            try
            {
                marketMachineChannelApp machineApp = new marketMachineChannelApp();
                marketShopApp shopApp = new marketShopApp();
                //List<marketSalesShopEntity> shops = shopApp.getShopByUserId(User.Identity.GetUserId());
                List<machineViewModel> Machines = new List<machineViewModel>();
                //foreach (marketSalesShopEntity shop in shops)
                //{
                    List<marketMachineModelEntity> macs = machineApp.getMachineModelByShop(shopCode);
                    foreach (marketMachineModelEntity mac in macs)
                    {
                        if ( !Machines.Exists(p => p.MACHINE_MODEL_NO.Equals(mac.MACHINE_MODEL_NO)) && mac.BANDCODE != null && mac.BANDCODE.Equals(bandCode))
                        {
                            Machines.Add(new machineViewModel()
                            {
                                ShopCode = shopCode,
                                MACHINE_MODEL_NO = mac.MACHINE_MODEL_NO,
                                T_TVSIZEID = mac.T_TVSIZEID
                                ,
                                DESCP = mac.DESCP,
                                BANDCODE = mac.BANDCODE,
                                BANDNAME = mac.BANDNAME
                            });
                        }
                    }

                //}
                return Machines;
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
        /// 型号列表
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Route("GetMachineInfoV5")]
        public List<machineViewModel> GetMachineInfoV5(string shopCode)
        {
            try
            {
                marketMachineChannelApp machineApp = new marketMachineChannelApp();
                marketShopApp shopApp = new marketShopApp();
                //List<marketSalesShopEntity> shops = shopApp.getShopByUserId(User.Identity.GetUserId());
                List<machineViewModel> Machines = new List<machineViewModel>();
                //foreach (marketSalesShopEntity shop in shops)
                //{
                List<marketMachineModelEntity> macs = machineApp.getMachineModelByShop(shopCode);
                foreach (marketMachineModelEntity mac in macs)
                {
                    if (!Machines.Exists(p => p.MACHINE_MODEL_NO.Equals(mac.MACHINE_MODEL_NO)) && mac.BANDCODE!=null)
                    {
                        Machines.Add(new machineViewModel()
                        {
                            ShopCode = shopCode,
                            MACHINE_MODEL_NO = mac.MACHINE_MODEL_NO,
                            T_TVSIZEID = mac.T_TVSIZEID
                            ,
                            DESCP = mac.DESCP,
                            BANDCODE = mac.BANDCODE,
                            BANDNAME = mac.BANDNAME
                        });
                    }
                }

                //}
                return Machines;
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
        /// 型号列表
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Route("GetMachineInfoV3")]
        public List<machineViewModel> GetMachineInfoV3(string shopCode)
        {
            try
            {
                marketMachineChannelApp machineApp = new marketMachineChannelApp();
                marketShopApp shopApp = new marketShopApp();
               // List<marketSalesShopEntity> shops = shopApp.getShopByUserId(User.Identity.GetUserId());
                List<machineViewModel> Machines = new List<machineViewModel>();
               // foreach (marketSalesShopEntity shop in shops)
                //{
                    List<marketMachineModelEntity> macs = machineApp.getMachineModelByShop(shopCode);
                    foreach (marketMachineModelEntity mac in macs)
                    {
                        if (!Machines.Exists(p => p.MACHINE_MODEL_NO.Equals(mac.MACHINE_MODEL_NO)) && mac.BANDCODE != null)
                        {
                            Machines.Add(new machineViewModel()
                            {
                                ShopCode = shopCode,
                                MACHINE_MODEL_NO = mac.MACHINE_MODEL_NO,
                                T_TVSIZEID = mac.T_TVSIZEID
                                ,
                                DESCP = mac.DESCP,
                                BANDCODE = mac.BANDCODE,
                                BANDNAME = mac.BANDNAME
                            });
                        }
                    }

                //}
                return Machines;
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
        /// 型号列表
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Route("GetMachineInfo")]
        public List<machineViewModel> GetMachineInfo()
        {
            try { 
            marketMachineChannelApp machineApp = new marketMachineChannelApp();
            marketShopApp shopApp = new marketShopApp();
            List<marketSalesShopEntity> shops = shopApp.getShopByUserId(User.Identity.GetUserId());
            List<machineViewModel> Machines = new List<machineViewModel>();
            foreach (marketSalesShopEntity shop in shops)
            {
                List<marketMachineModelEntity> macs= machineApp.getMachineModelByShop(shop.SHOP_CODE);
                foreach (marketMachineModelEntity mac in macs)
                {
                    if (!Machines.Exists(p => p.MACHINE_MODEL_NO.Equals(mac.MACHINE_MODEL_NO)) && mac.BANDCODE != null)
                    {
                        Machines.Add(new machineViewModel() { ShopCode = shop.SHOP_CODE, MACHINE_MODEL_NO = mac.MACHINE_MODEL_NO, T_TVSIZEID = mac.T_TVSIZEID
                            , DESCP = mac.DESCP, BANDCODE=mac.BANDCODE, BANDNAME=mac.BANDNAME });
                    }
                }
                    
            }
            return Machines;
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
        /// 型号列表
        /// </summary>
        /// <param name="typeId">小类ID</param>
        /// <param name="shopCode">门店编号</param>
        /// <param name="bandCode">品牌</param>
        /// <returns></returns>
        [HttpGet]
        [Route("GetMachineInfoByTypeId")]
        public List<machineViewModel> GetMachineInfoByTypeId(String typeId,string shopCode, string bandCode)
        {
            try
            {
                marketMachineChannelApp machineApp = new marketMachineChannelApp();
                marketShopApp shopApp = new marketShopApp();
                //List<marketSalesShopEntity> shops = shopApp.getShopByUserId(User.Identity.GetUserId());
                List<machineViewModel> Machines = new List<machineViewModel>();
                //foreach (marketSalesShopEntity shop in shops)
                //{
                JS5_S12_MACHINE_MODELTableAdapter ad = new JS5_S12_MACHINE_MODELTableAdapter();
                var typeMacs = ad.GetDataByTypeCode(typeId);
                List<marketMachineModelEntity> macs = machineApp.getMachineModelByShop(shopCode);
                foreach (marketMachineModelEntity mac in macs)
                {
                    if (!Machines.Exists(p => p.MACHINE_MODEL_NO.Equals(mac.MACHINE_MODEL_NO)) && mac.BANDCODE!=null && mac.BANDCODE.Equals(bandCode)  &&    typeMacs.Where(p => p.MACHINE_MODEL_NO.Equals(mac.MACHINE_MODEL_NO)).Count() > 0)
                    {
                        Machines.Add(new machineViewModel()
                        {
                            ShopCode = shopCode,
                            MACHINE_MODEL_NO = mac.MACHINE_MODEL_NO,
                            T_TVSIZEID = mac.T_TVSIZEID
                            ,
                            DESCP = mac.DESCP,
                            BANDCODE = mac.BANDCODE,
                            BANDNAME = mac.BANDNAME
                        });
                    }
                }

                //}
                return Machines;
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
        /// 获取尺寸信息
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Route("GetTvSizeInfo")]
        public List<tvSizeViewModel> GetTvSizeInfo()
        {
            marketTvSizeApp app = new marketTvSizeApp();
            List<marketTvsizeinfoEntity> ents = app.getTvSize();
            List<tvSizeViewModel> model = new List<tvSizeViewModel>();
            foreach (marketTvsizeinfoEntity ent in ents)
            {
                model.Add(new tvSizeViewModel() { T_TVSIZEID = ent.T_TVSIZEID, T_TVSIZENAME = ent.T_TVSIZENAME });
            }
            return model;
        }

        /// <summary>
        /// 获取已上报数据
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Route("GetSalesActual")]
        public List<salesActualModel> GetSalesActual()
        {
            marketSalesActualApp app = new marketSalesActualApp();
            List<salesActualAllModel> list = app.GetSalesActualByUserId(User.Identity.GetUserId());
            List<salesActualModel> models = new List<salesActualModel>();
            foreach (salesActualAllModel ent in list)
            {
                models.Add(new salesActualModel() { Actual_Day=ent.SALES_DATE, Actual_Price=ent.SALES_PRICE, Actual_Qty=ent.SALES_QTY, Actual_Type=ent.A_TYPE, MACHINE_MODEL_NO=ent.MACHINE_MODEL_NO,
                  SHOP_CODE=ent.CH_SHOP_CODE, SHOP_NAME= ent.SHOP_NAME});
            }
            return models;
        }
        /// <summary>  
/// 得到本周第一天(以星期天为第一天)  
/// </summary>  
/// <param name="datetime"></param>  
/// <returns></returns>  
public DateTime GetWeekFirstDaySun(DateTime datetime)  
{  
    //星期天为第一天  
    int weeknow = Convert.ToInt32(datetime.DayOfWeek);  
    int daydiff = (-1) * weeknow;  
  
    //本周第一天  
    string FirstDay = datetime.AddDays(daydiff).ToString("yyyy-MM-dd");  
    return Convert.ToDateTime(FirstDay);  
}
        /// <summary>
        /// 得到本周最后一天(以星期六为最后一天)
        /// </summary>
        /// <param name="datetime"></param>
        /// <returns></returns>
        public DateTime GetWeekLastDaySat(DateTime datetime)
        {
            //星期六为最后一天
            int weeknow = Convert.ToInt32(datetime.DayOfWeek);
            int daydiff = (7 - weeknow) - 1;

            //本周最后一天
            string LastDay = datetime.AddDays(daydiff).ToString("yyyy-MM-dd");
            return Convert.ToDateTime(LastDay);
        }




        [HttpGet]
        [AllowAnonymous]
        [Route("GetHomeData")]
        public homeModel GetHomeData(string salesNo)
        {
            try
            {
                DateTime now = DateTime.Now;
                homeModel model = new homeModel();
                //当日
                marketSalesActualApp app = new marketSalesActualApp();
                DateTime startTime = DateTime.Parse(now.ToString("yyyy-MM-dd ") + "00:00:00");
                DateTime EndTime = DateTime.Parse(now.ToString("yyyy-MM-dd ") + "23:59:59");
                model.DayQty = app.GetQty(startTime, EndTime, salesNo);
                model.DayTotalQty= app.GetQty(startTime, EndTime);
                model.DayTotalAmount = app.GetAmount(startTime, EndTime);
                //当月
                 startTime = DateTime.Parse(now.ToString("yyyy-MM-") + "01 00:00:00");
                 EndTime = DateTime.Parse(now.AddMonths(1).ToString("yyyy-MM-") + "01 23:59:59").AddDays(-1);
                model.MonthQty = app.GetQty(startTime, EndTime, salesNo);
                //上月
                startTime = DateTime.Parse(now.AddMonths(-1).ToString("yyyy-MM-") + "01 00:00:00");
                EndTime = DateTime.Parse(now.ToString("yyyy-MM-") + "01 23:59:59").AddDays(-1);
                model.LastMonthQty= app.GetQty(startTime, EndTime, salesNo);
                //上周
                 startTime = DateTime.Parse(GetWeekFirstDaySun(now.AddDays(-7)).ToString("yyyy-MM-dd ") + "00:00:00");
                 EndTime = DateTime.Parse(GetWeekLastDaySat(now.AddDays(-7)).ToString("yyyy-MM-dd ") + "23:59:59");
                model.LastWeekQty = app.GetQty(startTime, EndTime, salesNo);
                //昨日
                startTime = DateTime.Parse(GetWeekFirstDaySun(now.AddDays(-1)).ToString("yyyy-MM-dd ") + "00:00:00");
                EndTime = DateTime.Parse(GetWeekLastDaySat(now.AddDays(-1)).ToString("yyyy-MM-dd ") + "23:59:59");
                model.LastDayLtCount= app.GetCT(startTime, EndTime, salesNo);
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
        }
        [HttpGet]
        [AllowAnonymous]
        [Route("GetSalesActualV3Image")]
        public List<imageModel> GetSalesActualV3Image(string id)
        {
            List<imageModel> models = new List<imageModel>();
            marketSalesActualApp app = new marketSalesActualApp();
            var list= app.getImage(id);
            foreach (DataSetStanby.TF_FILEINFORow row in list)
            {
                var model = new imageModel() { id = row.T_FILENAME.Substring(0, row.T_FILENAME.Length-4) };
                model.url =  "https://iretailerapp.flnet.com/Messages/" + row.PHYSICAL_PATH.Replace("/data/upload/", "") + "/" + row.T_FILENAME;
                models.Add(model);
            }
            return models;

        }
        public string getTypeCode(string TypeCode)
        {
            if (TypeCode.Equals("S01"))
            {
                return "NormalGoods";
            }else if (TypeCode.Equals("S02"))
            {
                return "GroupGoods";
            }
            else if (TypeCode.Equals("S04"))
            {
                return "PrototypeGoods";
            }
            return TypeCode;
        }
        /// <summary>
        /// 获取销售上报数据
        /// </summary>
        /// <param name="salesNo">SalesNo</param>
        /// <param name="queryType">查询类型,1:指定日期,2：当周,3:上周,4:当月,5：上月</param>
        /// <param name="queryDay">查询日期</param>
        /// <param name="goodsType">销售类型:S01:正常销售,S02:团购,S04:样机销售</param>
        /// <param name="queryType2">查询类型2,1：个人销售，2：门店汇总</param>
        /// <returns></returns>
        [HttpGet]
        [AllowAnonymous]
        [Route("GetSalesActualV3")]
        public salesActualModelV2 GetSalesActualV3(string salesNo, string queryType, DateTime queryDay, string goodsType,string queryType2)
        {
            try
            {
                marketSalesActualApp app = new marketSalesActualApp();
                salesActualModelV2 model = new salesActualModelV2();
                marketSalesApp salerApp = new marketSalesApp();
                marketProductTypeApp productType = new marketProductTypeApp();
                JS5_AREA_CODETableAdapter areaAd = new JS5_AREA_CODETableAdapter();
                JS5_S12_MACHINE_MODELTableAdapter modelAd = new JS5_S12_MACHINE_MODELTableAdapter();
                if (queryType.Equals("1"))
                {
                    model.dateString = queryDay.ToString("yyyy/MM/dd");
                    model.Detail = new List<salesActualModelV2Detail>();
                    DateTime startTime = DateTime.Parse(queryDay.ToString("yyyy-MM-dd ") + "00:00:00");
                    DateTime EndTime = DateTime.Parse(queryDay.ToString("yyyy-MM-dd ") + "23:59:59");
                    var res = app.GetSalesActualList(startTime, EndTime, goodsType, salesNo, "2");
                    
                    foreach (DataSetStanby.V_SALES_TRANS_APP_QUERYRow row in res)
                    {
                        string CONSUMER_ProvinceID = null;
                        if (!row.IsAREA_IDNull())
                        {
                            var areaList= areaAd.GetDataById(row.AREA_ID);
                            if (areaList.Count > 0)
                            {
                                CONSUMER_ProvinceID = areaList[0].PARENT_ID;
                            }
                        }
                        string sex = "0";
                        if (!row.IsSEXNull())
                        {
                            sex = row.SEX.Equals("M") ? "0" : "1";
                        }
                        string SmallTypeId = null;
                        string BigTypeId = null; 
                        var SmallTypes = modelAd.GetDataByModelNo(row.MACHINE_MODEL_NO);
                        if (SmallTypes.Count > 0)
                        {
                            SmallTypeId = SmallTypes[0].PRODUCT_TYPE_INFO_ID;
                           var bigType= productType.getBigType(SmallTypeId, "BigType");
                            if (bigType != null)
                            {
                                BigTypeId = bigType.ID;
                            }
                        }
                        
                          
                        model.Detail.Add(new salesActualModelV2Detail()
                        {
                            Actual_Day = row.SALES_DATE.ToString("yyyy/MM/dd"),
                            Actual_Price = (double)row.SALES_PRICE,
                            Actual_Qty = (int)row.SALES_QTY,
                            Actual_Type = row.TRANS_TYPE_NAME,
                            GOODS_TYPE_CODE = getTypeCode(row.GOODS_TYPE_CODE),
                            CONSUMER_NAME = row.CUSTOMER_NAME,
                            GOODS_TYPE_NAME = row.GOODS_TYPE_NAME,
                            sales_No = row.EMPLOYEE_ID,

                            SHOP_CODE = row.SHOP_CODE,
                            SHOP_NAME = row.SHOP_NAME,
                            MACHINE_MODEL_NO = row.MACHINE_MODEL_NO
                          ,
                            INVOICE_FLAG = row.INVOICE_FLAG,
                            id = row.ID,
                            TRANS_NO = row.TRANS_NO.ToString(),
                            BAY_NAME = row.CONSUMER_NAME,
                            BAY_PHONE_NO = row.CONSUMER_PHONE_NO
                            ,
                            salesName = row.CH_REPORT_NAME,
                            INVOICE_STATUS_NAME = row.INVOICE_STATUS_CODE,
                            UNQUALIFIED_REASON_NAME = row.UNQUALIFIED_REASON_CODE, BOARD_FLAG = (int)row.BOARD_FLAG
                              , BRAND_CODE = row.BRAND_CODE, TVSIZE = (int)row.TVSIZE
                              , T_TVSIZEID = row.T_TVSIZEID
                              , ACT_NO = row.ACT_NO, ACT_NAME=row.ACT_NAME
                              , CONSUMER_ADD=row.ADDR, CONSUMER_AGE=row.AGE_PERIOD_ID, CONSUMER_XINGBIE= sex
                              , CONSUMER_ARERID=row.AREA_ID, CONSUMER_ProvinceID= CONSUMER_ProvinceID, BigTypeId=BigTypeId, SmallTypeId=SmallTypeId
                              , SAMPLE_SN_NO=row.SN_NO
                        });

                    }

                }
                else if (queryType.Equals("2"))
                {
                    queryDay = DateTime.Now;
                    GregorianCalendar gc = new GregorianCalendar();
                    int weekOfYear = gc.GetWeekOfYear(queryDay, CalendarWeekRule.FirstDay, DayOfWeek.Monday);

                    model.dateString = queryDay.ToString("yyyy") + "W" + weekOfYear.ToString() + "(" + GetWeekFirstDaySun(queryDay).ToString("MMdd") + "-" + GetWeekLastDaySat(queryDay).ToString("MMdd") + ")";
                    model.Detail = new List<salesActualModelV2Detail>();
                    DateTime startTime = DateTime.Parse(GetWeekFirstDaySun(queryDay).ToString("yyyy-MM-dd ") + "00:00:00");
                    DateTime EndTime = DateTime.Parse(GetWeekLastDaySat(queryDay).ToString("yyyy-MM-dd ") + "23:59:59");
                    var res = app.GetSalesActualList(startTime, EndTime, goodsType, salesNo, queryType2);
                    foreach (DataSetStanby.V_SALES_TRANS_APP_QUERYRow row in res)
                    {
                        string CONSUMER_ProvinceID = null;
                        if (!row.IsAREA_IDNull())
                        {
                            var areaList = areaAd.GetDataById(row.AREA_ID);
                            if (areaList.Count > 0)
                            {
                                CONSUMER_ProvinceID = areaList[0].PARENT_ID;
                            }
                        }
                        string sex = "0";
                        if (!row.IsSEXNull())
                        {
                            sex = row.SEX.Equals("M") ? "0" : "1";
                        }
                        string SmallTypeId = null;
                        string BigTypeId = null;
                        var SmallTypes = modelAd.GetDataByModelNo(row.MACHINE_MODEL_NO);
                        if (SmallTypes.Count > 0)
                        {
                            SmallTypeId = SmallTypes[0].PRODUCT_TYPE_INFO_ID;
                            var bigType = productType.getBigType(SmallTypeId, "BigType");
                            if (bigType != null)
                            {
                                BigTypeId = bigType.ID;
                            }
                        }
                        model.Detail.Add(new salesActualModelV2Detail()
                        {
                            Actual_Day = row.SALES_DATE.ToString("yyyy/MM/dd"),
                            Actual_Price = (double)row.SALES_PRICE,
                            Actual_Qty = (int)row.SALES_QTY,
                            Actual_Type = row.TRANS_TYPE_NAME,
                            GOODS_TYPE_CODE = getTypeCode(row.GOODS_TYPE_CODE),
                            CONSUMER_NAME = row.CUSTOMER_NAME,
                            GOODS_TYPE_NAME = row.GOODS_TYPE_NAME,
                            sales_No = row.EMPLOYEE_ID,
                            SHOP_CODE = row.SHOP_CODE,
                            SHOP_NAME = row.SHOP_NAME,
                            MACHINE_MODEL_NO = row.MACHINE_MODEL_NO
                              ,
                            INVOICE_FLAG = row.INVOICE_FLAG,
                            id = row.ID,
                            TRANS_NO = row.TRANS_NO.ToString()
                            ,
                            BAY_NAME = row.CONSUMER_NAME,
                            BAY_PHONE_NO = row.CONSUMER_PHONE_NO
                            , salesName= row.CH_REPORT_NAME,
                            INVOICE_STATUS_NAME = row.INVOICE_STATUS_CODE,
                            UNQUALIFIED_REASON_NAME = row.UNQUALIFIED_REASON_CODE,
                            BOARD_FLAG = (int)row.BOARD_FLAG
                            ,
                            BRAND_CODE = row.BRAND_CODE,
                            TVSIZE = (int)row.TVSIZE
                             ,
                            T_TVSIZEID = row.T_TVSIZEID
                             ,
                            ACT_NO = row.ACT_NO,
                            ACT_NAME = row.ACT_NAME
                             ,
                            CONSUMER_ADD = row.ADDR,
                            CONSUMER_AGE = row.AGE_PERIOD_ID,
                            CONSUMER_XINGBIE = sex
                              ,
                            CONSUMER_ARERID = row.AREA_ID,
                            CONSUMER_ProvinceID = CONSUMER_ProvinceID,
                            BigTypeId = BigTypeId,
                            SmallTypeId = SmallTypeId,
                            SAMPLE_SN_NO = row.SN_NO
                        });



                    }
                    model.Mast = (from a in model.Detail
                                  group a by new { a.MACHINE_MODEL_NO,a.sales_No,a.salesName } into g
                                  //orderby new ComparerItem() { OrderIndex = b.Key., Id = b.Key.Id } descending
                                  select new salesActualModelV2Mast
                                  {
                                      salesName = g.Key.salesName,
                                      MACHINE_MODEL_NO = g.Key.MACHINE_MODEL_NO,
                                      Actual_Amount = g.Sum(t => t.Actual_Price * t.Actual_Qty),
                                      Actual_Qty = g.Sum(t => t.Actual_Qty)


                                  }).ToList();
                    var shops= (from a in model.Detail
                                group a by new { a.SHOP_CODE,a.SHOP_NAME } into g
                                //orderby new ComparerItem() { OrderIndex = b.Key., Id = b.Key.Id } descending
                                select new 
                                {
                                    SHOP_CODE = g.Key.SHOP_CODE,
                                    SHOP_NAME = g.Key.SHOP_NAME
                                }).ToList();
                    model.ShopMast = new List<salesActualModelV2Shop>();
                    foreach (var shop in shops)
                    {
                        salesActualModelV2Shop shopmast = new salesActualModelV2Shop() {  shopCode=shop.SHOP_CODE, shopName=shop.SHOP_NAME, Mast= new List<salesActualModelV2Mast>()};
                        shopmast.Mast= (from a in model.Detail.Where(p=>p.SHOP_CODE.Equals(shop.SHOP_CODE))
                                        group a by new { a.sales_No,a.MACHINE_MODEL_NO,a.salesName } into g
                                        //orderby new ComparerItem() { OrderIndex = b.Key., Id = b.Key.Id } descending
                                        select new salesActualModelV2Mast
                                        {
                                             salesName= g.Key.salesName,
                                            MACHINE_MODEL_NO = g.Key.MACHINE_MODEL_NO,
                                            Actual_Amount = g.Sum(t => t.Actual_Price * t.Actual_Qty),
                                            Actual_Qty = g.Sum(t => t.Actual_Qty)


                                        }).ToList();
                        model.ShopMast.Add(shopmast);


                    }

                }
                else if (queryType.Equals("3"))
                {
                    queryDay = DateTime.Now.AddDays(-7);
                    GregorianCalendar gc = new GregorianCalendar();
                    int weekOfYear = gc.GetWeekOfYear(queryDay, CalendarWeekRule.FirstDay, DayOfWeek.Monday);

                    model.dateString = queryDay.ToString("yyyy") + "W" + weekOfYear.ToString() + "(" + GetWeekFirstDaySun(queryDay).ToString("MMdd") + "-" + GetWeekLastDaySat(queryDay).ToString("MMdd") + ")";
                    model.Detail = new List<salesActualModelV2Detail>();
                    DateTime startTime = DateTime.Parse(GetWeekFirstDaySun(queryDay).ToString("yyyy-MM-dd ") + "00:00:00");
                    DateTime EndTime = DateTime.Parse(GetWeekLastDaySat(queryDay).ToString("yyyy-MM-dd ") + "23:59:59");
                    var res = app.GetSalesActualList(startTime, EndTime, goodsType, salesNo, queryType2);
                    foreach (DataSetStanby.V_SALES_TRANS_APP_QUERYRow row in res)
                    {
                        string CONSUMER_ProvinceID = null;
                        if (!row.IsAREA_IDNull())
                        {
                            var areaList = areaAd.GetDataById(row.AREA_ID);
                            if (areaList.Count > 0)
                            {
                                CONSUMER_ProvinceID = areaList[0].PARENT_ID;
                            }
                        }
                        string sex = "0";
                        if (!row.IsSEXNull())
                        {
                            sex = row.SEX.Equals("M") ? "0" : "1";
                        }
                        string SmallTypeId = null;
                        string BigTypeId = null;
                        var SmallTypes = modelAd.GetDataByModelNo(row.MACHINE_MODEL_NO);
                        if (SmallTypes.Count > 0)
                        {
                            SmallTypeId = SmallTypes[0].PRODUCT_TYPE_INFO_ID;
                            var bigType = productType.getBigType(SmallTypeId, "BigType");
                            if (bigType != null)
                            {
                                BigTypeId = bigType.ID;
                            }
                        }
                        model.Detail.Add(new salesActualModelV2Detail()
                        {
                            Actual_Day = row.SALES_DATE.ToString("yyyy/MM/dd"),
                            Actual_Price = (double)row.SALES_PRICE,
                            Actual_Qty = (int)row.SALES_QTY,
                            Actual_Type = row.TRANS_TYPE_NAME,
                            GOODS_TYPE_CODE = getTypeCode(row.GOODS_TYPE_CODE),
                            CONSUMER_NAME = row.CUSTOMER_NAME,
                            GOODS_TYPE_NAME = row.GOODS_TYPE_NAME,
                            sales_No = row.EMPLOYEE_ID,
                            SHOP_CODE = row.SHOP_CODE,
                            SHOP_NAME = row.SHOP_NAME,
                            MACHINE_MODEL_NO = row.MACHINE_MODEL_NO
                             ,
                            INVOICE_FLAG = row.INVOICE_FLAG,
                            id = row.ID,
                            TRANS_NO = row.TRANS_NO.ToString()
                            ,
                            BAY_NAME = row.CONSUMER_NAME,
                            BAY_PHONE_NO = row.CONSUMER_PHONE_NO
                            ,
                            salesName = row.CH_REPORT_NAME,
                            INVOICE_STATUS_NAME = row.INVOICE_STATUS_CODE,
                            UNQUALIFIED_REASON_NAME = row.UNQUALIFIED_REASON_CODE,
                            BOARD_FLAG = (int)row.BOARD_FLAG
                            ,
                            BRAND_CODE = row.BRAND_CODE,
                            TVSIZE = (int)row.TVSIZE
                             ,
                            T_TVSIZEID = row.T_TVSIZEID
                             ,
                            ACT_NO = row.ACT_NO,
                            ACT_NAME = row.ACT_NAME
                             ,
                            CONSUMER_ADD = row.ADDR,
                            CONSUMER_AGE = row.AGE_PERIOD_ID,
                            CONSUMER_XINGBIE = sex
                              ,
                            CONSUMER_ARERID = row.AREA_ID,
                            CONSUMER_ProvinceID = CONSUMER_ProvinceID,
                            BigTypeId = BigTypeId,
                            SmallTypeId = SmallTypeId,
                            SAMPLE_SN_NO = row.SN_NO
                        });



                    }
                    model.Mast = (from a in model.Detail
                                  group a by new { a.MACHINE_MODEL_NO, a.sales_No,a.salesName } into g
                                  //orderby new ComparerItem() { OrderIndex = b.Key., Id = b.Key.Id } descending
                                  select new salesActualModelV2Mast
                                  {
                                      salesName = g.Key.salesName,
                                      MACHINE_MODEL_NO = g.Key.MACHINE_MODEL_NO,
                                      Actual_Amount = g.Sum(t => t.Actual_Price * t.Actual_Qty),
                                      Actual_Qty = g.Sum(t => t.Actual_Qty)


                                  }).ToList();
                    var shops = (from a in model.Detail
                                 group a by new { a.SHOP_CODE, a.SHOP_NAME } into g
                                 //orderby new ComparerItem() { OrderIndex = b.Key., Id = b.Key.Id } descending
                                 select new
                                 {
                                     SHOP_CODE = g.Key.SHOP_CODE,
                                     SHOP_NAME = g.Key.SHOP_NAME
                                 }).ToList();
                    model.ShopMast = new List<salesActualModelV2Shop>();
                    foreach (var shop in shops)
                    {
                        salesActualModelV2Shop shopmast = new salesActualModelV2Shop() { shopCode = shop.SHOP_CODE, shopName = shop.SHOP_NAME, Mast = new List<salesActualModelV2Mast>() };
                        shopmast.Mast = (from a in model.Detail.Where(p => p.SHOP_CODE.Equals(shop.SHOP_CODE))
                                         group a by new { a.sales_No, a.MACHINE_MODEL_NO,a.salesName } into g
                                         //orderby new ComparerItem() { OrderIndex = b.Key., Id = b.Key.Id } descending
                                         select new salesActualModelV2Mast
                                         {
                                             salesName = g.Key.salesName,
                                             MACHINE_MODEL_NO = g.Key.MACHINE_MODEL_NO,
                                             Actual_Amount = g.Sum(t => t.Actual_Price * t.Actual_Qty),
                                             Actual_Qty = g.Sum(t => t.Actual_Qty)


                                         }).ToList();
                        model.ShopMast.Add(shopmast);

                    }


                }
                else if (queryType.Equals("4"))
                {
                    queryDay = DateTime.Now;
                    GregorianCalendar gc = new GregorianCalendar();
                    int weekOfYear = gc.GetWeekOfYear(queryDay, CalendarWeekRule.FirstDay, DayOfWeek.Monday);

                    model.dateString = queryDay.ToString("yyyyMM");
                    model.Detail = new List<salesActualModelV2Detail>();
                    DateTime startTime = DateTime.Parse(queryDay.ToString("yyyy-MM-") + "01 00:00:00");
                    DateTime EndTime = DateTime.Parse(queryDay.AddMonths(1).ToString("yyyy-MM-") + "01 23:59:59").AddDays(-1);
                    var res = app.GetSalesActualList(startTime, EndTime, goodsType, salesNo, queryType2);
                    foreach (DataSetStanby.V_SALES_TRANS_APP_QUERYRow row in res)
                    {
                        string CONSUMER_ProvinceID = null;
                        if (!row.IsAREA_IDNull())
                        {
                            var areaList = areaAd.GetDataById(row.AREA_ID);
                            if (areaList.Count > 0)
                            {
                                CONSUMER_ProvinceID = areaList[0].PARENT_ID;
                            }
                        }
                        string sex = "0";
                        if (!row.IsSEXNull())
                        {
                            sex = row.SEX.Equals("M") ? "0" : "1";
                        }
                        string SmallTypeId = null;
                        string BigTypeId = null;
                        var SmallTypes = modelAd.GetDataByModelNo(row.MACHINE_MODEL_NO);
                        if (SmallTypes.Count > 0)
                        {
                            SmallTypeId = SmallTypes[0].PRODUCT_TYPE_INFO_ID;
                            var bigType = productType.getBigType(SmallTypeId, "BigType");
                            if (bigType != null)
                            {
                                BigTypeId = bigType.ID;
                            }
                        }
                        model.Detail.Add(new salesActualModelV2Detail()
                        {
                            Actual_Day = row.SALES_DATE.ToString("yyyy/MM/dd"),
                            Actual_Price = (double)row.SALES_PRICE,
                            Actual_Qty = (int)row.SALES_QTY,
                            Actual_Type = row.TRANS_TYPE_NAME,
                            GOODS_TYPE_CODE = getTypeCode(row.GOODS_TYPE_CODE),
                            CONSUMER_NAME = row.CUSTOMER_NAME,
                            GOODS_TYPE_NAME = row.GOODS_TYPE_NAME,
                            sales_No = row.EMPLOYEE_ID,
                            SHOP_CODE = row.SHOP_CODE,
                            SHOP_NAME = row.SHOP_NAME,
                            MACHINE_MODEL_NO = row.MACHINE_MODEL_NO
                             ,
                            INVOICE_FLAG = row.INVOICE_FLAG,
                            id = row.ID,
                            TRANS_NO = row.TRANS_NO.ToString()
                            ,
                            BAY_NAME = row.CONSUMER_NAME,
                            BAY_PHONE_NO = row.CONSUMER_PHONE_NO
                            ,
                            salesName = row.CH_REPORT_NAME,
                            INVOICE_STATUS_NAME = row.INVOICE_STATUS_CODE,
                            UNQUALIFIED_REASON_NAME = row.UNQUALIFIED_REASON_CODE,
                            BOARD_FLAG = (int)row.BOARD_FLAG
                            ,
                            BRAND_CODE = row.BRAND_CODE,
                            TVSIZE = (int)row.TVSIZE
                             ,
                            T_TVSIZEID = row.T_TVSIZEID
                             ,
                            ACT_NO = row.ACT_NO,
                            ACT_NAME = row.ACT_NAME
                             ,
                            CONSUMER_ADD = row.ADDR,
                            CONSUMER_AGE = row.AGE_PERIOD_ID,
                            CONSUMER_XINGBIE = sex
                              ,
                            CONSUMER_ARERID = row.AREA_ID,
                            CONSUMER_ProvinceID = CONSUMER_ProvinceID,
                            BigTypeId = BigTypeId,
                            SmallTypeId = SmallTypeId,
                            SAMPLE_SN_NO = row.SN_NO
                        });



                    }
                    model.Mast = (from a in model.Detail
                                  group a by new { a.MACHINE_MODEL_NO, a.sales_No,a.salesName } into g
                                  //orderby new ComparerItem() { OrderIndex = b.Key., Id = b.Key.Id } descending
                                  select new salesActualModelV2Mast
                                  {
                                      salesName = g.Key.salesName,
                                      MACHINE_MODEL_NO = g.Key.MACHINE_MODEL_NO,
                                      Actual_Amount = g.Sum(t => t.Actual_Price * t.Actual_Qty),
                                      Actual_Qty = g.Sum(t => t.Actual_Qty)


                                  }).ToList();
                    var shops = (from a in model.Detail
                                 group a by new { a.SHOP_CODE, a.SHOP_NAME } into g
                                 //orderby new ComparerItem() { OrderIndex = b.Key., Id = b.Key.Id } descending
                                 select new
                                 {
                                     SHOP_CODE = g.Key.SHOP_CODE,
                                     SHOP_NAME = g.Key.SHOP_NAME
                                 }).ToList();
                    model.ShopMast = new List<salesActualModelV2Shop>();
                    foreach (var shop in shops)
                    {
                        salesActualModelV2Shop shopmast = new salesActualModelV2Shop() { shopCode = shop.SHOP_CODE, shopName = shop.SHOP_NAME, Mast = new List<salesActualModelV2Mast>() };
                        shopmast.Mast = (from a in model.Detail.Where(p => p.SHOP_CODE.Equals(shop.SHOP_CODE))
                                         group a by new { a.sales_No,a.salesName, a.MACHINE_MODEL_NO } into g
                                         //orderby new ComparerItem() { OrderIndex = b.Key., Id = b.Key.Id } descending
                                         select new salesActualModelV2Mast
                                         {
                                             salesName = g.Key.salesName,
                                             MACHINE_MODEL_NO = g.Key.MACHINE_MODEL_NO,
                                             Actual_Amount = g.Sum(t => t.Actual_Price * t.Actual_Qty),
                                             Actual_Qty = g.Sum(t => t.Actual_Qty)


                                         }).ToList();

                        model.ShopMast.Add(shopmast);
                    }

                }
                else if (queryType.Equals("5"))
                {
                    queryDay = DateTime.Now.AddMonths(-1);
                    GregorianCalendar gc = new GregorianCalendar();
                    int weekOfYear = gc.GetWeekOfYear(queryDay, CalendarWeekRule.FirstDay, DayOfWeek.Monday);

                    model.dateString = queryDay.ToString("yyyyMM");
                    model.Detail = new List<salesActualModelV2Detail>();
                    DateTime startTime = DateTime.Parse(queryDay.ToString("yyyy-MM-") + "01 00:00:00");
                    DateTime EndTime = DateTime.Parse(queryDay.AddMonths(1).ToString("yyyy-MM-") + "01 23:59:59").AddDays(-1);
                    var res = app.GetSalesActualList(startTime, EndTime, goodsType, salesNo, queryType2);
                    foreach (DataSetStanby.V_SALES_TRANS_APP_QUERYRow row in res)
                    {
                        string CONSUMER_ProvinceID = null;
                        if (!row.IsAREA_IDNull())
                        {
                            var areaList = areaAd.GetDataById(row.AREA_ID);
                            if (areaList.Count > 0)
                            {
                                CONSUMER_ProvinceID = areaList[0].PARENT_ID;
                            }
                        }
                        string sex = "0";
                        if (!row.IsSEXNull())
                        {
                            sex = row.SEX.Equals("M") ? "0" : "1";
                        }
                        string SmallTypeId = null;
                        string BigTypeId = null;
                        var SmallTypes = modelAd.GetDataByModelNo(row.MACHINE_MODEL_NO);
                        if (SmallTypes.Count > 0)
                        {
                            SmallTypeId = SmallTypes[0].PRODUCT_TYPE_INFO_ID;
                            var bigType = productType.getBigType(SmallTypeId, "BigType");
                            if (bigType != null)
                            {
                                BigTypeId = bigType.ID;
                            }
                        }
                        model.Detail.Add(new salesActualModelV2Detail()
                        {
                            Actual_Day = row.SALES_DATE.ToString("yyyy/MM/dd"),
                            Actual_Price = (double)row.SALES_PRICE,
                            Actual_Qty = (int)row.SALES_QTY,
                            Actual_Type = row.TRANS_TYPE_NAME,
                            GOODS_TYPE_CODE = getTypeCode(row.GOODS_TYPE_CODE),
                            CONSUMER_NAME = row.CUSTOMER_NAME,
                            GOODS_TYPE_NAME = row.GOODS_TYPE_NAME,
                            sales_No = row.EMPLOYEE_ID,
                            SHOP_CODE = row.SHOP_CODE,
                            SHOP_NAME = row.SHOP_NAME,
                            MACHINE_MODEL_NO = row.MACHINE_MODEL_NO
                             ,
                            INVOICE_FLAG = row.INVOICE_FLAG,
                            id = row.ID,
                            TRANS_NO = row.TRANS_NO.ToString()
                            ,
                            BAY_NAME = row.CONSUMER_NAME,
                            BAY_PHONE_NO = row.CONSUMER_PHONE_NO
                            ,
                            salesName = row.CH_REPORT_NAME,
                            INVOICE_STATUS_NAME = row.INVOICE_STATUS_CODE,
                            UNQUALIFIED_REASON_NAME = row.UNQUALIFIED_REASON_CODE,
                            BOARD_FLAG = (int)row.BOARD_FLAG
                            ,
                            BRAND_CODE = row.BRAND_CODE,
                            TVSIZE = (int)row.TVSIZE
                             ,
                            T_TVSIZEID = row.T_TVSIZEID
                             ,
                            ACT_NO = row.ACT_NO,
                            ACT_NAME = row.ACT_NAME
                             ,
                            CONSUMER_ADD = row.ADDR,
                            CONSUMER_AGE = row.AGE_PERIOD_ID,
                            CONSUMER_XINGBIE = sex
                              ,
                            CONSUMER_ARERID = row.AREA_ID,
                            CONSUMER_ProvinceID = CONSUMER_ProvinceID,
                            BigTypeId = BigTypeId,
                            SmallTypeId = SmallTypeId,
                            SAMPLE_SN_NO = row.SN_NO
                        });



                    }
                    model.Mast = (from a in model.Detail
                                  group a by new { a.MACHINE_MODEL_NO, a.sales_No,a.salesName } into g
                                  //orderby new ComparerItem() { OrderIndex = b.Key., Id = b.Key.Id } descending
                                  select new salesActualModelV2Mast
                                  {
                                      salesName = g.Key.salesName,
                                      MACHINE_MODEL_NO = g.Key.MACHINE_MODEL_NO,
                                      Actual_Amount = g.Sum(t => t.Actual_Price * t.Actual_Qty),
                                      Actual_Qty = g.Sum(t => t.Actual_Qty)


                                  }).ToList();
                    var shops = (from a in model.Detail
                                 group a by new { a.SHOP_CODE, a.SHOP_NAME } into g
                                 //orderby new ComparerItem() { OrderIndex = b.Key., Id = b.Key.Id } descending
                                 select new
                                 {
                                     SHOP_CODE = g.Key.SHOP_CODE,
                                     SHOP_NAME = g.Key.SHOP_NAME
                                 }).ToList();
                    model.ShopMast = new List<salesActualModelV2Shop>();
                    foreach (var shop in shops)
                    {
                        salesActualModelV2Shop shopmast = new salesActualModelV2Shop() { shopCode = shop.SHOP_CODE, shopName = shop.SHOP_NAME, Mast = new List<salesActualModelV2Mast>() };
                        shopmast.Mast = (from a in model.Detail.Where(p => p.SHOP_CODE.Equals(shop.SHOP_CODE))
                                         group a by new { a.sales_No,a.salesName, a.MACHINE_MODEL_NO } into g
                                         //orderby new ComparerItem() { OrderIndex = b.Key., Id = b.Key.Id } descending
                                         select new salesActualModelV2Mast
                                         {
                                             salesName = g.Key.salesName,
                                             MACHINE_MODEL_NO = g.Key.MACHINE_MODEL_NO,
                                             Actual_Amount = g.Sum(t => t.Actual_Price * t.Actual_Qty),
                                             Actual_Qty = g.Sum(t => t.Actual_Qty)


                                         }).ToList();

                        model.ShopMast.Add(shopmast);
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
        }

        /// <summary>
        /// 获取销售上报数据
        /// </summary>
        /// <param name="salesNo">SalesNo</param>
        /// <param name="queryType">查询类型,1:指定日期,2：当周,3:上周,4:当月,5：上月</param>
        /// <param name="queryDay">查询日期</param>
        /// <param name="goodsType">销售类型:S01:正常销售,S02:团购,S04:样机销售</param>
        /// <returns></returns>
        [HttpGet]
        [AllowAnonymous]
        [Route("GetSalesActualV2")]
        public salesActualModelV2 GetSalesActualV2(string salesNo, string queryType,DateTime queryDay, string goodsType)
        {
            try
            {
                marketSalesActualApp app = new marketSalesActualApp();
                salesActualModelV2 model = new salesActualModelV2();
                if (queryType.Equals("1"))
                {
                    model.dateString = queryDay.ToString("yyyy/MM/dd");
                    model.Detail = new List<salesActualModelV2Detail>();
                    DateTime startTime = DateTime.Parse(queryDay.ToString("yyyy-MM-dd ") + "00:00:00");
                    DateTime EndTime = DateTime.Parse(queryDay.ToString("yyyy-MM-dd ") + "23:59:59");
                    var res = app.GetSalesActualList(startTime, EndTime, goodsType, salesNo);
                    foreach (DataSetStanby.V_SALES_TRANS_APP_QUERYRow row in res)
                    {
                        model.Detail.Add(new salesActualModelV2Detail()
                        {
                            Actual_Day = row.SALES_DATE.ToString("yyyy/MM/dd"),
                            Actual_Price = (double)row.SALES_PRICE,
                            Actual_Qty = (int)row.SALES_QTY,
                            Actual_Type = row.TRANS_TYPE_NAME,
                            GOODS_TYPE_CODE = row.GOODS_TYPE_CODE,
                            CONSUMER_NAME = row.CUSTOMER_NAME,
                            GOODS_TYPE_NAME = row.GOODS_TYPE_NAME,
                            sales_No = row.EMPLOYEE_ID,
                            SHOP_CODE = row.SHOP_CODE,
                            SHOP_NAME = row.SHOP_NAME,
                            MACHINE_MODEL_NO = row.MACHINE_MODEL_NO
                          ,
                            INVOICE_FLAG = row.INVOICE_FLAG,
                            id = row.ID,
                            TRANS_NO = row.TRANS_NO.ToString(),
                            BAY_NAME = row.CONSUMER_NAME,
                            BAY_PHONE_NO = row.CONSUMER_PHONE_NO
                        });
                    }

                }
                else if (queryType.Equals("2"))
                {
                    queryDay = DateTime.Now;
                    GregorianCalendar gc = new GregorianCalendar();
                    int weekOfYear = gc.GetWeekOfYear(queryDay, CalendarWeekRule.FirstDay, DayOfWeek.Monday);

                    model.dateString = queryDay.ToString("yyyy") + "W" + weekOfYear.ToString() + "(" + GetWeekFirstDaySun(queryDay).ToString("MMdd") + "-" + GetWeekLastDaySat(queryDay).ToString("MMdd") + ")";
                    model.Detail = new List<salesActualModelV2Detail>();
                    DateTime startTime = DateTime.Parse(GetWeekFirstDaySun(queryDay).ToString("yyyy-MM-dd ") + "00:00:00");
                    DateTime EndTime = DateTime.Parse(GetWeekLastDaySat(queryDay).ToString("yyyy-MM-dd ") + "23:59:59");
                    var res = app.GetSalesActualList(startTime, EndTime, goodsType, salesNo);
                    foreach (DataSetStanby.V_SALES_TRANS_APP_QUERYRow row in res)
                    {
                        model.Detail.Add(new salesActualModelV2Detail()
                        {
                            Actual_Day = row.SALES_DATE.ToString("yyyy/MM/dd"),
                            Actual_Price = (double)row.SALES_PRICE,
                            Actual_Qty = (int)row.SALES_QTY,
                            Actual_Type = row.TRANS_TYPE_NAME,
                            GOODS_TYPE_CODE = row.GOODS_TYPE_CODE,
                            CONSUMER_NAME = row.CUSTOMER_NAME,

                            GOODS_TYPE_NAME = row.GOODS_TYPE_NAME,
                            sales_No = row.EMPLOYEE_ID,
                            SHOP_CODE = row.SHOP_CODE,
                            SHOP_NAME = row.SHOP_NAME,
                            MACHINE_MODEL_NO = row.MACHINE_MODEL_NO
                              ,
                            INVOICE_FLAG = row.INVOICE_FLAG,
                            id = row.ID,
                            TRANS_NO = row.TRANS_NO.ToString()
                            ,
                            BAY_NAME = row.CONSUMER_NAME,
                            BAY_PHONE_NO = row.CONSUMER_PHONE_NO
                        });



                    }
                    model.Mast = (from a in model.Detail
                                  group a by new { a.MACHINE_MODEL_NO } into g
                                  //orderby new ComparerItem() { OrderIndex = b.Key., Id = b.Key.Id } descending
                                  select new salesActualModelV2Mast
                                  {
                                      MACHINE_MODEL_NO = g.Key.MACHINE_MODEL_NO,
                                      Actual_Amount = g.Sum(t => t.Actual_Price * t.Actual_Qty),
                                      Actual_Qty = g.Sum(t => t.Actual_Qty)


                                  }).ToList();

                }
                else if (queryType.Equals("3"))
                {
                    queryDay = DateTime.Now.AddDays(-7);
                    GregorianCalendar gc = new GregorianCalendar();
                    int weekOfYear = gc.GetWeekOfYear(queryDay, CalendarWeekRule.FirstDay, DayOfWeek.Monday);

                    model.dateString = queryDay.ToString("yyyy") + "W" + weekOfYear.ToString() + "(" + GetWeekFirstDaySun(queryDay).ToString("MMdd") + "-" + GetWeekLastDaySat(queryDay).ToString("MMdd") + ")";
                    model.Detail = new List<salesActualModelV2Detail>();
                    DateTime startTime = DateTime.Parse(GetWeekFirstDaySun(queryDay).ToString("yyyy-MM-dd ") + "00:00:00");
                    DateTime EndTime = DateTime.Parse(GetWeekLastDaySat(queryDay).ToString("yyyy-MM-dd ") + "23:59:59");
                    var res = app.GetSalesActualList(startTime, EndTime, goodsType, salesNo);
                    foreach (DataSetStanby.V_SALES_TRANS_APP_QUERYRow row in res)
                    {
                        model.Detail.Add(new salesActualModelV2Detail()
                        {
                            Actual_Day = row.SALES_DATE.ToString("yyyy/MM/dd"),
                            Actual_Price = (double)row.SALES_PRICE,
                            Actual_Qty = (int)row.SALES_QTY,
                            Actual_Type = row.TRANS_TYPE_NAME,
                            GOODS_TYPE_CODE = row.GOODS_TYPE_CODE,
                            CONSUMER_NAME = row.CUSTOMER_NAME,
                            GOODS_TYPE_NAME = row.GOODS_TYPE_NAME,
                            sales_No = row.EMPLOYEE_ID,
                            SHOP_CODE = row.SHOP_CODE,
                            SHOP_NAME = row.SHOP_NAME,
                            MACHINE_MODEL_NO = row.MACHINE_MODEL_NO
                             ,
                            INVOICE_FLAG = row.INVOICE_FLAG,
                            id = row.ID,
                            TRANS_NO = row.TRANS_NO.ToString()
                            ,
                            BAY_NAME = row.CONSUMER_NAME,
                            BAY_PHONE_NO = row.CONSUMER_PHONE_NO
                        });



                    }
                    model.Mast = (from a in model.Detail
                                  group a by new { a.MACHINE_MODEL_NO } into g
                                  //orderby new ComparerItem() { OrderIndex = b.Key., Id = b.Key.Id } descending
                                  select new salesActualModelV2Mast
                                  {
                                      MACHINE_MODEL_NO = g.Key.MACHINE_MODEL_NO,
                                      Actual_Amount = g.Sum(t => t.Actual_Price * t.Actual_Qty),
                                      Actual_Qty = g.Sum(t => t.Actual_Qty)


                                  }).ToList();

                }
                else if (queryType.Equals("4"))
                {
                    queryDay = DateTime.Now;
                    GregorianCalendar gc = new GregorianCalendar();
                    int weekOfYear = gc.GetWeekOfYear(queryDay, CalendarWeekRule.FirstDay, DayOfWeek.Monday);

                    model.dateString = queryDay.ToString("yyyyMM");
                    model.Detail = new List<salesActualModelV2Detail>();
                    DateTime startTime = DateTime.Parse(queryDay.ToString("yyyy-MM-") + "01 00:00:00");
                    DateTime EndTime = DateTime.Parse(queryDay.AddMonths(1).ToString("yyyy-MM-") + "01 23:59:59").AddDays(-1);
                    var res = app.GetSalesActualList(startTime, EndTime, goodsType, salesNo);
                    foreach (DataSetStanby.V_SALES_TRANS_APP_QUERYRow row in res)
                    {
                        model.Detail.Add(new salesActualModelV2Detail()
                        {
                            Actual_Day = row.SALES_DATE.ToString("yyyy/MM/dd"),
                            Actual_Price = (double)row.SALES_PRICE,
                            Actual_Qty = (int)row.SALES_QTY,
                            Actual_Type = row.TRANS_TYPE_NAME,
                            GOODS_TYPE_CODE = row.GOODS_TYPE_CODE,
                            CONSUMER_NAME = row.CUSTOMER_NAME,
                            GOODS_TYPE_NAME = row.GOODS_TYPE_NAME,
                            sales_No = row.EMPLOYEE_ID,
                            SHOP_CODE = row.SHOP_CODE,
                            SHOP_NAME = row.SHOP_NAME,
                            MACHINE_MODEL_NO = row.MACHINE_MODEL_NO
                             ,
                            INVOICE_FLAG = row.INVOICE_FLAG,
                            id = row.ID,
                            TRANS_NO = row.TRANS_NO.ToString()
                            ,
                            BAY_NAME = row.CONSUMER_NAME,
                            BAY_PHONE_NO = row.CONSUMER_PHONE_NO
                        });



                    }
                    model.Mast = (from a in model.Detail
                                  group a by new { a.MACHINE_MODEL_NO } into g
                                  //orderby new ComparerItem() { OrderIndex = b.Key., Id = b.Key.Id } descending
                                  select new salesActualModelV2Mast
                                  {
                                      MACHINE_MODEL_NO = g.Key.MACHINE_MODEL_NO,
                                      Actual_Amount = g.Sum(t => t.Actual_Price * t.Actual_Qty),
                                      Actual_Qty = g.Sum(t => t.Actual_Qty)


                                  }).ToList();

                }
                else if (queryType.Equals("5"))
                {
                    queryDay = DateTime.Now.AddMonths(-1);
                    GregorianCalendar gc = new GregorianCalendar();
                    int weekOfYear = gc.GetWeekOfYear(queryDay, CalendarWeekRule.FirstDay, DayOfWeek.Monday);

                    model.dateString = queryDay.ToString("yyyyMM");
                    model.Detail = new List<salesActualModelV2Detail>();
                    DateTime startTime = DateTime.Parse(queryDay.ToString("yyyy-MM-") + "01 00:00:00");
                    DateTime EndTime = DateTime.Parse(queryDay.AddMonths(1).ToString("yyyy-MM-") + "01 23:59:59").AddDays(-1);
                    var res = app.GetSalesActualList(startTime, EndTime, goodsType, salesNo);
                    foreach (DataSetStanby.V_SALES_TRANS_APP_QUERYRow row in res)
                    {
                        model.Detail.Add(new salesActualModelV2Detail()
                        {
                            Actual_Day = row.SALES_DATE.ToString("yyyy/MM/dd"),
                            Actual_Price = (double)row.SALES_PRICE,
                            Actual_Qty = (int)row.SALES_QTY,
                            Actual_Type = row.TRANS_TYPE_NAME,
                            GOODS_TYPE_CODE = row.GOODS_TYPE_CODE,
                            CONSUMER_NAME = row.CUSTOMER_NAME,
                            GOODS_TYPE_NAME = row.GOODS_TYPE_NAME,
                            sales_No = row.EMPLOYEE_ID,
                            SHOP_CODE = row.SHOP_CODE,
                            SHOP_NAME = row.SHOP_NAME,
                            MACHINE_MODEL_NO = row.MACHINE_MODEL_NO
                             ,
                            INVOICE_FLAG = row.INVOICE_FLAG,
                            id = row.ID,
                            TRANS_NO = row.TRANS_NO.ToString()
                            ,
                            BAY_NAME = row.CONSUMER_NAME,
                            BAY_PHONE_NO = row.CONSUMER_PHONE_NO
                        });



                    }
                    model.Mast = (from a in model.Detail
                                  group a by new { a.MACHINE_MODEL_NO } into g
                                  //orderby new ComparerItem() { OrderIndex = b.Key., Id = b.Key.Id } descending
                                  select new salesActualModelV2Mast
                                  {
                                      MACHINE_MODEL_NO = g.Key.MACHINE_MODEL_NO,
                                      Actual_Amount = g.Sum(t => t.Actual_Price * t.Actual_Qty),
                                      Actual_Qty = g.Sum(t => t.Actual_Qty)


                                  }).ToList();

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
        }

        //[HttpGet]
        //[Route("GetSalesActiveAct")]
        //public List<ActiveActModel> GetSalesActiveAct(String salesNo)
        //{

        //}
        /// <summary>
        /// 补录发票
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("addSuppUp")]
        public async Task<IHttpActionResult> addSuppUp(UppUpModel model)
        {
            marketSalesApp userapp = new marketSalesApp();

            UserInfoResultModel t = userapp.GetUserInfo(User.Identity.GetUserId());
            marketSalesActualApp app = new marketSalesActualApp();

            var res = app.SuppUp(model.id, t.SalesNo, model.files_id);
            if (!res.isOk)
            {
                return BadRequest(res.errorMessage);
            }
            return Ok();
        }

        /// <summary>
        /// 删除整个门店活动上报
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [Route("DeleteSalesActiveAct")]
        public async Task<IHttpActionResult> DeleteSalesActiveAct(String id)
        {
            marketSalesActiveActApp actApp = new marketSalesActiveActApp();
            var res = actApp.deleteAct(id);
            if (!res.isOk)
            {
                return BadRequest(res.errorMessage);
            }
            return Ok();
        }
        /// <summary>
        /// 删除活动上报中的促销内容
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("DeleteSalesActiveActSub")]
        public async Task<IHttpActionResult> DeleteSalesActiveActSub(String id)
        {
            marketSalesActiveActApp actApp = new marketSalesActiveActApp();
            var res= actApp.deleteActSub(id);
            if (!res.isOk)
            {
                return BadRequest(res.errorMessage);
            }
            return Ok();
        }
        /// <summary>
        /// 获取已上报活动数据
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Route("GetSalesActiveAct")]
        public List<ActiveActModel> GetSalesActiveAct(String salesNo)
        {
            List<Models.ActiveActModel> models = new List<Models.ActiveActModel>();
            marketSalesActiveListApp app = new marketSalesActiveListApp();
            List<ActiveModel2> tempModel = app.getList(salesNo);
            marketSalesActiveActApp actApp = new marketSalesActiveActApp();
            foreach (var temp in tempModel)
            {

                var actList = actApp.getAct(salesNo, temp.ACT_NO);
                foreach (var act in actList)
                {
                    string patch = act.Created_Time.Value.ToString("yyyyMM");
                    Models.ActiveActModel model = new Models.ActiveActModel()
                    {
                          id=act.id,
                        ACT_END_DATE = temp.ACT_END_DATE,
                        ACT_NAME = temp.ACT_NAME,
                        ACT_NO = temp.ACT_NO,
                        ACT_START_DATE = temp.ACT_START_DATE,
                        ACT_TYPE_CODE = temp.ACT_TYPE_CODE,
                        ACT_TYPE_NAME = temp.ACT_TYPE_NAME,
                        CUSTOMER_CODE = temp.CUSTOMER_CODE,
                        CUSTOMER_NAME = temp.CUSTOMER_NAME,
                        T_TYPEID = temp.T_TYPEID,
                        T_TYPENAME = temp.T_TYPENAME,
                          sales_No=act.sales_No,
                           SHOP_CODE=act.SHOP_CODE,
                            SHOP_NAME=act.SHOP_NAME,
                             file_id_Type002=act.file_id_Type002,
                              file_id_Type003=act.file_id_Type003,
                               file_id_Type004=act.file_id_Type004, ADD_DATE=act.Created_Time.Value
                    };

                    model.file_Model_Type002 = new List<imageModel>();
                    model.file_Model_Type003 = new List<imageModel>();
                    model.file_Model_Type004 = new List<imageModel>();
                    if (act.file_id_Type002 != null && !act.file_id_Type002.Equals(""))
                    {
                        string[] files = act.file_id_Type002.Split(",".ToArray());
                        foreach (var file in files)
                        {
                            model.file_Model_Type002.Add(new imageModel() { id = file, url = "https://iretailerapp.flnet.com/Messages/APPUploadFile/" + patch + "/" + file + ".jpg" });
                        }
                    }
                    if (act.file_id_Type003 != null && !act.file_id_Type003.Equals(""))
                    {
                        string[] files = act.file_id_Type003.Split(",".ToArray());
                        foreach (var file in files)
                        {
                            model.file_Model_Type003.Add(new imageModel() { id = file, url = "https://iretailerapp.flnet.com/Messages/APPUploadFile/" + patch + "/" + file + ".jpg" });
                        }
                    }
                    if (act.file_id_Type004 != null && !act.file_id_Type004.Equals(""))
                    {
                        string[] files = act.file_id_Type004.Split(",".ToArray());
                        foreach (var file in files)
                        {
                            model.file_Model_Type004.Add(new imageModel() { id = file, url = "https://iretailerapp.flnet.com/Messages/APPUploadFile/" + patch + "/" + file + ".jpg" });
                        }
                    }
                    var subEnts = actApp.getActSub(act.id);
                    model.ActiveSub = new List<ActiveActModelSub>();
                    foreach (var subEnt in subEnts)
                    {
                        ActiveActModelSub modelSub = new ActiveActModelSub() { BRAND_CODE=subEnt.BRAND_CODE, BRAND_NAME=subEnt.BRAND_NAME, id=subEnt.id,
                         file_id_Type001= subEnt.file_id_Type001, IS_NEW_PRD_FLAG=subEnt.IS_NEW_PRD_FLAG.Value, MACHINE_MODEL_NO=subEnt.MACHINE_MODEL_NO, SALES_PRICE=subEnt.SALES_PRICE.Value,
                         TVSIZE=subEnt.TVSIZE.Value, T_TVSIZEID=subEnt.T_TVSIZEID};
                        modelSub.file_Model_Type001 = new List<imageModel>();
                        if (subEnt.file_id_Type001 != null && !subEnt.file_id_Type001.Equals(""))
                        {
                            string[] files = subEnt.file_id_Type001.Split(",".ToArray());
                            foreach (var file in files)
                            {
                                modelSub.file_Model_Type001.Add(new imageModel() { id = file, url = "https://iretailerapp.flnet.com/Messages/APPUploadFile/" + patch + "/" + file + ".jpg" });
                            }
                        }
                        model.ActiveSub.Add(modelSub);
                    }
                    models.Add(model);
                }

                
                
            }
            return models;
        }
        /// <summary>
        /// 活动上报
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("SalesActiveActV2")]
        public async Task<IHttpActionResult> SalesActiveActV2(ActiveActModel model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);

            }
            marketSalesActiveActApp app = new marketSalesActiveActApp();

            marketSalesActiveActEntity ent = new marketSalesActiveActEntity()
            {
                ACT_NAME = model.ACT_NAME,
                ACT_NO = model.ACT_NO,
                file_id_Type002 = model.file_id_Type002,
                file_id_Type003 = model.file_id_Type003,
                file_id_Type004 = model.file_id_Type004,
                sales_No = model.sales_No,
                SHOP_CODE = model.SHOP_CODE,
                SHOP_NAME = model.SHOP_NAME,

            };
            //List<marketSalesActiveActSubEntity> subEnts = new List<marketSalesActiveActSubEntity>();
            //foreach (var submodel in model.ActiveSub)
            //{
            //    subEnts.Add(new marketSalesActiveActSubEntity()
            //    {

            //        BRAND_CODE = submodel.BRAND_CODE,
            //        BRAND_NAME = submodel.BRAND_NAME,
            //        file_id_Type001 = submodel.file_id_Type001,
            //        IS_NEW_PRD_FLAG = submodel.IS_NEW_PRD_FLAG,
            //        MACHINE_MODEL_NO = submodel.MACHINE_MODEL_NO,
            //        SALES_PRICE = submodel.SALES_PRICE,
            //        TVSIZE = submodel.TVSIZE,
            //        T_TVSIZEID = submodel.T_TVSIZEID,
            //    });
            //}
            salesActualChangeRes res = app.insertEnt(ent, null);
            if (!res.isOk)
            {
                return BadRequest(res.errorMessage);
            }

            return Ok();
        }


        /// <summary>
        /// 样品列表查询
        /// </summary>
        /// <param name="salesNo"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("GetSalesSampleAct")]
        public List<SampleActModel> GetSalesSampleAct(String salesNo)
        {
            List<SampleActModel> models = new List<SampleActModel>();
            marketSampleActApp app = new marketSampleActApp();
            var ents = app.getEnts(salesNo);
            foreach (var ent in ents)
            {
                string patch = ent.Created_Time.Value.ToString("yyyyMM");
                SampleActModel model = new SampleActModel() {
                    id = ent.id, PRODUCT_TYPE_CODE = ent.PRODUCT_TYPE_CODE,
                    PRODUCT_TYPE_NAME = ent.PRODUCT_TYPE_NAME,
                    sales_No = ent.sales_No,
                    SAMPLE_UP_NO = ent.SAMPLE_UP_NO,
                    file_ids = ent.file_ids,
                    SHOP_CODE = ent.SHOP_CODE,
                    SHOP_NAME = ent.SHOP_NAME,
                    SN_NO = ent.SN_NO,
                     day=ent.SAMPLE_DATE.Value,
                       MACHINE_MODEL_NO=ent.MACHINE_MODEL_NO,
                        newDay=ent.Modify_Time.Value,
                    files = new List<imageModel>(), SAMPLE_TYPE_CODE=ent.SAMPLE_TYPE_CODE, SAMPLE_TYPE_NAME=ent.SAMPLE_TYPE_NAME
                    , PRODUCT_STATUS_CODE=ent.PRODUCT_STATUS_CODE, REMARK=ent.REMARK
                };
                if (ent.file_ids != null && !ent.file_ids.Equals(""))
                {
                    string[] files = ent.file_ids.Split(",".ToArray());
                    foreach (var file in files)
                    {
                        model.files.Add(new imageModel() { id = file, url = "https://iretailerapp.flnet.com/Messages/APPUploadFile/" + patch + "/" + file + ".jpg" });
                    }
                }
                models.Add(model);
            }
            return models;

        }
        /// <summary>
        /// 获取样机序号列表
        /// </summary>
        /// <param name="shopCode">门店代号</param>
        /// <param name="mac">型号</param>
        /// <returns></returns>
        [HttpGet]
        [Route("GetSampleSnNos")]
        [AllowAnonymous]
        public List<String> GetSampleSnNos(string shopCode, string mac)
        {
            try { 
            V_SALES_SAMPLE_APP_SN_QUERYTableAdapter app = new V_SALES_SAMPLE_APP_SN_QUERYTableAdapter();
            var ents = app.GetDataBy(shopCode, mac);
            List<String> model = new List<string>();
            foreach (var ent in ents)
            {
                model.Add(ent.SN_NO);
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
}
        /// <summary>
        /// 样品上报
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("SalesSampleAct")]
        public async Task<IHttpActionResult> SalesSampleAct(SampleActModel model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);

            }

            marketSalesApp userapp = new marketSalesApp();

            UserInfoResultModel t = userapp.GetUserInfo(User.Identity.GetUserId());

            marketSampleActApp app = new marketSampleActApp();
            salesActualChangeRes res = app.SaveEnt( new marketSampleActEntity() {
                 file_ids=model.file_ids,
                  PRODUCT_TYPE_CODE=model.PRODUCT_TYPE_CODE,
                   PRODUCT_TYPE_NAME=model.PRODUCT_TYPE_NAME,
                    sales_No=model.sales_No,
                     SAMPLE_UP_NO=model.SAMPLE_UP_NO,
                      SHOP_CODE=model.SHOP_CODE,
                       id=model.id,
                        SHOP_NAME=model.SHOP_NAME,
                         MACHINE_MODEL_NO=model.MACHINE_MODEL_NO,
                         SN_NO =model.SN_NO,
                SAMPLE_DATE = model.day

            }, t.SalesNo);
            if (!res.isOk)
            {
                return BadRequest(res.errorMessage);
            }

            return Ok();
        }
        /// <summary>
        /// 样品上报
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("SalesSampleActV2")]
        public async Task<IHttpActionResult> SalesSampleActV2(SampleActModel model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);

            }

            marketSalesApp userapp = new marketSalesApp();

            UserInfoResultModel t = userapp.GetUserInfo(User.Identity.GetUserId());

            marketSampleActApp app = new marketSampleActApp();
            if (model.isDelete == 1)
            {
                salesActualChangeRes res = app.DeleteEnt(model.id);
                if (!res.isOk)
                {
                    return BadRequest(res.errorMessage);
                }
            }
            else
            {
                salesActualChangeRes res = app.SaveEnt(new marketSampleActEntity()
                {
                    file_ids = model.file_ids,
                    PRODUCT_TYPE_CODE = model.PRODUCT_TYPE_CODE,
                    PRODUCT_TYPE_NAME = model.PRODUCT_TYPE_NAME,
                    sales_No = model.sales_No,
                    SAMPLE_UP_NO = model.SAMPLE_UP_NO,
                    SHOP_CODE = model.SHOP_CODE,
                    id = model.id,
                    SHOP_NAME = model.SHOP_NAME,
                    MACHINE_MODEL_NO = model.MACHINE_MODEL_NO,
                    SN_NO = model.SN_NO,
                    SAMPLE_TYPE_CODE = model.SAMPLE_TYPE_CODE,
                    SAMPLE_TYPE_NAME = model.SAMPLE_TYPE_NAME,
                    UP_TYPE_CODE = model.UP_TYPE_CODE,
                    UP_TYPE_NAME = model.UP_TYPE_NAME,
                    SAMPLE_DATE = model.day, PRODUCT_STATUS_CODE = model.PRODUCT_STATUS_CODE, REMARK = model.REMARK
                }, t.SalesNo);
                if (!res.isOk)
                {
                    return BadRequest(res.errorMessage);
                }
            }
            

            return Ok();
        }

        /// <summary>
        /// 活动上报
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("SalesActiveAct")]
        public async Task<IHttpActionResult> SalesActiveAct(ActiveActModel model)
        {
            return BadRequest("APP版本过旧，请更新新版再使用该功能");
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);

            }
            marketSalesActiveActApp app = new marketSalesActiveActApp();

            marketSalesActiveActEntity ent = new marketSalesActiveActEntity() {
                ACT_NAME = model.ACT_NAME,
                ACT_NO = model.ACT_NO,
                file_id_Type002 = model.file_id_Type002,
                file_id_Type003 = model.file_id_Type003,
                file_id_Type004 = model.file_id_Type004,
                sales_No = model.sales_No,
                SHOP_CODE = model.SHOP_CODE,
                SHOP_NAME = model.SHOP_NAME,
                
            };
            List<marketSalesActiveActSubEntity> subEnts = new List<marketSalesActiveActSubEntity>();
            foreach (var submodel in model.ActiveSub)
            {
                subEnts.Add(new marketSalesActiveActSubEntity()
                {

                    BRAND_CODE = submodel.BRAND_CODE,
                    BRAND_NAME = submodel.BRAND_NAME,
                    file_id_Type001 = submodel.file_id_Type001,
                    IS_NEW_PRD_FLAG = submodel.IS_NEW_PRD_FLAG,
                    MACHINE_MODEL_NO = submodel.MACHINE_MODEL_NO,
                    SALES_PRICE = submodel.SALES_PRICE,
                    TVSIZE = submodel.TVSIZE,
                    T_TVSIZEID = submodel.T_TVSIZEID,
                });
            }
            salesActualChangeRes res = app.insertEnt(ent, subEnts);
            if (!res.isOk)
            {
                return BadRequest(res.errorMessage);
            }

            return Ok();
        }


        [HttpGet]
        [AllowAnonymous]
        [Route("GetActualOther")]
        public List<SalesOtherModel> GetActualOther(String salesNo)
        {
            try { 
            List<SalesOtherModel> models = new List<SalesOtherModel>();
            marketSalesOtherApp app = new marketSalesOtherApp();
                marketShopApp shopApp = new marketShopApp();
            var list = app.getAct(salesNo);
                marketBrandApp banApp = new marketBrandApp();
            foreach (var item in list)
            {
                string patch = item.Created_Time.Value.ToString("yyyyMM");
                SalesOtherModel model = new SalesOtherModel() {
                     ACT_NAME=item.ACT_NAME,
                      ACT_NO=item.ACT_NO,
                       IS_CURVED_FLAG= item.IS_CURVED_FLAG==null ?0:item.IS_CURVED_FLAG.Value,
                         IS_OLED_FLAG= item.IS_OLED_FLAG == null ? 0 : item.IS_OLED_FLAG.Value,
                          IS_QUANTUM_DOT_FLAG= item.IS_QUANTUM_DOT_FLAG == null ? 0 : item.IS_QUANTUM_DOT_FLAG.Value,
                           IS_SMART_TV_FLAG= item.IS_SMART_TV_FLAG == null ? 0 : item.IS_SMART_TV_FLAG.Value,
                            KEY_MODEL_CODE=item.KEY_MODEL_CODE,
                             sales_No=item.userid,
                              SALES_PRICE= item.SALES_PRICE == null ? 0 : item.SALES_PRICE.Value,
                               T_TVIZEID=item.T_TVIZEID,
                                BRAND_CODE=item.BRAND_CODE,
                                 shopCode=item.shopCode,
                                  prdFileIds=item.prdFileIds,
                                   id=item.id, prdFiles= new List<imageModel>(),
                                    CreatedTime=item.Created_Time.Value,
                                     BRAND_NAME= banApp.GetOtherBrandName(item.BRAND_CODE),
                                      shopName= shopApp.getShopName(item.shopCode)

                };

                if (model.prdFileIds != null && !model.prdFileIds.Equals(""))
                {
                    string[] files = model.prdFileIds.Split(",".ToArray());
                    foreach (var file in files)
                    {
                        model.prdFiles.Add(new imageModel() { id = file, url = "https://iretailerapp.flnet.com/Messages/APPUploadFile/" + patch + "/" + file + ".jpg" });
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
        [HttpPost]
        [Route("SalesActualOther")]
        public async Task<IHttpActionResult> SalesActualOther(SalesOtherModel model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);

            }
            marketSalesOtherApp app = new marketSalesOtherApp();
            salesActualChangeRes res = app.insertSales(new marketSalesOtherEntity() {
                BRAND_CODE = model.BRAND_CODE,
                KEY_MODEL_CODE = model.KEY_MODEL_CODE,
                IS_CURVED_FLAG = model.IS_CURVED_FLAG,
                IS_OLED_FLAG = model.IS_OLED_FLAG,
                IS_QUANTUM_DOT_FLAG = model.IS_QUANTUM_DOT_FLAG,
                 T_TVIZEID=model.T_TVIZEID,
                  SALES_PRICE=model.SALES_PRICE,
                   userid= model.sales_No,
                    shopCode=model.shopCode, IS_SMART_TV_FLAG=model.IS_SMART_TV_FLAG, prdFileIds=model.prdFileIds, ACT_NO=model.ACT_NO,ACT_NAME=model.ACT_NAME
            });
            if (!res.isOk)
            {
                return BadRequest(res.errorMessage);
            }
            return Ok();
        }
        /// <summary>
        /// 销售上报或修改
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("SalesActualV2")]
        public async Task<IHttpActionResult> SalesActualV2(salesActualModel model)
        {
            return BadRequest("版本已停用，请下载更新新版APP！");
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }


            var agent = this.Request.Headers.UserAgent;
           // return BadRequest(agent.Count().ToString());
            bool isAndroid = false;
            foreach (var value in agent)
            {
                if (value.ToString().IndexOf("okhttp") >= 0)
                {
                    isAndroid = true;
                   // return BadRequest("安卓");
                }
            }
           // return BadRequest("IOS");
            marketSalesActualApp app = new marketSalesActualApp();
            marketMachineModelEntity modelEnt = app.GetMachineModel(model.MACHINE_MODEL_NO);

            salesActualChangeRes res = app.SalesActualV2(new marketSalesActualEntity()
            {
                Actual_Day = model.Actual_Day,
                Actual_Price = model.Actual_Price,
                Actual_Qty = model.Actual_Qty,
                Actual_Type = model.Actual_Type,
                MACHINE_MODEL_NO = model.MACHINE_MODEL_NO,
                sales_No = model.sales_No,
                SHOP_CODE = model.SHOP_CODE,
                T_TVSIZEID = modelEnt.T_TVSIZEID,
                isSync = 0,
                 CONSUMER_NAME= model.CONSUMER_NAME,
                  CONSUMER_PHONE_NO=model.CONSUMER_PHONE_NO,
                   GOODS_TYPE_CODE= model.GOODS_TYPE_CODE,
                    GOODS_TYPE_NAME=model.GOODS_TYPE_NAME,
                     file_id=model.file_id, ACT_NO=model.ACT_NO, ACT_NAME=model.ACT_NAME, BOARD_FLAG=model.BOARD_FLAG
            }, isAndroid);
            if (!res.isOk)
            {
                return BadRequest(res.errorMessage);
            }
            return Ok();
        }
        /// <summary>
        /// 销售上报或修改
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("SalesActualV5")]
        public async Task<IHttpActionResult> SalesActualV5(salesActualModel model)
        {

            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                string XINBIE = null;
                if (model.CONSUMER_XINGBIE != null)
                {
                    XINBIE = model.CONSUMER_XINGBIE.Equals("0") ? "M" : "F";
                }

                var agent = this.Request.Headers.UserAgent;
                // return BadRequest(agent.Count().ToString());
                bool isAndroid = false;
                foreach (var value in agent)
                {
                    if (value.ToString().IndexOf("okhttp") >= 0)
                    {
                        isAndroid = true;
                        // return BadRequest("安卓");
                    }
                }
                // return BadRequest("IOS");
                marketSalesActualApp app = new marketSalesActualApp();
                marketMachineModelEntity modelEnt = app.GetMachineModel(model.MACHINE_MODEL_NO);

                salesActualChangeRes res = app.SalesActualV3(new marketSalesActualEntity()
                {
                    Actual_Day = model.Actual_Day,
                    Actual_Price = model.Actual_Price,
                    Actual_Qty = model.Actual_Qty,
                    Actual_Type = model.Actual_Type,
                    MACHINE_MODEL_NO = model.MACHINE_MODEL_NO,
                    sales_No = model.sales_No,
                    SHOP_CODE = model.SHOP_CODE,
                    T_TVSIZEID = modelEnt.T_TVSIZEID,
                    isSync = 0,
                    CONSUMER_NAME = model.CONSUMER_NAME,
                    CONSUMER_PHONE_NO = model.CONSUMER_PHONE_NO,
                    GOODS_TYPE_CODE = model.GOODS_TYPE_CODE,
                    GOODS_TYPE_NAME = model.GOODS_TYPE_NAME,
                    file_id = model.file_id,
                    ACT_NO = model.ACT_NO,
                    ACT_NAME = model.ACT_NAME,
                    BOARD_FLAG = model.BOARD_FLAG,
                    CH_NO = model.old_id,
                    CONSUMER_ADD = model.CONSUMER_ADD,
                    CONSUMER_AGE = model.CONSUMER_AGE,
                    CONSUMER_XINGBIE = XINBIE,
                    CONSUMER_ARERID = model.CONSUMER_ARERID,
                    SAMPLE_SN_NO = model.SAMPLE_SN_NO, deviceUUid=model.deviceUUid
                }, true);

                if (!res.isOk)
                {
                    return BadRequest(res.errorMessage);
                }
                
               SalesActualRetunModel retunModel = new SalesActualRetunModel();
               

                V_CRM_MEMBER_APPTableAdapter memberAd = new V_CRM_MEMBER_APPTableAdapter();
                //var table = memberAd.GetDataByPhone(model.CONSUMER_PHONE_NO);
                retunModel.isMember = 0;
                retunModel.isNeedMember = 0;
                /*
                if (table.Rows.Count > 0)
                {
                    retunModel.isMember = 1;
                    retunModel.memberNumber = table[0].MEMBER_NO;
                }
                
                marketShopApp shopApp = new marketShopApp();
                var shopEnt = shopApp.getShop(model.SHOP_CODE);
                if (shopEnt != null)
                {
                    if (shopEnt.T_BUID.Equals("SZ"))
                    {
                        retunModel.isNeedMember = 1;
                    }
                }
                */
                return Ok(retunModel);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }


        }
        /// <summary>
        /// 销售上报或修改
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("SalesActualV4")]
        public async Task<IHttpActionResult> SalesActualV4(salesActualModel model)
        {
           
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                string XINBIE = null;
                if (model.CONSUMER_XINGBIE != null)
                {
                    XINBIE = model.CONSUMER_XINGBIE.Equals("0") ? "M" : "F";
                }
                
                var agent = this.Request.Headers.UserAgent;
                // return BadRequest(agent.Count().ToString());
                bool isAndroid = false;
                foreach (var value in agent)
                {
                    if (value.ToString().IndexOf("okhttp") >= 0)
                    {
                        isAndroid = true;
                        // return BadRequest("安卓");
                    }
                }
                // return BadRequest("IOS");
                marketSalesActualApp app = new marketSalesActualApp();
                marketMachineModelEntity modelEnt = app.GetMachineModel(model.MACHINE_MODEL_NO);
                
                salesActualChangeRes res = app.SalesActualV3(new marketSalesActualEntity()
                {
                    Actual_Day = model.Actual_Day,
                    Actual_Price = model.Actual_Price,
                    Actual_Qty = model.Actual_Qty,
                    Actual_Type = model.Actual_Type,
                    MACHINE_MODEL_NO = model.MACHINE_MODEL_NO,
                    sales_No = model.sales_No,
                    SHOP_CODE = model.SHOP_CODE,
                    T_TVSIZEID = modelEnt.T_TVSIZEID,
                    isSync = 0,
                    CONSUMER_NAME = model.CONSUMER_NAME,
                    CONSUMER_PHONE_NO = model.CONSUMER_PHONE_NO,
                    GOODS_TYPE_CODE = model.GOODS_TYPE_CODE,
                    GOODS_TYPE_NAME = model.GOODS_TYPE_NAME,
                    file_id = model.file_id,
                    ACT_NO = model.ACT_NO,
                    ACT_NAME = model.ACT_NAME,
                    BOARD_FLAG = model.BOARD_FLAG,
                    CH_NO = model.old_id,
                    CONSUMER_ADD = model.CONSUMER_ADD,
                    CONSUMER_AGE = model.CONSUMER_AGE,
                    CONSUMER_XINGBIE = XINBIE,
                    CONSUMER_ARERID = model.CONSUMER_ARERID,
                    SAMPLE_SN_NO = model.SAMPLE_SN_NO
                }, true);

                if (!res.isOk)
                {
                    return BadRequest(res.errorMessage);
                }
                /*
               SalesActualRetunModel retunModel = new SalesActualRetunModel();
                
                V_CRM_MEMBER_APPTableAdapter memberAd = new V_CRM_MEMBER_APPTableAdapter();
                var table = memberAd.GetDataByPhone(model.CONSUMER_PHONE_NO);
                retunModel.isMember = 0;
                retunModel.isNeedMember = 0;
                
                if (table.Rows.Count > 0)
                {
                    retunModel.isMember = 1;
                    retunModel.memberNumber = table[0].MEMBER_NO;
                }
                marketShopApp shopApp = new marketShopApp();
                var shopEnt = shopApp.getShop(model.SHOP_CODE);
                if (shopEnt != null)
                {
                    if (shopEnt.T_BUID.Equals("SZ"))
                    {
                        retunModel.isNeedMember = 1;
                    }
                }
                */
                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
            
            
        }
        /// <summary>
        /// 销售上报或修改
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("SalesActualV3")]
        public async Task<IHttpActionResult> SalesActualV3(salesActualModel model)
        {
            return BadRequest("版本已停用，请下载更新新版APP！");
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }


            var agent = this.Request.Headers.UserAgent;
            // return BadRequest(agent.Count().ToString());
            bool isAndroid = false;
            foreach (var value in agent)
            {
                if (value.ToString().IndexOf("okhttp") >= 0)
                {
                    isAndroid = true;
                    // return BadRequest("安卓");
                }
            }
            // return BadRequest("IOS");
            marketSalesActualApp app = new marketSalesActualApp();
            marketMachineModelEntity modelEnt = app.GetMachineModel(model.MACHINE_MODEL_NO);

            salesActualChangeRes res = app.SalesActualV3(new marketSalesActualEntity()
            {
                Actual_Day = model.Actual_Day,
                Actual_Price = model.Actual_Price,
                Actual_Qty = model.Actual_Qty,
                Actual_Type = model.Actual_Type,
                MACHINE_MODEL_NO = model.MACHINE_MODEL_NO,
                sales_No = model.sales_No,
                SHOP_CODE = model.SHOP_CODE,
                T_TVSIZEID = modelEnt.T_TVSIZEID,
                isSync = 0,
                CONSUMER_NAME = model.CONSUMER_NAME,
                CONSUMER_PHONE_NO = model.CONSUMER_PHONE_NO,
                GOODS_TYPE_CODE = model.GOODS_TYPE_CODE,
                GOODS_TYPE_NAME = model.GOODS_TYPE_NAME,
                file_id = model.file_id,
                ACT_NO = model.ACT_NO,
                ACT_NAME = model.ACT_NAME,
                BOARD_FLAG = model.BOARD_FLAG, CH_NO=model.old_id
            }, isAndroid);
            if (!res.isOk)
            {
                return BadRequest(res.errorMessage);
            }
            return Ok();
        }

        /// <summary>
        /// 销售上报或修改
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("SalesActual")]
        public async Task<IHttpActionResult> SalesActual(salesActualModel model)
        {
            return BadRequest("版本已停用，请下载更新新版APP！");
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            marketSalesActualApp app = new marketSalesActualApp();
            marketMachineModelEntity modelEnt = app.GetMachineModel(model.MACHINE_MODEL_NO);
            
            salesActualChangeRes res = app.SalesActual(new marketSalesActualEntity() { Actual_Day = model.Actual_Day, Actual_Price=model.Actual_Price,
                 Actual_Qty=model.Actual_Qty, Actual_Type=model.Actual_Type, MACHINE_MODEL_NO= model.MACHINE_MODEL_NO, sales_No=model.sales_No,
                  SHOP_CODE=model.SHOP_CODE, T_TVSIZEID= modelEnt.T_TVSIZEID, isSync=0
            });
            if (!res.isOk)
            {
                return BadRequest(res.errorMessage);
            }
            return Ok();
        }
        /// <summary>
        /// 图片上报&意见和建议
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("ReportInfo")]
        
        public async Task<IHttpActionResult> ReportInfo(rePortViewModel model)
        {

            marketreportInfoApp app = new marketreportInfoApp();
            marketreportInfoEntity ent = new marketreportInfoEntity()
            { type= model.type, desc=model.type, subType=model.subType, fileId=model.fileId , F_Id=System.Guid.NewGuid().ToString()};
            ent.F_CreatorTime = System.DateTime.Now;
            ent.F_CreatorUserId = User.Identity.GetUserId();
            app.report(ent);
            return Ok();
        }
        /// <summary>
        /// 图片上传接口
        /// </summary>
        /// <param name="LoginToken"></param>
        /// <param name="Base64String"></param>
        /// <returns></returns>
        [HttpPost]


        static bool RunCmd2( string cmdStr)
        {
            bool result = false;
            try
            {
                using (Process myPro = new Process())
                {
                    myPro.StartInfo.FileName = "xcopy";
                    myPro.StartInfo.UseShellExecute = false;
                    myPro.StartInfo.RedirectStandardInput = true;
                    myPro.StartInfo.RedirectStandardOutput = true;
                    myPro.StartInfo.RedirectStandardError = true;
                    myPro.StartInfo.CreateNoWindow = true;
                    myPro.Start();
                    //如果调用程序路径中有空格时，cmd命令执行失败，可以用双引号括起来 ，在这里两个引号表示一个引号（转义）
                    string str = string.Format(@"{1} {2}", cmdStr, "&exit");

                    myPro.StandardInput.WriteLine(str);
                    myPro.StandardInput.AutoFlush = true;
                    myPro.WaitForExit();

                    result = true;
                }
            }
            catch
            {

            }
            return result;
        }

        private static string CmdPath = @"C:\Windows\System32\cmd.exe";

        /// <summary>
        /// 执行cmd命令
        /// 多命令请使用批处理命令连接符：
        /// <![CDATA[
        /// &:同时执行两个命令
        /// |:将上一个命令的输出,作为下一个命令的输入
        /// &&：当&&前的命令成功时,才执行&&后的命令
        /// ||：当||前的命令失败时,才执行||后的命令]]>
        /// 其他请百度
        /// </summary>
        /// <param name="cmd"></param>
        /// <param name="output"></param>
        public static void RunCmd(string cmd, out string output)
        {
            cmd = cmd + " &exit";//说明：不管命令是否成功均执行exit命令，否则当调用ReadToEnd()方法时，会处于假死状态
            using (Process p = new Process())
            {
                p.StartInfo.FileName = CmdPath;
                p.StartInfo.UseShellExecute = false;        //是否使用操作系统shell启动
                p.StartInfo.RedirectStandardInput = true;   //接受来自调用程序的输入信息
                p.StartInfo.RedirectStandardOutput = true;  //由调用程序获取输出信息
                p.StartInfo.RedirectStandardError = true;   //重定向标准错误输出
                p.StartInfo.CreateNoWindow = true;          //不显示程序窗口
                p.Start();//启动程序

                //向cmd窗口写入命令
                p.StandardInput.WriteLine(cmd);
                p.StandardInput.AutoFlush = true;

                //获取cmd窗口的输出信息
                output = p.StandardOutput.ReadToEnd();
                p.WaitForExit();//等待程序执行完退出进程
                p.Close();
            }
        }
        /// <summary>
        /// 通过multipart/form-data方式上传文件
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [Route("UploadImageV3")]
        [AllowAnonymous]
        public async Task<fileViewModel> PostFile2()
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
                string fileid = "";
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
                        fileinfo.CopyTo(System.Configuration.ConfigurationManager.AppSettings["filePatch"] + "\\" + System.DateTime.Now.ToString("yyyyMM") + "\\" + ent.id + ".jpg", true);

                        // System.IO.File.Copy(fileDir + "\\" + ent.id + ".jpg", System.Configuration.ConfigurationManager.AppSettings["filePatch"] + "\\" + System.DateTime.Now.ToString("yyyyMM") + "\\" + ent.id + ".jpg", true);
                    }
                    fileinfo.Delete();//删除原文件
                    fileid = fileid + ent.id + ",";
                    //
                }
                return new fileViewModel() { fileId = fileid};
            }
            catch (System.Exception e)
            {
                return new fileViewModel() { fileId = e.Message };
            }
            
        }

        /// <summary>
        /// 通过multipart/form-data方式上传文件
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [Route("UploadImageV2")]
        [AllowAnonymous]
        public async Task<fileViewModel> PostFile()
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
                        fileinfo.CopyTo(System.Configuration.ConfigurationManager.AppSettings["filePatch"] + "\\" + System.DateTime.Now.ToString("yyyyMM") + "\\" + ent.id +".jpg", true);

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
            return new fileViewModel() { fileId = ""};
        }



        [Route("UploadImage")]
        [AllowAnonymous]
        public async Task<fileViewModel> UploadImage()
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
                        fileinfo.CopyTo(System.Configuration.ConfigurationManager.AppSettings["filePatch"] + "\\" + System.DateTime.Now.ToString("yyyyMM") + "\\" + ent.id + ".jpg", true);

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



    }
}