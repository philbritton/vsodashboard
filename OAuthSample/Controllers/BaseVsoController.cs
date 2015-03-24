using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using VSOOAuthClientSample.VSO;
using VSOOAuthClientSample.VSO.BuildApi;

namespace VSOOAuthClientSample.Controllers
{
    public abstract class BaseVsoController : Controller
    {
        internal String _baseUrl = "https://{0}.visualstudio.com/DefaultCollection/_apis/";

        /// <summary>
        /// Your visual studio account name
        /// </summary>
        internal String _account = "cybprototype";

        /// <summary>
        /// Api version query parameter
        /// </summary>
        internal String _apiVersion = "api-version=1.0";


        internal async Task<String> GetAsync(HttpClient client, String apiUrl)
        {
            var responseBody = String.Empty;

            var queryUrl = apiUrl;
            var queryString = HttpUtility.ParseQueryString(apiUrl);
            if (queryString.HasKeys())
            {
                queryUrl += "&" + _apiVersion;
            }
            else
            {
                queryUrl += "?" + _apiVersion;
            }

            try
            {
                using (HttpResponseMessage response = client.GetAsync(queryUrl).Result)
                {
                    response.EnsureSuccessStatusCode();
                    responseBody = await response.Content.ReadAsStringAsync();
                }
            }
            catch (Exception ex)
            {
                responseBody = ex.ToString();
                FileLogger.Error(ex, responseBody);
            }

            return responseBody;
        }

        internal async Task<String> PostAsync(HttpClient client,
                                          StringContent content,
                                          String apiUrl)
        {
            var responseBody = String.Empty;

            try
            {
                using (HttpResponseMessage response = client.PostAsync(apiUrl + _apiVersion, content).Result)
                {
                    response.EnsureSuccessStatusCode();
                    responseBody = await response.Content.ReadAsStringAsync();
                }
            }
            catch (Exception ex)
            {
                FileLogger.Information(ex.ToString());
            }

            return responseBody;
        }
    }
}