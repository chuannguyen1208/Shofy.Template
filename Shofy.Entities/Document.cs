using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shofy.Entities
{
    public class Document
    {
        public Guid Id { get; set; }
        public string Name { get; set; } = null!;
        public string ContentType { get; set; } = null!;
        public string Path { get; set; } = null!;
        public DocumentStatus DocumentStatus { get; set; }
        public string? Error { get; set; }
    }

    public enum DocumentStatus
    {
        New = 0,
        Processing = 1,
        Ready = 2,
        Error = 3,
    }
}
