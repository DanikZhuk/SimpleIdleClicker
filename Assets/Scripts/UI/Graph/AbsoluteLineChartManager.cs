using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using XCharts.Runtime;

namespace UI.Graph
{
    public class AbsoluteLineChartManager : MonoBehaviour
    {
        [SerializeField] private LineChart chart;

        public void Initialize()
        {
            chart.ClearData();
            var serie = chart.GetSerie(0);
            serie.areaStyle.color = Color.white;
            chart.RefreshChart();
        }

        public void UpdateValue(List<float> history)
        {
            chart.ClearData();
            var series = chart.GetSerie(0);
            if (series == null) return;

            if (history.Count > 0)
            {
                series.areaStyle.color =
                    history[^2] > history.Last() ? Color.darkRed : Color.darkGreen;
                series.lineStyle.color = series.areaStyle.color;
            }

            foreach (var f in history) chart.AddData(series.index, f);

            chart.RefreshChart();
        }
    }
}