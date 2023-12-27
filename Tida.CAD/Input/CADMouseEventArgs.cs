using System.Windows;
using System.Windows.Input;

namespace Tida.CAD.Input
{
    /// <summary>
    /// CADMouseEventArgs;
    /// </summary>
    public class CadMouseEventArgs : CadRoutedEventArgs
    {
        public CadMouseEventArgs(Point position)
        {
            Position = position;
        }

        /// <summary>
        /// The mouse position in CAD coordinates
        /// </summary>
        public Point Position { get; }

        /// <summary>
        /// The native mouse event args in WPF
        /// </summary>
        public MouseEventArgs MouseEventArgs { get; set; }
    }
}