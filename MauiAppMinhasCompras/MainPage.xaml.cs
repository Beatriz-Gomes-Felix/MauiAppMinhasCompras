using MauiAppMinhasCompras.Models;
using System.Collections.ObjectModel;
using System.Globalization;

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

    private async void OnAtualizarLista(object sender, EventArgs e)
    {
        await CarregarProdutos();
        atualizarLista.IsRefreshing = false;
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
                throw new Exception("Preencha a descrição.");

            if (pckCategoria.SelectedItem == null)
                throw new Exception("Selecione uma categoria.");

            // CORREÇÃO DA VÍRGULA/PONTO:
            // Tenta converter usando a cultura atual do sistema
            if (!double.TryParse(txtQuantidade.Text, CultureInfo.CurrentCulture, out double qtd))
                qtd = 0;

            if (!double.TryParse(txtPreco.Text, CultureInfo.CurrentCulture, out double preco))
                throw new Exception("Preço inválido.");

            var p = new Produto
            {
                Descricao = txtDescricao.Text,
                Quantidade = qtd,
                Preco = preco,
                Categoria = pckCategoria.SelectedItem.ToString(),
                DataCadastro = dtpData.Date
            };

            await App.Db.Insert(p);
            await DisplayAlert("Sucesso!", "Produto adicionado!", "OK");

            searchBar.Text = string.Empty;
            await CarregarProdutos();

            txtDescricao.Text = txtQuantidade.Text = txtPreco.Text = string.Empty;
            pckCategoria.SelectedIndex = -1;
            dtpData.Date = DateTime.Now;
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

    private async void OnItemSelected(object sender, SelectionChangedEventArgs e)
    {
        try
        {
            if (e.CurrentSelection.FirstOrDefault() is Produto produtoSelecionado)
            {
                ((CollectionView)sender).SelectedItem = null;

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