using System;
using System.ComponentModel;
using System.Collections.Generic;
using System.Linq;
using System.ComponentModel.Composition;

namespace VisualizerComponents
{
	/// <summary>
	/// Base container for all Visualizers.
	/// </summary>
	/// <remarks>
	/// This class is partial as well as abstract. Be sure any changes to this class also correctly updates the auto-
	/// generated file from the GUI designer.
	/// 
	/// All implementing classes will be exported with MEF (Managed Extensibility Framework).
	/// If you do not want your deriving class to be exported and show up in the main window drop-down menu, use the attribute [PartNotDiscoverable].
	/// See http://randomactsofcoding.blogspot.com/2009/11/working-with-managed-extensibility.html for tutorial on MEF.
	/// </remarks>
	[InheritedExport(typeof(MemoryVisualizerContainer))]
	public abstract partial class MemoryVisualizerContainer : Gtk.Bin, INotifyPropertyChanged
	{
		protected MemoryVisualizerContainer()
		{
			// Defines the default 'help' command.
			this.CommandLineMarshal.AddCommand("help", (commandName, args) => 
			{
				return new CommandLine.CommandResult(true, this.CommandLineHelp);
			});
		}
		
		/// <summary>
		/// Gets standard information about this Visualizer.
		/// </summary>
		/// <value>The information.</value>
		public abstract string Information { get; }

		/// <summary>
		/// The name of this visualizer.
		/// </summary>
		/// <value>The name.</value>
		public abstract string VisualizerName { get; }

		/// <summary>
		/// The string to display when the user types in 'help' into the command line.
		/// </summary>
		public abstract string CommandLineHelp { get; }

		/// <summary>
		/// The CommandLineMarshal for this visualizer.
		/// </summary>
		/// <remarks>
		/// The 'help' command is already predefined to return the CommandLineHelp string.
		/// To override this behavior, remove the 'help' command and readd it with the custom definition.
		/// </remarks>
		public CommandLine.CommandLineMarshal CommandLineMarshal { get; protected set; } = new CommandLine.CommandLineMarshal();

		/// <summary>
		/// Reset this Visualizer.
		/// </summary>
		public abstract void Reset();

		/// <summary>
		/// Step forward in the visualization by one state.
		/// </summary>
		public abstract void StepForward();

		/// <summary>
		/// Step backwards in the visualization by one state.
		/// </summary>
		public abstract void StepBack();

		/// <summary>
		/// A mapping of wire names to lists of wires. Wires are triggered by name, so triggering a name triggers the entire list.
		/// </summary>
		protected Dictionary<string, List<WireWidget>> Wires;

		/// <summary>
		/// Initializes the wires.
		/// </summary>
		protected void InitWires()
		{
			this.Wires = new Dictionary<string, List<WireWidget>>();

			List<WireWidget> wires = new List<WireWidget>();

			var children = new Queue<Gtk.Widget>(this.Children);
			while (children.Count != 0)
			{
				var child = children.Dequeue();
				if (child is WireWidget)
					wires.Add((WireWidget)child);

				if (child is Gtk.Container)
				{
					foreach (var subChild in ((Gtk.Container)child).Children)
					{
						children.Enqueue(subChild);
					}
				}
			}

			foreach (var wire in wires)
			{
				if (!Wires.ContainsKey(wire.Identifier))
				{
					Wires.Add(wire.Identifier, new List<WireWidget>());
				}
				Wires[wire.Identifier].Add(wire);
			}
		}

		/// <summary>
		/// Changes the style of wires by name.
		/// </summary>
		/// <param name="wireID">The name of the wires to change.</param>
		/// <param name="style">The style to change to.</param>
		public void ChangeWireStyle(string wireID, VisualizerStyle style)
		{
			foreach (var wire in Wires[wireID])
			{
				wire.VisualizerStyle = style;
			}
		}

		/// <summary>
		/// Tries to change the wire style.
		/// </summary>
		/// <returns><c>true</c>, if change wire style was changed, or <c>false</c> otherwise.</returns>
		/// <param name="wireID">The name of the wires to change.</param>
		/// <param name="style">The style to change to.</param>
		public bool TryChangeWireStyle(string wireID, VisualizerStyle style)
		{
			if (Wires.ContainsKey(wireID))
			{
				ChangeWireStyle(wireID, style);
				return true;
			}
			return false;
		}

		/// <summary>
		/// Sets the styles of all the wires.
		/// </summary>
		/// <param name="style">The style to change to.</param>
		public void SetAllWiresStyle(VisualizerStyle style)
		{
			foreach (var wireList in Wires.Values)
			{
				foreach (var wire in wireList)
				{
					wire.VisualizerStyle = style;
				}
			}
		}
		/// <summary>
		/// Property for a description.
		/// </summary>
		private String description;

		/// <summary>
		/// Occurs when a property changes.
		/// </summary>
		public event PropertyChangedEventHandler PropertyChanged;

		/// <summary>
		/// Method for property changed event.
		/// </summary>
		public void OnPropertyChanged(PropertyChangedEventArgs e)
		{
			PropertyChangedEventHandler handler = PropertyChanged;
			if (handler != null)
				handler(this, e);
		}

		/// <summary>
		/// Method for property change.
		/// </summary>
		public void OnPropertyChanged(string propertyName)
		{
			OnPropertyChanged(new PropertyChangedEventArgs(propertyName));
		}

		/// <summary>
		/// The description of the visualizer.
		/// </summary>
		public string Description
		{
			get { return description; }
			set
			{
				if (value != description)
				{
					description = value;
					OnPropertyChanged(nameof(Description));
				}
			}
		}
	}
}