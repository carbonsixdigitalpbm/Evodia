﻿//using System;
//using System.Web;

//public class Paging
//{
//    public int ItemsPerPage { get; set; }
//    public int CurrentPage { get; set; }
//    public int PreviousPage { get; set; }
//    public int NextPage { get; set; }
//    public double TotalPages { get; set; }
//    public int Skip { get; set; }
//    public int Take { get; set; }

//    public static Paging GetPages(int itemCount, int itemsPerPage)
//    {
//        int page;
//        int.TryParse(HttpContext.Current.Request.QueryString["page"], out page);
//        if (page == 0) page = 1;

//        var pages = new Paging
//        {
//            ItemsPerPage = itemsPerPage,
//            CurrentPage = page,
//            PreviousPage = page - 1,
//            NextPage = page + 1,
//            TotalPages = Math.Ceiling(itemCount/(double) itemsPerPage),
//            Skip = page*itemsPerPage - itemsPerPage,
//            Take = itemsPerPage
//        };

//        return pages;
//    }
//}