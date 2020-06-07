using System;
using System.Collections.Generic;

namespace HICMigration.Models
{
    public partial class PageContent
    {
        public string Page { get; set; }
        public int Revision { get; set; }
        public string Title { get; set; }
        public DateTime DateTime { get; set; }
        public string Username { get; set; }
        public string Content { get; set; }
        public string Comment { get; set; }
        public string OriginalName { get; set; }
        public string OriginalContent { get; set; }
    }
}
