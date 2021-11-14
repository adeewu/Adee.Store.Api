using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;

namespace Microsoft.EntityFrameworkCore
{
    public static class RelationalPropertyBuilderExtensions
    {
        /// <summary>
        /// 枚举描述
        /// </summary>
        /// <typeparam name="TProperty"></typeparam>
        /// <param name="propertyBuilder"></param>
        /// <param name="comment"></param>
        /// <returns></returns>
        public static PropertyBuilder<TProperty> HasCommentForEnum<TProperty>(
            [JetBrains.Annotations.NotNull] this PropertyBuilder<TProperty> propertyBuilder,
            [JetBrains.Annotations.CanBeNull] string comment)
            where TProperty : struct
        {
            var propertyType = typeof(TProperty);

            var nameDescriptions = propertyType
                .GetMembers()
                .Select(p => new
                {
                    p.Name,
                    Description = Attribute.GetCustomAttributes(p).OfType<DescriptionAttribute>().Select(d => d.Description).SingleOrDefault()
                });

            var result = new List<string> { comment };
            foreach (var itemValue in propertyType.GetEnumValues())
            {
                var name = propertyType.GetEnumName(itemValue);
                if (name == null) continue;

                var description = nameDescriptions.Where(p => p.Name == name).Select(p => p.Description).SingleOrDefault();
                if (string.IsNullOrWhiteSpace(description))
                {
                    result.Add($"{name}：{(int)itemValue}");
                }
                else
                {
                    result.Add($"{name}：{(int)itemValue}[{description}]");
                }
            }
            var enumComment = result.JoinAsString("；");

            propertyBuilder.HasComment(enumComment);

            return propertyBuilder;
        }
    }
}
