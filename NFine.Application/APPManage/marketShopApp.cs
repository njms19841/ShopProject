using NFine.Domain._03_Entity.APPManage;
using NFine.Domain._04_IRepository.APPManage;
using NFine.Domain._04_IRepository.TaskManage;
using NFine.Repository.APPManage;
using NFine.Repository.TaskManage;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NFine.Application.APPManage
{
    
    public class marketShopApp
    {
        private ImarketSalesRepository service = new marketSalesRepository();
        ImarketSalesShopRepository shopRepository = new marketSalesShopRepository();
        private IPsiSalesEmpOrgRepository empService = new PsiSalesEmpOrgRepository();
        public marketSalesShopEntity getShop(String shopCode)
        {



            return shopRepository.IQueryable().First(p => p.SHOP_CODE.Equals(shopCode));
        }
        
        public List<marketSalesShopEntity> getShopByUserId(String Userid)
        {



            return shopRepository.getShopByUserId(Userid);
        }
        public List<marketSalesShopEntity> getAllShopByEmpCode(String empCode)
        {
            
            List<marketSalesShopEntity> ents = new List<marketSalesShopEntity>();
            var orgs=  empService.IQueryable().Where(p => p.EMPLOYEE_CODE.Equals(empCode) && p.ACTIVE_FLAG == 1 && p.JOB_CODE.Equals("BUManager")).ToList();
            foreach (var org in orgs)
            {
                var sales = empService.FindList("SELECT * FROM psi_salesemporg where FIND_IN_SET(ORG_ID, getOrgList('"+ org.ORG_ID+ "')) and ACTIVE_FLAG=1");
                string empStr = "";
                var t = from e in sales
                        group e by new { Code = e.EMPLOYEE_CODE} into g
                        select new { Code = g.Key.Code};
              
                foreach (var sale in t)
                {
                    empStr = empStr + "," + sale.Code;
                }
                var Shops= shopRepository.FindList("SELECT * FROM market_sales_shop where SHOP_CODE in(SELECT sales_ShopNO FROM market_sales where  FIND_IN_SET(sales_No, '" + empStr + "') AND Active = 1)");
                ents.AddRange(Shops.ToList());

                //shopRepository.FindList("");
            }
            //return shopRepository.getShopByUserId(Userid);\
            return ents;
        }


        public string getCustomerName(string customerCode)
        {
            return shopRepository.IQueryable().First(p => p.CUSTOMER_CODE.Equals(customerCode)).CUSTOMER_NAME;
        }
        public string getShopName(string shopCode)
        {
            try
            {
                var list = shopRepository.IQueryable().Where(p => p.SHOP_CODE.Equals(shopCode));
                if (list.Count() > 0)
                {
                    return list.First().SHOP_NAME;
                }

                return "";
            }
            catch { return ""; }
        }

    }
}
