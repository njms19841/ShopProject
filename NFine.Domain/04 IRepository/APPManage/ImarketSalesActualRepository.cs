using NFine.Data;
using NFine.Domain._02_ViewModel;
using NFine.Domain._03_Entity.APPManage;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NFine.Domain._04_IRepository.APPManage
{
   public  interface ImarketSalesActualRepository : IRepositoryBase<marketSalesActualEntity>
   {
        salesActualChangeRes SalesActual(marketSalesActualEntity ent);
        salesActualChangeRes SalesActualV2(marketSalesActualEntity ent, bool isAndroid);
        salesActualChangeRes SalesActualV3(marketSalesActualEntity ent, bool isAndroid);
        List<salesActualAllModel> GetSalesActual();

        List<salesActualAllModel> GetSalesActualByUserId(String UserId);
         salesActualChangeRes SuppUp(String id, String salesNo, String filesId);
    }

}
