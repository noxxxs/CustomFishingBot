using OpenCvSharp;
using OpenCvSharp.Extensions;
using System.Drawing;


namespace CustomFishingBot
{
  public class Core
  {
    private static Rectangle _fishingArea = new Rectangle(570, 520, 800, 120);
    private static bool _isActive = false;

    public static void Main()
    {
      Thread.Sleep(1000);
      while (true)
      {
        if (SystemAPI.IsPressed(ConsoleKey.NumPad0) && !_isActive)
        {
          RunCycle();
        } 
      }
    }

    public static void RunCycle()
    {
      _isActive = true;
      using Bitmap firstBmp = ScreenCapture.Capture(_fishingArea);
      using Mat firstFrame = BitmapConverter.ToMat(firstBmp);
      ScreenCapture.FindGreenSectors(firstFrame);

      Cv2.ImWrite("debug.png", firstFrame);


      DateTime endTime = DateTime.Now.AddSeconds(6);
      while (DateTime.Now < endTime)
      {
        using Bitmap bmp = ScreenCapture.Capture(_fishingArea);
        using Mat frame = BitmapConverter.ToMat(bmp);

        bool onGreen = ScreenCapture.Find(frame);

        if (onGreen)
        {
          SystemAPI.RMBClick();
          _isActive = false;
          return;
        }

        Thread.Sleep(5);
      }
      _isActive = false;
    }
  }
}
