using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ClassLibrary;
using Xunit;
using FluentAssertions;

namespace ModelTests
{
    public class SplinesDataTests
    {
        [Fact]
        public void ConstructorTest()
        {
            var md = new MeasuredData(10, 0.001, 3.94, SPf.cubic, Uniform.uniform);
            var sp = new SplineParameters(100, -1, 0, 0.05, 2.97);

            var sd = new SplinesData(md, sp);
            Assert.NotNull(md);
            Assert.NotNull(sp);
        }

        [Fact]
        public void InterpolationExceptionTest()
        {
            var code = new Action(() =>
            {
                var md = new MeasuredData(1, 2, 4, SPf.cubic, Uniform.uniform);
                var sp = new SplineParameters(100, -1, 0, 0.05, 2.97);
                var sd = new SplinesData(md, sp);

                sd.Interpolate();
            });
            code.Should().Throw<Exception>();

            code = new Action(() =>
            {
                var md = new MeasuredData(10, 0.589, 2, SPf.cubic, Uniform.uniform);
                var sp = new SplineParameters(0, -1, 0, 0.07, 0.07);
                var sd = new SplinesData(md, sp);

                sd.Interpolate();
            });
            code.Should().Throw<Exception>();

            code = new Action(() =>
            {
                MeasuredData md = null;
                var sp = new SplineParameters(100, -1, 0, 0.05, 2.97);
                var sd = new SplinesData(md, sp);

                sd.Interpolate();
            });
            code.Should().Throw<Exception>();

            code = new Action(() =>
            {
                var md = new MeasuredData(10, 4, 2, SPf.cubic, Uniform.uniform);
                SplineParameters sp = null;
                var sd = new SplinesData(md, sp);

                sd.Interpolate();
            });
            code.Should().Throw<Exception>();
        }
    }
}
