using OptimaJet.Workflow.Core.Runtime;
using OptimaJet.Workflow.Plugins;

namespace MyPlugin;

public class MyPlugin : IWorkflowPlugin
{

    public string Name => nameof(MyPlugin);
    public bool Disabled { get; set; }
    public Dictionary<string, string> PluginSettings => new();

    public void OnPluginAdd(WorkflowRuntime runtime, List<string>? schemes = null)
    {
        // Do nothing
    }

    public Task OnRuntimeStartAsync(WorkflowRuntime runtime)
    {
        // Do nothing
        return Task.CompletedTask;
    }
}