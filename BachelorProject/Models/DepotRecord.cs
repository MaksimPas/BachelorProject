using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;
using BachelorProject.CustomAttributes;
using BachelorProject.Properties;

namespace BachelorProject.Models
{
    public class DepotRecord
    {

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [ForeignKey("Equipment")]
        public int EquipmentCodeId { get; set; }
        public Equipment Equipment { get; set; }

        //[DataType(DataType.Date, ErrorMessage = "Feil dato")
        
        [DataType(DataType.Date, ErrorMessageResourceName = "DateErrror", ErrorMessageResourceType = typeof(ValidationResources))]
        //DataFormatString must be ISO standard {0:yyyy-MM-dd}, see https://stackoverflow.com/questions/31097748/date-does-not-display-from-model-on-html-input-type-date
        //otherwise value="{date}" will not work in VIEW
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ConvertEmptyStringToNull =true,NullDisplayText = "Ingen utløpsdato", ApplyFormatInEditMode = true)]
        public DateTime? ExpirationDate { get; set; } //using nullable type because the field can be null

        [DataType(DataType.DateTime)]
        public DateTime DateOfRecord { get; set; }
        
        [Required(ErrorMessage = "Feltet Antall er obligatorisk!")]
        [NotNegativeNumber(ErrorMessage = "Verdien må være større enn 0")]
        public int QuantityOriginal { get; set; }
        [Required]
        public int QuantityLeft { get; set; }

        public string Information { get; set; } //can be used to write comments

        //not necessary yet:
        //FK
        //public int DistributorId { get; set; }

        //public Distributor Distributor { get; set; }

        //not necessary yet:
        //FK
        //public int IsAtLocationId { get; set; }

        //public Location Location { get; set; }
    }
}