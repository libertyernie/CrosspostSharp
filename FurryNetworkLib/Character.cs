using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Text;

namespace FurryNetworkLib {
    public class Character {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Display_name { get; set; }
        public bool Default_character { get; set; }
        public bool Private { get; set; }
        public bool Noindex { get; set; }
        public string Avatar { get; set; }
        public int Avatar_explicit { get; set; }
        public string Banner { get; set; }
        public int Banner_explicit { get; set; }
        public bool Staff { get; set; }
        public object Deleted { get; set; }
        public DateTime? Last_login { get; set; }
        public bool Accepting_commissions { get; set; }
        public object Pricesheet { get; set; }
        public object Pricesheet_nsfw { get; set; }
        public object Pricesheet_instructions { get; set; }
        public object Pricesheet_instructions_nsfw { get; set; }
        public bool Admin_block { get; set; }
        public IEnumerable<object> Commission_media { get; set; }
        public object Ticket_id { get; set; }
        public object Points { get; set; }
        public int Promotes { get; set; }
        public bool Enabled { get; set; }
        public bool IsAuthenticatedUser { get; set; }
        public bool Following { get; set; }
        public bool FollowingCommissions { get; set; }
        public object Banners { get; set; }
        public CharacterStats Stats { get; set; }

		[JsonProperty(PropertyName = "avatars")]
		public JToken _avatars_json { get; set; }

		public Avatars Avatars => _avatars_json is JObject
			? JsonConvert.DeserializeObject<Avatars>(_avatars_json.ToString())
			: null;
	}
}
