﻿using AutoMapper;
using EVF.Data.Pocos;
using EVF.Data.Repository.Interfaces;
using EVF.Email.Bll.Interfaces;
using EVF.Evaluation.Bll.Interfaces;
using EVF.Evaluation.Bll.Models;
using EVF.Helper;
using EVF.Helper.Components;
using EVF.Helper.Interfaces;
using EVF.Helper.Models;
using EVF.Report.Bll.Interfaces;
using EVF.Report.Bll.Models;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Transactions;

namespace EVF.Report.Bll
{
    public class VendorEvaluationReportBll : IVendorEvaluationReportBll
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
        /// The summary evaluation manager provides summary evaluation functionality.
        /// </summary>
        private readonly IReportService _reportService;
        /// <summary>
        /// The email task provides email task functionality.
        /// </summary>
        private readonly IEmailTaskBll _emailTask;
        /// <summary>
        /// The email service provides email service functionality.
        /// </summary>
        private readonly IEmailService _emailService;
        /// <summary>
        /// The ClaimsIdentity in token management.
        /// </summary>
        private readonly IManageToken _token;
        /// <summary>
        /// White space for new line.
        /// </summary>
        private const string leftSpace = "              ";

        #endregion

        #region [Constructors]

        /// <summary>
        /// Initializes a new instance of the <see cref="VendorEvaluationReportBll" /> class.
        /// </summary>
        /// <param name="unitOfWork">The utilities unit of work.</param>
        /// <param name="summaryEvaluation">The summary evaluation manager provides summary evaluation functionality.</param>
        /// <param name="reportService">The summary evaluation manager provides summary evaluation functionality.</param>
        /// <param name="token">The ClaimsIdentity in token management.</param>
        /// <param name="emailService">The email service provides email service functionality.</param>
        public VendorEvaluationReportBll(IUnitOfWork unitOfWork, ISummaryEvaluationBll summaryEvaluation,
                                         IReportService reportService, IManageToken token, IEmailService emailService, IEmailTaskBll emailTask)
        {
            _unitOfWork = unitOfWork;
            _token = token;
            _summaryEvaluation = summaryEvaluation;
            _reportService = reportService;
            _emailService = emailService;
            _emailTask = emailTask;
        }

        #endregion

        #region [Methods]

        /// <summary>
        /// Get evaluation list status approved only.
        /// </summary>
        /// <param name="periodItemId">The period item identity target.</param>
        /// <returns></returns>
        public IEnumerable<EvaluationReportViewModel> GetList()
        {
            var data = _unitOfWork.GetRepository<Data.Pocos.Evaluation>().Get(x => x.Status == ConstantValue.WorkflowStatusApproved);
            return this.InitalViewModel(data);

        }

        /// <summary>
        /// Get evaluation list status approved only.
        /// </summary>
        /// <param name="periodItemId">The period item identity target.</param>
        /// <returns></returns>
        public IEnumerable<EvaluationReportViewModel> GetList(int periodItemId)
        {
            var data = _unitOfWork.GetRepository<Data.Pocos.Evaluation>().Get(x => x.PeriodItemId == periodItemId &&
                                                                        x.Status == ConstantValue.WorkflowStatusApproved);
            return this.InitalViewModel(data);

        }

        /// <summary>
        /// Inital evaluation data to view model.
        /// </summary>
        /// <param name="data">The evaluation collection data.</param>
        /// <returns></returns>
        private IEnumerable<EvaluationReportViewModel> InitalViewModel(IEnumerable<Data.Pocos.Evaluation> data)
        {
            var result = new List<EvaluationReportViewModel>();
            var vendors = data.Select(x => x.VendorNo).ToArray();
            var vendorList = _unitOfWork.GetRepository<Data.Pocos.Vendor>().GetCache(x => vendors.Contains(x.VendorNo));
            var comList = _unitOfWork.GetRepository<Hrcompany>().GetCache();
            var purOrgList = _unitOfWork.GetRepository<PurchaseOrg>().GetCache();
            var valueHelp = _unitOfWork.GetRepository<ValueHelp>().GetCache(x => x.ValueType == ConstantValue.ValueTypeWeightingKey);
            var gradeList = _unitOfWork.GetRepository<GradeItem>().GetCache();
            var templateList = _unitOfWork.GetRepository<EvaluationTemplate>().GetCache();

            foreach (var item in data)
            {
                result.Add(new EvaluationReportViewModel
                {
                    Id = item.Id,
                    CompanyName = comList.FirstOrDefault(x => x.SapcomCode == item.ComCode)?.LongText,
                    VendorName = vendorList.FirstOrDefault(x => x.VendorNo == item.VendorNo)?.VendorName,
                    PurchaseOrgName = purOrgList.FirstOrDefault(x => x.PurchaseOrg1 == item.PurchasingOrg)?.PurchaseName,
                    WeightingKey = valueHelp.FirstOrDefault(x => x.ValueKey == item.WeightingKey)?.ValueText,
                    TotalScore = item.TotalScore.Value,
                    GradeName = this.GetGradeName(item.TotalScore.Value, gradeList, templateList.FirstOrDefault(x => x.Id == item.EvaluationTemplateId)),
                    IsPrintReport = item.IsPrintReport ?? false,
                    IsSendEmail = item.IsSendEmail ?? false,
                    SendEmailDate = item.SendEmailDate.HasValue ? UtilityService.DateTimeToString(item.SendEmailDate.Value, ConstantValue.DateTimeFormat) : string.Empty
                });
            }

            return result;
        }

