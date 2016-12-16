using System;
namespace VisualizerComponents
{
	/// <summary>
	/// Basic wrapper around WireWidget for easy-to-use vertical wire in the GUI designer.
	/// </summary>
	/// <remarks>
	/// This class only exists so when using the GUI designer, there is a vertical wire.
	/// It is exactly the same as a WireWidget.
	/// </remarks>
	[System.ComponentModel.ToolboxItem(true)]
	public partial class VWireWidget : WireWidget
	{
		public VWireWidget()
		{
			this.Build();
		}
	}
}