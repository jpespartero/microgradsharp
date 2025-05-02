namespace MicroGradSharp
{
    public class Layer
    {
        /// <summary>
        /// The neurons in this layer.
        /// </summary>
        public Neuron[] Neurons { get; }

        /// <summary>
        /// Creates a new layer with the specified number of neurons.
        /// Each neuron will have the specified number of inputs.
        /// </summary>
        /// <param name="neuronCount"></param>
        public Layer(int inputSize, int neuronCount)
        {
            Neurons = new Neuron[neuronCount];
            for (int i = 0; i < neuronCount; i++)
            {
                Neurons[i] = new Neuron(inputSize);
            }
        }

        /// <summary>
        /// Performs a forward pass through the layer.
        /// </summary>
        /// <param name="inputs"></param>
        public Value[] Forward(Value[] inputs)
        {
            if (inputs.Length != Neurons[0].Weights.Length)
            {
                throw new ArgumentException("Input size must match the number of weights.");
            }
            // Compute the output of each neuron
            Value[] outputs = new Value[Neurons.Length];
            for (int i = 0; i < Neurons.Length; i++)
            {
                outputs[i] = Neurons[i].Forward(inputs);
            }
            return outputs;

        }
    }



}