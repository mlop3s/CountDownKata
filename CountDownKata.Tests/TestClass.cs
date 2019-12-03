// NUnit 3 tests
// See documentation : https://github.com/nunit/docs/wiki/NUnit-Documentation
using NUnit.Framework;
using System;
using System.Threading;


namespace CountDownKata.Tests
{

    [TestFixture]
    public class TestClass
    {
        [Test]
        public void CurrentTimeShouldUpdateOnTick()
        {
            // TODO: Add your test code here
            var countDown = new CountDown();
            countDown.Seconds = 14;
            countDown.Minutes = 10; 
            var millis = countDown.StartTime.TotalMilliseconds;
            countDown.StartCounter();
            Thread.Sleep(1000);
            Assert.That(countDown.Current.TotalMilliseconds, Is.LessThan(millis), "must be less");
            countDown.StopCommand.Execute(null);
            countDown.Minutes = 15;

            Assert.That(countDown.Elapsed.TotalMilliseconds, Is.EqualTo(0), "must be 0");
        }


        [TestCase(2,0,1)]
        public void ElapsedTime(int sec, int min, int expectedSec)
        {
            var countDown = new CountDown();
            countDown.Seconds = sec;
            countDown.Minutes = min; 
            var millis = countDown.StartTime.TotalMilliseconds;

            countDown.StartCounter();
            Thread.Sleep(expectedSec * 1000);

            var elapsedSeconds = countDown.Elapsed.TotalSeconds;
            var delta= Math.Abs(elapsedSeconds - (double)expectedSec);
            Assert.That(delta, Is.LessThan(1), $"must be less. Was {elapsedSeconds}");
        }

        [Test]
        public void StopButtonEnabledTest()
        {
            var countDown = new CountDown();
            countDown.Minutes = 14;
            countDown.Seconds = 15;
            Assert.That(countDown.StopCommand.CanExecute(null), Is.False, "should be disabled");

            var fired = false;
            countDown.StopCommand.CanExecuteChanged += (s,e) =>
            {
                fired = true;
            };
            
            countDown.StartCounter();
            
            Assert.That(countDown.StopCommand.CanExecute(null), Is.True);
            Assert.That(fired, Is.True, "not fired");
        }

        [Test]
        public void StartButtonEnabledTest()
        {
            var countDown = new CountDown();
            countDown.Minutes=10; 
            Assert.That(countDown.StartCommand.CanExecute(null), Is.True, "should be enabled");

            var fired = false;
            countDown.StartCounter();

            countDown.StartCommand.CanExecuteChanged += (s,e) =>
            {
                fired = true;
            };
            
            countDown.StopCommand.Execute(null);
            
            Assert.That(countDown.StartCommand.CanExecute(null), Is.True);
            Assert.That(fired, Is.True, "not fired");
        }




    }
}
