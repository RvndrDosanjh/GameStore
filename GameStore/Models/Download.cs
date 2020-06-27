using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace GameStore.Models
{
    public class Download
    {
        [Key]
        public int Id { get; set; }
        public int GameId { get; set; }
        public virtual Game Game { get; set; }
        public string Email { get; set; }
        public string Comment { get; set; }
        public DateTime Date { get; set; }
       
    }
}
