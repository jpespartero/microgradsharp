using System;
using MicroGradSharp;

namespace MicroGradSharp.Console
{
    public static class Program
    {
        static void Main(string[] args)
        {
            System.Console.WriteLine("MicroGradSharp Console Test");
            System.Console.WriteLine("===========================");

            var a = new Value(2.0);
            var b = new Value(-3.0);
            var c = new Value(10.0);
            var d = a*b + c;

            d.Grad = 1.0;
            d.Backward(); // Calculate gradients
            System.Console.WriteLine($"Created value: {d}");

            // Print children of the value recursively with increasing indentation
            System.Console.WriteLine("Children of the value:");
            PrintValueTree(d, 0);
            
            // Print an improved ASCII visualization
            System.Console.WriteLine("\nASCII Graph Visualization:");
            
            AsciiGraphVisualizer.PrintAsciiGraph(d);
        }

        /// <summary>
        /// Recursively prints a value and its children with increasing indentation
        /// </summary>
        /// <param name="value">The value to print</param>
        /// <param name="depth">Current depth in the tree</param>
        static void PrintValueTree(Value value, int depth)
        {
            string indent = new string('-', depth);
            System.Console.WriteLine($"{indent}{value}");
            
            if (value.Previous != null)
            {
                foreach (var child in value.Previous)
                {
                    PrintValueTree(child, depth + 1);
                }
            }
        }
    }
}
