/// <summary>
/// 类说明：HttpHelper类，用来实现Http访问，Post或者Get方式的，直接访问，带Cookie的，带证书的等方式，可以设置代理
/// 重要提示：请不要自行修改本类，如果因为你自己修改后将无法升级到新版本。如果确实有什么问题请到官方网站提建议，
/// 我们一定会及时修改
/// 编码日期：2011-09-20
/// 编 码 人：苏飞
/// 联系方式：361983679  
/// 官方网址：http://www.sufeinet.com/thread-3-1-1.html
/// 修改日期：2015-09-08
/// 版 本 号：1.5
/// </summary>
using System;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Net;
using System.Net.Security;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Text.RegularExpressions;

// namespaces...
namespace Schedule.Common.Core.Http
{
    // public enums...
    /// <summary>
    /// Post的数据格式默认为string
    /// </summary>
    public enum PostDataType
    {
        /// <summary>
        /// 字符串类型，这时编码Encoding可不设置
        /// </summary>
        String,
        /// <summary>
        /// Byte类型，需要设置PostdataByte参数的值编码Encoding可设置为空
        /// </summary>
        Byte,
        /// <summary>
        /// 传文件，Postdata必须设置为文件的绝对路径，必须设置Encoding的值
        /// </summary>
        FilePath
    }

    /// <summary>
    /// Cookie返回类型
    /// </summary>
    public enum ResultCookieType
    {
        /// <summary>
        /// 只返回字符串类型的Cookie
        /// </summary>
        String,
        /// <summary>
        /// CookieCollection格式的Cookie集合同时也返回String类型的cookie
        /// </summary>
        CookieCollection
    }

    /// <summary>
    /// 返回类型
    /// </summary>
    public enum ResultType
    {
        /// <summary>
        /// 表示只返回字符串 只有Html有数据
        /// </summary>
        String,
        /// <summary>
        /// 表示返回字符串和字节流 ResultByte和Html都有数据返回
        /// </summary>
        Byte
    }

    // public classes...
    /// <summary>
    /// Http连接操作帮助类
    /// </summary>
    public class HttpHelper
    {
        // private fields...
        private IPEndPoint _IPEndPoint = null;
        private Encoding encoding = Encoding.Default;
        private Encoding postencoding = Encoding.Default;
        private HttpWebRequest request = null;
        private HttpWebResponse response = null;

