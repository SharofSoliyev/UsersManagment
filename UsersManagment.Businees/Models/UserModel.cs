using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace UsersManagment.Businees.Models
{
  public  class UserModel
    {
        
        public int Id { get; set; }
        
        public string FirstName { get; set; }
        public string LastName { get; set; }

        [Required]
        public string UserName { get; set; }
        [Required]
        public string Password { get; set; }

        [JsonIgnore]
        public bool IsActive { get; set; }
    }
}
