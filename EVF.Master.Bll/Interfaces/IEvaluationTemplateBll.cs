using EVF.Helper.Models;
using EVF.Master.Bll.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace EVF.Master.Bll.Interfaces
{
    public interface IEvaluationTemplateBll
    {
        /// <summary>
        /// Get Evaluation template list.
        /// </summary>
        /// <returns></returns>
        IEnumerable<EvaluationTemplateViewModel> GetList();
        /// <summary>
        /// Get Detail of Evaluation Template.
        /// </summary>
        /// <param name="id">The identity of evaluation template.</param>
        /// <returns></returns>
        EvaluationTemplateViewModel GetDetail(int id);
        /// <summary>
        /// Validate information value in evaluation template logic.
        /// </summary>
        /// <param name="model">The evaluation template information value.</param>
        /// <returns></returns>
        ResultViewModel ValidateData(EvaluationTemplateViewModel model);
        /// <summary>
        /// Insert new evaluation template.
        /// </summary>
        /// <param name="model">The evaluation template information value.</param>
        /// <returns></returns>
        ResultViewModel Save(EvaluationTemplateViewModel model);
        /// <summary>
        /// Update evaluation template group.
        /// </summary>
        /// <param name="model">The evaluation template information value.</param>
        /// <returns></returns>
        ResultViewModel Edit(EvaluationTemplateViewModel model);
        /// <summary>
        /// Remove evaluation template.
        /// </summary>
        /// <param name="id">The identity of evaluation template.</param>
        /// <returns></returns>
        ResultViewModel Delete(int id);
        /// <summary>
        /// Get evaluation template for display.
        /// </summary>
        /// <param name="id">The identity of evaluation template.</param>
        /// <returns></returns>
        EvaluationTemplateDisplayViewModel LoadTemplate(int id);
    }
}
