using HtmlAgilityPack;
using Newtonsoft.Json;
using Web_Scraper_Recetas.Models;

namespace Web_Scraper_Recetas.Scraper
{
    public class Raspador
    {
        public static List<Receta> recetas = [];
        public static int id = 1;
        public static DateTime fechaActual = DateTime.Now;
        public static async Task<List<Receta>> Main()
        {
            int page = 1;
            bool hasNextPage = true;
            while (page <= 3)
            {
                hasNextPage = await ScrapMenu("https://cookpad.com/mx/buscar/listas?page=1");
                page++;
            }
            return recetas;
        }

        public static async Task<bool> ScrapMenu(string url)
        {
            var client = new HttpClient();
            var request = new HttpRequestMessage(HttpMethod.Get, url);
            var response = await client.SendAsync(request);
            if (response.EnsureSuccessStatusCode().IsSuccessStatusCode)
            {
                string htmldoc = await response.Content.ReadAsStringAsync();
                HtmlDocument document = new HtmlDocument();
                document.LoadHtml(htmldoc);

                var contenedorLista = document.DocumentNode.SelectNodes("//div[@id='search-recipes-list-container']");

                var lista = contenedorLista.Descendants("li").ToList();
                var listaUrl = new List<string?>();

                foreach (var item in lista)
                {
                    var item1 = item.Descendants("a").FirstOrDefault()?.GetAttributeValue("href", string.Empty).ToString();
                    if (item1 != null)
                    {
                        await ScrapReceta(item1);
                    } 
                }
            }
            else
            {
                return false;
            }
            return true;
        }

        public static async Task<string> ObtenerImagenes(string url)
        {
            var client = new HttpClient();
            var request = new HttpRequestMessage(HttpMethod.Get, "https://cookpad.com" + url);
            var response = await client.SendAsync(request);
            if (response.EnsureSuccessStatusCode().IsSuccessStatusCode)
            {

                string htmldoc = await response.Content.ReadAsStringAsync();
                HtmlDocument document = new HtmlDocument();
                document.LoadHtml(htmldoc);

                var body = document.DocumentNode.Descendants("picture").FirstOrDefault();

                if (body != null)
                {
                    var img = body.Descendants("img").FirstOrDefault();
                    string imgUrl = img.GetAttributeValue("src", string.Empty).ToString();

                    return imgUrl;
                } else
                {
                    return "N/A";
                }

            }
            return "N/A";
        }

        public static async Task<bool> ScrapReceta(string url)
        {
            var client = new HttpClient();
            var request = new HttpRequestMessage(HttpMethod.Get, "https://cookpad.com"+url);
            var response = await client.SendAsync(request);
            if (response.EnsureSuccessStatusCode().IsSuccessStatusCode)
            {
                
                string htmldoc = await response.Content.ReadAsStringAsync();
                HtmlDocument document = new HtmlDocument();
                document.LoadHtml(htmldoc);

                var cuerpo = document.DocumentNode.Descendants("div").Where(div => div.Id == "recipe").FirstOrDefault();

                if (cuerpo == null) return false;

                //Titulo
                var titulo = cuerpo.Descendants("h1").First().InnerText.Trim();

                //Tiempo de preparacion
                var tiempo = cuerpo.Descendants("div").FirstOrDefault(div => div.Id.Contains("cooking_time_recipe"));
                string? tiempoText = tiempo != null ? tiempo.InnerText.Trim() : null;


                //Para # de personas
                var cantidad = cuerpo.Descendants("div").FirstOrDefault(div => div.Id.Contains("serving_recipe"));
                string? cantidadText = cantidad != null ? cantidad.InnerText.Trim() : null;


                //Lista de ingredientes
                var listaIngredientes = cuerpo.Descendants("div").Where(div => div.Id == "ingredients").FirstOrDefault();

                var ingredientes = new List<string>();

                if (listaIngredientes != null )
                {
                    foreach (var item in listaIngredientes.Descendants("li").ToList())
                    {
                        ingredientes.Add(item.InnerText.Trim());
                    }
                }

                //Pasos para la receta 
                var listaPasos = cuerpo.Descendants("div").Where(div => div.Id == "steps").FirstOrDefault();

                var pasos = new List<string>();

                var Imagenes = new List<string>();

                if (listaPasos != null)
                {
                    foreach (var item in listaPasos.Descendants("li").ToList())
                    {
                        pasos.Add(item.InnerText.Trim());

                        var imagenes = item.Descendants("a").ToList();

                        foreach (var item1 in imagenes)
                        {
                            var urlImage = item1.GetAttributeValue("href", string.Empty);
                            if (urlImage != null)
                            {
                                string cadena = await ObtenerImagenes(urlImage);
                                if (cadena != "N/A")
                                {
                                    Imagenes.Add(cadena);
                                }
                            }
                        }

                    }
                }

                recetas.Add(new Receta()
                {
                    Id = id,
                    Name = titulo,
                    Description = JsonConvert.SerializeObject(ingredientes),
                    IsVisible = true,
                    Created = fechaActual,
                    TimeToMake = tiempoText,
                    portions = cantidadText,
                    InfoAditional = JsonConvert.SerializeObject(pasos),
                    Imagenes = JsonConvert.SerializeObject(Imagenes),
                });


            }
            else
            {
                return false;
            }
            return true;
        }
    }
}
