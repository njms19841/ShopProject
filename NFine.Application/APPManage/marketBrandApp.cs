
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
    public class marketBrandApp
    {
        private ImarketBrandRepository service = new marketBrandRepository();
        
        public List<marketBrandEntity> GetBrnadInfo()
        {
           
            return service.IQueryable() .OrderBy(p => p.BRAND_CODE).ToList();

        }
        public DataSynchronizationLib.DataSetPop.V_COMPETITOR_MACHINEDataTable GetBrnadOtherInfo()
        {
            DataSynchronizationLib.DataSetPopTableAdapters.V_COMPETITOR_MACHINETableAdapter ad = new DataSynchronizationLib.DataSetPopTableAdapters.V_COMPETITOR_MACHINETableAdapter();
            return ad.GetData();
        }
        public DataSynchronizationLib.DataSetPop.V_COMPETITOR_MACHINEDataTable GetBrnadOtherInfo(string Brnad,int tvsize)
        {
            DataSynchronizationLib.DataSetPopTableAdapters.V_COMPETITOR_MACHINETableAdapter ad = new DataSynchronizationLib.DataSetPopTableAdapters.V_COMPETITOR_MACHINETableAdapter();
            return ad.GetDataBy(Brnad,(decimal)tvsize);
        }
        public string GetOtherBrandName(string code)
        {
            DataSynchronizationLib.DataSetPopTableAdapters.V_COMPETITOR_MACHINETableAdapter ad = new DataSynchronizationLib.DataSetPopTableAdapters.V_COMPETITOR_MACHINETableAdapter();
            var list = ad.GetDataById(code);
            if (list != null && list.Count > 0)
            {
                return list.First().BRAND;
            }
            else
            {
                return "";
            }
        }
        public String GetBrandName(String code)
        {
            var list = service.IQueryable().Where(p => p.BRAND_CODE.Equals(code));
            if (list != null && list.Count() > 0)
            {
                return list.First().BRAND_NAME;
            }
            else
            {
                return "";
            }
              
        }
    }
}
