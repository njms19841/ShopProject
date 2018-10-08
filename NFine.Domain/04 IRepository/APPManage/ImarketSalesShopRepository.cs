using NFine.Data;
using NFine.Domain._03_Entity.APPManage;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NFine.Domain._04_IRepository.APPManage
{
    public interface ImarketSalesShopRepository : IRepositoryBase<marketSalesShopEntity>
    {
         List<marketSalesShopEntity> getShopByUserId(String Userid);
        List<marketSalesShopEntity> getShopBySalesNo(string SalesNo);
    }
}
