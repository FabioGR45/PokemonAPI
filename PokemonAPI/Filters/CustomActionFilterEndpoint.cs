using Microsoft.AspNetCore.Mvc.Filters;

namespace PokemonAPI.Filters
{
    public class CustomActionFilterEndpoint : Attribute, IActionFilter
    {
        public void OnActionExecuted(ActionExecutedContext context)
        {
            Console.WriteLine("Filtro de Endpoint - executado depois do método - 1");
        }

        public void OnActionExecuting(ActionExecutingContext context)
        {
            Console.WriteLine("Filtro de Endpoint - executado antes do método - 1");
        }
    }
}
