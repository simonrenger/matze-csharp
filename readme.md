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
- A exporter class which allows to export the generated grid to binary (see specifications below), json or yaml or toml or txt (see specifications)
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

### `namespace Matze`:

```csharp
    class MazeGenerator
    {
        public MazeGenerator();
        public MazeGenerator(int seed);
        public bool Add<T>() where T : Algorithm;
        public bool Remove<T>() where T : Algorithm;
        public BitGrid Run<T>(int width = 10, int height = 10);
    }      
```

*Constructor(s)*

If no seed is provided the current time step is the seed.

`MazeGenerator.Add<T>()`

Added a given algorithm which is driven from the `abstract class Algorithm` to the list of possible Algorithms.

`MazeGenerator.Remove<T>()`

Removes a provided algorithm from the Directory. 

`BitGrid MazeGenerator.Run<T>(int width = 10, int height = 10)`

Will generate a `BitGrid`  based on the provided `T` algorithm with the given size;

***



### `namespace Matze.Algorithms`

The home of all the supported algorithms. All algorithms **must** inherit from the `abstract class Algorithm` . In order to guarantee the same Interface all Algorithms must provide the `static method`:

```csharp
BitGrid Generate(Random rand, int width, int hight);
```

> ***Side Note:*** If you want to extend the library with your own Algorithm just inherit from `Algorithm` and provide a static `BitGrid Generate(Random rand, int width, int hight);` method and the Maze Generator class understand it and can use it.

```csharp
    abstract class Algorithm
    {
        protected static Dictionary<Directions, int[]> directions;
        protected static Dictionary<Directions, Directions> oppositeDirections;
        protected static int E => (int)Directions.E;
        protected static int S => (int)Directions.S;
        protected static int N => (int)Directions.N;
        protected static int W => (int)Directions.W;
    }
```

`Dictionary<Directions, int[]> directions`

Contains the *Key* *Value* pairs of the translation of the Directions Enum to integers.

`Dictionary<Directions, Directions> oppositeDirections`

Contains the opposite direction of the given direction.

*Properties*

The properties are just there for connivance.

### Algorithms

- `Algorithms.RecursiveBacktracking`
- `Algorithms.KruskalRandomized`
- `Algorithms.Eller`
- `Algorithms.Prim`



***



### `namespace Matze.Utils`

```csharp
 class BitGrid
 {
    public BitGrid(int width, int height);
    public BitGrid(List<List<int>> grid);
    public BitGrid(byte[][] bytes);
    public BitGrid(string source);
    public List<int> this[int index];
    public byte[][] ToBytes();
    public string ToString(bool newLine = true);
    public int Width;
    public int Height;
 }
```

*Constructor(s)*

`public BitGrid(int width, int height);`

creates a grid with the provided size (width and height). The base value of all cells is 0.

`public BitGrid(List<List<int>> grid)`

Sets the given grid as the grid.

> **Warning:** No check will be done.

`public BitGrid(byte[][] bytes)`

Generates a grid based on the provided bytes.

> **Warning:** If any of the provided bytes does not match any numeric value the generator could produce an Exception will be thrown.

`public BitGrid(string source)`

Will generate a grid based on the provided string. (see `ToString()` method for specification).

> **Warning:** If any number in the string does not match any numeric value the generator could produce an Exception will be thrown.

`BitGrid.[int index]`

Returns the underlying list element which allows easy access to the grid.

*example:*

```csharp
var hasNoWallNorth = (grid[y][x] & (int)Directions.N) != 0;
```

*Properties*

`BitGrid.Width` 

Returns the size of the underlying Width List. (This is equal to the `width` variable provided in the constructor)

`BitGrid.Height` 

Returns the size of the underlying Width List. (This is equal to the `height` variable provided in the constructor)

*Conversation Methods*

`BitGrid.ToBytes`

Exports the grid to a byte array.

`BitGrid.ToString(bool newLine = true)`

Will export the grid to an string in the format: 

```
[number];[number];[number]\n
[number];[number];[number]\n
[number];[number];[number]\n
```

Unless `newLine` is specified as `false` in that case the `\n` will be replaced with `|`

``` 
[number];[number];[number]|[number];[number];[number]|[number];[number];[number]|
```



```csharp
class Writer
{
    public static void BitsToConsole(BitGrid grid);
    public static void ToConsole(BitGrid grid);
    public static void WriteGridTo(BitGrid grid, Stream stream);
    public static void WriteBitsTo(BitGrid grid, Stream stream);
    public static void ToDisk(BitGrid grid, string file = "grid.txt");
}
```

`Writer.BitsToConsole`

Writes the bits of the grid to the `System.Console.OpenStandardOutput()` stream. (Calls`Writer.WriteBitsTo`)

`Writer.ToConsole`

Writes grid in ASCII representation to the `System.Console.OpenStandardOutput()` stream. (Calls`Writer.WriteGridTo`)

`Writer.WriteGridTo`

Writes grid in ASCII representation to the provided stream.

`Writer.WriteBitsTo`

Writes the bits of the grid to the provided stream.

`Writer.ToDisk`

Writes the bits and the ASCII representation to the disk.
