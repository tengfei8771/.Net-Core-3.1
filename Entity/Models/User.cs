using System;
using System.Collections.Generic;

namespace Entity.Models
{
    public partial class User
    {
        public User()
        {
            PetInfo = new HashSet<PetInfo>();
        }

        public string Id { get; set; }
        public string PassWord { get; set; }
        public string Account { get; set; }
        public string PetId { get; set; }

        public virtual PetInfo Pet { get; set; }
        public virtual ICollection<PetInfo> PetInfo { get; set; }
    }
}
