using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Xml;

// namespaces...
namespace Schedule.Common.Util
{
    // public classes...
    /// <summary>
    /// 从XmlSerializer生成的XmlDocument中反序列化对象
    /// </summary>
    public class XmlDeserializer
    {
        // private fields...
        /// <summary>
        /// 反序列化上下文
        /// </summary>
        private XmlDeserializeContext ctx;

        // public fields...
        /// <summary>
        /// IList类型
        /// </summary>
        public static readonly Type IListType = typeof(System.Collections.IList);
        /// <summary>
        /// IDictionary类型
        /// </summary>
        public static readonly Type IDictionaryType = typeof(System.Collections.IDictionary);

        // private constructors...
        private XmlDeserializer(XmlDeserializeContext ctx)
        {
            this.ctx = ctx;
        }

        // private methods...
        /// <summary>
        /// 反序列化自定义类型数据
        /// </summary>
        /// <param name="type"></param>
        /// <param name="element"></param>
        /// <returns></returns>
        private object DeserializeCustom(Type type, XmlElement element)
        {
            var obj = Activator.CreateInstance(type, true);
            if (!type.IsValueType)
            {
                this.ctx.Objects[element.GetAttribute("id")] = obj;
            }
            foreach (XmlNode node in element.ChildNodes)
            {
                if (node.NodeType == XmlNodeType.Element)
                {
                    var field = this.ctx.GetField(type, node.Name);
                    if (field == null)
                    {
                        continue;
                    }
                    var eFld = node as XmlElement;
                    if (eFld.HasAttribute("null"))
                    {
                        continue;
                    }
                    else
                    {
                        if (eFld.HasAttribute("ref"))
                        {
                            var refid = eFld.GetAttribute("ref");
                            field.SetValue(obj, this.DeserializeObject(this.ctx.Elements[refid]));
                        }
                        else
                        {
                            if (this.IsPrimritive(field.FieldType))
                            {
                                field.SetValue(obj, this.DeserializePrimritive(field.FieldType, eFld.FirstChild == null ? string.Empty : eFld.FirstChild.Value));
                            }
                            else
                            {
                                if (IListType.IsAssignableFrom(field.FieldType))
                                {
                                    field.SetValue(obj, this.DeserializeIList(field.FieldType, eFld));
                                }
                                else
                                {
                                    if (IDictionaryType.IsAssignableFrom(field.FieldType))
                                    {
                                        field.SetValue(obj, this.DeserializeIDictionary(field.FieldType, eFld));
                                    }
                                }
                            }
                        }
                    }
                }
            }

            return obj;
        }
        /// <summary>
        /// 反序列化IDictionary类型数据
        /// </summary>
        /// <param name="type"></param>
        /// <param name="element"></param>
        /// <returns></returns>
        private object DeserializeIDictionary(Type type, XmlElement element)
        {
            var dct = Activator.CreateInstance(type, true) as IDictionary;
            foreach (XmlNode node in element.ChildNodes)
            {
                if (node.NodeType == XmlNodeType.Element && node.Name == "item")
                {
                    XmlElement eKey = null, eValue = null;
                    foreach (XmlNode node1 in node.ChildNodes)
                    {
                        if (node1.NodeType == XmlNodeType.Element)
                        {
                            switch (node1.Name)
                            {
                                case "key":
                                    eKey = node1 as XmlElement;
                                    break;

                                case "value":
                                    eValue = node1 as XmlElement;
                                    break;
                            }
                        }
                    }

                    if (eKey != null && eValue != null)
                    {
                        dct[this.DeserializeItem(eKey)] = this.DeserializeItem(eValue);
                    }
                }
            }
            return dct;
        }
        /// <summary>
        /// 反序列化IList类型数据
        /// </summary>
        /// <param name="type"></param>
        /// <param name="element"></param>
        /// <returns></returns>
        private object DeserializeIList(Type type, XmlElement element)
        {
            var list = Activator.CreateInstance(type, true) as IList;
            foreach (XmlNode node in element.ChildNodes)
            {
                if (node.NodeType == XmlNodeType.Element && node.Name == "item")
                {
                    list.Add(this.DeserializeItem(node as XmlElement));
                }
            }
            return list;
        }
        /// <summary>
        /// 反序列化子元素
        /// </summary>
        /// <param name="element"></param>
        /// <returns></returns>
        private object DeserializeItem(XmlElement element)
        {
            if (element.HasAttribute("null"))
            {
                return null;
            }
            else
            {
                if (element.HasAttribute("ref"))
                {
                    var refid = element.GetAttribute("ref");
                    if (this.ctx.Objects.ContainsKey(refid))
                    {
                        return this.ctx.Objects[refid];
                    }
                    else
                    {
                        return this.DeserializeObject(this.ctx.Elements[refid]);
                    }
                }
                else
                {
                    if (element.HasAttribute("type"))
                    {
                        var type = this.ctx.Types[element.GetAttribute("type")];
                        if (this.IsPrimritive(type))
                        {
                            if (element.FirstChild == null)
                            {
                                return null;
                            }
                            else
                            {
                                return this.DeserializePrimritive(type, element.FirstChild.Value);
                            }
                        }

                        else
                        {
                            if (IListType.IsAssignableFrom(type))
                            {
                                return this.DeserializeIList(type, element);
                            }
                            else
                            {
                                if (IDictionaryType.IsAssignableFrom(type))
                                {
                                    return this.DeserializeIDictionary(type, element);
                                }
                                else
                                {
                                    return this.DeserializeCustom(type, element);
                                }
                            }
                        }
                    }
                }
            }
            return null;
        }
        /// <summary>
        /// 从完整的XmlElement中反序列化一个对象
        /// </summary>
        /// <param name="element"></param>
        private object DeserializeObject(XmlElement element)
        {
            if (element.HasAttribute("null"))
            {
                return null;
            }
            if (element.HasAttribute("id"))
            {
                var id = element.GetAttribute("id");
                if (this.ctx.Objects.ContainsKey(id))
                {
                    return this.ctx.Objects[id];
                }
            }

