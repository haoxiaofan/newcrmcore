﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;
using NewLibCore.Data.Mapper.MapperExtension;
namespace NewCrmCore.Domain.Entitys.System
{

    [Serializable, Description("消息")]
    public partial class Notify : DomainModelBase
    {
        [PropertyRequired, InputRange(4, 10)]
        public String Title { get; private set; }

        [PropertyRequired, InputRange(1, 20)]
        public String Content { get; private set; }

        [PropertyDefaultValue(typeof(Boolean), false)]
        public Boolean IsNotify { get; private set; }

        [PropertyDefaultValue(typeof(Boolean), false)]
        public Boolean IsRead { get; private set; }

        [PropertyRequired]
        public Int32 AccountId { get; private set; }

        [PropertyRequired]
        public Int32 ToAccountId { get; private set; }

        public Notify(String title, String content, Int32 accountId, Int32 toAccountId)
        {
            Title = title;
            Content = content;
            AccountId = accountId;
            ToAccountId = toAccountId;

            IsNotify = true;
            IsRead = false;
        }

        public Notify() { }
    }

    public partial class Notify
    {
        public Notify Read()
        {
            IsRead = true;
            OnPropertyChanged(new PropertyArgs(nameof(IsRead), IsRead));
            return this;
        }
    }
}
