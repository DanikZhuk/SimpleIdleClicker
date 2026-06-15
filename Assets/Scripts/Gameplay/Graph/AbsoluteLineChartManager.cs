using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using XCharts.Runtime;

namespace DefaultNamespace
{
    public class AbsoluteLineChartManager : MonoBehaviour
    {
        [SerializeField] private LineChart chart;

        private bool _isChartReady;

        private void Start()
        {
            InitializeChart();
        }

        public void UpdateValue(List<float> history)
        {
            chart.ClearData();
            if (!_isChartReady) return;
            var serie = chart.GetSerie(0);
            if (serie == null) return;

            if (history.Count > 0)
            {
                serie.areaStyle.color =
                    history[^2] < history.Last() ? Color.darkRed : Color.darkGreen;
                serie.lineStyle.color = serie.areaStyle.color;
            }

            foreach (var f in history)
            {
                chart.AddData(serie.index, f);
            }

            chart.RefreshChart();
        }

        private void InitializeChart()
        {
            chart.ClearData();
            var serie = chart.GetSerie(0);
            serie.areaStyle.color = Color.white;
            chart.RefreshChart();
            _isChartReady = true;
        }
    }
}