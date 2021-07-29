using System;

namespace Benchmark.NET
{
    [AttributeUsage(AttributeTargets.Method, AllowMultiple = false, Inherited = false)]
    public class BenchmarkAttribute : Attribute
    { }
}
