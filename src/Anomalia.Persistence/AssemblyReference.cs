using System.Reflection;

namespace Anomalia.Persistence;
public static class AssemblyReference
{
    public static readonly Assembly Assembly = typeof(AssemblyReference).Assembly;
}