using Microsoft.AspNetCore.Mvc.ModelBinding;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CrudUsuario.Application.Extensions
{
    public static class ModelStateExtension
    {
        public static List<string> GetErrors(this ModelStateDictionary modelState)
        {
            var result = new List<string>();

            foreach (var item in modelState.Values)
                result.AddRange(item.Errors.Select(x => x.ErrorMessage));

            return result;
        }
    }
}
