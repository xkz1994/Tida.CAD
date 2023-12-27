using System.Collections.Generic;
using System.ComponentModel;
using System.Windows;

namespace Tida.CAD.Events
{
    /// <summary>
    /// The event args used while selecting draw objects with clicking;
    /// </summary>
    public class ClickSelectingEventArgs : CancelEventArgs
    {
        public ClickSelectingEventArgs(Point position, IList<DrawObject> hitedDrawObjects)
        {
            HitPosition = position;
            HitedDrawObjects = hitedDrawObjects;
        }

        /// <summary>
        /// The position where the mouse clicked
        /// </summary>
        public Point HitPosition { get; }

        /// <summary>
        /// The draw object to select
        /// </summary>
        public IList<DrawObject> HitedDrawObjects { get; }
    }
}