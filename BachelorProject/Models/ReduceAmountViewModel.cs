using BachelorProject.CustomAttributes;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using System.Web;

namespace BachelorProject.Models
{
    public class ReduceAmountViewModel
    {
        public static ReduceAmountViewModel Create(int equipmentId, int reduceQuantity)
        {
            return new ReduceAmountViewModel
            {
                EquipmentId = equipmentId,
                ReduceQuantity = reduceQuantity
            };
        }
        public int EquipmentId { get; set; }

        [Required(ErrorMessage = "Skriv inn antall")]
        [NotNegativeNumber(ErrorMessage = "Antallet må være større enn 0")]
        public int ReduceQuantity { get; set; }
    }
}