using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Market.APIService.Models
{

    public class ReplyTaskModel
    {
        /// <summary>
        /// Task Id
        /// </summary>
        public string taskId { get; set; }
        /// <summary>
        /// 工作报告标题
        /// </summary>
        public string title { get; set; }
        /// <summary>
        /// 工作报告内容
        /// </summary>
        public string context { get; set; }
        /// <summary>
        /// 工作报告文件ID
        /// </summary>
        public string fileId { get; set; }
        /// <summary>
        /// 工作报告文件扩展名
        /// </summary>
        public string fileExt { get; set; }
    }
    public class QueryTaskModel
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
        public DateTime starTime { get; set; }
        /// <summary>
        /// 结束时间
        /// </summary>
        public DateTime endTime { get; set; }

        /// <summary>
        /// 重复任务被拆解后的当天
        /// </summary>
        public DateTime day { get; set; }
        /// <summary>
        /// 重复任务被拆解后的当天
        /// </summary>
        public string dayString { get; set; }

        /// <summary>
        /// 重复类型
        /// 1 = 一次
        ///4 = 每日
        ///8 = 每周
        ///16 = 每月
        /// </summary>
        public int freqType { get; set; }
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
        public int alertType { get; set; }
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
        public int importantType { get; set; }
        /// <summary>
        /// 任务描述
        /// </summary>
        public string desc { get; set; }
        /// <summary>
        /// 是否删除
        /// </summary>
        public int isDelete { get; set; }
      
        /// <summary>
        /// 文件ID
        /// </summary>
        public string fileUrl { get; set; }
        public string fileId { get; set; }
        
        /// <summary>
        /// 任务状态 1：待接受 2：處理中 3：已過期 4：完成 5：已刪除的任務
        /// </summary>
        public int status { get; set; }
        /// <summary>
        /// 任务跳转URL
        /// </summary>
        public string taskUrl { get; set; }
        /// <summary>
        /// 文件名
        /// </summary>
        public string fileName { get; set; }
        /// <summary>
        /// 扩展名
        /// </summary>
        public string fileExt { get; set; }
        /// <summary>
        /// 是否为全天事件
        /// </summary>
        public int isAll { get; set; }
        /// <summary>
        /// 事件来源
        /// </summary>
        public string taskSource { get; set; }

        /// <summary>
        /// 通知是否要回覆類別（PDG1365：001：不需回覆，002：可回覆可不回覆，003：必須回覆）
        /// </summary>
        public string MESSAGE_REPLY_TYPE_CODE { get; set; }
        /// <summary>
        /// 通知是否要回覆類別（PDG1365：001：不需回覆，002：可回覆可不回覆，003：必須回覆）
        /// </summary>
        public string MESSAGE_REPLY_TYPE_NAME { get; set; }

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
        /// 工作报告主題
        /// </summary>
        public string REPLY_SUBJECT { get; set; }
        /// <summary>
        /// 工作报告內容
        /// </summary>
        public string REPLY_CONTENT { get; set; }
        /// <summary>
        /// 工作报告附件ID
        /// </summary>
        public string REPLY_FileId { get; set; }
        /// <summary>
        /// 工作报告附件URL
        /// </summary>
        public string Reply_fileUrl { get; set; }
        /// <summary>
        /// 是否已经回复工作报告
        /// </summary>
        public int isReply { get; set; }

        /// <summary>
        /// 是否为通知
        /// </summary>
        public int isMessage { get; set; }
        /// <summary>
        /// 定位地址(12.2342,112.1232)
        /// </summary>
        public string Location { get; set; }
        /// <summary>
        /// 地址
        /// </summary>
        public string address { get; set; }
    }
    public class TaskReport
    {
        public string id { get; set; }

        /// <summary>
        /// 创建用户
        /// </summary>
        public string userId { get; set; }
        public string userName { get; set; }
        public DateTime? reportTime { get; set; }
        /// <summary>
        /// 状态 1:未审阅 2:已审阅
        /// </summary>
        public int? state { get; set; }
        /// <summary>
        /// 标题
        /// </summary>
        public string title { get; set; }
        /// <summary>
        /// 内容
        /// </summary>
        public string context { get; set; }
        /// <summary>
        /// 附件ID
        /// </summary>
        public string fileId { get; set; }
        /// <summary>
        /// 附件扩展名
        /// </summary>
        public string fileExt { get; set; }
        //附件文件名
        public string fileName { get; set; }
        /// <summary>
        /// 附件Url
        /// </summary>
        public string fileUrl { get; set; }
        /// <summary>
        /// 报告类型 1:日报 2:周报 3:月报 4:其他
        /// </summary>
        public int? reportType { get; set; }
        /// <summary>
        /// 工作报告用户列表（查询用）
        /// </summary>
        public TaskReportUsers users { get; set; }
        /// <summary>
        /// 审阅人
        /// </summary>
        public List<string> AllowUser { get; set; }
        /// <summary>
        /// 抄送人
        /// </summary>
        public List<string> ReadUser { get; set; }
        /// <summary>
        /// 语音文件ID
        /// </summary>
        public string audoFileId { get; set; }
        /// <summary>
        /// 语音文件URL
        /// </summary>
        public string audoFileUrl { get; set; }
    }
    public class TaskReportUsers
    {
        /// <summary>
        ///审阅人
        /// </summary>
        public List<TaskUser> AllowUser { get; set; }

        /// <summary>
        /// 抄送人
        /// </summary>
        public List<TaskUser> ReadUser { get; set; }
    }
    public class TaskUsers
    {
        /// <summary>
        /// 责任人
        /// </summary>
        public List<TaskUser> responsibilityUser { get; set; }

        /// <summary>
        /// 参与人
        /// </summary>
        public List<TaskUser> participateUser { get; set; }
    }
    public class TaskUser
    {
        /// <summary>
        /// Userid
        /// </summary>
        public string userId { get; set; }
        /// <summary>
        /// 用户名称
        /// </summary>
        public string userName { get; set; }

        /// <summary>
        /// 用户头像URL
        /// </summary>
        public string picUrl { get; set; }

        /// <summary>
        /// 群组名称
        /// </summary>
        public string GroupName { get; set; }

        public string Desc { get; set; }
        public string JobName { get; set; }

    }
   
    public class MemberMessageInModel
    {
        /// <summary>
        /// 会员ID
        /// </summary>
        public string memberId { get; set; }
        /// <summary>
        /// 门店代号
        /// </summary>
        public string shopCode { get; set; }
        /// <summary>
        /// 分类会员ID
        /// </summary>
        public string mfMemberId { get; set; }

        /// <summary>
        /// 进店时间
        /// </summary>
        public DateTime InTime { get; set; }
        /// <summary>
        /// 会员类型代号
        /// </summary>
        public string MemberTypeCode { get; set; }
        /// <summary>
        /// 会员类型名称
        /// </summary>
        public string MemberTypeName { get; set; }
        /// <summary>
        /// 图像路径
        /// </summary>
        public string picUrl { get; set; }


    }

    public class MemberMessageOutModel
    {
        /// <summary>
        /// 分类会员ID
        /// </summary>
        public string mfMemberId { get; set; }
        /// <summary>
        /// 门店代号
        /// </summary>
        public string shopCode { get; set; }
        /// <summary>
        /// 返回消息
        /// </summary>
        public string message { get; set; }
        /// <summary>
        /// 是否包含错误
        /// </summary>
        public bool hasError { get; set; }
    }
    public class TaskModel
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
        public DateTime starTime { get; set; }
        /// <summary>
        /// 结束时间
        /// </summary>
        public DateTime endTime { get; set; }

        /// <summary>
        /// 重复类型
        /// 1 = 一次
        ///4 = 每日
        ///8 = 每周
        ///16 = 每月
        /// </summary>
        public int freqType { get; set; }
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
        public int alertType { get; set; }
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
        public int importantType { get; set; }
        /// <summary>
        /// 任务描述
        /// </summary>
        public string desc { get; set; }
        /// <summary>
        /// 是否删除
        /// </summary>
        public int isDelete { get; set; }
        /// <summary>
        /// 责任人
        /// </summary>
        public string[] responsibilityUser { get; set; }

        /// <summary>
        /// 参与人
        /// </summary>
        public string[] participateUser { get; set; }
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

        /// <summary>
        /// 是否为全天事件
        /// </summary>
        public int isAll { get; set; }
        /// <summary>
        /// 事件来源
        /// </summary>
        public string taskSource { get; set; }
        /// <summary>
        /// 定位地址(12.2342,112.1232)
        /// </summary>
        public string Location { get; set; }
        /// <summary>
        /// 地址
        /// </summary>
        public string address { get; set; }
    }
    }