using System;
using ClassLibrary;
using Xunit;

namespace ModelTests
{
    public class MeasuredDataTests
    {
        [Theory]
        [InlineData(5, 0, 1, SPf.linear, Uniform.non_uniform)]
        [InlineData(10, 0.001, 3.94, SPf.cubic, Uniform.uniform)]
        [InlineData(4, -3, -2.073, SPf.random, Uniform.uniform)]
        public void ConstructorTest(int N, double sgstart, double sgend, SPf ftype, Uniform ndtype)
        {
            var md = new MeasuredData(N, sgstart, sgend, ftype, ndtype);
            Assert.Equal(N, md.nodeNum);
            Assert.Equal(sgstart, md.segBounds[0]);
            Assert.Equal(sgend, md.segBounds[1]);
            Assert.Equal(ftype, md.funcType);
            Assert.Equal(ndtype, md.nodetype);
            Assert.NotNull(md.nodes);
            Assert.NotNull(md.measurVals);
        }

        [Fact]
        public void CopyConstructorTest()
        {
            var md = new MeasuredData(10, 0.001, 3.94, SPf.cubic, Uniform.uniform);
            var cmd = new MeasuredData(md);

            Assert.Equal(md.nodeNum, cmd.nodeNum);
            Assert.Equal(md.segBounds[0], cmd.segBounds[0]);
            Assert.Equal(md.segBounds[1], cmd.segBounds[1]);
            Assert.Equal(md.funcType, cmd.funcType);
            Assert.Equal(md.nodetype, cmd.nodetype);
            Assert.Equal(md.step, cmd.step);
            Assert.NotNull(md.nodes);
            Assert.NotNull(md.measurVals);
        }
    }
}
