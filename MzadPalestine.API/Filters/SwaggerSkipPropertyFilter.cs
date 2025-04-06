using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;
using System.Reflection;

namespace MzadPalestine.API.Filters
{
    /// <summary>
    /// فلتر لمعالجة المراجع الدائرية في Swagger
    /// </summary>
    public class SwaggerSkipPropertyFilter : ISchemaFilter
    {
        public void Apply(OpenApiSchema schema, SchemaFilterContext context)
        {
            if (schema?.Properties == null || context.Type == null)
            {
                return;
            }

            // تحقق إذا كان النوع هو نوع معقد
            if (!context.Type.IsPrimitive && context.Type != typeof(string))
            {
                var excludedProperties = context.Type.GetProperties()
                    .Where(p => IsComplexType(p.PropertyType) && IsCyclical(context.Type, p.PropertyType))
                    .Select(p => p.Name)
                    .ToList();

                foreach (var excludedProperty in excludedProperties)
                {
                    var propertyToExclude = schema.Properties.Keys
                        .SingleOrDefault(x => string.Equals(x, excludedProperty, StringComparison.OrdinalIgnoreCase));

                    if (propertyToExclude != null && schema.Properties.ContainsKey(propertyToExclude))
                    {
                        schema.Properties.Remove(propertyToExclude);
                    }
                }
            }
        }

        private static bool IsComplexType(Type type)
        {
            return 
                !type.IsPrimitive && 
                type != typeof(string) && 
                type != typeof(decimal) && 
                type != typeof(DateTime) && 
                type != typeof(Guid) && 
                !type.IsEnum && 
                !IsNullableSimpleType(type);
        }

        private static bool IsNullableSimpleType(Type type)
        {
            return 
                type.IsGenericType && 
                type.GetGenericTypeDefinition() == typeof(Nullable<>) && 
                !IsComplexType(type.GetGenericArguments()[0]);
        }

        private static bool IsCyclical(Type parentType, Type propertyType)
        {
            if (parentType == propertyType)
            {
                return true;
            }

            if (propertyType.IsGenericType)
            {
                foreach (var arg in propertyType.GetGenericArguments())
                {
                    if (IsCyclical(parentType, arg))
                    {
                        return true;
                    }
                }
            }

            var props = propertyType.GetProperties();
            return props.Any(prop => IsCyclical(parentType, prop.PropertyType));
        }
    }
} 