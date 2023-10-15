using Shofy.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shofy.UseCases.Documents
{
    public class DocumentDTO
    {
        public string Name { get; set; } = null!;
        public string ContentType { get; set; } = null!;
        public string Path { get; set; } = null!;
        public DocumentStatus DocumentStatus { get; set; }
    }
}
