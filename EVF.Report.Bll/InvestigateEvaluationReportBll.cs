using EVF.Data.Pocos;
using EVF.Data.Repository.Interfaces;
using EVF.Evaluation.Bll.Interfaces;
using EVF.Evaluation.Bll.Models;
using EVF.Helper;
using EVF.Helper.Components;
using EVF.Helper.Interfaces;
using EVF.Report.Bll.Interfaces;
using EVF.Report.Bll.Models;
using NPOI.HSSF.Util;
using NPOI.SS.UserModel;
using NPOI.SS.Util;
using NPOI.XSSF.UserModel;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Linq.Expressions;
using System.Text;

namespace EVF.Report.Bll
{
    public class InvestigateEvaluationReportBll : IEvaluationSummaryReportBll
    {

        #region [Fields]

        /// <summary>
        /// The utilities unit of work for manipulating utilities data in database.
        /// </summary>
        private readonly IUnitOfWork _unitOfWork;
        /// <summary>
        /// The summary evaluation manager provides summary evaluation functionality.
        /// </summary>
        private readonly ISummaryEvaluationBll _summaryEvaluation;
        /// <summary>
        /// The ClaimsIdentity in token management.
        /// </summary>
        private readonly IManageToken _token;

        #endregion

        #region [Constructors]

        /// <summary>
        /// Initializes a new instance of the <see cref="InvestigateEvaluationReportBll" /> class.
        /// </summary>
        /// <param name="unitOfWork">The utilities unit of work.</param>
        /// <param name="token">The ClaimsIdentity in token management.</param>
        public InvestigateEvaluationReportBll(IUnitOfWork unitOfWork, ISummaryEvaluationBll summaryEvaluation, IManageToken token)
        {
            _unitOfWork = unitOfWork;
            _summaryEvaluation = summaryEvaluation;
            _token = token;
        }

        #endregion

        #region [Methods]

        /// <summary>
        /// Export summary evaluation excel report.
        /// </summary>
        /// <param name="model">The filter criteria value.</param>
        public ResponseFileModel ExportSummaryReport(EvaluationSummaryReportRequestModel model)
        {
            var dataList = this.GetDataCollection(model);
            var summaryList = this.GetSummarys(dataList);
            return this.ExportExcel(dataList, summaryList);
        }

        /// <summary>
        /// Get evaluation data.
        /// </summary>
        /// <param name="model">The filter criteria value.</param>
        /// <returns></returns>
        private IEnumerable<Data.Pocos.Evaluation> GetDataCollection(EvaluationSummaryReportRequestModel model)
        {
            var whereClause = this.BuildDynamicWhereClause(model);
            return _unitOfWork.GetRepository<Data.Pocos.Evaluation>().Get(whereClause);
        }

        /// <summary>
        /// Get summary evaluation data.
        /// </summary>
        /// <param name="evaluationList">The evaluation collection.</param>
        /// <returns></returns>
        private IEnumerable<SummaryEvaluationViewModel> GetSummarys(IEnumerable<Data.Pocos.Evaluation> evaluationList)
        {
            var summaryList = new List<SummaryEvaluationViewModel>();
            foreach (var evaluation in evaluationList)
            {
                var summary = _summaryEvaluation.GetDetail(evaluation.Id);
                summaryList.Add(summary);
            }
            return summaryList;
        }

        /// <summary>
        /// Export evaluation summary stat to excel report.
        /// </summary>
        /// <param name="evaluationList">The evaluation collection.</param>
        /// <param name="summaryList">The summary evaluation collection.</param>
        private ResponseFileModel ExportExcel(IEnumerable<Data.Pocos.Evaluation> evaluationList,
                                 IEnumerable<SummaryEvaluationViewModel> summaryList)
        {
            var result = new ResponseFileModel();
            int maxCountUser = summaryList.Select(x => x.UserLists.Count).Max();
            string sheetName = "รายงานตรวจสอบสถานะการประเมิน";

            using (var memoryStream = new MemoryStream())
            {

                IWorkbook workbook = new XSSFWorkbook();

                ISheet sheet1 = workbook.CreateSheet(sheetName);

                sheet1.AddMergedRegion(new CellRangeAddress(3, 3, 0, 7));
                sheet1.AddMergedRegion(new CellRangeAddress(5, 6, 0, 0));
                sheet1.AddMergedRegion(new CellRangeAddress(5, 6, 1, 1));
                sheet1.AddMergedRegion(new CellRangeAddress(5, 6, 2, 3));
                sheet1.AddMergedRegion(new CellRangeAddress(5, 6, 4, 4));
                sheet1.AddMergedRegion(new CellRangeAddress(5, 6, 5, 5));
                sheet1.AddMergedRegion(new CellRangeAddress(5, 6, 6, 6));
                sheet1.AddMergedRegion(new CellRangeAddress(5, 6, 7, 7));
                sheet1.AddMergedRegion(new CellRangeAddress(5, 6, 8, 8));
                sheet1.AddMergedRegion(new CellRangeAddress(5, 6, 9, 9));

                int rowIndex = 3;
                int cellHeaderIndex = this.GenerateHeaderTable(workbook, sheet1, ref rowIndex, maxCountUser);
                this.GenerateContentTable(workbook, sheet1, summaryList, evaluationList, rowIndex, cellHeaderIndex);
                
                workbook.Write(memoryStream);

                result.FileContent = memoryStream.ToArray();
                result.FileName = $"EvaluationSummary_{DateTime.Now.ToString("yyyyMMddHHmmss")}.xlsx";
            }
            return result;
        }

