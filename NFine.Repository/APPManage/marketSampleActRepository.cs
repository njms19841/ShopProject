using NFine.Data;
using NFine.Domain._03_Entity.APPManage;
using NFine.Domain._04_IRepository.APPManage;
using NFine.Repository.APPManage;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NFine.Domain._02_ViewModel;
using DataSynchronizationLib.DataSetPopTableAdapters;

namespace NFine.Repository.APPManage
{
    public class marketSampleActRepository : RepositoryBase<marketSampleActEntity>, ImarketSampleActRepository
    {
        public salesActualChangeRes DeleteEnt(string id)
        {
            var tempEnt = this.FindEntity(id);
            
            if (tempEnt.SAMPLE_TYPE_CODE != null && tempEnt.SAMPLE_TYPE_CODE.Equals("001"))
            {
                return new salesActualChangeRes() { isOk = false, errorCode = "0002", errorMessage = "删除失败，不能删除资产样机！" };
            }
            
            try
            {
                tempEnt.Modify_Time = System.DateTime.Now;
                tempEnt.isDeleted = 1;
                this.Update(tempEnt);
                SyncToPSI_delete(tempEnt);
            }
            catch (Exception e)
            {
                tempEnt.isDeleted = 0;
                this.Update(tempEnt);
                return new salesActualChangeRes() { errorCode = "0001", isOk = false, errorMessage = e.Message };

            }
            
            return new salesActualChangeRes() { isOk = true, errorCode = "0001", errorMessage = "删除成功" };
        }
        public salesActualChangeRes SaveEnt(marketSampleActEntity ent,string SalesNo)
        {
            if (ent.id != null && !ent.id.Equals(""))
            {
                try
                {
                   var tempEnt= this.FindEntity(ent.id);
                    if (ent.SN_NO != null)
                    {
                        //判断SN是否有改过
                        if (tempEnt.SN_NO == null || !tempEnt.SN_NO.Equals(ent.SN_NO))
                        {
                            var sncount = this.IQueryable().Count(p => p.SN_NO.Equals(ent.SN_NO) && p.isDeleted != 1);
                            if (sncount > 0)
                            {
                                return new salesActualChangeRes() { errorCode = "0001", isOk = false, errorMessage = "上报失败,S/N编号重复" };
                            }
                        }
                    }
                    

                    tempEnt.MACHINE_MODEL_NO = ent.MACHINE_MODEL_NO;
                    tempEnt.PRODUCT_TYPE_CODE = ent.PRODUCT_TYPE_CODE;
                    tempEnt.PRODUCT_TYPE_NAME = ent.PRODUCT_TYPE_NAME;
                    tempEnt.sales_No = SalesNo;
                    tempEnt.SAMPLE_UP_NO = ent.SAMPLE_UP_NO;
                    tempEnt.SHOP_CODE = ent.SHOP_CODE;
                    tempEnt.SHOP_NAME = ent.SHOP_NAME;
                    tempEnt.SN_NO = ent.SN_NO;
                    tempEnt.file_ids = ent.file_ids;
                    tempEnt.Modify_Time = System.DateTime.Now;
                    tempEnt.isSync = 0;
                    tempEnt.UP_TYPE_CODE = ent.UP_TYPE_CODE;
                    tempEnt.UP_TYPE_NAME = ent.UP_TYPE_NAME;
                    tempEnt.SAMPLE_TYPE_CODE = ent.SAMPLE_TYPE_CODE;
                    tempEnt.SAMPLE_TYPE_NAME = ent.SAMPLE_TYPE_NAME;
                    tempEnt.SOURCE_SAMPLE_UP_NO = ent.SAMPLE_UP_NO;
                    tempEnt.SAMPLE_DATE = ent.SAMPLE_DATE;
                    if (ent.PRODUCT_STATUS_CODE == null)
                    {
                        ent.PRODUCT_STATUS_CODE = tempEnt.PRODUCT_STATUS_CODE;
                    }
                    else
                    {
                        tempEnt.PRODUCT_STATUS_CODE = ent.PRODUCT_STATUS_CODE;
                    }
                    
                    tempEnt.REMARK = ent.REMARK;
                    

                    this.Update(tempEnt);
                    try
                    {
                        SyncToPSI(tempEnt, true);
                        tempEnt.isSync = 1;
                        this.Update(tempEnt);
                    }
                    catch (Exception e)
                    {
                        tempEnt.isSync = 0;
                        this.Update(tempEnt);
                        return new salesActualChangeRes() { errorCode = "0001", isOk = false, errorMessage = e.Message };

                    }
                }
                catch (Exception e)
                {
                    return new salesActualChangeRes() { errorCode = "0001", isOk = false, errorMessage = e.Message };
                }
                return new salesActualChangeRes() { isOk = true, errorCode = "0001", errorMessage = "上报成功" };
            }
            else
            {
                if (ent.SN_NO != null)
                {
                   
                        var sncount = this.IQueryable().Count(p => p.SN_NO.Equals(ent.SN_NO) && p.isDeleted != 1);
                        if (sncount > 0)
                        {
                            return new salesActualChangeRes() { errorCode = "0001", isOk = false, errorMessage = "上报失败,S/N编号重复" };
                        }
                   
                }
                ent.id = System.Guid.NewGuid().ToString();
                ent.Created_Time = System.DateTime.Now;
                ent.PRODUCT_STATUS_CODE = "001";
                ent.Modify_Time= System.DateTime.Now;
                ent.isSync = 0;

                var lastEnt = this.IQueryable().OrderByDescending(p => p.Created_Time).FirstOrDefault();
                ent.SAMPLE_UP_NO = "SUA" + ent.Created_Time.Value.ToString("yyyyMMddHHmmssffff");

                /*if (lastEnt != null && lastEnt.Created_Time.Value.Year.Equals(ent.Created_Time.Value.Year) && lastEnt.Created_Time.Value.Month.Equals(ent.Created_Time.Value.Month))
                {
                    if (lastEnt.SAMPLE_UP_NO != null && !lastEnt.SAMPLE_UP_NO.Equals(""))
                    {
                        int no = int.Parse(lastEnt.SAMPLE_UP_NO.Substring(9));
                        no = no + 1;
                        ent.SAMPLE_UP_NO = "SUA" + ent.Created_Time.Value.ToString("yyyyMM") + no.ToString("00000");
                    }
                }
                */
                this.Insert(ent);
                try
                {
                    SyncToPSI(ent, false);
                    ent.isSync = 1;
                    this.Update(ent);
                }
                catch (Exception e)
                {
                    ent.isDeleted = 1;
                    this.Update(ent);
                    return new salesActualChangeRes() { errorCode = "0001", isOk = false, errorMessage = e.Message };

                }
                return new salesActualChangeRes() { isOk = true, errorCode = "0001", errorMessage = "上报成功" };
            }
            
        }
        private void SyncToPSI_delete(marketSampleActEntity ent)
        {
            var res2 = from x in dbcontext.Set<marketSalesShopEntity>() where x.SHOP_CODE.Equals(ent.SHOP_CODE) select x;
            DateTime date = System.DateTime.Now;
            string CREATE_DATE = date.ToString("yyyyMMddHHmmss");
            DataSynchronizationLib.DataSetPopTableAdapters.JS5_S12_SALES_FILE_UPLOADTableAdapter fileAd = new JS5_S12_SALES_FILE_UPLOADTableAdapter();
            // if (!isChange)
            //{
            
            JS5_S12_SALES_SAMPLE_UPTableAdapter sampleAd = new JS5_S12_SALES_SAMPLE_UPTableAdapter();
            sampleAd.InsertQuery(ent.id, 1, ent.sales_No, CREATE_DATE, CREATE_DATE, "PSIadmin_APP@" + CREATE_DATE + "@APP", "PSIadmin_APP@" + CREATE_DATE + "@APP",0, "FLNET"
                , ent.SAMPLE_UP_NO, res2.First().CUSTOMER_CODE, res2.First().CUSTOMER_NAME, ent.SHOP_CODE, ent.SHOP_NAME, ent.PRODUCT_TYPE_CODE, ent.PRODUCT_TYPE_NAME, ent.MACHINE_MODEL_NO, ent.SN_NO, ent.SAMPLE_DATE.Value, "无", ent.id, 1, "PSIadmin_APP", date, date, ent.UP_TYPE_CODE, ent.UP_TYPE_NAME,
                ent.SAMPLE_TYPE_CODE, ent.SAMPLE_TYPE_NAME, ent.SOURCE_SAMPLE_UP_NO, ent.PRODUCT_STATUS_CODE, ent.REMARK
                );
       
            QueriesTableAdapter tad = new QueriesTableAdapter();
            String outMessage = "";

            tad.SP_SALES_SAMPLE_UPLOAD("FLNET", ent.id, out outMessage);
            if (!outMessage.Equals("OK"))
            {
                throw new Exception(outMessage);
            }
        }
        //}
    private void SyncToPSI(marketSampleActEntity ent,bool isChange)
        {
            var res2 = from x in dbcontext.Set<marketSalesShopEntity>() where x.SHOP_CODE.Equals(ent.SHOP_CODE) select x;
            DateTime date = System.DateTime.Now;
            string CREATE_DATE = date.ToString("yyyyMMddHHmmss");
            DataSynchronizationLib.DataSetPopTableAdapters.JS5_S12_SALES_FILE_UPLOADTableAdapter fileAd = new JS5_S12_SALES_FILE_UPLOADTableAdapter();
            // if (!isChange)
            //{
            
            JS5_S12_SALES_SAMPLE_UPTableAdapter sampleAd = new JS5_S12_SALES_SAMPLE_UPTableAdapter();
                sampleAd.InsertQuery(ent.id, 1, ent.sales_No, CREATE_DATE, CREATE_DATE, "PSIadmin_APP@" + CREATE_DATE + "@APP", "PSIadmin_APP@" + CREATE_DATE + "@APP", 1, "FLNET"
                    , ent.SAMPLE_UP_NO, res2.First().CUSTOMER_CODE, res2.First().CUSTOMER_NAME, ent.SHOP_CODE, ent.SHOP_NAME, ent.PRODUCT_TYPE_CODE, ent.PRODUCT_TYPE_NAME,ent.MACHINE_MODEL_NO, ent.SN_NO,ent.SAMPLE_DATE.Value, "无", ent.id, 1, "PSIadmin_APP", date, date,ent.UP_TYPE_CODE,ent.UP_TYPE_NAME,
                    ent.SAMPLE_TYPE_CODE,ent.SAMPLE_TYPE_NAME,ent.SOURCE_SAMPLE_UP_NO, ent.PRODUCT_STATUS_CODE,ent.REMARK
                    );
                if (ent.file_ids != null && !ent.file_ids.Equals(""))
                {
                    string[] fileids = ent.file_ids.Split(",".ToCharArray());
                    string physical_path = System.DateTime.Now.ToString("yyyyMM");

                    foreach (string fileid in fileids)
                    {
                        //写入发票信息
                        fileAd.InsertQuery(System.Guid.NewGuid().ToString(), fileid + ".jpg", ent.sales_No, System.DateTime.Now, ent.sales_No, System.DateTime.Now, 1, "jpg", 0, "", fileid + ".jpg", physical_path, "", "P13056", ent.id, "PDG1351", "001", "样品照片", ent.id,
                            "Normal", "正常", 1, "PSIadmin_APP", date, date);
                    }

                }
                QueriesTableAdapter tad = new QueriesTableAdapter();
                String outMessage = "";

                tad.SP_SALES_SAMPLE_UPLOAD ("FLNET", ent.id, out outMessage);
                if (!outMessage.Equals("OK"))
                {
                    throw new Exception(outMessage);
                }
            }
        //}
        }
}
