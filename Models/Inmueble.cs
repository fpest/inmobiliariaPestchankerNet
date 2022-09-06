using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace inmobiliariaPestchanker.Models
{
    public class Inmueble
    {
        [Key]
        [Display(Name = "Código")]
        public int Id { get; set; }
		[Required]
        public int IdPropietario { get; set; }
        [Required]
        public string Direccion { get; set; }
        [Required]
        public decimal CoordenadaN { get; set; }
		[Required]
		public decimal CoordenadaE { get; set; }
        [Required]
        public int IdTipoInmueble { get; set; }
		[Required]
        public string TipoUso { get; set; }
        [Required]
        public int CantidadAmbientes { get; set; }
		[Required]
		public decimal PrecioInmueble { get; set; }
		[Required]
        public Boolean Disponible { get; set; }
        [Required]
	    public Propietario Duenio { get; set; }
     //   [Required]
	 //   public String TipoInmueble { get; set; }
        
		       
    
}}
