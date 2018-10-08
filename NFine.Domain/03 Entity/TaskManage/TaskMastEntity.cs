using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NFine.Domain._03_Entity.TaskManage
{
    public class TaskMastEntity : IEntity<TaskMastEntity>
    {
        
        /// <summary>
        /// TaskId
        /// </summary>
        public string id { get; set; }
        /// <summary>
        /// 创建用户
        /// </summary>
        public string createdUserId { get; set; }
        /// <summary>
        /// 修改用户
        /// </summary>
        public string modifyUserId { get; set; }
        /// <summary>
        /// 任務類型
        /// </summary>
        public string taskType { get; set; }
        /// <summary>
        /// 任務類型名称
        /// </summary>
        public string taskTypeName { get; set; }
        /// <summary>
        /// 任務主題
        /// </summary>
        public string taskName { get; set; }
        /// <summary>
        /// 开始时间
        /// </summary>
        public DateTime? starTime { get; set; }
        /// <summary>
        /// 结束时间
        /// </summary>
        public DateTime? endTime { get; set; }
        /// <summary>
        /// 重复类型
        /// 1 = 一次
        //4 = 每日
        //8 = 每周
        //16 = 每月
        /// </summary>
        public int? freqType { get; set; }
        /// <summary>
        /// 提醒类型
        /// 0：不提醒
        /// 1：发生时
        /// 2：提前5分钟
        /// 3：提前10分钟
        /// 4：提前30分钟
        /// 5：提前1小时
        /// 6：提前2小时
        /// 7：提前6小时
        /// 8：提前1天
        /// 9：提前2天
        /// </summary>
        public int? alertType { get; set; }
        /// <summary>
        /// 客戶名称
        /// </summary>
        public string CustomerName { get; set; }
        /// <summary>
        /// 客户代号
        /// </summary>
        public string CustomerCode { get; set; }
        /// <summary>
        /// 重要程度  0:一般 1:重要
        /// </summary>
        public int? importantType { get; set; }
        /// <summary>
        /// 任务描述
        /// </summary>
        public string desc { get; set; }
        /// <summary>
        /// 是否删除
        /// </summary>
        public int? isDelete { get; set; }
        /// <summary>
        /// 文件ID
        /// </summary>
        public string fileId { get; set; }
        /// <summary>
        /// 文件名
        /// </summary>
        public string fileName { get; set; }
        /// <summary>
        /// 扩展名
        /// </summary>
        public string fileExt { get; set; }
        public DateTime? createdTime { get; set; }
        public DateTime? modifyTime { get; set; }
       
        
        /// <summary>
        /// 是否为全天事件
        /// </summary>
        public int? isAll { get; set; }
        /// <summary>
        /// 事件来源
        /// </summary>
        public string taskSource { get; set; }
        /// <summary>
        /// 消息单号
        /// </summary>
        public string MESSAGE_BILL_NO { get; set; }
        /// <summary>
        /// 消息序号
        /// </summary>
        public int? MESSAGE_SUB_NO { get; set; }
        /// <summary>
        /// 通知是否要回覆類別（PDG1365：001：不需回覆，002：可回覆可不回覆，003：必須回覆）
        /// </summary>
        public string MESSAGE_REPLY_TYPE_CODE { get; set; }
        /// <summary>
        /// 通知是否要回覆類別（PDG1365：001：不需回覆，002：可回覆可不回覆，003：必須回覆）
        /// </summary>
        public string MESSAGE_REPLY_TYPE_NAME { get; set; }
        /// <summary>
        /// 接收人員工ID
        /// </summary>
        public string RECEIVE_EMPLOYEE_CODE { get; set; }
        /// <summary>
        /// 接收人員工姓名
        /// </summary>
        public string RECEIVE_EMPLOYEE_NAME { get; set; }
        /// <summary>
        /// 接收人管理組織ID
        /// </summary>
        public string RECEIVE_MANAGE_ORG_ID { get; set; }
        /// <summary>
        /// 接收人職務代碼（PDG1202：GuideMan 导购员，SalesMan 业务员，SupervisorMan 督导
        ///ChannelManager 渠道长，BUManager 战区长，CityManager 城市经理，BigBuManage大区长）
        /// </summary>
        public string RECEIVE_JOB_CODE { get; set; }
        /// <summary>
        /// 通知緊急程度類別（PDG1363：001：一般，002：緊急，003：特急，099：其他）
        /// </summary>
        public string URGENCY_TYPE_CODE { get; set; }
        /// <summary>
        /// 通知緊急程度類別（PDG1363：001：一般，002：緊急，003：特急，099：其他）
        /// </summary>
        public string URGENCY_TYPE_NAME { get; set; }
        /// <summary>
        /// 是否已读
        /// </summary>
        public int? isRead { get; set; }
        /// <summary>
        /// 第一次已读时间
        /// </summary>
        public DateTime? ReadTime { get; set; }
        public int? isReply { get; set; }
        /// <summary>
        /// 回覆主題
        /// </summary>
        public string REPLY_SUBJECT { get; set; }
        /// <summary>
        /// 回覆內容
        /// </summary>
        public string REPLY_CONTENT { get; set; }
        /// <summary>
        /// 回覆附件ID
        /// </summary>
        public string REPLY_FileId { get; set; }
        public string REPLY_fileExt { get; set; }
        public string taskUrl { get; set; }
        public string Location { get; set; }
        public string address { get; set; }


    }
}
