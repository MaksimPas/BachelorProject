using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace BachelorProject.Models
{
    public class DepotRecord
    {

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [ForeignKey("Equipment")]
        [Index("IX_EquiplemtIdANDExpDate", 1, IsUnique = true)]
        public int EquipmentCodeId { get; set; }
        public Equipment Equipment { get; set; }


        [Index("IX_EquiplemtIdANDExpDate", 2 , IsUnique = true)]
        [Display(Name = "Insert Expiration Date")]
        [Required(ErrorMessage = "You must specify the date of the event!")]
        [DataType(DataType.Date, ErrorMessage = "Wrong date")]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public DateTime ExpirationDate { get; set; }


        //not necessary yet:
        //FK
        //public int DeliveryId { get; set; }

        //public Delivery Delivery { get; set; }

        //not necessary yet:
        //FK
        //public int IsAtLocationId { get; set; }

        //public Location Location { get; set; }

        public int QuantityOriginal { get; set; }

        public int QuantityLeft { get; set; }
    }
}