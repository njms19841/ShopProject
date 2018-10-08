using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NFine.Domain._02_ViewModel
{
    public class ActiveModel2
    {
        /// <summary>
        /// 活动编号
        /// </summary>
        public string ACT_NO { get; set; }
        /// <summary>
        /// 活动名称
        /// </summary>
        public string ACT_NAME { get; set; }
        /// <summary>
        /// 活动类型
        /// 001：國蘇、002：區域、003：特殊門店
        /// </summary>
        public string ACT_TYPE_CODE { get; set; }
        /// <summary>
        /// 活动类型名称
        /// </summary>
        public string ACT_TYPE_NAME { get; set; }
        /// <summary>
        /// 渠道ID
        /// </summary>
        public string T_TYPEID { get; set; }
        /// <summary>
        /// 渠道名称
        /// </summary>
        public string T_TYPENAME { get; set; }
        /// <summary>
        /// 客户代号
        /// </summary>
        public string CUSTOMER_CODE { get; set; }
        /// <summary>
        /// 客户名称
        /// </summary>
        public string CUSTOMER_NAME { get; set; }
        /// <summary>
        /// 活动开始时间
        /// </summary>
        public DateTime ACT_START_DATE { get; set; }
        /// <summary>
        /// 活动结束时间
        /// </summary>
        public DateTime ACT_END_DATE { get; set; }
        public List<UserShopInfoModel2> Shops { get; set; }
    }
    public class UserShopInfoModel2
    {
        public string ShopCode { get; set; }
        public string ShopName { get; set; }
        public double LONGITUDE { get; set; }
        public double LATITUDE { get; set; }
    }
    public class salesActualAllModel
    {
        public string Actual_Type { get; set; }
        public string CREATOR_ID { get; set; }
        public string CREATE_DATE { get; set; }
        public string CREATE_INFO { get; set; }
        public int ACTIVE_FLAG { get; set; }
        public string COMPANY_CODE { get; set; }
        public string SALES_DATE { get; set; }
        public string T_BUID { get; set; }
        public string T_BUNAME { get; set; }
        public string MACHINE_MODEL_NO { get; set; }
        public int SALES_QTY { get; set; }
        public double SALES_PRICE { get; set; }
        public double SALES_AMOUNT { get; set; }
        public string T_TYPEID { get; set; }
        public string T_TYPENAME { get; set; }
        public string T_TVSIZEID { get; set; }
        public double TVSIZE { get; set; }
        public string CH_SALES_CODE { get; set; }
        public string CH_SALES_NAME { get; set; }
        public string CH_SHOP { get; set; }
        public string CH_SHOP_CODE { get; set; }
        public int CH_QTY { get; set; }
        public double CH_PRICE { get; set; }
        public double CH_AMOUNT { get; set; }
        public string SHOP_NAME { get; set; }
        public string CH_REPORT_ID { get; set; }
        public string CH_REPORT_NAME { get; set; }
        public string CH_REPORT_PHONE_NO { get; set; }
        public string CH_MEMO { get; set; }
        public int UP_FINISH_FLAG { get; set; }
        public string UP_BY { get; set; }
        public string A_TYPE { get; set; }
        public string id { get; set; }
        public DateTime UP_DATETIME { get; set; }
        public string GOODS_TYPE_CODE { get; set; }
        public string GOODS_TYPE_NAME { get; set; }
        public string CONSUMER_ID { get; set; }
        public string CONSUMER_NAME { get; set; }
        public string CONSUMER_PHONE_NO { get; set; }
        public string file_id { get; set; }
        public string ACT_NO { get; set; }
        /// <summary>
        /// 活动名称
        /// </summary>
        public string ACT_NAME { get; set; }
        public int BOARD_FLAG { get; set; }
        public string CH_NO { get; set; }
        /// <summary>
        /// 客户性别
        /// </summary>
        public string CONSUMER_XINGBIE { get; set; }
        /// <summary>
        /// 客户年龄
        /// </summary>
        public string CONSUMER_AGE { get; set; }

        /// <summary>
        /// 客户地址
        /// </summary>
        public string CONSUMER_ADD { get; set; }
        /// <summary>
        /// 客户地址市ID
        /// </summary>
        public string CONSUMER_ARERID { get; set; }
        /// <summary>
        /// 样品序号
        /// </summary>
        public string SAMPLE_SN_NO { get; set; }
    }
    public class salesActualChangeRes
    {
        public string errorCode { get; set; }
        public bool isOk { get; set; }
        public string errorMessage { get; set; }
    }

}
