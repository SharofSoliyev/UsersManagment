using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using UsersManagment.Businees.Helpers;

namespace UsersManagment.Businees.Models
{
  public  class LoginTokenModel
    {
        public string Token { get; set; }

        [DataType(DataType.Date)]
        [JsonConverter(typeof(YearMonthDateHourMinuteSecond))]
        public DateTime Expiration { get; set; }

        [JsonIgnore]
        public bool IsActive { get; set; }
    }
}
