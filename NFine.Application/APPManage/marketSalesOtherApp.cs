
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
    public class marketSalesOtherApp
    {
        private ImarketSalesOtherRepository service = new marketSalesOtherRepository();
        public salesActualChangeRes insertSales(marketSalesOtherEntity ent)
        {
            return service.insertSales(ent);
        }
        public List<marketSalesOtherEntity> getAct(string userId)
        {
            return service.IQueryable().Where(p => p.userid.Equals(userId)).OrderByDescending(p=> p.Created_Time ).ThenBy(p => p.BRAND_CODE).ThenBy(p => p.shopCode).ToList();
        }
        public int getActCount(string userId, string activeNo, string shopCode)
        {
            return service.IQueryable().Where(p => p.userid.Equals(userId) && p.ACT_NO.Equals(activeNo) && p.shopCode.Equals(shopCode)).Count();
            //return rep.IQueryable().Where(p => p.sales_No.Equals(salesNo) && p.SHOP_CODE.Equals(shopCode) && p.is_Delete != 1 && p.ACT_NO.Equals(activeNo) && p.Created_Time >= startTime && p.Created_Time <= endTime).Count();
        }

    }
}
