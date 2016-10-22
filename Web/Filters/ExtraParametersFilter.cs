using System;
using System.Collections.Generic;
using System.Linq;
using Models.Api;
using Swashbuckle.Swagger.Model;
using Swashbuckle.SwaggerGen.Generator;

namespace GdShows.Filters
{
	public class ExtraParametersFilter : IOperationFilter
	{
		public void Apply(Operation operation, OperationFilterContext context)
		{
			var descriptor = context.ApiDescription.ActionDescriptor;
			var pagable = descriptor.GetCustomAttributes<PagableAttribute>().FirstOrDefault();
			if(operation.Parameters == null)
            {
                operation.Parameters = new List<IParameter>();
            }
			if (pagable != null)
			{
				operation.Parameters.Add(new NonBodyParameter
				{
					Name = nameof(Pagable.Limit).ToCamelCase(),
					Type = "integer",
					Default = pagable.DefaultLimit
				});

				operation.Parameters.Add(new NonBodyParameter
				{
					Name = nameof(Pagable.Offset).ToCamelCase(),
					Type = "integer",
					Default = 0
				});
			}

			var sortable = descriptor.GetCustomAttributes<SortableAttribute>().FirstOrDefault();

			if (sortable != null)
			{
				operation.Parameters.Add(new NonBodyParameter
				{
					Name = nameof(Sortable.SortName).ToCamelCase(),
					Type = "string",
					Default = sortable.DefaultSortName
				});

				operation.Parameters.Add(new NonBodyParameter
				{
					Name = nameof(Pagable.Descending).ToCamelCase(),
					Type = "boolean",
					Default = sortable.DefaultSortDescending
				});
			}

			var ensureToken = descriptor.GetCustomAttributes<EnsureTokenAttribute>().FirstOrDefault();

			if (ensureToken != null)
			{
				operation.Parameters?.Add(new NonBodyParameter
				{
					Name="SecurityToken".ToCamelCase(),
					Type = "string",
				});
			}
		}

		
	}
}