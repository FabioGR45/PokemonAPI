using PokemonAPI.Dto;
using PokemonAPI.Models;

namespace PokemonAPI.Interfaces
{
    public interface IRepository
    {
        public int GetLastId();
        public List<PokemonModel> Get(int page, int maxResults);
        public PokemonModel GetId(int id);
        public PokemonModel GetPokemonById(int id);
        //public PokemonModel Insert(PokemonDto newPokemon);
        public List<PokemonModel> RecoverPokemon(string name, int page, int maxResults);
        public PokemonModel AddPokemon(PokemonDto entity);
        public PokemonModel PutPoke(int id, PokemonDto updatePoke);
        public PokemonModel PartialUpdate(int id, PokemonPatchDto updatePoke);
        public PokemonModel RemovePokemon(int id);
        public string Battle_TesteAlfa (int id1, int id2);
        public string GetAllStatus();


    }
}
