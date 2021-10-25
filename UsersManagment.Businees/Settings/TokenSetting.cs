using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UsersManagment.Businees.Settings
{
   public class TokenSetting
    {
        public string Secret { get; set; }
        public int ExpiresInDays { get; set; }
    }
}
