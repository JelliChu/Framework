using System.Drawing;

namespace MvvmFramework.WinUI;

public class ImageSamplingService
{
    public Color GetAverageColour(Bitmap bitmap, Point[] sampleCoords)
    {
        var r = 0;
        var g = 0;
        var b = 0;
        var total = 0;

        foreach(var coord in sampleCoords)
        {
            var color = bitmap.GetPixel(coord.X, coord.Y);

            r += color.R;
            g += color.G;
            b += color.B;

            total++;
        }

        if(total == 0)
        {
            return Color.White;
        }

        return Color.FromArgb(
           r / total,
           g / total,
           b / total);
    }
}
