using System;
namespace VisualizerComponents
{
	/// <summary>
	/// A Widget that acts like a "wire".
	/// 
	/// A base class for <see cref="VWireWidget"/> and <see cref="HWireWidget"/>. While this class can be used on its
	/// own, VWireWidget and HWireWidget should be used in the GUI designer.
	/// </summary>
	/// <remarks>
	/// /// The thickness of the wire is defined by VisualizerStyle.OutlineWidth. The wire color is defined by
	/// VisualizerStyle.OutlineColor.
	/// 
	/// Set the <see cref="Identifier"/>and you can use the <see cref="MemoryVisualizerContainer.ChangeWireStyle"/> to
	/// change all wires with the same identifier. Makes it easy to build non-straight wires or many wires that should
	/// be highlighted at the same time.
	/// 
	/// This Widget really isn't meant to be used directly. <see cref="HWireWidget"/> and <see cref="VWireWidget"/> are
	/// built to be used directly in the GUI designer. As wires are typically very thin and this class has no knowledge
	/// of the direction of the thinness, the other classes have that built in.
	/// </remarks>
	[System.ComponentModel.ToolboxItem(true)]
	public partial class WireWidget : Gtk.Bin
	{
		public WireWidget()
		{
			this.Build();
			this.VisualizerStyle = VisualizerComponents.VisualizerStyle.StandardStyles.WireDefault;
		}

		/// <summary>
		/// Gets or sets the identifier. All Wires in a <see cref="MemoryVisualizerContainer"/> with the same Identifier
		/// can have their styles all changed at once with <see cref="MemoryVisualizerContainer.ChangeWireStyle"/>.
		/// Makes it easy to build non-straight wires or many wires that should be highlighted at the same time.
		/// </summary>
		/// <value>The identifier.</value>
		public string Identifier { get; set; }

		private VisualizerStyle m_VisualizerStyle;
		/// <summary>
		/// Gets or sets the visualizer style. It uses the VisualizerStyle.OutlineColor to determine the wire color.
		/// Visualizer.OutlineWidth is used to determine the wire 'width'.
		/// </summary>
		/// <value>The visualizer style.</value>
		public VisualizerStyle VisualizerStyle
		{
			get { return m_VisualizerStyle; }
			set
			{
				m_VisualizerStyle = value;
				this.eventbox2.ModifyBg(Gtk.StateType.Normal, m_VisualizerStyle.OutlineColor);
				if (this.WidthRequest > this.HeightRequest)
					this.HeightRequest = m_VisualizerStyle.OutlineWidth;
				else
					this.WidthRequest = m_VisualizerStyle.OutlineWidth;
			}
		}
	}
}