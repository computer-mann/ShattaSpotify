using System;
using System.Collections.Generic;
using System.Text;

namespace StreamNote.Database.Commons.Database.Entities
{
    public class FcmUserTokens
    {
        public int Id { get; set; }
        public string UserId { get; set; }
        public AudioUser User { get; set; } = default!;
        public string FcmToken { get; set; } = default!;
        public DateTime Timestamp { get; set; } = DateTime.UtcNow;
    }
}
