
// This file has been generated by the GUI designer. Do not modify.
namespace VisualizerComponents
{
	public partial class WireWidget
	{
		private global::Gtk.EventBox eventbox2;

		protected virtual void Build()
		{
			global::Stetic.Gui.Initialize(this);
			// Widget VisualizerComponents.WireWidget
			global::Stetic.BinContainer.Attach(this);
			this.WidthRequest = 50;
			this.HeightRequest = 50;
			this.Name = "VisualizerComponents.WireWidget";
			// Container child VisualizerComponents.WireWidget.Gtk.Container+ContainerChild
			this.eventbox2 = new global::Gtk.EventBox();
			this.eventbox2.Name = "eventbox2";
			this.Add(this.eventbox2);
			if ((this.Child != null))
			{
				this.Child.ShowAll();
			}
			this.Hide();
		}
	}
}
