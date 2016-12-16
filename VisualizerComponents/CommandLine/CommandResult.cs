using System;
namespace VisualizerComponents.CommandLine
{
	public class CommandResult
	{
		/// <summary>
		/// The standard success result. Use if everything went well and there is no message to the user.
		/// </summary>
		public static CommandResult DefaultSuccess = new CommandResult(true, string.Empty);
		public static CommandResult NotRecognized = new CommandResult(false, "Command not recognized.");

		/// <summary>
		/// Initializes a new instance of the <see cref="T:VisualizerComponents.CommandLine.CommandResult"/> class.
		/// </summary>
		/// <param name="success">Set to <c>true</c> if the command was successful.</param>
		/// <param name="message">A message to the user (success or failure). This should be set if the command failed.</param>
		public CommandResult(bool success, string message)
		{
			Success = success;
			Message = message;
		}

		/// <summary>
		/// Gets a value indicating whether this <see cref="T:VisualizerComponents.CommandLine.CommandResult"/> is success.
		/// </summary>
		/// <value><c>true</c> if success; otherwise, <c>false</c>.</value>
		public bool Success { get; }
		/// <summary>
		/// Gets the message that should be displayed to the user.
		/// </summary>
		public string Message { get; }
	}
}