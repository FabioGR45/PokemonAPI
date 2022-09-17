using Microsoft.AspNetCore.Mvc;
using PokemonAPI.Dto;
using PokemonAPI.Filters;
using PokemonAPI.Interfaces;
using PokemonAPI.Logs;
using PokemonAPI.Models;
using PokemonAPI.Dto;
using System.Buffers.Text;
using System.Text.Json;
using Microsoft.AspNetCore.Authorization;

namespace PokemonAPI.Controllers
{
    [ApiController]
    [Route("[controller]")]
    [CustomAsyncActionFilterController]
    public class PokemonController : ControllerBase
    {

        private readonly ILogger<PokemonController> _logger;
        private readonly List<PokemonModel> _database;
        private readonly IRepository _repository;

        public PokemonController(ILogger<PokemonController> logger, IRepository repository)
        {
            _logger = logger;
            _database = new List<PokemonModel>();
            _repository = repository;
        }

        [CustomActionFilterEndpoint]
        [HttpGet("Paginado")]
        [ProducesResponseType(typeof(PokemonModel), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(PokemonModel), StatusCodes.Status204NoContent)]
        [ProducesResponseType(typeof(PokemonModel), StatusCodes.Status404NotFound)]
        public async Task<IActionResult> Get(int page, int maxResults)
        {
            return Ok(_repository.Get(page, maxResults));
        }

        [HttpGet("{id}")]
        [ProducesResponseType(typeof(PokemonModel), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(PokemonModel), StatusCodes.Status204NoContent)]
        [ProducesResponseType(typeof(PokemonModel), StatusCodes.Status404NotFound)]
        public async Task<ActionResult<PokemonModel>> GetId(int id)
        {
            using var reader = new StreamReader($"{Environment.CurrentDirectory}\\database.json");
            var json = await reader.ReadToEndAsync();
            var data = JsonSerializer.Deserialize<List<PokemonModel>>(json);

            var setLastId = data.OrderBy(x => x.Id).Last().Id + 1;

            if (id > setLastId || id == 0 || _repository.GetPokemonById(id) == null)
                return NotFound("Pokémon não encontrado");

            return Ok(_repository.GetId(id));
        }

        [HttpGet("Battle_TesteAlfa")]
        [ProducesResponseType(typeof(PokemonModel), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(PokemonModel), StatusCodes.Status204NoContent)]
        [ProducesResponseType(typeof(PokemonModel), StatusCodes.Status404NotFound)]
        public async Task<ActionResult<PokemonModel>> GetBattle([FromQuery] int id1, [FromQuery] int id2)
        {

            using var reader = new StreamReader(".\\database.json");
            var json = reader.ReadToEnd();
            var data = JsonSerializer.Deserialize<List<PokemonModel>>(json);

            var setLastId = data.OrderBy(x => x.Id).Last().Id + 1;

            if (id1 > setLastId || id2 > setLastId || id1 == 0 || id2 == 0 || _repository.GetPokemonById(id1) == null || _repository.GetPokemonById(id2) == null)
                return NotFound("Pokémon não encontrado");

            return Ok(_repository.Battle_TesteAlfa(id1, id2));

        }

        [HttpGet("GetAllStatus")]
        [ProducesResponseType(typeof(PokemonModel), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(PokemonModel), StatusCodes.Status204NoContent)]
        [ProducesResponseType(typeof(PokemonModel), StatusCodes.Status404NotFound)]
        public async Task<ActionResult<List<PokemonModel>>> GetAllStatus()
        {

            return Ok(_repository.GetAllStatus());

        }

        [HttpPost]
        [ProducesResponseType(typeof(PokemonModel), StatusCodes.Status201Created)]
        [ProducesResponseType(typeof(string), StatusCodes.Status415UnsupportedMediaType)]
        [Authorize]
        public async Task<ActionResult<List<PokemonModel>>> RecoverPokemon(int page, int maxResults, [FromQuery] string name)
        {

            using var reader = new StreamReader($"{Environment.CurrentDirectory}\\database.json");
            var json = await reader.ReadToEndAsync();
            var data = JsonSerializer.Deserialize<List<PokemonModel>>(json);

            var setLastId = data.OrderBy(x => x.Id).Last().Id + 1;
            var upperWord = name.ToUpper();

            var result = data.Where(x => x.Name.Contains(upperWord));

            var filteredData = result;

            if (!string.IsNullOrWhiteSpace(name))
                filteredData = result.Where(x => x.Type.Contains(upperWord)).ToList();
            else
                return BadRequest("Pokémon não encontrado");

            if(filteredData.Count() == 0)
                return BadRequest("Pokémon não encontrado");

            return Ok(_repository.RecoverPokemon(upperWord, page, maxResults));

        }

