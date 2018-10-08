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
    
    public class marketTvSizeApp
    {
        ImarketTvsizeinfoRepository repository = new marketTvsizeinfoRepository();
        public List<marketTvsizeinfoEntity> getTvSize()
        {
           return repository.IQueryable().OrderBy(p=>p.TVSIZE).ToList();
        }
    }
}
