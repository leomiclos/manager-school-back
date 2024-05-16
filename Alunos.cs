using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

public class Aluno
{
    private DateTime _dNascimento;


    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]        
    [Key]
    public int iCodAluno { get; set; }
    public string? sNome { get; set; }

    public DateTime dNascimento
    {
        get { return _dNascimento; }
        set { _dNascimento = value; }
    }

    public string? sCPF { get; set; }
    public string? sEndereco { get; set; }
    public string? sCelular { get; set; }
    public int iCodEscola { get; set; }

    // Método para obter a data de nascimento formatada
    public string GetFormattedNascimento()
    {
        return _dNascimento.ToString("dd/MM/yyyy");
    }
}
