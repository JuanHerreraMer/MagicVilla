using MagicVilla_Utilidad;
using MagicVilla_Web.Models;
using MagicVilla_Web.Models.Dto;
using MagicVilla_Web.Services.IServices;

namespace MagicVilla_Web.Services
{
    public class NumeroVillaService : BaseService, INumeroVillaService
    {
        //public readonly IHttpClientFactory _httpClient;
        private string _numeroVillaUrl;
        public NumeroVillaService(IHttpClientFactory httpClient, IConfiguration configuration) :base(httpClient)
        {
            _httpClient = httpClient;

            _numeroVillaUrl = configuration.GetValue<string>("ServiceUrls:API_URL");

        }
        public Task<T> Actualizar<T>(NumeroVillaUpdateDto dto, string token)
        {
            return SendAsync<T>(new APIRequest()
            {
                APITipo = DS.APITipo.PUT,
                Datos = dto,
                Url = _numeroVillaUrl + "/api/NumeroVilla/"+dto.VillaNo,
                Token = token
            });
        }

        public Task<T> Crear<T>(NumeroVillaCreateDto dto, string token)
        {
            return SendAsync<T>(new APIRequest()
            {
                APITipo = DS.APITipo.POST,
                Datos = dto,
                Url = _numeroVillaUrl + "/api/NumeroVilla",
                Token = token
            });
        }

        public Task<T> Obtener<T>(int id, string token)
        {
            return SendAsync<T>(new APIRequest()
            {
                APITipo = DS.APITipo.GET,
                Url = _numeroVillaUrl + "/api/NumeroVilla/" + id,
                Token = token
            });
        }

        public Task<T> ObtenerTodos<T>(string token)
        {
            return SendAsync<T>(new APIRequest()
            {
                APITipo = DS.APITipo.GET,
                Url = _numeroVillaUrl + "/api/NumeroVilla/",
                Token = token
            });
        }

        public Task<T> Remover<T>(int id, string token)
        {
            return SendAsync<T>(new APIRequest()
            {
                APITipo = DS.APITipo.DELETE,
                Url = _numeroVillaUrl + "/api/NumeroVilla/" + id,
                Token = token
            });
        }
    }
}
