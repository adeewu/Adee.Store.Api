using Adee.Store.Attributes;
using Microsoft.OpenApi.Any;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection;

namespace Adee.Store.Utils.Filtes
{
    /// <summary>
    /// Add enum value descriptions to Swagger
    /// </summary>
    public class SwaggerEnumFilter : IDocumentFilter
    {
        public void Apply(OpenApiDocument swaggerDoc, DocumentFilterContext context)
        {
            var dict = Assembly.GetEntryAssembly().GetReferencedAssemblies()
                .Where(p => p.Name.StartsWith("Adee.Store."))
                .Select(p => Assembly.Load(p))
                .SelectMany(p => p.GetTypes().Where(t => t.IsEnum()))
                .ToDictionary(p => p.FullName, p => p);

            swaggerDoc
                .Components
                .Schemas
                .Where(p => p.Value.Enum != null)
                .Where(p => p.Value.Enum.Any())
                .ToList()
                .ForEach(p =>
                {
                    var itemType = dict.Where(d => d.Key == p.Key).Select(d => d.Value).FirstOrDefault();

                    var enums = p.Value.Enum.Cast<OpenApiInteger>();

                    p.Value.Description += DescribeEnum(itemType, enums);
                });
        }

        public static string DescribeEnum(Type type, IEnumerable<OpenApiInteger> enums)
        {
            if (type == null) return string.Empty;

            var enumDescriptions = new List<string>();
            foreach (var item in enums)
            {
                var value = Enum.Parse(type, item.Value.ToString());
                var desc = GetDescription(type, value);

                if (string.IsNullOrEmpty(desc))
                {
                    enumDescriptions.Add($"{item.Value}:{Enum.GetName(type, value)}; ");
                }
                else
                {
                    enumDescriptions.Add($"{item.Value}:{Enum.GetName(type, value)},{desc}; ");
                }
            }
            return $"<br/>{Environment.NewLine}{string.Join("<br/>" + Environment.NewLine, enumDescriptions)}";
        }

        private static string GetDescription(Type t, object value)
        {
            foreach (MemberInfo mInfo in t.GetMembers())
            {
                if (mInfo.Name == t.GetEnumName(value))
                {
                    foreach (Attribute attr in Attribute.GetCustomAttributes(mInfo))
                    {
                        if (attr.GetType() == typeof(DescriptionAttribute))
                        {
                            return ((DescriptionAttribute)attr).Description;
                        }
                    }
                }
            }
            return string.Empty;
        }
    }

    public class SwaggerEnumOperationFilter : IOperationFilter
    {
        public void Apply(OpenApiOperation operation, OperationFilterContext context)
        {
            context.ApiDescription.ParameterDescriptions
                .Where(p => p.Type.IsNotNull())
                .Where(p => p.Type.IsEnum())
                .Select(p => new
                {
                    p.Type,
                    Schema = context.SchemaRepository.Schemas.Where(s => s.Key == p.Type.Name).Select(p => p.Value).FirstOrDefault(),
                    Parameter = operation.Parameters.Where(p => p.Name == p.Name).FirstOrDefault(),
                })
                .Where(p => p.Schema != null)
                .Select(p => new
                {
                    p.Parameter,
                    Description = SwaggerEnumFilter.DescribeEnum(p.Type, p.Schema.Enum.Cast<OpenApiInteger>()),
                })
                .ForEach(p => p.Parameter.Description += p.Description);
        }
    }

    public class SwaggerExcludeFilter : ISchemaFilter
    {
        public void Apply(OpenApiSchema schema, SchemaFilterContext context)
        {
            if (schema?.Properties == null) return;

            var excludedProperties = context.Type.GetProperties().Where(t => t.GetCustomAttribute<SwaggerExcludeAttribute>().IsNotNull());

            foreach (var excludedProperty in excludedProperties)
            {
                var propertyToRemove = schema.Properties.Keys.SingleOrDefault(x => x.ToLower() == excludedProperty.Name.ToLower());

                if (propertyToRemove != null)
                {
                    schema.Properties.Remove(propertyToRemove);
                }
            }
        }
    }
}
