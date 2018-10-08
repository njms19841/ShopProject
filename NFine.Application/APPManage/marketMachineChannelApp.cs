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
    public class marketMachineChannelApp
    {
        ImarketMachineModelRepository repository = new marketMachineModelRepository();
        public List<marketMachineModelEntity> getMachineModelByShop(string Shop)
        {
            return repository.getMachineModelByShop(Shop);
        }
        public int getMachineModelCountByShop(string Shop)
        {
            return repository.getMachineModelCountByShop(Shop);
        }
    }
}
