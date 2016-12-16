using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;

namespace VisualizerComponents.CommandLine
{
	/// <summary>
	/// Contains convience methods for parsing arguments to commands.
	/// </summary>
	public static class ArgumentParser
	{
		/// <summary>
		/// Parses a value.
		/// </summary>
		/// <param name="value">The value to parse.</param>
		/// <typeparam name="T">The type of the value being parsed.</typeparam>
		public static T Parse<T>(string value)
		{
			return (T)TypeDescriptor.GetConverter(typeof(T)).ConvertFromString(value);
		}

		/// <summary>
		/// Attempts to parse a value.
		/// </summary>
		/// <returns><c>true</c>, if parse was successful, or <c>false</c> otherwise.</returns>
		/// <param name="value">The value to parse.</param>
		/// <param name="outVar">The variable to stick the parsed value in.</param>
		/// <typeparam name="T">The type of the value being parsed.</typeparam>
		public static bool TryParse<T>(string value, out T outVar)
		{
			try
			{
				outVar = Parse<T>(value);
				return true;
			}
			catch (Exception)
			{
				outVar = default(T);
				return false;
			}
		}
	}
}