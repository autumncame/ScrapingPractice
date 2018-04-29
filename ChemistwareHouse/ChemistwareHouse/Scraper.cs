using System;
using System.Collections.Generic; //for List
using System.Net.Http; //
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks; //异步task
using HtmlAgilityPack;


namespace ChemistwareHouse
{
    public class Scraper
    {
        public async static Task<List<string>> ListA2BabyFormulas()  //async异步调用
        {
            String url = @"https://www.chemistwarehouse.com.au/search/go?w=A2%20Stage&ts=custom";

            using (HttpClient httpClient = new HttpClient())  //using 实现IDdisposal 不用手动dispose
            {
                var response = await httpClient.GetAsync(url); //下载需要时间，所以需要异步调用，如果多线程不需要等了

                var html = await response.Content.ReadAsStringAsync();

                //create a new html document with agilty pack
                HtmlDocument document = new HtmlDocument();

                //load the html from response
                document.LoadHtml(html);

                var products = document.DocumentNode.DescendantsAndSelf()
                    .Where(node => node.Name.ToLower() == "div" && node.HasClass("Product") )
                    .Select(node => node.ParentNode.GetAttributeValue("href","")) //转换
                    .Where(link => link != null && link != "")
                    .ToList(); //转换成list列表 ToArray有时候用

                return products;
            }
        }


        public async static Task<decimal?> GetPriceForProduct(string url) //?允许为空
        {
            using (HttpClient client = new HttpClient())
            {
                var response = await client.GetAsync(url);
                var html = await response.Content.ReadAsStringAsync();
                HtmlDocument document = new HtmlDocument();
                document.LoadHtml(html);

                var first = document.DocumentNode.DescendantsAndSelf()
                    .Where(node => node.Name.ToLower() == "div" && node.HasClass("Price"))
                    .Select(node => node.InnerText)
                    .FirstOrDefault();
                if (first != null)
                {
                    Regex rgxPrice = new Regex(@"\$([\d\.]+)"); //+匹配1到多个 （）group 
                    var match = rgxPrice.Match(first);
                    if (match.Success)
                    {
                        decimal price = 0;
                        if (decimal.TryParse(match.Groups[1].Value, out price))
                        {
                            return price;
                        }
                    }
                }
                return null;
            }
        }

    }
}

