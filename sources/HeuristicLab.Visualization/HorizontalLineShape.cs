﻿using System.Drawing;

namespace HeuristicLab.Visualization {
  public class MinMaxLineShape : WorldShape {
    private readonly LineShape minLineShape;
    private readonly LineShape maxLineShape;

    /// <summary>
    /// Initializes the HorizontalLineShape.
    /// </summary>
    /// <param name="color">color for the LineShape</param>
    /// <param name="yMin">y value for lower line</param>
    /// <param name="yMax">y value for upper line</param>
    /// <param name="thickness">line thickness</param>
    /// <param name="style">line style</param>
    public MinMaxLineShape(double yMin, double yMax, Color color, int thickness, DrawingStyle style) {
      minLineShape = new LineShape(0, yMin, 1, yMin, color, thickness, style);
      maxLineShape = new LineShape(0, yMax, 1, yMax, color, thickness, style);
      shapes.Add(minLineShape);
      shapes.Add(maxLineShape);
    }


    /// <summary>
    /// Draws the HorizontalLineShape.
    /// </summary>
    /// <param name="graphics">graphics handle to draw to</param>
    /// <param name="viewport">rectangle in value-coordinates to display</param>
    /// <param name="clippingArea">rectangle in screen-coordinates to draw</param>
    public override void Draw(Graphics graphics, Rectangle viewport, RectangleD clippingArea) {
      minLineShape.X1 = ClippingArea.X1;
      minLineShape.X2 = ClippingArea.X2;
      maxLineShape.X1 = ClippingArea.X1;
      maxLineShape.X2 = ClippingArea.X2;
      base.Draw(graphics, viewport, clippingArea);
    }


    public double YMin {
      set {
        minLineShape.Y1 = value;
        minLineShape.Y2 = value;
      }
    }

    public double YMax {
      set {
        maxLineShape.Y1 = value;
        maxLineShape.Y2 = value;
      }
    }
  }
}