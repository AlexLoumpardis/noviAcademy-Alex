using Microsoft.AspNetCore.Mvc;
using WorldRank.Api.Dtos;
using WorldRank.Application;
using WorldRank.Domain;

namespace WorldRank.Api.Controllers;

[ApiController]
[Route("players")]
public class PlayersController : ControllerBase
{
	private readonly IPlayerService _players;

	public PlayersController(IPlayerService players) => _players = players;

	[HttpPost]
	public async Task<IActionResult> Create([FromBody] CreatePlayerRequest request, CancellationToken ct)
	{
		Player player;
		try
		{
			player = await _players.Create(request.Name, request.Score, ct);
		}
		catch (ArgumentException ex)
		{
			return BadRequest(new { error = ex.Message });
		}

		return CreatedAtAction(nameof(GetById), new { id = player.Id }, PlayerResponse.From(player));
	}

	[HttpGet("{id:int}")]
	public async Task<IActionResult> GetById(int id, CancellationToken ct)
	{
		var player = await _players.GetById(id, ct);
		return player is null ? NotFound() : Ok(PlayerResponse.From(player));
	}

	[HttpGet]
	public async Task<IActionResult> GetAll(CancellationToken ct)
	{
		var players = await _players.GetAll(ct);
		return Ok(players.Select(PlayerResponse.From));
	}
}
