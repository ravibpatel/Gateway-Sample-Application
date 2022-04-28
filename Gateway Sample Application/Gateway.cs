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
        /// <param name="schedule">Set it to timestamp when you want to send this message.</param>
        /// <param name="isMMS">Set it to true if you want to send MMS message instead of SMS.</param>
        /// <param name="attachments">Comma separated list of image links you want to attach to the message. Only works for MMS messages.</param>
        /// <param name="prioritize">Set it to true if you want to prioritize this message.</param>
        /// <exception>If there is an error while sending a message.</exception>
        /// <returns>The dictionary containing information about the message.</returns>
        public static Dictionary<string, object> SendSingleMessage(string number, string message, string device = "0",
            long? schedule = null, bool isMMS = false, string attachments = null, bool prioritize = false)
        {
            var values = new Dictionary<string, object>
            {
                { "number", number },
                { "message", message },
                { "schedule", schedule },
                { "key", Key },
                { "devices", device },
                { "type", isMMS ? "mms" : "sms" },
                { "attachments", attachments },
                { "prioritize", prioritize ? 1 : 0 }
            };

            return GetObjects(GetResponse($"{Server}/services/send.php", values)["messages"])[0];
        }

        /// <summary>
        /// Send multiple messages to different mobile numbers.
        /// </summary>
        /// <param name="messages">The array containing numbers and messages.</param>
        /// <param name="option">Set this to USE_SPECIFIED if you want to use devices and SIMs specified in devices argument.
        /// Set this to USE_ALL_DEVICES if you want to use all available devices and their default SIM to send messages.
        /// Set this to USE_ALL_SIMS if you want to use all available devices and all their SIMs to send messages.</param>
        /// <param name="devices">The array of ID of devices you want to use to send these messages.</param>
        /// <param name="schedule">Set it to timestamp when you want to send this message.</param>
        /// <param name="useRandomDevice">Set it to true if you want to send messages using only one random device from selected devices.</param>
        /// <exception>If there is an error while sending messages.</exception>
        /// <returns>The array containing messages.</returns>
        public static Dictionary<string, object>[] SendMessages(List<Dictionary<string, string>> messages,
            Option option = Option.USE_SPECIFIED, string[] devices = null, long? schedule = null,
            bool useRandomDevice = false)
        {
            var values = new Dictionary<string, object>
            {
                { "messages", JsonConvert.SerializeObject(messages) },
                { "schedule", schedule },
                { "key", Key },
                { "devices", devices },
                { "option", (int)option },
                { "useRandomDevice", useRandomDevice }
            };

            return GetObjects(GetResponse($"{Server}/services/send.php", values)["messages"]);
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
        /// <param name="schedule">Set it to timestamp when you want to send this message.</param>
        /// <param name="isMMS">Set it to true if you want to send MMS message instead of SMS.</param>
        /// <param name="attachments">Comma separated list of image links you want to attach to the message. Only works for MMS messages.</param>
        /// <exception>If there is an error while sending messages.</exception>
        /// <returns>The array containing messages.</returns>
        public static Dictionary<string, object>[] SendMessageToContactsList(int listID, string message,
            Option option = Option.USE_SPECIFIED, string[] devices = null, long? schedule = null, bool isMMS = false,
            string attachments = null)
        {
            var values = new Dictionary<string, object>
            {
                { "listID", listID },
                { "message", message },
                { "schedule", schedule },
                { "key", Key },
                { "devices", devices },
                { "option", (int)option },
                { "type", isMMS ? "mms" : "sms" },
                { "attachments", attachments }
            };

            return GetObjects(GetResponse($"{Server}/services/send.php", values)["messages"]);
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

            return GetObjects(GetResponse($"{Server}/services/read-messages.php", values)["messages"])[0];
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

            return GetObjects(GetResponse($"{Server}/services/read-messages.php", values)["messages"]);
        }

        /// <summary>
        /// Get messages using the status.
        /// </summary>
        /// <param name="status">The status of messages you want to retrieve.</param>
        /// <param name="deviceID">The deviceID of the device which messages you want to retrieve.</param>
        /// <param name="simSlot">Sim slot of the device which messages you want to retrieve. Similar to array index. 1st slot is 0 and 2nd is 1.</param>
        /// <param name="startTimestamp">Search for messages sent or received after this time.</param>
        /// <param name="endTimestamp">Search for messages sent or received before this time.</param>
        /// <exception>If there is an error while getting messages.</exception>
        /// <returns>The array containing messages.</returns>
        public static Dictionary<string, object>[] GetMessagesByStatus(string status, int? deviceID = null,
            int? simSlot = null, long? startTimestamp = null,
            long? endTimestamp = null)
        {
            var values = new Dictionary<string, object>
            {
                { "key", Key },
                { "status", status },
                { "deviceID", deviceID },
                { "simSlot", simSlot },
                { "startTimestamp", startTimestamp },
                { "endTimestamp", endTimestamp }
            };

            return GetObjects(GetResponse($"{Server}/services/read-messages.php", values)["messages"]);
        }

        /// <summary>
        /// Resend a message using the ID.
        /// </summary>
        /// <param name="id">The ID of a message you want to resend.</param>
        /// <exception>If there is an error while resending a message.</exception>
        /// <returns>The dictionary containing information about the message.</returns>
        public static Dictionary<string, object> ResendMessageByID(int id)
        {
            var values = new Dictionary<string, object>
            {
                { "key", Key },
                { "id", id }
            };

            return GetObjects(GetResponse($"{Server}/services/resend.php", values)["messages"])[0];
        }

        /// <summary>
        /// Resend messages using the Group ID.
        /// </summary>
        /// <param name="groupID">The group ID of messages you want to resend.</param>
        /// <param name="status">The status of messages you want to resend.</param>
        /// <exception>If there is an error while resending messages.</exception>
        /// <returns>The array containing messages.</returns>
        public static Dictionary<string, object>[] ResendMessagesByGroupID(string groupID, string status = null)
        {
            var values = new Dictionary<string, object>
            {
                { "key", Key },
                { "groupId", groupID },
                { "status", status }
            };

            return GetObjects(GetResponse($"{Server}/services/resend.php", values)["messages"]);
        }

        /// <summary>
        /// Resend messages using the status.
        /// </summary>
        /// <param name="status">The status of messages you want to resend.</param>
        /// <param name="deviceID">The deviceID of the device which messages you want to resend.</param>
        /// <param name="simSlot">Sim slot of the device which messages you want to resend. Similar to array index. 1st slot is 0 and 2nd is 1.</param>
        /// <param name="startTimestamp">Resend messages sent or received after this time.</param>
        /// <param name="endTimestamp">Resend messages sent or received before this time.</param>
        /// <exception>If there is an error while resending messages.</exception>
        /// <returns>The array containing messages.</returns>
        public static Dictionary<string, object>[] ResendMessagesByStatus(string status, int? deviceID = null,
            int? simSlot = null, long? startTimestamp = null,
            long? endTimestamp = null)
        {
            var values = new Dictionary<string, object>
            {
                { "key", Key },
                { "status", status },
                { "deviceID", deviceID },
                { "simSlot", simSlot },
                { "startTimestamp", startTimestamp },
                { "endTimestamp", endTimestamp }
            };

            return GetObjects(GetResponse($"{Server}/services/resend.php", values)["messages"]);
        }

        /// <summary>
        /// Add a new contact to contacts list.
        /// </summary>
        /// <param name="listID">The ID of the contacts list where you want to add this contact.</param>
        /// <param name="number">The mobile number of the contact.</param>
        /// <param name="name">The name of the contact.</param>
        /// <param name="resubscribe">Set it to true if you want to resubscribe this contact if it already exists.</param>
        /// <returns>A dictionary containing details about a newly added contact.</returns>
        public static Dictionary<string, object> AddContact(int listID, string number, string name = null,
            bool resubscribe = false)
        {
            var values = new Dictionary<string, object>
            {
                { "key", Key },
                { "listID", listID },
                { "number", number },
                { "name", name },
                { "resubscribe", resubscribe ? '1' : '0' },
            };
            JObject jObject = (JObject)GetResponse($"{Server}/services/manage-contacts.php", values)["contact"];
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
                { "key", Key },
                { "listID", listID },
                { "number", number },
                { "unsubscribe", '1' }
            };
            JObject jObject = (JObject)GetResponse($"{Server}/services/manage-contacts.php", values)["contact"];
            return jObject.ToObject<Dictionary<string, object>>();
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
                { "key", Key }
            };
            JToken credits = GetResponse($"{Server}/services/send.php", values)["credits"];
            if (credits.Type != JTokenType.Null)
            {
                return credits.ToString();
            }

            return "Unlimited";
        }

        /// <summary>
        /// Send USSD request.
        /// </summary>
        /// <param name="request">USSD request you want to execute. e.g. *150#</param>
        /// <param name="device">The ID of a device you want to use to send this message.</param>
        /// <param name="simSlot">Sim you want to use for this USSD request. Similar to array index. 1st slot is 0 and 2nd is 1.</param>
        /// <exception>If there is an error while sending a USSD request.</exception>
        /// <returns>A dictionary containing details about USSD request that was sent.</returns>
        public static Dictionary<string, object> SendUssdRequest(string request, int device, int? simSlot = null)
        {
            var values = new Dictionary<string, object>
            {
                { "key", Key },
                { "request", request },
                { "device", device },
                { "sim", simSlot }
            };

            JObject jObject = (JObject)GetResponse($"{Server}/services/send-ussd-request.php", values)["request"];
            return jObject.ToObject<Dictionary<string, object>>();
        }

        /// <summary>
        /// Get a USSD request using the ID.
        /// </summary>
        /// <param name="id">The ID of a USSD request you want to retrieve.</param>
        /// <exception>If there is an error while getting a USSD request.</exception>
        /// <returns>A dictionary containing details about USSD request you requested.</returns>
        public static Dictionary<string, object> GetUssdRequestByID(int id)
        {
            var values = new Dictionary<string, object>
            {
                { "key", Key },
                { "id", id }
            };

            return GetObjects(GetResponse($"{Server}/services/read-ussd-requests.php", values)["requests"])[0];
        }

        /// <summary>
        /// Get USSD requests using the request text.
        /// </summary>
        /// <param name="request">The request text you want to look for.</param>
        /// <param name="deviceID">The deviceID of the device which USSD requests you want to retrieve.</param>
        /// <param name="simSlot">Sim slot of the device which USSD requests you want to retrieve. Similar to array index. 1st slot is 0 and 2nd is 1.</param>
        /// <param name="startTimestamp">Search for USSD requests sent after this time.</param>
        /// <param name="endTimestamp">Search for USSD requests sent before this time.</param>
        /// <exception>If there is an error while getting USSD requests.</exception>
        /// <returns>The array containing USSD requests.</returns>
        public static Dictionary<string, object>[] GetUssdRequests(string request, int? deviceID = null,
            int? simSlot = null, int? startTimestamp = null, int? endTimestamp = null)
        {
            var values = new Dictionary<string, object>
            {
                { "key", Key },
                { "request", request },
                { "deviceID", deviceID },
                { "simSlot", simSlot },
                { "startTimestamp", startTimestamp },
                { "endTimestamp", endTimestamp }
            };

            return GetObjects(GetResponse($"{Server}/services/read-ussd-requests.php", values)["requests"]);
        }

        /// <summary>
        /// Get all enabled devices.
        /// </summary>
        /// <exception>If there is an error while getting devices.</exception>
        /// <returns>The array containing all enabled devices</returns>
        public static Dictionary<string, object>[] GetDevices()
        {
            var values = new Dictionary<string, object>
            {
                { "key", Key }
            };

            return GetObjects(GetResponse($"{Server}/services/get-devices.php", values)["devices"]);
        }

        private static Dictionary<string, object>[] GetObjects(JToken messagesJToken)
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
                            throw new InvalidDataException(
                                "Missing data in request. Please provide all the required information to send messages.");
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