using System.Collections.Generic;

namespace Logik
{
    public class OrderContacts
    {
        public ICollection<Profiles> Golf { get; set; }
        public ICollection<Profiles> Hockey { get; set; }
        public ICollection<Profiles> Kampsport { get; set; }
        public ICollection<Profiles> Fotboll { get; set; }
    }
}
