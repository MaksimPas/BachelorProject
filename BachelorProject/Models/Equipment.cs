using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace BachelorProject.Models
{
    public class Equipment
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Column(TypeName = "VARCHAR")]
        [StringLength(300)]
        [Index("IX_NameAndType", 1, IsUnique = true)]
        public string NameAndType { get; set; }


        ////Can be applied later:
        //[Column(TypeName = "VARCHAR")]
        //[StringLength(300)]
        //[Index("IX_NameAndType", 2, IsUnique = true)]
        //public string Type { get; set; }

        public ICollection<DepotRecord> DepotRecord { get; set; }
    }
}