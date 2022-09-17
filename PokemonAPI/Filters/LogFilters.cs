using PokemonAPI.Models;
using PokemonAPI.Interfaces;
using Microsoft.AspNetCore.Mvc.Filters;
using PokemonAPI.Logs;
using PokemonAPI;

namespace PokemonAPI.Filters
{
    public class CustomLogsFilter : IResultFilter, IActionFilter
    {
        private readonly List<int> _sucessStatusCodes;
        private readonly IRepository _repository;
        private readonly Dictionary<int, PokemonModel> _contextDict;

        public CustomLogsFilter(IRepository repository)
        {
            _repository = repository;
            _contextDict = new Dictionary<int, PokemonModel>();
            _sucessStatusCodes = new List<int>() { StatusCodes.Status200OK, StatusCodes.Status201Created, StatusCodes.Status204NoContent };
        }

        public void OnActionExecuting(ActionExecutingContext context)
        {
            if (string.Equals(context.ActionDescriptor.RouteValues["controller"], "Pokemon", StringComparison.InvariantCultureIgnoreCase))
            {
                int id = 0;
                if (context.ActionArguments.ContainsKey("id") && int.TryParse(context.ActionArguments["id"].ToString(), out id))
                {
                    if (context.HttpContext.Request.Method.Equals("put", StringComparison.InvariantCultureIgnoreCase)
                        || context.HttpContext.Request.Method.Equals("patch", StringComparison.InvariantCultureIgnoreCase)
                        || context.HttpContext.Request.Method.Equals("delete", StringComparison.InvariantCultureIgnoreCase))
                    {
                        var character = _repository.GetPokemonById(id);
                        if (character != null)
                        {
                            var gameClone = character.clone();
                            _contextDict.Add(id, gameClone);
                        }
                    }
                }
            }
        }

        public void OnResultExecuted(ResultExecutedContext context)
        {
            if (context.HttpContext.Request.Path.Value.StartsWith("/Pokemon", StringComparison.InvariantCulture))
            {
                if (_sucessStatusCodes.Contains(context.HttpContext.Response.StatusCode))
                {
                    int.TryParse(context.HttpContext.Request.Path.ToString().Split("/").Last(), out int id);

                    if (context.HttpContext.Request.Method.Equals("put", StringComparison.InvariantCultureIgnoreCase)
                        || context.HttpContext.Request.Method.Equals("patch", StringComparison.InvariantCultureIgnoreCase))
                    {
                        var afterUpdate = _repository.GetPokemonById(id);
                        if (afterUpdate != null)
                        {
                            PokemonModel beforeUpdate;
                            if (_contextDict.TryGetValue(id, out beforeUpdate))
                            {
                                CustomLogs.SaveLog("Pokemon", afterUpdate.Id, afterUpdate.Name, context.HttpContext.Request.Method, beforeUpdate, afterUpdate);
                                _contextDict.Remove(id);
                            }
                        }
                    }
                    else if (context.HttpContext.Request.Method.Equals("delete", StringComparison.InvariantCultureIgnoreCase))
                    {
                        PokemonModel beforeUpdate;
                        if (_contextDict.TryGetValue(id, out beforeUpdate))
                        {
                            CustomLogs.SaveLog("Pokemon", beforeUpdate.Id, beforeUpdate.Name, context.HttpContext.Request.Method);
                            _contextDict.Remove(id);
                        }
                    }
                }
            }
        }

        #region Unused
        public void OnActionExecuted(ActionExecutedContext context)
        {

        }

        public void OnResultExecuting(ResultExecutingContext context)
        {

        }
        #endregion
    }
}