        // private methods...
        /// <summary>
        /// 通过设置这个属性，可以在发出连接的时候绑定客户端发出连接所使用的IP地址。 
        /// </summary>
        /// <param name="servicePoint"></param>
        /// <param name="remoteEndPoint"></param>
        /// <param name="retryCount"></param>
        /// <returns></returns>
        private IPEndPoint BindIPEndPointCallback(ServicePoint servicePoint, IPEndPoint remoteEndPoint, int retryCount)
        {
            return _IPEndPoint;
        }
        /// <summary>
        /// 回调验证证书问题
        /// </summary>
        /// <param name="sender">流对象</param>
        /// <param name="certificate">证书</param>
        /// <param name="chain">X509Chain</param>
        /// <param name="errors">SslPolicyErrors</param>
        /// <returns>bool</returns>
        private bool CheckValidationResult(object sender, X509Certificate certificate, X509Chain chain, SslPolicyErrors errors)
        {
            return true;
        }
        /// <summary>
        /// 提取网页Byte
        /// </summary>
        /// <returns></returns>
        private byte[] GetByte()
        {
            var ResponseByte = (byte[])null;
            using (var _stream = new MemoryStream())
            {
                if (response.ContentEncoding != null && response.ContentEncoding.Equals("gzip", StringComparison.InvariantCultureIgnoreCase))
                {
                    new GZipStream(response.GetResponseStream(), CompressionMode.Decompress).CopyTo(_stream, 10240);
                }
                else
                {
                    response.GetResponseStream().CopyTo(_stream, 10240);
                }

                ResponseByte = _stream.ToArray();
            }
            return ResponseByte;
        }
        /// <summary>
        /// 获取数据的并解析的方法
        /// </summary>
        /// <param name="item"></param>
        /// <param name="result"></param>
        private void GetData(HttpItem item, HttpResult result)
        {
            if (response == null)
            {
                return;
            }

            result.StatusCode = response.StatusCode;

            result.StatusDescription = response.StatusDescription;

            result.Header = response.Headers;

            result.ResponseUri = response.ResponseUri.ToString();

            if (response.Cookies != null)
            {
                result.CookieCollection = response.Cookies;
            }
            if (response.Headers["set-cookie"] != null)
            {
                result.Cookie = response.Headers["set-cookie"];
            }



            var ResponseByte = GetByte();


            if (ResponseByte != null & ResponseByte.Length > 0)
            {
                SetEncoding(item, result, ResponseByte);

                result.Html = encoding.GetString(ResponseByte);
            }
            else
            {
                result.Html = string.Empty;
            }
        }
        /// <summary>
        /// 设置证书
        /// </summary>
        /// <param name="item"></param>
        private void SetCer(HttpItem item)
        {
            if (!string.IsNullOrWhiteSpace(item.CerPath))
            {
                ServicePointManager.ServerCertificateValidationCallback = new System.Net.Security.RemoteCertificateValidationCallback(CheckValidationResult);

                request = (HttpWebRequest)WebRequest.Create(item.URL);
                SetCerList(item);

                request.ClientCertificates.Add(new X509Certificate(item.CerPath));
            }
            else
            {
                request = (HttpWebRequest)WebRequest.Create(item.URL);
                SetCerList(item);
            }
        }
        /// <summary>
        /// 设置多个证书
        /// </summary>
        /// <param name="item"></param>
        private void SetCerList(HttpItem item)
        {
            if (item.ClentCertificates != null && item.ClentCertificates.Count > 0)
            {
                foreach (X509Certificate c in item.ClentCertificates)
                {
                    request.ClientCertificates.Add(c);
                }
            }
        }
        /// <summary>
        /// 设置Cookie
        /// </summary>
        /// <param name="item">Http参数</param>
        private void SetCookie(HttpItem item)
        {
            if (!string.IsNullOrEmpty(item.Cookie))
            {
                request.Headers[HttpRequestHeader.Cookie] = item.Cookie;
            }
            if (item.ResultCookieType == ResultCookieType.CookieCollection)
            {
                request.CookieContainer = new CookieContainer();
                if (item.CookieCollection != null && item.CookieCollection.Count > 0)
                {
                    request.CookieContainer.Add(item.CookieCollection);
                }
            }
        }
        /// <summary>
        /// 设置编码
        /// </summary>
        /// <param name="item">HttpItem</param>
        /// <param name="result">HttpResult</param>
        /// <param name="ResponseByte">byte[]</param>
        private void SetEncoding(HttpItem item, HttpResult result, byte[] ResponseByte)
        {
            if (item.ResultType == ResultType.Byte)
            {
                result.ResultByte = ResponseByte;
            }
            if (encoding == null)
            {
                var meta = Regex.Match(Encoding.Default.GetString(ResponseByte), "<meta[^<]*charset=([^<]*)[\"']", RegexOptions.IgnoreCase);
                var c = string.Empty;
                if (meta != null && meta.Groups.Count > 0)
                {
                    c = meta.Groups[1].Value.ToLower().Trim();
                }
                if (c.Length > 2)
                {
                    try
                    {
                        encoding = Encoding.GetEncoding(c.Replace("\"", string.Empty).Replace("'", string.Empty).Replace(";", string.Empty).Replace("iso-8859-1", "gbk").Trim());
                    }
                    catch
                    {
                        if (string.IsNullOrEmpty(response.CharacterSet))
                        {
                            encoding = Encoding.UTF8;
                        }
                        else
                        {
                            encoding = Encoding.GetEncoding(response.CharacterSet);
                        }
                    }
                }
                else
                {
                    if (string.IsNullOrEmpty(response.CharacterSet))
                    {
                        encoding = Encoding.UTF8;
                    }
                    else
                    {
                        encoding = Encoding.GetEncoding(response.CharacterSet);
                    }
                }
            }
        }
        /// <summary>
        /// 设置Post数据
        /// </summary>
        /// <param name="item">Http参数</param>
        private void SetPostData(HttpItem item)
        {
            if (!request.Method.Trim().ToLower().Contains("get"))
            {
                if (item.PostEncoding != null)
                {
                    postencoding = item.PostEncoding;
                }
                var buffer = (byte[])null;

                if (item.PostDataType == PostDataType.Byte && item.PostdataByte != null && item.PostdataByte.Length > 0)
                {
                    buffer = item.PostdataByte;
                }
                else
                {
                    if (item.PostDataType == PostDataType.FilePath && !string.IsNullOrWhiteSpace(item.Postdata))
                    {
                        var r = new StreamReader(item.Postdata, postencoding);
                        buffer = postencoding.GetBytes(r.ReadToEnd());
                        r.Close();
                    }
                    else
                    {
                        if (!string.IsNullOrWhiteSpace(item.Postdata))
                        {
                            buffer = postencoding.GetBytes(item.Postdata);
                        }
                    }
                }
                if (buffer != null)
                {
                    request.ContentLength = buffer.Length;
                    request.GetRequestStream().Write(buffer, 0, buffer.Length);
                }
            }
        }
        /// <summary>
        /// 设置代理
        /// </summary>
        /// <param name="item">参数对象</param>
        private void SetProxy(HttpItem item)
        {
            var isIeProxy = false;
            if (!string.IsNullOrWhiteSpace(item.ProxyIp))
            {
                isIeProxy = item.ProxyIp.ToLower().Contains("ieproxy");
            }
            if (!string.IsNullOrWhiteSpace(item.ProxyIp) && !isIeProxy)
            {
                if (item.ProxyIp.Contains(":"))
                {
                    var plist = item.ProxyIp.Split(':');
                    var myProxy = new WebProxy(plist[0].Trim(), Convert.ToInt32(plist[1].Trim()));

                    myProxy.Credentials = new NetworkCredential(item.ProxyUserName, item.ProxyPwd);

                    request.Proxy = myProxy;
                }
                else
                {
                    var myProxy = new WebProxy(item.ProxyIp, false);

                    myProxy.Credentials = new NetworkCredential(item.ProxyUserName, item.ProxyPwd);

                    request.Proxy = myProxy;
                }
            }
            else
            {
                if (isIeProxy)
                {
                }
                else
                {
                    request.Proxy = item.WebProxy;
                }
            }
        }
        /// <summary>
        /// 为请求准备参数
        /// </summary>
        /// <param name="item">参数列表</param>
        private void SetRequest(HttpItem item)
        {
            SetCer(item);
            if (item.IPEndPoint != null)
            {
                _IPEndPoint = item.IPEndPoint;

                request.ServicePoint.BindIPEndPointDelegate = new BindIPEndPoint(BindIPEndPointCallback);
            }

            if (item.Header != null && item.Header.Count > 0)
            {
                foreach (string key in item.Header.AllKeys)
                {
                    request.Headers.Add(key, item.Header[key]);
                }
            }
            SetProxy(item);
            if (item.ProtocolVersion != null)
            {
                request.ProtocolVersion = item.ProtocolVersion;
            }
            request.ServicePoint.Expect100Continue = item.Expect100Continue;

            request.Method = item.Method;
            request.Timeout = item.Timeout;
            request.KeepAlive = item.KeepAlive;
            request.ReadWriteTimeout = item.ReadWriteTimeout;
            if (!string.IsNullOrWhiteSpace(item.Host))
            {
                request.Host = item.Host;
            }
            if (item.IfModifiedSince != null)
            {
                request.IfModifiedSince = Convert.ToDateTime(item.IfModifiedSince);
            }
            request.Accept = item.Accept;

            request.ContentType = item.ContentType;

            request.UserAgent = item.UserAgent;

            encoding = item.Encoding;

            request.Credentials = item.ICredentials;

            SetCookie(item);

            request.Referer = item.Referer;

            request.AllowAutoRedirect = item.Allowautoredirect;
            if (item.MaximumAutomaticRedirections > 0)
            {
                request.MaximumAutomaticRedirections = item.MaximumAutomaticRedirections;
            }

            SetPostData(item);

            if (item.Connectionlimit > 0)
            {
                request.ServicePoint.ConnectionLimit = item.Connectionlimit;
            }
        }

