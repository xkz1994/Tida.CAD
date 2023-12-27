using System.Windows;
using System.Windows.Input;

namespace Tida.CAD.Input
{
    /// <summary>
    /// CADMouseButtonEventArgs
    /// </summary>
    public class CadMouseButtonEventArgs : CadRoutedEventArgs
    {
        public CadMouseButtonEventArgs(Point position)
        {
            Position = position;
        }

        /// <summary>
        /// The mouse position in CAD coordinates
        /// </summary>
        public Point Position { get; }

        /// <summary>
        /// MouseButtonEventArgs in WPF
        /// </summary>
        public MouseButtonEventArgs MouseButtonEventArgs { get; set; }
    }
}