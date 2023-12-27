using System;
using System.Linq;
using System.Windows;
using System.Windows.Media;

namespace Tida.CAD.DrawObjects
{
    /// <summary>
    /// DrawObject——Rectangle;
    /// </summary>
    public class Rectangle : DrawObject
    {
        static Rectangle()
        {
            DefaultSelectionPen = new Pen
            {
                Brush = Brushes.Blue,
                Thickness = 3
            };
            DefaultSelectionPen.Freeze();
        }

        private static readonly Pen DefaultSelectionPen;

        private Pen _pen;

        /// <summary>
        /// The pen used for borders;
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

        private Pen _selectionPen = DefaultSelectionPen;

        /// <summary>
        /// The pen used for borders when selected;
        /// </summary>
        public Pen SelectionPen
        {
            get => _selectionPen;
            set
            {
                _selectionPen = value;
                RaiseVisualChanged();
            }
        }

        private Brush _background;

        /// <summary>
        /// The background brush;
        /// </summary>
        public Brush Background
        {
            get => _background;
            set
            {
                _background = value;
                RaiseVisualChanged();
            }
        }

        private CadRect _rectangle2D;

        public CadRect Rectangle2D
        {
            get => _rectangle2D;
            set
            {
                _rectangle2D = value;
                RaiseVisualChanged();
            }
        }

        public Rectangle(CadRect rect)
        {
            Rectangle2D = rect;
        }

        public override CadRect? GetBoundingRect()
        {
            return Rectangle2D;
        }

        public override bool ObjectInRectangle(CadRect rect, ICadScreenConverter cadScreenConverter, bool anyPoint)
        {
            if (anyPoint)
                return Rectangle2D.GetVertexes()?.Any(p => rect.Contains(p)) ?? false;

            return Rectangle2D.GetVertexes()?.All(p => rect.Contains(p)) ?? false;
        }

        public override bool PointInObject(Point point, ICadScreenConverter cadScreenConverter)
        {
            return Rectangle2D.Contains(point);
        }

        public override void Draw(ICanvas canvas)
        {
            if (canvas == null)
            {
                throw new ArgumentNullException(nameof(canvas));
            }

            canvas.DrawRectangle(Rectangle2D, Background, IsSelected ? SelectionPen : Pen);

            base.Draw(canvas);
        }
    }
}