        /// <summary>
        /// Generate header table.
        /// </summary>
        /// <param name="workbook">The npoi workbook interface.</param>
        /// <param name="sheet1">The npoi sheet interface.</param>
        /// <param name="rowIndex">The row target index.</param>
        /// <param name="maxCountUser">The max count user for generate dynamic header column.</param>
        /// <returns></returns>
        private int GenerateHeaderTable(IWorkbook workbook,ISheet sheet1, 
                                         ref int rowIndex, int maxCountUser)
        {
            IRow topicRow = sheet1.CreateRow(rowIndex);
            ExcelService.CreateTopicCell(workbook, sheet1, topicRow, 0, "รายงานตรวจสอบสถานะการประเมิน");
            rowIndex = rowIndex + 2;
            string[] mainHeaders = new string[]
            {
                    "เลขที่ใบประเมิน",
                    "บริษัท",
                    "รอบการประเมิน",
                    "รอบการประเมิน",
                    "รหัสผู้ขาย",
                    "ชื่อผู้ขาย",
                    "ชื่อประเภทผู้ขาย",
                    "ชื่อกลุ่มจัดซื้อ",
                    "จำนวนผู้ประเมิน",
                    "ประเมินแล้วเสร็จ",
            };
            IRow headerRow = sheet1.CreateRow(rowIndex);
            headerRow.Height = 500;
            int cellHeaderIndex = 0;
            foreach (var item in mainHeaders)
            {
                ExcelService.SetCellHeaderStyle(workbook, headerRow, cellHeaderIndex);
                ExcelService.CreateHeaderCell(workbook, sheet1, headerRow, cellHeaderIndex, item);
                cellHeaderIndex++;
            }

            string[] userHeaders = new string[]
            {
                    "ผู้ประเมิน",
                    "สถานะ",
                    "เหตุผล"
            };

            for (int i = 0; i < maxCountUser; i++)
            {
                foreach (var item in userHeaders)
                {
                    ExcelService.SetCellHeaderStyle(workbook, headerRow, cellHeaderIndex);
                    ExcelService.CreateHeaderCell(workbook, sheet1, headerRow, cellHeaderIndex, item);
                    sheet1.AddMergedRegion(new CellRangeAddress(5, 6, cellHeaderIndex, cellHeaderIndex));
                    cellHeaderIndex++;
                }
            }
            rowIndex++;
            IRow headerRow2 = sheet1.CreateRow(rowIndex);
            ExcelService.SetCellContentStyle(workbook, headerRow2, 0, cellHeaderIndex - 1);
            return cellHeaderIndex;
        }

        /// <summary>
        /// Get evaluation status text.
        /// </summary>
        /// <param name="isAction">The evaluation action flag.</param>
        /// <param name="isReject">The evaluation reject flag.</param>
        /// <returns></returns>
        private string GetEvaStatus(bool isAction, bool isReject)
        {
            string status = "ไม่ประเมิน";
            if (isAction)
            {
                status = "ประเมิน";
            }
            if (isReject)
            {
                status = "ไม่ประสงค์ประเมิน";
            }
            return status;
        }

