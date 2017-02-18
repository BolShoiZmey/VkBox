using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Catel.Data;

namespace VKbox.ViewModels
{
    public class User : ModelBase
    {
        public string Id;
        public string LastName;
        public string Name;

        public User(string id, string name, string lastName)
        {
            Id = id;
            Name = name;
            LastName = lastName;
        }

        public override string ToString()
        {
            return $"{Name} {LastName}";
        }
    }
}
