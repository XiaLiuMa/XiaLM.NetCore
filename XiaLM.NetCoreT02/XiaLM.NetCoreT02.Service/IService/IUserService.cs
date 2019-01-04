using System;
using System.Collections.Generic;
using XiaLM.NetCoreT02.Db.Entities;
using XiaLM.NetCoreT02.Service.Dtos;

namespace XiaLM.NetCoreT02.Service.IService
{
    public interface IUserService
    {
        User CheckUser(string userName, string password);

        List<UserDto> GetUserByDepartment(Guid departmentId, int startPage, int pageSize, out int rowCount);

        UserDto InsertOrUpdate(UserDto dto);

        /// <summary>
        /// 根据Id集合批量删除
        /// </summary>
        /// <param name="ids">Id集合</param>
        void DeleteBatch(List<Guid> ids);

        /// <summary>
        /// 删除
        /// </summary>
        /// <param name="id">Id</param>
        void Delete(Guid id);

        /// <summary>
        /// 根据Id获取实体
        /// </summary>
        /// <param name="id">Id</param>
        /// <returns></returns>
        UserDto Get(Guid id);
    }
}
