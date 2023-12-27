using System.Collections.Generic;
using System.Windows;
using System.Windows.Media;

namespace Tida.CAD
{
    /// <summary>
    /// ICanvas 接口描述了单位坐标系中的绘制方法，
    /// </summary>
    /// <remarks>
    /// 请注意，y 轴是向前的，这在大多数渲染坐标系中是相反的，此处将使用的坐标系名为“CAD 坐标”
    /// </remarks>
    public interface ICanvas
    {
        /// <summary>
        /// 屏幕坐标和CAD坐标之间的转换器
        /// </summary>
        ICadScreenConverter CadScreenConverter { get; }

        /// <summary>
        /// 绘画上下文，类似于 GDI 的 Graphics
        /// </summary>
        DrawingContext DrawingContext { get; }

        /// <summary>
        /// Draw a Line
        /// </summary>
        void DrawLine(Pen pen, Point point0, Point point1);

        /// <summary>
        /// Draw an arc
        /// </summary>
        void DrawArc(Pen pen, Point center, double radius, double beginAngle, double angle, bool smallAngle);

        /// <summary>
        /// Draw a Polygon
        /// </summary>
        void DrawPolygon(IEnumerable<Point> points, Brush brush, Pen pen);

        /// <summary>
        /// Draw an ellipse
        /// </summary>
        void DrawEllipse(Brush brush, Pen pen, Point center, double radiusX, double radiusY);

        /// <summary>
        /// Draw text
        /// </summary>
        void DrawText(FormattedText formattedText, Point origin);

        /// <summary>
        /// Draw a path
        /// </summary>
        void DrawCurve(Pen pen, IEnumerable<Point> points);

        /// <summary>
        /// Draw a rectangle
        /// </summary>
        void DrawRectangle(CadRect cadRect, Brush brush, Pen pen);
    }
}