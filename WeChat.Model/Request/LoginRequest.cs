﻿namespace Model.Request
{
	/// <summary>
	/// 登录请求数据类
	/// </summary>
	public class LoginRequest : RequestBase
    {
        public string Code { get; set; }
    }
}