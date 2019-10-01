using System;
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
        public const string CriteriaKpiGroupEmpty= "กรุณาเลือก กลุ่มตัวชี้วัด";
        public const string CriteriaItemScoreGreatethanScoreGroup = "ขออภัย คะแนนกลุ่มตัวชี้วัด ไม่สามารถน้อยกว่าหรือมากกว่า คะแนนตัวชี้วัดรวมกันได้";
        public const string EvaluationRejectTaskIsAction = "ขออภัย ไม่สามารถปฎิเสธงานที่ทำการประเมินแล้วได้";
        public const string KpiGroupOverFiftySapFields = "ขออภัย ไม่สามารถสร้างกลุ่มตัวชี้วัด เกิน 50 กลุ่มได้";
        public const string IsUseMessageFormat = "ขออภัย ไม่สามารถแก้ไข หรือ ลบ {0} เนื่องจากมีการนำไปใช้งานแล้ว";
        public const string KpiMessage = "ตัวชี้วัด";
        public const string KpiGroupMessage = "กลุ่มตัวชี้วัด";
        public const string CriteriaMessage = "หลักเกณฑ์";
        public const string EvaluationTemplateMessage = "Template แบบประเมิน";
        public const string LevelPointMessage = "ระดับคะแนน";
        public const string GradeMessage = "เกณฑ์การประเมิน";
    }
}
