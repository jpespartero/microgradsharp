using Xunit;

namespace MicroGradSharp.Tests
{
    public class ValueTests
    {
        [Fact]
        public void Value_CanBeInstantiated()
        {
            // This is just a placeholder test to verify the project structure
            var value = new Value(5.0);
            Assert.NotNull(value);
        }

        public void Value_KarpatyVideoExample()
        {
            //ASCII Graph Visualization:
            // └── [0] Value=0.71, Grad=1.00, Op=Tanh
            //└── [n] Value=0.88, Grad=0.50, Op=Add
            //├── [2] Value=-6.00, Grad=0.50, Op=Add
            //│   ├── [x1*w1] Value=-6.00, Grad=0.50, Op=Multiply
            //│   │   ├── [x1] Value=2.00, Grad=-1.50, Op=None
            //│   │   └── [w1] Value=-3.00, Grad=1.00, Op=None
            //│   └── [x2*w2] Value=0.00, Grad=0.50, Op=Multiply
            //│       ├── [x2] Value=0.00, Grad=0.50, Op=None
            //│       └── [w2] Value=1.00, Grad=0.00, Op=None
            //└── [9] Value=6.88, Grad=0.50, Op=None



        }
    }
} 