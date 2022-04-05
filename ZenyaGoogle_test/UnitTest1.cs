using System;
using Xunit;

namespace ZenyaGoogle_test
{
    public class Test_test
    {
        public bool IsPrime(int candidate)
        {
            if (candidate == 1)
            {
                return false;
            }
            throw new NotImplementedException("Not fully implemented.");
        }

        [Fact]
        public void Test1()
        {
            bool result = IsPrime(1);

            Assert.False(result, "1 should not be prime");
        }
    }
}
