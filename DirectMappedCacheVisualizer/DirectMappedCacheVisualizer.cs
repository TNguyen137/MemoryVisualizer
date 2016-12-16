using System;
using System.Linq;
using System.Collections.Generic;

using VisualizerComponents.CommandLine;

namespace DirectMappedCacheVisualizer
{
	// HACK: This entire visualizer was hacked together.
	// As an example, the arrows you see on the UI are just one giant png file with all the arrows on it. :P
	/// <summary>
	/// A visualizer implementation that demonstrates a direct-mapped cache.
	/// </summary>
	[System.ComponentModel.ToolboxItem(true)]
	public partial class DirectMappedCacheVisualizer : VisualizerComponents.MemoryVisualizerContainer
	{
		public DirectMappedCacheVisualizer()
		{
			this.Build();

			this.CommandLineMarshal.AddCommand("read", ReadCommand);
			this.CommandLineMarshal.AddCommand("write", WriteCommand);

			this.table = new TableRow[4];

			this.table[0] = new TableRow {
				Valid = this.valid0_entry,
				Tag = this.tag0_entry,
				Data = this.data0_entry
			};
			this.table[1] = new TableRow {
				Valid = this.valid1_entry,
				Tag = this.tag1_entry,
				Data = this.data1_entry
			};
			this.table[2] = new TableRow {
				Valid = this.valid2_entry,
				Tag = this.tag2_entry,
				Data = this.data2_entry
			};
			this.table[3] = new TableRow {
				Valid = this.valid3_entry,
				Tag = this.tag3_entry,
				Data = this.data3_entry
			};

			this.table[0].Valid.Text = "FALSE";
			this.table[1].Valid.Text = "TRUE";
			this.table[2].Valid.Text = "TRUE";
			this.table[3].Valid.Text = "FALSE";

			this.table[0].Tag.Text = "000";
			this.table[1].Tag.Text = "111";
			this.table[2].Tag.Text = "001";
			this.table[3].Tag.Text = "111";

			this.table[0].Data.Text = "00000000";
			this.table[1].Data.Text = "11001101";
			this.table[2].Data.Text = "10101010";
			this.table[3].Data.Text = "11111111";

			Description = String.Empty;
		}

		public override string Information
		{
			get {
				return "A simple direct-mapped cache.";
			}
		}

		public override string VisualizerName
		{
			get {
				return "Direct Mapped Cache Visualizer";
			}
		}

		public override string CommandLineHelp
		{
			get {
				return "Commands:\n'read (int address)'\n"
				+ "'write (int address) (int value)'";
			}
		}

		public override void Reset()
		{
			for (int i = 0; i < this.table.Length; i++)
			{
				this.table[i].Valid.ModifyBase(Gtk.StateType.Normal);
				this.table[i].Tag.ModifyBase(Gtk.StateType.Normal);
				this.table[i].Data.ModifyBase(Gtk.StateType.Normal);
			}

			this.tagEqual_widget.DataText = "";
			this.cacheHit_widget.DataText = "";
			this.cacheData_widget.DataText = "";
			Description = "Visualizer Reset";

			this.StepNumber = null;
		}

		/// <summary>
		/// Starts a read operation.
		/// </summary>
		/// <param name="address">The virtual address to read from.</param>
		public void StartRead(int address)
		{
			this.Reset();
			this.Address = address;
			this.Tag = (Address >> 2);
			this.Index = (Address & 0x3);
			this.StepNumber = -1;
			this.StepForward();

			this.Description = "23";
		}

		/// <summary>
		/// Starts a write operation.
		/// </summary>
		/// <param name="address">The virtual address to write to.</param>
		/// <param name="value">The value to write.</param>
		public void StartWrite(int address, int value)
		{
			Reset();
			this.Address = address;
			this.Tag = (Address >> 2);
			this.Index = (Address & 0x3);
			this.StepNumber = null;

			this.memory_address_widget.DataText = Convert.ToString(Address, 2);
			this.tag_widget.DataText = Convert.ToString(Tag, 2);
			this.index_widget.DataText = Convert.ToString(Index, 2);

			this.table[Index].Valid.Text = "TRUE";
			this.table[Index].Tag.Text = Convert.ToString(Tag, 2);
			this.table[Index].Data.Text = Convert.ToString(value, 2);

			this.table[Index].Valid.ModifyBase(Gtk.StateType.Normal, Write);
			this.table[Index].Tag.ModifyBase(Gtk.StateType.Normal, Write);
			this.table[Index].Data.ModifyBase(Gtk.StateType.Normal, Write);
		}

