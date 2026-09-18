# Part G: Short Answers

## 1. .csproj contents

<!-- IMPORTANT: replace this with the real content of YOUR .csproj file -->

```xml
<Project Sdk="Microsoft.NET.Sdk">

  <PropertyGroup>
    <OutputType>Exe</OutputType>
    <TargetFramework>net9.0</TargetFramework>
    <ImplicitUsings>enable</ImplicitUsings>
    <Nullable>enable</Nullable>
  </PropertyGroup>

</Project>
```

All four properties are present:

- **OutputType** = `Exe`: the project builds a runnable console app (not a library).
- **TargetFramework** = `net9.0`: the project is compiled for .NET 9.
- **ImplicitUsings** = `enable`: common `using` lines (like `System`) are added automatically, so `Console.WriteLine` works without `using System;`.
- **Nullable** = `enable`: nullable reference types are on, so the compiler warns about possible null problems.

## 2. Do #region / #endregion change the compiled output?

No. They are only for the editor. The compiler ignores them, so the compiled program is exactly the same with or without them.

I can still use them to group related code (for example all the helper methods) and collapse it in Visual Studio, so a long file is easier to read and navigate. But if I need many regions to understand a file, the file is probably too big and should be split into smaller classes.

## 3. When to use /// XML doc comments instead of //

I use `///` when I document something that other code will use: a public class, a method, its parameters and its return value. Visual Studio reads them and shows the text in IntelliSense when someone hovers over or calls the method, and tools can generate documentation from them. The compiler can also warn when a parameter is missing from the docs.

```csharp
/// <summary>Finds the number that appears only once in the array.</summary>
/// <param name="nums">Array where every number appears twice except one.</param>
/// <returns>The single number.</returns>
static int FindSingleNumber(int[] nums) { ... }
```

I use a plain `//` for notes inside the code, like explaining why a line is written in a certain way. Those are only for people reading the source, not for people using the method.
