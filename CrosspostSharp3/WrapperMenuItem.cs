using ArtworkSourceSpecification;

namespace CrosspostSharp3 {
	public class WrapperMenuItem {
		public readonly IArtworkSource BaseWrapper;
		public readonly string DisplayName;

		public WrapperMenuItem(IArtworkSource baseWrapper, string displayName) {
			BaseWrapper = baseWrapper;
			DisplayName = displayName;
		}

		public override string ToString() {
			return DisplayName;
		}
	}
}
