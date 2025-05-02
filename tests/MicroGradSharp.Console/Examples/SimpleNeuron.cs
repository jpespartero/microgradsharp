namespace MicroGradSharp.Console.Examples
{

    public class SimpleNeuronExample : IExample
    {
        public string Name { get; } = "Simple Neuron Example";

        public string Description { get; } = "This example demonstrates a simple neuron with two inputs, two weights, and a bias. It computes the output using the Tanh activation function and visualizes the computation graph.";

        public void Run()
        {
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
    }

}