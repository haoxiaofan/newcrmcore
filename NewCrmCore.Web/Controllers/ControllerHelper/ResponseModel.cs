﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace NewCrmCore.Web.Controllers.ControllerHelper
{
	public class ResponseModel<T>
	{
		public ResponseModel()
		{
			IsSuccess = false;
			Message = "";
			Model = default(T);
		}

		public Boolean IsSuccess { get; set; }

		public String Message { get; set; }

		public T Model { get; set; }
	}

	public class ResponseModels<T>: ResponseModel<T>
	{
		public Int32 TotalCount { get; set; }
	}

	public class ResponseModel: ResponseModel<String>
	{

	}
}