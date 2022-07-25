using Microsoft.AspNetCore.Http;

namespace _101SoftwareTest.Entities
{
    public class ImagePropertyDto
    {
        public int Id { get; set; }
        public int IdPropiedad { get; set; }
        public string ImagenNombre { get; set; }
        public string Imagen { get; set; }
        public bool Enabled { get; set; }
    }
}
