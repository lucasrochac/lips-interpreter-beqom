using LispInterpreter.Service;
using Microsoft.AspNetCore.Mvc;

namespace LispInterpreter.Api.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class LispInterpreterController : ControllerBase
    {
        private readonly ILogger<LispInterpreterController> _logger;
        private readonly ILispInterpreterService _service;

        public LispInterpreterController(
            ILogger<LispInterpreterController> logger,
            ILispInterpreterService service)
        {
            _service = service;
            _logger = logger;
        }

        [HttpPost(Name = "Evaluate")]
        public IActionResult Evaluate([FromBody] List<string> expressions)
        {
            try
            {
                var result = _service.InterpretExpression(expressions);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest($"Iterpretation failed: {ex.Message}");
            }
        }
    }
}