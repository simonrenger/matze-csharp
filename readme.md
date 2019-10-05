# Matze

![CI](https://travis-ci.com/simonrenger/matze-csharp.svg?token=qVi7zNCeA8wTViy22r3s&branch=netcore)

**Matze** is a cross platform Maze Generation library. This library follows three basic principles:

1. The library API should be easy to use
2. The library should be easy to extend
3. The library components do only what they are supposed to do.

The library consist out of two (main) components:

1. the generator: Generated with the algorithms the maze (called grid)
2. the algorithms: different algorithms. (see list below)

Besides those components the library has some utilities to make working with it easier:

- A writer class which allows to write the generated grid to any provided (if provided) stream
- A exporter class which allows to export the generated grid to binary (see specifications below), json or yaml  or txt (see specifications)
- A command line tool to run the library and test it. [More information at its repository](https://github.com/simonrenger/matze-csharp-cli) 

This project supports **.Net Framework 4.x** and was mainly developed with **.netcore 2.2**

### Dependencies

- .Net Core 2.2
- Newtonsoft.Json

## Algorithms

- Recursive Backtracking
- Kruskal's Randomized
- Eller
- Prim

## Example

```csharp
...
int seed = 23123213213;
string file = "grid";
MazeGenerator mazeGenerator = new MazeGenerator(seed);
mazeGenerator.Add<Algorithms.RecursiveBacktracking>();
mazeGenerator.Add<Algorithms.KruskalRandomized>();
mazeGenerator.Add<Algorithms.Eller>();
mazeGenerator.Add<Algorithms.Prim>();
var grid = mazeGenerator.Run<Algorithms.RecursiveBacktracking>(width, height);
Writer.BitsToConsole(grid);
Writer.ToConsole(grid);
Writer.ToDisk(grid, file);
...
var output = Export.ToJSON(grid);
var newGrid = Import.ParseJson(output);
Writer.ToDisk(newGrid, file,Format.Json);
```



## API
