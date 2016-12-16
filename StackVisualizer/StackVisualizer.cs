using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using VisualizerComponents.CommandLine;

namespace StackVisualizer
{
	[System.ComponentModel.ToolboxItem(true)]
	public partial class StackVisualizer : VisualizerComponents.MemoryVisualizerContainer
	{
		/// <summary>
		/// The current stack.
		/// </summary>
		Stack thisStack;

		/// <summary>
		/// The previous stack.
		/// </summary>
		Stack stepBackStack;

		/// <summary>
		/// The step modes.
		/// </summary>
		List<bool> stepModes; // true - add; false - remove

		/// <summary>
		/// The step number.
		/// </summary>
		int stepNumber;

		/// <summary>
		/// The GUI elementes cooresponding to the entries in the stack.
		/// </summary>
		Gtk.Entry[] entries;

		/// <summary>
		/// The number of entries.
		/// </summary>
		const int NUM_ENTRIES = 6;

		public StackVisualizer()
		{
			Build();
			this.thisStack = new Stack();
			this.stepBackStack = new Stack();
			this.stepModes = new List<Boolean>();
			this.stepNumber = 0;
			// add entries to an array to be accessed by index
			this.entries = new Gtk.Entry[NUM_ENTRIES];
			entries[0] = this.entryzero;
			entries[1] = this.entry1;
			entries[2] = this.entry2;
			entries[3] = this.entry3;
			entries[4] = this.entry4;
			entries[5] = this.entry5;
			for (int i = 0; i < NUM_ENTRIES; i++) {
				this.entries[i].IsEditable = false;
			}
			Description = String.Empty;

			this.CommandLineMarshal.AddCommand("push", this.Push);
			this.CommandLineMarshal.AddCommand("pop", this.Pop);
		}

		public override string Information
		{
			get
			{
				return "A Simple Stack Visualizer.";
			}
		}

		public override string VisualizerName
		{
			get
			{
				return "Stack Visualizer";
			}
		}

		public override string CommandLineHelp
		{
			get
			{
				return "Commands:\n" +
					"'pop' - Pop from the stack\n"
					+ "'push (value)' - Push a value to the stack";
			}
		}

		public override void Reset()
		{
			this.thisStack = new Stack();
			this.stepBackStack = new Stack();
			this.stepModes = new List<Boolean>();
			this.stepNumber = 0;
			for (int i = 0; i < NUM_ENTRIES; i++) {
				this.entries[i].Text = String.Empty;
			}
			Description = "Visualizer Reset";
		}

		public override void StepForward()
		{
			Description = "Stepping Forward is disabled for this visualizer.";
			return;
		}

		public override void StepBack()
		{
			if (stepNumber == 0) {
				Description = "Cannot step back.";
				return;
			}
			if (stepModes[stepNumber - 1]) { // if last step was an add step
				this.thisStack.Pop();
				this.entries[this.thisStack.Count].Text = String.Empty;
			}
			else { // last step was a remove step
				this.thisStack.Push(stepBackStack.Pop());
				this.entries[this.thisStack.Count - 1].Text = this.thisStack.Peek().ToString();
			}
			Description = String.Empty;
			this.stepNumber--;
		}

		/// <summary>
		/// Runs a pop command, which pops the top element off of the stack.
		/// </summary>
		/// <param name="commandname">The name of the command as passed by the user.</param>
		/// <param name="args">Arguments to the command.</param>
		public CommandResult Pop(string commandname, IEnumerable<string> args)
		{
			var argsList = args.ToList();
			if (argsList.Count != 0)
				return new CommandResult(false, "The pop command does not take any arguments.");

			if (this.thisStack.Count == 0)
			{
				Description = "Stack is empty.  Cannot remove more elements.";
				return CommandResult.DefaultSuccess;
			}

			//  clear value from correct entry box in the GUI and pop from the stack
			this.stepBackStack.Push(this.thisStack.Pop());
			this.entries[this.thisStack.Count].Text = String.Empty;
			Description = String.Format("Element removed from index {0}.", this.thisStack.Count);
			this.stepModes.Insert(stepNumber, false);
			this.stepNumber++;

			return CommandResult.DefaultSuccess;
		}

		/// <summary>
		/// Runs a push command, which pushes an element onto the stack.
		/// </summary>
		/// <param name="commandname">The name of the command as passed by the user.</param>
		/// <param name="args">Arguments to the command.</param>
		public CommandResult Push(string commandName, IEnumerable<string> args)
		{
			var argsList = args.ToList();
			if(argsList.Count != 1)
				return new CommandResult(false, "The push command takes exactly one argument.");

			if (this.thisStack.Count >= 6)
			{
				Description = "Stack is full.  Cannot add more elements.";
				return CommandResult.DefaultSuccess;;
			}
			//  display the value in the correct entry box in the GUI and push to the stack
			this.entries[this.thisStack.Count].Text = argsList[0].ToString();
			this.thisStack.Push(argsList[0]);
			Description = String.Format("Element added at index {0}.", this.thisStack.Count - 1);
			this.stepModes.Insert(stepNumber, true);
			this.stepNumber++;

			return CommandResult.DefaultSuccess;
		}

	}
}