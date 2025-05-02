namespace MicroGradSharp
{
    public class Value
    {

        #region Properties

        /// <summary>
        /// 
        /// </summary>
        public double Data { get; }

        /// <summary>
        /// 
        /// </summary>
        public double Grad { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string Label { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public Func<double>? Backguards { get; set; }
        
        /// <summary>
        /// Previous values in the computation graph.
        /// </summary>
        public Value[] Previous { get; }

        /// <summary>
        /// 
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
            // Implement the backward pass logic here
            // This is a placeholder for the actual backward pass implementation
            if (Backguards != null)
            {
                double result = Backguards();
                foreach (var prev in Previous)
                {
                    prev.Backward();
                }
            }
        }
        
        #region Activation Functions

        public Value Tanh()
        {
            Value result = new Value(Math.Tanh(Data), [this], Operation.Tanh);
            result.Backguards = () =>
            {
                Grad += (1.0 - Math.Pow(result.Data, 2)) * result.Grad;
                return result.Data;
            };
            return result;
        }

        public Value Sigmoid()
        {
            Value result = new Value(1.0 / (1.0 + Math.Exp(-Data)), [this], Operation.Sigmoid);
            result.Backguards = () =>
            {
                Grad += (1.0 - result.Data) * result.Data * result.Grad;
                return result.Data;
            };
            return result;
        }
        public Value ReLU()
        {
            Value result = new Value(Math.Max(0.0, Data), [this], Operation.ReLU);
            result.Backguards = () =>
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
            result.Backguards = () =>
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
            result.Backguards = () =>
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
            return a + (-b);
        }

        public static Value operator -(Value a, double b)
        {
            return a + (-b);
        }
        public static Value Pow(Value a, int power)
        {
            Value result = new Value(Math.Pow(a.Data, power), [a, new Value(power)], Operation.Power);
            result.Backguards = () =>
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
            return a / new Value(b);
        }

        public static Value operator /(double a, Value b)
        {
            return a * Pow(b, -1);
        }
        public static Value Exp(Value a)
        {
            Value result = new Value (Math.Exp(a.Data));
            result.Backguards = () =>
            {
                a.Grad += Math.Exp(a.Data) * result.Grad;
                return result.Data;
            };
            return result;
        }

        #endregion

    }
}