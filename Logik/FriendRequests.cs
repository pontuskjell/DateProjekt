namespace Logik
{
    public class FriendRequests
    {
        public int Id { get; set; }
        public string Status { get; set; } = "Waiting";
        public virtual Profiles ToProfile { get; set; }
        public virtual Profiles FromProfile { get; set; }
    }
}
