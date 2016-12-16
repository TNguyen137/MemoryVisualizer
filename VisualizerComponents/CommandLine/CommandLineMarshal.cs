using System;
using System.Collections.Generic;
using System.Linq;

namespace VisualizerComponents.CommandLine
{
	/// <summary>
	/// Command delegate for the CommandLineMarshal class.
	/// </summary>
	public delegate CommandResult CommandDelegate(string commandName, IEnumerable<string> args);

	/// <summary>
	/// Manages the commands for a visualizer.
	/// </summary>
	public class CommandLineMarshal
	{
		protected Dictionary<string, CommandDelegate> CommandDictionary = new Dictionary<string, CommandDelegate>();

		/// <summary>
		/// Adds a command. Will throw exception if commandName is already in dictionary.
		/// </summary>
		/// <param name="commandName">Command name.</param>
		/// <param name="commandDelegate">Command delegate.</param>
		public void AddCommand(string commandName, CommandDelegate commandDelegate)
		{
			if(commandDelegate == null)
			{
				throw new ArgumentNullException(nameof(commandDelegate));
			}
			CommandDictionary.Add(commandName, commandDelegate);
		}

		/// <summary>
		/// Removes a command. Will throw exception if the command does not exist.
		/// </summary>
		/// <param name="commandName">Command name.</param>
		public void RemoveCommand(string commandName)
		{
			CommandDictionary.Remove(commandName);
		}

		/// <summary>
		/// Takes a string from the user and parses it to call the correct command.
		/// </summary>
		/// <returns>The result of the command.</returns>
		/// <param name="commandString">The string from the user.</param>
		public CommandResult HandleCommand(string commandString)
		{
			if(CommandDictionary.Count == 0)
			{
				return new CommandResult(false, "No commands registered with this visualizer.");
			}

			if(string.IsNullOrWhiteSpace(commandString))
			{
				return CommandResult.NotRecognized;
			}

			var tokens = commandString.Trim().Split(' ');

			if (CommandDictionary.ContainsKey(tokens[0]))
			{
				try
				{
					// Index by the first token (the command name)
					// Then call the CommandDelegate, passing the commandname and the rest of the arguments.
					return CommandDictionary[tokens[0]](tokens[0], tokens.Skip(1));
				}
				// Only catch exceptions that the visualizer has thrown. Truly exceptional cases should crash the application.
				catch (VisualizerException ex)
				{
					if (string.IsNullOrWhiteSpace(ex.Message))
					{
						return new CommandResult(false, ex.ToString());
					}
					else
					{
						return new CommandResult(false, "Exception: " + ex.Message);
					}
				}
			}

			// Should never reach this.
			return CommandResult.NotRecognized;
		}
	}
}