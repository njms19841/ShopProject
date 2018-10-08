using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Market.APIService.Models
{
    public class BandModel
    {
        public string BRAND_CODE { get; set; }
        public string BRAND_NAME { get; set; }
    }
    public class SampleActModel
    {
        public string MACHINE_MODEL_NO { get; set; }
        /// <summary>
        /// ID,在新增时不用传，修改时需要传
        /// </summary>
        public string id { get; set; }
        /// <summary>
        /// 提报编号
        /// </summary>
        public string SAMPLE_UP_NO { get; set; }
        /// <summary>
        /// 门店代号
        /// </summary>
        public string SHOP_CODE { get; set; }
        /// <summary>
        /// 门店名称
        /// </summary>
        public string SHOP_NAME { get; set; }
        /// <summary>
        /// 产品类型
        /// </summary>
        public string PRODUCT_TYPE_CODE { get; set; }
        /// <summary>
        /// 产品类型名称
        /// </summary>

        public string PRODUCT_TYPE_NAME { get; set; }
        /// <summary>
        /// SN
        /// </summary>
        public string SN_NO { get; set; }
        
        /// <summary>
        /// 图片文件ID
        /// </summary>
        public string file_ids { get; set; }
        /// <summary>
        /// 出样照片,新增时不用传
        /// </summary>
        public List<imageModel> files { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string sales_No { get; set; }
        /// <summary>
        /// 盘点日期
        /// </summary>
        public DateTime day { get; set; }
        /// <summary>
        /// 最新盘点日期
        /// </summary>
        public DateTime newDay { get; set; }

        /// <summary>
        /// 上報類型代号（NormalUp：正常上報 / StocktakeUp：盤點上報）
        /// </summary>
        public string UP_TYPE_CODE { get; set; }
        /// <summary>
        /// 上報類型（NormalUp：正常上報 / StocktakeUp：盤點上報）
        /// </summary>
        public string UP_TYPE_NAME { get; set; }
        /// <summary>
        /// 樣機類型代号（001：資產樣機 / 002：折扣樣機 / 003：客戶出樣/ 004:保价样机）
        /// </summary>
        public string SAMPLE_TYPE_CODE { get; set; }
        /// <summary>
        /// 樣機類型（001：資產樣機 / 002：折扣樣機 / 003：客戶出樣/ 004:保价样机）
        /// </summary>
        public string SAMPLE_TYPE_NAME { get; set; }
        /// <summary>
        /// 是否删除
        /// </summary>
        public int isDelete { get; set; }

        /// <summary>
        /// 产品状态（001：上样 ，002：維修，003銷售下架，004 外借，005 歸還，006 销样）
        /// </summary>
        public string PRODUCT_STATUS_CODE { get; set; }
        /// <summary>
        /// 备注
        /// </summary>
        public string REMARK { get; set; }

    }
        public class ActiveActModel
    {
        /// <summary>
        /// 资料Id,新增时不用上传
        /// </summary>
        public string id { get; set; }
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
        /// ,新增时不用传
        /// </summary>
        public string ACT_TYPE_CODE { get; set; }
        /// <summary>
        /// 活动类型名称,新增时不用传
        /// </summary>
        public string ACT_TYPE_NAME { get; set; }
        /// <summary>
        /// 渠道ID,新增时不用传
        /// </summary>
        public string T_TYPEID { get; set; }
        /// <summary>
        /// 渠道名称,新增时不用传
        /// </summary>
        public string T_TYPENAME { get; set; }
        /// <summary>
        /// 客户代号,新增时不用传
        /// </summary>
        public string CUSTOMER_CODE { get; set; }
        /// <summary>
        /// 客户名称,新增时不用传
        /// </summary>
        public string CUSTOMER_NAME { get; set; }
        /// <summary>
        /// 活动开始时间,新增时不用传
        /// </summary>
        public DateTime ACT_START_DATE { get; set; }
        /// <summary>
        /// 活动结束时间,新增时不用传
        /// </summary>
        public DateTime ACT_END_DATE { get; set; }

        public string sales_No { get; set; }
        /// <summary>
        /// 门店代号
        /// </summary>
        public string SHOP_CODE { get; set; }
        /// <summary>
        /// 门店名称
        /// </summary>
        public string SHOP_NAME { get; set; }
        /// <summary>
        /// 活動照片
        /// </summary>
        public string file_id_Type002 { get; set; }
        /// <summary>
        /// DM照片
        /// </summary>
        public string file_id_Type003 { get; set; }
        /// <summary>
        /// 其他
        /// </summary>
        public string file_id_Type004 { get; set; }
        /// <summary>
        /// 活动照片,新增时不用传
        /// </summary>
        public List<imageModel> file_Model_Type002 { get; set; }
        /// <summary>
        /// DM照片,新增时不用传
        /// </summary>
        public List<imageModel> file_Model_Type003 { get; set; }
        /// <summary>
        /// 其他照片,新增时不用传
        /// </summary>
        public List<imageModel> file_Model_Type004 { get; set; }
        public List<ActiveActModelSub> ActiveSub { get; set; }
        public DateTime ADD_DATE { get; set; }
       

    }
    public class ActiveActModelSub
    {
       public string id { get; set; }
        
        /// <summary>
        /// 品牌代号
        /// </summary>
        public string BRAND_CODE { get; set; }
        /// <summary>
        /// 品牌名称
        /// </summary>
        public string BRAND_NAME { get; set; }
        /// <summary>
        /// 关键型号
        /// </summary>
        public string MACHINE_MODEL_NO { get; set; }
        /// <summary>
        /// 销售单价
        /// </summary>
        public double SALES_PRICE { get; set; }
        /// <summary>
        /// 尺寸ID
        /// </summary>
        public string T_TVSIZEID { get; set; }
        /// <summary>
        /// 尺寸
        /// </summary>
        public int TVSIZE { get; set; }
        /// <summary>
        /// 是否为新品
        /// </summary>
        public int IS_NEW_PRD_FLAG { get; set; }
        /// <summary>
        /// 促銷照片ID
        /// </summary>
        public string file_id_Type001 { get; set; }
        
        /// <summary>
        /// 促銷照片,新增时不用传
        /// </summary>
        public List<imageModel> file_Model_Type001 { get; set; }
        
    }
    public class imageModel
    {
        /// <summary>
        /// 图片ID
        /// </summary>
        public string id { get; set; }
        /// <summary>
        /// 图片url
        /// </summary>
        public string url { get; set; }

    }
    public class ProdModel
    {
        /// <summary>
        /// 类型
        /// </summary>
        public string CODE { get; set; }
        /// <summary>
        /// 类型名称
        /// </summary>
        public string NAME { get; set; }

    }
    public class ActiveModel
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
        public string SHOP_CODE { get; set; }
        /// <summary>
        /// 门店名称
        /// </summary>
        public string SHOP_NAME { get; set; }

        public bool is_New { get; set; }
        public bool is_OtherNew { get; set; }
        //public List<UserShopInfoModel> Shops { get; set; }
        //public List<ActiveActModelSub> ActiveSub { get; set; }
    }
    public class SalesOtherModel
    {
        /// <summary>
        /// 资料id
        /// </summary>
        public string id { get; set; }
        /// <summary>
        /// 品牌ID
        /// </summary>
        public string BRAND_CODE { get; set; }
        /// <summary>
        /// 品牌名称
        /// </summary>
        public string BRAND_NAME { get; set; }
        /// <summary>
        /// 重点型号名称
        /// </summary>
        public string KEY_MODEL_CODE { get; set; }
        /// <summary>
        /// 销售价格
        /// </summary>
        public double SALES_PRICE { get; set; }
        /// <summary>
        /// 尺寸ID
        /// </summary>
        public string T_TVIZEID { get; set; }
        /// <summary>
        /// 是否为曲面
        /// </summary>
        public int IS_CURVED_FLAG { get; set; }
        /// <summary>
        /// 是否为OLED
        /// </summary>
        public int IS_OLED_FLAG { get; set; }
        /// <summary>
        /// 是否量子点
        /// </summary>
        public int IS_QUANTUM_DOT_FLAG { get; set; }
        /// <summary>
        /// 工号
        /// </summary>
        public string sales_No { get; set; }
        public string shopCode { get; set; }
        public string shopName { get; set; }
        public int IS_SMART_TV_FLAG { get; set; }
        public string prdFileIds { get; set; }
        public List<imageModel> prdFiles { get; set; }
        public string ACT_NO { get; set; }
        /// <summary>
        /// 活动名称
        /// </summary>
        public string ACT_NAME { get; set; }
        public DateTime CreatedTime { get; set; }
    }
    public class NoMemberModel
    {
        /// <summary>
        /// 分类会员ID
        /// </summary>
        public String MF_MEMBER_ID { get; set; }
       /// <summary>
       /// 会员编号
       /// </summary>
        public String MEMBER_NO { get; set; }
        /// <summary>
        /// 会员类型
        /// </summary>
        public String MEMBER_TYPE { get; set; }
        /// <summary>
        /// 会员类型名称
        /// </summary>
        public String MEMBER_TYPENAME { get; set; }
        /// <summary>
        /// 到访时间
        /// </summary>
        public DateTime VISIT_TIME { get; set; }
        /// <summary>
        /// 头像路径
        /// </summary>
        public String picUrl { get; set; }

    }
    public class MessageModel
    {
        public String MessageId { get; set; }
        public String Title { get; set; }
        public String Desc { get; set; }
        public String ContextUrl { get; set; }
        public DateTime MessageTime { get; set; }
        public string MessageType { get; set; }
    }
    public class Member_SmartPrd
    {
        /// <summary>
        /// 品类
        /// </summary>
        public String type { get; set; }
        /// <summary>
        /// 品牌
        /// </summary>
        public String bord { get; set; }
        /// <summary>
        /// 产品类型
        /// </summary>
        public String name { get; set; }
    }
    public class Member_Smart
    {
        
       
        /// <summary>
        /// 名称
        /// </summary>
        public String name { get; set; }

        /// <summary>
        /// 排名
        /// </summary>
        public int rick { get; set; }

        /// <summary>
        /// 是否为星标
        /// </summary>
        public bool isS { get; set; }


    }


    public class Member_ByModeMast
    {
        /// <summary>
        /// 单号
        /// </summary>
        public string MAC { get; set; }
        /// <summary>
        /// 明细
        /// </summary>

        public List<Member_ByMode> detail { get; set; }
        /// <summary>
        /// 销售金额
        /// </summary>
        public double amt { get; set; }
        /// <summary>
        /// 购买时间
        /// </summary>
        public DateTime byTime { get; set; }

    }
    public class Member_ByMode
    {
        /// <summary>
        /// 型号
        /// </summary>
        public string MAC { get; set; }
        /// <summary>
        /// 型号名称
        /// </summary>
        public string MACName { get; set; }


        /// <summary>
        /// 购买数量
        /// </summary>
        public int qty { get; set; }
        /// <summary>
        /// 单价
        /// </summary>
        public double price { get; set; }


        /// <summary>
        /// 购买时间
        /// </summary>
        public DateTime byTime { get; set; }

    }
    public class Member_IOSHOPMode
    {
        /// <summary>
        /// 到访门店
        /// </summary>
        public string SHOP_NAME { get; set; }
        /// <summary>
        /// 到访时间
        /// </summary>
        public DateTime Time { get; set; }

    }
    public class MemberMode
    {
        /// <summary>
        /// 姓名
        /// </summary>
        public String name { get; set; }
        /// <summary>
        /// 性别
        /// </summary>
        public String six { get; set; }
        /// <summary>
        /// 手机号
        /// </summary>
        public String phoneNumber { get; set; }
       /// <summary>
       /// 注册门店
       /// </summary>
        public String regShop { get; set; }
        /// <summary>
        /// 注册门店代码
        /// </summary>
        public String regShopCode { get; set; }
        /// <summary>
        /// 年龄
        /// </summary>
        public int age { get; set; }
        /// <summary>
        /// 会员类型
        /// </summary>
        public string MemberType { get; set; }
        /// <summary>
        /// 地址
        /// </summary>
        public String adder { get; set; }
        /// <summary>
        /// 微信
        /// </summary>
        public String weiChat { get; set; }
        
        /// <summary>
        /// 用户头像URL
        /// </summary>
        public String picRUL { get; set; }
       
        /// <summary>
        /// 分类会员ID
        /// </summary>
        public String MF_MEMBER_ID { get; set; }
        /// <summary>
        /// 会员编号
        /// </summary>
        public String MEMBER_NO { get; set; }
        /// <summary>
        /// 会员卡号
        /// </summary>
        public String MEMBER_CARDNO { get; set; }
        /// <summary>
        /// 会员积分点
        /// </summary>
        public int MEMBER_POINT { get; set; }

        /// <summary>
        /// 备注
        /// </summary>
        public String MEMO { get; set; }
        /// <summary>
        /// 最近购买日期
        /// </summary>
        public DateTime LAST_BUY_DATE { get; set; }
        /// <summary>
        /// 最近到访门店
        /// </summary>
        public String LAST_INSHOP { get; set; }
        /// <summary>
        /// 最近到访时间
        /// </summary>
        public DateTime LAST_INSHOP_TIME { get; set; }
        /// <summary>
        /// 最近购买型号
        /// </summary>
        public String LAST_BUY_MODEL { get; set; }
        /// <summary>
        /// 最近购买数量
        /// </summary>
        public int LAST_BUY_QTY { get; set; }
        /// <summary>
        /// 到访次数
        /// </summary>
        public int VISIT_COUNT { get; set; }
        /// <summary>
        /// 关注型号列表
        /// </summary>
        public List<string> lookMacsList { get; set; }
       /// <summary>
       /// 购买历史记录(停用)
       /// </summary>
       public List<Member_ByMode> ByList { get; set; }
        /// <summary>
        /// 购买历史记录
        /// </summary>
        public List<Member_ByModeMast> ByList2 { get; set; }
        /// <summary>
        /// 到访历史记录
        /// </summary>
        public List<Member_IOSHOPMode> IOList { get; set; }
        /// <summary>
        /// 推荐商品
        /// </summary>
        public List<Member_SmartPrd> SmartList { get; set; }

        /// <summary>
        /// 推荐商品
        /// </summary>
        public List<Member_Smart> SmartProd { get; set; }

        /// <summary>
        /// 品牌
        /// </summary>
        public List<Member_Smart> SmartBrod { get; set; }

        /// <summary>
        /// 推荐产品类型
        /// </summary>
        public List<Member_Smart> SmartType { get; set; }



        /// <summary>
        /// 另外4张图片
        /// </summary>
        public List<string> picRULOthers { get; set; }
        /// <summary>
        /// 月平均到访次数
        /// </summary>
        public int AVGMONTH_VISITCOUNT { get; set; }
        /// <summary>
        /// 会员地址
        /// </summary>
        public string MEMBER_ADDR { get; set; }

        /// <summary>
        /// 会员等级 Nomember:非会员, Sliver:普通会员,Gold:VIP会员,Diamond:钻石会员
        /// </summary>
        public string MEMBER_LEVEL_CODE { get; set; }

        /// <summary>
        /// 会员等级 Nomember:非会员, Sliver:普通会员,Gold:VIP会员,Diamond:钻石会员
        /// </summary>
        public string MEMBER_LEVEL_NAME { get; set; }
        /// <summary>
        /// 消费类型
        /// </summary>
        public string CG_TYPE_NAME { get; set; }
        /// <summary>
        /// 识别位置
        /// </summary>
        public string SCAN_AREA { get; set; }
        /// <summary>
        /// 首次到访时间
        /// </summary>
        public DateTime FAST_TIME { get; set; }

    }
    public class MemberQueryMode
    {
        public int vistaCount { get; set; }
        /// <summary>
        /// 姓名
        /// </summary>
        public String name { get; set; }
        /// <summary>
        /// 会员类型
        /// </summary>
        public String type { get; set; }
        /// <summary>
        /// 头像URL
        /// </summary>
        public String picUrl { get; set; }
        /// <summary>
        /// 分类会员ID
        /// </summary>
        public String MF_MEMBER_ID { get; set; }
        /// <summary>
        /// 会员编号
        /// </summary>
        public String MEMBER_NO { get; set; }
        /// <summary>
        /// 会员注册时间
        /// </summary>
        public DateTime RegeditTime { get; set; }
        /// <summary>
        /// 电话号码
        /// </summary>
        public string phoneNumber { get; set; }
        /// <summary>
        /// 月平均到访次数
        /// </summary>
        public int AVGMONTH_VISITCOUNT { get; set; }
        /// <summary>
        /// 会员地址
        /// </summary>
        public string MEMBER_ADDR { get; set; }

        /// <summary>
        /// 会员等级 Sliver:普通会员,Gold:VIP会员,Diamond:钻石会员
        /// </summary>
        public string MEMBER_LEVEL_CODE { get; set; }

        /// <summary>
        /// 会员等级 Sliver:普通会员,Gold:VIP会员,Diamond:钻石会员
        /// </summary>
        public string MEMBER_LEVEL_NAME { get; set; }
        /// <summary>
        /// 消费类型
        /// </summary>
        public string CG_TYPE_NAME { get; set; }
        /// <summary>
        /// 识别位置
        /// </summary>
        public string SCAN_AREA { get; set; }
    }
        public class MemberRegestMode
    {
        /// <summary>
        /// 姓名
        /// </summary>
        public String name { get; set; }
        /// <summary>
        /// 性别
        /// </summary>
        public String six { get; set; }
        /// <summary>
        /// 手机号
        /// </summary>
        public String phoneNumber { get; set; }
        /// <summary>
        /// 地址
        /// </summary>
        public String adder { get; set; }
        /// <summary>
        /// 微信
        /// </summary>
        public String weiChat { get; set; }
        /// <summary>
        /// 购买型号
        /// </summary>
        public String bayMac { get; set; }
        /// <summary>
        /// 购买数量
        /// </summary>
        public int number { get; set; }
        /// <summary>
        /// 关注型号,多个用逗号分隔
        /// </summary>
        public String lookMacs { get; set; }
        /// <summary>
        /// 用户头像1
        /// </summary>
        public String pic1id { get; set; }
        /// <summary>
        /// 用户头像2
        /// </summary>
        public String pic2id { get; set; }
        /// <summary>
        /// 用户头像3
        /// </summary>
        public String pic3id { get; set; }
        /// <summary>
        /// 用户头像4
        /// </summary>
        public String pic4id { get; set; }
        /// <summary>
        /// 用户头像5
        /// </summary>
        public String pic5id { get; set; }

        /// <summary>
        /// 分类会员ID
        /// </summary>
        public String MF_MEMBER_ID { get; set; }
        /// <summary>
        /// 会员编号
        /// </summary>
        public String MEMBER_NO { get; set; }
        /// <summary>
        /// 门店编号
        /// </summary>
        public String SHOP_CODE { get; set; }
        /// <summary>
        /// 备注
        /// </summary>
        public String MEMO { get; set; }
      

    }
    public class OrderModel
    {
        /// <summary>
        ///  訂單日期
        /// </summary>
        public String ORDER_DATE { get; set; }
        /// <summary>
        /// 訂單編號
        /// </summary>
        public String ORDER_CODE { get; set; }
        /// <summary>
        /// 型號
        /// </summary>
        public String MACHINE_MODEL_NO { get; set; }
        /// <summary>
        /// 訂單數量
        /// </summary>
        public int ORDER_SALE_QTY { get; set; }
        /// <summary>
        /// 物流單號
        /// </summary>
        public String LOGIST_CAR_CODE { get; set; }
        /// <summary>
        /// 物流狀態
        /// </summary>
        public int PROCESS_FLAG { get; set; }
        /// <summary>
        /// 物流狀態說明
        /// </summary>
        public String PROCESS_FLAG_NAME { get; set; }
        /// <summary>
        /// 發貨地
        /// </summary>
        public String DC_NAME { get; set; }
        /// <summary>
        /// 發貨市
        /// </summary>
        public String FACTORY_CITY { get; set; }
        /// <summary>
        /// 顧客代碼
        /// </summary>
        public String CUSTOMER_CODE { get; set; }
        /// <summary>
        /// 顧客名稱
        /// </summary>
        public String CUSTOMER_NAME { get; set; }
        /// <summary>
        /// 發貨日期
        /// </summary>
        public String LOGIST_DATE { get; set; }
        /// <summary>
        /// 發貨數量
        /// </summary>
        public int QTY { get; set; }
        /// <summary>
        /// 到貨日期
        /// </summary>
        public String ARRIVAL_DATE { get; set; }
        /// <summary>
        /// 簽收日期
        /// </summary>
        public String SIGNUP_UPLOAD_DATE { get; set; }
        /// <summary>
        /// 簽收數量
        /// </summary>
        public int SIGNUP_QTY { get; set; }
        /// <summary>
        /// 收貨省
        /// </summary>
        public String RECEIVE_PROVINCE { get; set; }
        /// <summary>
        /// 收貨市
        /// </summary>
        public String RECEIVE_CITY { get; set; }

    }
   
    public class CustomerModel
    {
        public string Code { get; set; }
        public string Name { get; set; }
    }
    public class AgeModel
    {
        public string Code { get; set; }
        public string Name { get; set; }
    }
    public class VerModel
    {
        public string id { get; set; }
        public string Ver { get; set; }
        public string Type { get; set; }
        public string DowloandUrl { get; set; }
    }
    public class fileViewModel
    {
        public string fileId { get; set; }
    }
    public class rePortViewModel
    {
        /// <summary>
        /// 上报类型
        /// </summary>
        public string type
        {
            get; set;
        }
        /// <summary>
        /// 子类型
        /// </summary>
        public string subType
        {
            get; set;
        }
        /// <summary>
        /// 内容
        /// </summary>
        public string desc
        {
            get; set;
        }
        /// <summary>
        /// 图片id，多个采用逗号分隔,xxxxxx,s123123,xxxxxxx
        /// </summary>
        public string fileId
        {
            get; set;
        }
    }
    public class machineViewModel
    {
        /// <summary>
        /// 门店
        /// </summary>
        public string ShopCode { get; set; }
        /// <summary>
        /// 型号
        /// </summary>
        public string MACHINE_MODEL_NO { get; set; }
        /// <summary>
        /// 电视尺寸
        /// </summary>
        public string T_TVSIZEID { get; set; }
       /// <summary>
       /// 描述
       /// </summary>
        public string DESCP { get; set; }
        public string BANDCODE { get; set; }
        public string BANDNAME { get; set; }
    }
    public class tvSizeViewModel
    {
        /// <summary>
        /// 电视尺寸ID
        /// </summary>
        public string T_TVSIZEID { get; set; }
        /// <summary>
        /// 尺寸名称
        /// </summary>
        public string T_TVSIZENAME { get; set; }

      

    }
    public class homeModel
    {
        /// <summary>
        /// 本日销售台数
        /// </summary>
        public int DayQty { get; set; }
        /// <summary>
        /// 本月销售台数
        /// </summary>
        public int MonthQty { get; set; }
        /// <summary>
        /// 上周销售台数
        /// </summary>
        public int LastWeekQty { get; set; }
        /// <summary>
        /// 上月销售台数
        /// </summary>
        public int LastMonthQty { get; set; }
        /// <summary>
        /// 今日成交总台数
        /// </summary>
        public int DayTotalQty { get; set; }
        /// <summary>
        /// 当日成交总金额
        /// </summary>
        public double DayTotalAmount { get; set; }

        /// <summary>
        /// 前日未上传发票数
        /// </summary>
        public int LastDayLtCount { get; set; }

        
    }
    
    public class salesActualModelV2
    {
        /// <summary>
        /// 日期范围
        /// </summary>
        public string dateString { get; set; }

        /// <summary>
        /// 汇总数据
        /// </summary>
        public List<salesActualModelV2Mast> Mast { get; set; }
        /// <summary>
        /// 门店汇总数据
        /// </summary>
        public List<salesActualModelV2Shop> ShopMast { get; set; }

        /// <summary>
        /// 明细数据
        /// </summary>
        public List<salesActualModelV2Detail> Detail { get; set; }



    }
    public class salesActualModelV2Shop
    {
        /// <summary>
        /// 门店编号
        /// </summary>
        public String shopCode { get; set; }
        /// <summary>
        /// 门店名称
        /// </summary>
        public String  shopName { get; set; }
        /// <summary>
        /// 汇总数据
        /// </summary>
        public List<salesActualModelV2Mast> Mast { get; set; }
    }
        public class salesActualModelV2Mast
    {
        /// <summary>
        /// 员工名称
        /// </summary>
        public string salesName { get; set; }
        /// <summary>
        /// 型号
        /// </summary>
        public string MACHINE_MODEL_NO { get; set; }
        /// <summary>
        /// 销售数量
        /// </summary>
        public int Actual_Qty { get; set; }

        /// <summary>
        /// 销售金额
        /// </summary>
        public double Actual_Amount { get; set; }
    }
    public class UppUpModel
    {
        /// <summary>
        /// 员工编号
        /// </summary>
        public string salesNo { get; set; }
        /// <summary>
        /// 单号
        /// </summary>
        public string id { get; set; }
        /// <summary>
        /// 发票文件ID
        /// </summary>
        public string files_id { get; set; }

    }
        public class salesActualModelV2Detail
    {
        public string ACT_NO { get; set; }
        public string ACT_NAME { get; set; }
        public string T_TVSIZEID { get; set; }
        public string BRAND_CODE { get; set; }
        public int TVSIZE { get; set; }
        /// <summary>
        /// id
        /// </summary>
        public string id { get; set; }
        public string BAY_NAME { get; set; }
        public string BAY_PHONE_NO { get; set; }

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
        /// 客户地址省ID
        /// </summary>
        public string CONSUMER_ProvinceID { get; set; }

        /// <summary>
        /// 单号
        /// </summary>
        public string TRANS_NO { get; set; }
        /// <summary>
        /// 是否已上传发票 Y:是，N：否
        /// </summary>
        public string INVOICE_FLAG { get; set; }
        /// <summary>
        /// 工号
        /// </summary>
        public string sales_No { get; set; }
        /// <summary>
        /// 员工名称
        /// </summary>
        public string salesName { get; set; }
        /// <summary>
        /// 门店代码
        /// </summary>
        public string SHOP_CODE { get; set; }


        /// <summary>
        /// 门店名称
        /// </summary>
        public string SHOP_NAME { get; set; }

        /// <summary>
        /// 上报日期
        /// </summary>
        public string Actual_Day { get; set; }
        /// <summary>
        /// 销售数量
        /// </summary>
        public int Actual_Qty { get; set; }

        /// <summary>
        /// 销售单价
        /// </summary>
        public double Actual_Price { get; set; }
        /// <summary>
        /// 销售类型
        /// </summary>
        public string GOODS_TYPE_CODE { get; set; }
        /// <summary>
        /// 销售类型名称
        /// </summary>
        public string GOODS_TYPE_NAME { get; set; }
        /// <summary>
        /// 型号
        /// </summary>
        public string MACHINE_MODEL_NO { get; set; }
        /// <summary>
        /// 大类ID
        /// </summary>
        public string BigTypeId { get; set; }
        /// <summary>
        /// 小类ID
        /// </summary>
        public string SmallTypeId { get; set; }
        /// <summary>
        /// 客户名称
        /// </summary>
        public string CONSUMER_NAME { get; set; }
        /// <summary>
        /// 上报类型
        /// </summary>
        public string Actual_Type { get; set; }
        /// <summary>
        /// 发票状态
        /// </summary>
        public string INVOICE_STATUS_NAME { get; set; }
        /// <summary>
        /// 不合格原因
        /// </summary>
        public string UNQUALIFIED_REASON_NAME { get; set; }
        /// <summary>
        /// 是否有赠品
        /// </summary>
        public int BOARD_FLAG { get; set; }

        /// <summary>
        /// 样机序号
        /// </summary>
        public string SAMPLE_SN_NO { get; set; }


    }
    public class SalesActualRetunModel
    {
        /// <summary>
        /// 是否是会员
        /// </summary>
        public int isMember { get; set; }
        /// <summary>
        /// 是否为深圳战区
        /// </summary>
        public int isNeedMember { get; set; }
        /// <summary>
        /// 会员编号
        /// </summary>
        public string memberNumber { get; set; }
    }
        public class salesActualModel
    {
        [Required]
        [Display(Name = "工号")]
        public string sales_No { get; set; }

        [Required]
        [Display(Name = "门店代码")]
        public string SHOP_CODE { get; set; }

       
        [Display(Name = "门店名称")]
        public string SHOP_NAME { get; set; }

        [Required]
        [Display(Name = "上报日期")]
        public string Actual_Day { get; set; }

        [Required]
        [Display(Name = "尺寸ID")]
        public string T_TVSIZEID { get; set; }

        [Required]
        [Display(Name = "型号")]
        public string MACHINE_MODEL_NO { get; set; }

        [Required]
        [Display(Name = "上报类型")]
        /// <summary>
        /// 1:销售上报
        /// 2:销售更正
        /// 3:销退
        /// </summary>
        public string Actual_Type { get; set; }

        [Required]
        [Display(Name = "销售数量")]
        public int Actual_Qty { get; set; }

        [Required]
        [Display(Name = "销售单价")]
        public double Actual_Price { get; set; }
        [Display(Name = "销售类型")]
        public string GOODS_TYPE_CODE { get; set; }
        [Display(Name = "销售类型名称")]
        public string GOODS_TYPE_NAME { get; set; }

        [Display(Name = "客户名称")]
        public string CONSUMER_NAME { get; set; }

        [Display(Name = "客户性别")]
        public string CONSUMER_XINGBIE { get; set; }
        [Display(Name = "客户年龄")]
        public string CONSUMER_AGE { get; set; }

        [Display(Name = "客户地址")]
        public string CONSUMER_ADD { get; set; }

        [Display(Name = "客户地址市ID")]
        public string CONSUMER_ARERID { get; set; }

        [Display(Name = "客户手机号")]
        public string CONSUMER_PHONE_NO { get; set; }
        [Display(Name = "发票图片ID")]
        public string file_id { get; set; }
        [Display(Name = "活动代号")]
        public string ACT_NO { get; set; }
        [Display(Name = "活动名称")]
        public string ACT_NAME { get; set; }

        [Display(Name = "上报设备ID")]
        public string deviceUUid { get; set; }
        

        /// <summary>
        /// 是否有赠品
        /// </summary>
            public int BOARD_FLAG { get; set; }
        /// <summary>
        /// 如果为销售更正，销退，传原始资料ID
        /// </summary>
        public string old_id { get; set; }

        /// <summary>
        /// 样品序号
        /// </summary>
        public string SAMPLE_SN_NO { get; set; }
    }
}