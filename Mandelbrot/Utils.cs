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

    public static int MandelbrotM(decimal real, decimal imaginary, int max)
    {
      //doh! max is too small.
      decimal zr = 0;
      decimal zi = 0;
      for (int i = 0; i < max; i++)
      {
        var zr2 = zr * zr;
        var zi2 = zi * zi;
        if (zr2 + zi2  >= 4)
        {
          return i;
        }
        zi = 2 * zi * zr + imaginary;
        zr = zr2 - zi2 + real;
      }
      return 0;
    }


    public static int MAX_TRIES = 100;
    private static readonly byte[] COLORS = new byte[MAX_TRIES * 3 ];
    public static void GetColor(int i, out byte r, out byte g, out byte b)
    {
      r = COLORS[i * 3 + 0];
      g = COLORS[i * 3 + 1];
      b = COLORS[i * 3 + 2];
    }
  }
}
