using Models.Data;
using OpenQA.Selenium;

namespace Crawler;

/// <summary>
/// 浏览器接口
/// </summary>
public interface IBrowser
{
    /// <summary>
    /// 获取已经通过 QuerySelector 方法获取到的元素
    /// </summary>
    public IWebElement Queryed { get; }
    /// <summary>
    /// 获取已经通过 QuerySelectorAll 方法获取到的元素. 不能连续链式调用.
    /// </summary>
    public IEnumerable<IWebElement> QueryedAll { get; }
    /// <summary>
    /// 等待时间
    /// </summary>
    public TimeSpan Timeout { get; set; }
    /// <summary>
    /// 获取浏览器 Driver 的异步方法
    /// </summary>
    public Task<IWebDriver> GetDriver();
    /// <summary>
    /// 更换 IP 的方法. 默认实现是通过访问 922 的 API 链接来提取 IP.
    /// </summary>
    public Task<IBrowser> ChangeIp(string country);
    /// <summary>
    /// 获取已打开浏览器 IP 的数据. 
    /// </summary>
    public IPData GetIPData();
    /// <summary>
    /// 根据 Css 选择器来模拟输入值. 不能链式调用.
    /// </summary>
    /// <param name="css"></param>
    /// <param name="keys"></param>
    public IBrowser SendKeysByCss(string css, string keys);
    /// <summary>
    /// 根据 Css 选择器来模拟点击. 不能链式调用.
    /// </summary>
    /// <param name="css"></param>
    public IBrowser ClickByCss(string css);
    /// <summary>
    /// 根据 Css 获取元素列表并存储至 _queryedAll 字段.
    /// </summary>
    /// <param name="keys"></param>
    public IBrowser QuerySelectorAll(string css);
    /// <summary>
    /// 根据 Css 选择器来选择元素.
    /// 要通过 End 方法来储存已获取的元素.
    /// </summary>
    /// <param name="css"></param>
    public IBrowser QuerySelector(string css);
    /// <summary>
    /// 清空已搜索的, 并把结果保存至类内 _queryed 和 _queryedAll 字段.
    /// </summary>
    /// <returns></returns>
    public IBrowser End();
    /// <summary>
    /// 代理方法. 对本类内已选择的元素 (_queryed) 执行 SendKeys 方法
    /// </summary>
    /// <param name="keys"></param>
    public IBrowser SendKeys(string keys);
    /// <summary>
    /// 代理方法. 对本类内已选择的元素 (_queryed) 执行 Click 方法
    /// </summary>
    /// <param name="keys"></param>
    public IBrowser Click();
    /// <summary>
    /// 代理方法. 使用 js 脚本点按由 Css 查询到的元素.
    /// </summary>
    /// <param name="css"></param>
    public IBrowser ClickByJsCss(string css);
    /// <summary>
    /// 代理方法. 对本类内已选择的元素 (_queryed) 执行 Clear 方法
    /// </summary>
    /// <param name="keys"></param>
    public IBrowser Clear();
    /// <summary>
    /// 持续等待到元素无法获取为止.
    /// </summary>
    /// <param name="css"></param>
    /// <param name="timeout"></param>
    public Task<IBrowser> WaitUntilNull(string css, int timeout);
    /// <summary>
    /// 让浏览器滚
    /// </summary>
    /// <returns></returns>
    public Task Quit();
    /// <summary>
    /// 刷新 Ip 信息
    /// </summary>
    /// <returns></returns>
    public Task<IBrowser> RefreshIpData();
    /// <summary>
    /// 根据 css 来获取 Selector 并按照索引选择值
    /// </summary>
    /// <returns></returns>
    public IBrowser SelectByCss(string css, int index);
    /// <summary>
    /// 根据 css 来获取 Selector 并按照字符串选择值
    /// </summary>
    /// <param name="css"></param>
    /// <param name="name"></param>
    public IBrowser SelectByCss(string css, string name);
    /// <summary>
    /// 点击 iframe 下面的元素
    /// </summary>
    /// <param name="iframeCss"></param>
    /// <param name="eleCss"></param>
    /// <returns></returns>
    public IBrowser ClickEleOfIframe(string iframeCss, string eleCss);
}