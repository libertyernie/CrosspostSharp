using DeviantArtFs.ParameterTypes;
using System.Collections.Generic;
using System.Text;

namespace CrosspostSharp3.DeviantArt {
	public record DeviantArtLicense(string Name, License License) {
		public override string ToString() => Name;

		public static IEnumerable<DeviantArtLicense> ListAll() {
			foreach (var x in License.All) {
				if (x is License.CreativeCommons cc) {
					StringBuilder name = new("CC BY");
					if (cc.Item2.IsCC_NonCommercial)
						name.Append("-NC");
					if (cc.Item3.IsCC_NoDerivatives)
						name.Append("-ND");
					if (cc.Item3.IsCC_ShareAlike)
						name.Append("-SA");
					yield return new DeviantArtLicense(name.ToString(), x);
				} else {
					yield return new DeviantArtLicense(x.ToString(), x);
				}
			}
		}
	}
}
