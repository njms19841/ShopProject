using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NFine.Domain._03_Entity.APPManage
{
    public class marketSalesActualEntity : IEntity<marketSalesActualEntity>
    {
       public string id { get; set; }
        public string sales_No { get; set; }
        public string SHOP_CODE { get; set; }
        public string Actual_Day { get; set; }
        public string T_TVSIZEID { get; set; }
        public string MACHINE_MODEL_NO { get; set; }
        public DateTime? Created_Time { get; set; }
        public DateTime? Modify_Time { get; set; }
        public string Actual_Type { get; set; }
        public int? Actual_Qty { get; set; }
        public double? Actual_Price { get; set; }
        public int? isSync { get; set; }

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
        public int? BOARD_FLAG { get; set; }
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
        /// <summary>
        /// 上报设备ID
        /// </summary>
        public string deviceUUid { get; set; }
        
    }
}
