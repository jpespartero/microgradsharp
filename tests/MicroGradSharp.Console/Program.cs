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

            // Inputs
            var x1 = new Value(2.0);
            x1.Label = "x1";
            var x2 = new Value(0.0);
            x2.Label = "x2";

            // Weights
            var w1 = new Value(-3.0);    
            w1.Label = "w1";
            var w2 = new Value(1.0);
            w2.Label = "w2";

            // Bias of the neuron
            var b = new Value(6.8813735870195432);
            
            // x1*w1 + x2*w2 + b
            var x1w1 = x1 * w1; // Multiply x1 and w1
            x1w1.Label = "x1*w1";
            var x2w2 = x2 * w2; // Multiply x2 and w2
            x2w2.Label = "x2*w2";

            var n = x1w1 + x2w2 + b; // Add the results and bias
            n.Label = "n"; // Label the neuron output

            var o = n.Tanh(); // Apply the Tanh activation function to the neuron output
            o.Label = "o"; // Label the output
            
            o.Grad = 1.0;
            o.Backward(); // Calculate gradients
            
            // Print an improved ASCII visualization
            System.Console.WriteLine("\nASCII Graph Visualization:");
            
            AsciiGraphVisualizer.PrintAsciiGraph(o);
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
