using Microsoft.AspNetCore.Mvc;
using WorldRank.Api.Dtos;
using WorldRank.Application;
using WorldRank.Domain;

namespace WorldRank.Api.Controllers;

[ApiController]
[Route("wallets")]
public class WalletsController : ControllerBase
{
	private readonly IWalletService _wallets;

	public WalletsController(IWalletService wallets) => _wallets = wallets;

	[HttpPost]
	public async Task<IActionResult> Create([FromBody] CreateWalletRequest request, CancellationToken ct)
	{
		try
		{
			var wallet = await _wallets.Create(request.PlayerId, request.Currency, request.InitialBalance, ct);
			return CreatedAtAction(nameof(GetById), new { id = wallet.Id }, WalletResponse.From(wallet));
		}
		catch (PlayerNotFoundException ex)
		{
			return NotFound(new { error = ex.Message });
		}
		catch (WalletException ex)
		{
			return BadRequest(new { error = ex.Message });
		}
	}

	[HttpGet("{id:int}")]
	public async Task<IActionResult> GetById(int id, CancellationToken ct)
	{
		var wallet = await _wallets.GetById(id, ct);
		return wallet is null ? NotFound() : Ok(WalletResponse.From(wallet));
	}

	[HttpPost("{id:int}/deposit")]
	public async Task<IActionResult> Deposit(int id, [FromBody] DepositRequest request, CancellationToken ct)
	{
		try
		{
			var wallet = await _wallets.Deposit(id, request.Amount, ct);
			return wallet is null ? NotFound() : Ok(WalletResponse.From(wallet));
		}
		catch (WalletException ex)
		{
			return BadRequest(new { error = ex.Message });
		}
	}
}
