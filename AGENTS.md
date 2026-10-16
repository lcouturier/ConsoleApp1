# Project Guidelines

## Project Structure

- `ConsoleApp1/` contains the .NET 8 console application and reusable types in the `ConsoleApp1` namespace.
- `ConsoleApp1.Tests/` contains the xUnit test project and references the application project.
- `ConsoleApp1.sln` currently includes only the application project. Do not assume solution-level test commands discover `ConsoleApp1.Tests`.
- Ignore generated content under `bin/` and `obj/`; never edit it manually.

## Code Style

- Keep nullable reference types enabled and address nullability warnings instead of suppressing them without justification.
- Follow the style of the nearest file. New production types should normally use the file-scoped `ConsoleApp1` namespace.
- Prefer small, focused methods and preserve the public extension-method APIs unless a task explicitly requires a breaking change.
- Preserve deferred execution for `IEnumerable<T>` extensions and `Func<T>` pipelines. Avoid eager materialization unless required by the behavior.
- Use descriptive identifiers; avoid unrelated cleanup or formatting changes.

## Tests

- Add or update xUnit tests for every behavioral change.
- Place tests in `ConsoleApp1.Tests/`, using the `ConsoleApp1.Tests` namespace and `[Fact]` for single-case tests.
- Name tests after the behavior and scenario, following examples such as `Into_Func_DefersEvaluation` and `SeparatedBy_EmptySequence_ReturnsEmpty`.
- Cover empty inputs, null/default values, deferred enumeration, and repeated invocation when those cases are relevant.

## Build and Validation

Run commands from the repository root:

```sh
dotnet restore ConsoleApp1.Tests/ConsoleApp1.Tests.csproj
dotnet build ConsoleApp1.Tests/ConsoleApp1.Tests.csproj --no-restore
dotnet test ConsoleApp1.Tests/ConsoleApp1.Tests.csproj --no-build
```

Use `dotnet run --project ConsoleApp1/ConsoleApp1.csproj` when a change affects console output or application startup. Before finishing, run the narrowest relevant test first, then the full test project for shared library changes.