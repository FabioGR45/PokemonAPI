using System;

namespace PokemonAPI.Models
{
    public class PokemonModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Type { get; set; }
        public string Region { get; set; }
        public int Hp { get; set; }
        public int Attack { get; set; }

        public PokemonModel()
        {


        }

        public PokemonModel(int id, string name, string type, string region, int hp, int attack)
        {
            Id = id;
            Name = name;
            Type = type;
            Region = region;
            Hp = hp;
            Attack = attack;
        }

        public PokemonModel clone()
        {
            return (PokemonModel)this.MemberwiseClone(); // Shallow Clone
        }

    }
}
