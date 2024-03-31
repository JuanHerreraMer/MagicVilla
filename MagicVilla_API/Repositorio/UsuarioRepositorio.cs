using AutoMapper;
using MagicVilla_API.Datos;
using MagicVilla_API.Modelos;
using MagicVilla_API.Modelos.Dto;
using MagicVilla_API.Repositorio.IRepositorio;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace MagicVilla_API.Repositorio
{
    public class UsuarioRepositorio : IUsuarioRepositorio
    {
        private readonly ApplicationDbContext _db;
        private string secretKey;
        private readonly UserManager<UsuarioAplicacion> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly IMapper _mapper;

        public UsuarioRepositorio(ApplicationDbContext db, IConfiguration conf, UserManager<UsuarioAplicacion> userManager, IMapper mapper, RoleManager<IdentityRole> roleManager)
        {
            _db = db;
            secretKey = conf.GetValue<string>("ApiSettings:Secret");
            _userManager = userManager;
            _mapper = mapper;
            _roleManager = roleManager;
        }
        public bool IsUsuarioUnico(string userName)
        {
            var usuario = _db.usuarioAplicacion.FirstOrDefault(u => u.UserName.ToLower() == userName.ToLower());
            if (usuario == null)
            {
                return true;
            }
            return false;
        }

        public async Task<LoginResponseDto> Login(LoginRequestDto loginRequestDto)
        {
            var usuario = await _db.usuarioAplicacion.FirstOrDefaultAsync(u => u.UserName.ToLower() == loginRequestDto.UserName.ToLower());

            bool isValid = await _userManager.CheckPasswordAsync(usuario, loginRequestDto.Password);

            if (usuario == null && isValid == false)
            {
                return new LoginResponseDto
                {
                    Token = "",
                    Usuario = null
                };
            }

            var roles = await _userManager.GetRolesAsync(usuario);

            var token = GenerateJwtToken(usuario, roles);
            var loginResponseDto = new LoginResponseDto
            {
                Token = token,
                Usuario = _mapper.Map<UsuarioDto>(usuario)
            };

            return loginResponseDto;
        }

        public async Task<UsuarioDto> Registrar(RegistroRequestDto registroRequestDto)
        {
            UsuarioAplicacion usuario = new()
            {
                UserName = registroRequestDto.UserName,
                Email = registroRequestDto.UserName,
                NormalizedEmail = registroRequestDto.UserName.ToUpper(),
                Nombres = registroRequestDto.Nombres,
            };

            try
            {
                var resultado = await _userManager.CreateAsync(usuario, registroRequestDto.Password);
                if (resultado.Succeeded)
                {
                    if (!_roleManager.RoleExistsAsync("admin").GetAwaiter().GetResult())
                    {
                        await _roleManager.CreateAsync(new IdentityRole("admin"));
                    }
                    await _userManager.AddToRoleAsync(usuario, "admin");
                    var usuarioAp = _db.usuarioAplicacion.FirstOrDefault(u => u.UserName == registroRequestDto.UserName);
                    return _mapper.Map<UsuarioDto>(usuarioAp);
                }
            }
            catch (Exception ex)
            {

                throw;
            }

            return new UsuarioDto();

        }


        private string GenerateJwtToken(UsuarioAplicacion usuario, IList<string> roles)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(secretKey);
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new Claim[]
                {
                    new Claim(ClaimTypes.Name, usuario.UserName),
                    new Claim(ClaimTypes.Role, roles.FirstOrDefault())
                }),
                Expires = DateTime.UtcNow.AddDays(7),
                SigningCredentials = new(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature),
            };

            var token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);
        }
    }
}
