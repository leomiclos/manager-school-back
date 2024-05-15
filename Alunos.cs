public class Aluno
{



    public int Id { get; set; }
    public int iCodAluno { get; set; }
    public string? sNome { get; set; }
    public DateTime dNascimento { get; set; }
    public string? sCPF { get; set; }
    public string? sEndereco { get; set; }
    public string? sCelular { get; set; }
    public int iCodEscola { get; set; }

    // Relacionamento com a Escola
    // public Escola? Escola { get; set; }
}