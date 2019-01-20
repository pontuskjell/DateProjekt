using System;

namespace Logik
{
    public class TopVisited
    {
        public int Id { get; set; }
        public DateTime DateVisited { get; set; }
        public virtual Profiles Visitor { get; set; }
        public virtual Profiles Profile { get; set; }
    }
}
