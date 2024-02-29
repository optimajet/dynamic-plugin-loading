# Workflow Engine: Dynamic Plugin Loading

In this repository, there is a project integrated with the [WorkflowEngine](https://workflowengine.io/) 
and featuring the implementation of dynamic plugin loading. This code serves as an example of 
following the tutorial outlined in the [documentation](https://workflowengine.io/documentation/dynamic-plugin-loading).

In this tutorial, we'll implement dynamic plugin loading. This allows you to customize the Workflow Engine
by placing compiled DLLs with plugins into the *plugins* folder. Plugins are classes that implement `IWorkflowPlugin`,
enabling the engine to incorporate new Actions, Rules, ect. Learn more about [plugins](https://workflowengine.io/documentation/plugins).

### Motivation

You might be interested in this functionality for a variety of reasons:

- You distribute your software with a Workflow Engine to your clients and want to provide them with customization
  options without needing access to the source code.
- You want to separate dependencies used in the project with the Workflow Engine from those used for plugins.
- You manage a large number of plugins and want to simplify their modification or installation.

### Environment Requirements

- Application with integrated Workflow Engine.
- IDE for working with C# code, such as [Visual Studio](https://visualstudio.microsoft.com/).

## Tutorial

In this tutorial, we'll step through implementing dynamic plugin loading, which requires:

1. Writing a `PluginLoader` class for dynamically loading assemblies.
2. Adding an extension method for loading plugins into the `WorkflowRuntime`.
3. Creating a new project with plugin implementation and exporting its DLL.
4. Testing and ensuring everything works well.

### PluginLoader

This class will inherit from the System class `AssemblyLoadContext`, aiming to correctly load DLLs and resolve
their dependencies at the specified path.

[Go to the code >PluginLoader.cs](https://github.com/optimajet/dynamic-plugin-loading/blob/master/DynamicPluginLoading/PluginLoader.cs)

### Extension Method

The extension method allows us to add dynamic loading of plugins into the Workflow Engine Runtime
initialization pipeline alongside other settings.

[Go to the code >Extensions.cs](https://github.com/optimajet/dynamic-plugin-loading/blob/master/DynamicPluginLoading/Extensions.cs)

### Project with Plugin

To test the functionality, we need to create a test project from which we'll import plugins into the Workflow Engine.
This requires several steps:

1. Create a new project `MyPlugin`.
   ```bash
   dotnet new classlib --name MyPlugin
   dotnet sln add MyPlugin
   rm MyPlugin\Class1.cs
   ```
2. Add a package reference to `WorkflowEngine.NETCore-Core` to implement `IWorkflowPlugin`.
   ```bash
   dotnet add MyPlugin package WorkflowEngine.NETCore-Core
   ```
3. Add a new class `MyPlugin` implementing the `IWorkflowPlugin` interface. 
[Go to the code >MyPlugin.cs](https://github.com/optimajet/dynamic-plugin-loading/blob/master/MyPlugin/MyPlugin.cs)
4. Build the project and copy its DLL to the `plugins/{plugin_name}/…` folder.
   ```bash
   dotnet build
   mkdir -p DynamicPluginLoading/bin/Debug/net8.0/plugins/MyPlugin
   cp -r MyPlugin/bin/Debug/net8.0/* DynamicPluginLoading/bin/Debug/net8.0/plugins/MyPlugin/
   ```

### Testing

Finally, we have the ability to connect dynamic plugin loading to the WorkflowRuntime creation pipeline
with the plugin name specified.

```csharp
var runtime = new WorkflowRuntime()
    .WithPlugin(new BasicPlugin())
    // &att>
    .WithDynamicPlugins("MyPlugin")
     // <&att
    .AsSingleServer();
```

If you've followed the steps exactly, `MyPlugin` will appear among the plugins in `WorkflowRuntime`,
which you can verify by checking `WorkflowRuntime.Plugins`.

```csharp
foreach (var plugin in runtime.Plugins)
{
    Console.WriteLine($"- {plugin.Name}");
}

// Output:
// - BasicPlugin
// - MyPlugin
```

## Conclusion

Now you can easily enhance plugin management in your project.