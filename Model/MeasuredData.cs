using System;

namespace ClassLibrary
{
    public class MeasuredData
    {
        public int nodeNum { get; set; }
        public double[] segBounds { get; set; } = new double[2] { 0.0, 0.0 };
        public SPf funcType { get; set; }
        public double[] nodes { get; private set; }
        public double[] measurVals { get; set; }
        public double step { get; set; }
        public Uniform nodetype { get; set; }

        public MeasuredData(int N, double sgstart, double sgend, SPf ftype, Uniform ndtype)
        {
            nodeNum = N;
            nodes = new double[N];
            measurVals = new double[N];
            segBounds[0] = sgstart;
            segBounds[1] = sgend;
            funcType = ftype;
            nodetype = ndtype;
            step = (sgend - sgstart) / (N - 1);
            nodes[0] = sgstart;

            switch (nodetype)
            {
                case Uniform.uniform:
                    for (int i = 1; i < N; i++)
                    {
                        nodes[i] = nodes[i - 1] + step;
                    }
                    break;
                case Uniform.non_uniform:
                    Random rnd = new Random();
                    for (int i = 1; i < N - 1; i++)
                    {
                        nodes[i] = rnd.NextDouble() * (sgend - sgstart) + sgstart;
                    }

                    nodes[N - 1] = sgend;
                    Array.Sort(nodes);
                    break;
            }

            switch (ftype)
            {
                case SPf.linear:
                    makeLinear();
                    break;
                case SPf.cubic:
                    makeCubic();
                    break;
                case SPf.random:
                    makeRandom();
                    break;
            }
        }

        public MeasuredData(MeasuredData md)
        {
            nodeNum = md.nodeNum;
            segBounds = md.segBounds;
            funcType = md.funcType;
            nodetype = md.nodetype;
            step = md.step;
            nodes = new double[md.nodes.Length];
            nodes = md.nodes;
            measurVals = new double[md.measurVals.Length];
            measurVals = md.measurVals;
        }

        public void makeLinear()
        {
            for (int i = 0; i < nodeNum; i++)
            {
                measurVals[i] = (1.0 / 50) * nodes[i] + 0.5;
            }
        }

        public void makeCubic()
        {
            for (int i = 0; i < nodeNum; i++)
            {
                measurVals[i] = 0.031 * (nodes[i] - 1) * (nodes[i] + 7) * (nodes[i] + 2);
            }
        }

        public void makeRandom()
        {
            Random rnd = new Random();
            for (int i = 0; i < nodeNum; i++)
            {
                measurVals[i] = rnd.NextDouble() * 10;
            }
        }
    }
}
