﻿using Configuration.Helper;
using Newtonsoft.Json;
using NLog;
using RestSharp;
using System;
using Model;

namespace WeChat.Core
{
    /// <summary>
    /// 微信接口处理类
    /// </summary>
    public class WeChatServiceHandler
    {
        #region field
        private readonly IRestClient _client;
        #endregion

        #region log
        private readonly Logger logger = LogManager.GetCurrentClassLogger();
        #endregion

        #region .ctor
        public WeChatServiceHandler(IRestClient client)
        {
            _client = client;
			_client.BaseUrl = new Uri(ConfigurationHelper.WeChatApiAddr);
        }
        #endregion

        #region 微信方法
        /// <summary>
        /// 获取OpenId
        /// </summary>
        /// <param name="code"></param>
        /// <returns>成功时返回OpenIdResultSuccess，失败时返回OpenIdResultError</returns>
        public dynamic GetOpenId(string code)
        {
            try
            {
                IRestRequest request = new RestRequest("sns/jscode2session", Method.GET);
                request.AddParameter("appid", ConfigurationHelper.AppId);
                request.AddParameter("secret", ConfigurationHelper.AppSecret);
                request.AddParameter("js_code", code);
                request.AddParameter("grant_type", "authorization_code");

                IRestResponse response = _client.Execute(request);
                if (response.Content.Contains("openid"))
                {
                    OpenIdResultSuccess result = JsonConvert.DeserializeObject<OpenIdResultSuccess>(response.Content);
                    return result;
                }
                else
                {
                    OpenIdResultFail result = JsonConvert.DeserializeObject<OpenIdResultFail>(response.Content);
                    return result;
                }
            }
            catch (Exception ex)
            {
                logger.Error(ex);
                throw ex;
            }
        }
        #endregion
    }
}