        // public methods...
        /// <summary>
        /// 根据相传入的数据，得到相应页面数据
        /// </summary>
        /// <param name="item">参数类对象</param>
        /// <returns>返回HttpResult类型</returns>
        public HttpResult GetHtml(HttpItem item)
        {
            var result = new HttpResult();
            try
            {
                SetRequest(item);
            }
            catch (Exception ex)
            {
                return new HttpResult() { Cookie = string.Empty, Header = null, Html = ex.Message, StatusDescription = "配置参数时出错：" + ex.Message };
            }
            try
            {
                using (response = (HttpWebResponse)request.GetResponse())
                {
                    GetData(item, result);
                }
            }
            catch (WebException ex)
            {
                if (ex.Response != null)
                {
                    using (response = (HttpWebResponse)ex.Response)
                    {
                        GetData(item, result);
                    }
                }
                else
                {
                    result.Html = ex.Message;
                }
            }
            catch (Exception ex)
            {
                result.Html = ex.Message;
            }
            if (item.IsToLower)
            {
                result.Html = result.Html.ToLower();
            }
            return result;
        }
    }

    /// <summary>
    /// Http请求参考类
    /// </summary>
    public class HttpItem
    {
        // private fields...
        private string _Accept = "text/html, application/xhtml+xml, */*";
        private string _ContentType = "text/html";
        private Boolean _expect100continue = true;
        private DateTime? _IfModifiedSince = null;
        private IPEndPoint _IPEndPoint = null;
        private Boolean _KeepAlive = true;
        private string _Method = "GET";
        private PostDataType _PostDataType = PostDataType.String;
        private ResultType resulttype = ResultType.String;
        private WebHeaderCollection header = new WebHeaderCollection();
        private ResultCookieType _ResultCookieType = ResultCookieType.String;
        private ICredentials _ICredentials = CredentialCache.DefaultCredentials;
        private int _ReadWriteTimeout = 30000;
        private int _Timeout = 100000;
        private string _UserAgent = "Mozilla/5.0 (compatible; MSIE 9.0; Windows NT 6.1; Trident/5.0)";
        private Boolean allowautoredirect = false;
        private int connectionlimit = 1024;
        private Boolean isToLower = false;

