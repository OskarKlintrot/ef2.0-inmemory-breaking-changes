using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;

namespace Intranet.Test.Tools.Extensions
{
    public static class ActionResultExtensions
    {
        public static int CountModelItems(this IActionResult actionResult)
        {
            return GetModelAs<IEnumerable<object>>(actionResult).Count();
        }

        public static bool ModelStateIsValid(this IActionResult actionResult)
        {
            return ((ViewResult)actionResult).ViewData.ModelState.IsValid;
        }

        public static object GetModel(this IActionResult actionResult)
        {
            return ((ViewResult)actionResult).Model;
        }

        public static IEnumerable<ModelError> GetModelStateErrorMessages(this IActionResult actionResult)
        {
            return actionResult.GetModelState().SelectMany(m => m.Value.Errors.Where(e => e.ErrorMessage != null));
        }

        public static IEnumerable<ModelError> GetModelStateErrorMessages(this IActionResult actionResult, string key)
        {
            actionResult.GetModelState().TryGetValue(key, out ModelStateEntry value);
            return value.Errors.Where(e => e.ErrorMessage != null);
        }

        public static ModelStateDictionary GetModelState(this IActionResult actionResult)
        {
            return ((ViewResult)actionResult).ViewData.ModelState;
        }

        public static T GetModelAs<T>(this IActionResult actionResult)
            where T : class
        {
            return actionResult.GetModel() as T;
        }

        public static bool ValidateActionRedirect(this IActionResult actionResult, string action)
        {
            return ((RedirectToActionResult)actionResult).ActionName.Equals(action);
        }
    }
}
