using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Awlog.Models
{
    public class BPost
    {
        public int Id { get; set; }
        public string Header { get; set; }
        public string Message { get; set; }
        public BAttachments[] Attachments { get; set; }
    }
}
