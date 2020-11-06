using System;
using System.Collections.Generic;

namespace Entity.Models
{
    public partial class PetInfo
    {
        public PetInfo()
        {
            User = new HashSet<User>();
        }

        public string PetId { get; set; }
        public string PetName { get; set; }
        public int? PetAge { get; set; }
        public int? PetSex { get; set; }
        public string OwnerId { get; set; }

        public virtual User Owner { get; set; }
        public virtual ICollection<User> User { get; set; }
    }
}
