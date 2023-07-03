using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.IO;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using OpenBLive.Runtime.Data;
using OpenBLive.Runtime.Utilities;
using Logger = OpenBLive.Runtime.Utilities.Logger;
#if NET5_0_OR_GREATER
using System.Net;
#elif UNITY_2020_3_OR_NEWER
using UnityEngine.Networking;
#endif

namespace OpenBLive.Runtime
{
    /// <summary>
    /// 各类b站api
    /// </summary>
    public static class BApi
    {
        /// <summary>
        /// 是否为测试环境的api
        /// </summary>
        public static bool isTestEnv;

        /// <summary>
        /// 开放平台域名
        /// </summary>
        private static string OpenLiveDomain =>
            isTestEnv ? "http://test-live-open.biliapi.net" : "https://live-open.biliapi.com";

        /// <summary>
        /// 获取长链地址(需要access_key_id与access_key_secret)
        /// </summary>
        private const string k_WssInfoApi = "/v1/common/websocketInfo";

        /// <summary>
        /// 客户端获取长链地址
        /// </summary>
        private const string k_WssInfoByClientApi = "/v1/common/websocketInfoByClient?csrf=";

        /// <summary>
        /// 房间信息聚合接口 “stream”-流信息 “show”-展示相关 “status”-状态 “area”-分区信息
        /// </summary>
        private const string k_RoomDetailApi = "/v1/common/roomDetail";

        /// <summary>
        /// 根据分区和子分区拉取房间列表和初步信息（在白名单中）一次拉取最多50条
        /// </summary>
        private const string k_RoomListApi = "/v1/common/roomListByArea";

        /// <summary>
        /// 获取房间长号和短号
        /// </summary>
        private const string k_RoomIdApi = "/v1/common/roomIdInfo";

        /// <summary>
        /// 互动游戏开启
        /// </summary>
        private const string k_InteractivePlayStart = "/v1/app/start";

        /// <summary>
        /// 互动游戏关闭
        /// </summary>
        private const string k_InteractivePlayEnd = "/v1/app/end";

        /// <summary>
        /// 互动游戏心跳
        /// </summary>
        private const string k_InteractivePlayHeartBeat = "/v1/app/heartbeat";

        /// <summary>
        /// 互动游戏批量心跳
        /// </summary>
        private const string k_InteractivePlayBatchHeartBeat = "/v1/app/batchHeartbeat";
        /// <summary>
        /// 小程序开始
        /// </summary>
        private const string k_PluginStart = "/v1/app/pluginStart";


        private const string k_Post = "POST";
        private const string k_Get = "GET";


        /// <summary>
        /// 通过房间号获取长链
        /// </summary>
        /// <param name="roomId"></param>
        /// <returns></returns>
        public static async Task<WebsocketInfo> GetWebsocketInfoViaPass(long roomId)
        {
            WebsocketInfo websocketInfo = default;
            var postUrl = OpenLiveDomain + k_WssInfoApi;
            var param = $"{{\"room_id\":{roomId}}}";

            var result = await RequestWebUTF8(postUrl, k_Post, param);

            if (!string.IsNullOrEmpty(result))
            {
                try
                {
                    websocketInfo = JsonConvert.DeserializeObject<WebsocketInfo>(result);
                }
                catch (Exception e)
                {
                    Logger.LogError("转换wssInfo失败，返回数据： " + result + e.Message);
                }
            }


            return websocketInfo;
        }

        /// <summary>
        /// 通过b站登录消息获取长链
        /// </summary>
        /// <param name="auth"></param>
        /// <returns></returns>
        public static async Task<WebsocketInfo> GetWebsocketInfoViaBToken(BToken auth)
        {
            WebsocketInfo websocketInfo = default;
            var postUrl = OpenLiveDomain + k_WssInfoByClientApi + auth.biliJct;
            var param = $"{{\"uid\":{123}}}";

            StringBuilder sb = new StringBuilder();
            sb.Append("bili_jct=");
            sb.Append(auth.biliJct);
            sb.Append("; ");
            sb.Append("SESSDATA=");
            sb.Append(auth.sessData);

            var result = await RequestWebUTF8(postUrl, k_Post, param, sb.ToString());
            if (!string.IsNullOrEmpty(result))
            {
                try
                {
                    websocketInfo = JsonConvert.DeserializeObject<WebsocketInfo>(result);
                }
                catch (Exception e)
                {
                    Logger.LogError("转换wssInfo失败，返回数据： " + result + e.Message);
                }
            }

            return websocketInfo;
        }


