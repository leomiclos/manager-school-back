using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

public class Escola
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int iCodEscola { get; set; }

    public string? sDescricao { get; set; }
}
