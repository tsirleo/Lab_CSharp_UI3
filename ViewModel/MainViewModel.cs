using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Windows.Input;
using ClassLibrary;
using OxyPlot;
using OxyPlot.Legends;
using OxyPlot.Series;

namespace ViewModel
{
    public enum VMftype
    {
        linear,
        cubic,
        random
    }

    public enum VMuniform
    {
        uniform,
        non_uniform
    }

    public interface IErrorReporter
    {
        void ReportError(string message);
    }

    public class MainViewModel : ViewModelBase, IDataErrorInfo
    {
        public VMftype vmftype { get; set; } = VMftype.linear;
        public VMuniform vmuniform { get; set; } = VMuniform.uniform;

        public int ndNum { get; set; } = 8;
        public double sgstart { get; set; } = 0.0;
        public double sgend { get; set; } = 1.0;
        private SPf ftype { get; set; }

        private Uniform ndtype { get; set; }
        public int spndNum { get; set; } = 100;
        public double derstart { get; set; } = 1.0;
        public double derend { get; set; } = 1.0;
        public double intstart { get; set; } = 0.0;
        public double intend { get; set; } = 1.0;

        public MeasuredData md { get; set; }
        public SplineParameters sp { get; set; }
        public SplinesData sd { get; set; }
        public PlotModel plotModel { get; set; }

        public ObservableCollection<string> mdList { get; set; } = new ObservableCollection<string>();
        public ObservableCollection<string> splnList { get; set; } = new ObservableCollection<string>();

        public ICommand ApplyDataCommand { get; private set; }
        public ICommand SplineCommand { get; private set; }

        private readonly IErrorReporter errorReporter;

        public string this[string columnName]
        {
            get
            {
                string error = string.Empty;

                switch (columnName)
                {
                    case nameof(ndNum):
                        if (ndNum <= 2)
                            error = "Node number must be greater or equal 2";
                        break;
                    case nameof(sgstart):
                        if (sgstart >= sgend)
                        {
                            error = "Beginning of segment must be less than end";
                        }
                        break;
                    case nameof(sgend):
                        if (sgend <= sgstart)
                        {
                            error = "End of segment must be greater than beginning";
                        }
                        break;
                    case nameof(spndNum):
                        if (spndNum <= 2)
                        {
                            error = "Spline nodes number must be greater or equal 2";
                        }
                        break;
                    case nameof(intstart):
                        if (intstart < sgstart || intstart > sgend || intstart >= intend)
                        {
                            error = "Beginning of segment must be less than end, and don't go beyond the original segment";
                        }
                        break;
                    case nameof(intend):
                        if (intend < sgstart || intend > sgend || intend <= intstart)
                        {
                            error = "End of segment must be greater than beginning, and don't go beyond the original segment";
                        }
                        break;
                }

                return error;
            }
        }

        public string Error { get; }

        public MainViewModel(IErrorReporter errorReporter)
        {
            this.errorReporter = errorReporter;

            //ApplyDataCommand = new RelayCommand(_ => { OnApplyData(); });
            //SplineCommand = new RelayCommand(_ => { OnSpline(); });

            ApplyDataCommand = new RelayCommand(_ => { OnApplyData(); },
                _ =>
                {
                    return this[nameof(ndNum)] == String.Empty && this[nameof(sgstart)] == String.Empty && this[nameof(sgend)] == String.Empty;
                });

            SplineCommand = new RelayCommand(_ => { OnSpline(); },
                _ =>
                {
                    return md != null && this[nameof(spndNum)] == String.Empty &&
                           this[nameof(intstart)] == String.Empty && this[nameof(intend)] == String.Empty;
                });
        }

        private void OnApplyData()
        {
            try
            {
                mdList.Clear();
                splnList.Clear();
                switch (vmftype)
                {
                    case VMftype.linear:
                        ftype = SPf.linear;
                        break;
                    case VMftype.cubic:
                        ftype = SPf.cubic;
                        break;
                    case VMftype.random:
                        ftype = SPf.random;
                        break;
                }

                switch (vmuniform)
                {
                    case VMuniform.uniform:
                        ndtype = Uniform.uniform;
                        break;
                    case VMuniform.non_uniform:
                        ndtype = Uniform.non_uniform;
                        break;
                }
                ApplyMeasureData();
                DrawDots();
                RaisePropertyChanged(nameof(mdList));
                RaisePropertyChanged(nameof(splnList));
                RaisePropertyChanged(nameof(plotModel));
            }
            catch (Exception e)
            {
                errorReporter.ReportError($"An unexpected error has occurred: {e.Message}");
            }
        }

