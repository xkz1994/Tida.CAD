using System.Windows.Input;

namespace Tida.CAD.Input
{
    /// <summary>
    /// CADKeyEventArgs
    /// </summary>
    public class CadKeyEventArgs : CadRoutedEventArgs
    {
        /// <summary>
        /// KeyEventArgs in WPF
        /// </summary>
        public KeyEventArgs KeyEventArgs { get; set; }
    }
}