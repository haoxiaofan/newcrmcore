﻿using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using NewCrmCore.Domain.Entitys.System;
using NewCrmCore.Infrastructure.CommonTools;

namespace NewCrmCore.Domain.Services.Interface
{
	public interface ILoggerContext
	{
		/// <summary>
		/// 添加日志
		/// </summary>
		Task AddLoggerAsync(Log log);

		/// <summary>
		/// 获取日志列表
		/// </summary>
		/// <returns></returns>
		Task<PagingModel<Log>> GetLogsAsync(Int32 accountId, Int32 logLevel, Int32 pageIndex, Int32 pageSize);
	}
}