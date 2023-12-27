using System.Windows;

namespace Tida.CAD
{
    /// <summary>
    /// The converter between screen coordinates and CAD coordinates;
    /// 屏幕坐标和CAD坐标之间的转换器;
    /// </summary>
    public interface ICADScreenConverter
    {
        /// <summary>
        /// The actual width of the cad in pixel;
        /// cad 的实际宽度（以像素为单位）;
        /// </summary>
        double ActualWidth { get; }

        /// <summary>
        /// The actual height of the cad in pixel;
        /// cad 的实际高度（以像素为单位）;
        /// </summary>
        double ActualHeight { get; }

        /// <summary>
        /// Convert the point in CAD coordinates into a point in rendering coordinates;
        /// 将 CAD 坐标中的点转换为渲染坐标中的点;
        /// </summary>
        /// <param name="cadPoint"></param>
        /// <returns></returns>
        Point ToScreen(Point cadPoint);

        /// <summary>
        /// Convert a length in cad coordinates into a length in pixel;
        /// 将 cad 坐标中的长度转换为像素中的长度;
        /// </summary>
        /// <param name="unitvalue"></param>
        /// <returns></returns>
        double ToScreen(double unitvalue);

        /// <summary>
        /// Convert a length in pixel into a length in cad coordinates;
        /// 将以像素为单位的长度转换为以 cad 坐标为单位的长度;
        /// </summary>
        /// <param name="pixelLength"></param>
        /// <returns></returns>
        double ToCAD(double pixelLength);

        /// <summary>
        /// Convert a point in rendering coordinates into a point in CAD coordinates;
        /// 将渲染坐标中的点转换为 CAD 坐标中的点;
        /// </summary>
        /// <param name="screenPoint"></param>
        /// <returns></returns>
        Point ToCAD(Point screenPoint);


        /// <summary>
        /// The current zoom of the cad;
        /// cad 的当前缩放;
        /// </summary>
        double Zoom { get; }
    }
}