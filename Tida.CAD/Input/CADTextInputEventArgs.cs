using System.Windows.Input;

namespace Tida.CAD.Input
{
    /// <summary>
    /// CADTextInputEventArgs
    /// </summary>
    public class CadTextInputEventArgs : CadRoutedEventArgs
    {
        /// <summary>
        /// TextCompositionEventArgs in WPF
        /// </summary>
        public TextCompositionEventArgs TextCompositionEventArgs { get; set; }
    }
}