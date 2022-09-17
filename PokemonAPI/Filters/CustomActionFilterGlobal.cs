using Microsoft.AspNetCore.Mvc.Filters;

namespace PokemonAPI.Filters
{
    public class CustomActionFilterGlobal : IActionFilter
    {
        public void OnActionExecuted(ActionExecutedContext context)
        {
            Console.WriteLine("Filtro global - executado depois de tudo - 3");
        }

        public void OnActionExecuting(ActionExecutingContext context)
        {
            Console.WriteLine("Filtro global - executado antes de tudo - 3");
        }
    }
}
