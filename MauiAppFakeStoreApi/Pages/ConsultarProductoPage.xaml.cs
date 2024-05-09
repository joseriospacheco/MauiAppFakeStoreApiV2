using MauiAppFakeStoreApi.Models;
using System.Text.Json;

namespace MauiAppFakeStoreApi.Pages;

public partial class ConsultarProductoPage : ContentPage
{
	public ConsultarProductoPage()
	{
		InitializeComponent();
	}

    private async void BtnConsultar_Clicked(object sender, EventArgs e)
    {
		var idProducto = EntryCodigoProducto.Text;

        using var http = new HttpClient();
        var resp = await http.GetAsync($"https://fakestoreapi.com/products/{idProducto}");

        if (resp.IsSuccessStatusCode)
        {

            var resposeBody = await resp.Content.ReadAsStringAsync();
            if (!String.IsNullOrEmpty(resposeBody))
            {

                var product = JsonSerializer.Deserialize<ResponseProduct>(resposeBody);

                if (product is not null)
                {

                    EntryTituloProducto.Text = product.Title;
                    EntryPrecioProducto.Text = product.Price.ToString();
                    EntryCategoriaProducto.Text = product.Category;
                    EntryDescripcionProducto.Text = product.Description;
                    Img.Source = product.Image;

                }

            }
            else
            {
                EntryTituloProducto.Text = String.Empty;
                EntryPrecioProducto.Text = String.Empty;
                EntryCategoriaProducto.Text = String.Empty;
                EntryDescripcionProducto.Text = String.Empty;
                Img.Source = String.Empty;

                await DisplayAlert("Resultado", "No se ha encontrado un producto", "OK");

            }
        }
        else
        {

            await DisplayAlert("Resultado", "No se ha encontrado un producto", "OK");


        }

    }
}