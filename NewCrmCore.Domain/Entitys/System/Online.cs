﻿using System;
using System.ComponentModel;
using NewLibCore.Data.Mapper.MapperExtension;

namespace NewCrmCore.Domain.Entitys.System
{
	[Description("在线人数"), Serializable]
    public partial class Online : DomainModelBase
    { 
		#region public property

		/// <summary>
		/// Ip地址
		/// </summary>
		[PropertyRequired]
		public String IpAddress
        {
            get { return _ipAddress; }
            private set
            {
                _ipAddress = value;
            }
        }

		/// <summary>
		/// 用户Id
		/// </summary>
		[PropertyRequired]
		public Int32 AccountId
        {
            get { return _accountId; }
            private set
            {
                _accountId = value;
            }
        }
        #endregion

        #region ctor

        /// <summary>
        /// 实例化一个在线状态的对象
        /// </summary>
        public Online(String ipAddress, Int32 accountId)
        {
            IpAddress = ipAddress;
            AccountId = accountId;
        }


        /// <summary>
        /// 实例化一个在线状态的对象
        /// </summary>
        public Online()
        {
        }

        #endregion
    }
}
