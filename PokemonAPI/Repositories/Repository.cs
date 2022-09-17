using Microsoft.AspNetCore.Mvc;
using PokemonAPI.Dto;
using PokemonAPI.Interfaces;
using PokemonAPI.Models;
using System.Text.Json;

namespace PokemonAPI.Repositories
{
    public class Repository : IRepository
    {
        private readonly string _databaseFile;
        private List<PokemonModel> pokemons;

        public Repository()
        {
            _databaseFile = $"{Environment.CurrentDirectory}\\database.json";
        }
        public PokemonModel AddPokemon(PokemonDto entity)
        {
            var reader = new StreamReader($"{Environment.CurrentDirectory}\\database.json");
            var json = reader.ReadToEnd();
            reader.Dispose();
            var data = JsonSerializer.Deserialize<List<PokemonModel>>(json);

            var setLastId = data.OrderBy(x => x.Id).Last().Id + 1;

            var newPokemon = new PokemonModel
            {
                Id = setLastId,
                Name = entity.Name.ToUpper(),
                Type = entity.Type.ToUpper(),
                Region = entity.Region.ToUpper(),
                Hp = entity.Hp,
                Attack = entity.Attack
            };


            data.Add(newPokemon);
            var content = JsonSerializer.Serialize(data);
            System.IO.File.WriteAllText($"{Environment.CurrentDirectory}\\database.json", content);
            return newPokemon;
        }

        public List<PokemonModel> Get(int page, int maxResults)
        {
            var desserialized = JsonSerializer.Deserialize<List<PokemonModel>>(File.ReadAllText(_databaseFile));
            var response = desserialized.Skip((page - 1) * maxResults).Take(maxResults).ToList();
            return response;
        }

        public PokemonModel GetId(int id)
        {
            using var reader = new StreamReader($"{Environment.CurrentDirectory}\\database.json");
            var json = reader.ReadToEnd();
            var data = JsonSerializer.Deserialize<List<PokemonModel>>(json);

            var result = data.Where(x => x.Id == id).FirstOrDefault();

            return result;
        }

        public PokemonModel GetPokemonById(int id)
        {
            using var reader = new StreamReader($"{Environment.CurrentDirectory}\\database.json");
            var json = reader.ReadToEnd();
            var data = JsonSerializer.Deserialize<List<PokemonModel>>(json);

            var character = data.Where(x => x.Id == id).FirstOrDefault();
            if (character == null) return null;

            return character;
        }


        public List<PokemonModel> RecoverPokemon(string name, int page, int maxResults)
        {
            var desserialized = JsonSerializer.Deserialize<List<PokemonModel>>(File.ReadAllText(_databaseFile));

            var result = desserialized.Where(x => x.Name.Contains(name));

            var response = result.Skip((page - 1) * maxResults).Take(maxResults).ToList();

            var finalResult = response.Where(x => x.Name.Contains(name)).ToList().OrderBy(x => x.Id);

            return finalResult.ToList();
        }

        public PokemonModel PutPoke(int id, PokemonDto entity)
        {

            var reader = new StreamReader($"{Environment.CurrentDirectory}\\database.json");
            var json = reader.ReadToEnd();
            reader.Dispose();
            var data = JsonSerializer.Deserialize<List<PokemonModel>>(json);
            var content = "";

            var setLastId = data.OrderBy(x => x.Id).Last().Id + 1;

            if (id >= setLastId)
            {

                var newPokemon = new PokemonModel
                {
                    Id = setLastId,
                    Name = entity.Name.ToUpper(),
                    Type = entity.Type.ToUpper(),
                    Region = entity.Region.ToUpper(),
                    Hp = entity.Hp,
                    Attack = entity.Attack
                };

                data.Add(newPokemon);
                content = JsonSerializer.Serialize(data);
                System.IO.File.WriteAllText($"{Environment.CurrentDirectory}\\database.json", content);
                return (newPokemon);

            }

            var pokeUpdate = data.Where(x => x.Id == id).First();
            PokemonModel basePokemon = pokeUpdate;
            var keepId = pokeUpdate.Id;

            data.Remove(pokeUpdate);

            var changedPokemon = new PokemonModel
            {
                Id = keepId,
                Name = entity.Name.ToUpper(),
                Type = entity.Type.ToUpper(),
                Region = entity.Region.ToUpper(),
                Hp = entity.Hp,
                Attack = entity.Attack
            };

            data.Add(changedPokemon);
            content = JsonSerializer.Serialize(data);
            System.IO.File.WriteAllText($"{Environment.CurrentDirectory}\\database.json", content);

            return changedPokemon;

        }

            public PokemonModel PartialUpdate(int id, PokemonPatchDto entity)
        {

            var reader = new StreamReader($"{Environment.CurrentDirectory}\\database.json");
            var json = reader.ReadToEnd();
            reader.Dispose();
            var data = JsonSerializer.Deserialize<List<PokemonModel>>(json);
            var content = "";

            var pokeUpdate = data.Where(x => x.Id == id).First();
            PokemonModel basePokemon = pokeUpdate;

            data.Remove(pokeUpdate);

            var changedPokemon = new PokemonModel
            {
                Id = basePokemon.Id,
                Name = basePokemon.Name.ToUpper(),
                Type = entity.Type.ToUpper(),
                Region = basePokemon.Region.ToUpper(),
                Hp = basePokemon.Hp,
                Attack = basePokemon.Attack
            };

            data.Add(changedPokemon);
            content = JsonSerializer.Serialize(data);
            System.IO.File.WriteAllText($"{Environment.CurrentDirectory}\\database.json", content);

            return basePokemon;

        }

