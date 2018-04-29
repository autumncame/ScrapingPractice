using System;
using System.Diagnostics;
using Xunit;
using System.Linq;
using ChemistwareHouse;
using System.Threading.Tasks;
using Shouldly;

namespace ChemistWarehouse.Tests
{
    public class Units
    {
        [Fact(DisplayName = "Get List of A2 Baby Formular")]
        public async void GetListOfA2BabyFormulas()
        {
            var urls = await ChemistwareHouse.Scraper.ListA2BabyFormulas();

            Debugger.Break();

            urls.Count.ShouldBe(4);
               
        }

        [Fact(DisplayName = "Get price")]
        public async void GetPrices()
        {         
            var urls = await ChemistwareHouse.Scraper.ListA2BabyFormulas();
            var tasks = urls.Select( url => ChemistwareHouse.Scraper.GetPriceForProduct(url)).ToList();

            await Task.WhenAll(tasks);
            var prices = tasks.Select(Task => Task.Result).ToList();

            //var prices = await Task.Run(
            //    urls.Select(url => ChemistwareHouse.Scraper.GetPriceForProduct(url)).ToArray()

            //    );
            Debugger.Break();

            urls.Count.ShouldBe(4);

        }





    }
}
