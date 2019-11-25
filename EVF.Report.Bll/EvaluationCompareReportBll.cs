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
        public ResponseFileModel ExportEvaluationCompareReport(EvaluationCompareReportRequestModel model)
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
        private IEnumerable<Data.Pocos.Evaluation> GetDataCollection(EvaluationCompareReportRequestModel model)
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
                                 EvaluationCompareReportRequestModel model)
        {
            var result = new ResponseFileModel();
            string sheetName = "รายงานเปรียบเทียบการประเมิน";

            using (var memoryStream = new MemoryStream())
            {
                IWorkbook workbook = new XSSFWorkbook();
                ISheet sheet1 = workbook.CreateSheet(sheetName);

                sheet1.AddMergedRegion(new CellRangeAddress(2, 2, 0, 7));
                sheet1.AddMergedRegion(new CellRangeAddress(3, 3, 0, 3));

                int[] periodItemIds = this.GetPeriodByCriteria(model);
                int rowIndex = 2;

                this.GenerateTopicReport(workbook, sheet1, ref rowIndex, model);
                this.GenerateHeaderTable(workbook, sheet1, ref rowIndex);
                this.GeneratePeriodHeaderTable(workbook, sheet1, periodItemIds);
                this.GenerateContent(workbook, sheet1, rowIndex, periodItemIds, evaluationList, summaryList);

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
                                         EvaluationCompareReportRequestModel model)
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

        /// <summary>
        /// Generate period header table.
        /// </summary>
        /// <param name="workbook">The npoi workbook interface.</param>
        /// <param name="sheet1">The npoi sheet interface.</param>
        /// <param name="rowIndex">The row target index.</param>
        private void GeneratePeriodHeaderTable(IWorkbook workbook, ISheet sheet1,
                                               int[] periodItemIds)
        {
            int startCellPeriod = 6;
            int startRowPeriod = 5;
            var periodItemList = _unitOfWork.GetRepository<PeriodItem>().GetCache(x => periodItemIds.Contains(x.Id),
                                                                                  orderBy: x => x.OrderBy(y => y.StartEvaDate));
            foreach (var item in periodItemList)
            {
                ExcelService.SetCellHeaderStyle(workbook, sheet1.GetRow(startRowPeriod), startCellPeriod, startCellPeriod + 2);
                ExcelService.SetCellHeaderStyle(workbook, sheet1.GetRow(startRowPeriod + 1), startCellPeriod, startCellPeriod + 2);
                ExcelService.SetCellHeaderStyle(workbook, sheet1.GetRow(startRowPeriod + 2), startCellPeriod, startCellPeriod + 2);
                ExcelService.CreateHeaderCell(workbook, sheet1, sheet1.GetRow(startRowPeriod), startCellPeriod, $"รอบการประเมิน {item.PeriodName}");
                ExcelService.CreateHeaderCell(workbook, sheet1, sheet1.GetRow(startRowPeriod + 1), startCellPeriod,
                                              this.GetPeriodDisplayTh(item.StartEvaDate.Value, item.EndEvaDate.Value));
                ExcelService.CreateHeaderCell(workbook, sheet1, sheet1.GetRow(startRowPeriod + 2), startCellPeriod, "เกรด");
                ExcelService.CreateHeaderCell(workbook, sheet1, sheet1.GetRow(startRowPeriod + 2), startCellPeriod + 1, "คะแนน");
                ExcelService.CreateHeaderCell(workbook, sheet1, sheet1.GetRow(startRowPeriod + 2), startCellPeriod + 2, "เกณฑ์การประเมิน");

                sheet1.AddMergedRegion(new CellRangeAddress(startRowPeriod, startRowPeriod, startCellPeriod, startCellPeriod + 2));
                sheet1.AddMergedRegion(new CellRangeAddress(startRowPeriod + 1, startRowPeriod + 1, startCellPeriod, startCellPeriod + 2));
                startCellPeriod = startCellPeriod + 3;
            }

        }

        /// <summary>
        /// Get period display header report.
        /// </summary>
        /// <param name="start">The start evaluation date.</param>
        /// <param name="end">The end evaluation date.</param>
        /// <returns></returns>
        private string GetPeriodDisplayTh(DateTime start, DateTime end)
        {
            var cultureInfo = new System.Globalization.CultureInfo("TH-th");
            string startMonth = start.ToString("MMM", cultureInfo);
            string endMonth = end.ToString("MMM", cultureInfo);
            return $"{startMonth} - {endMonth} {end.Year}";
        }

        /// <summary>
        /// Get period by criteria selected.
        /// </summary>
        /// <param name="model">The criteria value.</param>
        /// <returns></returns>
        private int[] GetPeriodByCriteria(EvaluationCompareReportRequestModel model)
        {
            List<int> result = new List<int>();
            if (model.Year != null)
            {
                var periodAll = _unitOfWork.GetRepository<Period>().GetCache(x => model.Year.Contains(x.Year)).Select(x => x.Id);
                var periodItemIds = _unitOfWork.GetRepository<PeriodItem>().GetCache(x => periodAll.Contains(x.PeriodId.Value),
                                                                                          orderBy: x => x.OrderBy(y => y.StartEvaDate)).Select(x => x.Id);
                if (model.PeriodItemId != null)
                {
                    periodItemIds = periodItemIds.Where(x => model.PeriodItemId.Contains(x));
                }
                result.AddRange(periodItemIds);
            }
            else
            {
                result.AddRange(_unitOfWork.GetRepository<PeriodItem>().GetCache(orderBy: x => x.OrderBy(y => y.StartEvaDate)).Select(x => x.Id));
            }
            return result.ToArray();
        }

        /// <summary>
        /// Merge main content header table.
        /// </summary>
        /// <param name="sheet1">The npoi sheet interface.</param>
        /// <param name="rowIndex">The row target index.</param>
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
        private string GenerateCriteria(EvaluationCompareReportRequestModel model)
        {
            StringBuilder stringBuilder = new StringBuilder();
            string year = string.Empty;
            if (model.Year != null)
            {
                year = string.Join(",", model.Year);
            }
            stringBuilder.AppendLine($"   Criteria ที่เลือก");
            stringBuilder.AppendLine($"ปี : {year}");
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
        /// <param name="periodItemIds">The period item identity.</param>
        /// <returns></returns>
        private string GetPeriodItemName(int[] periodItemIds)
        {
            string result = "";
            if (periodItemIds != null)
            {
                var temp = _unitOfWork.GetRepository<PeriodItem>().GetCache(x => periodItemIds.Contains(x.Id)).Select(x => x.PeriodName);
                result = string.Join(",", temp);
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
        /// Generate evaluation compare report content.
        /// </summary>
        /// <param name="workbook">The npoi workbook interface.</param>
        /// <param name="sheet1">The npoi sheet interface.</param>
        /// <param name="rowIndex">The row target index.</param>
        /// <param name="periodItemIds">The period item identitys.</param>
        /// <param name="evaluationList">The evaluation collection.</param>
        /// <param name="summaryList">The summary evaluation collection.</param>
        private void GenerateContent(IWorkbook workbook, ISheet sheet1, int rowIndex, int[] periodItemIds,
                                     IEnumerable<Data.Pocos.Evaluation> evaluationList,
                                     IEnumerable<SummaryEvaluationViewModel> summaryList)
        {
            var groupVendor = evaluationList.Select(x => x.VendorNo).Distinct();
            var vendorList = _unitOfWork.GetRepository<Data.Pocos.Vendor>().GetCache();
            var valueHelpList = _unitOfWork.GetRepository<ValueHelp>().GetCache(x => x.ValueType == ConstantValue.ValueTypeWeightingKey); ;
            foreach (var item in groupVendor)
            {
                rowIndex++;
                IRow contentRow = sheet1.CreateRow(rowIndex);
                int cellMainContent = 0;
                var vendor = vendorList.FirstOrDefault(x => x.VendorNo == item);
                var evaluationGroup = evaluationList.Where(x => x.VendorNo == item);

                var evaluationGroupDistinct = evaluationGroup.Select(x =>
                                                              new { x.WeightingKey, x.ComCode, x.PurchasingOrg }).Distinct();

                ExcelService.CreateContentCell(workbook, sheet1, contentRow, cellMainContent++, item);
                ExcelService.CreateContentCell(workbook, sheet1, contentRow, cellMainContent++, vendor?.VendorName);
                bool isFirst = true;
                foreach (var evaInfo in evaluationGroupDistinct)
                {
                    int cellContent = cellMainContent;
                    var valueHelp = valueHelpList.FirstOrDefault(x => x.ValueKey == evaInfo.WeightingKey);

                    string[] mainContent = new string[]
                    {
                            evaInfo.WeightingKey,
                            valueHelp.ValueText,
                            evaInfo.ComCode,
                            evaInfo.PurchasingOrg
                    };

                    IRow contentRow2 = this.GenerateMainContent(workbook, sheet1, contentRow, mainContent,
                                                                ref isFirst, ref rowIndex, ref cellContent);

                    var tempEvaluationGroup = evaluationGroup.Where(x => x.WeightingKey == evaInfo.WeightingKey &&
                                                                       x.ComCode == evaInfo.ComCode &&
                                                                       x.PurchasingOrg == evaInfo.PurchasingOrg);

                    this.GeneratePeriodContent(workbook, sheet1, contentRow2, cellContent, periodItemIds,
                                               tempEvaluationGroup, summaryList);
                }
            }
        }

        /// <summary>
        /// Generate main content data.
        /// </summary>
        /// <param name="workbook">The npoi workbook interface.</param>
        /// <param name="sheet1">The npoi sheet interface.</param>
        /// <param name="contentRow">The content last row.</param>
        /// <param name="mainContent">The main content generate.</param>
        /// <param name="isFirst">The vendor is first generate row or not.</param>
        /// <param name="rowIndex">The row target index.</param>
        /// <param name="cellContent">The cell content index.</param>
        /// <returns></returns>
        private IRow GenerateMainContent(IWorkbook workbook, ISheet sheet1, IRow contentRow,
                                         string[] mainContent, ref bool isFirst,
                                         ref int rowIndex, ref int cellContent)
        {
            IRow contentRow2 = contentRow;
            if (isFirst)
            {
                foreach (var content in mainContent)
                {
                    ExcelService.CreateContentCell(workbook, sheet1, contentRow2, cellContent++, content);
                }
                isFirst = false;
            }
            else
            {
                rowIndex++;
                contentRow2 = sheet1.CreateRow(rowIndex);
                ExcelService.SetCellContentStyle(workbook, contentRow2, 0, 1);
                foreach (var content in mainContent)
                {
                    ExcelService.CreateContentCell(workbook, sheet1, contentRow2, cellContent++, content);
                }
            }
            return contentRow2;
        }

        /// <summary>
        /// Generate period item content data.
        /// </summary>
        /// <param name="workbook">The npoi workbook interface.</param>
        /// <param name="sheet1">The npoi sheet interface.</param>
        /// <param name="contentRow2">The content row target.</param>
        /// <param name="startCellPeriod">The start cell for generate.</param>
        /// <param name="periodItemIds">The all period item identity.</param>
        /// <param name="tempEvaluationGroup">The evaluation conllection group.</param>
        /// <param name="summaryList">The summary evaluation collection.</param>
        private void GeneratePeriodContent(IWorkbook workbook, ISheet sheet1, IRow contentRow2,
                                           int startCellPeriod, int[] periodItemIds,
                                           IEnumerable<Data.Pocos.Evaluation> tempEvaluationGroup,
                                           IEnumerable<SummaryEvaluationViewModel> summaryList)
        {
            foreach (var periodItemId in periodItemIds)
            {
                ExcelService.SetCellContentStyle(workbook, contentRow2, startCellPeriod, startCellPeriod + 2);
                var evaluation = tempEvaluationGroup.FirstOrDefault(x => x.PeriodItemId == periodItemId);
                if (evaluation != null)
                {
                    var summary = summaryList.FirstOrDefault(x => x.Id == evaluation.Id);
                    ExcelService.CreateContentCell(workbook, sheet1, contentRow2, startCellPeriod, summary.GradeNameEn);
                    ExcelService.CreateContentCell(workbook, sheet1, contentRow2, startCellPeriod + 1, evaluation.TotalScore.Value.ToString());
                    ExcelService.CreateContentCell(workbook, sheet1, contentRow2, startCellPeriod + 2, summary.GradeName);
                }
                startCellPeriod = startCellPeriod + 3;
            }
        }

        /// <summary>
        /// Build dynamic where clause with criteria value.
        /// </summary>
        /// <param name="model">The criteria value.</param>
        /// <returns></returns>
        private Expression<Func<Data.Pocos.Evaluation, bool>> BuildDynamicWhereClause(EvaluationCompareReportRequestModel model)
        {
            // simple method to dynamically plugin a where clause
            var predicate = PredicateBuilder.True<Data.Pocos.Evaluation>(); // true -where(true) return all
            //filter status
            predicate = predicate.And(s => s.Status == ConstantValue.WorkflowStatusApproved);
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
            if (model.Year != null)
            {
                var periodAll = _unitOfWork.GetRepository<Period>().GetCache(x => model.Year.Contains(x.Year)).Select(x => x.Id);
                var periodItemIds = _unitOfWork.GetRepository<PeriodItem>().GetCache(x => periodAll.Contains(x.PeriodId.Value)).Select(x => x.Id);
                if (model.PeriodItemId != null)
                {
                    periodItemIds = periodItemIds.Where(x => model.PeriodItemId.Contains(x));
                }
                predicate = predicate.And(s => periodItemIds.Contains(s.PeriodItemId.Value));
            }
            return predicate;
        }

        #endregion

    }
}
