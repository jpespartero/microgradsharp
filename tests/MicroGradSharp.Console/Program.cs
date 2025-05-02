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

            // Example 1: Simple Neuron 
            var simpleNeuronExample = new MicroGradSharp.Console.Examples.SimpleNeuronExample();
            System.Console.WriteLine($"Running example: {simpleNeuronExample.Name}");
            System.Console.WriteLine(simpleNeuronExample.Description);
            simpleNeuronExample.Run();
            System.Console.WriteLine("Press any key to continue...");

            // Example 2: Multi-Layer Perceptron
            var mlpExample = new MicroGradSharp.Console.Examples.MultiLayerPerceptronExample();
            System.Console.WriteLine($"Running example: {mlpExample.Name}");    
            System.Console.WriteLine(mlpExample.Description);
            mlpExample.Run();
            System.Console.WriteLine("Press any key to continue...");


        }

    }
}
