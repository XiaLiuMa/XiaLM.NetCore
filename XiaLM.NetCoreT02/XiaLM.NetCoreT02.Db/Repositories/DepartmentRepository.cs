using XiaLM.NetCoreT02.Db.Entities;
using XiaLM.NetCoreT02.Db.IRepositories;

namespace XiaLM.NetCoreT02.Db.Repositories
{
    public class DepartmentRepository : BaseRepository<Department>, IDepartmentRepository
    {
        public DepartmentRepository(BaseDBContext dbcontext) : base(dbcontext)
        {

        }
    }
}
