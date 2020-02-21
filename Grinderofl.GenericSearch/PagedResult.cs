#pragma warning disable 1591
using System;

namespace Grinderofl.GenericSearch
{
    /// <summary>
    /// Offers a reusable base class for a paged viewmodel result
    /// </summary>
    public abstract class PagedResult
    {
        protected PagedResult(int total)
        {
            Total = total;
        }

        public int Page { get; set; }

        public int Rows { get; set; }

        public int Total { get; }

        private int? pages;
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

        public bool HasPreviousPage => Page > 1;
        public bool HasNextPage => Page < Pages;
        public int PreviousPage => Page - 1;
        public int NextPage => Page + 1;

        private int? startPage;
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

        protected virtual int MaximumPreviousPages => 5;
        protected virtual int MaximumNextPages => 4;

        public virtual bool IsActivePage(int page) => Page == page;
    }
}