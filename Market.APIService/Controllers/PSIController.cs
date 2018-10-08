using DataSynchronizationLib.SCMTableAdapters;
using Market.APIService.Models;
using NFine.Application.APPManage;
using NFine.Application.TaskManage;
using NFine.Domain._03_Entity.APPManage;
using NFine.Domain._04_IRepository.TaskManage;
using NFine.Repository.TaskManage;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Market.APIService.Controllers
{
    public class MemberListModel
    {
        /// <summary>
        /// 名字
        /// </summary>
        public string name { get; set; }
        /// <summary>
        /// 会员号
        /// </summary>
        public string memberNo { get; set; }
        /// <summary>
        /// 分类会员号码
        /// </summary>
        public string mf_memberNo { get; set; }
        /// <summary>
        /// 电话号码
        /// </summary>
        public string phoneNumber { get; set; }
        /// <summary>
        /// 头像
        /// </summary>
        public string pic_Url { get; set; }
        /// <summary>
        /// 到访时间
        /// </summary>
        public string time { get; set; }
        /// <summary>
        /// 门店
        /// </summary>
        public string shop { get; set; }
        /// <summary>
        /// 关注型号,多个用逗号分隔
        /// </summary>
        public String lookMacs { get; set; }
        /// <summary>
        /// 到访次数
        /// </summary>
        public int vistaCount { get; set; }
        public string beachId { get; set; }
        /// <summary>
        /// 是否已接待
        /// </summary>
        public int IsRead { get; set; }

        /// <summary>
        /// 月平均到访次数
        /// </summary>
        public int AVGMONTH_VISITCOUNT { get; set; }
        /// <summary>
        /// 会员地址
        /// </summary>
        public string MEMBER_ADDR { get; set; }

        /// <summary>
        /// 会员等级 Sliver:普通会员,Gold:VIP会员,Diamond:钻石会员
        /// </summary>
        public string MEMBER_LEVEL_CODE { get; set; }

        /// <summary>
        /// 会员等级 Sliver:普通会员,Gold:VIP会员,Diamond:钻石会员
        /// </summary>
        public string MEMBER_LEVEL_NAME { get; set; }
        /// <summary>
        /// 消费类型
        /// </summary>
        public string CG_TYPE_NAME { get; set; }
        /// <summary>
        /// 识别位置
        /// </summary>
        public string SCAN_AREA { get; set; }

        


    }
        public class VModel
    {
        public int TotalTagQty { get; set; }
        public int SelfTotalTagQty { get; set; }
        public int SelfTotalCQty { get; set; }
        public int TotalCQty { get; set; }
        public string OrgId { get; set; }
        public string OrgName { get; set; }
        public string UserId { get; set; }
        public string Month { get; set; }
        public string MonthName { get; set; }
        public string Prodect { get; set; }
        public string ProdectName { get; set; }
        public string Brand { get; set; }
        public string BrandName { get; set; }
        public string billNo { get; set; }
        public string salesCode { get; set; }
        public int type { get; set; }
        public List<subVModel> subModel  { get; set; }
        public List<qtyModel> cQtyModel { get; set; }
        

    }
    public class tagModel
    {
       
        public string OrgId { get; set; }
        public string PorgId { get; set; }
        public string UserId { get; set; }
        public string Month { get; set; }
        public string MonthName { get; set; }
        public string Prodect { get; set; }
        public string billNo { get; set; }
        public string BrandCode { get; set; }
        public List<qtyModel> subModel { get; set; }

    }
    public class subVModel
    {
        public string cityName { get; set; }
        public string cityCode { get; set; }
        public string popCode { get; set; }
        public string popName { get; set; }
        public int tagQty { get; set; }
        public int cmQty { get; set; }
    }
    public class PModel
    {
        

        public string UserId { get; set; }
        public string isDet { get; set; }
        public List<KeyValueModel> Month { get; set; }
        public List<KeyValueModel> Prodect { get; set; }
        public List<KeyValueModel> Brand { get; set; }
        public List<KeyValueModel> OrgList { get; set; }
        public List<KeyValueModel> CityList { get; set; }
        public int type { get; set; }
        public string salesCode { get; set; }
    }
    public class qtyModel
    {
        public string day { get; set; }
        public int value { get; set; }

        public int status { get; set; }
        public int isClose { get; set; }
    }
    public class KeyValueModel
    {
        public string key { get; set; }
        public string keyValue { get; set; }
        public bool isSelected { get; set; }
        public string id { get; set; }

    }
    public class shTagModel
    {
        public string month { get; set; }
        public string userid { get; set; }
        public string  prodect { get; set; }
        public string orgId { get; set; }
        public string bllNo { get; set; }
        public string PorgId { get; set; }
        public string BrandCode { get; set; }
        public int qty { get; set; }
       /// <summary>
       /// 1:按比例分配 2:指定数量分配
       /// </summary>
        public int type { get; set; }

    }
    public class sQtyModel
    {
        public string day { get; set; }
        public string userid { get; set; }
        public string prodect { get; set; }
        public string orgId { get; set; }
        public string PorgId { get; set; }
        public string BrandCode { get; set; }
        public int qty { get; set; }
       

    }
    public class userInfo
    {
        public string userId { get; set; }
        public string salesCode { get; set; }
    }

    public class PSIController : Controller
    {
        private VModel getdata(string BrandValue, string ProdectValue, string MonthValue,string UserId,int type,string salesCode,string orgCode)
        {
            
            VModel model = new VModel();
            marketSalesApp salesApp = new marketSalesApp();
            //组织ID 219
            //查询目标总数
            model.type = type;
            DateTime m = DateTime.ParseExact(MonthValue, "yyyyMM", CultureInfo.CurrentCulture);
            if (type == 2)
            {
                string porgCode = salesApp.getOrginfoBySubId(orgCode, "BU").id;
                model.OrgId = orgCode;
                model.OrgName = salesApp.getOrgInfo(orgCode).MANAGE_ORG_NAME;
                model.Prodect = ProdectValue;
                //查找Bill单号

                JS5_S12_SALES_TARGET_BILL_MTableAdapter ad = new JS5_S12_SALES_TARGET_BILL_MTableAdapter();
                var totalEnts = ad.GetDataBy(salesApp.getOrginfoBySubId(porgCode, "CHANNEL").id, model.Prodect, m.ToString("yyyy/MM"),BrandValue);

                if (totalEnts.Count <= 0)
                {

                    model.TotalTagQty = 0;
                    model.billNo = null;

                }
                else
                {
                   
                    model.billNo = totalEnts.First().BILL_NO;

                    JS5_S12_SALES_TARGET_BILL_DTableAdapter dad = new JS5_S12_SALES_TARGET_BILL_DTableAdapter();

                    model.TotalTagQty =(int) dad.QtyQuery(BrandValue, porgCode, model.Prodect, model.billNo);
                }
                model.Brand = BrandValue;
                JS5_S12_BRAND_INFOTableAdapter BandAd = new JS5_S12_BRAND_INFOTableAdapter();
                model.BrandName = BandAd.GetDataByCode(BrandValue).First().BRAND_NAME;

                model.Month = MonthValue;
                model.MonthName = m.ToString("yyyy年MM月");

                JS5_S12_PRODUCT_TYPE_INFOTableAdapter prdAd = new JS5_S12_PRODUCT_TYPE_INFOTableAdapter();
                model.ProdectName = prdAd.GetDataByCode(ProdectValue).First().TREE_NODE_NAME;

                //查询总共已提交数量

                PSIApp psiApp = new PSIApp();
                var cents = psiApp.CmList(model.Prodect, MonthValue, porgCode,BrandValue);
                model.TotalCQty = cents.Sum(p => p.cmQty).Value;//psiApp.TotalCmQty(ProdectValue, MonthValue, model.OrgId);
                model.UserId = UserId;
                model.subModel = new List<subVModel>();
                var ents = psiApp.CmList(model.Prodect, MonthValue, model.OrgId, UserId,BrandValue);

                model.SelfTotalTagQty = psiApp.TagList(model.Prodect, MonthValue, model.OrgId, UserId,BrandValue).Sum(p => p.totalTagQty).Value;

               
               model.SelfTotalCQty =  ents.Sum(p => p.cmQty).Value;//psiApp.TotalCmQty(ProdectValue, MonthValue, model.OrgId,UserId);
                model.cQtyModel = new List<qtyModel>();
                DateTime startDay = m;
                DateTime temp = m;
                for (int i = 0; i < 35; i++)
                {

                    if (startDay.Month == temp.Month)
                    {
                        int isClose = 0;
                        if (DateTime.Compare(temp, DateTime.ParseExact(DateTime.Now.ToString("yyyy-MM-dd 00:00:00"), "yyyy-MM-dd HH:mm:ss", CultureInfo.CurrentCulture)) < 0)
                            isClose = 1;
                        var ent = ents.Find(p => p.day.Value.ToString("yyyy-MM-dd").Equals(temp.ToString("yyyy-MM-dd")));
                        if (ent != null)
                        {
                            model.cQtyModel.Add(new qtyModel()
                            {
                                day = temp.ToString("yyyy-MM-dd"),
                                status = ent.status.HasValue ? ent.status.Value : 0,
                                value = ent.cmQty.Value,
                                isClose = isClose
                            });

                        }
                        else
                        {
                            model.cQtyModel.Add(new qtyModel()
                            {
                                day = temp.ToString("yyyy-MM-dd"),
                                status = 0,
                                value = 0,
                                isClose = isClose
                            });
                        }
                    }
                    temp = temp.AddDays(1);

                }
            }
            else
            {
                model.OrgId = orgCode;
                model.OrgName = salesApp.getOrgInfo(orgCode).MANAGE_ORG_NAME;
                model.Prodect = ProdectValue;
                
                //查找Bill单号

                JS5_S12_SALES_TARGET_BILL_MTableAdapter ad = new JS5_S12_SALES_TARGET_BILL_MTableAdapter();
                var totalEnts = ad.GetDataBy(salesApp.getOrginfoBySubId(orgCode, "CHANNEL").id, model.Prodect, m.ToString("yyyy/MM"), BrandValue);

                if (totalEnts.Count <= 0)
                {

                    model.TotalTagQty = 0;
                    model.billNo = null;

                }
                else
                {

                    model.billNo = totalEnts.First().BILL_NO;

                    JS5_S12_SALES_TARGET_BILL_DTableAdapter dad = new JS5_S12_SALES_TARGET_BILL_DTableAdapter();

                    model.TotalTagQty = (int)dad.QtyQuery(BrandValue, orgCode, model.Prodect, model.billNo);
                }
                model.Brand = BrandValue;
                JS5_S12_BRAND_INFOTableAdapter BandAd = new JS5_S12_BRAND_INFOTableAdapter();
                model.BrandName = BandAd.GetDataByCode(BrandValue).First().BRAND_NAME;

                model.Month = MonthValue;
                
                model.MonthName = m.ToString("yyyy年MM月");

                JS5_S12_PRODUCT_TYPE_INFOTableAdapter prdAd = new JS5_S12_PRODUCT_TYPE_INFOTableAdapter();
                model.ProdectName = prdAd.GetDataByCode(ProdectValue).First().TREE_NODE_NAME;

                //查询总共已提交数量

                PSIApp psiApp = new PSIApp();
                var cents = psiApp.CmList(model.Prodect, MonthValue, model.OrgId,model.Brand);
                model.TotalCQty = cents.Sum(p => p.cmQty).Value;//psiApp.TotalCmQty(ProdectValue, MonthValue, model.OrgId);
                model.UserId = UserId;
                model.subModel = new List<subVModel>();
                //查询城市列表
                //JS5_S12_SALES_MANAGE_ORG_INFOTableAdapter orgInfo = new JS5_S12_SALES_MANAGE_ORG_INFOTableAdapter();

               
                
                var orgs = salesApp.getOrginfoByPCode(orgCode, "CITY");
                var tagEnts = psiApp.TagList(model.Prodect, MonthValue, model.OrgId,BrandValue);
                foreach (var org in orgs)
                {
                    
                        var ent = cents.Where(p => p.orgId.Equals(org.id));
                        var tagEnt = tagEnts.Find(p => p.orgId.Equals(org.id));

                    var sales = salesApp.getUserInfoByOrgId(org.id);
                    //if (sales != null)
                    //{
                    var subModel = new subVModel()
                    {
                        cmQty = ent.Sum(p => p.cmQty).Value,
                        tagQty = tagEnt == null ? 0 : tagEnt.totalTagQty.Value,
                        cityCode = org.id,
                        cityName = org.MANAGE_ORG_NAME,
                        popCode = "",
                        popName = "",

                    };
                       
                    if (sales != null)
                    {
                        subModel.popCode = sales.id;
                        subModel.popName = sales.Name;

                    }
                    model.subModel.Add(subModel);
                    //}
                   
                }

            }
            
           

            return model;
        }
        [HttpPost]
        public ActionResult cmQty(sQtyModel model)
        {
            PSIApp psiApp = new PSIApp();
            marketSalesApp salesApp = new marketSalesApp();
            psiApp.cQty(model.prodect, model.userid, model.userid, salesApp.getOrginfoBySubId(model.orgId, "BU").id, model.orgId, model.qty, model.day,model.BrandCode);
            return Content("提交成功！");
        }

        [HttpPost]
        public ActionResult shTag(shTagModel model)
        {
            if (model.type == 1)
            {
                JS5_S12_SALES_TARGET_BILL_DTableAdapter dad = new JS5_S12_SALES_TARGET_BILL_DTableAdapter();
                int totalQty = (int)dad.QtyQuery(model.BrandCode, model.PorgId, model.prodect, model.bllNo);
                //var tagEnt = tagAd.GetDataByNo(model.bllNo);
                if (totalQty <= 0)
                {
                    return Content("分配失败，未找到目标！");
                }
                marketSalesApp salesApp = new marketSalesApp();
                //查询城市列表
                //JS5_S12_SALES_MANAGE_ORG_INFOTableAdapter orgInfo = new JS5_S12_SALES_MANAGE_ORG_INFOTableAdapter();

                // List<DataSynchronizationLib.SCM.JS5_S12_SALES_MANAGE_ORG_INFORow> citys = new List<DataSynchronizationLib.SCM.JS5_S12_SALES_MANAGE_ORG_INFORow>();

                // var orgs = orgInfo.GetDataByPid(model.PorgId);
                //foreach (var org in orgs)
                //{

                //  foreach (var orgCity in orgInfo.GetDataByPid(org.ID))
                //{
                //  citys.Add(orgCity);
                //}

                //}

                var orgs = salesApp.getOrginfoByPCode(model.PorgId, "CITY");
                PSIApp psiApp = new PSIApp();
                foreach (var org in orgs)
                {

                    int qty = (totalQty/ orgs.Count) + 1;
                    var sales = salesApp.getUserInfoByOrgId(org.id);
                    if (sales != null)
                    {
                        psiApp.shTag(model.prodect, sales.id, model.userid, model.PorgId, org.id, model.month, qty,model.BrandCode);
                    }
                   
                    
                }


               
            }
            else
            {
                PSIApp psiApp = new PSIApp();
                psiApp.shTag(model.prodect, model.userid, model.userid, model.PorgId, model.orgId, model.month, model.qty,model.BrandCode);
            }

            return Content("分配成功！");
        }

        public PartialViewResult TagDataView(string BrandValue, string ProdectValue, string MonthValue, string MonthName, string UserId,string orgId,string porgId,string billNo)
        {

            JS5_S12_SALES_TARGET_BILL_DTableAdapter ad = new JS5_S12_SALES_TARGET_BILL_DTableAdapter();
            var ents = ad.GetDataBy(BrandValue,orgId,ProdectValue,billNo);
            tagModel model = new tagModel() { billNo=billNo, Month=MonthValue, MonthName=MonthName, BrandCode=BrandValue, UserId=UserId, OrgId=orgId, Prodect=ProdectValue, PorgId=porgId };
            model.subModel = new List<qtyModel>();
            foreach (var ent in ents)
            {
                model.subModel.Add(new qtyModel() { day=ent.SALES_DATE.ToString("yyyy-MM-dd"), value=(int)ent.TARGET_QTY });
            }
            return PartialView("_PSITagDayPartialPage", model);
        }

        public PartialViewResult QueryData(string BrandValue,string ProdectValue,string MonthValue,string UserId,string SalesNo,string OrgCode)
        {   
            return PartialView("_PSIPartialPage", getdata(BrandValue, ProdectValue, MonthValue, UserId,1, SalesNo,OrgCode));
        }
        public PartialViewResult QueryCData(string BrandValue, string ProdectValue, string MonthValue, string UserId,string SalesNo, string OrgCode)
        {
            return PartialView("_PSICMPartialPage", getdata(BrandValue, ProdectValue, MonthValue, UserId, 2, SalesNo, OrgCode));
        }
        [HttpPost]
        public ActionResult setStatus(string userId, string city, string month, string prodect,int status,string BrandCode)
        {
            PSIApp psiApp = new PSIApp();
            psiApp.setStatus(prodect, month, city, userId,status, BrandCode);
            return Content("操作成功！");
        }
        public PartialViewResult DayDataView(string userId, string city, string  month, string  prodect,string BrandCode)
        {
            //prodect = "4";
            PSIApp psiApp = new PSIApp();
            var ents = psiApp.CmList(prodect, month, city, userId, BrandCode);
           
            tagModel model = new tagModel() ;
            model.subModel = new List<qtyModel>();
           
            foreach (var ent in ents.OrderBy(p => p.day))
            {
                model.subModel.Add(new qtyModel() { day = ent.day.Value.ToString("yyyy-MM-dd"), value = ent.cmQty.Value});
            }
            return PartialView("_PSIDayPartialPage", model);
        }
        public ActionResult Index2(string userId,string type,string SalesNo)
        {
            int typeValue = int.Parse(type);

            PModel model = new PModel();
            model.Month = new List<KeyValueModel>();
            DateTime now = DateTime.Now;
            model.Month.Add(new KeyValueModel() { key = now.AddMonths(-2).ToString("yyyyMM"), keyValue = now.AddMonths(-2).ToString("yyyy年MM月"), isSelected = false });
            model.Month.Add(new KeyValueModel() { key = now.AddMonths(-1).ToString("yyyyMM"), keyValue = now.AddMonths(-1).ToString("yyyy年MM月"), isSelected = false });
            model.Month.Add(new KeyValueModel() { key = now.ToString("yyyyMM"), keyValue = now.ToString("yyyy年MM月"), isSelected = true });
            model.Month.Add(new KeyValueModel() { key = now.AddMonths(1).ToString("yyyyMM"), keyValue = now.AddMonths(1).ToString("yyyy年MM月"), isSelected = false });
            model.Month.Add(new KeyValueModel() { key = now.AddMonths(2).ToString("yyyyMM"), keyValue = now.AddMonths(2).ToString("yyyy年MM月"), isSelected = false });
            model.Brand = new List<KeyValueModel>();
            JS5_S12_BRAND_INFOTableAdapter ad = new JS5_S12_BRAND_INFOTableAdapter();
            var ents = ad.GetDataBy();

            foreach (var ent in ents)
            {
                model.Brand.Add(new KeyValueModel() { key = ent.BRAND_CODE, keyValue = ent.BRAND_NAME, isSelected = false });
            }
            model.Brand.First().isSelected = true;
            model.Prodect = new List<KeyValueModel>();
            var ad2 = new JS5_S12_PRODUCT_TYPE_INFOTableAdapter();
            var ents2 = ad2.GetDataBy();
            foreach (var ent in ents2)
            {
                model.Prodect.Add(new KeyValueModel() { key = ent.ID, keyValue = ent.TREE_NODE_NAME, isSelected = false });
            }
            model.Prodect.First().isSelected = true;
            marketSalesApp salesApp = new marketSalesApp();
            model.UserId = userId;
            model.type = typeValue;
            model.salesCode = SalesNo;
            if (typeValue == 1)
            {
                model.OrgList = new List<KeyValueModel>();
                var orglist = salesApp.getEmpOrgBySalesNo(SalesNo, "BUManager");
                foreach (var org in orglist)
                {
                    var orginfo = salesApp.getOrgInfo(org.ORG_ID);
                    model.OrgList.Add(new KeyValueModel() { key = orginfo.id, keyValue = orginfo.MANAGE_ORG_NAME });
                }
                model.OrgList.First().isSelected = true;
                ViewData["VModel"] = getdata(model.Brand.First().key, model.Prodect.First().key, now.ToString("yyyyMM"), userId, typeValue, SalesNo, model.OrgList.First().key);
            }
            else if (typeValue == 2)
            {
                model.CityList = new List<KeyValueModel>();
                var orglist = salesApp.getEmpOrgBySalesNo(SalesNo, "Citymanager");
                foreach (var org in orglist)
                {
                    var orginfo = salesApp.getOrgInfo(org.ORG_ID);
                    model.CityList.Add(new KeyValueModel() { key = orginfo.id, keyValue = orginfo.MANAGE_ORG_NAME});
                }
                model.CityList.First().isSelected = true;
                ViewData["VModel"] = getdata(model.Brand.First().key, model.Prodect.First().key, now.ToString("yyyyMM"), userId, typeValue, SalesNo, model.CityList.First().key);
            }
           
            
            ViewData["PModel"] = model;
            //ViewData["ValueModel"] = model;
            return View("PSIView");
        }
        [HttpPost]
        public ActionResult savePopInfo(MemberRegestMode model)
        {
            user_leaveAPP app = new user_leaveAPP();
            //app.AllowLeave(id, int.Parse(state), userId);
            return Content("操作成功！");
        }
        public PartialViewResult popView(string mfid,string userId)
        {
            MemberMode model = new MemberMode();
            marketShopApp shopApp = new marketShopApp();
            
           
            V_CRM_MEMBER_APPTableAdapter ad = new V_CRM_MEMBER_APPTableAdapter();
            var table = ad.GetDataByMFID(mfid);
            if (table.Count > 0)
            {
                var row = table[0];
                model = new MemberMode()
                {
                    adder = row.MEMBER_ADDR,
                    LAST_BUY_DATE = row.LAST_BUY_DATE,
                    LAST_BUY_MODEL = row.LAST_BUY_MODEL,
                    LAST_BUY_QTY = (int)row.LAST_BUY_QTY,
                    LAST_INSHOP = shopApp.getShopName(row.LAST_INSHOP_CODE),
                    LAST_INSHOP_TIME = row.LAST_INSHOP_TIME,
                    MEMBER_NO = row.MEMBER_NO,
                    MF_MEMBER_ID = row.MF_MEMBER_ID,
                    MEMO = row.MEMO,
                    name = row.MEMBER_NAME,
                    phoneNumber = row.MOBILE,
                    weiChat = row.WECHAT_NO,
                    VISIT_COUNT = (int)row.VISIT_COUNT,
                    picRUL = row.IsIMGFACETIMENull() ? "" : row.IMGFACETIME.Replace("/data/upload", "https://iretailerapp.flnet.com/MessagesQC"),
                    age = (int)row.AGE, MemberType = row.CG_TYPE_NAME, regShop = shopApp.getShopName(row.REG_SHOP_CODE),
                    regShopCode = row.REG_SHOP_CODE
                };
                if (model.regShopCode == null)
                {
                    List<marketSalesShopEntity> shops = shopApp.getShopByUserId(userId);
                    if (shops.Count > 0)
                    {
                        model.regShop = shops[0].SHOP_NAME;
                        model.regShopCode = shops[0].SHOP_CODE;
                    }
                }
                model.IOList = new List<Member_IOSHOPMode>();
                V_CRM_MEMBER_IOSHOP_LOG_APPTableAdapter IOAD = new V_CRM_MEMBER_IOSHOP_LOG_APPTableAdapter();
                var iologs = IOAD.GetDataByMFID(row.MF_MEMBER_ID);
                foreach (var item in iologs)
                {
                    model.IOList.Add(new Member_IOSHOPMode() { SHOP_NAME = item.SHOP_NAME, Time = item.VISIT_TIME });
                }
                model.ByList = new List<Member_ByMode>();
                V_CRM_MEMBER_HISSALES_APPTableAdapter byAD = new V_CRM_MEMBER_HISSALES_APPTableAdapter();
                var bylogs = byAD.GetData(row.MOBILE);
                foreach (var item in bylogs)
                {
                    model.ByList.Add(new Member_ByMode() { byTime = item.BUY_DATE, MAC = item.BUY_MODEL, qty = (int)item.BUY_QTY });
                }
                model.lookMacsList = new List<string>();
                if (!row.IsFOLLOW_PRODUCTNull())
                {
                    model.lookMacsList.AddRange(row.FOLLOW_PRODUCT.Split(",".ToCharArray()));
                }
               
            }
            return PartialView("POPView", model);
        }
        public PartialViewResult PopInfoList(string bid, string shopCode)
        {
            TaskMemberApp app = new TaskMemberApp();
            marketShopApp shopApp = new marketShopApp();
            var items = app.getData(bid, shopCode);
            List<MemberListModel> list = new List<MemberListModel>();
            V_CRM_MEMBER_APPTableAdapter ad = new V_CRM_MEMBER_APPTableAdapter();

            foreach (var item in items)
            {
                var temp = new MemberListModel()
                {
                    memberNo = item.memberId,
                    mf_memberNo = item.mfMemberId,
                    shop = shopApp.getShopName(item.shopCode),
                    pic_Url = item.picUrl == null ? "" : item.picUrl.Replace("/data/upload", "https://iretailerapp.flnet.com/MessagesQC"),
                    time = item.InTime.Value.ToString("yyyy-MM-dd HH:mm"),
                    name = ""
                };
                var table = ad.GetDataByNo(item.mfMemberId);
                if (table.Count > 0)
                {
                    temp.name = table[0].IsMEMBER_NAMENull() ? "" : table[0].MEMBER_NAME;
                }
                list.Add(temp);

            }
            return PartialView("_POPListSubView", list);
        }
        public ActionResult PopInfo(string bid,string shopCode,string userId)
        {
            // if (billNo.Equals("MBSTT201802061701585396"))
            //{
            TaskMemberApp app = new TaskMemberApp();
            marketShopApp shopApp = new marketShopApp();
            var items = app.getData(bid, shopCode);
            List<MemberListModel> list = new List<MemberListModel>();
            V_CRM_MEMBER_APPTableAdapter ad = new V_CRM_MEMBER_APPTableAdapter();
            
            foreach (var item in items)
            {
                var temp= new MemberListModel()
                {
                    memberNo = item.memberId,
                    mf_memberNo = item.mfMemberId,
                    shop = shopApp.getShopName(item.shopCode),
                    pic_Url = item.picUrl == null ? "" : item.picUrl.Replace("/data/upload", "https://iretailerapp.flnet.com/MessagesQC"),
                    time = item.InTime.Value.ToString("yyyy-MM-dd HH:mm"), name=""
                };
                var table = ad.GetDataByNo(item.mfMemberId);
                if (table.Count > 0)
                {
                    temp.name = table[0].IsMEMBER_NAMENull() ? "" : table[0].MEMBER_NAME;
                }
                list.Add(temp);

            }
            ViewData["vModel"] = list;
            ViewData["bid"] = bid;
            ViewData["userId"] = userId;
            ViewData["shopCode"] = shopCode;
            return View("POPListView");
            //}
            //else
            //{
              //  return View("POPView2");
            //}
        }
        // GET: PSI
        public ActionResult Index(string userId)
        {
            
            if (userId.Equals(null))
            {
                return View("_NoFunction");
            }
            // UserRoleAd.GetDataByCode();
            marketSalesApp salesApp = new marketSalesApp();
            var userInfo = salesApp.getUserInfoBySalesNo(userId);
            
            if (userInfo == null)
            {
                return View("_NoFunction");
            }
            userId = userInfo.id;
            var empEntitys = salesApp.getEmpOrgBySalesNo(userInfo.SalesNo);
            List<string> rolues = new List<string>();
            foreach (var empEnt in empEntitys)
            {
                if (!rolues.Exists(p => p.Equals(empEnt.JOB_CODE)) && (empEnt.JOB_CODE.Equals("BUManager") || empEnt.JOB_CODE.Equals("Citymanager")))
                {
                    rolues.Add(empEnt.JOB_CODE);
                }
            }
           
            int type = 1;
            //判断用户角色
            if (rolues.Count <= 0)
            {
                return View("_NoFunction");
            }
            else
            {
                if (rolues.Exists(p => p.Equals("BUManager")) && rolues.Exists(p => p.Equals("Citymanager")))//表示是多角色需要进入多角色选择画面
                {

                    ViewData["userInfo"] = new userInfo() { userId = userInfo.id,  salesCode= userInfo.SalesNo};
                    return View("_SelectRoue");
                }
                else if (rolues.Exists(p => p.Equals("BUManager")))//进入战区长功能
                {
                    type = 1;
                }
                else//进入城市经理功能
                {
                    type = 2;
                }
            }

            
            
                PModel model = new PModel();
                model.Month = new List<KeyValueModel>();
                DateTime now = DateTime.Now;
                model.Month.Add(new KeyValueModel() { key = now.AddMonths(-2).ToString("yyyyMM"), keyValue = now.AddMonths(-2).ToString("yyyy年MM月"), isSelected = false });
                model.Month.Add(new KeyValueModel() { key = now.AddMonths(-1).ToString("yyyyMM"), keyValue = now.AddMonths(-1).ToString("yyyy年MM月"), isSelected = false });
                model.Month.Add(new KeyValueModel() { key = now.ToString("yyyyMM"), keyValue = now.ToString("yyyy年MM月"), isSelected = true });
                model.Month.Add(new KeyValueModel() { key = now.AddMonths(1).ToString("yyyyMM"), keyValue = now.AddMonths(1).ToString("yyyy年MM月"), isSelected = false });
                model.Month.Add(new KeyValueModel() { key = now.AddMonths(2).ToString("yyyyMM"), keyValue = now.AddMonths(2).ToString("yyyy年MM月"), isSelected = false });
                model.Brand = new List<KeyValueModel>();
                JS5_S12_BRAND_INFOTableAdapter ad = new JS5_S12_BRAND_INFOTableAdapter();
                var ents = ad.GetDataBy();

                foreach (var ent in ents)
                {
                    model.Brand.Add(new KeyValueModel() { key = ent.BRAND_CODE, keyValue = ent.BRAND_NAME, isSelected = false });
                }
                model.Brand.First().isSelected = true;
                model.Prodect = new List<KeyValueModel>();
                var ad2 = new JS5_S12_PRODUCT_TYPE_INFOTableAdapter();
                var ents2 = ad2.GetDataBy();
                foreach (var ent in ents2)
                {
                    model.Prodect.Add(new KeyValueModel() { key = ent.ID, keyValue = ent.TREE_NODE_NAME, isSelected = false });
                }
            model.Prodect.First().isSelected = true;
            model.UserId = userId;
            model.type = type;
            model.salesCode = userInfo.SalesNo;
            if (type == 1)
            {
                model.OrgList = new List<KeyValueModel>();
                var orglist = salesApp.getEmpOrgBySalesNo(userInfo.SalesNo, "BUManager");
                foreach (var org in orglist)
                {
                    var orginfo = salesApp.getOrgInfo(org.ORG_ID);
                    model.OrgList.Add(new KeyValueModel() { key = orginfo.id, keyValue = orginfo.MANAGE_ORG_NAME });
                }
                model.OrgList.First().isSelected = true;
                ViewData["VModel"] = getdata(model.Brand.First().key, model.Prodect.First().key, now.ToString("yyyyMM"), userId, type, userInfo.SalesNo, model.OrgList.First().key);
            }
            else if (type == 2)
            {
                model.CityList = new List<KeyValueModel>();
                var orglist = salesApp.getEmpOrgBySalesNo(userInfo.SalesNo, "Citymanager");
                foreach (var org in orglist)
                {
                    var orginfo = salesApp.getOrgInfo(org.ORG_ID);
                    model.CityList.Add(new KeyValueModel() { key = orginfo.id, keyValue = orginfo.MANAGE_ORG_NAME });
                }
                model.CityList.First().isSelected = true;
                ViewData["VModel"] = getdata(model.Brand.First().key, model.Prodect.First().key, now.ToString("yyyyMM"), userId, type, userInfo.SalesNo, model.CityList.First().key);
            }
              
               
               
                ViewData["PModel"] = model;
                //ViewData["ValueModel"] = model;
                return View("PSIView");
           
        }
    }
}