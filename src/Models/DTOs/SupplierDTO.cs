using System.ComponentModel.DataAnnotations;

namespace backend.Models
{
    public class SupplierDto
    {
        [Required(ErrorMessage = "La razón social es obligatorio.")]
        [StringLength(100, ErrorMessage = "La razón social no puede exceder los 100 caracteres.")]
        public required string BusinessName { get; set; }

        [Required(ErrorMessage = "El nombre comercial es obligatorio.")]
        [StringLength(100, ErrorMessage = "El nombre comercial no puede exceder los 100 caracteres.")]
        public required string TradeName { get; set; }

        [Required(ErrorMessage = "La identificación Tributaria es obligatorio.")]
        [StringLength(50, ErrorMessage = "La identificación Tributaria no puede exceder los 50 caracteres.")]
        public required string TaxId { get; set; }

        [Required(ErrorMessage = "El número telefónico es obligatorio.")]
        [Phone(ErrorMessage = "El número telefónico no es válido.")]
        public required string Phone { get; set; }

        [Required(ErrorMessage = "El correo electrónico es obligatorio.")]
        [EmailAddress(ErrorMessage = "El correo electrónico no es válido.")]
        public required string Email { get; set; }

        [Required(ErrorMessage = "El sitio web es obligatorio.")]
        [Url(ErrorMessage = "El sitio web no es válido.")]
        public required string Website { get; set; }

        [Required(ErrorMessage = "La dirección física es obligatoria.")]
        public required string Address { get; set; }

        [Required(ErrorMessage = "El país es obligatorio.")]
        public required string Country { get; set; }

        [Required(ErrorMessage = "La facturación anual es obligatoria.")]
        public required decimal AnnualBilling { get; set; }
    }
}