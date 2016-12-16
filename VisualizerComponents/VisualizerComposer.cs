using System;
using System.Collections.Generic;
using System.Linq;

using System.ComponentModel.Composition;
using System.ComponentModel.Composition.Hosting;
using System.Reflection;

namespace VisualizerComponents
{
	public class PluginsComposer
	{
		/*
         * Pretty much this entire class uses things from http://randomactsofcoding.blogspot.com/2009/11/working-with-managed-extensibility.html.
         * If you don't know MEF, go read that tutorial. It's great.
         */

		/// <summary>
		/// Initializes a new instance of the <see cref="T:VisualizerComponents.PluginsComposer"/> class.
		/// </summary>
		/// <param name="PluginsFolder">Plugins folder. Defaults to the 'Visualizers' subdirectory of the current directory.</param>
		public PluginsComposer(string PluginsFolder = @"Visualizers")
		{
			this.PluginsFolder = PluginsFolder;
			this.Compose();
		}

		[ImportMany(typeof(MemoryVisualizerContainer))]
		public IEnumerable<MemoryVisualizerContainer> Visualizers { get; private set; }

		/// <summary>
		/// The path to the folder that the plugins are stored in.
		/// </summary>
		/// <value>The plugins folder.</value>
		public string PluginsFolder { get; private set; }

		private void Compose()
		{
			var catalog = new AggregateCatalog(new DirectoryCatalog(this.PluginsFolder), new AssemblyCatalog(Assembly.GetExecutingAssembly()));
			var container = new CompositionContainer(catalog);
			container.ComposeParts(this);
		}
	}
}