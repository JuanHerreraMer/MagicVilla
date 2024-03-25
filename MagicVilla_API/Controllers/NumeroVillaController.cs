using AutoMapper;
using MagicVilla_API.Datos;
using MagicVilla_API.Modelos;
using MagicVilla_API.Modelos.Dto;
using MagicVilla_API.Repositorio.IRepositorio;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Net;
using System.Reflection.Metadata.Ecma335;

namespace MagicVilla_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class NumeroVillaController : ControllerBase
    {
        private readonly ILogger<NumeroVillaController> _logger;
        private readonly IVillaRepositorio _villaRepo;
        private readonly INumeroVillaRepositorio _numeroRepo;
        private readonly IMapper _mapper;
        protected APIResponse _response;

        public NumeroVillaController(ILogger<NumeroVillaController> logger, IVillaRepositorio villaRepo, INumeroVillaRepositorio numeroRepo, IMapper mapper)
        {
            _logger = logger;
            _villaRepo = villaRepo;
            _numeroRepo = numeroRepo;
            _mapper = mapper;
            _response = new();
        }

        [HttpGet]
        [ProducesResponseType(200)]
        public async Task<ActionResult<APIResponse>> GetNumeroVillas()
        {
            try
            {
                _logger.LogInformation("Obtener los numeros de villas");

                IEnumerable<NumeroVilla> numeroVillaList = await _numeroRepo.ObtenerTodos(incluirPropiedades: "Villa");
                _response.Resultado = _mapper.Map<IEnumerable<NumeroVillaDto>>(numeroVillaList);
                _response.statusCode = HttpStatusCode.OK;
                //_response.isExitoso = true;

                return Ok(_response);
            }
            catch (Exception ex)
            {
                _response.isExitoso = false;
                _response.errorMessages = new List<string>() { ex.ToString() };
            }

            return _response;
        }

        [HttpGet("{id:int}", Name = "GetNumeroVilla")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        public async Task<ActionResult<APIResponse>> GetNumeroVilla(int id)
        {
            try
            {
                if (id == 0)
                {
                    _response.statusCode = HttpStatusCode.BadRequest;
                    _response.isExitoso=false;
                    return BadRequest(_response);
                }

                var numeroVilla = await _numeroRepo.Obtener(v => v.VillaNo == id, incluirPropiedades: "Villa");

                if (numeroVilla == null)
                {
                    _response.statusCode = HttpStatusCode.NotFound;
                    _response.isExitoso = false;
                    return NotFound(_response);
                }
                _response.Resultado = _mapper.Map<NumeroVillaDto>(numeroVilla);
                //_response.isExitoso = true;
                _response.statusCode = HttpStatusCode.OK;

                return Ok(_response);
            }
            catch (Exception ex)
            {
                _response.isExitoso = false;
                _response.errorMessages = new List<string>() { ex.ToString() };
            }

            return _response;
        }

        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(400)]
        [ProducesResponseType(500)]
        public async Task<ActionResult<APIResponse>> CrearNumeroVilla([FromBody] NumeroVillaCreateDto createDto)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                if (await _numeroRepo.Obtener(v => v.VillaNo == createDto.VillaNo) != null)
                {
                    ModelState.AddModelError("ErrorMessages", "Ya está creado");
                    return BadRequest(ModelState);
                }

                if(await _villaRepo.Obtener(v=>v.Id == createDto.VillaId) == null)
                {
                    ModelState.AddModelError("ErrorMessages", "ID Villa no existe");
                    return BadRequest(ModelState);
                }

                if (createDto == null) return BadRequest(createDto);

                NumeroVilla modelo = _mapper.Map<NumeroVilla>(createDto);
                modelo.FechaCreacion = DateTime.Now;
                modelo.FechaActualizacion = DateTime.Now;

                await _numeroRepo.Crear(modelo);
                _response.Resultado = modelo;
                _response.statusCode = HttpStatusCode.Created;

                return CreatedAtRoute("GetVilla", new { id = modelo.VillaNo }, _response);
            }
            catch (Exception ex)
            {
                _response.isExitoso = false;
                _response.errorMessages = new List<string>() { ex.ToString() };
            }

            return _response;
        }

        [HttpDelete("{id:int}")]
        [ProducesResponseType(204)]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        public async Task<IActionResult> DeleteNumeroVilla(int id)
        {
            try
            {
                if (id == 0)
                {
                    _response.isExitoso=false;
                    _response.statusCode=HttpStatusCode.BadRequest;
                    return BadRequest(_response);
                }

                var numeroVilla = await _numeroRepo.Obtener(v => v.VillaNo == id);
                if (numeroVilla == null)
                {
                    _response.isExitoso=false;
                    _response.statusCode=HttpStatusCode.NotFound;
                    return NotFound(_response);
                }

                await _numeroRepo.Remover(numeroVilla);

                _response.statusCode = HttpStatusCode.NoContent;

                return Ok(_response);
            }
            catch (Exception ex)
            {
                _response.isExitoso = false;
                _response.errorMessages = new List<string>() { ex.ToString() };
            }

            return BadRequest(_response);
        }

        [HttpPut("{id:int}")]
        [ProducesResponseType(204)]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        public async Task<IActionResult> UpdateNumeroVilla(int id, [FromBody] NumeroVillaUpdateDto updateDto)
        {
            try
            {
                if (updateDto == null || id != updateDto.VillaNo)
                {
                    _response.isExitoso = false;
                    _response.statusCode=HttpStatusCode.BadRequest;
                    return BadRequest(_response);
                }

                if (await _villaRepo.Obtener(v => v.Id == updateDto.VillaId) == null)
                {
                    ModelState.AddModelError("ErrorMessages", "ID Villa no existe");
                    return BadRequest(ModelState);
                }

                NumeroVilla modelo = _mapper.Map<NumeroVilla>(updateDto);

                await _numeroRepo.Actualizar(modelo);
                _response.statusCode = HttpStatusCode.NoContent;

                return Ok(_response);
            }
            catch (Exception ex)
            {
                _response.isExitoso = false;
                _response.errorMessages = new List<string>() { ex.ToString() };
            }

            return BadRequest(_response);
        }

    }
}
