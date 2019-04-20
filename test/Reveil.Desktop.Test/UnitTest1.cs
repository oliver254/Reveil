using System;
using Xunit;

namespace Reveil.Desktop.Test
{

    public class UnitTest1
    {
        [Fact]
        public void TestMethod1()
        {
            TimeSpan alarme = new TimeSpan(0, 1, 5);
            TimeSpan now = new TimeSpan(23, 0, 5);
            var diff = alarme - now;


            Assert.True(true);


        }
    }
}
