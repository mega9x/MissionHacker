﻿using Models.Data;
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
    public IBrowser ChangeIp(string country);
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
}