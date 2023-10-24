using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace LoremIpsum_back.Entities
{
    public class Endereco
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [MaxLength(20), Required(AllowEmptyStrings = false),
        RegularExpression("^(Comercial|Residencial)$", ErrorMessage = "Tipo de endereço deve ser 'Comercial' ou 'Residencial'.")]
        public string Tipo { get; set; }
        
        [MaxLength(8), Required(AllowEmptyStrings = false)]
        public string Cep { get; set; }

        [MaxLength(50), Required(AllowEmptyStrings = false)]
        public string Logradouro { get; set; }

        [MaxLength(6), Required(AllowEmptyStrings = false)]
        public string Numero { get; set; }

        [MaxLength(50)]
        public string? Complemento { get; set; }

        [MaxLength(50)]
        public string? Bairro { get; set; }

        [MaxLength(50), Required(AllowEmptyStrings = false)]
        public string Cidade { get; set; }

        [MaxLength(2), Required(AllowEmptyStrings = false)]
        public string UF { get; set; }
        
        [Required]
        public int IdCliente { get; set; }

        [ForeignKey("IdCliente")]
        public virtual Cliente Cliente { get; set; }
    }
}
