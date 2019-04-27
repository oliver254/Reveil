using Reveil.Core;
using Xunit;

namespace Reveil.Desktop.Test
{
    public class MathUtilTest
    {
        [Theory]
        [InlineData("6", 6)]
        [InlineData("4", 5)]
        [InlineData("11", 10)]
        public void ValidateNumberTest(string number, int value)
        {
            int result = MathUtil.ValidateNumber(number, 5, 10);

            Assert.Equal(value, result);
        }

        [Theory]
        [InlineData("6", 7)]
        [InlineData("10", 10)]
        [InlineData("5", 6)]
        [InlineData("4", 6)]
        [InlineData("11", 10)]
        public void IncrementNumberTest(string number, int value)
        {
            int result = MathUtil.IncrementDecrementNumber(number, 5, 10, true);

            Assert.Equal(value, result);
        }

        [Theory]
        [InlineData("6", 5)]
        [InlineData("11", 9)]
        [InlineData("4", 4)]
        [InlineData("0", 4)]
        public void DecrementNumberTest(string number, int value)
        {
            int result = MathUtil.IncrementDecrementNumber(number, 5, 10, false);

            Assert.Equal(value, result);
        }
    }
}
