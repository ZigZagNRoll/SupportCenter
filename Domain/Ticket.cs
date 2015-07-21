using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SC.BL.Domain.Validation;

namespace SC.BL.Domain
{
    public class Ticket
    {
        
        public int TicketNumber { get; set; }

        [NotAllowedValues(0)]
        public int AccountId { get; set; }

        [Required]
        [MaxLength(100,ErrorMessage="Er zijn maximaal 100 tekens toegestaan")]
        [NotAllowedValues(""," ", "test")]
        public string Text { get; set; }

        public DateTime DateOpened { get; set; }   
     
        [CustomValidation(typeof(EnumValidators), "EnumDefinedOnly")]
        
        public TicketState State { get; set; }
        
        public ICollection<TicketResponse> Responses { get; set; }

        public string GetInfo()
        {
            return String.Format("[{0}] {1} ({2} antwoorden)"
                                        , TicketNumber, Text
                                        , Responses == null ? 0 : Responses.Count);
        }
    }
}
