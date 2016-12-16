using System;
namespace VisualizerComponents
{
	/// <summary>
	/// Simple widget with a label and read-only entry box.
	/// </summary>
	[System.ComponentModel.ToolboxItem(true)]
	public partial class MemoryItemWidget : Gtk.Bin
	{
		/// <summary>
		/// Initializes a new instance of the <see cref="T:VisualizerComponents.MemoryItemWidget"/> class.
		/// </summary>
		public MemoryItemWidget()
		{
			this.Build();
		}

		/// <summary>
		/// The text of the label.
		/// </summary>
		/// <value>The label text.</value>
		public string LabelText
		{
			get { return this.Label.Text; }
			set { this.Label.Text = value; }
		}

		/// <summary>
		/// Gets or sets the data text.
		/// </summary>
		/// <value>The data text.</value>
		public string DataText
		{
			get { return this.Entry.Text; }
			set { this.Entry.Text = value; }
		}

		/// <summary>
		/// Whether or not this <see cref="T:VisualizerComponents.MemoryItemWidget" /> data text is user editable.
		/// </summary>
		/// <value><c>true</c> if data text is user editable; otherwise, <c>false</c>.</value>
		public bool DataTextUserEditable
		{
			get { return this.Entry.IsEditable; }
			set { this.Entry.IsEditable = value; }
		}

		/// <summary>
		/// Gets or sets the visualizer style.
		/// </summary>
		/// <value>The visualizer style.</value>
		//TODO: Implement Style changes once VisualizerStyle class is implemented.
		public VisualizerStyle VisualizerStyle { get; set; }
	}
}