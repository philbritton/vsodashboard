using MongoDB.Driver;
using MongoDB.Driver.Builders;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using VSOClientData;
using VSOClientData.Entities.Blogs;
using VSOOAuthClientSample.VSO;

namespace VSOOAuthClientSample.Controllers.Mongo
{
    public class BlogsController : Controller
    {
        // GET: Blogs
        public ActionResult Index()
        {
            var helper = new MongoHelper<Posts>("posts");

            var posts = helper.Collection.FindAll().SetFields(Fields.Exclude("Comments")).SetSortOrder(SortBy.Descending("Date")).ToList();


            return View();
        }
    }
}