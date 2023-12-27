using System.Windows;

namespace Tida.CAD
{
    /// <summary>
    /// 屏幕坐标和CAD坐标之间的转换器;
    /// </summary>
    public interface ICadScreenConverter
    {
        /// <summary>
        /// cad 的实际宽度（以像素为单位）
        /// </summary>
        double ActualWidth { get; }

        /// <summary>
        /// cad 的实际高度（以像素为单位）
        /// </summary>
        double ActualHeight { get; }

        /// <summary>
        /// 将 CAD 坐标中的点转换为渲染坐标中的点;
        /// </summary>
        Point ToScreen(Point unitPoint);

        /// <summary>
        /// 将 cad 坐标中的长度转换为像素中的长度;
        /// </summary>
        double ToScreen(double unitValue);

        /// <summary>
        /// 将以像素为单位的长度转换为以 cad 坐标为单位的长度
        /// </summary>
        double ToCad(double screenValue);

        /// <summary>
        /// 将渲染坐标中的点转换为 CAD 坐标中的点
        /// </summary>
        Point ToCad(Point screenPoint);

        /// <summary>
        /// cad 的当前缩放
        /// </summary>
        double Zoom { get; }
    }
}