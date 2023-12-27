using System;
using System.Collections.Generic;
using System.Windows;

namespace Tida.CAD
{
    /// <summary>
    /// 描述矩形的宽度、高度和位置（左下点）
    /// </summary>
    /// <remarks>
    /// 请注意，y 轴是向前的，这在大多数渲染坐标系中是相反的，此处将使用的坐标系名为“CAD 坐标”
    /// </remarks>
    public struct CadRect
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="CadRect"/> structure that has the
        ///     specified bottom-left corner location and the specified width and height.
        /// </summary>
        /// <param name="location">A point that specifies the location of the bottom-left corner of the rectangle.</param>
        /// <param name="size">A System.Windows.Size structure that specifies the width and height of the rectangle.</param>
        public CadRect(Point location, Size size)
        {
            X = location.X;
            Y = location.Y;
            Width = size.Width;
            Height = size.Height;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="CadRect"/> structure that has the
        ///     specified x-coordinate, y-coordinate, width, and height.
        /// </summary>
        /// <param name="x"> The x-coordinate of the bottom-left corner of the rectangle.</param>
        /// <param name="y"> The y-coordinate of the bottom-left corner of the rectangle.</param>
        /// <param name="width">The width of the rectangle.</param>
        /// <param name="height">The height of the rectangle.</param>
        public CadRect(double x, double y, double width, double height)
        {
            X = x;
            Y = y;
            Width = width;
            Height = height;
        }

        /// <summary>
        /// Constructor which sets the initial values to bound the two points provided.
        /// </summary>
        public CadRect(Point point1, Point point2)
        {
            X = Math.Min(point1.X, point2.X);
            Y = Math.Min(point1.Y, point2.Y);

            //  Max with 0 to prevent double weirdness from causing us to be (-epsilon..0)
            Width = Math.Max(Math.Max(point1.X, point2.X) - X, 0);
            Height = Math.Max(Math.Max(point1.Y, point2.Y) - Y, 0);
        }

        /// <summary>
        /// Gets or sets the x-axis value of the left side of the rectangle.
        /// </summary>
        public double X { get; set; }

        /// <summary>
        /// Gets or sets the y-axis value of the bottom side of the rectangle.
        /// </summary>
        public double Y { get; set; }

        /// <summary>
        /// Gets or sets the width of the rectangle.
        /// </summary>
        public double Width { get; set; }

        /// <summary>
        /// Gets or sets the height of the rectangle.
        /// </summary>
        public double Height { get; set; }

        /// <summary>
        /// Get the center point of the rectangle;
        /// </summary>
        public Point Center => new Point(X + Width / 2, Y + Height / 2);

        /// <summary>
        /// Get the top-left point of the rectangle;
        /// </summary>
        public Point TopLeft => new Point(X, Y + Height);

        /// <summary>
        /// Get the bottom-left point of the rectangle;
        /// </summary>
        public Point BottomLeft => new Point(X, Y);

        /// <summary>
        /// Get the bottom-right point of the rectangle;
        /// </summary>
        public Point BottomRight => new Point(X + Width, Y);

        /// <summary>
        /// Get the top-right point of the rectangle;
        /// </summary>
        public Point TopRight => new Point(X + Width, Y + Height);

        public bool Contains(Point point)
        {
            return (point.X - X) >= 0 && (point.X - X) <= Width && (point.Y - Y) >= 0 && (point.Y - Y) <= Height;
        }

        /// <summary>
        /// Get all the vertex points of the rect(顺时针);
        /// </summary>
        public readonly IEnumerable<Point> GetVertexes()
        {
            yield return BottomLeft;
            yield return BottomRight;
            yield return TopRight;
            yield return TopLeft;
        }

        /// <summary>
        /// Get all the borders of the rect(顺时针)
        /// </summary>
        /// <returns></returns>
        public readonly IEnumerable<CadLine> GetBorders()
        {
            yield return new CadLine(BottomLeft, BottomRight);
            yield return new CadLine(BottomRight, TopRight);
            yield return new CadLine(TopRight, TopLeft);
            yield return new CadLine(TopLeft, BottomLeft);
        }
    }
}