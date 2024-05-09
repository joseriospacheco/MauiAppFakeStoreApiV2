using MauiAppFakeStoreApi.Models;
using System.Text.Json;

namespace MauiAppFakeStoreApi.Pages;

public partial class AgregarProductoPage : ContentPage
{
    public AgregarProductoPage()
    {
        InitializeComponent();
        ConsultarCategoria();


    }


    public async void ConsultarCategoria()
    {

        using var http = new HttpClient();
        var resp = await http.GetAsync("https://fakestoreapi.com/products/categories");

        if (resp.IsSuccessStatusCode)
        {
            var resposeBody = await resp.Content.ReadAsStringAsync();
            if (!String.IsNullOrEmpty(resposeBody))
            {
                var categorias = JsonSerializer.Deserialize<List<string>>(resposeBody);

                if (categorias is not null) { 
                    PickerCategoria.ItemsSource = categorias;
                }

            }
            else
            {

                await DisplayAlert("Resultado", "No se ha encontrado un producto", "OK");

            }
        }
        else
        {

            await DisplayAlert("Resultado", "No se ha encontrado un producto", "OK");


        }
    }
    private async void BntAgegarProductoClicked(object sender, EventArgs e)
    {

        var product = new CreateProduct();
        product.Title = EntryTitulo.Text;
        product.Price = Convert.ToDecimal(EntryPrecio.Text);
        product.Description = EntryDescripcion.Text;
        product.Category = PickerCategoria.SelectedItem.ToString();



        var json = JsonSerializer.Serialize(product);
        var content = new StringContent(json);

        using var client = new HttpClient();
        var resp = await client.PostAsync("https://fakestoreapi.com/products", content);

        if (resp.IsSuccessStatusCode)
        {

            var contentBody = await resp.Content.ReadAsStringAsync();
            var productResult = JsonSerializer.Deserialize<IdProduct>(contentBody);

            await DisplayAlert("Mensaje", $"El producto creado es {productResult.Id} ", "OK");
        }

    }

    
}