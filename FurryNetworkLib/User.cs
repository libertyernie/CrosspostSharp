using System;
using System.Linq;

namespace FurryNetworkLib {
    public class User {
        public int Id { get; set; }
        public string Email { get; set; }
        public long Iast_login { get; set; }
        public long Created { get; set; }
        public bool Enabled { get; set; }
        public bool Verified { get; set; }
        public DateTime Birthdate { get; set; }
        public int Rating { get; set; }
        public bool Notify_as_buyer { get; set; }
        public bool Notify_as_seller { get; set; }
        public int[] Character_ids { get; set; }
        public int Current_character_id { get; set; }
        public Character[] characters { get; set; }
        public bool IsUnderage { get; set; }
        public bool EmailChange { get; set; }

        public Character DefaultCharacter => characters.FirstOrDefault(c => c.Default_character) ?? characters.FirstOrDefault();
    }
}
