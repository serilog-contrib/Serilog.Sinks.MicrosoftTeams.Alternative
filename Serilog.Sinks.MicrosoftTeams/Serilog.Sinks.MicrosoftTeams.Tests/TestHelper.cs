using Microsoft.Net.Http.Server;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Serilog.Events;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Serilog.Sinks.MicrosoftTeams.Tests
{
    internal static class TestHelper
    {
        private const string TestWebHook = "http://localhost:1234/webhook";

        private static async Task<T> WithTimeout<T>(this Task<T> taskToComplete, TimeSpan timeSpan)
        {
            var timeoutCancellationTokenSource = new CancellationTokenSource();
            var delayTask = Task.Delay(timeSpan, timeoutCancellationTokenSource.Token);
            var completedTask = await Task.WhenAny(taskToComplete, delayTask).ConfigureAwait(false);

            if (completedTask == delayTask)
            {
                throw new TimeoutException(string.Format("WithTimeout has timed out after {0}.", timeSpan));
            }

            timeoutCancellationTokenSource.Cancel();
            return await taskToComplete.ConfigureAwait(false);
        }

        public static ILogger CreateLogger()
        {
            var logger = new LoggerConfiguration()
                .MinimumLevel.Verbose()
                .WriteTo.MicrosoftTeams(new MicrosoftTeamsSinkOptions(TestWebHook, "Integration Tests"), LogEventLevel.Verbose)
                .CreateLogger();

            return logger;
        }

        public static async Task<IList<JObject>> CaptureRequestsAsync(int count)
        {
            var settings = new WebListenerSettings();
            settings.UrlPrefixes.Add(TestWebHook);

            var result = new List<JObject>();
            using (var listener = new WebListener(settings))
            {
                listener.Start();

                while (count-- > 0)
                {
                    using (var requestContext = await listener.AcceptAsync().WithTimeout(TimeSpan.FromSeconds(6)).ConfigureAwait(false))
                    {
                        var body = ReadBodyStream(requestContext.Request.Body);
                        result.Add(body);
                        requestContext.Response.StatusCode = 204;
                    }
                }
            }

            return result;
        }

        private static JObject ReadBodyStream(Stream stream)
        {
            using (var reader = new StreamReader(stream, Encoding.UTF8))
            {
                var json = reader.ReadToEnd();
                return JsonConvert.DeserializeObject<JObject>(json);
            }
        }

        public static JObject CreateMessage(string template, string renderedMessage, LogEventLevel logEventLevel,
            string color, int counter)
        {
            return new JObject
            {
                ["@type"] = "MessageCard",
                ["@context"] = "http://schema.org/extensions",
                ["title"] = "Integration Tests",
                ["text"] = renderedMessage,
                ["themeColor"] = color,
                ["sections"] = new JArray
                {
                    new JObject
                    {
                        ["title"] = "Properties",
                        ["facts"] = new JArray
                        {
                            new JObject
                            {
                                ["name"] = "Level",
                                ["value"] = logEventLevel.ToString()
                            },
                            new JObject
                            {
                                ["name"] = "MessageTemplate",
                                ["value"] = template
                            },
                            new JObject
                            {
                                ["name"] = "counter",
                                ["value"] = counter
                            }
                        }
                    },
                }
            };
        }
    }
}