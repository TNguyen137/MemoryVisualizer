using System;
using VisualizerComponents;
using System.Collections.Generic;
using System.Linq;

namespace DummyVisualizer
{
	/// <summary>
	/// Dummy visualizer for testing.
	/// </summary>
	[System.ComponentModel.ToolboxItem(true)]
	public partial class DummyVisualizer : VisualizerComponents.MemoryVisualizerContainer
	{
		public DummyVisualizer()
		{
			this.Build();
			this.InitWires();
			this.CommandLineMarshal.AddCommand("read", this.ReadFunc);
			this.CommandLineMarshal.AddCommand("write", this.WriteFunc);
		}

		public override string Information
		{
			get {
				return "This is a test visualizer.";
			}
		}

		public override string VisualizerName
		{
			get {
				return "Dummy Visualizer";
			}
		}

		public override string CommandLineHelp
		{
			get {
				return "read (int address) (int putWhere)\n"
				+ "write (int address) (int value)";
			}
		}

		public override void Reset()
		{
			this.StepNumber = null;
			this.Read = null;
			this.Address = null;
			this.PutWhere_Value = null;

			this.read_write_memoryitemwidget.DataText = string.Empty;
			this.putWhere_value_memoryitemwidget.LabelText = string.Empty;

			this.addr_init_memoryitemwidget.DataText = string.Empty;
			this.putWhere_value_memoryitemwidget.DataText = string.Empty;
			this.addr_step1_memoryitemwidget.DataText = string.Empty;
			this.putWhere_step1_memoryitemwidget.DataText = string.Empty;
			this.addr_step2_memoryitemwidget.DataText = string.Empty;
			this.putWhere_step2_memoryitemwidget.DataText = string.Empty;
		}

		/// <summary>
		/// Gets or sets the step number.
		/// </summary>
		/// <value>The step number.</value>
		protected int? StepNumber { get; set; } = -2;

		/// <summary>
		/// If true, the visualizer is reading. Otherwise, it's writing.
		/// </summary>
		protected bool? Read = null;

		/// <summary>
		/// The address being read from or written to.
		/// </summary>
		protected int? Address = null;

		/// <summary>
		/// The value being written, if applicable.
		/// </summary>
		protected int? PutWhere_Value = null;

		public override void StepForward()
		{
			if (this.Read == null)
				return;
			switch (this.StepNumber)
			{
				case -1:
					this.addr_init_memoryitemwidget.DataText = this.Address.ToString();
					this.addr_init_memoryitemwidget.VisualizerStyle = VisualizerComponents.VisualizerStyle.StandardStyles.Highlighted;
					this.putWhere_value_memoryitemwidget.DataText = this.PutWhere_Value.ToString();
					this.putWhere_value_memoryitemwidget.VisualizerStyle = VisualizerComponents.VisualizerStyle.StandardStyles.Highlighted;

					this.SetAllWiresStyle(VisualizerComponents.VisualizerStyle.StandardStyles.WireDefault);
					this.ChangeWireStyle("step-1", VisualizerComponents.VisualizerStyle.StandardStyles.WireHighlighted);

					this.StepNumber = 0;
					break;
				case 0:
					this.addr_init_memoryitemwidget.VisualizerStyle = VisualizerComponents.VisualizerStyle.StandardStyles.DefaultStyle;
					this.putWhere_value_memoryitemwidget.VisualizerStyle = VisualizerComponents.VisualizerStyle.StandardStyles.DefaultStyle;

					this.SetAllWiresStyle(VisualizerComponents.VisualizerStyle.StandardStyles.WireDefault);
					this.ChangeWireStyle("step0", VisualizerComponents.VisualizerStyle.StandardStyles.WireHighlighted);

					this.addr_step1_memoryitemwidget.DataText = this.Address.ToString();
					this.addr_step1_memoryitemwidget.VisualizerStyle = VisualizerComponents.VisualizerStyle.StandardStyles.Highlighted;
					this.putWhere_step1_memoryitemwidget.DataText = this.PutWhere_Value.ToString();
					this.putWhere_step1_memoryitemwidget.VisualizerStyle = VisualizerComponents.VisualizerStyle.StandardStyles.Highlighted;
					this.StepNumber = 1;
					break;
				case 1:
					this.addr_step1_memoryitemwidget.VisualizerStyle = VisualizerComponents.VisualizerStyle.StandardStyles.DefaultStyle;
					this.putWhere_step1_memoryitemwidget.VisualizerStyle = VisualizerComponents.VisualizerStyle.StandardStyles.DefaultStyle;

					this.SetAllWiresStyle(VisualizerComponents.VisualizerStyle.StandardStyles.WireDefault);
					this.ChangeWireStyle("step1", VisualizerComponents.VisualizerStyle.StandardStyles.WireHighlighted);

					this.addr_step2_memoryitemwidget.DataText = this.Address.ToString();
					this.addr_step2_memoryitemwidget.VisualizerStyle = VisualizerComponents.VisualizerStyle.StandardStyles.Highlighted;
					this.putWhere_step2_memoryitemwidget.DataText = this.PutWhere_Value.ToString();
					this.putWhere_step2_memoryitemwidget.VisualizerStyle = VisualizerComponents.VisualizerStyle.StandardStyles.Highlighted;
					this.StepNumber = 2;
					break;
				default:
					return;
			}
		}

