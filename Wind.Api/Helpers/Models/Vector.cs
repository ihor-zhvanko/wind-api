using System;
using System.Linq;
using System.Collections.Generic;

namespace Wind.Api.Helpers.Models
{
  public class Vector
  {
    public Vector(double x, double y)
    {
      X = x;
      Y = y;
    }

    public Vector(double x1, double x2, double y1, double y2)
      : this(x2 - x1, y2 - y1) { }

    public Vector(double angle)
      : this(Math.Cos(ToRadians(angle)), Math.Sin(ToRadians(angle)))
    {

    }

    private static double ToRadians(double angle)
    {
      return angle * Math.PI / 180;
    }

    public double X { get; set; }

    public double Y { get; set; }

    public double Length => Math.Sqrt(X * X + Y * Y);

    public double Angle => Math.Acos(X / Length);

    public Vector Normilize()
    {
      var length = Length;
      if (length == 0)
        throw new ArgumentException("Vector length is 0");

      X /= length;
      Y /= length;

      return this;
    }

    public Vector Multiply(double value)
    {
      X *= value;
      Y *= value;

      return this;
    }
  }
}