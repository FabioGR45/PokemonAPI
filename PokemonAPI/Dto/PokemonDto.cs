namespace PokemonAPI.Dto
{
    public class PokemonDto
    {
        public string Name { get; set; }
        public string Type { get; set; }
        public string Region { get; set; }
        public int Hp { get; set; }
        public int Attack { get; set; }


        public PokemonDto(string name, string type, string region, int hp, int attack)
        {
            Name = name;
            Type = type;
            Region = region;
            Hp = hp;
            Attack = attack;
        }
    }
}
