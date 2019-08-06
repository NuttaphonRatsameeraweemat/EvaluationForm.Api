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
    }
}
