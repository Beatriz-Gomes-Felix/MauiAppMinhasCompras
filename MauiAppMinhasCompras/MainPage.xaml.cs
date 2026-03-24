using MauiAppMinhasCompras.Models;
using System.Collections.ObjectModel;

namespace MauiAppMinhasCompras;

public partial class MainPage : ContentPage
{
    ObservableCollection<Produto> produtos = new ObservableCollection<Produto>();

    public MainPage()
    {
        InitializeComponent();
        listaProdutos.ItemsSource = produtos;
    }

    protected override async void OnAppearing()
    {
        base.OnAppearing();
        await CarregarProdutos();
    }

    private async Task CarregarProdutos()
    {
        var lista = await App.Db.GetAll();
        produtos.Clear();
        foreach (var p in lista) produtos.Add(p);
    }

    private async void OnSalvarClicked(object sender, EventArgs e)
    {
        try
        {
            if (string.IsNullOrWhiteSpace(txtDescricao.Text)) return;

            var p = new Produto
            {
                Descricao = txtDescricao.Text,
                Quantidade = Convert.ToDouble(txtQuantidade.Text),
                Preco = Convert.ToDouble(txtPreco.Text)
            };

            await App.Db.Insert(p);
            await CarregarProdutos();
            txtDescricao.Text = txtQuantidade.Text = txtPreco.Text = string.Empty;
        }
        catch (Exception ex)
        {
            await DisplayAlert("Erro", ex.Message, "OK");
        }
    }

    private async void OnDeletarClicked(object sender, EventArgs e)
    {
        var item = (sender as SwipeItem)?.CommandParameter as Produto;
        if (item != null && await DisplayAlert("Confirmação", $"Excluir {item.Descricao}?", "Sim", "Não"))
        {
            await App.Db.Delete(item.Id);
            await CarregarProdutos();
        }
    }
}