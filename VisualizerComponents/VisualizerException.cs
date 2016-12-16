using System;

namespace VisualizerComponents
{
	/// <summary>
	/// Base class for all the exceptions a visualizer should throw. If thrown while using the commandline, the
	/// application will gracefully catch the exception and show a message.
	/// </summary>
	public class VisualizerException : Exception
	{
	}
}