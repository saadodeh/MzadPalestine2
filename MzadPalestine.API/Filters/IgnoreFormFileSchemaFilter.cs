using Microsoft.AspNetCore.Http;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;

namespace MzadPalestine.API.Filters
{
    /// <summary>
    /// فلتر بسيط لتجاهل خصائص ملفات IFormFile و Stream في مخططات Swagger
    /// </summary>
    public class IgnoreFormFileSchemaFilter : ISchemaFilter
    {
        private static readonly Type[] IgnoredTypes = new[]
        {
            typeof(IFormFile),
            typeof(IFormFileCollection),
            typeof(Stream),
            typeof(byte[]),
            typeof(MemoryStream)
        };

        public void Apply(OpenApiSchema schema, SchemaFilterContext context)
        {
            if (schema?.Properties == null || context?.Type == null)
                return;

            // تجاهل النماذج البدائية
            if (context.Type.IsPrimitive || context.Type == typeof(string))
                return;

            // الحصول على جميع الخصائص التي يجب تجاهلها
            var propNamesToIgnore = context.Type.GetProperties()
                .Where(p => ShouldIgnoreProperty(p))
                .Select(p => p.Name)
                .ToList();

            // تجاهل الخصائص في المخطط
            foreach (var propName in propNamesToIgnore)
            {
                var matchingProps = schema.Properties
                    .Where(p => p.Key.Equals(propName, StringComparison.OrdinalIgnoreCase))
                    .Select(p => p.Key)
                    .ToList();

                foreach (var prop in matchingProps)
                {
                    if (schema.Properties.ContainsKey(prop))
                    {
                        // استبدال الخاصية بنوع بسيط
                        schema.Properties[prop] = new OpenApiSchema
                        {
                            Type = "string",
                            Format = "binary",
                            Description = "File upload field"
                        };
                    }
                }
            }
        }

        private bool ShouldIgnoreProperty(PropertyInfo property)
        {
            // التحقق من نوع الخاصية
            if (IgnoredTypes.Contains(property.PropertyType))
                return true;

            // التحقق من اسم الخاصية
            var propertyName = property.Name.ToLower();
            return propertyName.Contains("file") ||
                   propertyName.Contains("image") ||
                   propertyName.Contains("stream") ||
                   propertyName.Contains("photo") ||
                   propertyName.Contains("upload");
        }
    }
} 