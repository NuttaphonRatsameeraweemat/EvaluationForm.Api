using System;
using System.Collections.Generic;
using System.Text;

namespace EVF.Helper.Components
{
    public static class MessageValue
    {
        public const string UserRoleIsEmpty = "กรุณาติดต่อเจ้าหน้าที่ เพื่อเปิดสิทธิ์การใช้งาน";
        public const string LoginFailed = "ขออภัย Username หรือ Password ไม่ถูกต้อง";
        public const string InternalServerError = "ระบบเกิดข้อผิดพลาด กรุณาติดต่อเจ้าหน้าที่";
        public const string GradePointIncorrect = "กรุณากรอก ช่วงคะแนนให้ถูกต้อง";
        public const string GradePointOverRange = "กรุณาระบุช่วงคะแนน 0 ถึง 100 เท่านั้น";
    }
}
