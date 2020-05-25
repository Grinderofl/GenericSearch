using System;

namespace Grinderofl.GenericSearch
{
    /// <summary>
    /// Offers a reusable base class for a paged viewmodel result
    /// </summary>
    public abstract class PagedResult
    {
        /// <summary>
        /// Initializes a new instance of <see cref="PagedResult"/>
        /// </summary>
        /// <param name="total">Total number of items</param>
        protected PagedResult(int total)
        {
            Total = total;
        }

        /// <summary>
        /// Specifies the current page
        /// </summary>
        public int Page { get; set; }

        /// <summary>
        /// Specifies number of rows per page
        /// </summary>
        public int Rows { get; set; }

        /// <summary>
        /// Specifies the total number of results
        /// </summary>
        public int Total { get; }

        private int? pages;

        /// <summary>
        /// Specifies the total number of pages
        /// </summary>
        public int Pages
        {
            get
            {
                if (pages == null)
                {
                    pages = (int)Math.Ceiling((decimal)Total / (Rows == 0 ? 1 : Rows));
                    if (pages == 0) pages = 1;
                }

                return pages.Value;
            }
        }

        /// <summary>
        /// Specifies whether there is a previous page
        /// </summary>
        public bool HasPreviousPage => Page > 1;

        /// <summary>
        /// Specifies whether there is a next page
        /// </summary>
        public bool HasNextPage => Page < Pages;

        /// <summary>
        /// Specifies the number of the previous page
        /// </summary>
        public int PreviousPage => Page - 1;

        /// <summary>
        /// Specifies the number of the next page
        /// </summary>
        public int NextPage => Page + 1;

        private int? startPage;

        /// <summary>
        /// Specifies the start of the page range
        /// </summary>
        public int StartPage
        {
            get
            {
                if (startPage == null)
                {
                    CalculatePageRanges();
                }

                return startPage.GetValueOrDefault();
            }
        }

        private int? endPage;

        /// <summary>
        /// Specfies the end of the page range
        /// </summary>
        public int EndPage
        {
            get
            {
                if (endPage == null)
                {
                    CalculatePageRanges();
                }

                return endPage.GetValueOrDefault();
            }
        }

        private void CalculatePageRanges()
        {
            var start = Page - MaximumPreviousPages;
            var end = Page + MaximumNextPages;
            if (start <= 0)
            {
                end -= start - 1;
                start = 1;
            }

            var maximumPages = MaximumNextPages + MaximumNextPages + 1;

            if (end > Pages)
            {
                end = Pages;
                if (end > maximumPages) start = end - (maximumPages - 1);
            }

            startPage = start;
            endPage = end;
        }

        /// <summary>
        /// Specifies the maximum number of previous pages
        /// </summary>
        protected virtual int MaximumPreviousPages => 5;

        /// <summary>
        /// Specifies the maximum number of next pages
        /// </summary>
        protected virtual int MaximumNextPages => 4;

        /// <summary>
        /// Specifies whether the provided page number is the currently active page
        /// </summary>
        /// <param name="page">Page number to check</param>
        /// <returns></returns>
        public virtual bool IsActivePage(int page) => Page == page;
    }
}