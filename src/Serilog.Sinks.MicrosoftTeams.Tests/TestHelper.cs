// --------------------------------------------------------------------------------------------------------------------
// <copyright file="TestHelper.cs" company="Hämmer Electronics">
// The project is licensed under the MIT license.
// </copyright>
// <summary>
//   A helper class for the tests.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Serilog.Sinks.MicrosoftTeams.Tests
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Text;
    using System.Threading;
    using System.Threading.Tasks;

    using Microsoft.Net.Http.Server;

    using Newtonsoft.Json;
    using Newtonsoft.Json.Linq;

    using Serilog.Events;

    /// <summary>
    /// A helper class for the tests.
    /// </summary>
    internal static class TestHelper
    {
        /// <summary>
        /// The test web hook URL.
        /// </summary>
        private const string TestWebHook = "http://localhost:1234/webhook";

        /// <summary>
        /// Adds a timeout to the functionality.
        /// </summary>
        /// <typeparam name="T">The type parameter.</typeparam>
        /// <param name="taskToComplete">The task to run.</param>
        /// <param name="timeSpan">The time span.</param>
        /// <returns>A <see cref="Task"/> representing any asynchronous operation.</returns>
        private static async Task<T> WithTimeout<T>(this Task<T> taskToComplete, TimeSpan timeSpan)
        {
            var timeoutCancellationTokenSource = new CancellationTokenSource();
            var delayTask = Task.Delay(timeSpan, timeoutCancellationTokenSource.Token);
            var completedTask = await Task.WhenAny(taskToComplete, delayTask).ConfigureAwait(false);

            if (completedTask == delayTask)
            {
                throw new TimeoutException($"WithTimeout has timed out after {timeSpan}.");
            }

            timeoutCancellationTokenSource.Cancel();
            return await taskToComplete.ConfigureAwait(false);
        }

        /// <summary>
        /// Creates the logger.
        /// </summary>
        /// <param name="omitPropertiesSection">A value indicating whether the properties should be omitted or not.</param>
        /// <returns>An <see cref="ILogger"/>.</returns>
        public static ILogger CreateLogger(bool omitPropertiesSection = false)
        {
            var logger = new LoggerConfiguration()
                .MinimumLevel.Verbose()
                .WriteTo.MicrosoftTeams(new MicrosoftTeamsSinkOptions(TestWebHook, "Integration Tests", omitPropertiesSection: omitPropertiesSection))
                .CreateLogger();

            return logger;
        }

        /// <summary>
        /// Creates the logger.
        /// </summary>
        /// <param name="buttons">Buttons to output</param>
        /// <returns>An <see cref="ILogger"/>.</returns>
        public static ILogger CreateLoggerWithButtons(IEnumerable<MicrosoftTeamsSinkOptionsButton> buttons)
        {
            var logger = new LoggerConfiguration()
                .MinimumLevel.Verbose()
                .WriteTo.MicrosoftTeams(new MicrosoftTeamsSinkOptions(TestWebHook, "Integration Tests", buttons: buttons, omitPropertiesSection: true))
                .CreateLogger();

            return logger;
        }

        /// <summary>
        /// Captures the requests.
        /// </summary>
        /// <param name="count">The counter variable.</param>
        /// <returns>A <see cref="Task"/> representing any asynchronous operation.</returns>
        public static async Task<IList<JObject>> CaptureRequestsAsync(int count)
        {
            var settings = new WebListenerSettings();
            settings.UrlPrefixes.Add(TestWebHook);

            var result = new List<JObject>();
            using var listener = new WebListener(settings);
            listener.Start();

            while (count-- > 0)
            {
                using var requestContext = await listener.AcceptAsync().WithTimeout(TimeSpan.FromSeconds(6)).ConfigureAwait(false);
                var body = ReadBodyStream(requestContext.Request.Body);
                result.Add(body);
                requestContext.Response.StatusCode = 204;
            }

            return result;
        }

        /// <summary>
        /// Reads the body stream.
        /// </summary>
        /// <param name="stream">The body stream.</param>
        /// <returns>A <see cref="JObject"/> from the body stream.</returns>
        private static JObject ReadBodyStream(Stream stream)
        {
            using var reader = new StreamReader(stream, Encoding.UTF8);
            var json = reader.ReadToEnd();
            return JsonConvert.DeserializeObject<JObject>(json);
        }

        /// <summary>
        /// Creates the message.
        /// </summary>
        /// <param name="template">The template.</param>
        /// <param name="renderedMessage">The rendered message.</param>
        /// <param name="logEventLevel">The log event level.</param>
        /// <param name="color">The color.</param>
        /// <param name="counter">The counter.</param>
        /// <param name="occuredOn">The occured on date.</param>
        /// <returns>A <see cref="JObject"/> from the message.</returns>
        public static JObject CreateMessage(string template, string renderedMessage, LogEventLevel logEventLevel,
            string color, int counter, string occuredOn)
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
                            },
                            new JObject
                            {
                                ["name"] = "Occured on",
                                ["value"] = occuredOn
                            }
                        }
                    }
                }
            };
        }

        public static JObject CreateMessage(string renderedMessage, string color)
        {
            return new JObject
            {
                ["@type"] = "MessageCard",
                ["@context"] = "http://schema.org/extensions",
                ["title"] = "Integration Tests",
                ["text"] = renderedMessage,
                ["themeColor"] = color
            };
        }

        public static JObject CreateMessageWithButton(string renderedMessage, string color, IEnumerable<MicrosoftTeamsSinkOptionsButton> buttons)
        {
            var potentialAction = new JArray();
            foreach (var microsoftTeamsSinkOptionsButton in buttons)
            {
                var b = new JObject
                {
                    ["@type"] = "OpenUri",
                    ["name"] = microsoftTeamsSinkOptionsButton.Name,
                    ["targets"] = new JArray
                    {
                        new JObject
                        {
                            ["uri"] = microsoftTeamsSinkOptionsButton.Uri,
                            ["os"] = "default"
                        }
                    }
                };
                potentialAction.Add(b);
            }

            return new JObject
            {
                ["@type"] = "MessageCard",
                ["@context"] = "http://schema.org/extensions",
                ["title"] = "Integration Tests",
                ["text"] = renderedMessage,
                ["themeColor"] = color,
                ["potentialAction"] = potentialAction
            };
        }
    }
}