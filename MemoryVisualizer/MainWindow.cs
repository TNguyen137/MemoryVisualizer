using System;
using System.Collections.Generic;
using System.Linq;
using Gtk;
using System.ComponentModel;
using VisualizerComponents;

/// <summary>
/// The main window of the program. This provides the framework which displays the various visualizers.
/// </summary>
public partial class MainWindow : Gtk.Window
{
	private PluginsComposer m_PluginComposer;
	private List<MemoryVisualizerContainer> m_Visualizers;
	private MemoryVisualizerContainer m_CurrentVisualizer;

	public MainWindow() : base(Gtk.WindowType.Toplevel)
	{
		Build();
		this.PopulateVisualizers();
		this.LoadDefaultVisualizer();
		this.ShowAll();
	}

	#region GTK Window Handlers
	/// <summary>
	/// Event handler for when the user requests to close the main window.
	/// </summary>
	protected void OnDeleteEvent(object sender, DeleteEventArgs a)
	{
		Application.Quit();
		a.RetVal = true;
	}
	#endregion

	#region Menu Action Events
	/// <summary>
	/// Quits the application.
	/// </summary>
	/// <param name="sender">Sender.</param>
	/// <param name="e">E.</param>
	protected void onExit(object sender, EventArgs e)
	{
		Application.Quit();
	}

	/// <summary>
	/// Displays the about application screen.
	/// </summary>
	/// <param name="sender">Sender.</param>
	/// <param name="e">E.</param>
	protected void onAbout(object sender, EventArgs e)
	{
		var about = new AboutDialog();
		about.ProgramName = "Memory Visualizer";
		about.Version = "1.0.0";
		about.Run();
		about.Destroy();
	}

	protected void onSimHelp(object sender, EventArgs e)
	{
		var help = new AboutDialog();
		//help.CreatePangoLayout();
		help.ProgramName = this.m_CurrentVisualizer.CommandLineHelp;
		help.Run();
		help.Destroy();
	}
	#endregion

	#region Visualizer Controls Events
	protected void OnResetClicked(object sender, EventArgs e)
	{
		this.m_CurrentVisualizer.Reset();
	}

	protected void OnStepForwardClicked(object sender, EventArgs e)
	{
		this.m_CurrentVisualizer.StepForward();
	}

	protected void OnStepBackClicked(object sender, EventArgs e)
	{
		this.m_CurrentVisualizer.StepBack();
	}

	// This gets called when the user presses 'Enter' in commandLine_entry.
	/// <summary>
	/// Event handler for when 'Enter' is presed in commandLine_entry.
	/// </summary>
	protected void OnCommandLineEntryActivated(object sender, EventArgs e)
	{
		// HACK: Should not be a dialog box for most messages.
		// INFO: For some reason, GTK MessageDialog will not display the message if the message string contains angled 
		// brackets ('<', '>'). No idea why. Nonetheless, we should move away from a dialog box in the near future.
		var result = this.m_CurrentVisualizer.CommandLineMarshal.HandleCommand(this.commandLine_entry.Text);
		if(result != VisualizerComponents.CommandLine.CommandResult.DefaultSuccess)
		{
			MessageDialog md = new MessageDialog(null, DialogFlags.Modal, MessageType.Error, ButtonsType.Ok, result.Message);
			md.Run();
			md.Destroy();
		}
	}
	#endregion

	#region Visualizer Switching
	/// <summary>
	/// Switches the current visualizer to the specified visualizer.
	/// </summary>
	/// <param name="visualizer">The visualizer to switch to.</param>
	protected void SwitchVisualizer(MemoryVisualizerContainer visualizer)
	{
		if (this.m_CurrentVisualizer != null)
		{
			this.vbox2.Remove(this.m_CurrentVisualizer);
			this.m_CurrentVisualizer.PropertyChanged -= OnVisualizerLabelChange;
		}
		this.m_CurrentVisualizer = visualizer;
		this.m_CurrentVisualizer.PropertyChanged += OnVisualizerLabelChange;
		this.UpdateVisualizerDescription();
		this.vbox2.Add(this.m_CurrentVisualizer);
		this.vbox2.ShowAll();
	}

	/// <summary>
	/// Populates the m_Visualizers list by using the PluginsComposer.
	/// </summary>
	protected void PopulateVisualizers()
	{
		//TODO: Pass in the location of the folder for visualizer from the settings.
		m_PluginComposer = new PluginsComposer();
		m_Visualizers = m_PluginComposer.Visualizers.ToList();
		foreach (var v in m_Visualizers)
		{
			try
			{
				var text = v.VisualizerName;
				if (string.IsNullOrWhiteSpace(text))
				{
					this.visualizerSelector.AppendText(v.GetType().ToString());
				}
				else
					this.visualizerSelector.AppendText(text);
			}
			catch (NotImplementedException)
			{
				var md = new MessageDialog(null, DialogFlags.Modal, MessageType.Error, ButtonsType.Ok,
				    $"{v.GetType().ToString()} does not properly implement the {nameof(MemoryVisualizerContainer)} interface.");
				md.Run();
				md.Destroy();
			}
		}
	}

	/// <summary>
	/// Loads the first visualizer on the m_Visualizers list.
	/// </summary>
	protected void LoadDefaultVisualizer()
	{
		if (m_Visualizers == null)
			PopulateVisualizers();
		if (m_Visualizers.Count == 0) // No visualizers in visualizer folder
		{
			this.DescriptionBox.Text = "No visualizers found.  Please add visualizers and restart the application.";
			this.stepBack.Sensitive = false;
			this.stepForward.Sensitive = false;
			this.reset.Sensitive = false;
			this.play.Sensitive = false;
			this.pause.Sensitive = false;
			this.visualizerSelector.Sensitive = false;
			this.commandLine_entry.Sensitive = false;
			this.menubar1.Sensitive = false;
			return;
		}
		var v = m_Visualizers[0];
		
		if(v != null)
		{
			this.visualizerSelector.Active = 0;
			SwitchVisualizer(v);
		}
	}

	/// <summary>
	/// Handles the visualizerSelector combobox OnChanged event.
	/// </summary>
	protected void OnVisualizerSelectorChanged(object sender, EventArgs e)
	{
		var v = this.m_Visualizers[this.visualizerSelector.Active];
		SwitchVisualizer(v);
	}

	/// <summary>
	/// Updates the Description text on the UI.
	/// </summary>
	protected void UpdateVisualizerDescription()
	{
		if (m_CurrentVisualizer == null)
			DescriptionBox.Text = string.Empty;
		else
			DescriptionBox.Text = m_CurrentVisualizer.Description;
	}

	/// <summary>
	/// Handles the visualizer.OnPropertyChanged event.
	/// </summary>
	protected void OnVisualizerLabelChange(object sender, PropertyChangedEventArgs e)
	{
		if (e.PropertyName == nameof(MemoryVisualizerContainer.Description))
			UpdateVisualizerDescription();
	}
	#endregion
}