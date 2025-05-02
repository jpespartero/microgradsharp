namespace MicroGradSharp
{
    public class MultiLayerPerceptron
    {

        /// <summary>
        /// The layers of the MLP.
        /// Each layer contains a set of neurons.
        /// </summary>
        public Layer[] Layers { get; }

        /// <summary>
        /// The size of the input layer.
        /// </summary>
        public int InputSize { get; }

        /// <summary>
        /// The size of the output layer.
        /// </summary>
        public int OutputSize { get; }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="inputSize"></param>
        /// <param name="layerSizes"></param>
        public MultiLayerPerceptron(int inputSize, int[] layerSizes)
        {
            InputSize = inputSize;
            OutputSize = layerSizes[layerSizes.Length - 1];
            Layers = new Layer[layerSizes.Length];

            for (int i = 0; i < layerSizes.Length; i++)
            {
                int currentInputSize = (i == 0) ? inputSize : layerSizes[i - 1];
                Layers[i] = new Layer(currentInputSize, layerSizes[i]);
            }
        }

        /// <summary>
        /// Performs a forward pass through the MLP.
        /// </summary>
        /// <param name="inputs"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentException"></exception>
        public Value[] Forward(Value[] inputs)
        {
            if (inputs.Length != InputSize)
            {
                throw new ArgumentException($"Input size must be {InputSize}.");
            }

            // Compute the output of each layer
            // and pass the output to the next layer
            Value[] outputs = new Value[inputs.Length];
            for (int i = 0; i < Layers.Length; i++)
            {
                outputs = Layers[i].Forward(outputs);
            }
            return outputs;
        }

    }
    




}