using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using VSOOAuthClientSample.VSO;
using VSOOAuthClientSample.VSO.BuildApi;

namespace VSOOAuthClientSample.Controllers
{
    public class BuildsController : BaseVsoController
    {
        // GET: Build
        async public Task<ActionResult> Index()
        {
            _baseUrl = String.Format(_baseUrl, _account);

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

                        message = await GetAsync(client, _baseUrl + "build/builds");
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

        async public Task<ActionResult> RunSample()
        {
            var responseBody = String.Empty;

            ApiCollection<BuildDefinition> buildDefinitions;
            BuildRequest queueResult;
            Build buildResult;

            _baseUrl = String.Format(_baseUrl, _account);

            var ctoken = this.Request.Cookies["ctoken"];
            if (ctoken != null)
            {
                var ctokenValue = ctoken.Value;

                using (HttpClient client = new HttpClient())
                {
                    client.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));
                    client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", ctokenValue);

                    FileLogger.Information("==============Getting a list Build Definitions=================");

                    //Get a list of build definitions
                    responseBody = await GetAsync(client, _baseUrl + "build/definitions");

                    buildDefinitions = JsonConvert.DeserializeObject<ApiCollection<BuildDefinition>>(responseBody);

                    if (buildDefinitions.Value.Count() > 0)
                    {
                        foreach (var buildDefinition in buildDefinitions.Value)
                        {
                            FileLogger.Information(String.Format(
                                "{0} - {1}",
                                buildDefinition.Id,
                                buildDefinition.Name
                                ));
                        }

                        FileLogger.Information("==============Queueing a Build=================");

                        //Use the first build definition to queue a build
                        var firstBuildDefinition = buildDefinitions.Value.First<BuildDefinition>();

                        FileLogger.Information(String.Format(
                            "Queuing a build using build definition {0}...",
                            firstBuildDefinition.Id
                            ));

                        //Queue a build
                        var buildRequestPOSTData =
                            new BuildRequest()
                            {
                                Definition = new Definition()
                                {
                                    Id = firstBuildDefinition.Id
                                },
                                Priority = Priority.Normal,
                                Reason = Reason.Manual

                            };


                        var content = new StringContent(
                            JsonConvert.SerializeObject(buildRequestPOSTData),
                            Encoding.UTF8,
                            "application/json");
                        
                        responseBody = await PostAsync(client, content, _baseUrl + "build/requests");

                        queueResult = JsonConvert.DeserializeObject<BuildRequest>(responseBody);

                        FileLogger.Information(String.Format(
                             "Build request submitted successfully - ID: {0}",
                             queueResult.Id
                             ));

                        FileLogger.Information("==============Waiting for Build to Start Running=================");

                        //poll the server till the build is out of the queue
                        while (!String.IsNullOrEmpty(queueResult.Status) &&
                              queueResult.Status == "queued")
                        {
                            //Wait a while before polling the server
                            Thread.Sleep(8000);

                            FileLogger.Information("Polling the server...");

                            responseBody = await GetAsync(client,
                                String.Format(queueResult.Url,
                                queueResult.Id
                                ));

                            queueResult = JsonConvert.DeserializeObject<BuildRequest>(responseBody);

                            FileLogger.Information(String.Format("Build queue status: {0}", queueResult.Status));
                        }

                        FileLogger.Information("==============Waiting for Build to Complete=================");

                        //Get the status of the build we queued
                        responseBody = await GetAsync(client,
                                String.Format(queueResult.Builds.First<Build>().Url,
                                queueResult.Builds.First<Build>().Id
                                ));

                        buildResult = JsonConvert.DeserializeObject<Build>(responseBody);

                        //Poll until the build either fails or succeeds
                        while (buildResult.Status == BuildStatus.NotStarted
                               || buildResult.Status == BuildStatus.InProgress)
                        {
                            //Wait a while before polling the server
                            Thread.Sleep(8000);

                            //Get a Build 
                            responseBody = await GetAsync(client,
                                    String.Format(buildResult.Url,
                                    queueResult.Builds.First<Build>().Id
                                    ));

                            buildResult = JsonConvert.DeserializeObject<Build>(responseBody);

                            FileLogger.Information(String.Format(
                                 "Build {0}: {1}",
                                 buildResult.Id,
                                 buildResult.Status
                                 ));
                        }

                        if (buildResult.Status == BuildStatus.Succeeded)
                        {
                            FileLogger.Information(String.Format(
                                     "Build {0} drop: {1}  ",
                                     buildResult.Id,
                                     buildResult.BuildDrop.Url
                                     ));
                        }
                    }
                    else
                    {
                        FileLogger.Information("No build definitions found....");
                    }

                }
            }

            FileLogger.Information("Done...");

            return View();

        }


    }
}