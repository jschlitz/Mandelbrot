using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Numerics;

namespace Mandelbrot
{
  public static class Utils
  {
    static Utils()
    {
      //A static constructor. huh.
      COLORS[0] = 0;
      COLORS[1] = 0;
      COLORS[2] = 0;
      const int K = 7;
      for (int i = 1; i < MAX_TRIES; i ++)
      {
        COLORS[i * 3 + 0] = (byte)Math.Abs(255 - ((i * K * 2) % 512));
        COLORS[i * 3 + 1] = (byte)Math.Abs(255 - ((i * K * 3) % 512));
        COLORS[i * 3 + 2] = (byte)Math.Abs(255 - ((i * K * 5) % 512));
      }
    }



    public static int MandelbrotR(Complex z, int max, int it)
    {
      //THis is totally wrong. oops.

      //5.6 seconds @ resolution of 1/512
      if (z.Magnitude >= 2) return it;
      if (it > max) return -1;
      return MandelbrotR(z * z, max, it + 1);
    }

    public static int Mandelbrot(Complex c, int max)
    {
      //3.3 seconds @ resolution of 1/512
      Complex z = Complex.Zero;
      for (int i = 0; i < max; i++)
      {
        if (z.Magnitude >= 2)
        {
          return i;
        }
        z = z * z + c;
      }
      return 0;
    }

    public static int MAX_TRIES = 500;
    private static readonly byte[] COLORS = new byte[MAX_TRIES * 3 ];
    public static void GetColor(int i, out byte r, out byte g, out byte b)
    {
      r = COLORS[i * 3 + 0];
      g = COLORS[i * 3 + 1];
      b = COLORS[i * 3 + 2];
    }
  }
}
