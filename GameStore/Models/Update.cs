using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace GameStore.Models
{
    public class Update
    {
        [Key]
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public int GameId { get; set; }
        public virtual Game Game { get; set; }
        [Display(Name = "DateAdded")]
        [Required]
        public DateTime DateAdded { get; set; }
    }
}
