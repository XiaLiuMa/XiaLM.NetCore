using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Xml;

// namespaces...
namespace Schedule.Common.Core
{
    // public classes...
    /// <summary>
    /// XML 文档管理类
    /// </summary>
    public class XmlFileManage
    {
        // private fields...
        private static XmlFileManage _instance;
        /// <summary>
        /// 所有已打开的XML文档。使用文件名来做为关键词。应该不允许不同内容的文档使用相同的文件名。
        /// </summary>
        private Dictionary<string, XmlDocument> DocDic = new Dictionary<string, XmlDocument>();
        /// <summary>
        /// 文件对应的绝对路径
        /// </summary>
        private Dictionary<string, string> PathDic = new Dictionary<string, string>();
        private object saveLck = new object();
        private static object lckObj = new object();

        // private constructors...
        private XmlFileManage()
        {
        }

        // public methods...
        public static XmlFileManage GetInstance()
        {
            lock (lckObj)
            {
                if (_instance == null)
                {
                    _instance = new XmlFileManage();
                }
            }
            return _instance;
        }
        /// <summary>
        /// 加载文档。在程序运行的周期内，同一个文档只会被加载一次。同时在程序的生命周期内，不允许加载同名的文件。
        /// </summary>
        /// <param name="path">文档路径(绝对路径）</param>
        public XmlDocument LoadFile(string path)
        {
            lock (saveLck)
            {
                path = Path.GetFullPath(path);

                var fileName = Path.GetFileName(path);

                if (Path.GetExtension(path).Equals(".xml", StringComparison.CurrentCultureIgnoreCase) == false)
                {
                    throw new FileLoadException("不是XML文档");
                }

                if (PathDic.ContainsKey(fileName))
                {
                    if (PathDic[fileName].Equals(path) == false)
                    {
                        throw new FileLoadException("不允许加载同名的文件");
                    }
                    else
                    {
                        return DocDic[fileName];
                    }
                }
                else
                {
                    var doc = new XmlDocument();
                    doc.Load(path);
                    PathDic.Add(fileName, path);
                    DocDic.Add(fileName, doc);

                    return doc;
                }
            }
        }
        public void Remove(string path)
        {
            try
            {
                path = Path.GetFullPath(path);

                var fileName = Path.GetFileName(path);
                if (DocDic.ContainsKey(fileName))
                {
                    DocDic.Remove(fileName);
                    PathDic.Remove(fileName);
                }
            }
            catch
            {
            }
        }
        /// <summary>
        /// 保存XML文档
        /// </summary>
        /// <param name="doc"></param>
        public bool Save(XmlDocument doc)
        {
            lock (saveLck)
            {
                var value = DocDic.First(r =>
                    {
                        return r.Value == doc;
                    });
                if (value.Key != null)
                {
                    var path = PathDic[value.Key];
                    doc.Save(path);
                    return true;
                }
                return false;
            }
        }
        /// <summary>
        /// 选择匹配xpath表达式的节点
        /// </summary>
        /// <param name="doc"></param>
        /// <param name="xpath"></param>
        /// <returns></returns>
        public XmlNodeList SelectNodes(XmlDocument doc, string xpath)
        {
            var root = doc.DocumentElement;
            return root.SelectNodes(xpath);
        }
        /// <summary>
        /// 获取匹配XPATH表达式的第一个XMLNODE节点
        /// </summary>
        /// <param name="doc">文档</param>
        /// <param name="xPath">XPATH表达式（可参考 http://www.w3school.com.cn/ 的XPath教程。适合JS/JAVA/C#等）</param>
        /// <returns></returns>
        public XmlNode SelectSingle(XmlDocument doc, string xPath)
        {
            var root = doc.DocumentElement;
            return root.SelectSingleNode(xPath);
        }
    }
}
