using Microsoft.AspNetCore.Mvc.ModelBinding;
using Newtonsoft.Json.Linq;
using System.ComponentModel;

namespace Anomalias.App.Extensions;

public class StrongerIdBinder : IModelBinder
{
    public Task BindModelAsync(ModelBindingContext bindingContext)
    {
        string modelName = bindingContext.ModelName;
        Type modelType = bindingContext.ModelType;
        ValueProviderResult valueProviderResult = bindingContext.ValueProvider.GetValue(modelName);
        if (valueProviderResult == ValueProviderResult.None) return Task.CompletedTask;
        object? id = TryParse(modelType, valueProviderResult.FirstValue);
        StoreResult(bindingContext, modelName, id);
        return Task.CompletedTask;
    }
    private static void StoreResult(ModelBindingContext bindingContext, string modelName, object? id)
    {
        if (id is null)
            bindingContext.ModelState.TryAddModelError(modelName, "ID Inválido!");     
        else
        bindingContext.Result = ModelBindingResult.Success(id);        
    }

    private static object? TryParse(Type idType, string? rawValue) =>
    FromString(idType, VerifyPrefix(idType, rawValue)) is object parsedValue ? Activator.CreateInstance(idType, parsedValue) : null;

    private static object? FromString(Type idType, string? rawValue) =>
          rawValue is not null && GetContainedType(idType) is Type containedType
        ? TypeDescriptor.GetConverter(containedType).ConvertFromString(rawValue)
        : null;

    private static Type? GetContainedType(Type idType) =>
       idType.GetProperty("Value")?.PropertyType;

    private static string GetPrefix(string name) =>
     name[..(name.Length - 2)]+"-"; 
    
    private static string? VerifyPrefix(Type idType, string? rawValue)
    
    
    
    {
        string prefix = GetPrefix(idType.Name);
        if (rawValue is not null && rawValue.StartsWith(prefix))
            return rawValue = (rawValue[prefix.Length..]);
        return null;
        //throw new ArgumentException("Invalid ID");
    }

}


