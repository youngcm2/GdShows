using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Mvc.Abstractions;
using Microsoft.AspNetCore.Mvc.Controllers;

namespace GdShows.Filters
{
    public static class ActionExtensions
    {
		public static IEnumerable<T> GetCustomAttributes<T>(this ActionDescriptor actionDescriptor) where T : Attribute
		{
			var controllerActionDescriptor = actionDescriptor as ControllerActionDescriptor;
			if (controllerActionDescriptor != null)
			{
				return controllerActionDescriptor.MethodInfo.GetCustomAttributes(true).OfType<T>();
			}

			return Enumerable.Empty<T>();
		}
	}
}
