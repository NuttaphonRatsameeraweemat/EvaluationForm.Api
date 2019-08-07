using EVF.Bll.Components.ModelComponents;
using System;
using System.Collections.Generic;
using System.Text;

namespace EVF.Bll.Components.InterfaceComponents
{
    public interface IK2Service
    {
        /// <summary>
        /// Call k2 service start workflow route.
        /// </summary>
        /// <param name="processName">The workflow name to start.</param>
        /// <param name="folio">The task title display in k2.</param>
        /// <param name="dataFields">The datafields value in workflow.</param>
        /// <returns></returns>
        int StartWorkflow(string processName, string folio, Dictionary<string, object> dataFields);
        /// <summary>
        /// Call K2 service action workflow route.
        /// </summary>
        /// <param name="serialNumber">The identity workflow task number.</param>
        /// <param name="action">The action outcome workflow.</param>
        /// <param name="dataFields">The datafields value in workflow</param>
        /// <param name="allocatedUser">The allocated user action.</param>
        /// <returns></returns>
        string ActionWorkflow(string serialNumber, string action, Dictionary<string, object> dataFields, string allocatedUser = "");
        /// <summary>
        /// Call K2 service get workflist from k2.
        /// </summary>
        /// <param name="fromUser"></param>
        /// <returns></returns>
        IEnumerable<K2Model.TaskListModel> GetWorkList(string fromUser);
        /// <summary>
        /// Call K2 service set out of office user.
        /// </summary>
        /// <param name="fromUser">The delegate task from user.</param>
        /// <param name="toUser">The delegate task to user.</param>
        /// <param name="action">The action out of office (create, edit or delete)</param>
        /// <param name="startDate">The startdate of delegate task.</param>
        /// <param name="endDate">The enddate of delegate task.</param>
        /// <returns></returns>
        string SetOutofOffice(string fromUser, string toUser, string action, DateTime startDate, DateTime endDate);
        /// <summary>
        /// Execute Smartobject from k2.
        /// </summary>
        /// <param name="smartObjectName">The smartobject name.</param>
        /// <param name="methodName">The method name to execute.</param>
        /// <returns></returns>
        List<Dictionary<string, object>> ExecuteSmartObject(string smartObjectName, string methodName);
    }
}
