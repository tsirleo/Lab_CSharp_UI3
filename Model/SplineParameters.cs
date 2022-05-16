using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClassLibrary
{
    public class SplineParameters
    {
        public int nodeNum { get; set; }
        public double[] integBounds { get; set; } = new double[2] { 0.0, 0.0 };
        public double[] derivBounds { get; set; } = new double[2] { 0.0, 0.0 };

        public SplineParameters() { }

        public SplineParameters(int N, double derstart, double derend, double intstart, double intend)
        {
            nodeNum = N;
            derivBounds[0] = derstart;
            derivBounds[1] = derend;
            integBounds[0] = intstart;
            integBounds[1] = intend;
        }

        public SplineParameters(SplineParameters sp)
        {
            nodeNum = sp.nodeNum;
            integBounds = sp.integBounds;
            derivBounds = sp.derivBounds;
        }
    }
}