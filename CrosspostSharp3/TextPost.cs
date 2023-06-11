using Microsoft.FSharp.Collections;

namespace CrosspostSharp3 {
	public record struct TextPost(string Title, string HTMLDescription, bool Mature, bool Adult, FSharpList<string> Tags);
}
