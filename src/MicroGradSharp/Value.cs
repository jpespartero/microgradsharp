namespace MicroGradSharp
{
    public class Value
    {
        
        // For topological sorting in backward pass
        private bool _visited = false;

        #region Properties

        /// <summary>
        /// The scalar value stored in this node
        /// </summary>
        public double Data { get; }

        /// <summary>
        /// The gradient of the loss with respect to this value
        /// </summary>
        public double Grad { get; set; }

        /// <summary>
        /// Optional label for this value node
        /// </summary>
        public string Label { get; set; }

        /// <summary>
        /// Function to compute gradients during backpropagation
        /// </summary>
        public Func<double>? BackwardFunction { get; set; }
        
        /// <summary>
        /// Previous values in the computation graph.
        /// </summary>
        public Value[] Previous { get; }

        /// <summary>
        /// The operation that created this value
        /// </summary>
        public Operation Operation { get; }    

        #endregion

        /// <summary>
        /// 
        /// </summary>
        /// <param name="data"></param>
        public Value(double data, Value[] previous = null, Operation operation = Operation.None)
        {
            Data = data;
            Grad = 0.0;
            Label = "";
            Previous = previous ?? Array.Empty<Value>();
            Operation = operation;
        }
        
        public override string ToString()
        {
            return $"Value(data={Data}, grad={Grad})";
        }

        public override bool Equals(object? obj)
        {
            if (obj is not Value other)
                return false;
            
            // First compare data values
            if (Math.Abs(Data - other.Data) >= 1e-10)
                return false;
            
            // Then compare operations
            if (Operation != other.Operation)
                return false;
            
            // Compare previous values
            if (Previous.Length != other.Previous.Length)
                return false;
            
            // Check each previous value
            for (int i = 0; i < Previous.Length; i++)
            {
                if (!Previous[i].Equals(other.Previous[i]))
                    return false;
            }
            
            // Note: We deliberately skip comparing Backguards functions
            // as function equality is hard to determine and the functions
            // should be consistent if they were created with the same operations
            
            return true;
        }

        public override int GetHashCode()
        {
            unchecked
            {
                int hash = 17;
                hash = hash * 23 + Data.GetHashCode();
                hash = hash * 23 + Operation.GetHashCode();
                
                // Add hash codes of previous values
                foreach (var prev in Previous)
                {
                    hash = hash * 23 + prev?.GetHashCode() ?? 0;
                }
                
                return hash;
            }
        }

        public void Backward()
        {
            // Initialize gradient of output node to 1.0
            Grad = 1.0;
            
            // Build topological ordering of nodes
            var topo = new List<Value>();
            BuildTopo(this, topo);
            
            // Process nodes in reverse order (from outputs to inputs)
            foreach (var node in topo)
            {
                // Apply the backward function to compute gradients
                node.BackwardFunction?.Invoke();
            }
        }
        /// <summary>
        /// Builds a topological ordering of the computation graph
        /// </summary>
        private void BuildTopo(Value v, List<Value> topo, HashSet<Value> visited = null)
        {
            visited ??= new HashSet<Value>();
            
            if (visited.Contains(v))
                return;
                
            visited.Add(v);
            
            // Visit all children before adding this node
            foreach (var child in v.Previous)
            {
                BuildTopo(child, topo, visited);
            }
            
            topo.Add(v);
        }

        #region Activation Functions

        public Value Tanh()
        {
            Value result = new Value(Math.Tanh(Data), new[] { this }, Operation.Tanh);
            result.BackwardFunction = () =>
            {
                Grad += (1.0 - Math.Pow(result.Data, 2)) * result.Grad;
                return result.Data;
            };
            return result;
        }

        public Value Sigmoid()
        {
            Value result = new Value(1.0 / (1.0 + Math.Exp(-Data)), [this], Operation.Sigmoid);
            result.BackwardFunction = () =>
            {
                Grad += (result.Data * (1.0 - result.Data)) * result.Grad;
                return result.Data;
            };
            return result;
        }
        public Value ReLU()
        {
            Value result = new Value(Math.Max(0.0, Data), [this], Operation.ReLU);
            result.BackwardFunction = () =>
            {
                Grad += (Data > 0.0 ? 1.0 : 0.0) * result.Grad;
                return result.Data;
            };
            return result;
        }

        #endregion

        #region Arithmetic Operations
        
        public static bool operator ==(Value? a, Value? b)
        {
            if (ReferenceEquals(a, b))
                return true;
            
            if (a is null || b is null)
                return false;
            
            return a.Equals(b);
        }

        public static bool operator !=(Value? a, Value? b)
        {
            return !(a == b);
        }

        public static Value operator +(Value a, Value b)
        {
            Value result = new Value(a.Data + b.Data, [a, b], Operation.Add);
            result.BackwardFunction = () =>
            {
                a.Grad += result.Grad;
                b.Grad += result.Grad;
                return result.Data;
            };
            return result;
        }

        public static Value operator +(Value a, double b) 
        {
            return a + new Value(b);
        }

        public static Value operator +(double a, Value b)
        {
            return new Value(a) + b;
        }
        
        public static Value operator *(Value a, Value b)
        {
            Value result = new Value(a.Data * b.Data, [a, b], Operation.Multiply);
            result.BackwardFunction = () =>
            {
                a.Grad += b.Data * result.Grad;
                b.Grad += a.Data * result.Grad;
                return result.Data;
            };
            return result;
        }

        public static Value operator *(Value a, double b)
        {
            return a * new Value(b);
        }

        public static Value operator *(double a, Value b)
        {
            return new Value(a) * b;
        }

        public static Value operator -(Value a)
        {
            return a * (-1.0);
        }
   
        public static Value operator -(Value a, Value b)
        {
            return a + (-b);
        }

        public static Value operator -(double a, Value b)
        {
            return new Value(a) + (-b);
        }

        public static Value operator -(Value a, double b)
        {
            return a + (-b);
        }

        public static Value Pow(Value a, int power)
        {
            Value result = new Value(Math.Pow(a.Data, power), new[] { a }, Operation.Power);
            result.BackwardFunction = () =>
            {
                a.Grad += power * Math.Pow(a.Data, power-1) * result.Grad;
                return result.Data;
            };
            return result;
        }

        public static Value operator /(Value a, Value b)
        {
            return a * Pow(b, -1);
        }

        public static Value operator /(Value a, double b)
        {
            return a * new Value(1.0 / b);
        }

        public static Value operator /(double a, Value b)
        {
            return a * Pow(b, -1);
        }

        public static Value Exp(Value a)
        {
            Value result = new Value(Math.Exp(a.Data), [a], Operation.Exp);
            result.BackwardFunction = () =>
            {
                a.Grad += result.Data * result.Grad; // d/dx(e^x) = e^x
                return result.Data;
            };
            return result;
        }

        #endregion

    }
}