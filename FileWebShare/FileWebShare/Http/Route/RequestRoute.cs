﻿using System;
namespace FileWebShare
{
	public class RequestRoute
	{
		public string ControllerName { get; set; }
		public string ControllerMethod { get; set; }
		public string[] Parameters { get; set; }

		public RequestRoute()
		{ 
		}
	}
}
