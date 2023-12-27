using System;
using System.Windows;

namespace Tida.CAD.WPF
{
    /// <summary>
    /// <see cref="ICadScreenConverter"/>的一个实现
    /// </summary>
    public class WpfCadScreenConverter : ICadScreenConverter
    {
        /// <summary>
        /// Zoom为1.0时,工程数学单位和视图单位的比率;
        /// </summary>
        public const double ScreenResolution = 96;

        /// <summary>
        /// 默认缩放比例
        /// </summary>
        public const double DefaultZoom = 1;

        /// <summary>
        /// 当前缩放比例
        /// </summary>
        private double _zoom = DefaultZoom;

        public double Zoom
        {
            get => _zoom;
            set
            {
                if (value <= 0) throw new ArgumentException($"{nameof(Zoom)} should be larger than zero.");

                _zoom = value;
            }
        }

        /// <summary>
        /// 原点所在视图位置;
        /// </summary>
        public Point PanScreenPosition { get; set; }

        /// <summary>
        /// 实际视图宽度
        /// </summary>
        public double ActualWidth { get; set; }

        /// <summary>
        /// 实际视图高度
        /// </summary>
        public double ActualHeight { get; set; }

        public double ToScreen(double unitValue)
        {
            return unitValue * Zoom * ScreenResolution;
        }

        public Point ToScreen(Point unitPoint)
        {
            var screenX = ToScreen(unitPoint.X);
            var screenY = ToScreen(unitPoint.Y);

            return new Point(screenX + PanScreenPosition.X, -screenY + PanScreenPosition.Y);
        }

        public Point ToCad(Point screenPoint)
        {
            var unitX = ToCad(screenPoint.X - PanScreenPosition.X);
            var unitY = ToCad(-screenPoint.Y + PanScreenPosition.Y);

            return new Point(unitX, unitY);
        }

        public double ToCad(double screenValue)
        {
            return screenValue / (ScreenResolution * Zoom);
        }
    }
}