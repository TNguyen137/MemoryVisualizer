using System;
namespace VisualizerComponents
{
	/// <summary>
	/// Basic wrapper around WireWidget for easy-to-use horizontal wire in the GUI designer.
	/// </summary>
	/// <remarks>
	/// This class only exists so when using the GUI designer, there is a horizontal wire.
	/// It is exactly the same as a WireWidget.
	/// </remarks>
	[System.ComponentModel.ToolboxItem(true)]
	public partial class HWireWidget : WireWidget
	{
		public HWireWidget()
		{
			this.Build();
		}
	}
}