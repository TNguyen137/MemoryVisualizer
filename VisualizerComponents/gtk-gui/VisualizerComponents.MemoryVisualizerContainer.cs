
// This file has been generated by the GUI designer. Do not modify.
namespace VisualizerComponents
{
	public partial class MemoryVisualizerContainer
	{
		private global::Gtk.Fixed FixedContainer;

		protected virtual void Build()
		{
			global::Stetic.Gui.Initialize(this);
			// Widget VisualizerComponents.MemoryVisualizerContainer
			global::Stetic.BinContainer.Attach(this);
			this.Name = "VisualizerComponents.MemoryVisualizerContainer";
			// Container child VisualizerComponents.MemoryVisualizerContainer.Gtk.Container+ContainerChild
			this.FixedContainer = new global::Gtk.Fixed();
			this.FixedContainer.Name = "FixedContainer";
			this.FixedContainer.HasWindow = false;
			this.Add(this.FixedContainer);
			if ((this.Child != null))
			{
				this.Child.ShowAll();
			}
			this.Hide();
		}
	}
}
