using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ClassLibrary;
using Xunit;

namespace ModelTests
{
    public class SplineParametersTests
    {
        [Theory]
        [InlineData(5, 0, 1, 0.01, 9)]
        [InlineData(10, -1, 0, 0.001, 3.94)]
        [InlineData(4, 10, -2, -3, -2.073)]
        public void ConstructorTest(int N, double derstart, double derend, double intstart, double intend)
        {
            var sp = new SplineParameters(N, derstart, derend, intstart, intend);
            Assert.Equal(N, sp.nodeNum);
            Assert.Equal(derstart, sp.derivBounds[0]);
            Assert.Equal(derend, sp.derivBounds[1]);
            Assert.Equal(intstart, sp.integBounds[0]);
            Assert.Equal(intend, sp.integBounds[1]);
        }

        [Fact]
        public void CopyConstructorTest()
        {
            var sp = new SplineParameters(10, -1, 0, 0.001, 3.94);
            var csp = new SplineParameters(sp);

            Assert.Equal(sp.nodeNum, csp.nodeNum);
            Assert.Equal(sp.derivBounds[0], csp.derivBounds[0]);
            Assert.Equal(sp.derivBounds[1], csp.derivBounds[1]);
            Assert.Equal(sp.integBounds[0], csp.integBounds[0]);
            Assert.Equal(sp.integBounds[1], csp.integBounds[1]);
        }
    }
}