		public override void StepBack()
		{
			if (this.Read == null)
				return;
			switch (this.StepNumber)
			{
				case 2:
					this.addr_step1_memoryitemwidget.VisualizerStyle = VisualizerComponents.VisualizerStyle.StandardStyles.Highlighted;
					this.putWhere_step1_memoryitemwidget.VisualizerStyle = VisualizerComponents.VisualizerStyle.StandardStyles.Highlighted;

					this.SetAllWiresStyle(VisualizerComponents.VisualizerStyle.StandardStyles.WireDefault);
					this.ChangeWireStyle("step0", VisualizerComponents.VisualizerStyle.StandardStyles.WireHighlighted);

					this.addr_step2_memoryitemwidget.DataText = string.Empty;
					this.addr_step2_memoryitemwidget.VisualizerStyle = VisualizerComponents.VisualizerStyle.StandardStyles.DefaultStyle;
					this.putWhere_step2_memoryitemwidget.DataText = string.Empty;
					this.putWhere_step2_memoryitemwidget.VisualizerStyle = VisualizerComponents.VisualizerStyle.StandardStyles.DefaultStyle;
					this.StepNumber = 1;
					break;
				case 1:
					this.addr_init_memoryitemwidget.VisualizerStyle = VisualizerComponents.VisualizerStyle.StandardStyles.Highlighted;
					this.putWhere_value_memoryitemwidget.VisualizerStyle = VisualizerComponents.VisualizerStyle.StandardStyles.Highlighted;

					this.SetAllWiresStyle(VisualizerComponents.VisualizerStyle.StandardStyles.WireDefault);
					this.ChangeWireStyle("step-1", VisualizerComponents.VisualizerStyle.StandardStyles.WireHighlighted);

					this.addr_step1_memoryitemwidget.DataText = string.Empty;
					this.addr_step1_memoryitemwidget.VisualizerStyle = VisualizerComponents.VisualizerStyle.StandardStyles.DefaultStyle;
					this.putWhere_step1_memoryitemwidget.DataText = string.Empty;
					this.putWhere_step1_memoryitemwidget.VisualizerStyle = VisualizerComponents.VisualizerStyle.StandardStyles.DefaultStyle;
					this.StepNumber = 0;
					break;
				case 0:
					this.addr_init_memoryitemwidget.DataText = string.Empty;
					this.addr_init_memoryitemwidget.VisualizerStyle = VisualizerComponents.VisualizerStyle.StandardStyles.DefaultStyle;
					this.putWhere_value_memoryitemwidget.DataText = string.Empty;
					this.putWhere_value_memoryitemwidget.VisualizerStyle = VisualizerComponents.VisualizerStyle.StandardStyles.Highlighted;

					this.SetAllWiresStyle(VisualizerComponents.VisualizerStyle.StandardStyles.WireDefault);
					//this.ChangeWireStyle("step-1", VisualizerComponents.VisualizerStyle.StandardStyles.WireHighlighted);

					this.StepNumber = -1;
					break;
				case -1:
				default:
					return;
			}
		}

		/// <summary>
		/// Starts a read.
		/// </summary>
		/// <param name="address">The address to read from.</param>
		/// <param name="putWhere">The value to write. It's not used. Ignore it.</param>
		public void StartRead(int address, int putWhere)
		{
			this.Reset();
			this.Read = true;
			this.Address = address;
			this.PutWhere_Value = putWhere;
			this.StepNumber = -1;
			this.read_write_memoryitemwidget.DataText = "Read";
			this.putWhere_value_memoryitemwidget.LabelText = "Put Where";
			this.StepForward();
		}

		/// <summary>
		/// Starts a write.
		/// </summary>
		/// <param name="address">The address to write to.</param>
		/// <param name="value">The value to write.</param>
		public void StartWrite(int address, int value)
		{
			this.Reset();
			this.Read = false;
			this.Address = address;
			this.PutWhere_Value = value;
			this.StepNumber = -1;
			this.read_write_memoryitemwidget.DataText = "Write";
			this.putWhere_value_memoryitemwidget.LabelText = "Data";
			this.StepForward();
		}

		/// <summary>
		/// Runs a read command.
		/// </summary>
		/// <returns>The command result.</returns>
		/// <param name="commandName">The name of the command as passed by the user.</param>
		/// <param name="args">Arguments to the command.</param>
		public VisualizerComponents.CommandLine.CommandResult ReadFunc(string commandName, IEnumerable<string> args)
		{
			var myArgs = args.ToList();
			int address;
			if (myArgs.Count < 2)
				return new VisualizerComponents.CommandLine.CommandResult(false, "Not enough arguments.");
			if (!int.TryParse(myArgs[0], out address))
			{
				return new VisualizerComponents.CommandLine.CommandResult(false, "Address is not an integer.");
			}
			int putWhere;
			if (!int.TryParse(myArgs[1], out putWhere))
			{
				return new VisualizerComponents.CommandLine.CommandResult(false, "PutWhere is not an integer.");
			}

			this.StartRead(address, putWhere);

			return VisualizerComponents.CommandLine.CommandResult.DefaultSuccess;
		}

		/// <summary>
		/// Runs a write command.
		/// </summary>
		/// <returns>The result of the command.</returns>
		/// <param name="commandName">The name of the command as passed by the user.</param>
		/// <param name="args">Arguments to the command.</param>
		public VisualizerComponents.CommandLine.CommandResult WriteFunc(string commandName, IEnumerable<string> args)
		{
			var myArgs = args.ToList();
			int address;
			if (!int.TryParse(myArgs[0], out address))
			{
				return new VisualizerComponents.CommandLine.CommandResult(false, "Address is not an integer.");
			}
			int val;
			if (!int.TryParse(myArgs[1], out val))
			{
				return new VisualizerComponents.CommandLine.CommandResult(false, "Data value is not an integer.");
			}

			this.StartWrite(address, val);

			return VisualizerComponents.CommandLine.CommandResult.DefaultSuccess;
		}
	}
}