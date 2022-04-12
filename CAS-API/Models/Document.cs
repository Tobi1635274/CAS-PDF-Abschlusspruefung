using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CAS_API.Models
{
#pragma warning disable 1591
    [Table("Documents", Schema = "dbo")]
    public class Document
    {
        [Key]
        [Required]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; } = 0;
        public string Dateiname { get; set; } = string.Empty;
        public string Pfad { get; set; } = string.Empty;
        public DateTime LetzteAenderung{ get; set; } = DateTime.Now;
    }
#pragma warning restore 1591
}
