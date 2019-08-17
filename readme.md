# Matze

**Matze** is a cross platform Maze Generation library. This library follows three basic principles:

1. The library API should be easy to use
2. The library should be easy to extend
3. The library components do only what they are supposed to do.

The library consist out of two (main) components:

1. the generator: Generated with the algorithms the maze (called grid)
2. the algorithms: different algorithms. (see list below)

Besides those components the library has some utilities to make working with it easier:

- A writer class which allows to write the generated grid to any provided (if provided) stream
- A command line tool to run the library and test it. [More information at its repository](#) 

This project supports .Net Framework 4.x and was mainly developed with .netcore 2.2

## Algorithms

- Recursive Backtracking
- Kruskal's Randomized
- Eller
- Prim

## Example

```csharp
...
int seed = 23123213213;
MazeGenerator mazeGenerator = new MazeGenerator(seed);
mazeGenerator.Add<Algorithms.RecursiveBacktracking>();
mazeGenerator.Add<Algorithms.KruskalRandomized>();
mazeGenerator.Add<Algorithms.Eller>();
mazeGenerator.Add<Algorithms.Prim>();
var grid = mazeGenerator.Run<Algorithms.RecursiveBacktracking>(width, height);
Writer.BitsToConsole(grid);
Writer.ToConsole(grid);
Writer.ToDisk(grid, file);
```



## API

`namespace Matze`:

```csharp
    class MazeGenerator
    {
        public MazeGenerator();
        public MazeGenerator(int seed);
        public bool Add<T>() where T : Algorithm;
        public BitGrid Run<T>(int width = 10, int height = 10);
    }      
```

*Constructor(s)*

If no seed is provided the current time step is the seed.

`MazeGenerator.Add<T>()`

Added a given algorithm which is driven from the `abstract class Algorithm` to the list of possible Algorithms.

`BitGrid MazeGenerator.Run<T>(int width = 10, int height = 10)`

Will generate a `BitGrid`  based on the provided `T` algorithm with the given size;
