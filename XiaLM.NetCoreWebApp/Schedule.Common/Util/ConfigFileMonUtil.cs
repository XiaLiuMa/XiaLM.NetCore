using System;
using System.Collections.Generic;
using Microsoft.Extensions.FileProviders;
using Microsoft.Extensions.Primitives;
using Schedule.model.common;

namespace Schedule.Common.Util
{

    /* •——————————————————————————————————————————————————————————————————————————•
    * 使用方式
      | //public class test:IDisposable                                          |
      | //{                                                                      |
      | //    IConfigFileMon filemon;                                            |
      | //    public test()                                                      |
      | //    {   //启动监控                                                               |
      | //        filemon = ConfigFileMonUtil.ConfigFileMon<List<object>>("xxx") |
      | //            .NotifyOnChanged(p =>                                      |
      | //        {                                                              |
      |               //dosomething                                                           |
      | //        });                                                            |
      |                                                                          |
      | //    }                                                                  |
      |                                                                          |
      | //    public void Dispose()                                              |
      | //    {                                                                  |
      | //      if(filemon!=null)                                                |
      | //      {   //显示释放监控                                                             |
      | //          filemon.Dispose();                                           |
      | //      }                                                                |
      | //    }                                                                  |
      | //}                                                                      |
      •——————————————————————————————————————————————————————————————————————————• */

    public static class ConfigFileMonUtil
    {

        /// <summary>
        /// 监控xml序列化文件更改
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="filePath">filePath：必须是文件物理路径</param>
        /// <returns></returns>
        public static IConfigFileMon<T> ConfigFileMon<T>(string filePath)
        {
            return new ConfigFileMonIML<T>(filePath);
        }
    }

    public interface IConfigFileMon<T>
    {
        IConfigFileMon<T> NotifyOnChanged(Action<T> action);
    }

    internal class ConfigFileMonIML<T> : IConfigFileMon<T>
    {
        //HostFileChangeMonitor Monitor;
        IFileProvider fileProvider; //UNDONE:（不确定是否可行）

        Action<T> _action;

        T Item { get; set; }

        string _filePath;

        internal ConfigFileMonIML(string filePath)
        {
            _filePath = filePath;
             //Monitor = new HostFileChangeMonitor(new List<string>() { filePath });
             //Monitor.NotifyOnChanged(NotifyOnChanged);

            fileProvider = new PhysicalFileProvider(AppDomain.CurrentDomain.BaseDirectory);
            ChangeToken.OnChange(() => fileProvider.Watch(filePath), () => NotifyOnChanged(fileProvider));

            getItem();

        }

        private void getItem()
        {
           Item= SerializationHelper.Load<T>(_filePath);
        }

        private void NotifyOnChanged(object obj)
        {
            getItem();
            if(this._action!=null)
            {
                _action(Item);
            }

            //Monitor = new HostFileChangeMonitor(new List<string>() { _filePath });
            //Monitor.NotifyOnChanged(NotifyOnChanged);

            fileProvider = new PhysicalFileProvider(AppDomain.CurrentDomain.BaseDirectory);
            ChangeToken.OnChange(() => fileProvider.Watch(_filePath), () => NotifyOnChanged(fileProvider));
        }

        //public void Dispose()
        //{
        //    if(Monitor!=null)
        //    Monitor.Dispose();
        //}

        public IConfigFileMon<T> NotifyOnChanged(Action<T> action)
        {
            _action = action;
            _action(Item);
            return this;
        }
    }
}
