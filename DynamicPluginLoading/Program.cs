using DynamicPluginLoading;
using OptimaJet.Workflow.Core.Runtime;
using OptimaJet.Workflow.Plugins;

var runtime = new WorkflowRuntime()
    .WithPlugin(new BasicPlugin())
    .WithDynamicPlugins("MyPlugin")
    .AsSingleServer();

Console.WriteLine("Plugins loaded:");

foreach (var plugin in runtime.Plugins)
{
    Console.WriteLine($"- {plugin.Name}");
}