using System.IO;
using System.Runtime.Serialization.Json;

// namespaces...
namespace Schedule.Common.Util
{
    // public classes...
    public class JsonToModel
    {
        // public methods...
        /// <summary>
        /// JSON数据反序列化到实体类
        /// </summary>
        /// <param name="str">JSON数据</param>
        /// <param name="model">实体类对象</param>
        /// <returns>Obj</returns>
        public static object ToModel(string str, object model)
        {
            var stream2 = new MemoryStream();
            var ser2 = new DataContractJsonSerializer(model.GetType());
            var wr = new StreamWriter(stream2);
            var json = string.Empty;
            json = str.Replace('\'', '\"');
            wr.Write(json);
            wr.Flush();
            stream2.Position = 0;
            var obj = ser2.ReadObject(stream2);
            return obj;
        }
    }
}
