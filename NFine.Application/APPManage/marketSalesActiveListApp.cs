using NFine.Domain._02_ViewModel;
using NFine.Domain._03_Entity.APPManage;
using NFine.Domain._04_IRepository.APPManage;
using NFine.Repository.APPManage;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NFine.Application.APPManage
{
    public class marketSalesActiveListApp
    {
        private ImarketSalesActtiveListRepository activeRepostiory = new  marketSalesActtiveListRepository();

        private ImarketSalesActtiveListBuRepository activeBuRepostiory = new marketSalesActtiveListBuRepository();
        private ImarketSalesActtiveListShopRepository ActtiveShopRepostiory = new marketSalesActtiveListShopRepository();
        private ImarketSalesShopV2Repository ShopRepostiory = new marketSalesShopV2Repository();


        public List<ActiveModel2> getList(String salesNo,DateTime day)
        {
            

            List<ActiveModel2> models = new List<ActiveModel2>();

            string activeStartDate = day.ToString("yyyy-MM-dd ") + " 00:00:00";
            string activeEndDate = day.AddDays(30).ToString("yyyy-MM-dd ") + " 23:59:59";

            string sql = "select * from market_sales_activelist where (T_TYPEID in (  select  T_TYPEID from market_sales_shopv2 where employee_code = '" + salesNo + "') or T_TYPEID='ALL') and ((ACT_START_DATE<='" + activeEndDate + "' and ACT_START_DATE>='" + activeStartDate + "') or (ACT_END_DATE<='" + activeEndDate + "' and ACT_END_DATE>='" + activeStartDate + "')) ";

            //string activeStartDate = day.ToString("yyyy-MM-dd ") + " 00:00:00";
            //string activeEndDate = day.ToString("yyyy-MM-dd ") + " 23:59:59";

            //string sql = "select * from market_sales_activelist where T_TYPEID in (  select  T_TYPEID from market_sales_shopv2 where employee_code = '" + salesNo + "') and (ACT_START_DATE<='" + activeStartDate + "' and ACT_END_DATE>='" + activeStartDate + "')";
            //string sql = "select * from market_sales_activelist where T_TYPEID in (  select  T_TYPEID from market_sales_shopv2 where employee_code = '" + salesNo + "') and CUSTOMER_CODE in (    select  CUSTOMER_CODE from market_sales_shopv2 where   employee_code = '" + salesNo + "')and ((ACT_START_DATE<='" + activeEndDate + "' and ACT_START_DATE>='" + activeStartDate + "') or (ACT_END_DATE<='" + activeEndDate + "' and ACT_END_DATE>='" + activeStartDate + "')) ";
            var tempEnts = activeRepostiory.FindList(sql);
            //判断是否有门店的权限
            //判断是否有门店的权限
            foreach (var ent in tempEnts)
            {

                

                var activeShop = ActtiveShopRepostiory.IQueryable().Where(p => p.ACT_ID.Equals(ent.id)).ToList();
                if (activeShop.Count > 0)
                {
                    var Shops2 = ShopRepostiory.IQueryable().Where(p => p.ACTIVE_FLAG == 1 && p.EMPLOYEE_CODE.Equals(salesNo)).ToList();
                    ActiveModel2 model = new ActiveModel2()
                    {
                        ACT_NO = ent.ACT_NO,
                        ACT_END_DATE = ent.ACT_END_DATE.Value,
                        ACT_NAME = ent.ACT_NAME,
                        ACT_START_DATE = ent.ACT_START_DATE.Value,
                        ACT_TYPE_CODE = ent.ACT_TYPE_CODE,
                        ACT_TYPE_NAME = ent.ACT_TYPE_NAME,
                        CUSTOMER_CODE = ent.CUSTOMER_CODE,
                        CUSTOMER_NAME = ent.CUSTOMER_NAME,
                        T_TYPEID = ent.T_TYPEID,
                        T_TYPENAME = ent.T_TYPENAME
                    };
                    model.Shops = new List<UserShopInfoModel2>();
                    foreach (var shop in Shops2)
                    {
                        // if (activeShop.Find(p => p.SHOP_CODE.Equals(shop.SHOP_CODE)) != null)
                        //{
                        if (activeShop.Find(p => p.SHOP_CODE.Equals(shop.SHOP_CODE)) != null)
                        {
                            model.Shops.Add(new UserShopInfoModel2() { ShopCode = shop.SHOP_CODE, ShopName = shop.SHOP_NAME });
                        }
                        //}
                    }
                    if (model.Shops.Count > 0)
                    {
                        models.Add(model);
                    }
                }
                else
                {

                    ActiveModel2 model = new ActiveModel2()
                    {
                        ACT_NO = ent.ACT_NO,
                        ACT_END_DATE = ent.ACT_END_DATE.Value,
                        ACT_NAME = ent.ACT_NAME,
                        ACT_START_DATE = ent.ACT_START_DATE.Value,
                        ACT_TYPE_CODE = ent.ACT_TYPE_CODE,
                        ACT_TYPE_NAME = ent.ACT_TYPE_NAME,
                        CUSTOMER_CODE = ent.CUSTOMER_CODE,
                        CUSTOMER_NAME = ent.CUSTOMER_NAME,
                        T_TYPEID = ent.T_TYPEID,
                        T_TYPENAME = ent.T_TYPENAME
                    };
                    model.Shops = new List<UserShopInfoModel2>();

                    var bulist = activeBuRepostiory.IQueryable().Where(p => p.ACT_ID.Equals(ent.id));
                    if (bulist.Count() > 0)
                    {
                        foreach (var bu in bulist)
                        {
                            if (ent.T_TYPEID.Equals("ALL"))
                            {
                                if (bu.T_BUID.Equals("OT"))
                                {
                                    var Shops = ShopRepostiory.IQueryable().Where(p => p.ACTIVE_FLAG == 1 && p.EMPLOYEE_CODE.Equals(salesNo)).ToList();
                                    foreach (var shop in Shops)
                                    {
                                        if (model.Shops.Find(p => p.ShopCode.Equals(shop.SHOP_CODE)) == null)
                                        {
                                            model.Shops.Add(new UserShopInfoModel2() { ShopCode = shop.SHOP_CODE, ShopName = shop.SHOP_NAME });
                                        }
                                    }
                                }
                                else
                                {
                                    var Shops = ShopRepostiory.IQueryable().Where(p => p.ACTIVE_FLAG == 1 && p.EMPLOYEE_CODE.Equals(salesNo)).ToList();
                                    foreach (var shop in Shops)
                                    {
                                        if (model.Shops.Find(p => p.ShopCode.Equals(shop.SHOP_CODE)) == null)
                                        {
                                            model.Shops.Add(new UserShopInfoModel2() { ShopCode = shop.SHOP_CODE, ShopName = shop.SHOP_NAME });
                                        }
                                    }
                                }
                            }
                            else
                            {
                                if (bu.T_BUID.Equals("OT"))
                                {
                                    var Shops = ShopRepostiory.IQueryable().Where(p => p.ACTIVE_FLAG == 1 && p.EMPLOYEE_CODE.Equals(salesNo) && p.T_TYPEID.Equals(ent.T_TYPEID)).ToList();
                                    foreach (var shop in Shops)
                                    {
                                        if (model.Shops.Find(p => p.ShopCode.Equals(shop.SHOP_CODE)) == null)
                                        {
                                            model.Shops.Add(new UserShopInfoModel2() { ShopCode = shop.SHOP_CODE, ShopName = shop.SHOP_NAME });
                                        }
                                    }
                                }
                                else
                                {
                                    var Shops = ShopRepostiory.IQueryable().Where(p => p.ACTIVE_FLAG == 1 && p.EMPLOYEE_CODE.Equals(salesNo) && p.T_TYPEID.Equals(ent.T_TYPEID) && p.T_BUID.Equals(bu.T_BUID)).ToList();
                                    foreach (var shop in Shops)
                                    {
                                        if (model.Shops.Find(p => p.ShopCode.Equals(shop.SHOP_CODE)) == null)
                                        {
                                            model.Shops.Add(new UserShopInfoModel2() { ShopCode = shop.SHOP_CODE, ShopName = shop.SHOP_NAME });
                                        }
                                    }
                                }
                            }
                        }
                        if (model.Shops.Count > 0)
                        {
                            models.Add(model);
                        }
                    }
                    else
                    {
                        var Shops = ShopRepostiory.IQueryable().Where(p => p.ACTIVE_FLAG == 1 && p.EMPLOYEE_CODE.Equals(salesNo) && p.T_TYPEID.Equals(ent.T_TYPEID)).ToList();
                        foreach (var shop in Shops)
                        {
                            model.Shops.Add(new UserShopInfoModel2() { ShopCode = shop.SHOP_CODE, ShopName = shop.SHOP_NAME });
                        }
                        if (model.Shops.Count > 0)
                        {
                            models.Add(model);
                        }
                    }
                }



            }
            return models;
        }

        public List<ActiveModel2> getList(String salesNo)
        {
           // var Shops = ShopRepostiory.IQueryable().Where(p => p.ACTIVE_FLAG == 1 && p.EMPLOYEE_CODE.Equals(salesNo)).ToList();
            
            List<ActiveModel2> models = new List<ActiveModel2>();

            string activeStartDate = System.DateTime.Now.ToString("yyyy-MM-dd ") + " 00:00:00";
            string activeEndDate = System.DateTime.Now.AddDays(30).ToString("yyyy-MM-dd ") + " 23:59:59";

            string sql = "select * from market_sales_activelist where (T_TYPEID in (  select  T_TYPEID from market_sales_shopv2 where employee_code = '"+ salesNo + "') or T_TYPEID='ALL') and ((ACT_START_DATE<='" + activeEndDate + "' and ACT_START_DATE>='" + activeStartDate + "') or (ACT_END_DATE<='" + activeEndDate + "' and ACT_END_DATE>='" + activeStartDate + "')) ";
            //string sql = "select * from market_sales_activelist where T_TYPEID in (  select  T_TYPEID from market_sales_shopv2 where employee_code = '" + salesNo + "') and CUSTOMER_CODE in (    select  CUSTOMER_CODE from market_sales_shopv2 where   employee_code = '" + salesNo + "')and ((ACT_START_DATE<='" + activeEndDate + "' and ACT_START_DATE>='" + activeStartDate + "') or (ACT_END_DATE<='" + activeEndDate + "' and ACT_END_DATE>='" + activeStartDate + "')) ";
            var tempEnts = activeRepostiory.FindList(sql);
            //判断是否有门店的权限
            foreach (var ent in tempEnts)
            {
                 
                    
                    var activeShop = ActtiveShopRepostiory.IQueryable().Where(p => p.ACT_ID.Equals(ent.id)).ToList();
                
                if (activeShop.Count > 0)
                    {
                    var Shops2 = ShopRepostiory.IQueryable().Where(p => p.ACTIVE_FLAG == 1 && p.EMPLOYEE_CODE.Equals(salesNo) ).ToList();
                    ActiveModel2 model = new ActiveModel2()
                        {
                            ACT_NO = ent.ACT_NO,
                            ACT_END_DATE = ent.ACT_END_DATE.Value,
                            ACT_NAME = ent.ACT_NAME,
                            ACT_START_DATE = ent.ACT_START_DATE.Value,
                            ACT_TYPE_CODE = ent.ACT_TYPE_CODE,
                            ACT_TYPE_NAME = ent.ACT_TYPE_NAME,
                            CUSTOMER_CODE = ent.CUSTOMER_CODE,
                            CUSTOMER_NAME = ent.CUSTOMER_NAME,
                            T_TYPEID = ent.T_TYPEID,
                            T_TYPENAME = ent.T_TYPENAME
                        };
                        model.Shops = new List<UserShopInfoModel2>();
                        foreach (var shop in Shops2)
                        {
                        if (activeShop.Find(p => p.SHOP_CODE.Equals(shop.SHOP_CODE)) != null)
                        {
                            model.Shops.Add(new UserShopInfoModel2() { ShopCode = shop.SHOP_CODE, ShopName = shop.SHOP_NAME });
                        }
                    }
                        if (model.Shops.Count > 0)
                        {
                            models.Add(model);
                        }
                    }
                    else
                    {

                        ActiveModel2 model = new ActiveModel2()
                        {
                            ACT_NO = ent.ACT_NO,
                            ACT_END_DATE = ent.ACT_END_DATE.Value,
                            ACT_NAME = ent.ACT_NAME,
                            ACT_START_DATE = ent.ACT_START_DATE.Value,
                            ACT_TYPE_CODE = ent.ACT_TYPE_CODE,
                            ACT_TYPE_NAME = ent.ACT_TYPE_NAME,
                            CUSTOMER_CODE = ent.CUSTOMER_CODE,
                            CUSTOMER_NAME = ent.CUSTOMER_NAME,
                            T_TYPEID = ent.T_TYPEID,
                            T_TYPENAME = ent.T_TYPENAME
                        };
                        model.Shops = new List<UserShopInfoModel2>();

                        var bulist = activeBuRepostiory.IQueryable().Where(p => p.ACT_ID.Equals(ent.id));
                        if (bulist.Count() > 0)
                        {
                                foreach (var bu in bulist)
                                {
                            if (ent.T_TYPEID.Equals("ALL"))
                            {
                                if (bu.T_BUID.Equals("OT"))
                                {
                                    var Shops = ShopRepostiory.IQueryable().Where(p => p.ACTIVE_FLAG == 1 && p.EMPLOYEE_CODE.Equals(salesNo)).ToList();
                                    foreach (var shop in Shops)
                                    {
                                        if (model.Shops.Find(p => p.ShopCode.Equals(shop.SHOP_CODE)) == null)
                                        {
                                            model.Shops.Add(new UserShopInfoModel2() { ShopCode = shop.SHOP_CODE, ShopName = shop.SHOP_NAME });
                                        }
                                    }
                                }
                                else
                                {
                                    var Shops = ShopRepostiory.IQueryable().Where(p => p.ACTIVE_FLAG == 1 && p.EMPLOYEE_CODE.Equals(salesNo) && p.T_BUID.Equals(bu.T_BUID)).ToList();
                                    foreach (var shop in Shops)
                                    {
                                        if (model.Shops.Find(p => p.ShopCode.Equals(shop.SHOP_CODE)) == null)
                                        {
                                            model.Shops.Add(new UserShopInfoModel2() { ShopCode = shop.SHOP_CODE, ShopName = shop.SHOP_NAME });
                                        }
                                    }
                                }
                            }
                            else
                            {
                                if (bu.T_BUID.Equals("OT"))
                                {
                                    var Shops = ShopRepostiory.IQueryable().Where(p => p.ACTIVE_FLAG == 1 && p.EMPLOYEE_CODE.Equals(salesNo) && p.T_TYPEID.Equals(ent.T_TYPEID)).ToList();
                                    foreach (var shop in Shops)
                                    {
                                        if (model.Shops.Find(p => p.ShopCode.Equals(shop.SHOP_CODE)) == null)
                                        {
                                            model.Shops.Add(new UserShopInfoModel2() { ShopCode = shop.SHOP_CODE, ShopName = shop.SHOP_NAME });
                                        }
                                    }
                                }
                                else
                                {
                                    var Shops = ShopRepostiory.IQueryable().Where(p => p.ACTIVE_FLAG == 1 && p.EMPLOYEE_CODE.Equals(salesNo) && p.T_TYPEID.Equals(ent.T_TYPEID) && p.T_BUID.Equals(bu.T_BUID)).ToList();
                                    foreach (var shop in Shops)
                                    {
                                        if (model.Shops.Find(p => p.ShopCode.Equals(shop.SHOP_CODE)) == null)
                                        {
                                            model.Shops.Add(new UserShopInfoModel2() { ShopCode = shop.SHOP_CODE, ShopName = shop.SHOP_NAME });
                                        }
                                    }
                                }
                            }
                        }
                      
                                  
                                if (model.Shops.Count > 0)
                                {
                                    models.Add(model);
                                }
                        }
                        else
                        {
                            var Shops = ShopRepostiory.IQueryable().Where(p => p.ACTIVE_FLAG == 1 && p.EMPLOYEE_CODE.Equals(salesNo) && p.T_TYPEID.Equals(ent.T_TYPEID)).ToList();
                            foreach (var shop in Shops)
                            {
                                model.Shops.Add(new UserShopInfoModel2() { ShopCode = shop.SHOP_CODE, ShopName = shop.SHOP_NAME });
                            }
                            if (model.Shops.Count > 0)
                            {
                                models.Add(model);
                            }
                        }
                    }
                    
               

            }
            return models;
        }
    }
}
