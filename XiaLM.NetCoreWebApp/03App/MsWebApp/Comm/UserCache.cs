using Microsoft.Extensions.Caching.Memory;
using MsWebAppDal.Entity.UserManage;
using System;

namespace MsWebApp.Comm
{
    /// <summary>
    /// 用户缓存
    /// </summary>
    public class UserCache
    {
        private static MemoryCache cache = new MemoryCache(new MemoryCacheOptions());

        /// <summary>
        /// 用户是否登陆
        /// </summary>
        public static bool IsLogin
        {
            get
            {
                return (User)cache.Get("User") == null ? false : true;
            }
        }

        /// <summary>
        /// 当前用户
        /// </summary>
        public static User User
        {
            get
            {
                return (User)cache.Get("User");
            }
        }

        /// <summary>
        /// 设置缓存
        /// </summary>
        /// <param name="_user"></param>
        public static void SetCache(User _user)
        {
            cache.Set("User", _user, new MemoryCacheEntryOptions
            {
                SlidingExpiration = TimeSpan.FromMinutes(10)    //10分钟过期
            });
        }

        /// <summary>
        /// 移除缓存
        /// </summary>
        public static void RemoveCache()
        {
            cache.Remove("User");
        }
    }
}
