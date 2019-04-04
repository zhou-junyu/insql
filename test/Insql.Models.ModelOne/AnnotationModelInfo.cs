using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Insql.Models.ModelOne
{
    [Table("annotation_model_info")]
    public class AnnotationModelInfo
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Column("id")]
        public int Id { get; set; }

        [Column("name")]
        public string Name { get; set; }

        public int Size { get; set; }

        [NotMapped]
        public string Extra { get; set; }
    }
}
