﻿using System;
using System.Collections.Generic;
using System.Text;

namespace EVF.Helper.Components
{
    public static class MessageValue
    {
        public const string UserRoleIsEmpty = "กรุณาติดต่อเจ้าหน้าที่ เพื่อเปิดสิทธิ์การใช้งาน";
        public const string LoginFailed = "ขออภัย Username หรือ Password ไม่ถูกต้อง";
        public const string Unauthorized = "กรุณาเข้าสู่ระบบ เพื่อใช้งาน";
        public const string InternalServerError = "ระบบเกิดข้อผิดพลาด กรุณาติดต่อเจ้าหน้าที่";
        public const string GradePointIncorrect = "กรุณากรอก ช่วงคะแนนให้ถูกต้อง";
        public const string GradePointOverRange = "กรุณาระบุช่วงคะแนน 0 ถึง 100 เท่านั้น";
        public const string CriteriaOverScore = "ขออภัย คะแนนเต็ม ทั้งหมดรวมกัน ต้องไม่น้อยกว่า หรือ มากกว่า 100 คะแนน";
        public const string CriteriaKpiGroupEmpty= "กรุณาเลือก กลุ่มการประเมิน";
        public const string CriteriaItemScoreGreatethanScoreGroup = "ขออภัย คะแนนกลุ่มการประเมิน ไม่สามารถน้อยกว่าหรือมากกว่า คะแนนหัวข้อการประเมินรวมกันได้";
        public const string EvaluationRejectTaskIsAction = "ขออภัย ไม่สามารถปฎิเสธงานที่ทำการประเมินแล้วได้";
        public const string KpiGroupOverFiftySapFields = "ขออภัย ไม่สามารถสร้างกลุ่มการประเมิน เกิน 50 กลุ่มได้";
        public const string KpiGroupItemsDuplicates = "ขออภัย ไม่สามารถเลือกกลุ่มการประเมินซ้ำกันได้";
        public const string IsUseMessageFormat = "ขออภัย ไม่สามารถแก้ไข หรือ ลบ {0} เนื่องจากมีการนำไปใช้งานแล้ว";
        public const string HttpBadRequestMessage = "กรุณา กรอกข้อมูลให้ถูกต้อง";
        public const string KpiMessage = "หัวข้อการประเมิน";
        public const string KpiGroupMessage = "กลุ่มการประเมิน";
        public const string CriteriaMessage = "หลักเกณฑ์";
        public const string EvaluationTemplateMessage = "Template แบบประเมิน";
        public const string LevelPointMessage = "ระดับคะแนน";
        public const string GradeMessage = "เกณฑ์การประเมิน";
        public const string WorkflowFiloEvaluationProcess = "ประเมินผู้ขาย {0}";
        public const string StatusInvalidAction = "ไม่สามารถส่งอนุมัติได้ เนื่องจากใบประเมิน อยู่ในสถานะ {0}";
        public const string EvaluatorEmpty = "กรุณาเลือก ผู้ประเมิน หรือกลุ่มผู้ประเมิน";
        public const string PleaseSelectedEvaluationTemplate = "กรุณาเลือก Template แบบประเมิน";
        public const string PleaseSelectedVendor = "กรุณาเลือก ผู้ขาย";
        public const string PleaseSelectedWeightingKey = "กรุณาเลือก WeightingKey";
        public const string PleaseSelectedCompany = "กรุณาเลือก บริษัท";
        public const string PleaseSelectedPurchaseOrg = "กรุณาเลือก กลุ่มจัดซื้อ";
        public const string PleaseSelectedPeriod = "กรุณาเลือก ระยะเวลา";
        public const string PleaseSelectedKpiGroup = "กรุณาเลือก กลุ่มการประเมิน";
        public const string PleaseSelectedCriteria = "กรุณาเลือก หลักเกณฑ์";
        public const string PleaseSelectedGrade = "กรุณาเลือก เกณฑ์การประเมิน";
        public const string PleaseSelectedLevelPoint = "กรุณาเลือก ระดับคะแนน";
        public const string PleaseFillCriteriaName = "กรุณากรอก ชื่อหลักเกณฑ์";
        public const string PleaseFillEvaluationTemplateName = "กรุณากรอก ชื่อ Template แบบประเมิน";
        public const string PleaseFillScore = "กรุณากรอก คะแนน";
        public const string CriteriaNameOverLength = "ขออภัย ชื่อหลักเกณฑ์ ความยาวต้องไม่เกิน 200 ตัวอักษร";
        public const string EvaluationTemplateNameOverLength = "ขออภัย ชื่อ Template แบบประเมิน ความยาวต้องไม่เกิน 200 ตัวอักษร";
        public const string EvaluationLogSaveValidate = "ขออภัย ไม่สามารถบันทึกการประเมินได้ กรุณาประเมินให้ครบทุกข้อ";
    }
}
