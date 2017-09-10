﻿namespace Model.WeChat
{
	public class TemplateTitleList : Error
	{
		public TemplateTitle[] list { get; set; }
		public int total_count { get; set; }
	}

	public class TemplateTitle
	{
		public string id { get; set; }
		public string title { get; set; }
	}
}
