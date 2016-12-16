using System;
using System.Collections.Generic;
using System.Linq;
using VisualizerComponents.CommandLine;

namespace PagedVirtualMemoryVisualizer
{
	[System.ComponentModel.ToolboxItem(true)]
	public partial class PagedVirtualMemoryVisualizer : VisualizerComponents.MemoryVisualizerContainer
	{
		/// <summary>
		/// The page table.
		/// </summary>
		List<int[]> pageTable;

		/// <summary>
		/// The GUI elements cooresponding to entries in the page table.
		/// </summary>
		List<Gtk.Entry[]> pageTableGUI;

		/// <summary>
		/// The size of the page table.
		/// </summary>
		const int PAGE_TABLE_SIZE = 4;

		/// <summary>
		/// The physical memory.
		/// </summary>
		List<Gtk.Entry> physMemoryArr;

		/// <summary>
		/// The size of the physical memory.
		/// </summary>
		const int PHYS_MEMORY_SIZE = 16;

		/// <summary>
		/// The current virtual page number.
		/// </summary>
		int curVPN;

		/// <summary>
		/// The current offset.
		/// </summary>
		int curOffset;

		/// <summary>
		/// The current physical frame number.
		/// </summary>
		int curPFN;

		/// <summary>
		/// The current state of the visualization.
		/// </summary>
		int stateNumber;

		public PagedVirtualMemoryVisualizer()
		{
			this.Build();
			this.InitWires();

			this.pageTable = new List<int[]>(PAGE_TABLE_SIZE);
			this.pageTableGUI = new List<Gtk.Entry[]>(PAGE_TABLE_SIZE);
			pageTableGUI.Add(new Gtk.Entry[2] { entPageTableVPN, entPageTablePFN });
			pageTableGUI.Add(new Gtk.Entry[2] { entPageTableVPN1, entPageTablePFN1 });
			pageTableGUI.Add(new Gtk.Entry[2] { entPageTableVPN2, entPageTablePFN2 });
			pageTableGUI.Add(new Gtk.Entry[2] { entPageTableVPN3, entPageTablePFN3 });

			this.physMemoryArr = new List<Gtk.Entry>(PHYS_MEMORY_SIZE);
			physMemoryArr.Add(entPhysMem);
			physMemoryArr.Add(entPhysMem1);
			physMemoryArr.Add(entPhysMem2);
			physMemoryArr.Add(entPhysMem3);
			physMemoryArr.Add(entPhysMem4);
			physMemoryArr.Add(entPhysMem5);
			physMemoryArr.Add(entPhysMem6);
			physMemoryArr.Add(entPhysMem7);
			physMemoryArr.Add(entPhysMem8);
			physMemoryArr.Add(entPhysMem9);
			physMemoryArr.Add(entPhysMem10);
			physMemoryArr.Add(entPhysMem11);
			physMemoryArr.Add(entPhysMem12);
			physMemoryArr.Add(entPhysMem13);
			physMemoryArr.Add(entPhysMem14);
			physMemoryArr.Add(entPhysMem15);

			entVirtualAddressVPN.Text = String.Empty;
			stateNumber = 0;
			curVPN = 0;
			curOffset = 0;
			curPFN = 0;
			Description = String.Empty;

			//default entry box values
			pageTable.Add(new int[2] { 101, 00 });
			pageTableGUI[pageTable.Count - 1][0].Text = "101";
			pageTableGUI[pageTable.Count - 1][1].Text = "00";
			physMemoryArr[0].Text = "xDEADBEEF";

			this.CommandLineMarshal.AddCommand("vaddress", this.VAddress);
			this.CommandLineMarshal.AddCommand("pagetable", this.PageTable);
			this.CommandLineMarshal.AddCommand("memory", this.Memory);
		}

		public override string CommandLineHelp
		{
			get {
				return "Instructions: Type in commands (see below) to input values into the visualizer. \n" +
				"This visualizer demonstrates accessing values from physical memory using a virtual address. \n\n" +
				"Example: type the command 'vaddress 10100' and hit enter.  Then step forward twice. \nThis shows" +
				"how to access the value xDEADBEEF from physical\n memory using the virtual address 10100 and " +
				"the corresponding translation in the page table.\n\n" +
				"Commands:\n" +
				"'vaddress (address)' - inserts value into the virtual address area\n" +
				"\tConstraints: at least 4 digit number\n" +
				"'pagetable (VPN) (PFN)' - inserts VPN-PFN pair into page table\n" +
				"\tConstraints: a VPN-PFN pair in the format (vpn) (2 digit pfn)\n" +
				"'memory (address) (value)' - inserts value into physical memory\n" +
				"\tConstraints: a physical memory address and value \n\tin the format (physical memory addres) (value)";
			}
		}

		public override string Information
		{
			get {
				return "A Paged Virtual Memory Visualizer";
			}
		}

