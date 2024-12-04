using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using marvelapicomics.Models;

// you can minimize the type information in this class. Controllers in general should not carry much logic. Here you can abstract Generic to MarvelAPI Service. Your services should impply on what can be acheived with the API with the app.
// you should not need two Route classes your program.cs Added the default
[Route("[controller]")]
public class HomeController : Controller
{
    private readonly ILogger<HomeController> _logger;
    private readonly MarvelApiService _MarvelApiService;

    public HomeController(ILogger<HomeController> logger, MarvelApiService MarvelApiService)
    {
        _logger = logger;
        _MarvelApiService = MarvelApiService;
    }

    public async Task<IActionResult> IndexAsync()
    {

        var response = await _MarvelApiService.GetAsync<MarvelApiResponse<Character>>("characters");

        var filtered_results = response.Data.Results
                                .Where(c => !c.Thumbnail.Path.Contains("image_not_available") && !string.IsNullOrEmpty(c.Description))
                                .ToList();


        return View(filtered_results);
    }

    // Type of request is more important than the route. Here it renders a page hence assuming a get request
    [HttpGet("myhero")]
    // Pascal case preferred naming convention use it.
    public IActionResult myhero()
    {
        return View();
    }

    [HttpGet("getcharacters/{query}")]
    public async Task<IActionResult> Getcharacters([FromRoute] string query)
    {
        var response = await _MarvelApiService.getcharacters<MarvelApiResponse<Character>>(query);
        var filterd_results = response.Data.Results.Select(c => new { c.Name, c.Id }).ToList();

        return Ok(filterd_results);
    }
    // dont need to use Route and Get seperately use one as below
    [HttpGet("getcharactersByName/{query}")]
    public async Task<IActionResult> charactersByName([FromRoute] string query)
    {
        var response = await _MarvelApiService.getcharactersByName<MarvelApiResponse<Character>>(query);
        var filterd_results = response.Data.Results.Select(c => c.Id).FirstOrDefault();

        return RedirectToAction("Characters", new { id = filterd_results.ToString() });
    }

    [Route("characters/{id}")]
    [HttpGet]
    public async Task<IActionResult> characters([FromRoute] string id)
    {
        if (string.IsNullOrWhiteSpace(id))
        {
            return BadRequest(new { Message = "Character ID cannot be null or empty." });
        }
        var characterTask = _MarvelApiService.GetAsync<MarvelApiResponse<Character>>($"characters/{id}");
        // var comicTask =  _MarvelApiService.GetAsync<MarvelApiResponse<comicitem>>($"characters/{id}/comics");
        // var seriesTask =  _MarvelApiService.GetAsync<MarvelApiResponse<SeriesList>>($"characters/{id}/series");

        await Task.WhenAll(characterTask);
        var characterresponse = await characterTask;
        // var comicresponse = await comicTask;
        // var seriesresponse = await seriesTask;

        var viewmodel = new CharComicModel()
        {
            character = characterresponse.Data.Results.FirstOrDefault(c => c.Id.ToString() == id),
            // comics= comicresponse.Data.Results. Where(c=> c.thumbnail !=null && !string.IsNullOrEmpty(c.Description) && !c.thumbnail.Path.Contains("image_not_available") && !string.IsNullOrEmpty(c.thumbnail.Path)).Take(4).ToList(),
            // series= seriesresponse.Data.Results. Where(c=> c.thumbnail !=null && !string.IsNullOrEmpty(c.Description) && !c.thumbnail.Path.Contains("image_not_available") && !string.IsNullOrEmpty(c.thumbnail.Path)).Take(4).ToList()

        };
        return View(viewmodel);
    }

    // Fetch comics data
    [Route("characters/{id}/charactercomics")]
    public async Task<IActionResult> GetComics([FromRoute] string id)
    {
        var comicResponse = await _MarvelApiService.GetAsync<MarvelApiResponse<comicitem>>($"characters/{id}/comics");


        var comics = comicResponse.Data.Results
            .Where(c => c.thumbnail != null && !string.IsNullOrEmpty(c.Description) && !c.thumbnail.Path.Contains("image_not_available"))
            .Take(4)
            .ToList();
        if (comics.Count == 0)
        {
            return Json(new { message = "No comics found" }); // Return an empty list or a message
        }


        return Json(comics); // Return the comics data as JSON
    }

    // Fetch series data
    [Route("characters/{id}/characterseries")]
    public async Task<IActionResult> GetSeries([FromRoute] string id)
    {
        var seriesResponse = await _MarvelApiService.GetAsync<MarvelApiResponse<SeriesList>>($"characters/{id}/series");
        var series = seriesResponse.Data.Results
            .Where(s => s.thumbnail != null && !string.IsNullOrEmpty(s.Description) && !s.thumbnail.Path.Contains("image_not_available"))
            .Take(4)
            .ToList();
        if (series.Count == 0)
        {
            return Json(new { message = "No series found" }); // Return an empty list or a message
        }

        return Json(series); // Return the series data as JSON
    }

    [Route("characters/{id}/comics")]
    [HttpGet]
    public async Task<IActionResult> CharacterComics([FromRoute] string id, [FromQuery] int limit = 90, [FromQuery] int offset = 0)
    {
        if (string.IsNullOrWhiteSpace(id))
        {
            return BadRequest(new { Message = "Character ID cannot be null or empty." });
        }
        var response = await _MarvelApiService.GetAsync<MarvelApiResponse<comicitem>>($"characters/{id}/comics", limit, offset);
        return View(response.Data);
    }
    
    [Route("characters/{id}/series")]
    [HttpGet]
    public async Task<IActionResult> CharacterSeries([FromRoute] string id, [FromQuery] int limit = 90, [FromQuery] int offset = 0)
    {
        if (string.IsNullOrWhiteSpace(id))
        {
            return BadRequest(new { Message = "Character ID cannot be null or empty." });
        }
        var response = await _MarvelApiService.GetAsync<MarvelApiResponse<SeriesList>>($"characters/{id}/series", limit, offset);
        return View(response.Data);
    }

    [Route("series/{id}")]
    [HttpGet]
    public async Task<IActionResult> Series([FromRoute] string id)
    {
        if (string.IsNullOrWhiteSpace(id))
        {
            return BadRequest(new { Message = "Character ID cannot be null or empty." });
        }
        var response = await _MarvelApiService.GetAsync<MarvelApiResponse<SeriesList>>($"series/{id}");
        return View(response.Data.Results.FirstOrDefault());
    }

    [Route("comics/{id}")]
    [HttpGet]
    public async Task<IActionResult> Comics([FromRoute] string id)
    {
        if (string.IsNullOrWhiteSpace(id))
        {
            return BadRequest(new { Message = "Character ID cannot be null or empty." });
        }
        var response = await _MarvelApiService.GetAsync<MarvelApiResponse<comicitem>>($"comics/{id}");
        return View(response.Data.Results.FirstOrDefault());
    }


    [Route("privacy")]
    public IActionResult Privacy()
    {
        return View();
    }

    [Route("error")]
    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}

