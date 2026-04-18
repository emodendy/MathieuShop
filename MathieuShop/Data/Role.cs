using System;
using System.Collections.Generic;

namespace MathieuShop.Data
{
    public partial class Role
    {
        public int RoleId { get; set; }
        public string Name { get; set; } = null!;
        public DateTime? UpdatedAt { get; set; }

        public virtual ICollection<User> Users { get; set; } = new List<User>();
    }
}
