using NFine.Data;
using NFine.Domain._03_Entity.APPManage;
using NFine.Domain._04_IRepository.APPManage;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NFine.Repository.APPManage
{
    public class marketSalesShopRepository : RepositoryBase<marketSalesShopEntity>, ImarketSalesShopRepository
    {
        public List<marketSalesShopEntity> getShopByUserId(string Userid)
        {
            var t = from x in dbcontext.Set<marketSalesShopEntity>()
                    join p in dbcontext.Set<marketSalesEntity>() on new {ShopCode=x.SHOP_CODE } equals new { ShopCode=p.sales_ShopNo }
                    join c in dbcontext.Set<aspnetusersEntity>() on new { PhoneNumber = p.sales_No } equals new { PhoneNumber = c.EMPLOYEE_CODE }
                    where c.Id.Equals(Userid) && x.ACTIVE_FLAG==1 && p.Active==1 && c.active==1 select x 
                    ;
            return t.ToList();
        }
        public List<marketSalesShopEntity> getShopBySalesNo(string SalesNo)
        {
            var t = from x in dbcontext.Set<marketSalesShopEntity>()
                    join p in dbcontext.Set<marketSalesEntity>() on new { ShopCode = x.SHOP_CODE } equals new { ShopCode = p.sales_ShopNo }
                    where p.sales_No.Equals(SalesNo) && x.ACTIVE_FLAG == 1 && p.Active == 1
                    select x
                    ;
            
            return t.ToList();
        }
    }
}
