MemVis2
=======

MemVis2 is a visualizer system designed for displaying models of computer systems. It is primarily designed for visualizing cache memory systems, especially those taught in the CS-2200 course at Georgia Tech, but is capable of many different types of visualizations.

Release Notes
-------------
### v1.0

#### Features
* Visually step through complex data processes.
* Dynamic loading of each visualizer, allowing new visualizers to be added later with ease.
* Write your own visualizers and load them
* Visualizers:
	* Direct Mapped Cache
	* Paged Virtual Memory
	* Stack (Data structure)

#### Known Bugs
* If a visualizer generates an exception during the constructor, the application will crash. Fixing this will likely involve modifying the PluginsComposer class.

Installation
------------

### Prerequisites

* GTK# ([Windows Instructions][gtk#-windows], Included with Mono on Mac and Linux)
* .NET runtime (Either Mono or Microsoft .NET, Depending on OS)

For compilation, the following are required:

* [MonoDevelop][monodevelop] (Called Xamarin Studio on Windows)

### Steps

Once the prerequisites are installed, simply place `MemoryVisualizer.exe` wherever you would like. `VisualizerComponents.dll` should be placed in the same directory as `MemoryVisualizer.exe`. The visualizer DLLs should be placed in a subdirectory of the directory `MemoryVisualizer.exe` is in, and that directory should be called `Visualizers`.

When finished, you should have something like the following directory structure:

```
.
├── MemoryVisualizer.exe
├── VisualizerComponents.dll
└── Visualizers
    ├── DirectMappedCacheVisualizer.dll
    ├── DummyVisualizer.dll
    ├── PagedVirtualMemoryVisualizer.dll
    ├── StackVisualizer.dll
    └── VisualizerComponents.dll
```

### Usage

#### Windows
Simply double-click `MemoryVisualizer.exe` to run the program.

#### Linux
Run the following command:
    $ mono MemoryVisualizer.exe

Authors
-------

* Kevin Gibby
* Thi Nguyen
* Yissakhar Z. Beck
* Justin Thornburgh
* JT Vinson

[gtk#-windows]: http://www.mono-project.com/docs/gui/gtksharp/installer-for-net-framework/
[monodevelop]: http://www.monodevelop.com/
