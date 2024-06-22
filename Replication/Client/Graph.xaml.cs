using LiveCharts;
using LiveCharts.Wpf;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace Client
{
    /// <summary>
    /// Interaction logic for Graph.xaml
    /// </summary>
    /// 
    public class DataEntry
    {
        public string Timestamp { get; set; }
        public string Data { get; set; }
    }

    public partial class Graph : Window
    {
        private List<DataEntry> entries = new List<DataEntry>();
        private double totalSize = 0.0;

        public Graph(string data)
        {
            InitializeComponent();
            InitializeData(data);
            CreateChart();
            this.Title = "Total size of saved data: " + totalSize + " bytes";
        }

        private void InitializeData(string data)
        {
            string[] lines = data.Split(new[] { '\n' }, StringSplitOptions.RemoveEmptyEntries);

            foreach (var line in lines)
            {
                string[] parts = line.Split(new[] { ' ' }, 2, StringSplitOptions.RemoveEmptyEntries);
                if (parts.Length == 2)
                {
                    string _timestamp = parts[0] + " " + parts[1].Substring(0, 8); 
                    string _data = parts[1].Substring(9); 

                    entries.Add(new DataEntry { Timestamp = _timestamp, Data = _data });
                }
            }
        }

        private void CreateChart()
        {
            var chart = new CartesianChart();
            var values = new ChartValues<double>();

            foreach (var entry in entries)
            {
                double size = (double)entry.Data.Length;
                values.Add(size);
                totalSize += size;
            }

            var labels = entries.Select(e => e.Timestamp).ToArray();

            chart.Series.Add(new LineSeries
            {
                Title = "Data Size in bytes",
                Values = values
            });

            chart.AxisX.Add(new Axis
            {
                Title = "Timestamps",
                Labels = labels
            });


            Content = chart;
        }
    }
}
