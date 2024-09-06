using System.ComponentModel;

namespace Anomalias.Shared;

public static class StrongIdHelper<TId, TValue> where TId : struct
{
    public static string Serialize(TValue value) => $"{GetPrefix(typeof(TId).Name)}{value?.ToString()?.ToLower() ?? string.Empty}";   
    public static TId? Deserialize(string? id)
    {
        if (FromString(typeof(TId), VerifyPrefix(typeof(TId), id)) is TValue value)
            return (TId?)Activator.CreateInstance(typeof(TId), value);
        return null;
    }       
    public static TId? Deserialize(Guid? id) => Deserialize(id.ToString());
    private static string GetPrefix(string name)=>
     name[..(name.Length - 2)]+"-";    
    private static string? VerifyPrefix(Type idType, string? rawValue)
    {
        string prefix = GetPrefix(idType.Name);
        if (rawValue is not null && rawValue.StartsWith(prefix))
            rawValue = (rawValue[prefix.Length..]);
        return rawValue;
    }
    private static object? FromString(Type idType, string? rawValue)
    {
        return rawValue is not null && GetContainedType(idType) is Type containedType
      ? TypeDescriptor.GetConverter(containedType).ConvertFromString(rawValue)
      : null;
    }
    private static Type? GetContainedType(Type idType) =>
     idType.GetProperty("Value")?.PropertyType;
}
