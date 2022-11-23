namespace eTrainingSolution.WebApp.Areas.Identity.Models.Page
{
    public class PagingModel
    {
        /// <summary>
        /// Trang hiện tại
        /// </summary>
        public int currentPage { get; set; }

        /// <summary>
        /// Tổng số trang
        /// </summary>
        public int CountPage { get; set; }

        /// <summary>
        /// Trả về Url tương ứng với trang
        /// </summary>
        public Func<int?, string> GenerateUrl { get; set; } = default!;
    }
}