        private void OnSpline()
        {
            try
            {
                splnList.Clear();
                ApplySplineData();
                DrawSpline();
                RaisePropertyChanged(nameof(splnList));
                RaisePropertyChanged(nameof(plotModel));
            }
            catch (Exception e)
            {
                errorReporter.ReportError($"An unexpected error has occurred: {e.Message}");
            }
        }
        
        private void ApplyMeasureData()
        {
            md = new MeasuredData(ndNum, sgstart, sgend, ftype, ndtype);
            if (md != null)
            {
                for (int i = 0; i < ndNum; i++)
                {
                    mdList.Add($"{i + 1}) x = {md.nodes[i].ToString("F4")}   y = {md.measurVals[i].ToString("F4")}");
                }
            }
        }

        private void ApplySplineData()
        {
            if (md == null)
            {
                errorReporter.ReportError("Apply the grid data");
                //System.Windows.MessageBox.Show($"Apply the grid data", "Warning message", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
            else
            {
                sp = new SplineParameters(spndNum, derstart, derend, intstart, intend);
                sd = new SplinesData(md, sp);
                try
                {
                    sd.Interpolate();
                    splnList.Add($"left bound derivative = {sd.derives[1].ToString("F")}");
                    splnList.Add($"right bound derivative = {sd.derives[3].ToString("F")}");
                    splnList.Add($"integral value = {sd.integral.ToString("F3")}");
                    for (int i = 0; i < sp.nodeNum; i++)
                    {
                        splnList.Add($"{i + 1}) x = {sd.nodes[i].ToString("F4")}   y = {sd.splineVals[i].ToString("F4")}");
                    }
                }
                catch (Exception e)
                {
                    errorReporter.ReportError("An error occurred during interpolation and integration");
                    //System.Windows.MessageBox.Show($"An error occurred during interpolation and integration", "Error message", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
        }

        public void DrawDots()
        {
            try {
                if (md != null)
                {
                    this.plotModel = new PlotModel {Title = "Measured Data"};
                    this.plotModel.Series.Clear();
                    OxyColor color = OxyColors.DeepPink;
                    LineSeries lineSeries = new LineSeries();
                    for (int i = 0; i < md.nodeNum; i++)
                    {
                        lineSeries.Points.Add(new DataPoint(md.nodes[i], md.measurVals[i]));
                    }

                    lineSeries.Title = "Reference points";
                    lineSeries.Color = color;
                    lineSeries.LineStyle = LineStyle.None;
                    lineSeries.MarkerType = MarkerType.Circle;
                    lineSeries.MarkerSize = 4;
                    lineSeries.MarkerStroke = color;
                    lineSeries.MarkerFill = color;

                    Legend leg = new Legend();
                    this.plotModel.Legends.Add(leg);
                    this.plotModel.Series.Add(lineSeries);
                }
            }
            catch (Exception e)
            {
                errorReporter.ReportError("An error occurred when drawing points");
            }
        }

        public void DrawSpline()
        {
            try
            {
                this.plotModel = new PlotModel {Title = "Measured Data and Spline"};
                this.DrawDots();
                if (sd != null)
                {
                    this.plotModel.Series.Clear();
                    OxyColor color = OxyColors.DeepPink;
                    LineSeries lineSeries = new LineSeries();
                    for (int i = 0; i < sd.mesData.nodeNum; i++)
                    {
                        lineSeries.Points.Add(new DataPoint(sd.mesData.nodes[i], sd.mesData.measurVals[i]));
                    }

                    lineSeries.Title = "Reference points";
                    lineSeries.Color = color;
                    lineSeries.LineStyle = LineStyle.None;
                    lineSeries.MarkerType = MarkerType.Circle;
                    lineSeries.MarkerSize = 4;
                    lineSeries.MarkerStroke = color;
                    lineSeries.MarkerFill = color;

                    Legend leg = new Legend();
                    this.plotModel.Legends.Add(leg);
                    this.plotModel.Series.Add(lineSeries);

                    color = OxyColors.Aqua;
                    lineSeries = new LineSeries();
                    for (int i = 0; i < sd.nodes.Length; i++)
                    {
                        lineSeries.Points.Add(new DataPoint(sd.nodes[i], sd.splineVals[i]));
                    }

                    lineSeries.Title = "Spline";
                    lineSeries.Color = color;
                    lineSeries.MarkerSize = 4;

                    this.plotModel.Series.Add(lineSeries);
                }
            }
            catch (Exception e)
            {
                errorReporter.ReportError("An error occurred when drawing spline");
            }
        }
    }
}