        [HttpPost("Normal")]
        [ProducesResponseType(typeof(PokemonModel), StatusCodes.Status201Created)]
        [ProducesResponseType(typeof(string), StatusCodes.Status415UnsupportedMediaType)]
        [Authorize]
        public async Task<ActionResult<List<PokemonModel>>> AddPokemon([FromBody] PokemonDto entity)
        {
            var reader = new StreamReader ($"{Environment.CurrentDirectory}\\database.json");
            var json = reader.ReadToEnd();
            reader.Dispose();
            var data = JsonSerializer.Deserialize<List<PokemonModel>>(json);

            return Ok(_repository.AddPokemon(entity));
        }

        [HttpPut("{id}")]
        [ProducesResponseType(typeof(PokemonModel), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(string), StatusCodes.Status415UnsupportedMediaType)]
        [Authorize]
        public async Task<IActionResult> PutPoke([FromRoute] int id, [FromBody] PokemonDto entity)
        {

            var reader = new StreamReader($"{Environment.CurrentDirectory}\\database.json");
            var json = reader.ReadToEnd();
            reader.Dispose();
            var data = JsonSerializer.Deserialize<List<PokemonModel>>(json);

            var setLastId = data.OrderBy(x => x.Id).Last().Id + 1;

            if (id == 0)
                return NotFound("Impossível atualizar ou criar com ID 0");

            return Ok(_repository.PutPoke(id, entity));
        }

        [HttpPatch("{id}")]
        [ProducesResponseType(typeof(PokemonModel), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(string), StatusCodes.Status415UnsupportedMediaType)]
        [Authorize]
        public async Task<IActionResult> PartialUpdate([FromRoute] int id, [FromBody] PokemonPatchDto entity)
        {

            var reader = new StreamReader($"{Environment.CurrentDirectory}\\database.json");
            var json = reader.ReadToEnd();
            reader.Dispose();
            var data = JsonSerializer.Deserialize<List<PokemonModel>>(json);

            var setLastId = data.OrderBy(x => x.Id).Last().Id + 1;

            if (id > setLastId || id == 0 || _repository.GetPokemonById(id) == null)
                return NotFound("Pokémon não encontrado");

            return Ok(_repository.PartialUpdate(id, entity));

        }

        #region FullPatch
        /*
        [HttpPatch("{id}")]
        [ProducesResponseType(typeof(PokemonModel), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(string), StatusCodes.Status415UnsupportedMediaType)]
        public async Task<IActionResult> FullPatch([FromRoute] int id, [FromBody] PokemonDto entity)
        {

            var reader = new StreamReader($"{Environment.CurrentDirectory}\\database.json");
            var json = reader.ReadToEnd();
            reader.Dispose();
            var data = JsonSerializer.Deserialize<List<PokemonModel>>(json);

            var setLastId = data.OrderBy(x => x.Id).Last().Id + 1;

            if (id > setLastId)
                return NotFound("Pokémon não encontrado");



            var pokeUpdate = data.Where(x => x.Id == id).FirstOrDefault();
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
            var content = JsonSerializer.Serialize(data);
            System.IO.File.WriteAllText($"{Environment.CurrentDirectory}\\database.json", content);

            //_logger.LogInformation($"{DateTime.Now.ToString("G")} - Pokémon {basePokemon.Id} - {basePokemon.Name} - Alterado de {JsonSerializer.Serialize(basePokemon)} para {JsonSerializer.Serialize(changedPokemon)}");

            return Ok(data);

        }
        */
        #endregion

        [HttpDelete("{id}")]
        [ProducesResponseType(typeof(PokemonModel), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(PokemonModel), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(string), StatusCodes.Status415UnsupportedMediaType)]
        [Authorize]
        public async Task<ActionResult<List<PokemonModel>>> DeletePokemon([FromRoute] int id)
        {

            var reader = new StreamReader($"{Environment.CurrentDirectory}\\database.json");
            var json = reader.ReadToEnd();
            reader.Dispose();
            var data = JsonSerializer.Deserialize<List<PokemonModel>>(json);

            var setLastId = data.OrderBy(x => x.Id).Last().Id + 1;

            if (id > setLastId || id == 0 || _repository.GetPokemonById(id) == null)
                return NotFound("Pokémon não encontrado");

            return Ok(_repository.RemovePokemon(id));
            
        }
    }
}