            if (element.HasAttribute("ref"))
            {
                var id = element.GetAttribute("ref");
                if (this.ctx.Objects.ContainsKey(id))
                {
                    return this.ctx.Objects[id];
                }
                else
                {
                    return this.DeserializeObject(this.ctx.Elements[id]);
                }
            }

            var type = this.ctx.Types[element.GetAttribute("type")];
            if (this.IsPrimritive(type))
            {
                return this.DeserializePrimritive(type, element.FirstChild.Value);
            }
            if (IListType.IsAssignableFrom(type))
            {
                return this.DeserializeIList(type, element);
            }
            else
            {
                if (IDictionaryType.IsAssignableFrom(type))
                {
                    return this.DeserializeIDictionary(type, element);
                }
                else
                {
                    return this.DeserializeCustom(type, element);
                }
            }
        }
        /// <summary>
        /// 从字符串中反序列化基本类型数据
        /// </summary>
        private object DeserializePrimritive(Type type, string text)
        {
            object value;
            if (ObjectUtil.ParsePrimritive(type, text, out value))
            {
                return value;
            }
            return null;
        }
        /// <summary>
        /// 判断是否基本类型
        /// </summary>
        /// <param name="t"></param>
        /// <returns></returns>
        private bool IsPrimritive(Type t)
        {
            if (t.IsPrimitive || t.IsEnum || t == typeof(string))
            {
                return true;
            }
            return false;
        }

        // public methods...
        /// <summary>
        /// 指定xml文件路径并创建新的XmlDeserializer对象
        /// </summary>
        /// <param name="fileName"></param>
        /// <returns></returns>
        public static XmlDeserializer Create(string fileName)
        {
            var ctx = XmlDeserializeContext.Load(fileName);
            return new XmlDeserializer(ctx);
        }
        /// <summary>
        /// 指定xml流并创建新的XmlDeserializer对象
        /// </summary>
        /// <param name="stream"></param>
        /// <returns></returns>
        public static XmlDeserializer Create(Stream stream)
        {
            var ctx = XmlDeserializeContext.Load(stream);
            return new XmlDeserializer(ctx);
        }
        /// <summary>
        /// 指定XmlReader并创建新的XmlDeserializer对象
        /// </summary>
        /// <param name="reader"></param>
        /// <returns></returns>
        public static XmlDeserializer Create(XmlReader reader)
        {
            var ctx = XmlDeserializeContext.Load(reader);
            return new XmlDeserializer(ctx);
        }
        /// <summary>
        /// 从Xml文档中反序列化一系列对象
        /// </summary>
        public List<object> Deserialize()
        {
            var list = new List<object>();
            foreach (XmlNode node in this.ctx.Document.DocumentElement.ChildNodes)
            {
                if (node.NodeType == XmlNodeType.Element && node.Name == "obj")
                {
                    list.Add(this.DeserializeObject(node as XmlElement));
                }
            }
            return list;
        }
    }
}
