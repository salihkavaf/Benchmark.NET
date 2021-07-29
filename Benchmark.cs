using Castle.DynamicProxy;

using Microsoft.Extensions.Logging;

using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace Benchmark.NET
{
    public class Benchmark
    {
        private readonly ILogger _logger;

        private readonly Stack<Stopwatch> _stack = new Stack<Stopwatch>();
        private readonly List<string> _records = new List<string>();

        public Benchmark(ILogger logger)
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        public void Begin()
        {
            _stack.Push(Stopwatch.StartNew());
        }

        public void End(string name)
        {
            var sw = _stack.Pop();
            sw.Stop();

            _records.Add($"{name} elapsed time = {sw.ElapsedMilliseconds}ms");
        }

        public void Clear()
        {
            _records.Clear();
            _stack.Clear();
        }

        public void LogAll()
        {
            foreach (var rec in _records)
            {
                _logger.LogDebug("[Benchmark] " + rec);
            }
        }

        public T Wrap<T>(params object[] args) where T : class
        {
            var gen = new ProxyGenerator();

            return (T)gen.CreateClassProxy(typeof(T), args, new BenchmarkInterceptor(this));
        }
    }
}
