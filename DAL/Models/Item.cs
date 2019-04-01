using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace DAL.JecaestevezApp.Models
{
    public class Item
    {
        [Key]
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public DateTime Expiration { get; set; }
        public bool IsEnable { get; set; }
    }
}
