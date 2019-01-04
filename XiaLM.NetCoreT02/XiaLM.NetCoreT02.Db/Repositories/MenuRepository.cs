using XiaLM.NetCoreT02.Db.Entities;
using XiaLM.NetCoreT02.Db.IRepositories;

namespace XiaLM.NetCoreT02.Db.Repositories
{
    public class MenuRepository : BaseRepository<Menu>, IMenuRepository
    {
        public MenuRepository(BaseDBContext dbcontext) : base(dbcontext)
        {

        }
    }
}
