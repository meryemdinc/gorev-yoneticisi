// Controllers/AuthController.cs
using GorevYoneticisiProjesi.Models;
using GorevYoneticisiProjesi.Services;
using Microsoft.AspNetCore.Mvc;
using GorevYoneticisiProjesi.Dto;
namespace GorevYoneticisiProjesi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly IUserService _userService;

        public AuthController(IUserService userService)
        {
            _userService = userService;
        }
        //kayıt ayarları
        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegisterDto dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            try
            {
                var user = await _userService.RegisterAsync(dto.Username, dto.Password);

                // Burada UserResponseDto sınıfı kullanmak yerine direkt anonim objeye dönüyoruz:
                var result = new
                {
                    Id = user.Id,
                    Username = user.Username
                   
                };

                // 201 Created dönmek isterseniz:
                // return Created(string.Empty, result);
                // veya direkt Ok:
                return Ok(result);
            }
            catch (Exception ex)
            {
                return Conflict(new { message = ex.Message });
            }
        }
        //login ayarları
        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginDto dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            try
            {
                var user = await _userService.ValidateUserAsync(dto.Username, dto.Password);
                var token = await _userService.GenerateJwtTokenAsync(user);
                return Ok(new { token });
            }
            catch (UnauthorizedAccessException)
            {
                return Unauthorized(new { message = "Kullanıcı adı veya parola hatalı." });
            }
            catch (Exception)
            {
                return StatusCode(500, new { message = "Beklenmedik bir hata oluştu." });
            }
        }
    }
}