        /// <summary>
        /// Get Grade from total score.
        /// </summary>
        /// <param name="totalScore">The total score evaluation.</param>
        /// <param name="gradeList">The grade item collection.</param>
        /// <param name="evaluationTemplate">The evaluation template.</param>
        /// <returns></returns>
        private string GetGradeName(int totalScore, IEnumerable<GradeItem> gradeList, EvaluationTemplate evaluationTemplate)
        {
            string result = string.Empty;
            var grades = gradeList.Where(x => x.GradeId == evaluationTemplate.GradeId);
            return grades.FirstOrDefault(x => x.StartPoint <= totalScore && x.EndPoint >= totalScore)?.GradeNameTh;
        }

        /// <summary>
        /// Export vendor evaluation report
        /// </summary>
        /// <param name="id">The evaluation identity.</param>
        /// <returns></returns>
        public ResponseFileModel EvaluationExportReport(int id)
        {
            var response = new ResponseFileModel();
            string path = this.GetReportDirectory("VendorEvaluationReport", id.ToString());
            if (this.ExistReport(path))
            {
                response = this.GetFile(path);
            }
            else
            {
                var reportData = this.GenerateDataReport(id);
                response = _reportService.CallVendorEvaluationReport(reportData);
                this.GenerateFile(path, response);
                this.UpdateStatusPrintFlag(id);
            }
            return response;
        }

        /// <summary>
        /// Send email evaluation report to vendor. 
        /// </summary>
        /// <param name="id">The evaluation identity.</param>
        /// <returns></returns>
        public ResultViewModel SendVendorEvaluaitonReportEmail(int id)
        {
            var result = new ResultViewModel();
            string path = this.GetReportDirectory("VendorEvaluationReport", id.ToString());
            if (!this.ExistReport(path))
            {
                this.EvaluationExportReport(id);
            }
            string[] allfiles = Directory.GetFiles(path);
            this.SendEmailToVendor(id, allfiles[0]);
            this.UpdateStatusSendEmailFlag(id);
            return result;
        }

        /// <summary>
        /// Update print report flag to true.
        /// </summary>
        /// <param name="id">The evaluation identity.</param>
        private void UpdateStatusPrintFlag(int id)
        {
            using (var scope = new TransactionScope())
            {
                var data = _unitOfWork.GetRepository<Data.Pocos.Evaluation>().GetById(id);
                if (!(data.IsPrintReport.HasValue && data.IsPrintReport.Value))
                {
                    data.IsPrintReport = true;
                    _unitOfWork.GetRepository<Data.Pocos.Evaluation>().Update(data);
                    _unitOfWork.Complete(scope);
                }
            }
        }

        /// <summary>
        /// Update send email flag to true.
        /// </summary>
        /// <param name="id">The evaluation identity.</param>
        private void UpdateStatusSendEmailFlag(int id)
        {
            using (var scope = new TransactionScope())
            {
                var data = _unitOfWork.GetRepository<Data.Pocos.Evaluation>().GetById(id);
                data.IsSendEmail = true;
                data.SendEmailDate = DateTime.Now;
                _unitOfWork.GetRepository<Data.Pocos.Evaluation>().Update(data);
                _unitOfWork.Complete(scope);
            }
        }

        /// <summary>
        /// Validate exits report or not.
        /// </summary>
        /// <param name="path">The directory report path.</param>
        /// <returns></returns>
        private bool ExistReport(string path)
        {
            return Directory.Exists(path);
        }

