using Microsoft.AspNetCore.Mvc;

namespace ApiRetry.Controllers;

[ApiController]
[Route("[controller]")]
public class ServiceRequestsController: ControllerBase
{
    private readonly ILogger<ServiceRequestsController> _logger;

    public ServiceRequestsController(ILogger<ServiceRequestsController> logger)
    {
        _logger = logger;
    }

    [HttpGet(Name = "GetData")]
    public async Task<IActionResul> Get()
    {
        await ConnectToApi();
        return Ok();
    }


    private async Task ConnectToApi()
    {
        var url = "https://chuck-norris-jokes.p.rapidapi.com/de/jokes/random";

        var client = new RestClient();
        var request = new RestRequest(url, Method.GET);
        request.AddHeader("X-RapidAPI-Key", "SIGN-UP-FOR-KEY");
        request.AddHeader("X-RapidAPI-Host", "chuck-norris-jokes.p.rapidapi.com");
        IRestResponse response =  await client.Execute(request);


        if(response.IsSuccessful)
        {
            Console.WriteLine(response.Content);
        }
        else
        {
            Console.Write(response.ErrorMessage);
        }

    }

}