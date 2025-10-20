public class CadastrarRequest
{
    public int Id { get; set; } 
    public string Cpf { get; set; } = string.Empty;
    public int Mes { get; set; }
    public int Ano { get; set; }
    public double M3Consumidos { get; set; }
    public string Bandeira { get; set; } = string.Empty;
    public bool possuiEsgoto { get; set; }
}