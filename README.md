[![Build status](https://ci.appveyor.com/api/projects/status/wu1y7il3kecc9n9l/branch/master?svg=true)](https://ci.appveyor.com/project/mwoelk83/mono-cecil-fluent/branch/master)
# Mono.Cecil.Fluent
Fluent API for Mono.Cecil. **Currently under developement.** This project is free and open source. Everyone is invited to contribute, fork, share and use the code.

## Example

````csharp
var method = type.NewMethod("addorsub")
    .WithParameter<int>("a")
    .WithParameter<int>("b")
    .WithParameter<bool>("add")
    .Returns<int>()
    .Ldarg("add")
    .IfTrue()
      .Ldarg("a")
      .Ldarg("b")
      .Add()
    .Else()
      .Ldarg("a")
      .Ldarg("b")
      .Sub()
    .Endif()
    .Ret(); 
````

## Getting the code

To get the source code:

````shell
git clone https://github.com/mwoelk83/Mono.Cecil.Fluent.git
````

## License

This program is free software: you can redistribute it and/or modify it under the terms of the GNU General Public License as published by the Free Software Foundation, either version 3 of the License, or (at your option) any later version. See [LICENSE.txt](https://github.com/mwoelk83/Mono.Cecil.Fluent/blob/master/License.txt) for details. Check out the terms of the license before you contribute, fork, copy or do anything with the code. If you decide to contribute you agree to grant copyright of all your contribution to this project. Your work will be licensed with the project at GPL version 3, along the rest of the code.
