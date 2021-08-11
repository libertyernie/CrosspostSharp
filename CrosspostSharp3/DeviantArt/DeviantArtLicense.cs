using DeviantArtFs.ParameterTypes;
using System.Collections.Generic;
using System.Text;

namespace CrosspostSharp3.DeviantArt {
	public record DeviantArtLicense {
		public string Name { get; init; }
		public License License { get; init; }

		public override string ToString() => Name;

		public static IEnumerable<DeviantArtLicense> ListAll() {
			yield return new DeviantArtLicense {
				Name = "Default",
				License = License.DefaultLicense
			};
			foreach (var cc in CreativeCommonsLicenseDefinition.All) {
				StringBuilder name = new("CC BY");
				if (cc.commercialUse.IsNonCommercial)
					name.Append("-NC");
				if (cc.derivativeWorks.IsNoDerivatives)
					name.Append("-ND");
				if (cc.derivativeWorks.IsShareAlike)
					name.Append("-SA");
				yield return new DeviantArtLicense {
					Name = name.ToString(),
					License = License.NewCreativeCommonsLicense(cc)
				};
			}
		}
	}
}
