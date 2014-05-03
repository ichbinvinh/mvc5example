using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace MvcMesswert.Models
{
    public class Group<T, K>
    {
        public K Key;
        public IEnumerable<T> Values;
    }

    public class MyViewModel
    {
        public IEnumerable<Group<Messwert, DateTime>> TypeDatetime { get; set; }
        public IEnumerable<Group<Messwert, decimal>> TypeDecimal { get; set; }
        public IEnumerable<Group<Messwert, string>> TypeString { get; set; }
        public IEnumerable<Messwert> TypeMesswert { get; set; }
        
    }

    public class Messwert
    {
        public int ID { get; set; }
        [Display(Name = "Time")]
        [DataType(DataType.DateTime)]
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy H:mm:ss}", ApplyFormatInEditMode = true, HtmlEncode= true)]
        public DateTime Time { get; set; }
        public string Template { get; set; }
        public decimal Value { get; set; }

    }

    class MesswertDBContext : DbContext
    {
        public DbSet<Messwert> Messwerte { get; set; }
    }
   
}