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
        private static readonly string Server = Settings.Default.SERVER;
        private static readonly string Key = Settings.Default.API_KEY;

        public enum Option
        {
            USE_SPECIFIED = 0,
            USE_ALL_DEVICES = 1,
            USE_ALL_SIMS = 2
        }

        /// <summary>
        /// Send single message to specific mobile number.
        /// </summary>
        /// <param name="number">The mobile number where you want to send message.</param>
        /// <param name="message">The message you want to send.</param>
        /// <param name="device">The ID of a device you want to use to send this message.</param>
        /// <exception>If there is an error while sending a message.</exception>
        /// <returns>The dictionary containing information about the message.</returns>
        public static Dictionary<string, object> SendSingleMessage(string number, string message, string device = "0")
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
        /// <param name="option">Set this to USE_SPECIFIED if you want to use devices and SIMs specified in devices argument.
        /// Set this to USE_ALL_DEVICES if you want to use all available devices and their default SIM to send messages.
        /// Set this to USE_ALL_SIMS if you want to use all available devices and all their SIMs to send messages.</param>
        /// <param name="devices">The array of ID of devices you want to use to send these messages.</param>
        /// <exception>If there is an error while sending messages.</exception>
        /// <returns>The array containing messages.</returns>
        public static Dictionary<string, object>[] SendMessages(List<Dictionary<string, string>> messages, Option option = Option.USE_SPECIFIED, string[] devices = null)
        {
            var values = new Dictionary<string, object>
            {
                { "messages", JsonConvert.SerializeObject(messages)},
                { "key", Key },
                { "devices", devices },
                { "option", (int) option }
            };

            return GetMessages(GetResponse($"{Server}/services/send.php", values)["messages"]);
        }

        /// <summary>
        /// Send a message to contacts in specified contacts list.
        /// </summary>
        /// <param name="listID">The ID of the contacts list where you want to send this message.</param>
        /// <param name="message">The message you want to send.</param>
        /// <param name="option">Set this to USE_SPECIFIED if you want to use devices and SIMs specified in devices argument.
        /// Set this to USE_ALL_DEVICES if you want to use all available devices and their default SIM to send messages.
        /// Set this to USE_ALL_SIMS if you want to use all available devices and all their SIMs to send messages.</param>
        /// <param name="devices">The array of ID of devices you want to use to send these messages.</param>
        /// <exception>If there is an error while sending messages.</exception>
        /// <returns>The array containing messages.</returns>
        public static Dictionary<string, object>[] SendMessageToContactsList(int listID, string message, Option option = Option.USE_SPECIFIED, string[] devices = null)
        {
            var values = new Dictionary<string, object>
            {
                { "listID", listID},
                { "message", message},
                { "key", Key },
                { "devices", devices },
                { "option", (int) option }
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

        /// <summary>
        /// Get remaining message credits.
        /// </summary>
        /// <exception>If there is an error while getting message credits.</exception>
        /// <returns>The amount of message credits left.</returns>
        public static string GetBalance()
        {
            var values = new Dictionary<string, object>
            {
                {"key", Key}
            };
            JToken credits = GetResponse($"{Server}/services/send.php", values)["credits"];
            if (credits.Type != JTokenType.Null)
            {
                return credits.ToString();
            }
            return "Unlimited";
        }

        /// <summary>
        /// Add a new contact to contacts list.
        /// </summary>
        /// <param name="listID">The ID of the contacts list where you want to add this contact.</param>
        /// <param name="number">The mobile number of the contact.</param>
        /// <param name="name">The name of the contact.</param>
        /// <param name="resubscribe">Set it to true if you want to resubscribe this contact if it already exists.</param>
        /// <returns>A dictionary containing details about a newly added contact.</returns>
        public static Dictionary<string, object> AddContact(int listID, string number, string name = null, bool resubscribe = false)
        {
            var values = new Dictionary<string, object>
            {
                {"key", Key},
                {"listID", listID},
                {"number", number},
                {"name", name},
                {"resubscribe", resubscribe ? '1' : '0'},
            };
            JObject jObject = (JObject) GetResponse($"{Server}/services/manage-contacts.php", values)["contact"];
            return jObject.ToObject<Dictionary<string, object>>();
        }

        /// <summary>
        /// Unsubscribe a contact from the contacts list.
        /// </summary>
        /// <param name="listID">The ID of the contacts list from which you want to unsubscribe this contact.</param>
        /// <param name="number">The mobile number of the contact.</param>
        /// <returns>A dictionary containing details about the unsubscribed contact.</returns>
        public static Dictionary<string, object> UnsubscribeContact(int listID, string number)
        {
            var values = new Dictionary<string, object>
            {
                {"key", Key},
                {"listID", listID},
                {"number", number},
                {"unsubscribe", '1'}
            };
            JObject jObject = (JObject)GetResponse($"{Server}/services/manage-contacts.php", values)["contact"];
            return jObject.ToObject<Dictionary<string, object>>();
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
                using (StreamReader streamReader = new StreamReader(response.GetResponseStream()))
                {
                    var jsonResponse = streamReader.ReadToEnd();
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
            }

            throw new WebException($"HTTP Error : {(int)response.StatusCode} {response.StatusCode}");
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