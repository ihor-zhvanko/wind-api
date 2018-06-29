using System;
using Wind.Api.Models;

namespace Wind.Api.Helpers.Models
{
  public class InterpolationNode<TValue>
  {
    public Point Arg { get; set; }
    public TValue Value { get; set; }
  }
}