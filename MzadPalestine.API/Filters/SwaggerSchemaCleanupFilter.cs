using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;
using System;
using System.Collections.Generic;
using System.Linq;

namespace MzadPalestine.API.Filters
{
    /// <summary>
    /// فلتر لتنظيف النماذج في Swagger لتجنب المشاكل عند إنشاء swagger.json
    /// </summary>
    public class SwaggerSchemaCleanupFilter : ISchemaFilter
    {
        private static readonly HashSet<string> ProblematicPropertyNames = new HashSet<string>(StringComparer.OrdinalIgnoreCase)
        {
            "Password",
            "PasswordHash",
            "PasswordSalt",
            "File",
            "Stream",
            "Bytes",
            "Content",
            "ContentType",
            "Raw",
            "Binary"
        };

        public void Apply(OpenApiSchema schema, SchemaFilterContext context)
        {
            try
            {
                // تجاهل النماذج البدائية
                if (context.Type.IsPrimitive || context.Type == typeof(string) || context.Type == typeof(DateTime))
                {
                    return;
                }

                // تنظيف الخصائص الإشكالية
                if (schema.Properties != null && schema.Properties.Count > 0)
                {
                    var problematicProperties = schema.Properties
                        .Where(prop => IsProblematicProperty(prop.Key))
                        .Select(prop => prop.Key)
                        .ToList();

                    foreach (var propName in problematicProperties)
                    {
                        schema.Properties.Remove(propName);
                    }
                }

                // إذا كان النوع يتعلق بالملفات أو الدفق، تعيينه كسلسلة ثنائية
                if (IsFileRelatedType(context.Type))
                {
                    schema.Type = "string";
                    schema.Format = "binary";
                    schema.Properties?.Clear();
                }
            }
            catch (Exception ex)
            {
                // تسجيل الخطأ ولكن عدم رميه
                Console.WriteLine($"خطأ في SwaggerSchemaCleanupFilter: {ex.Message}");
            }
        }

        private bool IsProblematicProperty(string propertyName)
        {
            return ProblematicPropertyNames.Any(p => propertyName.Contains(p, StringComparison.OrdinalIgnoreCase));
        }

        private bool IsFileRelatedType(Type type)
        {
            if (type == null) return false;

            return 
                type == typeof(System.IO.Stream) ||
                type == typeof(System.IO.MemoryStream) ||
                type == typeof(Microsoft.AspNetCore.Http.IFormFile) ||
                type == typeof(Microsoft.AspNetCore.Http.IFormFileCollection) ||
                type == typeof(byte[]) ||
                (type.IsGenericType && type.GetGenericTypeDefinition() == typeof(List<>) && 
                 type.GetGenericArguments()[0] == typeof(Microsoft.AspNetCore.Http.IFormFile));
        }
    }
} 