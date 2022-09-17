using System.Text.Json;

namespace PokemonAPI.Logs
{
    public class CustomLogs
    {
        const string PUT = "put"; 
        const string PATCH = "patch";
        const string DELETE = "delete";

        public static void SaveLog(string type, int id, string name, string httpMethod, object? entityBefore = null, object entityAfter = null)
        {
            var now = DateTime.Now.ToString("G");

            if (httpMethod.Equals(PUT, StringComparison.InvariantCultureIgnoreCase) || httpMethod.Equals(PATCH, StringComparison.InvariantCultureIgnoreCase))
            {
                Console.WriteLine($"{now} - {type} {id} - {name} - Changed from {JsonSerializer.Serialize(entityBefore)} to {JsonSerializer.Serialize(entityAfter)}");
            }
            if (httpMethod.Equals(DELETE, StringComparison.InvariantCultureIgnoreCase))
            {
                Console.WriteLine($"{now} - {type} {id} - {name} - Removed");
            }
        }
    }
}