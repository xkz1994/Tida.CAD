using System;
using System.Collections.Generic;
using System.Windows;
using System.Linq;
using System.Windows.Media;

namespace Tida.CAD.WPF
{
    /// <summary>
    /// A WPF canvas implemented with <see cref="DrawingContext"/>
    /// </summary>
    public class WpfCanvas : ICanvas
    {
        /// <summary>
        /// The converter instance;
        /// </summary>
        public ICadScreenConverter CadScreenConverter { get; }

        /// <summary>
        /// An instance of DrawingContext,the value can be modified in run time;
        /// </summary>
        public DrawingContext DrawingContext { get; internal set; }

        /// <summary>
        /// Create an instance of WPFCanvas;
        /// </summary>
        /// <param name="cadScreenConverter">A converter instance</param>
        public WpfCanvas(ICadScreenConverter cadScreenConverter)
        {
            CadScreenConverter = cadScreenConverter ?? throw new ArgumentNullException(nameof(cadScreenConverter));
        }

        /// <summary>
        /// Validate <see cref="DrawingContext"/> is available at present;
        /// </summary>
        private void ValidateDrawingContext()
        {
            //如若DrawingContext为空,则不可执行动作;
            if (DrawingContext == null) throw new InvalidOperationException($"The {nameof(DrawingContext)} should be set to perform this operation.");
        }

        /// <summary>
        /// Draw a line
        /// </summary>
        public void DrawLine(Pen pen, Point point0, Point point1)
        {
            if (pen == null) return;
            ValidateDrawingContext();

            var screenPoint1 = CadScreenConverter.ToScreen(point0);
            var screenPoint2 = CadScreenConverter.ToScreen(point1);

            //平台转换后再进行绘制;
            DrawingContext.DrawLine(pen, screenPoint1, screenPoint2);
        }

        /// <summary>
        /// Draw an arc
        /// </summary>
        public void DrawArc(Pen pen, Point center, double radius, double beginAngle, double angle, bool smallAngle)
        {
            if (pen == null) return;
            ValidateDrawingContext();

            beginAngle %= (Math.PI * 2);
            angle %= (Math.PI * 2);

            var endAngle = beginAngle + angle;

            var startPoint = new Point(center.X + Math.Cos(beginAngle) * radius, center.Y + Math.Sin(beginAngle) * radius);
            var endPoint = new Point(center.X + Math.Cos(endAngle) * radius, center.Y + Math.Sin(endAngle) * radius);

            var startScreenPoint = CadScreenConverter.ToScreen(startPoint);
            var endScreenPoint = CadScreenConverter.ToScreen(endPoint);

            // 求两边之叉积,由叉积的符号决定是顺时针和逆时针;
            var cross = Math.Cos(beginAngle) * Math.Sin(endAngle) - Math.Sin(beginAngle) * Math.Cos(endAngle);

            var screenRadius = CadScreenConverter.ToScreen(radius);

            //因为数学坐标中，
            var arcGeometry = GetArcGeometry(
                startScreenPoint,
                endScreenPoint,
                screenRadius,
                smallAngle,
                cross < 0 ? SweepDirection.Clockwise : SweepDirection.Counterclockwise
            );

            DrawingContext.DrawGeometry(null, pen, arcGeometry);
        }

        /// <summary>
        /// 绘制(椭)圆
        /// </summary>
        public void DrawEllipse(Brush brush, Pen pen, Point center, double radiusX, double radiusY)
        {
            if (pen == null) return;
            ValidateDrawingContext();

            radiusX = CadScreenConverter.ToScreen(radiusX);
            radiusY = CadScreenConverter.ToScreen(radiusY);
            center = CadScreenConverter.ToScreen(center);
            NativeDrawEllipse(brush, pen, center, radiusX, radiusY);
        }

        /// <summary>
        /// 绘制文字
        /// </summary>
        public void DrawText(FormattedText formattedText, Point origin)
        {
            ValidateDrawingContext();
            var originScreenPoint = CadScreenConverter.ToScreen(origin);
            DrawingContext.DrawText(formattedText, originScreenPoint);
        }

        /// <summary>
        /// 绘制路径(未封闭区域);
        /// </summary>
        public void DrawCurve(Pen pen, IEnumerable<Point> points)
        {
            if (pen == null) return;
            ValidateDrawingContext();

            var path = GetCurveGeometry(points);
            DrawingContext.DrawGeometry(null, pen, path);
        }

        /// <summary>
        /// Draw a rectangle;
        /// </summary>
        /// <param name="brush">The brush to fill the rect</param>
        /// <param name="pen">The pen to decorate the border of the rect</param>
        /// <param name="rect"></param>
        public void DrawRectangle(CadRect rect, Brush brush, Pen pen)
        {
            if (pen == null) return;
            ValidateDrawingContext();

            var topLeftInScreen = CadScreenConverter.ToScreen(rect.TopLeft);
            var widthInScreen = CadScreenConverter.ToScreen(rect.Width);
            var heightInScreen = CadScreenConverter.ToScreen(rect.Height);
            var rectInScreen = new Rect(topLeftInScreen, new Size(widthInScreen, heightInScreen));
            DrawingContext.DrawRectangle(brush, pen, rectInScreen);
        }

