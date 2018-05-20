﻿using System;

namespace NewCrmCore.Infrastructure.CommonTools.CustomExtension
{
	public static class Extension
	{
		public static Int32 ToInt32(this Boolean boo)
		{
			return boo ? 1 : 0;
		}

		public static Int32 ToInt32(this Enum e)
		{
			return (Int32)Enum.Parse(e.GetType(), e.ToString());
		}
	}
}
