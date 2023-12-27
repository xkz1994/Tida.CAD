using System;

namespace Tida.CAD
{
    /// <summary>
    /// 可绘制对象
    /// </summary>
    public interface IDrawable
    {
        /// <summary>
        /// 本身图像已经发生了变化的事件
        /// </summary>
        event EventHandler VisualChanged;

        /// <summary>
        /// 可见状态发生了变化
        /// </summary>
        event EventHandler IsVisibleChanged;

        /// <summary>
        /// 绘制自身
        /// </summary>
        void Draw(ICanvas canvas);

        /// <summary>
        /// 是否可见
        /// </summary>
        bool IsVisible { get; set; }
    }
}