        public void DrawPolygon(IEnumerable<Point> points, Brush brush, Pen pen)
        {
            if (pen == null) return;
            ValidateDrawingContext();

            DrawFill(points, brush, pen);
        }

        /// <summary>
        /// Create a <see cref="PathGeometry"/> from an arc;
        /// </summary>
        private static PathGeometry GetArcGeometry(Point startScreenPoint, Point endScreenPoint, double screenRadius, bool smallAngle, SweepDirection sweepDirection)
        {
            /* // 大的半圆: https://www.cnblogs.com/dino623/p/arc_progress_bar.html
             * <Path Stroke="SlateBlue" StrokeThickness="4">
             *     <Path.Data>
             *         <PathGeometry>
             *             <PathFigure IsClosed="False" StartPoint="30,170">
             *                 <ArcSegment IsLargeArc="True"
             *                             Point="170,170"
             *                             Size="96,96"
             *                             SweepDirection="Clockwise" />
             *             </PathFigure>
             *         </PathGeometry>
             *     </Path.Data>
             * </Path>
             */

            // ArcSegment 用于描述 Path 中两点之间的一条椭圆弧
            // Point 终点（起始点在 Path 或前一个 Segment 中描述）
            // Size	X 轴和 Y 轴的半径
            // IsLargeArc 圆弧是整个圆形中大的那部分，还是小的那部分
            // SweepDirection 弧线绘制的方向
            var arcSegment = new ArcSegment(endScreenPoint, new Size(screenRadius, screenRadius), 0D,
                !smallAngle, sweepDirection, true);
            var segments = new PathSegment[] { arcSegment };
            var pathFigure = new PathFigure(startScreenPoint, segments, false);
            var figures = new[] { pathFigure };

            // 固定使当前对象不可修改
            arcSegment.Freeze();
            pathFigure.Freeze();

            return new PathGeometry(figures, FillRule.EvenOdd, null);
        }

        /// <summary>
        /// 以视图坐标为标准,绘制椭圆
        /// </summary>
        public void NativeDrawEllipse(Brush brush, Pen pen, Point center, double radiusX, double radiusY)
        {
            DrawingContext.DrawEllipse(brush, pen, center, radiusX, radiusY);
        }

        /// <summary>
        /// 通过点的集合获取三次贝赛尔曲线
        /// </summary>
        private PathGeometry GetCurveGeometry(IEnumerable<Point> points)
        {
            if (points == null) throw new ArgumentNullException(nameof(points));

            var screenPoints = points.Select(x => CadScreenConverter.ToScreen(x)).ToArray();
            var bezier = new PolyBezierSegment(screenPoints, true);
            var pathFigure = new PathFigure();
            var pathGeometry = new PathGeometry();

            pathFigure.Segments.Add(bezier);
            pathGeometry.Figures.Add(pathFigure);

            if (screenPoints.Length < 1) return pathGeometry;

            pathFigure.StartPoint = screenPoints[0];

            //因为此处使用的三次贝塞尔曲线要求点的数量为3的倍数,所以在未能正处情况下,重复最后一项至下一个三的倍数;
            var repeatCount = (3 - (screenPoints.Length % 3)) % 3;
            var lastScreenPoint = screenPoints[^1];
            for (var i = 0; i < repeatCount; i++)
            {
                bezier.Points.Add(lastScreenPoint);
            }

            return pathGeometry;
        }

        /// <summary>
        /// 根据所有的点，组成一个封闭区域，并且填充
        /// </summary>
        private void DrawFill(IEnumerable<Point> points, Brush brush, Pen pen)
        {
            if (points == null) throw new ArgumentNullException(nameof(points));

            ValidateDrawingContext();

            NativeDrawFill(
                points.Select(p => CadScreenConverter.ToScreen(p)),
                brush,
                pen
            );
        }

        /// <summary>
        /// 直接根据视图位置,绘制WPF封闭区域;
        /// </summary>
        /// <param name="screenPoints"></param>
        /// <param name="brush"></param>
        /// <param name="pen"></param>
        private void NativeDrawFill(IEnumerable<Point> screenPoints, Brush brush, Pen pen)
        {
            if (screenPoints == null) throw new ArgumentNullException(nameof(screenPoints));

            if (pen == null) return;
            ValidateDrawingContext();

            //操作PathGeometry中的Figures以绘制(封闭)区域
            var paths = new PathGeometry();

            var pfc = new PathFigureCollection();
            var pf = new PathFigure();
            pfc.Add(pf);

            //存储一个点表示当前的PathFigure的StartPoint是否被指定;
            var startPointSet = false;

            foreach (var p in screenPoints)
            {
                //若StartPoint未被设定(第一个节点),设定后继续下一次循环;
                if (!startPointSet)
                {
                    pf.StartPoint = p;
                    startPointSet = true;
                    continue;
                }

                //若若StartPoint被设定,则加入线段;
                var ps = new LineSegment { Point = p };
                pf.Segments.Add(ps);
            }

            pf.IsClosed = true;
            paths.Figures = pfc;
            DrawingContext.DrawGeometry(brush, pen, paths);
        }
    }
}