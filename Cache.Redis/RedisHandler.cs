﻿using Configuration.Helper;
using NLog;
using StackExchange.Redis;
using System;
using Model;

namespace Cache.Redis
{
    public class RedisHandler
    {
        #region Connection
        private static ConnectionMultiplexer _connection;
        private static ConnectionMultiplexer Connection
        {
            get
            {
                if (_connection == null || !_connection.IsConnected)
                {
                    _connection = ConnectionMultiplexer.Connect($"{ConfigurationHelper.RedisServerHost}:{ConfigurationHelper.RedisServerPort}{(string.IsNullOrEmpty(ConfigurationHelper.RedisPassword) ? string.Empty : string.Concat(",password=", ConfigurationHelper.RedisPassword))}");
                }

                return _connection;
            }
        }
        #endregion

        #region log
        private readonly Logger logger = LogManager.GetCurrentClassLogger();
        #endregion

        /// <summary>
        /// 保存OpenId到Redis服务器
        /// </summary>
        /// <param name="oirs"></param>
        /// <param name="ticks"></param>
        public OpenIdResultModel SaveOpenId(OpenIdResultSuccess oirs, long ticks)
        {
            try
            {
                var db = Connection.GetDatabase();

                OpenIdResultModel model = new OpenIdResultModel()
                {
                    Id = Guid.NewGuid(),
                    openid = oirs.openid,
                    session_key = oirs.session_key,
                    unionid = oirs.unionid
                };

                db.ObjectSet(model.Id.ToString(), model, TimeSpan.FromDays(ConfigurationHelper.ExpireDays.Value).Ticks);
                return model;
            }
            catch (Exception ex)
            {
                logger.Error(ex);
                throw ex;
            }
        }

        /// <summary>
        /// 通过ID获取OpenId
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public OpenIdResultModel GetSavedOpenId(Guid id)
        {
            try
            {
                var db = Connection.GetDatabase();

                OpenIdResultModel savedOpenId = db.ObjectGet<OpenIdResultModel>(id.ToString());
                return savedOpenId;
            }
            catch (Exception ex)
            {
                logger.Error(ex);
                throw ex;
            }
        }
    }
}