using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.OpenApi.Models;
using MzadPalestine.API.Filters;
using Swashbuckle.AspNetCore.SwaggerGen;
using System;
using System.IO;
using System.Linq;
using System.Reflection;

namespace MzadPalestine.API
{
    public static class ConfigureSwaggerExtensions
    {
        /// <summary>
        /// إضافة تكوينات إضافية للتعامل مع الملفات في Swagger
        /// </summary>
        public static void AddSwaggerFileSupport(this SwaggerGenOptions options)
        {
            // تكوين أنواع الملفات
            options.MapType<IFormFile>(() => new OpenApiSchema
            {
                Type = "string",
                Format = "binary"
            });
            
            options.MapType<Stream>(() => new OpenApiSchema
            {
                Type = "string",
                Format = "binary"
            });
            
            options.MapType<byte[]>(() => new OpenApiSchema
            {
                Type = "string",
                Format = "binary"
            });
            
            // إضافة الفلاتر
            options.SchemaFilter<FileUploadOperationFilter>();
            options.SchemaFilter<SwaggerSchemaCleanupFilter>();
            
            // ضبط تكوين إضافي
            options.CustomSchemaIds(type => type.Name); // استخدام اسم النوع فقط
        }
    }
    
    /// <summary>
    /// فلتر لإضافة دعم تحميل الملفات في Swagger
    /// </summary>
    public class FileUploadOperationFilter : ISchemaFilter
    {
        public void Apply(OpenApiSchema schema, SchemaFilterContext context)
        {
            if (context.Type == typeof(IFormFile) || 
                context.Type == typeof(IFormFileCollection) || 
                context.Type == typeof(Stream))
            {
                schema.Type = "string";
                schema.Format = "binary";
                schema.Properties?.Clear();
                schema.Items = null;
            }
        }
    }
} 