		int? StepNumber;
		int Address;
		int Tag;
		int Index;
		// Apparently if you call Widget.ModifyBase(statetype), it clears the previous background color. *shrug*
		Gdk.Color Highlight = new Gdk.Color(0, 180, 0);
		Gdk.Color Write = new Gdk.Color(0, 150, 150);

		/// <summary>
		/// A convience class which holds information about a row of the table.
		/// </summary>
		private class TableRow
		{
			public Gtk.Entry Valid;
			public Gtk.Entry Tag;
			public Gtk.Entry Data;
		}

		TableRow[] table;

		public override void StepForward()
		{
			if (StepNumber == null)
				return;
			switch (StepNumber)
			{
				case -1:
					// Split the address into tag and index.
					this.memory_address_widget.DataText = Convert.ToString(Address, 2);
					this.tag_widget.DataText = Convert.ToString(Tag, 2);
					this.index_widget.DataText = Convert.ToString(Index, 2);
					this.StepNumber = 0;
					break;
				case 0:
					// Highlight the table row.
					this.table[Index].Valid.ModifyBase(Gtk.StateType.Normal, Highlight);
					this.table[Index].Tag.ModifyBase(Gtk.StateType.Normal, Highlight);
					this.table[Index].Data.ModifyBase(Gtk.StateType.Normal, Highlight);
					this.StepNumber = 1;
					break;
				case 1:
					// Check if tag is equal.
					if (this.table[Index].Tag.Text == this.tag_widget.DataText)
						this.tagEqual_widget.DataText = "TRUE";
					else
						this.tagEqual_widget.DataText = "FALSE";
					this.StepNumber = 2;
					break;
				case 2:
					// Check if cache entry is valid and if tag matches.
					if (this.table[Index].Valid.Text == "TRUE" && this.tagEqual_widget.DataText == "TRUE")
						this.cacheHit_widget.DataText = "TRUE";
					else
						this.cacheHit_widget.DataText = "FALSE";
					this.cacheData_widget.DataText = this.table[Index].Data.Text;
					this.StepNumber = 3;
					break;
				default:
					return;
			}
		}

		public override void StepBack()
		{
			if (StepNumber == null)
				return;
			switch (StepNumber)
			{
				case 3:
					this.cacheHit_widget.DataText = "";
					this.cacheData_widget.DataText = "";
					this.StepNumber = 2;
					break;
				case 2:
					this.tagEqual_widget.DataText = "";
					this.StepNumber = 1;
					break;
				case 1:
					this.table[Index].Valid.ModifyBase(Gtk.StateType.Normal);
					this.table[Index].Tag.ModifyBase(Gtk.StateType.Normal);
					this.table[Index].Data.ModifyBase(Gtk.StateType.Normal);
					this.StepNumber = 0;
					break;
				case 0:
					this.tag_widget.DataText = "";
					this.index_widget.DataText = "";
					this.StepNumber = -1;
					break;
				default:
					return;
			}
		}

		/// <summary>
		/// Runs a read command.
		/// </summary>
		/// <returns>The result of the command.</returns>
		/// <param name="commandName">The name of the command as passed by the user.</param>
		/// <param name="args">Arguments to the command. In this case, it should be just an address to read from.</param>
		/// <seealso cref="StartRead" />
		CommandResult ReadCommand(string commandName, IEnumerable<string> args)
		{
			var myArgs = args.ToList();
			if (myArgs.Count < 1)
			{
				return new CommandResult(false, "Not enough arguments.");
			}

			int address;
			if (!int.TryParse(myArgs[0], out address))
			{
				return new CommandResult(false, "Address is not an integer.");
			}

			this.StartRead(address);

			return CommandResult.DefaultSuccess;
		}

		/// <summary>
		/// Runs a write command.
		/// </summary>
		/// <returns>The result of the command.</returns>
		/// <param name="commandName">The name of the command as passed by the user.</param>
		/// <param name="args">Arguments to the command. In this case, it should be an address to write to and a value to write.</param>
		/// <seealso cref="StartWrite" />
		public CommandResult WriteCommand(string commandName, IEnumerable<string> args)
		{
			var myArgs = args.ToList();
			int address;
			if (!int.TryParse(myArgs[0], out address))
			{
				return new CommandResult(false, "Address is not an integer.");
			}
			int val;
			if (!int.TryParse(myArgs[1], out val))
			{
				return new CommandResult(false, "Data value is not an integer.");
			}

			this.StartWrite(address, val);

			return CommandResult.DefaultSuccess;
		}
	}
}