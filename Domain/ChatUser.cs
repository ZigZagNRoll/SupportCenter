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
    public class ChatUser
    {
        [Required]
        [NotAllowedValues(0)]
        public string ConnectionId { get; set; }
        [NotAllowedValues("", " ", "test")]
        public string Name { get; set; }
        
    }
}
