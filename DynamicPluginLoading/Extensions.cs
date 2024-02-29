using System.Reflection;
using OptimaJet.Workflow.Core.Runtime;
using OptimaJet.Workflow.Plugins;

namespace DynamicPluginLoading;

public static class Extensions
{
    private const string PluginsFolder = "plugins";

    public static WorkflowRuntime WithDynamicPlugins(this WorkflowRuntime runtime, params string[] plugins)
    {
        foreach (var plugin in plugins)
        {
            var pluginFolder = $"{PluginsFolder}\\{plugin}";
            var dllPath = Path.Combine(Environment.CurrentDirectory, $"{pluginFolder}\\{plugin}.dll");

            try
            {
                var loader = new PluginLoader(dllPath);

                var assembly = loader.LoadFromAssemblyName(new AssemblyName(plugin));

                var workflowPluginTypes = assembly.GetTypes()
                    .Where(type => typeof(IWorkflowPlugin).IsAssignableFrom(type));

                foreach (var workflowPlugin in workflowPluginTypes)
                {
                    try
                    {
                        var instance = Activator.CreateInstance(workflowPlugin) as IWorkflowPlugin;
                        runtime.WithPlugin(instance);
                    }
                    catch (Exception)
                    {
                        Console.WriteLine("Create instance failed for plugin: " + workflowPlugin.FullName);
                    }
                }
            }
            catch (Exception)
            {
                Console.WriteLine($"Plugin {plugin} not found on path '{dllPath}'");
            }
        }

        return runtime;
    }
}