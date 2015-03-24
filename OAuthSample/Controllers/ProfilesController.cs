using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using VSOOAuthClientSample.VSO;

namespace VSOOAuthClientSample.Controllers
{
    public class ProfilesController : BaseVsoController
    {
        internal new string _baseUrl = "https://app.vssps.visualstudio.com/_apis/";

        // GET: Profile
        async public Task<ActionResult> Index()
        {
            var ctoken = this.Request.Cookies["ctoken"];
            if (ctoken != null)
            {
                var ctokenValue = ctoken.Value;

                var message = string.Empty;
                try
                {

                    using (var client = new HttpClient())
                    {
                        client.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));
                        client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", ctokenValue);

                        message = await GetAsync(client, _baseUrl + "profile/profiles/me");
                    }
                }
                catch (Exception ex)
                {
                    message = ex.ToString();
                    FileLogger.Error(ex, message);
                }

                ViewBag.Message = message;
            }



            return View();
        }
    }
}