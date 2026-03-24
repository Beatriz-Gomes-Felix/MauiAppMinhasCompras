using MauiAppMinhasCompras.Models;

namespace MauiAppMinhasCompras;

public partial class NovoProduto : ContentPage
{
    public NovoProduto()
    {
        InitializeComponent();
    }

    private async void OnAtualizarClicked(object sender, EventArgs e)
    {
        try
        {
            // Pega o produto que veio da outra tela
            var p = BindingContext as Produto;

            p.Descricao = txtDescricao.Text;
            p.Quantidade = Convert.ToDouble(txtQuantidade.Text);
            p.Preco = Convert.ToDouble(txtPreco.Text);

            // Tenta atualizar no banco
            await App.Db.Update(p);

            await DisplayAlert("Sucesso!", "Produto atualizado!", "OK");
            await Navigation.PopAsync(); // Volta para a lista
        }
        catch (Exception ex)
        {
            await DisplayAlert("Erro", "Falha ao atualizar: " + ex.Message, "OK");
        }
    }
}