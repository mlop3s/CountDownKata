// NUnit 3 tests
// See documentation : https://github.com/nunit/docs/wiki/NUnit-Documentation
using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using CountDownKata;
using System;
using System.Threading;


namespace CountDownKata.Tests
{

[TestFixture]
    public class TestClass
    {
        [Test]
        public void TestMethod()
        {
            // TODO: Add your test code here
            var countDown = new CountDown();
            countDown.Time = new TimeSpan(10, 14, 15);
            var millis = countDown.Time.TotalMilliseconds;
            countDown.StartTime();
            Thread.Sleep(1000);
            Assert.That(countDown.Current.TotalMilliseconds, Is.LessThan(millis), "must be less");
        }
    }
}
