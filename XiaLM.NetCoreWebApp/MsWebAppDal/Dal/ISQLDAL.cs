using System;
using System.Collections.Generic;
using System.Text;

namespace MsWebAppDal.Dal
{
    public interface ISQLDAL<T>
    {
        /// <summary>
        /// 添加数据
        /// </summary>
        /// <param name="t">实体类</param>
        /// <returns>添加是否成功(true:成功；false:失败；)</returns>
        bool Add(T t);

        /// <summary>
        /// 更新数据
        /// </summary>
        /// <param name="t">实体类</param>
        /// <returns>更新是否成功(true:成功；false:失败；)</returns>
        bool Update(T t);

        /// <summary>
        /// 根据实体类删除数据
        /// </summary>
        /// <param name="t">实体类</param>
        /// <returns>删除是否成功(true:成功；false:失败；)</returns>
        bool Delete(T t);

        /// <summary>
        /// 根据主键删除数据
        /// </summary>
        /// <param name="key">主键（需与实体类中主键顺序一致）</param>
        /// <returns>删除是否成功(true:成功；false:失败；)</returns>
        bool Delete(params string[] key);

        /// <summary>
        /// 删除所有数据
        /// </summary>
        /// <returns>删除是否成功(true:成功；false:失败；)</returns>
        bool DeleteAll();

        /// <summary>
        /// 批量执行删除语句
        /// </summary>
        /// <param name="LstCondation">删除条件集合</param>
        /// <returns>删除是否成功(true:成功；false:失败；)</returns>
        bool Delete(List<string> LstCondation);

        /// <summary>
        /// 根据主键查询
        /// </summary>
        /// <param name="key">主键（需与实体类中主键顺序一致）</param>
        /// <returns>实体类</returns>
        T Query(params string[] key);

        /// <summary>
        /// 判断实体类是否存在
        /// </summary>
        /// <param name="t">实体类</param>
        /// <returns>存在：true;不存在:false</returns>
        bool IsExist(T t);

        /// <summary>
        /// 根据主键判断是否存在
        /// </summary>
        /// <param name="key">主键（需与实体类中主键顺序一致）</param>
        /// <returns>存在：true;不存在:false</returns>
        bool IsExist(params string[] key);

        /// <summary>
        /// 查询所有数据
        /// </summary>
        /// <returns>实体类集合</returns>
        List<T> SelectAll();

        /// <summary>
        /// 根据条件查询符合条件的实体类集合
        /// </summary>
        /// <param name="condation">条件</param>
        /// <returns>实体类集合</returns>
        List<T> Select(string condation);

        /// <summary>
        /// 根据条件、页码、页数查询符合条件的记录数、分页集合
        /// </summary>
        /// <param name="condation">查询条件</param>
        /// <param name="page">页码</param>
        /// <param name="pagesize">页数</param>
        /// <param name="lst">返回的实体类集合</param>
        /// <param name="count">总数</param>
        /// <returns>查询是否成功(成功:true;失败:false)</returns>
        bool SelectPage(string condation, int page, int pagesize, out List<T> lst, out int count);


        /// <summary>
        /// 根据条件、页码、页数查询符合条件的记录数、分页集合
        /// </summary>
        /// <param name="condation">查询条件</param>
        /// <param name="page">页码</param>
        /// <param name="pagesize">页数</param>
        /// <param name="lst">返回的实体类集合</param>
        /// <param name="count">总数</param>
        /// <returns>查询是否成功(成功:true;失败:false)</returns>
        bool SelectPageByTime(string condation, int page, int pagesize, out List<T> lst, out int count);

        /// <summary>
        /// 根据条件、排序字段、页码、页数查询符合条件的记录数、分页集合
        /// </summary>
        /// <param name="condation">查询条件</param>
        /// <param name="page">页码</param>
        /// <param name="pagesize">页数</param>
        /// <param name="lst">返回的实体类集合</param>
        /// <param name="count">总数</param>
        /// <returns>查询是否成功(成功:true;失败:false)</returns>
        bool SelectPageEx(string condation, string order, int page, int pagesize, out List<T> lst, out int count);

        List<T> ExQuery(string sql);
    }
}
