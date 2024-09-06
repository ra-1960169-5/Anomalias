using System.Reflection;

namespace Anomalias.Infrastructure;
public static class AssemblyReference
{
    public static readonly Assembly Assembly = typeof(AssemblyReference).Assembly;
}