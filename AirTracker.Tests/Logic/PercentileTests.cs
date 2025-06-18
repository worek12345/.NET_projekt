using AirTracker.Models;
using AirTracker.DTO;
using AirTracker.Data;
using AirTracker.Repositories;
using Microsoft.EntityFrameworkCore;



namespace AirTracker.Tests.Logic
{
    public class PercentileTests
    {
        [Fact]
        public void Percentile_Calculates_Median_Correctly()
        {
            double[] values = { 10, 20, 30, 40, 50 };
            int n = values.Length;

            // Logika z kontrolera!
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

            Assert.Equal(30, Percentile(0.5)); // Mediana
        }

        [Fact]
        public void Percentile_Calculates_Min_Max()
        {
            double[] values = { 10, 20, 30, 40, 50 };
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

            Assert.Equal(10, Percentile(0)); // Min
            Assert.Equal(50, Percentile(1)); // Max
        }
    }
}
