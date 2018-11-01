using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Text;
using System.Web;
using Gateway_Sample_Application.Properties;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace SMS
{
    static class API
    {
        private static string Server = Settings.Default.SERVER;
        private static string Key = Settings.Default.API_KEY;

        /// <summary>
        /// Send single message to specific mobile number.
        /// </summary>
        /// <param name="number">The mobile number where you want to send message.</param>
        /// <param name="message">The message you want to send.</param>
        /// <param name="device">The ID of a device you want to use to send this message.</param>
        /// <exception>If there is an error while sending a message.</exception>
        /// <returns>The dictionary containing information about the message.</returns>
        public static Dictionary<string, object> SendSingleMessage(string number, string message, int device = 0)
        {
            var values = new Dictionary<string, object>
            {
                { "number", number},
                { "message", message},
                { "key", Key },
                { "devices", device }
            };

            return GetMessages(GetResponse($"{Server}/services/send.php", values)["messages"])[0];
        }

        /// <summary>
        /// Send multiple messages to different mobile numbers.
        /// </summary>
        /// <param name="messages">The array containing numbers and messages.</param>
        /// <param name="useAvailable">Set this to true if you want to use all available devices to send these messages.</param>
        /// <param name="devices">The array of ID of devices you want to use to send these messages.</param>
        /// <exception>If there is an error while sending messages.</exception>
        /// <returns>The array containing messages.</returns>
        public static Dictionary<string, object>[] SendMessages(List<Dictionary<string, string>> messages, bool useAvailable = true, int[] devices = null)
        {
            var values = new Dictionary<string, object>
            {
                { "messages", JsonConvert.SerializeObject(messages)},
                { "key", Key },
                { "devices", devices },
                { "useAvailable", useAvailable },
            };

            return GetMessages(GetResponse($"{Server}/services/send.php", values)["messages"]);
        }

        /// <summary>
        /// Get a message using the ID.
        /// </summary>
        /// <param name="id">The ID of a message you want to retrieve.</param>
        /// <exception>If there is an error while getting a message.</exception>
        /// <returns>The dictionary containing information about the message.</returns>
        public static Dictionary<string, object> GetMessageByID(int id)
        {
            var values = new Dictionary<string, object>
            {
                { "key", Key },
                { "id", id }
            };

            return GetMessages(GetResponse($"{Server}/services/read-messages.php", values)["messages"])[0];
        }

        /// <summary>
        /// Get messages using the Group ID.
        /// </summary>
        /// <param name="groupID">The group ID of messages you want to retrieve.</param>
        /// <exception>If there is an error while getting messages.</exception>
        /// <returns>The array containing messages.</returns>
        public static Dictionary<string, object>[] GetMessagesByGroupID(string groupID)
        {
            var values = new Dictionary<string, object>
            {
                { "key", Key },
                { "groupId", groupID }
            };

            return GetMessages(GetResponse($"{Server}/services/read-messages.php", values)["messages"]);
        }

        private static Dictionary<string, object>[] GetMessages(JToken messagesJToken)
        {
            JArray jArray = (JArray)messagesJToken;
            var messages = new Dictionary<string, object>[jArray.Count];
            for (var index = 0; index < jArray.Count; index++)
            {
                messages[index] = jArray[index].ToObject<Dictionary<string, object>>();
            }
            return messages;
        }

        private static JToken GetResponse(string url, Dictionary<string, object> postData)
        {
            var request = (HttpWebRequest)WebRequest.Create(url);
            var dataString = CreateDataString(postData);
            var data = Encoding.UTF8.GetBytes(dataString);

            request.Method = "POST";
            request.ContentType = "application/x-www-form-urlencoded";
            request.ContentLength = data.Length;
            ServicePointManager.Expect100Continue = true;
            ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;
            using (var stream = request.GetRequestStream())
            {
                stream.Write(data, 0, data.Length);
            }

            var response = (HttpWebResponse)request.GetResponse();

            if (response.StatusCode == HttpStatusCode.OK)
            {
                var jsonResponse = new StreamReader(response.GetResponseStream()).ReadToEnd();
                try
                {
                    JObject jObject = JObject.Parse(jsonResponse);
                    if ((bool)jObject["success"])
                    {
                        return jObject["data"];
                    }
                    throw new Exception(jObject["error"]["message"].ToString());
                }
                catch (JsonReaderException)
                {
                    if (string.IsNullOrEmpty(jsonResponse))
                    {
                        throw new InvalidDataException("Missing data in request. Please provide all the required information to send messages.");
                    }
                    throw new Exception(jsonResponse);
                }
            }
            else
            {
                throw new WebException($"HTTP Error : {(int)response.StatusCode} {response.StatusCode}");
            }
        }

        private static string CreateDataString(Dictionary<string, object> data)
        {
            StringBuilder dataString = new StringBuilder();
            bool first = true;
            foreach (var obj in data)
            {
                if (obj.Value != null)
                {
                    if (first)
                    {
                        first = false;
                    }
                    else
                    {
                        dataString.Append("&");
                    }
                    dataString.Append(HttpUtility.UrlEncode(obj.Key));
                    dataString.Append("=");
                    dataString.Append(obj.Value is string[]
                        ? HttpUtility.UrlEncode(JsonConvert.SerializeObject(obj.Value))
                        : HttpUtility.UrlEncode(obj.Value.ToString()));
                }
            }
            return dataString.ToString();
        }
    }
}