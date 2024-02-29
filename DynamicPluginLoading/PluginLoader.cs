using System.Reflection;
using System.Runtime.Loader;

namespace DynamicPluginLoading;

public class PluginLoader : AssemblyLoadContext
{
    private readonly AssemblyDependencyResolver _resolver;

    public PluginLoader(string path)
    {
        _resolver = new AssemblyDependencyResolver(path);
    }

    protected override Assembly? Load(AssemblyName assemblyName)
    {
        var assemblyPath = _resolver.ResolveAssemblyToPath(assemblyName);

        return assemblyPath != null
            ? LoadFromAssemblyPath(assemblyPath)
            : null;
    }

    protected override IntPtr LoadUnmanagedDll(string unmanagedDllName)
    {
        var libraryPath = _resolver.ResolveUnmanagedDllToPath(unmanagedDllName);

        return libraryPath != null
            ? LoadUnmanagedDllFromPath(libraryPath)
            : IntPtr.Zero;
    }
}