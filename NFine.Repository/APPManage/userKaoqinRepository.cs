using DataSynchronizationLib;
using DataSynchronizationLib.DataSetPopTableAdapters;
using NFine.Code;
using NFine.Data;
using NFine.Domain._02_ViewModel;
using NFine.Domain._03_Entity.APPManage;
using NFine.Domain._04_IRepository.APPManage;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NFine.Repository.APPManage
{
    public class userKaoqinRepository : RepositoryBase<userKaoqinEntity>, IuserKaoqinRepository
    {
        private const double EARTH_RADIUS = 6378137;
       
        /// <summary>

        /// 经纬度转化成弧度

        /// </summary>

        /// <param name="d"></param>

        /// <returns></returns>

        private static double Rad(double d)

        {

            return (double)d * Math.PI / 180d;

        }

        public static double GetDistance(double lat1, double lng1, double lat2, double lng2)
        {
            double radLat1 = Rad(lat1);

            double radLng1 = Rad(lng1);

            double radLat2 = Rad(lat2);

            double radLng2 = Rad(lng2);

            double a = radLat1 - radLat2;

            double b = radLng1 - radLng2;

            double result = 2 * Math.Asin(Math.Sqrt(Math.Pow(Math.Sin(a / 2), 2) + Math.Cos(radLat1) * Math.Cos(radLat2) * Math.Pow(Math.Sin(b / 2), 2))) * EARTH_RADIUS;

            return result;
        }
        public salesActualChangeRes Submit2(userKaoqinEntity itemsEntity)
        {
            //没有上班卡的情况不能刷下班卡
            if (itemsEntity.kaoqin_Type.Equals("2"))
            {
                var testRes = this.FindList("SELECT * FROM user_kaoqin where userId='" + itemsEntity.userId + "' and kaoqin_Type='1' and checkTime>='" + DateTime.Now.ToString("yyyy-MM-dd") + " 00:00:00" + "' and checkTime <='" + DateTime.Now.ToString("yyyy-MM-dd") + " 23:59:59" + "'");
                if (testRes.Count() <= 0)
                {
                    return new salesActualChangeRes() { errorCode = "0001", isOk = false, errorMessage = "打卡失败，未刷上班卡，不能刷下班卡！" };
                }
            }

            itemsEntity.id = System.Guid.NewGuid().ToString();
            this.Insert(itemsEntity);
            try
            {
                ImarketSalesRepository salesRepo = new marketSalesRepository();

                //var res = from x in dbcontext.Set<userKaoqinEntity>()
                //        join p in dbcontext.Set<aspnetusersEntity>() on new { id = x.userId } equals new { id = p.Id }
                //       join c in dbcontext.Set<marketSalesEntity>() on new { phone = p.PhoneNumber } equals new { phone = c.sales_PhoneNumber }
                //      where x.id.Equals(itemsEntity.id) && c.Active == 1
                //     select c;

                //if (res.Count() >= 1)
                //{
                var sales = salesRepo.GetUserInfo(itemsEntity.userId);
                
                String ShopCode = "";
                String SalesNo = "";
                SalesNo = sales.SalesNo;
                string KQ_MODE_CODE = "NormalKQ";
                if (sales.POP_TYPE_CODE.Equals("GuideMan"))
                {
                    ImarketSalesShopRepository shopRepository = new marketSalesShopRepository();
                    var Shop = shopRepository.getShopByUserId(itemsEntity.userId);
                    if (Shop.Count() <= 0)
                    {
                          itemsEntity.is_Sync = 0;
                         this.Update(itemsEntity);
                       
                        return new salesActualChangeRes() { errorCode = "0001", isOk = false, errorMessage = "打卡失败，门店关系错误，请联系管理员" };
                    }
                   
                    ShopCode = Shop.First().SHOP_CODE;
                    

                    //查找门店中的GPS位置
                    var res2 = from x in dbcontext.Set<marketSalesShopEntity>() where x.SHOP_CODE.Equals(ShopCode) && x.ACTIVE_FLAG == 1 select x;
                    double shop_LATITUDE = res2.First().LATITUDE.Value;
                    double shop_LONGITUDE = res2.First().LONGITUDE.Value;

                    double distance = userKaoqinRepository.GetDistance(shop_LATITUDE, shop_LONGITUDE, itemsEntity.LATITUDE.Value, itemsEntity.LONGITUDE.Value);
                    /**
                    if (!res.First().POP_TYPE_CODE.Equals("GuideMan"))
                    {
                        if ((itemsEntity.kaoqin_Type.Equals("1") || itemsEntity.kaoqin_Type.Equals("2")))
                        {
                            this.Delete(itemsEntity);
                            return new salesActualChangeRes() { errorCode = "0001", isOk = false, errorMessage = "业务员账号无法使用考勤打卡功能" };
                        }
                    }
                    **/
                   
                    if (itemsEntity.file_id == null || itemsEntity.file_id.Equals(""))
                    {
                            KQ_MODE_CODE = "SpecialKQ";
                       
                            if (itemsEntity.kaoqin_Type.Equals("1") || itemsEntity.kaoqin_Type.Equals("2"))
                            {
                            if (distance > 1000)
                            {
                                this.Delete(itemsEntity);
                                return new salesActualChangeRes() { errorCode = "0001", isOk = false, errorMessage = "打卡失败，距离门店距离超出1000米" };
                            }
                            }
                       
                    }
                    itemsEntity.DISTINCE = distance;
                    this.Update(itemsEntity);
                }
               

                    DateTime nowTime = System.DateTime.Now;
                    string CREATE_DATE = nowTime.ToString("yyyyMMddHHmmss");
                    string KaoQinType = "";
                    string ClockType = "";
                    if (itemsEntity.kaoqin_Type.Equals("1"))
                    {
                        
                            KaoQinType = "002";
                            ClockType = "ClockIn";
                       
                    }
                    else if (itemsEntity.kaoqin_Type.Equals("2"))
                    {
                        
                            KaoQinType = "002";
                            ClockType = "ClockOut";
                       
                       
                    }
                    else if (itemsEntity.kaoqin_Type.Equals("3"))
                    {
                        KaoQinType = "001";
                        ClockType = "ClockIn";
                    }
                    else if (itemsEntity.kaoqin_Type.Equals("4"))
                    {
                        KaoQinType = "001";
                        ClockType = "ClockOut";
                    }
                    else if (itemsEntity.kaoqin_Type.Equals("5"))
                    {
                        KaoQinType = "001";
                        ClockType = "ClockIn";
                    }
                    else if (itemsEntity.kaoqin_Type.Equals("6"))
                    {
                        KaoQinType = "001";
                        ClockType = "ClockOut";
                    }
                    else if (itemsEntity.kaoqin_Type.Equals("7"))
                    {
                        KaoQinType = "004";
                        ClockType = "ClockIn";
                    }
                    else if (itemsEntity.kaoqin_Type.Equals("8"))
                    {
                        KaoQinType = "004";
                        ClockType = "ClockOut";
                    }
                    else if (itemsEntity.kaoqin_Type.Equals("9"))
                    {
                        KaoQinType = "003";
                        ClockType = "ClockIn";
                    }
                    else if (itemsEntity.kaoqin_Type.Equals("10"))
                    {
                        KaoQinType = "003";
                        ClockType = "ClockOut";
                    }
                    else if (itemsEntity.kaoqin_Type.Equals("11"))
                    {
                        KaoQinType = "005";
                        ClockType = "ClockIn";
                    }
                    DataSynchronizationLib.DataSetPopTableAdapters.JS5_S12_SALES_KQ_DATA_UPLOADTableAdapter ad = new DataSynchronizationLib.DataSetPopTableAdapters.JS5_S12_SALES_KQ_DATA_UPLOADTableAdapter();
                    ad.InsertQuery(itemsEntity.id, 1, SalesNo, CREATE_DATE, CREATE_DATE, "PSIadmin_APP@" + CREATE_DATE + "@APP", "PSIadmin_APP@" + CREATE_DATE + "@APP", 1, "FLNET", ShopCode,
                        SalesNo, KaoQinType, ClockType, itemsEntity.checkTime.Value, itemsEntity.LONGITUDE.ToDecimal(), itemsEntity.LATITUDE.ToDecimal(), itemsEntity.DISTINCE.ToDecimal(), "", itemsEntity.id,1, "PSIadmin_APP", nowTime, nowTime, KQ_MODE_CODE,itemsEntity.adder);
                    DataSynchronizationLib.DataSetPopTableAdapters.JS5_S12_SALES_FILE_UPLOADTableAdapter fileAd = new JS5_S12_SALES_FILE_UPLOADTableAdapter();
                    if (itemsEntity.file_id != null && !itemsEntity.file_id.Equals(""))
                    {
                        string[] fileids = itemsEntity.file_id.Split(",".ToCharArray());
                        string physical_path = System.DateTime.Now.ToString("yyyyMM");

                        foreach (string fileid in fileids)
                        {
                            
                            fileAd.InsertQuery(System.Guid.NewGuid().ToString(), fileid + ".jpg", "PSIadmin_APP@" + CREATE_DATE + "@APP", System.DateTime.Now, "PSIadmin_APP@" + CREATE_DATE + "@APP", System.DateTime.Now, 1, "jpg", 0, "", fileid + ".jpg", physical_path, "", "P13014", itemsEntity.id, "PDG1354", "001", "打卡照片", itemsEntity.id,
                                "Normal", "正常", 1, "PSIadmin_APP", nowTime, nowTime);
                        }

                    }

                    QueriesTableAdapter tad = new QueriesTableAdapter();
                    String outMessage = "";

                    tad.SP_SALES_BATCH_KQ_UPLOAD_A("FLNET", itemsEntity.id, out outMessage);
                    if (!outMessage.Equals("OK"))
                    {
                        throw new Exception(outMessage);
                    }
                    itemsEntity.is_Sync = 1;
                    this.Update(itemsEntity);
                //}
                //else
                //{
                  //  itemsEntity.is_Sync = 0;
                   // this.Update(itemsEntity);
                    //this.Delete(itemsEntity);
                    //return new salesActualChangeRes() { errorCode = "0001", isOk = false, errorMessage = "打卡失败，门店关系错误，请联系管理员" };
                //}
            }
            catch (Exception ex)
            {
                this.Delete(itemsEntity);
                return new salesActualChangeRes() { errorCode = "0001", isOk = false, errorMessage = ex.Message };
            }
            return new salesActualChangeRes() { errorCode = "0001", isOk = true, errorMessage = "刷卡成功" };
        }
        public void Submit(userKaoqinEntity itemsEntity)
        {
            itemsEntity.id = Common.GuId();
            this.Insert(itemsEntity);

            



           
        }
    }
}
