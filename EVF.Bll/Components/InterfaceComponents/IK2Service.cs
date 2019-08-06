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
        /// <param name="startWorkflowModel">The start workflow value.</param>
        /// <returns></returns>
        int StartWorkflow(K2Model.StartWorkflowModel startWorkflowModel);
    }
}
