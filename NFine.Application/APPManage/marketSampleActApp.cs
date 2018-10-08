
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
    public class marketSampleActApp
    {
        private ImarketSampleActRepository service = new marketSampleActRepository();
        private ImarketSalesRepository salesRep = new marketSalesRepository();
        public salesActualChangeRes DeleteEnt(string id)
        {
            return service.DeleteEnt(id);
        }
        public salesActualChangeRes SaveEnt(marketSampleActEntity ent,string SalesNo)
        {
            return service.SaveEnt(ent, SalesNo);
        }
        public List<marketSampleActEntity> getEnts(string salesNo)
        {

            //string sales_ShopNo = salesRep.IQueryable().First(p => p.sales_No.Equals(salesNo) && p.Active == 1).sales_ShopNo;
            return service.FindList("select * from market_sample_act where SHOP_CODE in (select sales_ShopNo  from market_sales where sales_No = '"+ salesNo + "' and Active = 1) and (isDeleted =0 or isDeleted is null)");
        }
        public List<marketSampleActEntity> getEnts(string shopCode, string mac)
        {
            return service.FindList("select * from market_sample_act where SHOP_CODE ='"+ shopCode + "' and MACHINE_MODEL_NO='"+ mac + "' and Active = 1) and (isDeleted =0 or isDeleted is null) and SN_NO is not null");
     
        }
        
    }
}
