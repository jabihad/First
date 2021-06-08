using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace Entities.Models
{
    public class LoginActivity
    {
        [Key]
        public int LoginActivityId { get; set; }
        //[ForeignKey("UserId")]
        public string UserId { get; set; }
        public virtual User Users { get; set; }
        public DateTime LoginTime { get; set; }
        public DateTime? LogoutTime { get; set; }

    }
}
