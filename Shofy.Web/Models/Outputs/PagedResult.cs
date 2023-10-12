namespace Shofy.Web.Models.Outputs
{
    public class PagedResults<TViewModel>
    {
        public int ResultCount { get; set; }
        public IEnumerable<TViewModel> Results { get; set; } = Enumerable.Empty<TViewModel>();
    }
}