        public PokemonModel RemovePokemon(int id)
        {
            PokemonModel pkm;

            var reader = new StreamReader($"{Environment.CurrentDirectory}\\database.json");
            var json = reader.ReadToEnd();
            reader.Dispose();
            var data = JsonSerializer.Deserialize<List<PokemonModel>>(json);

            var deletePoke = data.Where(x => x.Id == id).First();
            //PokemonModel basePokemon = deletePoke;

            data.Remove((PokemonModel)data.Where(x => x.Id == id).First());
            var content = JsonSerializer.Serialize(data);
            System.IO.File.WriteAllText($"{Environment.CurrentDirectory}\\database.json", content);

            return deletePoke;
        }

        public int GetLastId()
        {
            try
            {
                var desserialized = JsonSerializer.Deserialize<List<PokemonModel>>(File.ReadAllText(_databaseFile));
                return desserialized.Last().Id;
            }
            catch (FileNotFoundException
            fe)
            {
                return 0;
            }
        }

        public string Battle_TesteAlfa(int id1, int id2)
        {

            using var reader = new StreamReader(".\\database.json");
            var json = reader.ReadToEnd();
            var data = JsonSerializer.Deserialize<List<PokemonModel>>(json);

            var poke1 = data.Where(x => x.Id == id1).First();
            var poke2 = data.Where(x => x.Id == id2).First();

            var Poke1Battlepoints = poke1.Hp + poke1.Attack;
            var Poke2Battlepoints = poke2.Hp + poke2.Attack;

            if (Poke1Battlepoints > Poke2Battlepoints)
            {
                return ($"Pokémon {poke1.Name} venceu!\n\n Informações:\n Nome: {poke1.Name}\n Tipo: {poke1.Type}\n Região: {poke1.Region}\n HP: {poke1.Hp}\n Ataque: {poke1.Attack}");

            }
            else if (Poke1Battlepoints < Poke2Battlepoints)
            {
                return ($"Pokémon {poke2.Name} venceu!\n\n Informações:\n Nome: {poke2.Name}\n Tipo: {poke2.Type}\n Região: {poke2.Region}\n HP: {poke2.Hp}\n Ataque: {poke2.Attack}");
            }
            else
            {
                return ($"Empatou!\n\n Informações:\n Nome: {poke1.Name}\n Tipo: {poke1.Type}\n Região: {poke1.Region}\n HP: {poke1.Hp}\n Ataque: {poke1.Attack} \n\n " +
                        $"Nome: {poke2.Name}\n Tipo: {poke2.Type}\n Região: {poke2.Region}\n HP: {poke2.Hp}\n Ataque: {poke2.Attack}");

            }

        }

        public string GetAllStatus() {

            using var reader = new StreamReader(".\\database.json");
            var json = reader.ReadToEnd();
            var data = JsonSerializer.Deserialize<List<PokemonModel>>(json);

            var totalHP = 0;
            var mediumHP = 0;

            var totalAttack = 0;
            var mediumAttack = 0;

            var totalPoke = 0;

            //MAIOR HP
            var highestHP = int.MinValue;
            var HighestHpName = "";


            //MAIOR ATAQUE
            var highestAttack = int.MinValue;
            var HighestAttackName = "";

            //LOOP
            var breakLoop = true;

            var resultHP = data.Where(x => x.Hp > totalHP).ToList();

            while (breakLoop)
            {

                foreach (var poke in resultHP)
                {
                    if (poke.Hp > highestHP)
                    {

                        highestHP = poke.Hp;
                        HighestHpName = poke.Name;

                    }
                }

                breakLoop = false;

            }

            breakLoop = true;

            var resultAttack = data.Where(x => x.Attack > totalAttack).ToList();

            while (breakLoop)
            {

                foreach (var poke in resultAttack)
                {
                    if (poke.Attack > highestAttack)
                    {

                        highestAttack = poke.Attack;
                        HighestAttackName = poke.Name;

                    }
                }

                breakLoop = false;

            }



            for (int i = 0; i < data.Count; i++)
            {
                totalPoke++;
            }

            //TOTAL DE HP
            foreach (var poke in resultHP)
            {
                totalHP = totalHP + poke.Hp;
            }

            //MÉDIA DE HP
            mediumHP = totalHP / totalPoke;

            //TOTAL DE ATAQUE
            foreach (var poke in resultAttack)
            {
                totalAttack = totalAttack + poke.Attack;
            }

            //MÉDIA DE ATAQUE
            mediumAttack = totalAttack / totalPoke;

            return ($"HP TOTAL = {totalHP} \nMÉDIA DE HP = {mediumHP} \nMAIOR HP = {highestHP} (Pokémon: {HighestHpName}) \n\nATAQUE TOTAL = {totalAttack} \nMÉDIA DE ATAQUE = {mediumAttack} " +
                $"\nMAIOR ATAQUE = {highestAttack} (Pokémon: {HighestAttackName})");

        }

    }
}
