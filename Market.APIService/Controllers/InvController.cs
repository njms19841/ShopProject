using DataSynchronizationLib.SCMTableAdapters;
using DataSynchronizationStanbyLib.DataSetStanbyTableAdapters;
using Market.APIService.Models;
using NFine.Application.APPManage;
using NFine.Application.TaskManage;
using NFine.Domain._02_ViewModel;
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
    public class CusModel
    {
        public string CustomerCode { get; set; }
        public string CustomerName { get; set; }
    }
    public class InvModels
    {
        public string CustomerCode { get; set; }
        public string CustomerName { get; set; }
        public List<InvModel> data { get; set; }
    }
    public class InvModel
    {
        public string modelCode { get; set; }
        public string qty1 { get; set; }
        public string qty2 { get; set; }
    }
    public class InvController : Controller
    {
        public ActionResult InvIndex(string userId)
        {
            marketShopApp shopApp = new marketShopApp();
            List<marketSalesShopEntity> shops = new List<marketSalesShopEntity>();
            shops.AddRange(shopApp.getShopByUserId(userId));
            marketSalesApp userapp = new marketSalesApp();
            UserInfoResultModel userinfo = userapp.GetUserInfo(userId);
            shops.AddRange(shopApp.getAllShopByEmpCode(userinfo.SalesNo));

            var t = from e in shops
                    group e by new { Code = e.CUSTOMER_CODE, Name = e.CUSTOMER_NAME } into g
                    select new CusModel { CustomerCode = g.Key.Code,  CustomerName = g.Key.Name };

            /*
            marketShopApp shopApp = new marketShopApp();
            List<marketSalesShopEntity> shops = shopApp.getShopByUserId(userId);
            List<CusModel> vModel = new List<CusModel>();
            foreach (var shop in shops)
            {
                if (vModel.Find(p => p.CustomerCode.Equals(shop.CUSTOMER_CODE)) == null)
                {
                    vModel.Add(new CusModel() { CustomerCode = shop.CUSTOMER_CODE, CustomerName = shop.CUSTOMER_NAME });
                }
            }*/
            ViewData["vModel"] = t.OrderBy(p => p.CustomerName).ToList(); ;
            ViewData["userId"] = userId;
            return View("InvQinView");
        }
        public PartialViewResult InvView(string customerCode)
        {
            V_SALES_CUSINV_APP_QUERYTableAdapter ad = new V_SALES_CUSINV_APP_QUERYTableAdapter();
            InvModels models = new InvModels();
            models.data = new List<InvModel>();
            models.CustomerCode = customerCode;
            marketShopApp shopApp = new marketShopApp();
            models.CustomerName = shopApp.getCustomerName(customerCode);
           

            var ents = ad.GetDataBy(customerCode);
            foreach (var ent in ents)
            {
                models.data.Add( new InvModel() { modelCode=ent.PRODUCT_NO, qty1=(ent.ENDINGINVQTY==null?"无": ent.ENDINGINVQTY), qty2=ent.INVENTORY_QTY.ToString() });
            }
           
            return PartialView("_InvQinPartialPage", models);
        }
        public PartialViewResult CustomerView(string userId)
        {
            marketShopApp shopApp = new marketShopApp();
            List<marketSalesShopEntity> shops = new List<marketSalesShopEntity>();
            shops.AddRange(shopApp.getShopByUserId(userId));
            marketSalesApp userapp = new marketSalesApp();
            UserInfoResultModel userinfo = userapp.GetUserInfo(userId);
            shops.AddRange(shopApp.getAllShopByEmpCode(userinfo.SalesNo));

            var t = from e in shops
                    group e by new { Code = e.CUSTOMER_CODE, Name = e.CUSTOMER_NAME } into g
                    select new CusModel { CustomerCode = g.Key.Code, CustomerName = g.Key.Name };

            return PartialView("_InvQinCustomerPartialPage", t.OrderBy(p => p.CustomerName).ToList());
        }

    }
}