using System.Drawing;

namespace HeuristicLab.Visualization.Legend {
  public class LegendItem {
    public LegendItem(string label, Color color, int thickness) {
      Label = label;
      Color = color;
      Thickness = thickness;
    }

    public string Label { get; set; }
    public Color Color { get; set; }
    public int Thickness { get; set; }
  }
}