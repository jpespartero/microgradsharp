namespace MicroGradSharp
{

    /// <summary>
    /// Represents a single neuron in a neural network.
    /// </summary>
    public class Neuron
    {
        /// <summary>
        /// The weights of the neuron, each representing the strength of the connection to an input.
        /// </summary>
        public Value[] Weights { get; }

        /// <summary>
        /// 
        /// </summary>
        public Value Bias { get; }

        /// <summary>
        /// The output of the neuron after applying the activation function.
        /// </summary>
        public Value Output { get; private set; }

        /// <summary>
        /// The activation function to be used by the neuron.
        /// Tanh by default.
        /// </summary>
        public ActivationFunction ActivationFunction { get; set; } = ActivationFunction.Tanh;


        /// <summary>
        /// 
        /// </summary>
        /// <param name="inputSize"></param>
        public Neuron(int inputSize)
        {
            Weights = new Value[inputSize];
            for (int i = 0; i < inputSize; i++)
            {
                Weights[i] = new Value(new Random().NextDouble());
            }
            Bias = new Value(new Random().NextDouble());
            Output = new Value(0.0);
        }


        public Value Forward(Value[] inputs)
        {
            if (inputs.Length != Weights.Length)
            {
                throw new ArgumentException("Input size must match the number of weights.");
            }

            // Compute the weighted sum of inputs and bias
            double activation = Bias.Data;
            for (int i = 0; i < inputs.Length; i++)
            {
                activation += Weights[i].Data * inputs[i].Data;
            }

            // Apply activation function
            Output = ApplyActivationFunction(activation);
            return Output;
        }


        private Value ApplyActivationFunction(double x)
        {   
            Value result = new Value(x);
            switch (ActivationFunction)
            {
                case ActivationFunction.Sigmoid:
                    return result.Sigmoid();
                case ActivationFunction.Tanh:
                    return result.Tanh();
                case ActivationFunction.ReLU:
                    return result.ReLU();
                default:
                    throw new NotSupportedException("Unsupported activation function.");
            }
        }   
    }
}


