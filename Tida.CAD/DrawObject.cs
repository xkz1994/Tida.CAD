using System;
using System.Windows;
using System.Windows.Input;
using Tida.CAD.Events;
using Tida.CAD.Input;

namespace Tida.CAD
{
    /// <summary>
    /// Draw object in layer
    /// </summary>
    public abstract partial class DrawObject : CadElement
    {
        public event EventHandler<CadMouseButtonEventArgs> PreviewMouseDown;
        public event EventHandler<CadMouseEventArgs> PreviewMouseMove;
        public event EventHandler<CadMouseButtonEventArgs> PreviewMouseUp;
        public event EventHandler<CadKeyEventArgs> PreviewKeyDown;
        public event EventHandler<CadKeyEventArgs> PreviewKeyUp;
        public event EventHandler<TextCompositionEventArgs> PreviewTextInput;

        /// <summary>
        /// The event raised when IsSelected changed
        /// </summary>
        public event EventHandler<ValueChangedEventArgs<bool>> IsSelectedChanged;

        private bool _isSelected;

        /// <summary>
        /// IsSelected
        /// </summary>
        public bool IsSelected
        {
            get => _isSelected;
            set
            {
                if (_isSelected == value) return;

                _isSelected = value;
                var e = new ValueChangedEventArgs<bool>(_isSelected, !_isSelected);
                OnSelectedChanged(e);
                IsSelectedChanged?.Invoke(this, e);
                RaiseVisualChanged();
            }
        }

        internal CadLayer InternalLayer { get; set; }

        /// <summary>
        /// The parent layer of the draw object;
        /// </summary>
        public CadLayer Layer => InternalLayer;
    }

    /// <summary>
    /// 绘制对象的交互（仅当<see cref="IsSelected"/> is True）时，这些交互才可用）
    /// </summary>
    public abstract partial class DrawObject
    {
        /// <summary>
        /// Indicates whether the point in inside the object
        /// </summary>
        public virtual bool PointInObject(Point point, ICadScreenConverter cadScreenConverter) => false;

        /// <summary>
        /// Indicated whether the object in inside a rectangle
        /// </summary>
        /// <param name="rect">The selection rectangle</param>
        /// <param name="anyPoint">指示当 rect 刚好与不在 rect 内的 draw object 相交时是否应命中 draw object</param>
        public virtual bool ObjectInRectangle(CadRect rect, ICadScreenConverter cadScreenConverter, bool anyPoint) => false;

        /// <summary>
        /// Get the bounding rect for the draw object
        /// </summary>
        public virtual CadRect? GetBoundingRect() => null;

        /// <summary>
        /// The method invoked while the mouse is released
        /// </summary>
        public void OnMouseUp(CadMouseButtonEventArgs e)
        {
            PreviewMouseUp?.Invoke(this, e);
            if (e.Handled) return;

            OnMouseUpCore(e);
        }

        /// <summary>
        /// The method invoked while mouse is pressed
        /// </summary>
        public void OnMouseDown(CadMouseButtonEventArgs e)
        {
            PreviewMouseDown?.Invoke(this, e);
            if (e.Handled) return;

            OnMouseDownCore(e);
        }

        /// <summary>
        /// The method invoked while mouse is moving
        /// </summary>
        public void OnMouseMove(CadMouseEventArgs e)
        {
            PreviewMouseMove?.Invoke(this, e);
            if (e.Handled) return;

            OnMouseMoveCore(e);
        }

        public void OnKeyUp(CadKeyEventArgs e)
        {
            PreviewKeyUp?.Invoke(this, e);

            if (e.Handled) return;

            OnKeyUpCore(e);
        }

        /// <summary>
        /// The method invoked while the mouse is released
        /// </summary>
        public void OnKeyDown(CadKeyEventArgs e)
        {
            PreviewKeyDown?.Invoke(this, e);
            if (e.Handled) return;

            OnKeyDownCore(e);
        }

        public void OnTextInput(TextCompositionEventArgs e)
        {
            PreviewTextInput?.Invoke(this, e);
            if (e.Handled) return;

            OnTextInputCore(e);
        }

        protected virtual void OnMouseUpCore(CadMouseButtonEventArgs e)
        {
        }

        protected virtual void OnMouseDownCore(CadMouseButtonEventArgs e)
        {
        }

        protected virtual void OnMouseMoveCore(CadMouseEventArgs e)
        {
        }

        /// <summary>
        /// The protected virtual method invoked while IsSelected changed
        /// </summary>
        protected virtual void OnSelectedChanged(ValueChangedEventArgs<bool> e)
        {
        }

        protected virtual void OnKeyUpCore(CadKeyEventArgs e)
        {
        }

        protected virtual void OnKeyDownCore(CadKeyEventArgs e)
        {
        }

        protected virtual void OnTextInputCore(TextCompositionEventArgs e)
        {
        }
    }
}