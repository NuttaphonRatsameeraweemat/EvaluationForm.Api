using System;
using System.Collections.Generic;
using System.Text;

namespace EVF.Email.Bll.Models
{
    public class EmailTaskViewModel
    {
        public EmailTaskViewModel()
        {
            Receivers = new List<EmailTaskReceiveViewModel>();
        }

        public int Id { get; set; }
        public DateTime TaskDate { get; set; }
        public string TaskCode { get; set; }
        public string TaskBy { get; set; }
        public string DocNo { get; set; }
        public string Subject { get; set; }
        public string Status { get; set; }

        public string Content { get; set; }

        public List<EmailTaskReceiveViewModel> Receivers { get; set; }
    }

    public class EmailTaskReceiveViewModel
    {
        public string Email { get; set; }
        public string FullName { get; set; }
        public string ReceiverType { get; set; }
    }

}
