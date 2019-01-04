using System;

namespace XiaLM.NetCoreT02.Service.Dtos
{
    public class RoleMenuDto
    {
        public Guid RoleId { get; set; }
        public RoleDto Role { get; set; }

        public Guid MenuId { get; set; }
        public MenuDto Menu { get; set; }
    }
}
