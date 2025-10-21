namespace GuilhermeGabriel.Requests
{
    public class CadastrarRequest
    {
        public string Cpf { get; set; } = string.Empty;
        public int Mes { get; set; }
        public int Ano { get; set; }
        public double M3Consumidos { get; set; }
        public string Bandeira { get; set; } = string.Empty;
        public bool possuiEsgoto { get; set; }
    }
}