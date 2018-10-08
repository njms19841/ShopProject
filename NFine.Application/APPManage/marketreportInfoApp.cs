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
    public class marketreportInfoApp
    {
        ImarketreportInfoRepository reportRepository = new marketreportInfoRepository();
        ImarketFilesRepository fileRepository = new marketFilesRepository();
        public void InsertFile(marketFilesEntity ent)
        {
            fileRepository.Insert(ent);
        }
        public void report(marketreportInfoEntity ent)
        {
            reportRepository.Insert(ent);
        }

    }
}
