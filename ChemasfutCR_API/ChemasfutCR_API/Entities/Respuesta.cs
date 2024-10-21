namespace ChemasfutCR_API.Entities
{
    public class Respuesta
    {
        public int Codigo { get; set; } = 0;
        public string? Mensaje { get; set; } = "Ok";
        public object? Contenido { get; set; }
    }
}
