using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WeasylSyncLib;

namespace WeasylSync {
	public static class TwitterIdTag {
		private static IList<char> _characters;
		static TwitterIdTag() {
			List<char> characters = new List<char>();
			for (int i = 0x2740; i <= 0x27BF; i++) {
				characters.Add((char)i);
			}
			_characters = characters;
		}

		public static string ToBase128(long l) {
			StringBuilder sb = new StringBuilder();
			while (l != 0) {
				int remainder = (int)(l % _characters.Count);
				sb.Insert(0, _characters[remainder]);
				l /= _characters.Count;
			}
			return sb.ToString();
		}

		public static long FromBase128(IEnumerable<char> str) {
			long result = 0;
			foreach (char c in str) {
				int index = _characters.IndexOf(c);
				if (index == -1) throw new FormatException();
				result = _characters.Count * result + index;
			}
			return result;
		}

		public static string Get(SubmissionBaseDetail submission) {
			if (submission is SubmissionDetail) {
				return $"🎨{ToBase128((submission as SubmissionDetail).submitid)}";
			} else if (submission is CharacterDetail) {
				return $"🆔{ToBase128((submission as CharacterDetail).charid)}";
			} else {
				return null;
			}
		}
	}
}
