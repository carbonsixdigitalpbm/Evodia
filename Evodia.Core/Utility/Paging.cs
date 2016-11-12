using System;
using System.Web;

namespace Evodia.Core.Utility
{
    public class Paging
    {
        public int TotalItems { get; set; }
        public int CurrentPage { get; set; }
        public int PageSize { get; set; }
        public int TotalPages { get; set; }
        public int StartPage { get; set; }
        public int EndPage { get; set; }
        public int Skip { get; set; }
        public int Take { get; set; }

        public static Paging GetPages(int totalItems, int pageSize = 10)
        {
            int page;
            int.TryParse(HttpContext.Current.Request.QueryString["page"], out page);
            if (page == 0) page = 1;

            var totalPages = (int)Math.Ceiling(totalItems / (decimal)pageSize);
            var currentPage = page;
            var startPage = currentPage - 3;
            var endPage = currentPage + 2;

            if (startPage <= 0)
            {
                endPage -= startPage - 1;
                startPage = 1;
            }

            if (endPage > totalPages)
            {
                endPage = totalPages;

                if (endPage > 10)
                {
                    startPage = endPage - 9;
                }
            }

            return new Paging()
            {
                TotalItems = totalItems,
                CurrentPage = currentPage,
                PageSize = pageSize,
                TotalPages = totalPages,
                StartPage = startPage,
                EndPage = endPage,
                Take = pageSize,
                Skip = page * pageSize - pageSize
            };
        }

    }
}