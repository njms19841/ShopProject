
using DataSynchronizationLib;
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
    public class marketProductTypeApp
    {
        private ImarketProductTypeRepository service = new marketProductTypeRepository();
        public List<marketProductTypeEntity> getTypes()
        {
            return service.IQueryable().ToList();
        }
        public DataSetPop.JS5_S12_PRODUCT_TYPE_INFORow getBigType(string sId, string TypeCode)
        {
           
            DataSynchronizationLib.DataSetPopTableAdapters.JS5_S12_PRODUCT_TYPE_INFOTableAdapter app = new DataSynchronizationLib.DataSetPopTableAdapters.JS5_S12_PRODUCT_TYPE_INFOTableAdapter();
           var list= app.GetDataById(sId);
            foreach (var item in list)
            {
                if (item.NODE_TYPE_CODE.Equals(TypeCode))
                {
                    return item;
                }
                else
                {
                    return getBigType(item.PARENT_ID, TypeCode);
                }
            }
            return null;
        }
        private List<DataSetPop.JS5_S12_PRODUCT_TYPE_INFORow> getTypes(string Pid, string TypeCode)
        {
            List<DataSetPop.JS5_S12_PRODUCT_TYPE_INFORow> list = new List<DataSetPop.JS5_S12_PRODUCT_TYPE_INFORow>();
            DataSynchronizationLib.DataSetPopTableAdapters.JS5_S12_PRODUCT_TYPE_INFOTableAdapter app = new DataSynchronizationLib.DataSetPopTableAdapters.JS5_S12_PRODUCT_TYPE_INFOTableAdapter();
            var types = app.GetSamllDataByBigId(Pid);
            foreach (var type in types)
            {
                if (type.NODE_TYPE_CODE.Equals(TypeCode))
                {
                    list.Add(type);
                }
                else
                {
                    list.AddRange(getTypes(type.ID, TypeCode));
                }
            }
            return list;
        }
        public List<DataSetPop.JS5_S12_PRODUCT_TYPE_INFORow> getSmailTypes(string Pid)
        {
            return getTypes(Pid, "SmallType");
        }
    }
}
