using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace ClassLibrary
{
    public class SplinesData
    {
        public MeasuredData mesData { get; set; }
        public SplineParameters splnParam { get; set; }
        public double[] splineVals { get; set; }
        public double integral { get; set; }
        public double[] nodes { get; private set; }
        public double[] derives { get; set; }

        public SplinesData(MeasuredData md, SplineParameters sp)
        {
            mesData = new MeasuredData(md);
            splnParam = new SplineParameters(sp);
        }

        public void Interpolate()
        {
            try
            {
                switch (mesData.nodetype)
                {
                    case Uniform.uniform:
                        int ret_u = 0;
                        double[] scoeff_u = new double[4 * (mesData.nodeNum - 1)];
                        int[] dorder_u = new int[1] { 1 };
                        double[] left_u = new double[1] { splnParam.integBounds[0] };
                        double[] right_u = new double[1] { splnParam.integBounds[1] };
                        double[] integmas_u = new double[1];
                        derives = new double[4];
                        splineVals = new double[splnParam.nodeNum];
                        nodes = new double[splnParam.nodeNum];
                        double hs = (mesData.segBounds[1] - mesData.segBounds[0]) / (splnParam.nodeNum - 1);
                        nodes[0] = mesData.segBounds[0];
                        for (int i = 1; i < splnParam.nodeNum; i++)
                        {
                            nodes[i] = nodes[i - 1] + hs;
                        }
                        InterpolateUni(mesData.nodeNum, 1, mesData.segBounds, mesData.measurVals, mesData.segBounds, splnParam.derivBounds,
                            scoeff_u, splnParam.nodeNum, mesData.segBounds, 1, dorder_u, splineVals, derives, left_u, right_u, integmas_u, ref ret_u);
                        if (ret_u == -1)
                        {
                            throw new Exception("Error in Interpolation uniform data");
                        }

                        integral = integmas_u[0];
                        break;
                    case Uniform.non_uniform:
                        int ret_nu = 0;
                        double[] scoeff_nu = new double[4 * (mesData.nodeNum - 1)];
                        int[] dorder_nu = new int[1] { 1 };
                        double[] left_nu = new double[1] { splnParam.integBounds[0] };
                        double[] right_nu = new double[1] { splnParam.integBounds[1] };
                        double[] integmas_nu = new double[1];
                        derives = new double[4];
                        splineVals = new double[splnParam.nodeNum];
                        nodes = new double[splnParam.nodeNum];
                        double hsn = (mesData.segBounds[1] - mesData.segBounds[0]) / (splnParam.nodeNum - 1);
                        nodes[0] = mesData.segBounds[0];
                        for (int i = 1; i < splnParam.nodeNum; i++)
                        {
                            nodes[i] = nodes[i - 1] + hsn;
                        }
                        InterpolateNonUni(mesData.nodeNum, 1, mesData.nodes, mesData.measurVals, mesData.segBounds, splnParam.derivBounds,
                            scoeff_nu, splnParam.nodeNum, mesData.segBounds, 1, dorder_nu, splineVals, derives, left_nu, right_nu, integmas_nu, ref ret_nu);
                        if (ret_nu == -1)
                        {
                            throw new Exception("Error in Interpolation non-uniform data");
                        }

                        integral = integmas_nu[0];
                        break;
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Error in Interpolation");
            }

        }
        [DllImport("..\\..\\..\\..\\x64\\DEBUG\\CPP_DLL.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern void InterpolateUni(int nx, int ny, double[] x, double[] y, double[] bounds, double[] derivBounds, double[] scoeff, int nsite, double[] site, int ndorder, int[] dorder, double[] result, double[] derives, double[] left, double[] right, double[] integres, ref int ret);
        [DllImport("..\\..\\..\\..\\x64\\DEBUG\\CPP_DLL.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern void InterpolateNonUni(int nx, int ny, double[] x, double[] y, double[] bounds, double[] derivBounds, double[] scoeff, int nsite, double[] site, int ndorder, int[] dorder, double[] result, double[] derives, double[] left, double[] right, double[] integres, ref int ret);
    }
}
