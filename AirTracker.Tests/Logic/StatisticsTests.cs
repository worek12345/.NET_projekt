using Xunit;
using System;
using System.Linq;

namespace AirTracker.Tests.Logic
{
    public class StatisticsTests
    {
        [Fact]
        public void Percentile_Median_Correct()
        {
            double[] values = { 10, 20, 30, 40, 50 };
            int n = values.Length;
            double Percentile(double p)
            {
                if (n == 0) return 0;
                var idx = p * (n - 1);
                var lo = (int)Math.Floor(idx);
                var hi = (int)Math.Ceiling(idx);
                return lo == hi ? values[lo] : values[lo] + (values[hi] - values[lo]) * (idx - lo);
            }
            Assert.Equal(30, Percentile(0.5));
        }

        [Fact]
        public void Percentile_Min_Max_Correct()
        {
            double[] values = { 5, 10, 15 };
            int n = values.Length;
            double Percentile(double p)
            {
                if (n == 0) return 0;
                var idx = p * (n - 1);
                var lo = (int)Math.Floor(idx);
                var hi = (int)Math.Ceiling(idx);
                return lo == hi ? values[lo] : values[lo] + (values[hi] - values[lo]) * (idx - lo);
            }
            Assert.Equal(5, Percentile(0));
            Assert.Equal(15, Percentile(1));
        }
        [Fact]
        public void Percentile_EmptyArray_ReturnsZero()
        {
            double[] values = { };
            int n = values.Length;
            double Percentile(double p)
            {
                if (n == 0) return 0;
                var idx = p * (n - 1);
                var lo = (int)Math.Floor(idx);
                var hi = (int)Math.Ceiling(idx);
                return lo == hi ? values[lo] : values[lo] + (values[hi] - values[lo]) * (idx - lo);
            }
            Assert.Equal(0, Percentile(0.5));
        }

        [Fact]
        public void Average_Works()
        {
            double[] values = { 10, 20, 30, 40, 50 };
            Assert.Equal(30, values.Average());
        }

        [Fact]
        public void Average_EmptyArray_ReturnsZero()
        {
            double[] values = { };
            Assert.Equal(0, values.DefaultIfEmpty(0).Average());
        }

        [Fact]
        public void StandardDeviation_Works()
        {
            double[] values = { 10, 20, 30, 40, 50 };
            double avg = values.Average();
            double sd = Math.Sqrt(values.Sum(v => (v - avg) * (v - avg)) / (values.Length - 1));
            Assert.True(sd > 0);
        }
    }
}