		public override string VisualizerName
		{
			get {
				return "Paged Virtual Memory Visualizer";
			}
		}

		public override void Reset()
		{
			this.pageTable = new List<int[]>();
			for (int i = 0; i < PAGE_TABLE_SIZE; i++)
			{
				this.pageTableGUI[i][0].Text = String.Empty;
				this.pageTableGUI[i][1].Text = String.Empty;
			}
			for (int i = 0; i < PHYS_MEMORY_SIZE; i++)
			{
				this.physMemoryArr[i].Text = String.Empty;
			}
			this.entMemoryAccessResult.Text = String.Empty;
			this.entPhysAddressPFN.Text = String.Empty;
			this.entPhysAddressOffset.Text = String.Empty;
			this.entVirtualAddressVPN.Text = String.Empty;
			this.entVirtualAddressOffset.Text = String.Empty;
			this.stateNumber = 0;
			this.SetAllWiresStyle(VisualizerComponents.VisualizerStyle.StandardStyles.WireDefault);
			Description = "Visualizer Reset";

			//default entry box values
			pageTable.Add(new int[2] { 101, 00 });
			pageTableGUI[pageTable.Count - 1][0].Text = "101";
			pageTableGUI[pageTable.Count - 1][1].Text = "00";
			physMemoryArr[0].Text = "xDEADBEEF";
		}

		public override void StepBack()
		{
			switch (stateNumber)
			{
				case 0:
					Description = "Cannot step back.";
					break;

				case 1:
					entPhysAddressPFN.Text = String.Empty;
					entPhysAddressOffset.Text = String.Empty;
					stateNumber--;

					this.SetAllWiresStyle(VisualizerComponents.VisualizerStyle.StandardStyles.WireDefault);

					Description = "Stepped back.";
					break;

				case 2:
					entMemoryAccessResult.Text = String.Empty;
					stateNumber--;

					this.SetAllWiresStyle(VisualizerComponents.VisualizerStyle.StandardStyles.WireDefault);
					this.ChangeWireStyle("state0", VisualizerComponents.VisualizerStyle.StandardStyles.WireHighlighted);
					this.ChangeWireStyle("state0-0", VisualizerComponents.VisualizerStyle.StandardStyles.WireHighlighted);

					Description = "Stepped back.";
					break;

				default:
					Description = "State number error";
					break;
			}
		}

		/*
		 * state:
		 * 0 - Only have the virtual address
		 * 1 - Accessed the page table and have the virtual address and physical address
		 * 2 - Accessed the physical memory and have virtual address, physical addess, and value at the physical address
		 */
		public override void StepForward()
		{
			switch (stateNumber)
			{
				case 0:
					if (entVirtualAddressVPN.Text.Equals(String.Empty))
					{
						Description = "Please enter a value to be used as a virtual address";
						break;
					}

					bool isInPageTable = false;
					for (int i = 0; i < pageTable.Count(); i++)
					{
						if (curVPN == pageTable[i][0])
						{
							curPFN = pageTable[i][1];
							isInPageTable = true;
						}
					}

					if (isInPageTable)
					{
						entPhysAddressPFN.Text = curPFN.ToString();
						entPhysAddressOffset.Text = entVirtualAddressOffset.Text;
						stateNumber++;

						this.SetAllWiresStyle(VisualizerComponents.VisualizerStyle.StandardStyles.WireDefault);
						this.ChangeWireStyle("state0", VisualizerComponents.VisualizerStyle.StandardStyles.WireHighlighted);
						this.ChangeWireStyle("state0-0", VisualizerComponents.VisualizerStyle.StandardStyles.WireHighlighted);

						Description = "Matching VPN-PFN mapping found in the Page Table.";
					}
					else
					{ 
						this.SetAllWiresStyle(VisualizerComponents.VisualizerStyle.StandardStyles.WireDefault);
						this.ChangeWireStyle("state0-0", VisualizerComponents.VisualizerStyle.StandardStyles.WireHighlighted);
						Description = "VPN not found in the Page Table.";
					}
					break;

				case 1:
					try
					{
						int physMemIndex = Convert.ToInt32(entPhysAddressPFN.Text + entPhysAddressOffset.Text, 2);
						entMemoryAccessResult.Text = physMemoryArr[physMemIndex].Text;
					}
					catch (FormatException)
					{
						Description = "Invalid PFN.  Should be a 4 digit binary number from 0000 to 1111"; 
					}
					stateNumber++;

					this.SetAllWiresStyle(VisualizerComponents.VisualizerStyle.StandardStyles.WireDefault);
					this.ChangeWireStyle("step1", VisualizerComponents.VisualizerStyle.StandardStyles.WireHighlighted);

					Description = "Value accessed from physical memory.";
					break;

				case 2:
					this.SetAllWiresStyle(VisualizerComponents.VisualizerStyle.StandardStyles.WireDefault);
					Description = "Cannot step forward.";
					break;

				default:
					Description = "State number error";
					break;
			}
			return;
		}

