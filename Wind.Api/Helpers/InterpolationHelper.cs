using System;
using System.Linq;
using Wind.Api.Models;
using Wind.Api.Helpers.Models;

namespace Wind.Api.Helpers
{
  public static class InterpolationHelper
  {
    public static double BilinearInterpolation(InterpolationNode<double>[] nodes, Point value)
    {
      var topLeft = nodes.OrderByDescending(n => n.Arg.Latitude).ThenBy(n => n.Arg.Longitude).First();
      var topRight = nodes.OrderBy(n => n.Arg.Latitude).ThenBy(n => n.Arg.Longitude).First();
      var bottomRight = nodes.OrderBy(n => n.Arg.Latitude).ThenByDescending(n => n.Arg.Longitude).First();
      var bottomLeft = nodes.OrderByDescending(n => n.Arg.Latitude).ThenByDescending(n => n.Arg.Longitude).First();

      var f11 = topLeft.Value;
      var f21 = topRight.Value;
      var f12 = bottomLeft.Value;
      var f22 = bottomRight.Value;

      var x1 = topLeft.Arg.Latitude;
      var y1 = topLeft.Arg.Longitude;
      var x2 = bottomRight.Arg.Latitude;
      var y2 = bottomRight.Arg.Longitude;

      var x = value.Latitude;
      var y = value.Longitude;

      var interpolated = (
        f11 * (x2 - x) * (y2 - y) +
        f21 * (x - x1) * (y2 - y) +
        f12 * (x2 - x) * (y - y1) +
        f22 * (x - x1) * (y - y1)
      ) / ((x2 - x1) * (y2 - y1));

      return interpolated;
    }

    public static Vector BilinearInterpolation(InterpolationNode<Vector>[] nodes, Point value)
    {
      var xNodes = nodes.Select(n => new InterpolationNode<double> { Arg = n.Arg, Value = n.Value.X }).ToArray();
      var interpolatedX = BilinearInterpolation(xNodes, value);

      var yNodes = nodes.Select(n => new InterpolationNode<double> { Arg = n.Arg, Value = n.Value.Y }).ToArray();
      var interpolatedY = BilinearInterpolation(yNodes, value);

      return new Vector(interpolatedX, interpolatedY);
    }
  }
}