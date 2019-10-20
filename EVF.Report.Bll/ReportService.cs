using EVF.Helper;
using EVF.Helper.Components;
using EVF.Helper.Interfaces;
using EVF.Report.Bll.Interfaces;
using EVF.Report.Bll.Models;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;

namespace EVF.Report.Bll
{
    public class ReportService : IReportService
    {

        #region [Fields]

        /// <summary>
        /// The config value in appsetting.json
        /// </summary>
        private readonly IConfigSetting _config;
        /// <summary>
        /// The HttpClient request.
        /// </summary>
        private readonly HttpClient _client;

        #endregion

        #region [Constructors]

        /// <summary>
        /// Initializes a new instance of the <see cref="ReportService" /> class.
        /// </summary>
        /// <param name="config">The config value.</param>
        public ReportService(IConfigSetting config)
        {
            _config = config;
            _client = new HttpClient();
        }

        #endregion

        #region [Methods]

        /// <summary>
        /// Get base url plus controller route api. 
        /// </summary>
        /// <param name="ControllersName">The controller route api.</param>
        /// <returns></returns>
        private Uri CallCommonApi(string ControllersName)
        {
            Uri url = new Uri(_config.ReportUrl + ControllersName);
            return new Uri(url + "");
        }

        public ResponseFileModel Try()
        {
            //using (HttpResponseMessage response = _client.PostAsync(
            //                                        this.CallCommonApi(string.Format("{0}/{1}", "VendorEvaluationReport", "Try")),
            //                                        UtilityService.SerializeContent(this.InitialModel())).Result)
            using (HttpResponseMessage response = _client.GetAsync(
                                                                    this.CallCommonApi(string.Format("{0}/{1}", "VendorEvaluationReport", "Try"))).Result)
            {
                if (!response.IsSuccessStatusCode)
                {
                    throw new HttpRequestException(string.Format(ConstantValue.HttpRequestFailedMessage, (int)response.StatusCode, response.StatusCode.ToString()));
                }
                return UtilityService.DeserializeContent<ResponseFileModel>(response.Content);
            }
        }

        private VendorEvaluationRequestModel InitialModel()
        {
            return new VendorEvaluationRequestModel
            {
                ApproveBy = "ธนพร เจริญกัลป์",
                CompanyNameEn = "BOON RAWD BREWERY Co., Ltd.",
                CompanyNameTh = "บริษัท บุญรอดบริวเวอรี่ จำกัด",
                ContentFooter = "บริษัทของท่าน ได้เกรด A คือ ผลการประเมินผู้ขายอยู่ในเกณฑ์ ดีมาก ให้รักษาคุณภาพสินค้าและบริการไว้" +
                                "บริษัทฯ ขอขอบคุณที่บริษัทของท่าน ที่เป็นบริษัทคู่ค้าที่ดีเสมอมา และหวังเป็นอย่างยิ่งว่า" +
                                "บริษัทฯ ของท่านจะพัฒนาสินค้าและบริการให้ดียิ่งขึ้นไป ",
                ContentHeader = "ตามที่ บริษัท บุญรอดบริวเวอรี่ จำกัด และบริษัทในเครือ ได้มีการซื้อสินค้าและบริการจาก" +
                                "บริษัทของท่าน เพื่อให้เกิดการพัฒนา คุณภาพสินค้า การให้บริการ การส่งมอบ และความสามารถใน" +
                                "การดำเนินธุรกิจร่วมกันตามหลักจริยธรรมที่โปร่งใส นั้น" +
                                "      บริษัทฯ จึงได้มีการประเมินผู้ขาย  เพื่อเป็นแนวทางในการพัฒนาและแก้ไขข้อบกพร่อง" +
                                "โดยมีเกณฑ์การประเมินผู้ขาย รายละเอียดดังนี้ ",
                DocNo = "BRB-1260-254/2561",
                GradeName = "A",
                KpiGroups = new List<VendorEvaluationRequestItemModel>
                {
                    new VendorEvaluationRequestItemModel{ KpiGroupName = "1.คุณภาพสินค้าและบริการ", MaxScore = 100 , Score = 80 },
                    new VendorEvaluationRequestItemModel{ KpiGroupName = "2.การส่งมอบสินค้าและบริการตรงตามระยะเวลาที่ก าหนด", MaxScore = 100 , Score = 80 },
                    new VendorEvaluationRequestItemModel{ KpiGroupName = "3.การติดต่อประสานงาน / การตอบกลับของผู้ขาย", MaxScore = 100 , Score = 80 },
                },
                MaxTotalScore = 100,
                PeriodName = "2561/2",
                PositionName = "ผู้จัดการฝ่ายจัดซื้อทั่วไป",
                PrintDate = "วันที่ 24 สิงหาคม 2561",
                TotalScore = 80,
                VendorName = "บริษัท แสงวิทย์ ซายน์ จำกัด"
            };
        }

        #endregion

    }
}
