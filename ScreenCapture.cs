using OpenCvSharp;
using System.Drawing;
using System.Threading.Tasks;

namespace CustomFishingBot
{
  internal class ScreenCapture
  {
    public static List<Rect> GreenZonesCoordinates = new();
    public static Bitmap Capture(Rectangle rect)
    {
      Bitmap bmp = new Bitmap(rect.Width, rect.Height);

      using Graphics g = Graphics.FromImage(bmp);

      g.CopyFromScreen(rect.Location, System.Drawing.Point.Empty, rect.Size);


      return bmp;
    }

    public static void FindGreenSectors(InputArray src)
    {
      GreenZonesCoordinates.Clear();

      Mat hsv = new();
      Cv2.CvtColor(src, hsv, ColorConversionCodes.BGR2HSV);

      Mat greenMask = new();
      Cv2.InRange(
        hsv,
        new Scalar(40, 100, 100),
        new Scalar(90, 255, 255),
        greenMask);

      Cv2.FindContours(
        greenMask,
        out OpenCvSharp.Point[][] contours,
        out _,
        RetrievalModes.External,
        ContourApproximationModes.ApproxSimple);

      // Add to list of green zone coordinates
      foreach (var contour in contours)
      {
        Rect rect = Cv2.BoundingRect(contour);

        if (rect.Width > 10)
          GreenZonesCoordinates.Add(rect);
      }
    }

    public static bool Find(InputArray src)
    {
      Mat hsv = new();

      Cv2.CvtColor(
          src,
          hsv,
          ColorConversionCodes.BGR2HSV);

      Mat whiteMask = new();

      Cv2.InRange(
          hsv,
          new Scalar(0, 0, 180),
          new Scalar(180, 60, 255),
          whiteMask);

      int sliderX = FindSliderX(whiteMask);

      if (sliderX < 0)
      {
        Console.WriteLine("Slider not found");
        return false;
      }

      Console.WriteLine($"Slider X = {sliderX}");

      return ValidateOnGreenSector(sliderX);
    }

    private static int FindSliderX(Mat whiteMask)
    {
      int bestX = -1;
      int maxWhitePixels = 0;

      for (int x = 0; x < whiteMask.Width; x++)
      {
        int count = 0;

        for (int y = 0; y < whiteMask.Height; y++)
        {
          if (whiteMask.At<byte>(y, x) > 0)
          {
            count++;
          }
        }

        if (count > maxWhitePixels)
        {
          maxWhitePixels = count;
          bestX = x;
        }
      }

      return bestX;
    }

    public static bool ValidateOnGreenSector(int sliderX)
    {
      foreach (var zone in GreenZonesCoordinates)
      {
        if (sliderX > zone.Left &&
            sliderX < zone.Right)
          return true;
      }
      return false;
    }



  }
}
