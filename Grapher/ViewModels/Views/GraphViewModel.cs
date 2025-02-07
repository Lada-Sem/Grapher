using Grapher.Common;
using LiveChartsCore;
using LiveChartsCore.Defaults;
using LiveChartsCore.SkiaSharpView;
using LiveChartsCore.SkiaSharpView.Painting.Effects;
using LiveChartsCore.SkiaSharpView.Painting;
using SkiaSharp;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Grapher.Utils;
using System.Windows.Controls;
using Grapher.Models;

namespace Grapher.ViewModels.Views
{
    public class GraphViewModel : ViewModelBase
    {
        private string _functionText;
        public string FunctionText
        {
            get => _functionText;
            set => Set(value, ref _functionText);
        }
        public ObservableCollection<ISeries> Series { get; set; }
        public Axis[] XAxes { get; set; }
            = new Axis[]
            {
                new Axis
                {
                    Name = "X Axis",
                    NamePaint = new SolidColorPaint(SKColors.White),

                    LabelsPaint = new SolidColorPaint(SKColors.White),
                    TextSize = 10,

                    SeparatorsPaint = new SolidColorPaint(SKColors.White)
                    {
                        StrokeThickness = 2,
                        PathEffect = new DashEffect(new float[] { 3, 3 })
                    }
                }
            };

        public Axis[] YAxes { get; set; }
            = new Axis[]
            {
                new Axis
                {
                    Name = "Y Axis",
                    NamePaint = new SolidColorPaint(SKColors.White),

                    LabelsPaint = new SolidColorPaint(SKColors.White),
                    TextSize = 20,

                    SeparatorsPaint = new SolidColorPaint(SKColors.White)
                    {
                        StrokeThickness = 2,
                        PathEffect = new DashEffect(new float[] { 3, 3 })
                    }
                }
            };

        public RelayCommand InvokeCommand { get; init; }
        public RelayCommand RestoreCommand { get; set; }
        public RelayCommand UpdateGraphCommand { get; set; }
        public RelayCommand<Button> PrintCommand { get; set; }
        public GraphOptions Options { get; init; }

        public GraphViewModel()
        {
            Options = new();
            Series = new ObservableCollection<ISeries>();
            InvokeCommand = new RelayCommand(InvokeFunction);
            RestoreCommand = new RelayCommand(RestoreFunction);
            PrintCommand = new(PrintFunction);
            UpdateGraphCommand = new(UpdateGraph);
            InitXY();
        }

        private void UpdateGraph()
        {
            RestoreFunction();
            InvokeFunction();
        }

        private void PrintFunction(Button button)
        {
            FunctionText += (string)button.Tag;
        }

        private void RestoreFunction()
        {
            Series.Clear();
            InitXY();
        }

        private void InvokeFunction()
        {
            if (!string.IsNullOrWhiteSpace(FunctionText))
            {
                Series.Clear();
                InitXY();
                if (Expression.IsExpression(FunctionText, new string[] { "x" }))
                {
                    var exp = new Expression(FunctionText, new string[] { "x" }, null);
                    var obs = new List<ObservablePoint>();
                    for (float i = Options.MinX; i < Options.MaxX; i = i + Options.Points)
                    {
                        var yResult = exp.CalculateValue([i]);
                        obs.Add(new ObservablePoint(i, yResult));
                    }
                    Series.Add(new LineSeries<ObservablePoint>
                    {
                        Values = obs,
                        Tag = "X",
                        Name = FunctionText,
                    });
                }
            }
        }

        private void InitXY()
        {
            Series.Add(new LineSeries<ObservablePoint>
            {
                Values = [new ObservablePoint(Options.MinX, 0), new ObservablePoint(Options.MaxX,0)],
                Fill = new SolidColorPaint(SKColors.Blue),
                GeometryFill = new SolidColorPaint(SKColors.Blue),
                Stroke = new SolidColorPaint(SKColors.Blue),
                Tag = "X",
                Name = "X",
            });
            Series.Add(new LineSeries<ObservablePoint>
            {
                Values = [new ObservablePoint(0, Options.MinY), new ObservablePoint(0, Options.MaxY)],
                Fill = new SolidColorPaint(SKColors.Red),
                GeometryFill = new SolidColorPaint(SKColors.Red),
                Stroke = new SolidColorPaint(SKColors.Red),
                Tag = "Y",
                Name = "Y",
            });

        }



    }
}