        // public properties...
        /// <summary>
        /// 请求标头值 默认为text/html, application/xhtml+xml, */*
        /// </summary>
        public string Accept
        {
            get
            {
                return _Accept;
            }
            set
            {
                _Accept = value;
            }
        }
        /// <summary>
        /// 支持跳转页面，查询结果将是跳转后的页面，默认是不跳转
        /// </summary>
        public Boolean Allowautoredirect
        {
            get
            {
                return allowautoredirect;
            }
            set
            {
                allowautoredirect = value;
            }
        }
        /// <summary>
        /// 证书绝对路径
        /// </summary>
        public string CerPath { get; set; }
        /// <summary>
        /// 设置509证书集合
        /// </summary>
        public X509CertificateCollection ClentCertificates { get; set; }
        /// <summary>
        /// 最大连接数
        /// </summary>
        public int Connectionlimit
        {
            get
            {
                return connectionlimit;
            }
            set
            {
                connectionlimit = value;
            }
        }
        /// <summary>
        /// 请求发送的数据类型默认 text/html
        /// </summary>
        public string ContentType
        {
            get
            {
                return _ContentType;
            }
            set
            {
                _ContentType = value;
            }
        }
        /// <summary>
        /// 请求时的Cookie
        /// </summary>
        public string Cookie { get; set; }
        /// <summary>
        /// Cookie对象集合
        /// </summary>
        public CookieCollection CookieCollection { get; set; }
        /// <summary>
        /// 返回数据编码默认为NUll,可以自动识别,一般为utf-8,gbk,gb2312
        /// </summary>
        public Encoding Encoding { get; set; }
        /// <summary>
        /// 获取或设置一个 System.Boolean 值，该值确定是否使用 100-Continue 行为。如果 POST 请求需要 100-Continue 响应，则为 true；否则为 false。默认值为 true。
        /// </summary>
        public Boolean Expect100Continue
        {
            get
            {
                return _expect100continue;
            }
            set
            {
                _expect100continue = value;
            }
        }
        /// <summary>
        /// header对象
        /// </summary>
        public WebHeaderCollection Header
        {
            get
            {
                return header;
            }
            set
            {
                header = value;
            }
        }
        /// <summary>
        /// 设置Host的标头信息
        /// </summary>
        public string Host { get; set; }
        /// <summary>
        /// 获取或设置请求的身份验证信息。
        /// </summary>
        public ICredentials ICredentials
        {
            get
            {
                return _ICredentials;
            }
            set
            {
                _ICredentials = value;
            }
        }
        /// <summary>
        /// 获取和设置IfModifiedSince，默认为当前日期和时间
        /// </summary>
        public DateTime? IfModifiedSince
        {
            get
            {
                return _IfModifiedSince;
            }
            set
            {
                _IfModifiedSince = value;
            }
        }
        /// <summary>
        /// 设置本地的出口ip和端口
        /// </summary>]
        /// <example>
        /// item.IPEndPoint = new IPEndPoint(IPAddress.Parse("192.168.1.1"),80);
        /// </example>
        public IPEndPoint IPEndPoint
        {
            get
            {
                return _IPEndPoint;
            }
            set
            {
                _IPEndPoint = value;
            }
        }
        /// <summary>
        /// 是否设置为全文小写，默认为不转化
        /// </summary>
        public Boolean IsToLower
        {
            get
            {
                return isToLower;
            }
            set
            {
                isToLower = value;
            }
        }
        /// <summary>
        /// 获取或设置一个值，该值指示是否与 Internet 资源建立持久性连接默认为true。
        /// </summary>
        public Boolean KeepAlive
        {
            get
            {
                return _KeepAlive;
            }
            set
            {
                _KeepAlive = value;
            }
        }
        /// <summary>
        /// 设置请求将跟随的重定向的最大数目
        /// </summary>
        public int MaximumAutomaticRedirections { get; set; }
        /// <summary>
        /// 请求方式默认为GET方式,当为POST方式时必须设置Postdata的值
        /// </summary>
        public string Method
        {
            get
            {
                return _Method;
            }
            set
            {
                _Method = value;
            }
        }
        /// <summary>
        /// Post请求时要发送的字符串Post数据
        /// </summary>
        public string Postdata { get; set; }
        /// <summary>
        /// Post请求时要发送的Byte类型的Post数据
        /// </summary>
        public byte[] PostdataByte { get; set; }
        /// <summary>
        /// Post的数据类型
        /// </summary>
        public PostDataType PostDataType
        {
            get
            {
                return _PostDataType;
            }
            set
            {
                _PostDataType = value;
            }
        }
        /// <summary>
        /// 设置或获取Post参数编码,默认的为Default编码
        /// </summary>
        public Encoding PostEncoding { get; set; }
        /// <summary>
        /// </summary>
        public Version ProtocolVersion { get; set; }
        /// <summary>
        /// 代理 服务IP,如果要使用IE代理就设置为ieproxy
        /// </summary>
        public string ProxyIp { get; set; }
        /// <summary>
        /// 代理 服务器密码
        /// </summary>
        public string ProxyPwd { get; set; }
        /// <summary>
        /// 代理Proxy 服务器用户名
        /// </summary>
        public string ProxyUserName { get; set; }
        /// <summary>
        /// 默认写入Post数据超时间
        /// </summary>
        public int ReadWriteTimeout
        {
            get
            {
                return _ReadWriteTimeout;
            }
            set
            {
                _ReadWriteTimeout = value;
            }
        }
        /// <summary>
        /// 来源地址，上次访问地址
        /// </summary>
        public string Referer { get; set; }
        /// <summary>
        /// Cookie返回类型,默认的是只返回字符串类型
        /// </summary>
        public ResultCookieType ResultCookieType
        {
            get
            {
                return _ResultCookieType;
            }
            set
            {
                _ResultCookieType = value;
            }
        }
        /// <summary>
        /// 设置返回类型String和Byte
        /// </summary>
        public ResultType ResultType
        {
            get
            {
                return resulttype;
            }
            set
            {
                resulttype = value;
            }
        }
        /// <summary>
        /// 默认请求超时时间
        /// </summary>
        public int Timeout
        {
            get
            {
                return _Timeout;
            }
            set
            {
                _Timeout = value;
            }
        }
        /// <summary>
        /// 请求URL必须填写
        /// </summary>
        public string URL { get; set; }
        /// <summary>
        /// 客户端访问信息默认Mozilla/5.0 (compatible; MSIE 9.0; Windows NT 6.1; Trident/5.0)
        /// </summary>
        public string UserAgent
        {
            get
            {
                return _UserAgent;
            }
            set
            {
                _UserAgent = value;
            }
        }
        /// <summary>
        /// 设置代理对象，不想使用IE默认配置就设置为Null，而且不要设置ProxyIp
        /// </summary>
        public WebProxy WebProxy { get; set; }
    }

