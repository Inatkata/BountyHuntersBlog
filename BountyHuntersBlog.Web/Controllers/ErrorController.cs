using Microsoft.AspNetCore.Mvc;

public class ErrorController : Controller
{
    [Route("error/404")]
    public IActionResult NotFoundPage() => View("NotFound");

    [Route("error/500")]
    public IActionResult ServerError() => View("ServerError");
}