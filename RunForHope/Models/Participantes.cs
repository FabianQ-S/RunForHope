using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Collections.Generic;
using System.ComponentModel;

namespace RunForHope.Models
{
    public class Participantes
    {
        [Key]
        public int Id { get; set; }

        // Información personal básica
        [Required(ErrorMessage = "El nombre es obligatorio.")]
        [StringLength(50, ErrorMessage = "El nombre no puede tener más de 50 caracteres.")]
        [DisplayName("Nombre")]
        public string Nombre { get; set; }

        [Required(ErrorMessage = "El apellido es obligatorio.")]
        [StringLength(50, ErrorMessage = "El apellido no puede tener más de 50 caracteres.")]
        [DisplayName("Apellido")]
        public string Apellido { get; set; }

        [Required(ErrorMessage = "El DNI es obligatorio.")]
        [RegularExpression(@"^\d{8}$", ErrorMessage = "El DNI debe tener exactamente 8 dígitos.")]
        [DisplayName("DNI")]
        public string DNI { get; set; }

        [Required(ErrorMessage = "La fecha de nacimiento es obligatoria.")]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        [DisplayName("Fecha de Nacimiento")]
        [CustomValidation(typeof(Participantes), nameof(ValidarFechaNacimiento))]
        public DateTime FechaNacimiento { get; set; }

        [Required(ErrorMessage = "El género es obligatorio.")]
        [DisplayName("Género")]
        public string Genero { get; set; }

        [DisplayName("¿Tiene alguna discapacidad?")]
        public bool Discapacidad { get; set; }

        // Información de contacto
        [Required(ErrorMessage = "El correo electrónico es obligatorio.")]
        [EmailAddress(ErrorMessage = "El correo electrónico no tiene un formato válido.")]
        [DisplayName("Correo Electrónico")]
        public string Correo { get; set; }

        [Required(ErrorMessage = "El número de teléfono es obligatorio.")]
        [RegularExpression(@"^\d{9}$", ErrorMessage = "El número de teléfono debe tener 9 dígitos.")]
        [DisplayName("Número de Teléfono")]
        [DataType(DataType.PhoneNumber)]
        public string NumeroTelefono { get; set; }

        [Required(ErrorMessage = "El distrito es obligatorio.")]
        [DisplayName("Distrito")]
        public string Distrito { get; set; }

        // Información médica
        [Required(ErrorMessage = "El tipo de sangre es obligatorio.")]
        [StringLength(3, ErrorMessage = "El tipo de sangre no puede tener más de 3 caracteres.")]
        [RegularExpression(@"^(A|B|AB|O)[+-]$", ErrorMessage = "El tipo de sangre debe ser un valor válido (A+, A-, B+, B-, AB+, AB-, O+, O-).")]
        [DisplayName("Tipo de Sangre")]
        public string TipoSangre { get; set; }

        // Información de registro
        [DataType(DataType.DateTime)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd HH:mm:ss}")]
        [DisplayName("Fecha de Registro")]
        [ScaffoldColumn(false)] // Oculta este campo en los formularios
        public DateTime FechaRegistro { get; set; } = DateTime.Now;

        [NotMapped]
        public List<SelectListItem> GeneroOptions { get; } = new List<SelectListItem>
        {
            new SelectListItem { Value = "Masculino", Text = "Masculino" },
            new SelectListItem { Value = "Femenino", Text = "Femenino" },
            new SelectListItem { Value = "Otro", Text = "Otro" }
        };

        // Método de validación personalizado para la edad
        public static ValidationResult ValidarFechaNacimiento(DateTime fechaNacimiento, ValidationContext context)
        {
            int edad = DateTime.Today.Year - fechaNacimiento.Year;
            if (fechaNacimiento > DateTime.Today.AddYears(-edad)) edad--;

            if (edad < 12)
            {
                return new ValidationResult("Debes tener al menos 12 años para participar en la carrera.");
            }

            if (fechaNacimiento > DateTime.Today)
            {
                return new ValidationResult("La fecha de nacimiento no puede ser en el futuro.");
            }

            return ValidationResult.Success;
        }
    }
}