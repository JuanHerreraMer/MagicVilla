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
        public Task<T> Actualizar<T>(NumeroVillaUpdateDto dto)
        {
            return SendAsync<T>(new APIRequest()
            {
                APITipo = DS.APITipo.PUT,
                Datos = dto,
                Url = _numeroVillaUrl + "/api/NumeroVilla/"+dto.VillaNo
            });
        }

        public Task<T> Crear<T>(NumeroVillaCreateDto dto)
        {
            return SendAsync<T>(new APIRequest()
            {
                APITipo = DS.APITipo.POST,
                Datos = dto,
                Url = _numeroVillaUrl + "/api/NumeroVilla"
            });
        }

        public Task<T> Obtener<T>(int id)
        {
            return SendAsync<T>(new APIRequest()
            {
                APITipo = DS.APITipo.GET,
                Url = _numeroVillaUrl + "/api/NumeroVilla/" + id
            });
        }

        public Task<T> ObtenerTodos<T>()
        {
            return SendAsync<T>(new APIRequest()
            {
                APITipo = DS.APITipo.GET,
                Url = _numeroVillaUrl + "/api/NumeroVilla/"
            });
        }

        public Task<T> Remover<T>(int id)
        {
            return SendAsync<T>(new APIRequest()
            {
                APITipo = DS.APITipo.DELETE,
                Url = _numeroVillaUrl + "/api/NumeroVilla/" + id
            });
        }
    }
}
