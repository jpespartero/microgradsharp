namespace MicroGradSharp.Console.Examples

{
    public interface IExample
    {
        /// <summary>
        /// The name of the example.
        /// </summary>
        string Name { get; }

        /// <summary>
        /// The description of the example.
        /// </summary>
        string Description { get; }

        /// <summary>
        /// The code to run the example.
        /// </summary>
        void Run();
    }
}

