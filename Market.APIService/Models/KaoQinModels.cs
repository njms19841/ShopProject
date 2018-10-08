using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Market.APIService.Models
{
   
    public class KaoQinModel
    {
        
        /// <summary>
        ///1	上班
        ///2	下班
        ///3	培训开始
        ///4	培训结束
        ///5	例会开始
        ///6	例会结束
        /// </summary>
        [Required]
        [Display(Name = "打卡类型")]
        public string KaoQinType { get; set; }

        public DateTime clockTime { get; set; }

        public Double LONGITUDE { get; set; }
        public Double LATITUDE { get; set; }

        public string file_id { get; set; }
        

    }
    public class LeaveModel
    {

        /// <summary>
        ///1	产假
        ///2	病假
        ///3	事假
        ///4	婚假
        ///5	丧假
        ///6	护理假
        ///7	年休假
        ///8	休息
        /// </summary>
        [Required]
       // [StringLength(100, ErrorMessage = "{0} 必须至少包含 {2} 个字符。", MinimumLength = 6)]
       // [DataType(DataType.Password)]
        [Display(Name = "请假类型")]
        public string LeaveType { get; set; }
        /// <summary>
        /// 1 全天
        /// 2 上午
        /// 3 下午
        /// </summary>
        [Required]
        [Display(Name = "时间范围")]
        public string DayType { get; set; }
        /// <summary>
        /// 请假原因
        /// </summary>
        [Required]
        [Display(Name = "请假原因")]
        [StringLength(100, ErrorMessage = "{0} 必须至少包含 {2} 个字符。", MinimumLength = 6)]
        public string Desc { get; set; }

        /// <summary>
        /// 请假日期
        /// </summary>
        [Required]
        [Display(Name = "请假日期")]
        public DateTime Day { get; set; }

        public DateTime StartDateTime { get; set; }
        public DateTime EndDateTime { get; set; }


    }
}