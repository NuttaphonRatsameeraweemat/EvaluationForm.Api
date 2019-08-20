using System;
using System.Collections.Generic;
using System.Text;

namespace EVF.Helper.Models
{
    /// <summary>
    /// K2 Model integrate K2 Service.
    /// </summary>
    public class K2Model
    {
        /// <summary>
        /// K2 Service Post start workflow.
        /// </summary>
        public class StartWorkflowModel
        {
            public K2ProfileModel K2Connect { get; set; }
            public string ProcessName { get; set; }
            public string Folio { get; set; }
            public Dictionary<string, object> DataFields { get; set; }
        }

        /// <summary>
        /// K2 Service Post action workflow.
        /// </summary>
        public class ActionWorkflowModel
        {
            public K2ProfileModel K2Connect { get; set; }
            public string SerialNumber { get; set; }
            public string Action { get; set; }
            public string AllocatedUser { get; set; }
            public Dictionary<string, object> Datafields { get; set; }
        }

        /// <summary>
        /// K2 Service Post task worklist.
        /// </summary>
        public class WorklistModel
        {
            public K2ProfileModel K2Connect { get; set; }
            public string FromUser { get; set; }
            public string ProcessFolder { get; set; }
        }

        /// <summary>
        /// K2 Service Post Set out of office.
        /// </summary>
        public class SetOutOfOfficeModel
        {
            public K2ProfileModel K2Connect { get; set; }
            public WorkflowDelegateModel WorkflowDelegate { get; set; }
        }

        /// <summary>
        /// K2 Service Delegate information.
        /// </summary>
        public class WorkflowDelegateModel
        {
            public string FromUser { get; set; }
            public string ToUser { get; set; }
            public string Action { get; set; }
            public DateTime? StartDate { get; set; }
            public DateTime? EndDate { get; set; }
        }

        /// <summary>
        /// Response Model from get workflist.
        /// </summary>
        public class TaskListModel
        {
            public TaskListModel()
            {
                DataFields = new Dictionary<string, object>();
            }

            public string AllocatedUser { get; set; }
            public string Folio { get; set; }
            public DateTime StartDate { get; set; }
            public string Folder { get; set; }
            public string Name { get; set; }
            public string FullName { get; set; }
            public string SerialNumber { get; set; }
            public Dictionary<string, object> DataFields { get; set; }
        }

        /// <summary>
        /// Request model get smartobject.
        /// </summary>
        public class SmartObjectModel
        {
            public string SmartObjectName { get; set; }
            public string ExecuteMethodName { get; set; }
        }

    }
}
