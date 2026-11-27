using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ReportesApi.DTOs;
using ReportesApi.Extensions;
using ReportesApi.Models;
using ReportesApi.Services.Interfaces;

namespace ReportesApi.Controllers;

[ApiController]
[Route("api/categorias")]
[Authorize]
[Produces("application/json")]
public class CategoriasController : ControllerBase
{
    private readonly ICategoriaService _service;

    public CategoriasController(ICategoriaService service) => _service = service;

    /// <summary>Lista categorías activas. El administrador puede pedir también las inactivas.</summary>
    [HttpGet]
    [ProducesResponseType(typeof(List<CategoriaResponseDto>), StatusCodes.Status200OK)]
    public async Task<ActionResult<List<CategoriaResponseDto>>> GetAll([FromQuery] bool incluirInactivas = false)
    {
        // Un ciudadano nunca ve categorías desactivadas aunque mande el parámetro
        var verInactivas = incluirInactivas && User.EsAdministrador();
        return Ok(await _service.GetAllAsync(verInactivas));
    }

    [HttpGet("{id:int}")]
    [ProducesResponseType(typeof(CategoriaResponseDto), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ErrorResponseDto), StatusCodes.Status404NotFound)]
    public async Task<ActionResult<CategoriaResponseDto>> GetById(int id) =>
        Ok(await _service.GetByIdAsync(id));

    [HttpPost]
    [Authorize(Roles = Rol.Administrador)]
    [ProducesResponseType(typeof(CategoriaResponseDto), StatusCodes.Status201Created)]
    [ProducesResponseType(typeof(ErrorResponseDto), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ErrorResponseDto), StatusCodes.Status403Forbidden)]
    [ProducesResponseType(typeof(ErrorResponseDto), StatusCodes.Status409Conflict)]
    public async Task<ActionResult<CategoriaResponseDto>> Create([FromBody] CategoriaCreateDto dto)
    {
        var creada = await _service.CreateAsync(dto);
        return CreatedAtAction(nameof(GetById), new { id = creada.Id }, creada);
    }

    /// <summary>Edita una categoría o la desactiva enviando "activa": false.</summary>
    [HttpPut("{id:int}")]
    [Authorize(Roles = Rol.Administrador)]
    [ProducesResponseType(typeof(CategoriaResponseDto), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ErrorResponseDto), StatusCodes.Status404NotFound)]
    [ProducesResponseType(typeof(ErrorResponseDto), StatusCodes.Status409Conflict)]
    public async Task<ActionResult<CategoriaResponseDto>> Update(int id, [FromBody] CategoriaUpdateDto dto) =>
        Ok(await _service.UpdateAsync(id, dto));
}