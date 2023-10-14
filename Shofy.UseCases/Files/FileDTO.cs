using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shofy.UseCases.Files
{
    public class FileDTO
    {
        public string Name { get; set; } = null!;
        public string ContentType { get; set; } = null!;
        public string Path { get; set; } = null!;
    }
}
