// See https://aka.ms/new-console-template for more information
using BenchmarkDotNet.Running;
using MultiIndex;

//Console.WriteLine("Hello, World!");

BenchmarkRunner.Run<MultiIndexDictionaryBenchmark>();
