using Shofy.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shofy.UseCases.Documents
{
    public interface IDocumentAdapter
    {
        Task<string> ResizeAsync(
            string documentPath, 
            int width);
    }
}
