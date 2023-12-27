using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Media;

namespace Tida.CAD.WPF
{
    /// <summary>
    /// Visible element
    /// </summary>
    public class VisualContainer : FrameworkElement
    {
        public VisualContainer()
        {
            Focusable = true;
        }

        /// <summary>
        /// 当前所有的可见对象
        /// </summary>
        private readonly List<Visual> _visuals = new List<Visual>();

        /// <summary>
        /// 获取界面上所有的可视化对象
        /// </summary>
        public IEnumerable<Visual> Visuals => _visuals.Select(p => p);

        /// <summary>
        /// 获取Visual的个数
        /// 重写VisualChildrenCount属性并返回已经增加了的可视化对象的数量
        /// </summary>
        protected override int VisualChildrenCount => _visuals.Count;

        /// <summary>
        /// 获取Visual
        /// 重写GetVisualChild()方法，当通过索引号请求可视化对象时，添加返回可视化对象所需的代码
        /// </summary>
        protected override Visual GetVisualChild(int index)
        {
            if (index < 0 || index >= _visuals.Count) return null;

            return _visuals[index];
        }

        /// <summary>
        /// 添加Visual
        /// </summary>
        public void AddVisual(Visual visual)
        {
            _visuals.Add(visual);
            // 元素调用AddVisualChild()和AddLogicalChild()方法来注册可视化对象。
            // 从技术角度看，为了显示可视化对象，不需要执行这些任务，但为了保证正确跟踪可视化对象、在可视化树和逻辑树中显示可视化对象以及使用其他WPF特性（如命中测试），需要执行这些操作.
            AddVisualChild(visual);
            AddLogicalChild(visual);
        }

        /// <summary>
        /// 插入Visual;
        /// </summary>
        public void InsertVisual(int index, Visual visual)
        {
            _visuals.Insert(index, visual);
            AddVisualChild(visual);
            AddLogicalChild(visual);
        }

        /// <summary>
        /// 删除Visual
        /// </summary>
        public void RemoveVisual(Visual visual)
        {
            if (!_visuals.Contains(visual))
            {
                throw new InvalidOperationException($"The Visual Children doesn't contain the visual.");
            }

            _visuals.Remove(visual);
            RemoveVisualChild(visual);
            RemoveLogicalChild(visual);
        }

        /// <summary>
        /// 清除视图
        /// </summary>
        protected void ClearVisuals()
        {
            foreach (var visual in _visuals)
            {
                RemoveVisualChild(visual);
                RemoveLogicalChild(visual);
            }

            _visuals.Clear();
        }

        /// <summary>
        /// 命中测试
        /// </summary>
        protected Visual GetVisual(Point point)
        {
            var hitResult = VisualTreeHelper.HitTest(this, point);
            return hitResult.VisualHit as Visual;
        }
    }
}