using System.Runtime.InteropServices;

namespace CustomFishingBot
{
  internal class SystemAPI
  {
    const uint RIGHTDOWN = 0x0008;
    const uint RIGHTUP = 0x0010;

    public static void RMBClick()
    {
      mouse_event(RIGHTDOWN, 0, 0, 0, UIntPtr.Zero);
      mouse_event(RIGHTUP, 0, 0, 0, UIntPtr.Zero);
    }

    [DllImport("user32.dll")]
    static extern void mouse_event(
        uint dwFlags,
        uint dx,
        uint dy,
        uint dwData,
        UIntPtr dwExtraInfo);

    [DllImport("user32.dll")]
    private static extern short GetAsyncKeyState(int vKey);

    public static bool IsPressed(ConsoleKey key)
    {
      return (GetAsyncKeyState((int)key) & 0x8000) != 0;
    }

  }
}
