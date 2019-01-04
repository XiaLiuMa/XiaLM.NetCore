using System;
using XiaLM.NetCoreT02.Db.Entities;

namespace XiaLM.NetCoreT02.Db.IRepositories
{
    public interface IDepartmentRepository : IBaseRepository<Department, Guid>
    {
    }
}
