using System;

namespace Tida.CAD
{
    /// <summary>
    /// The base class of things like Layer and draw objects
    /// </summary>
    public abstract class CadElement : IDrawable
    {
        /// <summary>
        /// Occurs when is visibly changed;
        /// </summary>
        public event EventHandler IsVisibleChanged;

        /// <summary>
        /// Occurs when the visual content changed;
        /// </summary>
        public event EventHandler VisualChanged;

        private bool _isVisible = true;

        /// <summary>
        /// IsVisible;
        /// </summary>
        public bool IsVisible
        {
            get => _isVisible;
            set
            {
                if (_isVisible == value) return;

                _isVisible = value;
                IsVisibleChanged?.Invoke(this, EventArgs.Empty);
                RaiseVisualChanged();
            }
        }

        /// <summary>
        /// Notify the components that registered the <see cref="IsVisibleChanged"/>;
        /// </summary>
        public void RaiseVisualChanged()
        {
            VisualChanged?.Invoke(this, EventArgs.Empty);
        }

        public virtual void Draw(ICanvas canvas)
        {
        }
    }
}