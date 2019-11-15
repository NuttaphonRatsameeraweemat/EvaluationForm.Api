using System;
using System.Collections.Generic;
using System.Text;

namespace EVF.Inbox.Bll.Models
{
    public class TaskViewModel
    {
        public TaskViewModel()
        {
            TaskList = new List<TaskListModel>();
            TaskOverView = new List<TaskOverViewModel>();
        }

        public List<TaskListModel> TaskList { get; set; }
        public List<TaskOverViewModel> TaskOverView { get; set; }

    }

    public class TaskListModel
    {
        //Data for post back when action task.
        public string SerialNumber { get; set; }
        public string ProcessCode { get; set; }
        public int Step { get; set; }
        public int DataId { get; set; }

        //Display information
        public string DocNo { get; set; }
        public string ProcessName { get; set; }
        public string VendorName { get; set; }
        public string PurchaseOrgName { get; set; }
        public int TotalScore { get; set; }
        public int GradeId { get; set; }
        public int GradeItemId { get; set; }
        public int EvaluationTemplateId { get; set; }
        public string GradeName { get; set; }
        public string GradeNameEn { get; set; }
        public string WeightingKeyName { get; set; }
        public string PeriodName { get; set; }
        public DateTime ReceiveDate { get; set; }
    }

    public class TaskOverViewModel
    {
        public int GradeItemId { get; set; }
        public string GradeEn { get; set; }
        public string GradeTh { get; set; }
        public string Color { get; set; }
        public int Count { get; set; }
    }

}
