using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Net;
using GdShows.Filters;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Primitives;
using Models.Api;

namespace GdShows.API
{
	public abstract class BaseController : Controller
	{
		public Pagable Paging { get; set; }
		public Sortable Sorting { get; set; }
		
		public override void OnActionExecuting(ActionExecutingContext context)
		{
			var pagable = context.ActionDescriptor.GetCustomAttributes<PagableAttribute>().FirstOrDefault();
			if (pagable != null)
			{
				Paging = new Pagable
				{
					Limit = GetQueryInt(nameof(Paging.Limit), pagable.DefaultLimit),
					Offset = GetQueryInt(nameof(Paging.Offset), 0),
				};
			}
			var sorting = context.ActionDescriptor.GetCustomAttributes<SortableAttribute>().FirstOrDefault();
			if (sorting != null)
			{
				Sorting = new Sortable
				{
					SortName = GetQuery(nameof(Sorting.SortName), sorting.DefaultSortName),
					Descending = GetQueryBool(nameof(Sorting.Descending), sorting.DefaultSortDescending)
				};
			}
			
			if (!ModelState.IsValid)
			{
				var errorList = ModelState.ToDictionary(
					kvp => kvp.Key,
					kvp => kvp.Value.Errors.Select(e => string.IsNullOrEmpty(e.ErrorMessage) ? e.Exception?.GetBaseException().Message : e.ErrorMessage).ToArray()
				);

				throw new ApiException(HttpStatusCode.BadRequest, "Invalid request", errorList);
			}

			base.OnActionExecuting(context);
		}

		private bool GetQueryBool(string key, bool defaultValue = false)
		{
			var httpRequest = Request;
			StringValues values;
			if (httpRequest.Query.TryGetValue(key, out values))
			{
				var first = values.FirstOrDefault();
				bool value;
				if (bool.TryParse(first, out value))
				{
					return value;
				}
			}

			return defaultValue;
		}

		private int? GetQueryInt(string key, int? defaultValue = null)
		{
			var httpRequest = Request;
			StringValues values;
			if (httpRequest.Query.TryGetValue(key, out values))
			{
				var first = values.FirstOrDefault();
				int value;
				if (int.TryParse(first, out value))
				{
					return value;
				}
			}

			return defaultValue;
		}

		private string GetQuery(string key, string defaultValue = null)
		{
			var httpRequest = Request;
			StringValues values;
			if (httpRequest.Query.TryGetValue(key, out values))
			{
				var first = values.FirstOrDefault();
				return first;
			}

			return defaultValue;
		}

		protected void ValidateParameters(object parameters)
		{
			if (parameters == null)
			{
				throw new ApplicationException("Parameters not provided");
			}

			var context = new ValidationContext(parameters);
			var errors = new List<ValidationResult>();
			var success = Validator.TryValidateObject(parameters, context, errors);

			if (!success)
			{
				var results = new List<string>();

				foreach (var error in errors)
				{
					results.Add(error.ErrorMessage);
				}

				var value = string.Join(",", results);

				throw new ApplicationException(value);
			}
		}
	}
}
