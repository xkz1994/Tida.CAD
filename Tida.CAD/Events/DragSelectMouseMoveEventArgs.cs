using System.Windows;

namespace Tida.CAD.Events
{
    /// <summary>
    /// The mouse move events args when drag selecting
    /// </summary>
    public class DragSelectMouseMoveEventArgs : RoutedEventArgs
    {
        public DragSelectMouseMoveEventArgs(CadRect rect, Point position)
        {
            Rect = rect;
            Position = position;
        }

        /// <summary>
        /// The rect that contains the dragging select area
        /// </summary>
        public CadRect Rect { get; }

        /// <summary>
        /// Whether anypoint select 是否选择任何点
        /// </summary>
        public bool? IsAnyPoint { get; set; }

        /// <summary>
        /// The position of mouse
        /// </summary>
        public Point Position { get; }
    }
}