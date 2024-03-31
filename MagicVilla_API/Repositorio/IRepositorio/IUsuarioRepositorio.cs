using MagicVilla_API.Modelos;
using MagicVilla_API.Modelos.Dto;

namespace MagicVilla_API.Repositorio.IRepositorio
{
    public interface IUsuarioRepositorio
    {
        bool IsUsuarioUnico(string userName);
        Task<LoginResponseDto> Login(LoginRequestDto loginRequestDto);
        Task<UsuarioDto> Registrar(RegistroRequestDto registroRequestDto);
    }
}
