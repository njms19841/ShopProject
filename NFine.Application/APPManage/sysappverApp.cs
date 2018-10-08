using NFine.Domain._03_Entity.APPManage;
using NFine.Repository.APPManage;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NFine.Application.APPManage
{
    public class sysappverApp
    {
        sysappverRepository repository = new sysappverRepository();
        public sysappverEntity getVer()
        {
            return repository.IQueryable().First();
        }
    }
}
