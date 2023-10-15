using ImageMagick;
using Microsoft.Extensions.Configuration;
using Shofy.UseCases.Documents;

namespace DocumentsAdapter
{
    public class DocumentAdapter : IDocumentAdapter
    {

        private readonly IConfiguration _configuration;

        public DocumentAdapter(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public async Task<string> ResizeAsync(string documentPath, int width)
        {
            var bytes = File.ReadAllBytes(documentPath);
            using var collection = new MagickImageCollection(bytes);

            // This will remove the optimization and change the image to how it looks at that point
            // during the animation. More info here: http://www.imagemagick.org/Usage/anim_basics/#coalesce
            collection.Coalesce();

            // Resize each image in the collection to a width of 200. When zero is specified for the height
            // the height will be calculated with the aspect ratio.
            foreach (var image in collection)
            {
                image.Resize(width, 0);
            }

            var fileName = Path.GetFileNameWithoutExtension(documentPath);
            var newPath = documentPath.Replace(fileName, $"{fileName}-resized");

            // Save the result
            collection.Write(newPath);

            return await Task.FromResult(newPath);
        }
    }
}