		/// <summary>
		/// Places a new virtual address for use in the visualizer and clears the currently used information.
		/// </summary>
		/// <returns>The result of the command.</returns>
		/// <param name="commandName">The command name as passed by the user.</param>
		/// <param name="args">Arguments to the command.</param>
		public CommandResult VAddress(string commandName, IEnumerable<string> args)
		{
			var argsList = args.ToList();
			if (argsList.Count() != 1)
			{
				return new CommandResult(false, "Enter a virtual address");
			}
			else if (argsList[0].Length < 4)
			{
				return new CommandResult(false, "Enter a virtual address of at least 4 digits");
			}

			try
			{
				string vpn = argsList[0].Substring(0, argsList[0].Length - 2);
				this.curVPN = Convert.ToInt32(vpn);
				string offset = argsList[0].Substring(argsList[0].Length - 2, 2);
				this.curOffset = Convert.ToInt32(offset);
				this.entVirtualAddressVPN.Text = vpn;
				this.entVirtualAddressOffset.Text = offset;
			}
			catch (FormatException)
			{
				return new CommandResult(false, "Invalid character.  Please enter only numbers");
			}
			catch (OverflowException)
			{
				return new CommandResult(false, "Number too big.");
			}

			if (!entVirtualAddressVPN.Text.Equals(String.Empty))
			{ // if there was already an active virtual address
				this.entPhysAddressPFN.Text = String.Empty;
				this.entPhysAddressOffset.Text = String.Empty;
				this.entMemoryAccessResult.Text = String.Empty;
				this.SetAllWiresStyle(VisualizerComponents.VisualizerStyle.StandardStyles.WireDefault);
				stateNumber = 0;
			}

			Description = "New virtual address inserted.";
			return CommandResult.DefaultSuccess;
		}

		/// <summary>
		/// Inserts a new VPN-PFN mapping in the page table.
		/// </summary>
		/// <returns>The result of the command.</returns>
		/// <param name="commandName">The command name as passed by the user.</param>
		/// <param name="args">Arguments to the command.</param>
		public CommandResult PageTable(string commandName, IEnumerable<string> args)
		{
			var argsList = args.ToList();
			if (argsList.Count != 2)
				return new CommandResult(false, "Please enter a VPN-PFN mapping in the following format: "
				+ "(VPN) (2 digit PFN)");

			if (argsList[1].Length != 2)
				return new CommandResult(false, "The offset should be 2 binary digits.");

			if (pageTable.Count() >= PAGE_TABLE_SIZE)
			{
				return new CommandResult(false, "Cannot add another entry.  The page table is full.");
			}

			try
			{
				pageTable.Add(new int[2] { Convert.ToInt32(argsList[0]), Convert.ToInt32(argsList[1]) });
			}
			catch (FormatException)
			{
				return new CommandResult(false, "Invalid character.  Please enter only numbers");
			}
			catch (OverflowException)
			{
				return new CommandResult(false, "Number too big.");
			}

			pageTableGUI[pageTable.Count - 1][0].Text = argsList[0];
			pageTableGUI[pageTable.Count - 1][1].Text = argsList[1];

			Description = "New VPN-PFN mapping inserted into the Page Table.";
			return CommandResult.DefaultSuccess;
		}

		/// <summary>
		/// Inserts a new value in a specified location in physical memory.
		/// </summary>
		/// <returns>The result of the command.</returns>
		/// <param name="commandName">The command name as passed by the user.</param>
		/// <param name="args">Arguments to the command.</param>
		public CommandResult Memory(string commandName, IEnumerable<string> args)
		{
			var argsList = args.ToList();
			if (argsList.Count != 2)
				return new CommandResult(false, "Please enter a memory address in the format: "
				+ "(memory address) (value).");

			if (argsList[0].Length != 4 && argsList[0].Length != 1 && argsList[0].Length != 2)
				return new CommandResult(false, "Please enter a memory address in the format of either\n"
				+ "1) A decimal number from 0 - 15\n2) A four digit binary number "
				+ "from 0000 to 1111");

			try
			{ // if length 4, then input is in binary, else is in decimal
				int memIndex = argsList[0].Length == 4 ? Convert.ToInt32(argsList[0], 2) : Convert.ToInt32(argsList[0]);
				if (memIndex < 0 || memIndex > 15)
					return new CommandResult(false, "Please enter a memory address from 0 - 15");

				physMemoryArr[memIndex].Text = argsList[1];
			}
			catch (FormatException)
			{
				return new CommandResult(false, "Invalid character.  Please enter only numbers\n" +
				"A 4 digit number must be binary");
			}
			catch (OverflowException)
			{
				return new CommandResult(false, "Number too big.");
			}

			Description = "New value inserted into physical memory.";
			return CommandResult.DefaultSuccess;
		}

	}
}