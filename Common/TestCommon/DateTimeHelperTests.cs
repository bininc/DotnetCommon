using System;
using Common;
using NUnit.Framework;

namespace TestCommon
{
    public class DateTimeHelperTests
    {
        [SetUp]
        public void Setup()
        {
            
        }

        [Test]
        public void Test1()
        {
            Console.WriteLine(DateTimeHelper.Now.ToDayBegin());
            Assert.AreEqual(DateTimeHelper.Now.ToDayBegin(),DateTime.Today);
            Console.WriteLine(DateTimeHelper.Now.ToFormatDayEndStr());
            Assert.AreEqual(DateTimeHelper.Now.ToFormatDayEndStr(), "2020-05-13 23:59:59");
            //Assert.Pass();
        }
    }
}