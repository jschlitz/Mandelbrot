using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Numerics;

namespace Mandelbrot
{
  /// <summary>
  /// Interaction logic for MainWindow.xaml
  /// </summary>
  public partial class MainWindow : Window
  {
    public MainWindow()
    {
      InitializeComponent();
    }

    static readonly double STEP = 1.0 / 512.0;
    const int MAX = 128;
    private void Recursive_Click(object sender, RoutedEventArgs e)
    {
      TimeSpan took = TimeTest((z,i) => Utils.MandelbrotR(z,MAX, i));
      MessageBox.Show(took.TotalMilliseconds.ToString());
    }

    private static TimeSpan TimeTest(Func<Complex, int, int> m)
    {
      var start = DateTime.Now;
      for (var r = -2.0; r < 1.0; r += STEP)
      {
        for (var i = -1.0; i < 1.0; i += STEP)
        {
          m(new Complex(r, i), 0);
        }
      }
      var took = DateTime.Now - start;
      return took;
    }

    private void Iterative_Click(object sender, RoutedEventArgs e)
    {
      TimeSpan took = TimeTest((z, i) => Utils.Mandelbrot(z, MAX));
      MessageBox.Show(took.TotalMilliseconds.ToString());
    }

    private void Button_Click(object sender, RoutedEventArgs e)
    {
      var start = DateTime.Now;

      DrawFractal();

      TheButton.Content = (DateTime.Now - start).TotalMilliseconds.ToString();

      ////http://www.interact-sw.co.uk/iangblog/2006/03/10/wpfrawpixels
    }


    double FractalWidth = DEFAULT_FRACTAL_WIDTH;
    double FractalHeight = DEFAULT_FRACTAL_HEIGHT;
    double FractalLeft= DEFAULT_X_MIN;
    double FractalTop = DEFAULT_Y_MIN;
    int GiveUp = 50;

    private void DrawFractal()
    {
      var w = (int)TargetWidth;
      var h = (int)TargetHeight;
      var dx = FractalWidth / w;
      var dy = FractalHeight / h;
      var arr = new byte[w * h * 4];
      //for (int y = 0; y < h; y++)
      Parallel.For(0, h, y =>
      {
        var imaginary = FractalTop + y * dx;
        for (int x = 0; x < w; x++)
        {
          var real = FractalLeft + dx * x;
          var escape = Utils.Mandelbrot(new Complex(real, imaginary), GiveUp);
          var i = (y * w + x) * 4;
          Utils.GetColor(escape, out arr[i + 2], out arr[i + 1], out arr[i + 0]);
          arr[i + 3] = 0xff;
        }
      });

      var bs = BitmapSource.Create(w, h,
        96, 96, PixelFormats.Bgra32, null, arr, w * 4);
      TheImage.Source = bs;
    }

    double TargetWidth = 300.0;
    double TargetHeight = 200.0;
    const double DEFAULT_FRACTAL_WIDTH = 3.0;
    const double DEFAULT_FRACTAL_HEIGHT = 2.0;
    const double DEFAULT_X_MIN = -2.0;
    const double DEFAULT_Y_MIN = -1.0;

    private void TheWindow_SizeChanged(object sender, SizeChangedEventArgs e)
    {
      if (TheBorder.ActualWidth / TheBorder.ActualHeight > 3.0 / 2.0)
      {
        TargetWidth = TheBorder.ActualHeight * 3.0 / 2.0;
        TargetHeight = TheBorder.ActualHeight;
      }
      else
      {
        TargetWidth = TheBorder.ActualWidth;
        TargetHeight = TheBorder.ActualWidth * 2.0 /3.0;
      }
    }

    private void TheImage_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
    {
      var p = UiToFractal(e.GetPosition(TheImage));
      GiveUp = Math.Min(Utils.MAX_TRIES, (int)(1.25 * GiveUp)); //wild guess at what increase is reasonable.
      FractalHeight /= 2.0;
      FractalWidth /= 2.0;
      FractalTop = p.Y - FractalHeight / 2.0;
      FractalLeft = p.X - FractalWidth / 2.0;
      DrawFractal();
    }

    private Point UiToFractal(Point p)
    {
      return new Point(FractalWidth * p.X / TheImage.ActualWidth + FractalLeft,
        FractalHeight * p.Y / TheImage.ActualHeight + FractalTop);
    }

    private void TheWindowLoaded(object sender, RoutedEventArgs e)
    {
      DrawFractal();
    }
  }
}
