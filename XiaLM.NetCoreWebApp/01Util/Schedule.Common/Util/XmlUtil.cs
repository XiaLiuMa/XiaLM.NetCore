using System;
using System.Collections;
using System.Collections.Generic;
using System.Xml;

// namespaces...
namespace Schedule.Common.Util
{
    // public classes...
    /// <summary>
    /// 提供对XML文档的常用操作
    /// </summary>
    public class XmlUtil
    {
        // private fields...
        private XmlDocument xmldoc;
        private XmlElement xmlelem;
        private XmlNode xmlnode;

        // private methods...
        /// <summary>  
        /// 设置节点属性  
        /// </summary>
        /// <param name="xe">节点所处的Element</param>
        /// <param name="htAttribute">节点属性，Key代表属性名称，Value代表属性值</param>
        private void SetAttributes(XmlElement xe, Hashtable htAttribute)
        {
            foreach (DictionaryEntry de in htAttribute)
            {
                xe.SetAttribute(de.Key.ToString(), de.Value.ToString());
            }
        }
        /// <summary>  
        /// 增加子节点到根节点下  
        /// </summary>
        /// <param name="rootNode">上级节点名称</param>
        /// <param name="XmlDoc">Xml文档</param>
        /// <param name="rootXe">父根节点所属的Element</param>
        /// <param name="SubNodes">子节点属性，Key为Name值，Value为InnerText值</param>
        private void SetNodes(string rootNode, XmlDocument XmlDoc, XmlElement rootXe, Dictionary<string, string> dicNode)
        {
            foreach (KeyValuePair<string, string> pair in dicNode)
            {
                xmlnode = XmlDoc.SelectSingleNode(rootNode);
                var subNode = XmlDoc.CreateElement(pair.Key.ToString());
                subNode.InnerText = pair.Value.ToString();
                rootXe.AppendChild(subNode);
            }
        }
        /// <summary>  
        /// 更新节点属性和子节点InnerText值  
        /// </summary>
        /// <param name="root">根节点名字</param>
        /// <param name="htAtt">需要更改的属性名称和值</param>
        /// <param name="htSubNode">需要更改InnerText的子节点名字和值</param>
        private void UpdateNodes(XmlNodeList root, Hashtable htAtt, Dictionary<string, string> dicNode)
        {
            foreach (XmlNode xn in root)
            {
                xmlelem = (XmlElement)xn;
                if (xmlelem.HasAttributes)
                {
                    foreach (DictionaryEntry de in htAtt)
                    {
                        if (xmlelem.HasAttribute(de.Key.ToString()))
                        {
                            xmlelem.SetAttribute(de.Key.ToString(), de.Value.ToString());
                        }
                    }
                }

                foreach (KeyValuePair<string, string> pair in dicNode)
                {
                    if (xmlelem.Name == pair.Key.ToString())
                    {
                        xmlelem.InnerText = pair.Value.ToString();
                    }
                }
            }
        }

        // public methods...
        /// <summary>  
        /// 删除指定节点下的子节点  
        /// </summary>
        /// <param name="XmlFile">Xml文件路径</param>
        /// <param name="fatherNode">制定节点</param>
        /// <returns>返回真为更新成功，否则失败</returns>
        public bool DeleteNodes(string XmlFile, string fatherNode)
        {
            try
            {
                xmldoc = new XmlDocument();
                xmldoc.Load(XmlFile);
                xmlnode = xmldoc.SelectSingleNode(fatherNode);
                xmlnode.ParentNode.RemoveChild(xmlnode);
                xmldoc.Save(XmlFile);
                return true;
            }
            catch (XmlException xe)
            {
                throw new XmlException(xe.Message);
            }
        }
        /// <summary>  
        /// 插入一个节点和它的若干子节点  
        /// </summary>
        /// <param name="XmlFile">Xml文件路径</param>
        /// <param name="NewNodeName">插入的节点名称</param>
        /// <param name="HasAttributes">此节点是否具有属性，True为有，False为无</param>
        /// <param name="fatherNode">此插入节点的父节点</param>
        /// <param name="htAtt">此节点的属性，Key为属性名，Value为属性值</param>
        /// <param name="dicNode">子节点的属性，Key为Name,Value为InnerText</param>
        /// <returns>返回真为更新成功，否则失败</returns>
        public bool InsertNode(string XmlFile, string NewNodeName, bool HasAttributes, string fatherNode, Hashtable htAtt, Dictionary<string, string> dicNode)
        {
            try
            {
                xmldoc = new XmlDocument();
                xmldoc.Load(XmlFile);
                var root = xmldoc.SelectSingleNode(fatherNode);
                xmlelem = xmldoc.CreateElement(NewNodeName);

                if (htAtt != null && HasAttributes)
                {
                    SetAttributes(xmlelem, htAtt);
                    if (dicNode != null)
                    {
                        SetNodes(xmlelem.Name, xmldoc, xmlelem, dicNode);
                    }
                }
                else
                {
                    if (dicNode != null)
                    {
                        SetNodes(xmlelem.Name, xmldoc, xmlelem, dicNode);
                    }
                }

                root.AppendChild(xmlelem);
                xmldoc.Save(XmlFile);

                return true;
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }
        /// <summary>  
        /// 更新节点  
        /// </summary>
        /// <param name="XmlFile">Xml文件路径</param>
        /// <param name="fatherNode">需要更新节点的上级节点</param>
        /// <param name="htAtt">需要更新的属性表，Key代表需要更新的属性，Value代表更新后的值</param>
        /// <param name="dicNode">需要更新的子节点的属性表，Key代表需要更新的子节点名字Name,Value代表更新后的值InnerText</param>
        /// <returns>返回真为更新成功，否则失败</returns>
        public bool UpdateNode(string XmlFile, string fatherNode, Hashtable htAtt, Dictionary<string, string> dicNode)
        {
            try
            {
                xmldoc = new XmlDocument();
                xmldoc.Load(XmlFile);
                var root = xmldoc.SelectSingleNode(fatherNode).ChildNodes;
                UpdateNodes(root, htAtt, dicNode);
                xmldoc.Save(XmlFile);
                return true;
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }
    }
}
