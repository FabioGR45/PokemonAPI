namespace PokemonAPI.Dto
{
    public class PokemonPatchDto
    {

        public string Type { get; set; }

        public PokemonPatchDto(string type)
        {

            Type = type;

        }

    }
}
