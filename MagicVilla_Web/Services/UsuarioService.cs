using MagicVilla_Utilidad;
using MagicVilla_Web.Models;
using MagicVilla_Web.Models.Dto;
using MagicVilla_Web.Services.IServices;

namespace MagicVilla_Web.Services
{
    public class UsuarioService : BaseService, IUsuarioService
    {
        private string _villaUrl;
        public UsuarioService(IHttpClientFactory httpClient, IConfiguration conf): base(httpClient)
        {
            _httpClient = httpClient;
            _villaUrl = conf.GetValue<string>("ServiceUrls:API_URL");
        }

        Task<T> IUsuarioService.Login<T>(LoginRequestDto dto)
        {
            return SendAsync<T>(new APIRequest()
            {
                APITipo = DS.APITipo.POST,
                Datos = dto,
                Url = _villaUrl + "/usuario/login"
            });
        }

        Task<T> IUsuarioService.Registrar<T>(RegistroRequestDto dto)
        {
            return SendAsync<T>(new APIRequest()
            {
                APITipo = DS.APITipo.POST,
                Datos = dto,
                Url = _villaUrl + "/usuario/registrar"
            });
        }
    }
}
