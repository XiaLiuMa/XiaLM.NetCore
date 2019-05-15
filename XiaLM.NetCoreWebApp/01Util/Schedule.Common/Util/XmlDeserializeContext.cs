using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Xml;

// namespaces...
namespace Schedule.Common.Util
{
    // internal classes...
    /// <summary>
    /// Xml反序列化上下文
    /// </summary>
    internal class XmlDeserializeContext
    {
        // private fields...
        /// <summary>
        /// 对象索引表
        /// </summary>
        private Dictionary<string, object> dctObject = new Dictionary<string, object>();
        /// <summary>
        /// 对象的XmlElement映射表
        /// </summary>
        private Dictionary<string, XmlElement> dctElements = new Dictionary<string, XmlElement>();
        /// <summary>
        /// 类型表
        /// </summary>
        private Dictionary<string, Type> dctType = new Dictionary<string, Type>();
        /// <summary>
        /// 所有相关类型的字段表
        /// </summary>
        private Dictionary<Type, Dictionary<string, FieldInfo>> dctFields =
            new Dictionary<Type, Dictionary<string, FieldInfo>>();
        /// <summary>
        /// xml文档对象
        /// </summary>
        private XmlDocument doc;

        // private constructors...
        /// <summary>
        /// 从指定文件构建XmlDeserializeContext对象
        /// </summary>
        private XmlDeserializeContext()
        {
        }

        // public properties...
        /// <summary>
        /// 获取Xml文档对象
        /// </summary>
        public XmlDocument Document
        {
            get
            {
                return this.doc;
            }
        }
        /// <summary>
        /// 对象的XmlElement映射表
        /// </summary>
        public Dictionary<string, XmlElement> Elements
        {
            get
            {
                return this.dctElements;
            }
        }
        /// <summary>
        /// 对象映射表
        /// </summary>
        public Dictionary<string, object> Objects
        {
            get
            {
                return this.dctObject;
            }
        }
        /// <summary>
        /// 类型映射表
        /// </summary>
        public Dictionary<string, Type> Types
        {
            get
            {
                return this.dctType;
            }
        }

        // private methods...
        /// <summary>
        /// 获取所有的类型定义，初始化类型表
        /// </summary>
        private void Parse()
        {
            foreach (XmlNode node in this.doc.DocumentElement.ChildNodes)
            {
                if (node.NodeType != XmlNodeType.Element)
                {
                    continue;
                }
                var element = node as XmlElement;
                switch (element.Name)
                {
                    case "types":
                        foreach (XmlNode item in node.ChildNodes)
                        {
                            if (item.NodeType == XmlNodeType.Element && item.Name == "type")
                            {
                                var name = item.FirstChild.Value;
                                var type = ObjectUtil.FindType(name);
                                this.dctType[item.Attributes["id"].Value] = type;

                                var dct = new Dictionary<string, FieldInfo>();
                                this.dctFields[type] = dct;
                                foreach (FieldInfo fld in ObjectUtil.GetTypeFields(type))
                                {
                                    dct[fld.Name] = fld;
                                }
                            }
                        }
                        break;

                    case "obj":
                        this.dctElements[element.GetAttribute("id")] = element;
                        break;
                }
            }
        }

        // public methods...
        /// <summary>
        /// 获取指定类型指定名称的FieldInfo。如果不存在，返回null
        /// </summary>
        /// <param name="type"></param>
        /// <param name="name"></param>
        /// <returns></returns>
        public FieldInfo GetField(Type type, string name)
        {
            if (!this.dctFields.ContainsKey(type))
            {
                return null;
            }
            var dct = this.dctFields[type];
            FieldInfo fld;
            dct.TryGetValue(name, out fld);
            return fld;
        }
        /// <summary>
        /// 从文件中加载Xml文档
        /// </summary>
        /// <param name="fileName"></param>
        /// <returns></returns>
        public static XmlDeserializeContext Load(string fileName)
        {
            var doc = new XmlDocument();
            using (var fs = File.OpenRead(fileName))
            {
                doc.Load(fs);
                return Load(doc);
            }
        }
        /// <summary>
        /// 从流中加载Xml文档
        /// </summary>
        /// <param name="stream"></param>
        /// <returns></returns>
        public static XmlDeserializeContext Load(Stream stream)
        {
            var doc = new XmlDocument();
            using (var reader = new XmlTextReader(stream))
            {
                doc.Load(reader);
                return Load(doc);
            }
        }
        /// <summary>
        /// 从流中加载Xml文档
        /// </summary>
        /// <param name="reader"></param>
        /// <returns></returns>
        public static XmlDeserializeContext Load(XmlReader reader)
        {
            var doc = new XmlDocument();
            doc.Load(reader);
            return Load(doc);
        }
        /// <summary>
        /// 解析指定XmlDocument对象，创建XmlDeserializeContext实例
        /// </summary>
        /// <param name="doc"></param>
        /// <returns></returns>
        public static XmlDeserializeContext Load(XmlDocument doc)
        {
            var obj = new XmlDeserializeContext();
            obj.doc = doc;
            obj.Parse();
            return obj;
        }
        /// <summary>
        /// 从字符串中加载Xml文档
        /// </summary>
        /// <param name="xml"></param>
        /// <returns></returns>
        public static XmlDeserializeContext LoadXml(string xml)
        {
            var doc = new XmlDocument();
            doc.LoadXml(xml);
            return Load(doc);
        }
    }
}