        /// <summary>
        /// Send email vendor evaluation report.
        /// </summary>
        /// <param name="id">The evaluation identity.</param>
        /// <param name="file">The directory report file.</param>
        private void SendEmailToVendor(int id, string file)
        {
            var data = _unitOfWork.GetRepository<Data.Pocos.Evaluation>().GetById(id);
            var vendor = _unitOfWork.GetRepository<Data.Pocos.Vendor>().GetCache(x => x.VendorNo == data.VendorNo).FirstOrDefault();
            var emailTemplate = _unitOfWork.GetRepository<EmailTemplate>().GetCache(x => x.EmailType == ConstantValue.EmailTypeVendorEvaluationReportNotice).FirstOrDefault();
            var company = _unitOfWork.GetRepository<Hrcompany>().GetCache(x => x.SapcomCode == data.ComCode).FirstOrDefault();
            string[] periodInfo = this.GetPeriodName(data.PeriodItemId.Value);

            string subject = emailTemplate.Subject;
            string content = emailTemplate.Content;

            subject = subject.Replace("%PERIOD%", periodInfo[0]);
            subject = subject.Replace("%PERIODITEM%", periodInfo[1]);

            content = content.Replace("%VENDOR%", vendor?.VendorName);
            content = content.Replace("%COMNAME%", company?.LongText);

            _emailService.SendEmail(new EmailModel
            {
                AttachmentPathFile = file,
                Body = content,
                Receiver = vendor.Email,
                Subject = subject,
            });

            _emailTask.Save(new Email.Bll.Models.EmailTaskViewModel
            {
                DocNo = data.DocNo,
                Content = $"{content} Attachment file name : {Path.GetFileName(file)}",
                Subject = subject,
                TaskCode = ConstantValue.EmailVendorEvaluationReportNoticeCode,
                Receivers = new List<Email.Bll.Models.EmailTaskReceiveViewModel>
                {
                    new Email.Bll.Models.EmailTaskReceiveViewModel{ Email = vendor.Email, FullName = vendor.VendorName, ReceiverType = ConstantValue.ReceiverTypeTo }
                },
                TaskBy = _token.AdUser,
                TaskDate = DateTime.Now,
                Status = ConstantValue.EmailTaskStatusSending
            });
        }

        /// <summary>
        /// Get Period information for generate content.
        /// </summary>
        /// <param name="periodItemId">The period item identity.</param>
        /// <returns></returns>
        private string[] GetPeriodName(int periodItemId)
        {
            var periodItem = _unitOfWork.GetRepository<PeriodItem>().GetCache(x => x.Id == periodItemId).FirstOrDefault();
            var period = _unitOfWork.GetRepository<Period>().GetCache(x => x.Id == periodItem.PeriodId).FirstOrDefault();
            return new string[] { period.Year.ToString(), periodItem.PeriodName };
        }

        /// <summary>
        /// Get report directory path.
        /// </summary>
        /// <param name="processCode">The process code folder.</param>
        /// <param name="id">The identity folder.</param>
        /// <returns></returns>
        private string GetReportDirectory(string processCode, string id)
        {
            return Path.Combine(Directory.GetCurrentDirectory(), "Reports", processCode, id.ToString());
        }

        /// <summary>
        /// Get report file.
        /// </summary>
        /// <param name="path">The report file path.</param>
        /// <returns></returns>
        private ResponseFileModel GetFile(string path)
        {
            var result = new ResponseFileModel();
            string[] allfiles = Directory.GetFiles(path);
            foreach (var item in allfiles)
            {
                var fileByte = File.ReadAllBytes(item);
                result.FileName = Path.GetFileName(item);
                result.FileContent = fileByte;
            }
            return result;
        }

        /// <summary>
        /// Generate file report when print or send email.
        /// </summary>
        /// <param name="path"></param>
        /// <param name="model"></param>
        private void GenerateFile(string path, ResponseFileModel model)
        {
            Directory.CreateDirectory(path);
            string savePath = Path.Combine(path, model.FileName);
            File.WriteAllBytes(savePath, model.FileContent);
        }

