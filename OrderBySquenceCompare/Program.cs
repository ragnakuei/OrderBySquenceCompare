using System;
using System.Collections.Generic;
using System.Linq;
using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Running;

namespace OrderBySquenceCompare
{
    class Program
    {
        static void Main(string[] args)
        {
            var summary = BenchmarkRunner.Run<TestRunner>();
        }
    }

    [ClrJob, MonoJob, CoreJob] // 可以針對不同的 CLR 進行測試
    [MinColumn, MaxColumn]
    [MemoryDiagnoser]
    public class TestRunner
    {
        private readonly TestClass _test = new TestClass();

        private readonly List<int> _ints1 = new List<int>();

        private readonly List<int> _ints2 = new List<int>();

        private readonly Random r = new Random();

        public TestRunner()
        {
            _ints1.Add(r.Next(1, 100));
            _ints1.Add(r.Next(1, 100));
            _ints1.Add(r.Next(1, 100));

            _ints2.Add(r.Next(1, 100));
            _ints2.Add(r.Next(1, 100));
            _ints2.Add(r.Next(1, 100));
        }

        [Benchmark]
        public void FullCompare1() => _test.FullCompare1(_ints1, _ints2);

        [Benchmark]
        public void FullCompare2() => _test.FullCompare2(_ints1, _ints2);
    }

    public class TestClass
    {
        private readonly List<int> _ints = Enumerable.Range(1, 10000).ToList();

        public bool FullCompare1(List<int> target1, List<int> target2)
        {
            return target1.All(t => target2.Contains(t));
        }
        public bool FullCompare2(List<int> target1, List<int> target2)
        {
            return target1.OrderBy(t1 => t1).SequenceEqual(target2.OrderBy(t2 => t2));
        }
    }
}
