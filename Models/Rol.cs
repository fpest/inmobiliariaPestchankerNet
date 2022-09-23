using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace inmobiliariaPestchanker.Models
{
    public class Rol
    {
        [Key]
        [Display(Name = "Código")]
        public int Id { get; set; }
		[Required]
        public string Descripcion { get; set; }
		       
    
        }}
