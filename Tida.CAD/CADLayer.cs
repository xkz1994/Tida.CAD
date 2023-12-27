using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Media;

namespace Tida.CAD
{
    /// <summary>
    /// The layer of cad
    /// </summary>
    public class CadLayer : CadElement
    {
        /// <summary>
        /// 对象被移除事件
        /// </summary>
        public event EventHandler<IEnumerable<DrawObject>> DrawObjectsRemoved;

        /// <summary>
        /// 对象被增加事件
        /// </summary>
        public event EventHandler<IEnumerable<DrawObject>> DrawObjectsAdded;

        /// <summary>
        /// 绘制对象集合已经被清除事件
        /// </summary>
        public event EventHandler DrawObjectsCleared;

        /// <summary>
        /// 绘制对象集合即将被清除事件
        /// </summary>
        public event EventHandler DrawObjectsClearing;

        private Brush _background;

        public Brush Background
        {
            get => _background;
            set
            {
                if (_background == value) return;

                _background = value;
                RaiseVisualChanged();
            }
        }

        /// <summary>
        /// The draw objects of the layer;
        /// </summary>
        private readonly List<DrawObject> _drawObjects = new List<DrawObject>();

        /// <summary>
        /// 绘画对象
        /// </summary>
        public IReadOnlyList<DrawObject> DrawObjects => new ReadOnlyCollection<DrawObject>(_drawObjects);

        /// <summary>
        /// Draw一个画布
        /// </summary>
        public override void Draw(ICanvas canvas)
        {
            if (Background == null) return;

            var topLeftPoint = canvas.CadScreenConverter.ToCad(new Point(0, 0));
            var bottomRightPoint = canvas.CadScreenConverter.ToCad(new Point(canvas.CadScreenConverter.ActualWidth, canvas.CadScreenConverter.ActualHeight));

            canvas.DrawRectangle(new CadRect(topLeftPoint, bottomRightPoint), Background, null);
        }

        /// <summary>
        /// Add draw object instance
        /// </summary>
        public void AddDrawObject(DrawObject drawObject)
        {
            AddDrawObjectCore(drawObject);
            RaiseVisualChanged();
            DrawObjectsAdded?.Invoke(this, new[] { drawObject });
        }

        /// <summary>
        /// Add a series of draw objects to the layer;
        /// </summary>
        /// <param name="drawObjects"></param>
        public void AddDrawObjects(IEnumerable<DrawObject> drawObjects)
        {
            if (drawObjects == null) throw new ArgumentNullException(nameof(drawObjects));

            foreach (var drawObject in drawObjects)
            {
                AddDrawObjectCore(drawObject);
            }

            RaiseVisualChanged();
            DrawObjectsAdded?.Invoke(this, drawObjects);
        }

        /// <summary>
        /// 移除绘制元素
        /// </summary>
        public void RemoveDrawObject(DrawObject drawObject)
        {
            RemoveDrawObjectCore(drawObject);

            RaiseVisualChanged();
            DrawObjectsRemoved?.Invoke(this, new[] { drawObject });
        }

        /// <summary>
        /// 移除绘制元素集合;
        /// </summary>
        /// <param name="drawObjects"></param>
        public void RemoveDrawObjects(IEnumerable<DrawObject> drawObjects)
        {
            if (drawObjects == null) throw new ArgumentNullException(nameof(drawObjects));

            foreach (var drawObject in drawObjects)
                if (_drawObjects.Contains(drawObject) && drawObject.Layer == this)
                    RemoveDrawObjectCore(drawObject);

            RaiseVisualChanged();
            DrawObjectsRemoved?.Invoke(this, drawObjects);
        }

        /// <summary>
        /// 清除绘制元素
        /// </summary>
        public void Clear()
        {
            DrawObjectsClearing?.Invoke(this, EventArgs.Empty);

            foreach (var drawObject in DrawObjects) drawObject.InternalLayer = null;

            _drawObjects.Clear();
            DrawObjectsCleared?.Invoke(this, EventArgs.Empty);
            RaiseVisualChanged();
        }

        /// <summary>
        /// 移除绘制元素核心;
        /// </summary>
        /// <param name="drawObject"></param>
        private void RemoveDrawObjectCore(DrawObject drawObject)
        {
            if (drawObject == null) throw new ArgumentNullException(nameof(drawObject));

            //检查绘制元素是否属于本实例;
            if (drawObject.Layer != this) throw new InvalidOperationException("This instance doesn't own the drawObject.");

            _drawObjects.Remove(drawObject);
            drawObject.InternalLayer = null;
        }

        /// <summary>
        /// 添加绘制对象核心;
        /// </summary>
        private void AddDrawObjectCore(DrawObject drawObject)
        {
            if (drawObject == null) throw new ArgumentNullException(nameof(drawObject));

            //检查是否是独立的绘制元素;
            if (drawObject.Layer != null) throw new InvalidOperationException("Please remove the drawObject from its parent first.");

            _drawObjects.Add(drawObject);
            drawObject.InternalLayer = this;
        }
    }
}