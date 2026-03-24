using SQLite;

namespace MauiAppMinhasCompras.Models
{
    public class Produto
    {
        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }

        public string Descricao { get; set; } = string.Empty;

        public double Quantidade { get; set; }

        public double Preco { get; set; }

        // ADICIONE ESTAS DUAS LINHAS ABAIXO:

        public string Categoria { get; set; } = string.Empty; // Desafio 1

        public DateTime DataCadastro { get; set; } // Desafio 2
    }
}