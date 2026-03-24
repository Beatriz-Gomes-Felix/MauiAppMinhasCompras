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
        try
        {
            var lista = await App.Db.GetAll();
            produtos.Clear();
            foreach (var p in lista) produtos.Add(p);
        }
        catch (Exception ex)
        {
            await DisplayAlert("Erro", "Falha ao carregar produtos: " + ex.Message, "OK");
        }
    }

    private async void OnSearchTextChanged(object sender, TextChangedEventArgs e)
    {
        try
        {
            string textoDigitado = e.NewTextValue;

            if (string.IsNullOrWhiteSpace(textoDigitado))
            {
                await CarregarProdutos();
            }
            else
            {
                var listaFiltrada = await App.Db.Search(textoDigitado);
                produtos.Clear();
                foreach (var p in listaFiltrada) produtos.Add(p);
            }
        }
        catch (Exception ex)
        {
            await DisplayAlert("Erro", "Erro na busca: " + ex.Message, "OK");
        }
    }

    private async void OnSalvarClicked(object sender, EventArgs e)
    {
        try
        {
            if (string.IsNullOrWhiteSpace(txtDescricao.Text))
            {
                throw new Exception("Por favor, preencha a descrição do produto.");
            }

            if (string.IsNullOrWhiteSpace(txtQuantidade.Text) || string.IsNullOrWhiteSpace(txtPreco.Text))
            {
                throw new Exception("Quantidade e Preço são obrigatórios.");
            }

            var p = new Produto
            {
                Descricao = txtDescricao.Text,
                Quantidade = Convert.ToDouble(txtQuantidade.Text),
                Preco = Convert.ToDouble(txtPreco.Text)
            };

            await App.Db.Insert(p);
            await DisplayAlert("Sucesso!", "Produto adicionado com sucesso!", "OK");

            searchBar.Text = string.Empty;
            await CarregarProdutos();

            txtDescricao.Text = txtQuantidade.Text = txtPreco.Text = string.Empty;
        }
        catch (Exception ex)
        {
            await DisplayAlert("Ops!", ex.Message, "OK");
        }
    }

    private async void OnDeletarClicked(object sender, EventArgs e)
    {
        try
        {
            var item = (sender as SwipeItem)?.CommandParameter as Produto;
            if (item != null && await DisplayAlert("Confirmação", $"Excluir {item.Descricao}?", "Sim", "Não"))
            {
                await App.Db.Delete(item.Id);
                await CarregarProdutos();
            }
        }
        catch (Exception ex)
        {
            await DisplayAlert("Erro", "Erro ao deletar: " + ex.Message, "OK");
        }
    }

    // MÉTODO DE NAVEGAÇÃO - AGENDA 05
    private async void OnItemSelected(object sender, SelectionChangedEventArgs e)
    {
        try
        {
            if (e.CurrentSelection.FirstOrDefault() is Produto produtoSelecionado)
            {
                // Limpa a seleção da lista para permitir clicar novamente
                ((CollectionView)sender).SelectedItem = null;

                // Navega para a página NovoProduto passando o objeto selecionado
                await Navigation.PushAsync(new NovoProduto
                {
                    BindingContext = produtoSelecionado
                });
            }
        }
        catch (Exception ex)
        {
            await DisplayAlert("Erro", "Erro ao abrir edição: " + ex.Message, "OK");
        }
    }
}