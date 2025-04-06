using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace MzadPalestine.API.Filters;

/// <summary>
/// فلتر مستندات Swagger لمعالجة المشاكل في النماذج المعقدة
/// </summary>
public class SwaggerDocumentFilter : IDocumentFilter
{
    public void Apply(OpenApiDocument swaggerDoc, DocumentFilterContext context)
    {
        try
        {
            if (swaggerDoc.Components?.Schemas == null) return;
            
            // حذف النماذج التي قد تسبب مشاكل
            RemoveProblematicSchemas(swaggerDoc);
            
            // تبسيط تعريفات النماذج لمنع مشاكل التعقيد
            SimplifySchemaDefinitions(swaggerDoc);

            // تنظيف المسارات المكررة وتحسين التوثيق
            CleanupAndEnhancePaths(swaggerDoc);
        }
        catch (Exception ex)
        {
            // تسجيل الخطأ ولكن لا ترميه
            Console.WriteLine($"خطأ في SwaggerDocumentFilter: {ex.Message}");
        }
    }
    
    private void RemoveProblematicSchemas(OpenApiDocument swaggerDoc)
    {
        if (swaggerDoc.Components?.Schemas == null) return;
        
        var problematicSchemas = new List<string>();
        
        foreach (var schema in swaggerDoc.Components.Schemas)
        {
            if (schema.Key.Contains("Stream") || 
                schema.Key.Contains("IFormFile") || 
                schema.Key.Contains("FormFile") ||
                schema.Key.Contains("IFormFileCollection") ||
                schema.Key.Contains("FormFileCollection") ||
                schema.Key.Contains("Byte") ||
                schema.Key.Contains("Raw"))
            {
                problematicSchemas.Add(schema.Key);
            }
        }
        
        foreach (var schemaName in problematicSchemas)
        {
            if (swaggerDoc.Components.Schemas.ContainsKey(schemaName))
            {
                swaggerDoc.Components.Schemas.Remove(schemaName);
            }
        }
    }
    
    private void SimplifySchemaDefinitions(OpenApiDocument swaggerDoc)
    {
        if (swaggerDoc.Components?.Schemas == null) return;
        
        // التعامل مع النماذج المعقدة
        foreach (var schema in swaggerDoc.Components.Schemas.Values)
        {
            RemoveNestedReferences(schema, 0);
        }
        
        // التعامل مع مخططات المسارات
        HandlePathSchemas(swaggerDoc);
    }
    
    private void HandlePathSchemas(OpenApiDocument swaggerDoc)
    {
        if (swaggerDoc.Paths == null) return;
        
        // حل مشكلة الدورات المرجعية في المسارات
        foreach (var path in swaggerDoc.Paths)
        {
            if (path.Value?.Operations == null) continue;
            
            foreach (var operation in path.Value.Operations)
            {
                if (operation.Value?.Responses == null) continue;
                
                foreach (var response in operation.Value.Responses)
                {
                    if (response.Value?.Content == null || response.Value.Content.Count == 0) continue;
                    
                    foreach (var content in response.Value.Content)
                    {
                        if (content.Value?.Schema != null)
                        {
                            RemoveNestedReferences(content.Value.Schema, 0);
                        }
                    }
                }
                
                // التعامل مع معلمات الإدخال
                if (operation.Value.RequestBody?.Content != null)
                {
                    foreach (var content in operation.Value.RequestBody.Content)
                    {
                        if (content.Value?.Schema != null)
                        {
                            RemoveNestedReferences(content.Value.Schema, 0);
                        }
                    }
                }
            }
        }
    }
    
    private void RemoveNestedReferences(OpenApiSchema schema, int depth)
    {
        // منع التعمق الزائد
        if (depth > 2)
        {
            schema.Properties?.Clear();
            schema.Items = null;
            schema.AdditionalProperties = null;
            return;
        }
        
        // معالجة الخصائص المتداخلة
        if (schema.Properties != null)
        {
            var propertiesToRemove = new List<string>();
            
            foreach (var property in schema.Properties)
            {
                // تحديد الخصائص التي قد تسبب مشاكل
                if (property.Key.Contains("File") || 
                    property.Key.Contains("Stream") || 
                    property.Key.Contains("Byte"))
                {
                    propertiesToRemove.Add(property.Key);
                }
                else
                {
                    RemoveNestedReferences(property.Value, depth + 1);
                }
            }
            
            // إزالة الخصائص المشكلة
            foreach (var prop in propertiesToRemove)
            {
                schema.Properties.Remove(prop);
            }
        }
        
        // معالجة العناصر في المصفوفات
        if (schema.Items != null)
        {
            RemoveNestedReferences(schema.Items, depth + 1);
        }
        
        // معالجة الخصائص الإضافية
        if (schema.AdditionalProperties != null)
        {
            RemoveNestedReferences(schema.AdditionalProperties, depth + 1);
        }
    }
    
    private void CleanupAndEnhancePaths(OpenApiDocument swaggerDoc)
    {
        if (swaggerDoc.Paths == null) return;
        
        var paths = swaggerDoc.Paths.ToList();
        foreach (var path in paths)
        {
            if (path.Value?.Operations == null) continue;
            
            var operations = path.Value.Operations.ToList();
            foreach (var operation in operations)
            {
                // إضافة معرف فريد لكل عملية
                if (string.IsNullOrEmpty(operation.Value.OperationId))
                {
                    operation.Value.OperationId = $"{operation.Key}_{path.Key.Replace("/", "_")}";
                }

                // إضافة وصف افتراضي إذا لم يكن موجوداً
                if (string.IsNullOrEmpty(operation.Value.Description))
                {
                    operation.Value.Description = $"Operation for {path.Key}";
                }
                
                // إضافة ملخص افتراضي إذا لم يكن موجوداً
                if (string.IsNullOrEmpty(operation.Value.Summary))
                {
                    operation.Value.Summary = $"{operation.Key} operation for {path.Key}";
                }
            }
        }
    }
} 