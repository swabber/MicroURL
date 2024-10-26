
namespace MicroURLData {
    public class ShortLink {
        public string ShortId { get; set; }
        public string OriginalUrl { get; set; }
        public User User { get; set; }
        public DateTime ExparationDate { get; set; }
    }
}
