using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FirstWebMVC.Models
{
    [Table("Persons")]
    public class Person{
        [Key]
        public string Id {get; set;}
        public string Name {get; set;}
        public string Gender {get; set;}
        public string email {get; set;}
    }
}