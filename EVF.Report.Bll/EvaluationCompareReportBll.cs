using EVF.Data.Pocos;
using EVF.Data.Repository.Interfaces;
using EVF.Evaluation.Bll.Interfaces;
using EVF.Evaluation.Bll.Models;
using EVF.Helper;
using EVF.Helper.Components;
using EVF.Helper.Interfaces;
using EVF.Report.Bll.Interfaces;
using EVF.Report.Bll.Models;
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
    public class EvaluationCompareReportBll : IEvaluationCompareReportBll
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
        /// Initializes a new instance of the <see cref="EvaluationCompareReportBll" /> class.
        /// </summary>
        /// <param name="unitOfWork">The utilities unit of work.</param>
        /// <param name="token">The ClaimsIdentity in token management.</param>
        public EvaluationCompareReportBll(IUnitOfWork unitOfWork, ISummaryEvaluationBll summaryEvaluation, IManageToken token)
        {
            _unitOfWork = unitOfWork;
            _summaryEvaluation = summaryEvaluation;
            _token = token;
        }

        #endregion

        #region [Methods]

        /// <summary>
        /// Export vendor evaluation status excel report.
        /// </summary>
        /// <param name="model">The filter criteria value.</param>
        public ResponseFileModel ExportEvaluationCompareReport(EvaluationSummaryReportRequestModel model)
        {
            var dataList = this.GetDataCollection(model);
            var summaryList = this.GetSummarys(dataList);
            return this.ExportExcel(dataList, summaryList, model);
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
        /// <param name="model">The filter criteria value.</param>
        private ResponseFileModel ExportExcel(IEnumerable<Data.Pocos.Evaluation> evaluationList,
                                 IEnumerable<SummaryEvaluationViewModel> summaryList,
                                 EvaluationSummaryReportRequestModel model)
        {
            var result = new ResponseFileModel();
            string sheetName = "รายงานเปรียบเทียบการประเมิน";

            using (var memoryStream = new MemoryStream())
            {

                IWorkbook workbook = new XSSFWorkbook();

                ISheet sheet1 = workbook.CreateSheet(sheetName);

                sheet1.AddMergedRegion(new CellRangeAddress(2, 2, 0, 7));
                sheet1.AddMergedRegion(new CellRangeAddress(3, 3, 0, 3));

                var periodItemList = _unitOfWork.GetRepository<PeriodItem>().GetCache();
                int rowIndex = 2;

                this.GenerateTopicReport(workbook, sheet1, ref rowIndex, model);
                this.GenerateHeaderTable(workbook, sheet1, ref rowIndex);

                //foreach (var item in evaluationList)
                //{
                //    var summary = summaryList.FirstOrDefault(x => x.Id == item.Id);
                //    var periodItem = periodItemList.FirstOrDefault(x => x.Id == item.PeriodItemId.Value);
                    

                //    //Merge header table.
                //    sheet1.AddMergedRegion(new CellRangeAddress(rowIndex - 1, rowIndex, 0, 0));
                //    sheet1.AddMergedRegion(new CellRangeAddress(rowIndex - 1, rowIndex, 1, 1));
                //    sheet1.AddMergedRegion(new CellRangeAddress(rowIndex - 1, rowIndex, 2, 3));
                //    sheet1.AddMergedRegion(new CellRangeAddress(rowIndex - 1, rowIndex, 4, 4));
                //    sheet1.AddMergedRegion(new CellRangeAddress(rowIndex - 1, rowIndex, 5, 5));
                //    sheet1.AddMergedRegion(new CellRangeAddress(rowIndex - 1, rowIndex, 6, 6));
                //    sheet1.AddMergedRegion(new CellRangeAddress(rowIndex - 1, rowIndex, 7, 7));
                //    sheet1.AddMergedRegion(new CellRangeAddress(rowIndex - 1, rowIndex, 8, 8));

                //    this.GenerateContentTable(workbook, sheet1, summary, item, ref rowIndex, periodItem, empList);
                //    rowIndex++;
                //}

                workbook.Write(memoryStream);

                result.FileContent = memoryStream.ToArray();
                result.FileName = $"EvaluationCompare_{DateTime.Now.ToString("yyyyMMddHHmmss")}.xlsx";
            }
            return result;
        }

        /// <summary>
        /// Generate topic report.
        /// </summary>
        /// <param name="workbook">The npoi workbook interface.</param>
        /// <param name="sheet1">The npoi sheet interface.</param>
        /// <param name="rowIndex">The row target index.</param>
        /// <param name="model">The filter criteria value.</param>
        private void GenerateTopicReport(IWorkbook workbook, ISheet sheet1, ref int rowIndex,
                                         EvaluationSummaryReportRequestModel model)
        {
            IRow topicRow = sheet1.CreateRow(rowIndex);
            ExcelService.CreateTopicCell(workbook, sheet1, topicRow, 0, $"รายงานเปรียบเทียบการประเมิน - วันที่พิมพ์ {UtilityService.DateTimeToStringTH(DateTime.Now, "dd MMM yyyy")}");
            rowIndex = rowIndex + 1;
            IRow criteriaRow = sheet1.CreateRow(3);
            criteriaRow.Height = 3000;
            ExcelService.CreateCriteriaCell(workbook, sheet1, criteriaRow, 0, $"{this.GenerateCriteria(model)}");
            ExcelService.SetCellCriteriaStyle(workbook, criteriaRow, 1, 3);
            rowIndex = rowIndex + 2;
        }

        /// <summary>
        /// Generate header table.
        /// </summary>
        /// <param name="workbook">The npoi workbook interface.</param>
        /// <param name="sheet1">The npoi sheet interface.</param>
        /// <param name="rowIndex">The row target index.</param>
        private void GenerateHeaderTable(IWorkbook workbook, ISheet sheet1,
                                         ref int rowIndex)
        {
            string[] mainHeaders = new string[]
            {
                    "รหัสผู้ขาย",
                    "ชื่อผู้ขาย",
                    "ประเภทผู้ขาย",
                    "ชื่อประเภทผู้ขาย",
                    "บริษัท",
                    "รหัสกลุ่มจัดซื้อ",
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

            rowIndex++;
            IRow headerRow2 = sheet1.CreateRow(rowIndex);
            ExcelService.SetCellContentStyle(workbook, headerRow2, 0, cellHeaderIndex - 1);
            

            rowIndex++;
            IRow headerRow3 = sheet1.CreateRow(rowIndex);
            ExcelService.SetCellContentStyle(workbook, headerRow3, 0, cellHeaderIndex - 1);

            this.MergeHeader(sheet1, rowIndex);
        }

        private void MergeHeader(ISheet sheet1, int rowIndex)
        {
            sheet1.AddMergedRegion(new CellRangeAddress(rowIndex - 2, rowIndex, 0, 0));
            sheet1.AddMergedRegion(new CellRangeAddress(rowIndex - 2, rowIndex, 1, 1));
            sheet1.AddMergedRegion(new CellRangeAddress(rowIndex - 2, rowIndex, 2, 2));
            sheet1.AddMergedRegion(new CellRangeAddress(rowIndex - 2, rowIndex, 3, 3));
            sheet1.AddMergedRegion(new CellRangeAddress(rowIndex - 2, rowIndex, 4, 4));
            sheet1.AddMergedRegion(new CellRangeAddress(rowIndex - 2, rowIndex, 5, 5));
        }

        /// <summary>
        /// Generate criteria selected information in report.
        /// </summary>
        /// <param name="model">The filter criteria value.</param>
        /// <returns></returns>
        private string GenerateCriteria(EvaluationSummaryReportRequestModel model)
        {
            StringBuilder stringBuilder = new StringBuilder();
            stringBuilder.AppendLine($"   Criteria ที่เลือก");
            stringBuilder.AppendLine($"ปี : {this.GetYear(model.PeriodId)}");
            stringBuilder.AppendLine($"รอบ : {this.GetPeriodItemName(model.PeriodItemId)}");
            stringBuilder.AppendLine($"บริษัท : {this.GetCompanyName(model.ComCode)}");
            stringBuilder.AppendLine($"กลุ่มจัดซื้อ : {this.GetPurchaseName(model.PurchaseOrg)}");
            stringBuilder.AppendLine($"ประเภทผู้ขาย : {this.GetWeightingKey(model.WeightingKey)}");
            return stringBuilder.ToString();
        }

        /// <summary>
        /// Get year selected.
        /// </summary>
        /// <param name="periodId">The period identity.</param>
        /// <returns></returns>
        private string GetYear(int? periodId)
        {
            string result = "";
            if (periodId.HasValue)
            {
                result = _unitOfWork.GetRepository<Period>().GetCache(x => x.Id == periodId.Value).FirstOrDefault().Year.ToString();
            }
            return result;
        }

        /// <summary>
        /// Get period item name.
        /// </summary>
        /// <param name="periodItemId">The period item identity.</param>
        /// <returns></returns>
        private string GetPeriodItemName(int? periodItemId)
        {
            string result = "";
            if (periodItemId.HasValue)
            {
                result = _unitOfWork.GetRepository<PeriodItem>().GetCache(x => x.Id == periodItemId.Value).FirstOrDefault().PeriodName;
            }
            return result;
        }

        /// <summary>
        /// Get company name.
        /// </summary>
        /// <param name="comCode">The company code identity.</param>
        /// <returns></returns>
        private string GetCompanyName(string comCode)
        {
            string result = "";
            if (!string.IsNullOrEmpty(comCode))
            {
                result = _unitOfWork.GetRepository<Hrcompany>().GetCache(x => x.SapcomCode == comCode).FirstOrDefault().LongText;
            }
            return result;
        }

        /// <summary>
        /// Get purchase org name.
        /// </summary>
        /// <param name="purchaseOrg">The purchase org code identity.</param>
        /// <returns></returns>
        private string GetPurchaseName(string purchaseOrg)
        {
            string result = "";
            if (!string.IsNullOrEmpty(purchaseOrg))
            {
                result = _unitOfWork.GetRepository<PurchaseOrg>().GetCache(x => x.PurchaseOrg1 == purchaseOrg).FirstOrDefault().PurchaseName;
            }
            return result;
        }

        /// <summary>
        /// Get weigthing key name.
        /// </summary>
        /// <param name="valueKey">The weighting key value key.</param>
        /// <returns></returns>
        private string GetWeightingKey(string valueKey)
        {
            string result = "";
            if (!string.IsNullOrEmpty(valueKey))
            {
                result = _unitOfWork.GetRepository<ValueHelp>().GetCache(x => x.ValueType == ConstantValue.ValueTypeWeightingKey &&
                                                                              x.ValueKey == valueKey).FirstOrDefault().ValueText;
            }
            return result;
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
                                          SummaryEvaluationViewModel summary,
                                          Data.Pocos.Evaluation evaluation,
                                          ref int rowIndex,
                                          PeriodItem periodItem, IEnumerable<Hremployee> empList)
        {
            rowIndex++;
            IRow contentRow = sheet1.CreateRow(rowIndex);
            int cellContentIndex = 0;
            string[] mainContent = new string[]
            {
                        evaluation.DocNo,
                        evaluation.ComCode,
                        UtilityService.DateTimeToString(periodItem.StartEvaDate.Value,"dd.MM.yyyy"),
                        UtilityService.DateTimeToString(periodItem.EndEvaDate.Value,"dd.MM.yyyy"),
                        periodItem.PeriodName,
                        evaluation.VendorNo,
                        summary.VendorName,
                        summary.WeightingKey,
                        summary.PurchasingOrgName,
            };

            foreach (var content in mainContent)
            {
                ExcelService.CreateContentCell(workbook, sheet1, contentRow, cellContentIndex, content);
                cellContentIndex++;
            }

            rowIndex += 2;

           

        }
        

        /// <summary>
        /// Generate header table score.
        /// </summary>
        /// <param name="workbook">The npoi workbook interface.</param>
        /// <param name="sheet1">The npoi sheet interface.</param>
        /// <param name="rowIndex">The row target index.</param>
        /// <param name="summary">The evaluation summary.</param>
        private void GenerateScoreHeader(IWorkbook workbook, ISheet sheet1, ref int rowIndex,
                                         SummaryEvaluationViewModel summary)
        {
            this.GenerateSubHeaderTable(workbook, sheet1, ref rowIndex);
            IRow subContentRow = sheet1.CreateRow(rowIndex);
            int cellSubContentIndex = 3;
            string[] subContent = new string[]
            {
                "คะแนนรวม",
                summary.Total.ToString(),
                "100",
                summary.GradeName,
                ""
            };
            sheet1.AddMergedRegion(new CellRangeAddress(rowIndex, rowIndex, 6, 7));
            foreach (var content in subContent)
            {
                ExcelService.CreateContentCell(workbook, sheet1, subContentRow, cellSubContentIndex, content);
                cellSubContentIndex++;
            }
            rowIndex += 1;
            IRow topicUserContent = sheet1.CreateRow(rowIndex);
            ExcelService.CreateContentCellNoBorder(workbook, sheet1, topicUserContent, 1, "คะแนนผู้ประเมิน");
            sheet1.AddMergedRegion(new CellRangeAddress(rowIndex, rowIndex, 1, 2));
            rowIndex += 1;
        }

        /// <summary>
        /// Get evaluation score kpi or kpi group.
        /// </summary>
        /// <param name="evaLogs">The evaluation log collection.</param>
        /// <param name="kpiGroupId">The kpi group identity.</param>
        /// <param name="kpiId">The kpi identity.</param>
        /// <returns></returns>
        private string GetScore(UserEvaluationDetailViewModel evaLogs,
                                int kpiGroupId, int kpiId)
        {
            string score = "";
            if (evaLogs != null)
            {
                var evaLog = evaLogs.EvaluationLogs.FirstOrDefault(x => x.KpiGroupId == kpiGroupId && x.KpiId == kpiId);
                if (evaLog != null)
                {
                    score = evaLog.Score.Value.ToString();
                }
            }
            return score;
        }

        /// <summary>
        /// Get max score kpi or kpi group.
        /// </summary>
        /// <param name="weigthingKey">The weighting key condition max score.</param>
        /// <param name="maxScore">The default max score.</param>
        /// <returns></returns>
        private string GetMaxScore(string weigthingKey, int maxScore)
        {
            string result = "100";
            if (!string.Equals("A2", weigthingKey, StringComparison.OrdinalIgnoreCase))
            {
                result = maxScore.ToString();
            }
            return result;
        }

        /// <summary>
        /// Generate sub header table in report.
        /// </summary>
        /// <param name="workbook">The npoi workbook interface.</param>
        /// <param name="sheet1">The npoi sheet interface.</param>
        /// <param name="rowIndex">The row target index.</param>
        private void GenerateSubHeaderTable(IWorkbook workbook, ISheet sheet1,
                                         ref int rowIndex)
        {
            string[] mainHeaders = new string[]
            {
                    "คะแนนที่ได้",
                    "คะแนนเต็ม",
                    "เกณฑ์การประเมินที่ได้",
                    "เกณฑ์การประเมินที่ได้"
            };
            sheet1.AddMergedRegion(new CellRangeAddress(rowIndex, rowIndex, 6, 7));
            IRow headerRow = sheet1.CreateRow(rowIndex);
            int cellHeaderIndex = 4;
            foreach (var item in mainHeaders)
            {
                ExcelService.SetCellHeaderStyle(workbook, headerRow, cellHeaderIndex);
                ExcelService.CreateHeaderCell(workbook, sheet1, headerRow, cellHeaderIndex, item);
                cellHeaderIndex++;
            }
            rowIndex++;
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
