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
    public async Task<IActionResult> Get()
    {

        //Se puede implementar una política por cada tipo de excepción, en este caso 
        //se hace para una excepción en general
        var retryPolicy = retryPolic
            .Handle<Exception>()
            .RetryAsync(5, onRetry: (exception, retryCount) =>
            {
                Console.WriteLine("Error:" + exception.Message + " ... Retry Count " + retryCount);
            });


        await retryPolicy.ExecuteAsync(async () =>
        {
            await ConnectToApi();
        })
        


      
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
            throw new Exception("Unable to connect to the service");
        }

    }

}