    /// <summary>
    /// Http返回参数类
    /// </summary>
    public class HttpResult
    {
        // private fields...
        private string _html = string.Empty;

        // public properties...
        /// <summary>
        /// Http请求返回的Cookie
        /// </summary>
        public string Cookie { get; set; }
        /// <summary>
        /// Cookie对象集合
        /// </summary>
        public CookieCollection CookieCollection { get; set; }
        /// <summary>
        /// header对象
        /// </summary>
        public WebHeaderCollection Header { get; set; }
        /// <summary>
        /// 返回的String类型数据 只有ResultType.String时才返回数据，其它情况为空
        /// </summary>
        public string Html
        {
            get
            {
                return _html;
            }
            set
            {
                _html = value;
            }
        }
        /// <summary>
        /// 获取重定向的URl
        /// </summary>
        public string RedirectUrl
        {
            get
            {
                try
                {
                    if (Header != null && Header.Count > 0)
                    {
                        if (Header.AllKeys.Any(k => k.ToLower().Contains("location")))
                        {
                            var locationurl = Header["location"].ToString().ToLower();

                            if (!string.IsNullOrWhiteSpace(locationurl))
                            {
                                var b = locationurl.StartsWith("http://") || locationurl.StartsWith("https://");
                                if (!b)
                                {
                                    locationurl = new Uri(new Uri(ResponseUri), locationurl).AbsoluteUri;
                                }
                            }
                            return locationurl;
                        }
                    }
                }
                catch
                {
                }
                return string.Empty;
            }
        }
        /// <summary>
        /// 最后访问的URl
        /// </summary>
        public string ResponseUri { get; set; }
        /// <summary>
        /// 返回的Byte数组 只有ResultType.Byte时才返回数据，其它情况为空
        /// </summary>
        public byte[] ResultByte { get; set; }
        /// <summary>
        /// 返回状态码,默认为OK
        /// </summary>
        public HttpStatusCode StatusCode { get; set; }
        /// <summary>
        /// 返回状态说明
        /// </summary>
        public string StatusDescription { get; set; }
    }
}
