using MauiAppMinhasCompras.Models;

namespace MauiAppMinhasCompras;

public partial class NovoProduto : ContentPage
{
    public NovoProduto()
    {
        InitializeComponent();
    }

    // Este método roda quando a tela abre, para preencher o Picker e o DatePicker
    protected override void OnAppearing()
    {
        base.OnAppearing();

        if (BindingContext is Produto p)
        {
            pckCategoria.SelectedItem = p.Categoria;
            dtpData.Date = p.DataCadastro;
        }
    }

    private async void OnAtualizarClicked(object sender, EventArgs e)
    {
        try
        {
            var p = BindingContext as Produto;

            if (p == null) throw new Exception("Produto não encontrado.");

            // Validações básicas
            if (string.IsNullOrWhiteSpace(txtDescricao.Text))
                throw new Exception("Descrição obrigatória.");

            if (pckCategoria.SelectedItem == null)
                throw new Exception("Selecione uma categoria.");

            // Atualiza o objeto com os novos valores da tela
            p.Descricao = txtDescricao.Text;
            p.Quantidade = Convert.ToDouble(txtQuantidade.Text);
            p.Preco = Convert.ToDouble(txtPreco.Text);
            p.Categoria = pckCategoria.SelectedItem.ToString(); // Desafio 1
            p.DataCadastro = dtpData.Date; // Desafio 2

            // Salva no Banco SQLite
            await App.Db.Update(p);

            await DisplayAlert("Sucesso!", "Produto atualizado com sucesso!", "OK");
            await Navigation.PopAsync();
        }
        catch (Exception ex)
        {
            await DisplayAlert("Erro", "Falha ao atualizar: " + ex.Message, "OK");
        }
    }

    private async void OnCancelarClicked(object sender, EventArgs e)
    {
        await Navigation.PopAsync();
    }
}