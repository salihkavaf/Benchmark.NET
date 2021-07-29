using Castle.DynamicProxy;

using System;
using System.Reflection;

namespace Benchmark.NET
{
    public class BenchmarkInterceptor : IInterceptor
    {
        private readonly Benchmark _bm;

        public BenchmarkInterceptor(Benchmark benchmark)
        {
            _bm = benchmark ?? throw new ArgumentNullException(nameof(benchmark));
        }

        public void Intercept(IInvocation invocation)
        {
            var attr = invocation.Method.GetCustomAttribute<BenchmarkAttribute>();
            if (attr is null)
            {
                invocation.Proceed();
                return;
            }

            _bm.Begin();
            invocation.Proceed();
            _bm.End(invocation.Method.Name);
        }
    }
}
