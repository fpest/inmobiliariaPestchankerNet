using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace inmobiliariaPestchanker.Models
{
    public class InmuebleLista
    {

        [Key]
        [Display(Name = "Código")]
        public int Id { get; set; }
        [Required]
        public String Propietario { get; set; }
        [Required]
        [Display(Name ="Dirección")]
        public string Direccion { get; set; }
        [Required]
        [Display(Name ="Tipo de Inmueble")]
        public String TipoInmueble { get; set; }
		[Required]
        public string TipoUso { get; set; }
        [Required]
        [Display(Name ="Cant. Ambientes")]
        public int CantidadAmbientes { get; set; }

		[Required]
         [Display(Name ="Precio")]
       
		public decimal PrecioInmueble { get; set; }
		[Required]
        public Boolean Disponible { get; set; }
               
    
}}
