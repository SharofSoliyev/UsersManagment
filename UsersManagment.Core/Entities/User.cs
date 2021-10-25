using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UsersManagment.Core.Entities.Base;

namespace UsersManagment.Core.Entities
{
 public class User : Entity
    {
     
        public string FirstName  { get; set; }
        public string LastName { get; set; }

        [Required]
        public string UserName { get; set; }

        [Required]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        public DateTime BirthDay { get; set; }

        public bool IsActive { get; set; }

        public string FullName
        {
            get
            {
                return $"{FirstName} {LastName}";
            }
        }

      

    }
}
