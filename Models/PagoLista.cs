using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace inmobiliariaPestchanker.Models
{
    public class PagoLista
    {
        [Key]
        [Display(Name = "Código")]
        public int Id { get; set; }
		[Required]
		public DateTime FechaPago { get; set; }
		[Required]
		public decimal Importe { get; set; }
		[Required]
		public int IdContrato { get; set; }
		[Required]
        public string InqApellido { get; set; }
		[Required]
        public string InqNombre { get; set; }
		[Required]
        public string Direccion { get; set; }
		
		[Required]
		public string PropApellido { get; set; }
		[Required]
        public string PropNombre { get; set; }
		[Required]
		public decimal PrecioContrato { get; set; }
[Required]
		public DateTime FechaInicioContrato { get; set; }
		[Required]
		public DateTime FechaFinContrato { get; set; }
				
		
	}
}
