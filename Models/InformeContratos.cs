using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace inmobiliariaPestchanker.Models
{
    public class InformeContratos
    {
        
		[Required]
        public int IdContrato { get; set; }
        [Required]
        public int IdInquilino { get; set; }
        [Required]
        public string Direccion { get; set; }
        
        public DateTime FechaInicio { get; set; }
        
        public DateTime FechaFin { get; set; }
        [Required]
        public decimal Precio { get; set; }
		[Required]
        public int IdPropietario { get; set; }
		[Required]
        public String NombreInquilino { get; set; }
		[Required]
        public String ApellidoInquilino { get; set; }
		[Required]
        public String NombrePropietario { get; set; }
		[Required]
        public String ApellidoPropietario { get; set; }
		
		       
    
}}
