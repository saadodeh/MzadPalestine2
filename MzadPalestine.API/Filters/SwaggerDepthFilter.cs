using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;
using System.Collections.Generic;
using System.Linq;

namespace MzadPalestine.API.Filters
{
    /// <summary>
    /// ملحق لتحديد عمق النماذج في Swagger
    /// </summary>
    public static class SwaggerDepthExtensions
    {
        /// <summary>
        /// يضيف قابلية تحديد العمق الأقصى للنماذج في Swagger
        /// </summary>
        public static void MaxDepth(this SwaggerGenOptions options, int maxDepth)
        {
            options.SchemaFilter<MaxDepthSchemaFilter>(maxDepth);
        }
    }

    /// <summary>
    /// فلتر لتحديد العمق الأقصى للنماذج في Swagger
    /// </summary>
    public class MaxDepthSchemaFilter : ISchemaFilter
    {
        private readonly int _maxDepth;
        private readonly HashSet<Type> _processedTypes;

        public MaxDepthSchemaFilter(int maxDepth)
        {
            _maxDepth = maxDepth;
            _processedTypes = new HashSet<Type>();
        }

        public void Apply(OpenApiSchema schema, SchemaFilterContext context)
        {
            if (context.Type == null)
            {
                return;
            }

            // تجنب المعالجة المتكررة للأنواع
            if (!_processedTypes.Add(context.Type))
            {
                return;
            }

            // تطبيق تحديد العمق
            ApplyDepthLimit(schema, context, 0, _maxDepth);
        }

        private void ApplyDepthLimit(OpenApiSchema schema, SchemaFilterContext context, int currentDepth, int maxDepth)
        {
            if (currentDepth >= maxDepth)
            {
                schema.Properties?.Clear();
                schema.AdditionalProperties = null;
                schema.Items = null;
                return;
            }

            // معالجة خصائص النموذج
            if (schema.Properties != null && schema.Properties.Count > 0)
            {
                foreach (var property in schema.Properties)
                {
                    if (property.Value.Reference != null)
                    {
                        // إذا وصلنا للعمق الأقصى، نعطل المزيد من المراجع
                        if (currentDepth + 1 >= maxDepth)
                        {
                            // حذف المرجع واستبداله بنوع بسيط
                            property.Value.Reference = null;
                            property.Value.Type = "object";
                            property.Value.Properties = new Dictionary<string, OpenApiSchema>();
                        }
                    }
                }
            }
        }
    }
} 