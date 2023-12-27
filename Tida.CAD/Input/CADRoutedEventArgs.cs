using System;

namespace Tida.CAD.Input
{
    /// <summary>
    ///  Contains state information and event data associated of a routed event
    /// </summary>
    public abstract class CadRoutedEventArgs : EventArgs
    {
        protected CadRoutedEventArgs()
        {
        }

        /// <summary>
        /// Gets or sets a value that indicates the present state of the event handling for
        ///     a routed event as it travels the route.
        /// 获取或设置一个值，该值指示路由事件在路由中行进时事件处理的当前状态
        /// </summary>
        public bool Handled { get; set; }
    }
}