using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using FirebaseAdmin;
using FirebaseAdmin.Messaging;
using Google.Apis.Auth.OAuth2;
using Nancy.Json;

namespace MojGrad.Services
{

    public enum tipNotifikacije
    {
        resenje,
        ocena
    }

    public static class Notifications
    {
        private static string serverKey = "AAAAtz6lsB4:APA91bH5HECGp7u82kM1jf6RCY5Tu1Cc4fmu9RcxpDnuF8O_2zK8VdtltJwFBPcamOeLB6rhdCKA-kkfxUrbZLdK_Iaq0Ga1emifaspBjk3AuKD-ltgj0ALxgsvbZiPMrYnO1W9dGE-3";
        private static string senderId = "787030061086";
        private static string webAddr = "https://fcm.googleapis.com/fcm/send";

        public static string dodatoResenje(string DeviceToken, long postId,int tip)
        {
            string title;
            
            string msg;

            string type;
            if (tip == (int)tipNotifikacije.resenje)
            {
                title = "Obaveštenje";
                msg = "neko je dodao rešenje";
                type = "resenje";
            }
            else 
            {
                title = "Obaveštenje";
                msg = "neko je ocenio vaše rešenje";

                type = "ocena";
            }
            var result = "-1";
            var httpWebRequest = (HttpWebRequest)WebRequest.Create(webAddr);
            httpWebRequest.ContentType = "application/json";
            httpWebRequest.Headers.Add(string.Format("Authorization: key={0}", serverKey));
            httpWebRequest.Headers.Add(string.Format("Sender: id={0}", senderId));
            httpWebRequest.Method = "POST";

            var payload = new
            {
                to = DeviceToken,
                priority = "high",
                content_available = true,
                notification = new
                {
                    body = msg,
                    title = title
                },
                data = new
                {
                    body = msg,
                    title=title,
                    postId=postId,
                    type=type
                }
            };
            var serializer = new JavaScriptSerializer();
            using (var streamWriter = new StreamWriter(httpWebRequest.GetRequestStream()))
            {
                string json = serializer.Serialize(payload);
                streamWriter.Write(json);
                streamWriter.Flush();
            }

            var httpResponse = (HttpWebResponse)httpWebRequest.GetResponse();
            using (var streamReader = new StreamReader(httpResponse.GetResponseStream()))
            {
                result = streamReader.ReadToEnd();
            }
            return result;
        }

        public static string ocenjenoResenje(string DeviceToken,long postId)
        {
            string title = "Obaveštenje";
            string msg = "neko je ocenio vaše rešenje";
            var result = "-1";
            var httpWebRequest = (HttpWebRequest)WebRequest.Create(webAddr);
            httpWebRequest.ContentType = "application/json";
            httpWebRequest.Headers.Add(string.Format("Authorization: key={0}", serverKey));
            httpWebRequest.Headers.Add(string.Format("Sender: id={0}", senderId));
            httpWebRequest.Method = "POST";

            var payload = new
            {
                to = DeviceToken,
                priority = "high",
                content_available = true,
                notification = new
                {
                    body = msg,
                    title = title
                },
                data = new
                {
                    body = msg,
                    title = title,
                    postId = postId,
                    type = "ocena"
                }
            };
            var serializer = new JavaScriptSerializer();
            using (var streamWriter = new StreamWriter(httpWebRequest.GetRequestStream()))
            {
                string json = serializer.Serialize(payload);
                streamWriter.Write(json);
                streamWriter.Flush();
            }

            var httpResponse = (HttpWebResponse)httpWebRequest.GetResponse();
            using (var streamReader = new StreamReader(httpResponse.GetResponseStream()))
            {
                result = streamReader.ReadToEnd();
            }
            return result;
        }
    }


}
