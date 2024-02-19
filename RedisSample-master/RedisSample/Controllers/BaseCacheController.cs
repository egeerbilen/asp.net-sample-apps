using Microsoft.AspNetCore.Mvc;
using RedisSample.Interfaces;
using RedisSample.Model;
using StackExchange.Redis;

namespace RedisSample.Controllers;

[ApiController]
[Route("api")]
public class BaseCacheController : ControllerBase
{
    private readonly IBaseCacheService _cacheService;

    public BaseCacheController(IBaseCacheService cacheService)
    {
        _cacheService = cacheService;
    }
    [HttpGet("cache")]
    public async Task<IActionResult> Get(string key)
    {
        return Ok(await _cacheService.GetValueAsync(key));
    }
   
    [HttpPost("cache")]
    public async Task<IActionResult> Post([FromBody] CacheRequestModel model) // [FromBody] �zniteli�i, ASP.NET Core'da bir parametrenin de�erinin iste�in g�vdesinden ba�lanmas�n� belirtmek i�in kullan�l�r. 
    {
        await _cacheService.SetValueAsync(model.Key, model.Value);
        return Ok();
    }

    [HttpDelete("cache")]
    public async Task<IActionResult> Delete(string key)
    {
        await _cacheService.Clear(key);
        return Ok();
    }
}
