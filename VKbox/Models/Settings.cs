using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using Catel.Data;
using Catel.Runtime.Serialization;

namespace VKbox.Models
{
    public class Settings : SavableModelBase<Settings>
    {
        [IncludeInSerialization]
        public string Login { get; set; }
        [IncludeInSerialization]
        public string Pass { get; set; }   
        public Settings()
        {
            
        }
    }
}
