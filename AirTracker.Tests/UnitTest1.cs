using System;
using Xunit;
using System.Linq;

namespace AirTracker.Tests
{
    public class UnitTest1
    {
        [Fact]
        public void Percentile_Returns_Correct_Median()
        {
            // Przyk³adowa logika percentyla (jak w Twoim kontrolerze)
            var values = new double[] { 10, 20, 30, 40, 50 };
            int n = values.Length;

            double Percentile(double p)
            {
                if (n == 0) return 0;
                var idx = p * (n - 1);
                var lo = (int)Math.Floor(idx);
                var hi = (int)Math.Ceiling(idx);
                return lo == hi
                    ? values[lo]
                    : values[lo] + (values[hi] - values[lo]) * (idx - lo);
            }

            // Sprawdzamy medianê
            Assert.Equal(30, Percentile(0.5));
        }
    }
}
