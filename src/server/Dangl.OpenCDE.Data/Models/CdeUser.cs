using System;

namespace Dangl.OpenCDE.Data.Models
{
    /// <summary>
    /// A locally cached reference to a user authenticated via an external identity
    /// provider (Supabase Auth). The <see cref="Id"/> is the provider's "sub" claim,
    /// kept here only so other tables can carry a foreign key to it -- there is no
    /// local password or role management.
    /// </summary>
    public class CdeUser
    {
        public Guid Id { get; set; }

        public string Email { get; set; }
    }
}
