using System;
using Gdk;

namespace VisualizerComponents
{
	/// <summary>
	/// Defines how visualizer elements will look so different visualizers can have a cohesive look.
	/// </summary>
	public class VisualizerStyle
	{
		/// <summary>
		/// The font color.
		/// </summary>
		/// <value>The color of the font.</value>
		public Color FontColor
		{
			get;
		}

		/// <summary>
		/// The fill color.
		/// </summary>
		/// <value>The color of the fill.</value>
		public Color FillColor
		{
			get;
		}

		/// <summary>
		/// The outline color.
		/// </summary>
		/// <value>The color of the outline.</value>
		public Color OutlineColor
		{
			get;
		}

		/// <summary>
		/// The width of the outline.
		/// </summary>
		/// <value>The width of the outline.</value>
		public int OutlineWidth
		{
			get;
		}

		/// <summary>
		/// The font.
		/// </summary>
		/// <value>The font.</value>
		// TODO: Change this to the correct type.
		public Font Font
		{
			get;
		}

		// HACK: This shouldn't exist, but I was too lazy to fix the other calls.
		public VisualizerStyle()
		{
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="T:VisualizerComponents.VisualizerStyle"/> class.
		/// </summary>
		/// <param name="fontColor">Font color.</param>
		/// <param name="fillColor">Fill color.</param>
		/// <param name="outlineColor">Outline color.</param>
		/// <param name="outlineWidth">Outline width.</param>
		/// <param name="font">Font.</param>
		public VisualizerStyle(Color fontColor, Color fillColor, Color outlineColor, int outlineWidth, Font font)
		{
			this.FontColor = fontColor;
			this.FillColor = fillColor;
			this.OutlineColor = outlineColor;
			this.OutlineWidth = outlineWidth;
			this.Font = font;
		}

		/// <summary>
		/// Static class for all standard styles.
		/// </summary>
		public static class StandardStyles
		{
			public static readonly Gdk.Color Black = new Gdk.Color(0, 0, 0);
			public static readonly Gdk.Color Green = new Gdk.Color(65, 219, 0);
			public static readonly VisualizerStyle DefaultStyle = new VisualizerStyle();
			public static readonly VisualizerStyle Highlighted = new VisualizerStyle();
			public static readonly VisualizerStyle WireDefault = new VisualizerStyle(Black, Black, Black, 3, new Font());
			public static readonly VisualizerStyle WireHighlighted = new VisualizerStyle(Black, Black, Green, 5, new Font());
		}
	}
}