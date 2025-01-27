﻿using System;
using System.Linq;
using System.Windows;
using System.Windows.Media;
using Tida.CAD.Extensions;

namespace Tida.CAD.DrawObjects
{
    /// <summary>
    /// The base draw object of lines;
    /// </summary>
    public abstract class LineBase : DrawObject
    {
        private Point _start;

        public Point Start
        {
            get => _start;
            set
            {
                _start = value;
                RaiseVisualChanged();
            }
        }

        private Point _end;

        public Point End
        {
            get => _end;
            set
            {
                _end = value;
                RaiseVisualChanged();
            }
        }

        private Pen _pen;

        /// <summary>
        /// The pen used when drawing the line;
        /// </summary>
        public Pen Pen
        {
            get => _pen;
            set
            {
                _pen = value;
                RaiseVisualChanged();
            }
        }

        public override CadRect? GetBoundingRect()
        {
            var bottomLeft = new Point(Math.Min(Start.X, End.X), Math.Min(Start.Y, End.Y));
            var width = Math.Abs(Start.X - End.X);
            var height = Math.Abs(Start.Y - End.Y);
            return new CadRect(bottomLeft, new Size(width, height));
        }

        public override bool PointInObject(Point point, ICadScreenConverter cadScreenConverter)
        {
            return Pen != null && base.PointInObject(point, cadScreenConverter);
        }

        public override bool ObjectInRectangle(CadRect rect, ICadScreenConverter cadScreenConverter, bool anyPoint)
        {
            // if both two points in inside the rect,then return true;
            if (rect.Contains(Start) && rect.Contains(End)) return true;

            // 或者，如果任何一点为真，则检查该线是否与矩形的任何边界相交
            if (anyPoint) return rect.GetBorders()?.Any(p => GeometryExtensions.GetIntersectPoint(p.Start, p.End, Start, End) != null) ?? false;

            return false;
        }

        public override void Draw(ICanvas canvas)
        {
            // Draw main line;
            canvas.DrawLine(Pen, Start, End);
        }
    }
}