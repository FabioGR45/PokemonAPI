using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using PokemonAPI.Models;
using System.Text.Json;

namespace PokemonAPI.Filters
{
    public class CustomAsyncActionFilterController : Attribute, IActionFilter
    {
        public void OnActionExecuted(ActionExecutedContext context)
        {
            Console.WriteLine("Filtro de controller - Executado depois da controller - 2");
        }

        public void OnActionExecuting(ActionExecutingContext context)
        {
            Console.WriteLine("Filtro de controller - Executado antes da controller - 2");

            #region ignorado
            //var resposta = new List<NovoProjetoModel>();
            //if(resposta.Count > 0)
            //{
            //    var model = new NovoProjetoModel();
            //    model.Id = 96;
            //    model.Name = "Short Circuit";
            //    resposta.Add(model);
            //    var shortCircuit = JsonSerializer.Serialize(resposta);
            //    context.Result = new ContentResult
            //    {
            //        Content = shortCircuit
            //    };
            //}
            #endregion

        }
    }
}
