namespace Sitecore.Support.Resources.Media
{
    using System.Drawing;
    using System.Drawing.Imaging;
    using System.IO;
    using Sitecore.Diagnostics;
    using Sitecore.Resources.Media;

    /// <summary>
    /// RestoreColorProfileProcessor class
    /// </summary>
    public class RestoreColorProfileProcessor
    {
        /// <summary>
        /// Runs the processor.
        /// </summary>
        /// <param name="args">The arguments.</param>
        public void Process([NotNull] GetMediaStreamPipelineArgs args)
        {
            Assert.ArgumentNotNull(args, "args");

            if (!(args.CustomData["ColorProfile"] is PropertyItem) || args.OutputStream == null)
            {
                return;
            }

            ImageFormat imageFormat = MediaManager.Config.GetImageFormat(args.MediaData.Extension, null);
            if (imageFormat == null)
            {
                return;
            }

            using (var stream = args.OutputStream.Stream)
            {
                var bitmap = new Bitmap(stream);
                bitmap.SetPropertyItem(args.CustomData["ColorProfile"] as PropertyItem);

                var memoryStream = new MemoryStream();
                bitmap.Save(memoryStream, bitmap.RawFormat);                
                args.OutputStream = new MediaStream(memoryStream, args.MediaData.Extension, args.MediaData.MediaItem);
            }
        }
    }
}