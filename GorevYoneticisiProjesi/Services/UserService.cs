using GorevYoneticisiProjesi.Data;
using GorevYoneticisiProjesi.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.Extensions.Configuration;
using BCrypt.Net;


namespace GorevYoneticisiProjesi.Services
{
  
    public class UserService : IUserService
    {
        private readonly AppDbContext _context;
        private readonly IConfiguration _config;

        public UserService(AppDbContext context, IConfiguration config)
        {
            _context = context;
            _config = config;
        }
        //yeni kullanıcı oluştur
        public async Task<User> RegisterAsync(string username, string password)
        {
            // Aynı kullanıcı adı var mı kontrol et
            var existingUser = await _context.Users.FirstOrDefaultAsync(u => u.Username == username);
            if (existingUser != null)
                throw new Exception("Bu kullanıcı adı zaten kayıtlı.");

            var user = new User { Id = Guid.NewGuid(), Username = username, PasswordHash = BCrypt.Net.BCrypt.HashPassword(password) };
            _context.Users.Add(user);
            await _context.SaveChangesAsync();
            return user;
        }
        // Kullanıcı adı(Öncelikle kullanıcı adı ile veri tabanından kullanıcıyı çeker) ve parolayı doğrula(BCrypt ile)
        public async Task<User> ValidateUserAsync(string username, string password)
        {
            var user = await _context.Users.SingleOrDefaultAsync(u => u.Username == username);
            if (user != null && BCrypt.Net.BCrypt.Verify(password, user.PasswordHash))
                return user;
            throw new UnauthorizedAccessException("Kullanıcı adı veya parola hatalı.");
        }
        //Kullanıcı için JWT (JSON Web Token) üretir.appsettings.json içindeki Jwt bölümünden ayarları alır
        public Task<string> GenerateJwtTokenAsync(User user)
        {
            var jwtSettings = _config.GetSection("Jwt");
            var keyString = jwtSettings["Key"];
            if (string.IsNullOrEmpty(keyString))
                throw new InvalidOperationException("Jwt:Key ayarı bulunamadı.");

            var issuer = jwtSettings["Issuer"];
            var audience = jwtSettings["Audience"];
            if (string.IsNullOrEmpty(issuer) || string.IsNullOrEmpty(audience))
                throw new InvalidOperationException("Jwt:Issuer veya Jwt:Audience ayarı bulunamadı.");

            if (!double.TryParse(jwtSettings["ExpireMinutes"], out var expireMinutes))
            {
                expireMinutes = 60;
            }

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(keyString));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var now = DateTime.UtcNow;
            var claims = new[]
            {
        new Claim(JwtRegisteredClaimNames.Sub, user.Username),
        new Claim("id", user.Id.ToString()),
        new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
        new Claim(JwtRegisteredClaimNames.Iat, new DateTimeOffset(now).ToUnixTimeSeconds().ToString(), ClaimValueTypes.Integer64)
    };
            var expires = now.AddMinutes(expireMinutes);

            var token = new JwtSecurityToken(
                issuer: issuer,
                audience: audience,
                claims: claims,
                notBefore: now,
                expires: expires,
                signingCredentials: creds);

            var tokenString = new JwtSecurityTokenHandler().WriteToken(token);
            return Task.FromResult(tokenString);
        }

        //Kullanıcıyı ID’si ile veri tabanından getirir.
        public async Task<User?> GetByIdAsync(Guid id)
        {
            return await _context.Users.FindAsync(id);
        }
    }
}
