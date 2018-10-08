using NFine.Data;
using NFine.Domain._03_Entity.APPManage;
using NFine.Domain._04_IRepository.APPManage;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NFine.Domain._02_ViewModel;

namespace NFine.Repository.APPManage
{
    public class marketMachineModelRepository : RepositoryBase<marketMachineModelEntity>, ImarketMachineModelRepository
    {
        public List<marketMachineModelEntity> getMachineModelByShop(string Shop)
        {
           /*
            var t = from x in dbcontext.Set<marketMachineModelEntity>()
                    join p in dbcontext.Set<marketChannelMachineEntity>() on new { ModelNo = x.MACHINE_MODEL_NO } equals new { ModelNo = p.MACHINE_MODEL_NO }
                    join c in dbcontext.Set<marketSalesShopEntity>() on new { ttypeid = p.T_TYPEID } equals new { ttypeid = c.T_TYPEID }
                    where c.SHOP_CODE.Equals(Shop) && x.isActive==1 && p.ACTIVE_FLAG==1 && p.ALLOW_FLAG==1
                    select x
                   ;
            return t.ToList();
*/
           // var t = from x in dbcontext.Set<marketMachineModelEntity>()
                //    where  x.isActive == 1 
                  //  select x
                  //;
           return dbcontext.Set<marketMachineModelEntity>().Where(p=>p.isActive==1) .ToList();
        }
        public int getMachineModelCountByShop(string Shop)
        {
            var t = from x in dbcontext.Set<marketMachineModelEntity>()
                    join p in dbcontext.Set<marketChannelMachineEntity>() on new { ModelNo = x.MACHINE_MODEL_NO } equals new { ModelNo = p.MACHINE_MODEL_NO }
                    join c in dbcontext.Set<marketSalesShopEntity>() on new { ttypeid = p.T_TYPEID } equals new { ttypeid = c.T_TYPEID }
                    where c.SHOP_CODE.Equals(Shop) && x.isActive == 1 && p.ACTIVE_FLAG == 1 && p.ALLOW_FLAG == 1
                    select x
                   ;
            return t.Count();
        }


    }
}
