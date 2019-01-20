using System.Collections.Generic;

namespace Logik
{
    public class Interests
    {
        public Interests()
        {
            Profiles = new HashSet<Profiles>();
        }
        public int Id { get; set; }
        public string Name { get; set; }
        public bool Checked { get; set; } = false;
        public virtual ICollection<Profiles> Profiles { get; set; }

    }
}
