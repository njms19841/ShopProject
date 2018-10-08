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
    public class marketSalesShopV2Repository : RepositoryBase<marketSalesShopV2Entity>, ImarketSalesShopV2Repository
    {
        public List<marketSalesShopV2Entity> getShopByUserId(string Userid)
        {
            throw new NotImplementedException();
        }
    }
}
