/*************************************************************************
 *  Copyright ? 2019 Mogoson. All rights reserved.
 *------------------------------------------------------------------------
 *  File         :  GraphUtility.cs
 *  Description  :  Utility for graph.
 *------------------------------------------------------------------------
 *  Author       :  Mogoson
 *  Version      :  1.0
 *  Date         :  4/19/2019
 *  Description  :  Initial development version.
 *************************************************************************/

using System.Drawing;
using System.Drawing.Imaging;
using System.IO;

namespace MGS.Graph
{
    /// <summary>
    /// Utility for graph.
    /// </summary>
    public sealed class GraphUtility
    {
        #region Public Method
        /// <summary>
        /// Get frames from image file.
        /// </summary>
        /// <param name="file"></param>
        /// <returns></returns>
        public static Bitmap[] GetFrames(string file)
        {
            if (!File.Exists(file))
            {
                return null;
            }

            try
            {
                var image = Image.FromFile(file);
                return GetFrames(image);
            }
            catch
            {
                return null;
            }
        }

        /// <summary>
        /// Get frames from image.
        /// </summary>
        /// <param name="image"></param>
        /// <returns></returns>
        public static Bitmap[] GetFrames(Image image)
        {
            if (image == null)
            {
                return null;
            }

            var dimension = new FrameDimension(image.FrameDimensionsList[0]);
            var framesCount = image.GetFrameCount(dimension);
            var frames = new Bitmap[framesCount];
            for (int i = 0; i < framesCount; i++)
            {
                var bitmap = new Bitmap(image.Width, image.Height);
                var graphics = Graphics.FromImage(bitmap);

                image.SelectActiveFrame(dimension, i);
                graphics.DrawImage(image, Point.Empty);
                frames[i] = bitmap;
            }
            return frames;
        }

        /// <summary>
        /// Get buffer data from bitmap.
        /// </summary>
        /// <param name="bitmap"></param>
        /// <param name="format"></param>
        /// <returns></returns>
        public static byte[] GetBuffer(Bitmap bitmap, ImageFormat format)
        {
            if (bitmap == null)
            {
                return null;
            }

            using (var stream = new MemoryStream())
            {
                bitmap.Save(stream, format);
                return stream.GetBuffer();
            }
        }
        #endregion
    }
}