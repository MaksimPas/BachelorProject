using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace BachelorProject.Models
{
    public class LogRecord
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        //fk
        [ForeignKey("ApplicationUser")]
        public string UserId { get; set; }
        public ApplicationUser ApplicationUser { get; set; }

        [DataType(DataType.DateTime)]        
        public DateTime DateOfRecord { get; set; }
        public LogAction Action { get; set; }
        public string InfoMessage { get; set; }

    }
}

public enum LogAction
{
    FORBRUK
}