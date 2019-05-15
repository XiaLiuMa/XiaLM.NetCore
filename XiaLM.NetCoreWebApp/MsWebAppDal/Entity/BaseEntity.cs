﻿using System;
using System.Collections.Generic;
using System.Text;

namespace MsWebAppDal.Entity
{
    public abstract class BaseEntity
    {
        /// <summary>
        /// 返回实体类主键值。可以为联合主键值。如果是联合主键值，则按实体类属性的定义顺序给出
        /// </summary>
        public abstract string[] GetKeyValues();

        /// <summary>
        /// 返回数据表自增字段
        /// </summary>
        /// <returns></returns>
        public abstract string[] GetDataBaseAuto();

        /// <summary>
        /// 返回表名
        /// </summary>
        public abstract string GetTableName();
    }
}
