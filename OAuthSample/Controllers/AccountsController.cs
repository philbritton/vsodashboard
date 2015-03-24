using MongoDB.Bson;
using MongoDB.Driver;
using MongoDB.Driver.Builders;
using Newtonsoft.Json;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using VSOClientData;
using VSOClientData.Entities.Vso;
using VSOOAuthClientSample.VSO;

namespace VSOOAuthClientSample.Controllers
{
    public class AccountsController : BaseVsoController
    {
        internal new string _baseUrl = "https://app.vssps.visualstudio.com/_apis/";


        async public Task<ActionResult> Index()
        {
            try
            {

                var helper = new MongoHelper<VsoAccount>("vsoAccounts");
                var myaccounts = helper.Collection.FindAs<VsoAccount>(Query<VsoAccount>.EQ(r => r.AccountStatus, "enabled"))
                    .SetFields(
                        Fields<VsoAccount>.Include(
                            r => r.AccountId,
                            r => r.AccountName,
                            r => r.AccountType,
                            r => r.AccountUrl,
                            r => r.CreatedDate))
                    .SetLimit(10)
                    .SetSkip(0)
                    .ToList();

                return View(myaccounts);
            }
            catch (Exception ex)
            {
                ViewBag.Message = ex.ToString();
                return View();
            }

        }

        async public Task<ActionResult> RetrieveMy()
        {
            ApiCollection<VsoAccount> accountCollection;

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

                        message = await GetAsync(client, _baseUrl + string.Format("Accounts?memberId={0}", "113df414-bea9-44d5-b585-d7d9c2e3a61a"));


                        accountCollection = JsonConvert.DeserializeObject<ApiCollection<VsoAccount>>(message);


                        var helper = new MongoHelper<VsoAccount>("vsoAccounts");

                        using (helper.Database.RequestStart())
                        {
                            var nonQueryList = new List<string>();
                            var queryList = new List<IMongoQuery>();
                            accountCollection.Value.ToList().ForEach(
                                (vso) =>
                                {
                                    queryList.Add(Query.EQ("AccountId", vso.AccountId));
                                    nonQueryList.Add(vso.AccountId);
                                });

                            var concernResults = helper.Collection.Remove(Query.NotIn("AccountId", nonQueryList.Select(s => (BsonValue)s)));


                            var foundItems = helper.Collection.Find(Query.Or(queryList));
                            foundItems.ToList().ForEach(
                                (indb) =>
                                {
                                    var ione = accountCollection.Value.FirstOrDefault(v => v.AccountId == indb.AccountId);

                                    var update = Update.Set("accountStatus", ione.AccountStatus)
                                        .Set("lastUpdatedDate", ione.LastUpdatedDate);

                                    var updatedItem = helper.Collection.FindAndModify(
                                        Query.EQ("accountId", indb.AccountId),
                                        null,
                                        update);

                                    var ifMessage = updatedItem.ErrorMessage;
                                });

                            accountCollection.Value.Where(w => !foundItems.Any(fi => fi.AccountId == w.AccountId)).ToList().ForEach(
                                (notindb) =>
                                {
                                    notindb.Date = DateTime.Now;
                                    var insertedItem = helper.Collection.Insert(notindb);
                                    var insertedItemId = insertedItem.Upserted;
                                    var insertUpdatedItem = insertedItem.UpdatedExisting;
                                });


                        }
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


        async public Task<ActionResult> WorkItems(string account, DateTime? asOfDate = null)
        {

            ApiCollection<VsoWorkItem> accountCollection;
            _baseUrl = string.Format(base._baseUrl, account);

            var ctoken = this.Request.Cookies["ctoken"];
            if (ctoken != null)
            {
                var ctokenValue = ctoken.Value;

                var message = string.Empty;
                try
                {
                    var realDate = asOfDate ?? DateTime.Now.AddYears(-3);
                    var isoDateTime = realDate.ToString("yyyy-MM-ddTHH:mm:ssZ");

                    using (var client = new HttpClient())
                    {
                        client.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));
                        client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", ctokenValue);

                        message = await GetAsync(client, _baseUrl + "wit/workitems?fields=System.Id,System.Title,System.WorkItemType,Microsoft.VSTS.Scheduling.RemainingWork&asOf=" + isoDateTime);


                        accountCollection = JsonConvert.DeserializeObject<ApiCollection<VsoWorkItem>>(message);

                    }
                }
                catch (Exception ex)
                {
                    @ViewBag.Message = ex.ToString();
                }

            }

            return View();
        }

    }
}