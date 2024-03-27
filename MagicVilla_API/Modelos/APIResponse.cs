using System.Net;

namespace MagicVilla_API.Modelos
{
    public class APIResponse
    {
        public APIResponse()
        {
            errorMessages = new List<string>();
        }
        public HttpStatusCode statusCode { get; set; }
        public bool isExitoso { get; set; } = true;
        public List<string> errorMessages { get; set; }

        //Prop object Resultado es usado para retornar cualquier tipo de lista, objecto propiamente tal u otra cosa
        //sería como un "Generico" o comodín
        public object Resultado { get; set; }
    }
}