        /// <summary>
        /// Generate data for export report vendor evaluation.
        /// </summary>
        /// <param name="id">The evaluation identity.</param>
        /// <returns></returns>
        private VendorEvaluationRequestModel GenerateDataReport(int id)
        {

            var data = _unitOfWork.GetRepository<Data.Pocos.Evaluation>().GetById(id);
            var company = _unitOfWork.GetRepository<HrcompanyAddress>().GetCache(x => x.SapComCode == data.ComCode).FirstOrDefault();
            var evaluationTemplate = _unitOfWork.GetRepository<EvaluationTemplate>().GetCache(x => x.Id == data.EvaluationTemplateId).FirstOrDefault();
            var gradePoint = _unitOfWork.GetRepository<GradeItem>().GetCache(x => x.GradeId == evaluationTemplate.GradeId &&
                                                                                   x.StartPoint <= data.TotalScore && x.EndPoint >= data.TotalScore).FirstOrDefault();
            //Get summary kpi and kpi group score.
            var scoreSummary = _summaryEvaluation.GetDetail(id);
            //Get report template.
            var template = _unitOfWork.GetRepository<ReportTemplate>().GetCache(x => x.ProcessCode == ConstantValue.VendorEvaluationReportProcess)
                                                                      .FirstOrDefault();
            var approveInfo = this.GetApproveByName(id);
            var culture = new CultureInfo("th-TH");
            var result = new VendorEvaluationRequestModel
            {
                DocNo = $"{company.SearchTermEn} - {data.DocNo} - {DateTime.Now.Date.ToString("yyyy", culture)}",
                TotalScore = data.TotalScore ?? 0,
                VendorName = this.GetVendorName(data.VendorNo),
                PrintDate = DateTime.Now.Date.ToString("dd MMMM yyyy", culture),
                GradeName = scoreSummary.GradeName,
                PeriodName = this.GetPeriodItemName(data.PeriodItemId.Value),
                ContentHeader = this.BuildContentHeader(template.Content1Th, template.Content2Th, template.Content3Th, company),
                ContentFooter = this.BuildContentFooter(template.Content4Th, template.Content5Th, gradePoint),
                CompanyNameTh = company.NameTh,
                CompanyNameEn = company.NameEn,
                CompanyAddressTh = this.GetCompanyAddress(company, "TH"),
                CompanyAddressEn = this.GetCompanyAddress(company, "EN"),
                KpiGroups = this.GetKpiGroupCollection(evaluationTemplate, data.WeightingKey, scoreSummary).ToList(),
                ApproveBy = approveInfo[0],
                PositionName = approveInfo[1],
                GradeNameEn = gradePoint.GradeNameEn
            };

            return result;
        }

        /// <summary>
        /// Build content header in report.
        /// </summary>
        /// <param name="content1">The content header first line.</param>
        /// <param name="content2">The content header second line.</param>
        /// <param name="content3">The content header third line.</param>
        /// <returns></returns>
        private string BuildContentHeader(string content1, string content2, string content3, HrcompanyAddress company)
        {
            content1 = content1.Replace("%COMPANY_NAME%", company.NameTh);
            StringBuilder contentHeader = new StringBuilder();
            contentHeader.AppendLine(content1);
            contentHeader.AppendLine($"{leftSpace}{content2}");
            contentHeader.AppendLine($"{leftSpace}{content3}");
            return contentHeader.ToString();
        }

        /// <summary>
        /// Build content footer in report.
        /// </summary>
        /// <param name="content4">The content footer first line.</param>
        /// <param name="content5">The content footer second line</param>
        /// <returns></returns>
        private string BuildContentFooter(string content4, string content5, GradeItem gradePoint)
        {
            content4 = content4.Replace("%GRADENAMEENG%", gradePoint.GradeNameEn);
            content4 = content4.Replace("%GRADENAMETH%", gradePoint.GradeNameTh);
            StringBuilder contentFooter = new StringBuilder();
            contentFooter.AppendLine(content4);
            contentFooter.AppendLine($"{leftSpace}{content5}");
            return contentFooter.ToString();
        }

        /// <summary>
        /// Get vendor name with vendor identity number.
        /// </summary>
        /// <param name="vendorNo">The vendor identity number.</param>
        /// <returns></returns>
        private string GetVendorName(string vendorNo)
        {
            return _unitOfWork.GetRepository<Data.Pocos.Vendor>().GetCache(x => x.VendorNo == vendorNo).FirstOrDefault()?.VendorName;
        }

        /// <summary>
        /// Get period item name with period item identity.
        /// </summary>
        /// <param name="periodItemId">The period item identity.</param>
        /// <returns></returns>
        private string GetPeriodItemName(int periodItemId)
        {
            return _unitOfWork.GetRepository<PeriodItem>().GetCache(x => x.Id == periodItemId).FirstOrDefault()?.PeriodName;
        }

