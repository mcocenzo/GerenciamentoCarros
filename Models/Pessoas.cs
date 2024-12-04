namespace GerenciamentoDeCarrosAPI.Models
{
    public class Pessoa
    {
        public int Id { get; set; }

        public string Nome { get; set; }

        public string Email { get; set; }

        public ICollection<CarroFavorito> CarrosFavoritos { get; set; }
    }
}
