namespace Web_Scraper_Recetas.Models
{
    public class Receta
    {
        //Atributos de la clase a scrapear
        public Receta()
        {
            Id = null;
            this.Name = null;
            this.Description = null;
            this.Imagen = null;
            this.Created = null;
            this.IsVisible = null;
            this.InfoAditional = null;
        }

        public Receta(int? id, string? name, string? description, string? imagen, DateTime? created, bool? isVisible, string? infoAditional)
        {
            Id = id;
            Name = name;
            Description = description;
            Imagen = imagen;
            Created = created;
            IsVisible = isVisible;
            InfoAditional = infoAditional;
        }

        public int? Id { get; set; }
        public string? Name { get; set; }
        public string? Description { get; set; }

        public string? Imagen { get; set; }
        public DateTime? Created { get; set; }
        public bool? IsVisible { get; set; }
        public string? InfoAditional { get; set; }
    }
}
