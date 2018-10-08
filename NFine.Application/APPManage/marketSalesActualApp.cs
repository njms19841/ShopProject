using DataSynchronizationStanbyLib;
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
    public class marketSalesActualApp
    {
        ImarketSalesActualRepository repository = new marketSalesActualRepository();
        ImarketMachineModelRepository ModelRepository = new marketMachineModelRepository();
        public salesActualChangeRes SuppUp(String id, String salesNo, String filesId)
        {
            return repository.SuppUp(id, salesNo, filesId);
        }
        public salesActualChangeRes SalesActual(marketSalesActualEntity ent)
        {
            return repository.SalesActual(ent);
        }
        public salesActualChangeRes SalesActualV2(marketSalesActualEntity ent,bool isAndroid )
        {
            return repository.SalesActualV2(ent, isAndroid);
        }
        public salesActualChangeRes SalesActualV3(marketSalesActualEntity ent, bool isAndroid)
        {
           /// return new salesActualChangeRes() { errorCode = "111", isOk = true };
            return repository.SalesActualV3(ent, isAndroid);
        }
        public List<salesActualAllModel> GetSalesActualByUserId(string UserId)
        {
            return repository.GetSalesActualByUserId(UserId);
        }
        public int GetQty(DateTime startDate, DateTime endDate)
        {
            DataSynchronizationStanbyLib.DataSetStanbyTableAdapters.HomeQueriesTableAdapter ad = new DataSynchronizationStanbyLib.DataSetStanbyTableAdapters.HomeQueriesTableAdapter();
            Nullable<Decimal> qty = ad.TotalQtyQuery(startDate, endDate);
            if (qty.HasValue)
            {
                return (int)qty.Value;
            }
            return 0;
        }
        public int GetQty(DateTime startDate, DateTime endDate,string salesNO)
        {
            DataSynchronizationStanbyLib.DataSetStanbyTableAdapters.HomeQueriesTableAdapter ad = new DataSynchronizationStanbyLib.DataSetStanbyTableAdapters.HomeQueriesTableAdapter();
           
            Nullable<Decimal> qty = ad.QtyQuery(startDate, endDate, salesNO);
            if (qty.HasValue)
            {
                return (int)qty.Value;
            }
            return 0;
        }
        public double GetAmount(DateTime startDate, DateTime endDate)
        {
            DataSynchronizationStanbyLib.DataSetStanbyTableAdapters.HomeQueriesTableAdapter ad = new DataSynchronizationStanbyLib.DataSetStanbyTableAdapters.HomeQueriesTableAdapter();
            //return (int)ad.TotalAmountQuery(startDate,endDate);

            Nullable<Decimal> qty = ad.TotalAmountQuery(startDate, endDate);
            if (qty.HasValue)
            {
                return (double)qty.Value;
            }
            return 0;
        }
        public int GetCT(DateTime startDate, DateTime endDate, string salesNO)
        {
            DataSynchronizationStanbyLib.DataSetStanbyTableAdapters.HomeQueriesTableAdapter ad = new DataSynchronizationStanbyLib.DataSetStanbyTableAdapters.HomeQueriesTableAdapter();
           // return (int)ad.CtQuery(startDate, endDate, salesNO);
            Nullable<Decimal> qty = ad.CtQuery(startDate, endDate, salesNO);
            if (qty.HasValue)
            {
                return (int)qty.Value;
            }
            return 0;
        }
        public DataSetStanby.TF_FILEINFODataTable getImage(string f_id)
        {
            DataSynchronizationStanbyLib.DataSetStanbyTableAdapters.TF_FILEINFOTableAdapter ad = new DataSynchronizationStanbyLib.DataSetStanbyTableAdapters.TF_FILEINFOTableAdapter();
           return ad.GetDataBy(f_id);
        }
        public DataSetStanby.V_SALES_TRANS_APP_QUERYDataTable GetSalesActualList(DateTime startDate,DateTime endDate,string GOODS_TYPE_CODE,string salesNO)
        {
            DataSynchronizationStanbyLib.DataSetStanbyTableAdapters.V_SALES_TRANS_APP_QUERYTableAdapter ad = new DataSynchronizationStanbyLib.DataSetStanbyTableAdapters.V_SALES_TRANS_APP_QUERYTableAdapter();
            return ad.GetDataBy(startDate, endDate, GOODS_TYPE_CODE, salesNO);
        }
        ImarketSalesShopRepository shopRepository = new marketSalesShopRepository();
        public DataSetStanby.V_SALES_TRANS_APP_QUERYDataTable GetSalesActualList(DateTime startDate, DateTime endDate, string GOODS_TYPE_CODE, string salesNO,string queryType2)
        {
            DataSetStanby.V_SALES_TRANS_APP_QUERYDataTable table = new DataSetStanby.V_SALES_TRANS_APP_QUERYDataTable();
            DataSynchronizationStanbyLib.DataSetStanbyTableAdapters.V_SALES_TRANS_APP_QUERYTableAdapter ad = new DataSynchronizationStanbyLib.DataSetStanbyTableAdapters.V_SALES_TRANS_APP_QUERYTableAdapter();
            if (queryType2.Equals("1"))
            {
                table= ad.GetDataBy(startDate, endDate, GOODS_TYPE_CODE, salesNO);
            }
            else if (queryType2.Equals("2"))
            {
                var shopList =  shopRepository.getShopBySalesNo(salesNO) ;
                foreach (var shop in shopList)
                {
                    var tempTable = ad.GetDataByShop(startDate, endDate, GOODS_TYPE_CODE, shop.SHOP_CODE);
                    foreach (DataSetStanby.V_SALES_TRANS_APP_QUERYRow row in tempTable)
                    {
                        if (table.Where(p => p.ID.Equals(row.ID)).Count() <= 0)
                        {
                            table.AddV_SALES_TRANS_APP_QUERYRow(row.ID, row.TRANS_NO, row.EMPLOYEE_ID, row.CH_REPORT_NAME, row.JOB_CODE, row.SALES_DATE, row.MACHINE_MODEL_NO, row.SALES_QTY,
                                row.SALES_PRICE, row.SALES_AMOUNT, row.T_TYPEID, row.T_TYPENAME, row.T_BUID, row.T_BUNAME, row.SHOP_CODE, row.SHOP_NAME, row.CUSTOMER_CODE,
                                row.CUSTOMER_NAME, row.GOODS_TYPE_CODE, row.GOODS_TYPE_NAME, row.TRANS_TYPE_CODE, row.TRANS_TYPE_NAME, row.CREATOR_ID,
                                row.CREATE_DATE, row.INVOICE_FLAG, row.CONSUMER_NAME, row.CONSUMER_PHONE_NO, row.INVOICE_STATUS_NAME, row.UNQUALIFIED_REASON_NAME, row.INVOICE_STATUS_CODE, row.UNQUALIFIED_REASON_CODE,row.BOARD_FLAG,row.BRAND_CODE,row.TVSIZE,row.T_TVSIZEID,row.ACT_NO,row.ACT_NAME
                                ,row.SEX,row.SEX_NAME,row.AREA_ID,row.ADDR,row.AGE_PERIOD_ID,row.AGE_PERIOD_NAME,row.CH_NO,row.SN_NO,row.BRAND_NAME,row.LOCK_TYPE_CODE,row.LOCK_TYPE_NAME);
                        }
                    }
                }
            }
            
            return table;
        }

        public  DataSynchronizationLib.DataSetPop.V_SALES_ORDER_APP_QUERYDataTable GetOrderList(String customer, decimal status)
        {
            DataSynchronizationLib.DataSetPopTableAdapters.V_SALES_ORDER_APP_QUERYTableAdapter ad = new DataSynchronizationLib.DataSetPopTableAdapters.V_SALES_ORDER_APP_QUERYTableAdapter();
            return ad.GetDataBy(customer, status);
        }

        public marketMachineModelEntity GetMachineModel(String ModelNo)
        {
           var list=  ModelRepository.IQueryable().Where(p => p.MACHINE_MODEL_NO.Equals(ModelNo));
            if (list.Count() > 0)
            {
                return list.ToList()[0];
            }
            return null;
        }
    }
}
