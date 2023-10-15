using Shofy.UseCases.Documents;

namespace DocumentsAdapter
{
    public class DocumentAdapter : IDocumentAdapter
    {
        public async Task<string> ResizeAsync(string documentPath)
        {
            return await Task.FromResult(documentPath);
        }
    }
}