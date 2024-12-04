namespace GerenciamentoDeCarrosAPI.Models
{
    public class CarroFavorito
    {
        public int PessoaId { get; set; }

        public Pessoa Pessoa { get; set; }

        public int CarroId { get; set; }

        public Carro Carro { get; set; }
    }
}
