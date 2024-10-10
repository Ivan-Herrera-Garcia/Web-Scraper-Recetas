namespace Web_Scraper_Recetas.Models
{
    public class Receta
    {
        //Atributos de la clase a scrapear
        public Receta()
        {
            this.Id = null;
            this.Name = null;
            this.Description = null;
            this.Imagenes = null;
            this.Created = null;
            this.IsVisible = null;
            this.InfoAditional = null;
            this.TimeToMake = null;
            this.NumberIngredients = null;
            this.portions = null;
        }

        public Receta(int? id, string? name, string? description, string? imagenes, DateTime? created, bool? isVisible, string? infoAditional, string? timeToMake, int? numberIngredients, string? portions)
        {
            Id = id;
            Name = name;
            Description = description;
            Imagenes = imagenes;
            Created = created;
            IsVisible = isVisible;
            InfoAditional = infoAditional;
            TimeToMake = timeToMake;
            NumberIngredients = numberIngredients;
            this.portions = portions;
        }

        public int? Id { get; set; }
        public string? Name { get; set; }
        public string? Description { get; set; }

        public string? Imagenes { get; set; }
        public DateTime? Created { get; set; }
        public bool? IsVisible { get; set; }
        public string? InfoAditional { get; set; }
        public string? TimeToMake { get; set; }
        public int? NumberIngredients { get; set; }
        public string? portions { get; set; }
    }
}