        /// <summary>
        /// Get approve information evaluation.
        /// </summary>
        /// <param name="id">The evaluation identity.</param>
        /// <returns></returns>
        private string[] GetApproveByName(int id)
        {
            List<string> result = new List<string>();
            var processInstance = _unitOfWork.GetRepository<WorkflowProcessInstance>().Get(x => x.DataId == id &&
                                                                                                x.ProcessCode == ConstantValue.EvaluationProcessCode)
                                                                                      .OrderByDescending(x => x.ProcessInstanceId).FirstOrDefault();

            var workflowFinalStep = _unitOfWork.GetRepository<WorkflowActivityLog>().Get(x => x.ProcessInstanceId == processInstance.ProcessInstanceId &&
                                                                                        x.Step > 1)
                                                                                    .OrderByDescending(x => x.Step).FirstOrDefault();

            var empInfo = _unitOfWork.GetRepository<Hremployee>().GetCache(x => x.Aduser == workflowFinalStep.ActionUser).FirstOrDefault();
            result.Add(string.Format(ConstantValue.EmpTemplate, empInfo.FirstnameTh, empInfo.LastnameTh));
            result.Add(this.GetPositionName(empInfo.PositionId));
            return result.ToArray();
        }

        /// <summary>
        /// Get position name with position identity.
        /// </summary>
        /// <param name="positionId">The position identity.</param>
        /// <returns></returns>
        private string GetPositionName(string positionId)
        {
            return _unitOfWork.GetRepository<Hrposition>().GetCache(x => x.PosId == positionId).FirstOrDefault()?.PosName;
        }

        /// <summary>
        /// Get kpi group collection for genarate report.
        /// </summary>
        /// <param name="templateId">The evaluation template identity.</param>
        /// <param name="weigthingKey">The weigthing key for maximun max score.</param>
        /// <param name="summary">The summary detail evaluation.</param>
        /// <returns></returns>
        private IEnumerable<VendorEvaluationRequestItemModel> GetKpiGroupCollection(EvaluationTemplate templateInfo, string weigthingKey,
                                                                                    SummaryEvaluationViewModel summary)
        {

            var criteriaInfo = _unitOfWork.GetRepository<Criteria>().GetCache(x => x.Id == templateInfo.CriteriaId).FirstOrDefault();
            var criteriaGroups = _unitOfWork.GetRepository<CriteriaGroup>().GetCache(x => x.CriteriaId == criteriaInfo.Id);
            var kpiGroups = _unitOfWork.GetRepository<KpiGroup>().GetCache();

            return this.InitialVendorEvaluationRequestItemModel(kpiGroups, criteriaGroups, weigthingKey, summary);
        }

        /// <summary>
        /// Initial data for vendor evaluaiton request item model.
        /// </summary>
        /// <param name="kpiGroups">The kpi group collection.</param>
        /// <param name="criteriaGroups">The criteria group collection.</param>
        /// <param name="weigthingKey">The weithing key for maximun max score.</param>
        /// <param name="summary">The summary detail evaluation.</param>
        /// <returns></returns>
        private IEnumerable<VendorEvaluationRequestItemModel> InitialVendorEvaluationRequestItemModel(IEnumerable<KpiGroup> kpiGroups,
                                                                                                IEnumerable<CriteriaGroup> criteriaGroups,
                                                                                                string weigthingKey,
                                                                                                SummaryEvaluationViewModel summary)
        {
            var result = new List<VendorEvaluationRequestItemModel>();
            foreach (var item in criteriaGroups)
            {
                int score = 0;
                var scoreSummary = summary.Summarys.FirstOrDefault(x => x.KpiGroupId == item.KpiGroupId && x.KpiId == 0);
                if (scoreSummary != null)
                {
                    score = Convert.ToInt32(scoreSummary.Score);
                }
                result.Add(new VendorEvaluationRequestItemModel
                {
                    KpiGroupName = $"{item.Sequence}. {kpiGroups.FirstOrDefault(x => x.Id == item.KpiGroupId.Value)?.KpiGroupNameTh}",
                    MaxScore = string.Equals(weigthingKey, "A2", StringComparison.OrdinalIgnoreCase) ? 100 : item.MaxScore.Value,
                    Score = score,
                });
            }
            return result;
        }

        /// <summary>
        /// Get address company.
        /// </summary>
        /// <param name="company">The company address informatino.</param>
        /// <param name="culture">The culture language return.</param>
        /// <returns></returns>
        private string GetCompanyAddress(HrcompanyAddress company, string culture)
        {
            string result = string.Empty;
            switch (culture)
            {
                case "TH":
                    result = $"{company.Address1Th}{company.Address2Th}{company.Address3Th}{company.DistrictTh}" +
                             $"{company.CityTh}{company.PostalCode} {company.Telephone}";
                    break;
                case "EN":
                    result = $"{company.Address1En}{company.Address2En}{company.Address3En}{company.DistrictEn}" +
                             $"{company.CityEn}{company.PostalCode} {company.Telephone}";
                    break;
            }

            return result;
        }

        #endregion

    }
}