        /// <summary>
        /// 获取真实房间号
        /// </summary>
        /// <param name="roomId"></param>
        /// <returns></returns>
        public static async Task<long> GetRoomIdInfo(long roomId)
        {
            long realRoomId = 0;
            var postUrl = OpenLiveDomain + k_RoomIdApi;
            var param = $"{{\"id\":{roomId}}}";

            var result = await RequestWebUTF8(postUrl, k_Post, param);
            if (string.IsNullOrEmpty(result)) return realRoomId;
            var json = JObject.Parse(result);
            try
            {
                realRoomId = json["data"]!["room_id"]!.ToObject<long>();
            }
            catch (Exception e)
            {
                Logger.LogError("json解析异常" + result + e.Message);
                throw;
            }

            return realRoomId;
        }

        public static async Task<string> StartInteractivePlay(string roomId, string appId)
        {
            var postUrl = OpenLiveDomain + k_InteractivePlayStart;
            var param = $"{{\"room_id\":{roomId},\"app_id\":{appId}}}";

            var result = await RequestWebUTF8(postUrl, k_Post, param);

            return result;
        }

        public static async Task<string> EndInteractivePlay(string appId, string gameId)
        {
            var postUrl = OpenLiveDomain + k_InteractivePlayEnd;
            var param = $"{{\"app_id\":{appId},\"game_id\":\"{gameId}\"}}";

            var result = await RequestWebUTF8(postUrl, k_Post, param);
            return result;
        }

        public static async Task<string> HeartBeatInteractivePlay(string gameId)
        {
            var postUrl = OpenLiveDomain + k_InteractivePlayHeartBeat;
            string param = "";
            if (gameId != null)
            {
                param = $"{{\"game_id\":\"{gameId}\"}}";
                
            }

            var result = await RequestWebUTF8(postUrl, k_Post, param);
            return result;
        }

        public static async Task<string> BatchHeartBeatInteractivePlay(string[] gameIds)
        {
            var postUrl = OpenLiveDomain + k_InteractivePlayBatchHeartBeat;
            GameIds games = new GameIds()
            {
                gameIds = gameIds
            };
            var param = JsonConvert.SerializeObject(games);
            var result = await RequestWebUTF8(postUrl, k_Post, param);
            return result;
        }

        public static async Task<string> PluginStart(string roomId, string appId)
        {
            var postUrl = OpenLiveDomain + k_PluginStart;
            var param = $"{{\"room_id\":{roomId},\"app_id\":{appId}}}";
            var result = await RequestWebUTF8(postUrl, k_Post, param);
            return result;
        }


        private static async Task<string> RequestWebUTF8(string url, string method, string param,
            string cookie = null)
        {
#if NET5_0_OR_GREATER
            string result = "";
            HttpWebRequest req = (HttpWebRequest) WebRequest.Create(url);
            req.Method = method;

            if (param != null)
            {
                SignUtility.SetReqHeader(req, param, cookie);
            }

            HttpWebResponse httpResponse = (HttpWebResponse) (await req.GetResponseAsync());
            Stream stream = httpResponse.GetResponseStream();

            if (stream != null)
            {
                using StreamReader reader = new StreamReader(stream, Encoding.UTF8);
                result = await reader.ReadToEndAsync();
            }

            return result;

#elif UNITY_2020_3_OR_NEWER
            UnityWebRequest webRequest = new UnityWebRequest(url);
            webRequest.method = method;
            if (param != null)
            {
                SignUtility.SetReqHeader(webRequest, param, cookie);
            }

            webRequest.downloadHandler = new DownloadHandlerBuffer();
            webRequest.disposeUploadHandlerOnDispose = true;
            webRequest.disposeDownloadHandlerOnDispose = true;
            await webRequest.SendWebRequest();
            var text = webRequest.downloadHandler.text;

            webRequest.Dispose();
            return text;
#endif
        }
#if UNITY_2020_3_OR_NEWER
        private static TaskAwaiter GetAwaiter(this UnityEngine.AsyncOperation asyncOp)
        {
            var tcs = new TaskCompletionSource<object>();
            asyncOp.completed += _ => { tcs.SetResult(null); };
            return ((Task) tcs.Task).GetAwaiter();
        }
#endif
    }
}