        /// <summary>
        /// Generate main content table.
        /// </summary>
        /// <param name="workbook">The npoi workbook interface.</param>
        /// <param name="sheet1">The npoi sheet interface.</param>
        /// <param name="summaryList">The evaluation summary result. </param>
        /// <param name="evaluationList">The evaluation data collection.</param>
        /// <param name="rowIndex">The row data index.</param>
        /// <param name="cellHeaderIndex">The max cell table generate.</param>
        private void GenerateContentTable(IWorkbook workbook, ISheet sheet1,
                                          IEnumerable<SummaryEvaluationViewModel> summaryList,
                                          IEnumerable<Data.Pocos.Evaluation> evaluationList, 
                                          int rowIndex, int cellHeaderIndex)
        {
            var periodItemList = _unitOfWork.GetRepository<PeriodItem>().GetCache();
            foreach (var item in evaluationList)
            {
                rowIndex++;
                var periodItem = periodItemList.FirstOrDefault(x => x.Id == item.PeriodItemId.Value);
                IRow contentRow = sheet1.CreateRow(rowIndex);
                int cellContentIndex = 0;
                var summary = summaryList.FirstOrDefault(x => x.Id == item.Id);
                string[] mainContent = new string[]
                {
                        item.DocNo,
                        item.ComCode,
                        UtilityService.DateTimeToString(periodItem.StartEvaDate.Value,"dd.MM.yyyy"),
                        UtilityService.DateTimeToString(periodItem.EndEvaDate.Value,"dd.MM.yyyy"),
                        item.VendorNo,
                        summary.VendorName,
                        summary.WeightingKey,
                        summary.PurchasingOrgName,
                        summary.UserLists.Count.ToString(),
                        summary.UserLists.Where(x=>x.IsAction).Count().ToString(),
                };

                foreach (var content in mainContent)
                {
                    ExcelService.CreateContentCell(workbook, sheet1, contentRow, cellContentIndex, content);
                    cellContentIndex++;
                }

                this.GenerateUserCellContent(summary, ref cellContentIndex, workbook, sheet1, contentRow);

                if (cellContentIndex < cellHeaderIndex)
                {
                    ExcelService.SetCellContentStyle(workbook, contentRow, cellContentIndex, cellHeaderIndex - 1);
                }
            }
        }

        /// <summary>
        /// Generate user cell content data.
        /// </summary>
        /// <param name="summary">The evaluation summary result.</param>
        /// <param name="cellContentIndex">The target cell content index.</param>
        /// <param name="workbook">The npoi workbook interface.</param>
        /// <param name="sheet1">The npoi sheet interface.</param>
        /// <param name="contentRow">The npoi content row interface.</param>
        private void GenerateUserCellContent(SummaryEvaluationViewModel summary, ref int cellContentIndex,
                                             IWorkbook workbook, ISheet sheet1, IRow contentRow)
        {
            foreach (var user in summary.UserLists)
            {
                string[] content = new string[]
                {
                            user.FullName,
                            this.GetEvaStatus(user.IsAction,user.IsReject),
                            user.ReasonReject
                };
                foreach (var userContent in content)
                {
                    ExcelService.CreateContentCell(workbook, sheet1, contentRow, cellContentIndex, userContent);
                    cellContentIndex++;
                }
            }
        }

        /// <summary>
        /// Build dynamic where clause with criteria value.
        /// </summary>
        /// <param name="model">The criteria value.</param>
        /// <returns></returns>
        private Expression<Func<Data.Pocos.Evaluation, bool>> BuildDynamicWhereClause(EvaluationSummaryReportRequestModel model)
        {
            // simple method to dynamically plugin a where clause
            var predicate = PredicateBuilder.True<Data.Pocos.Evaluation>(); // true -where(true) return all
            if (!string.IsNullOrEmpty(model.ComCode))
            {
                predicate = predicate.And(s => s.ComCode == model.ComCode);
            }
            if (!string.IsNullOrEmpty(model.PurchaseOrg))
            {
                predicate = predicate.And(s => s.PurchasingOrg == model.PurchaseOrg);
            }
            if (!string.IsNullOrEmpty(model.WeightingKey))
            {
                predicate = predicate.And(s => s.WeightingKey == model.WeightingKey);
            }

            if (model.PeriodItemId.HasValue)
            {
                predicate = predicate.And(s => s.PeriodItemId == model.PeriodItemId);
            }
            else if (model.PeriodId.HasValue)
            {
                var periodItemIds = _unitOfWork.GetRepository<PeriodItem>().GetCache(x => x.PeriodId == model.PeriodId).Select(x => x.Id).ToArray();
                predicate = predicate.And(s => periodItemIds.Contains(s.PeriodItemId.Value));
            }

            return predicate;
        }

        #endregion

    }
}
