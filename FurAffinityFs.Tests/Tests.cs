using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Linq;
using System.Threading.Tasks;
using R = FurAffinityFs.Requests;

namespace FurAffinityFs.Tests {
	[TestClass]
	public class Tests {
		private static readonly IFurAffinityCredentials C = new FurAffinityCredentials();

		[TestMethod]
		public async Task TestGetTimeZone() {
			var o = await R.GetTimeZone.ExecuteAsyncWithDefault(C, System.TimeZoneInfo.Utc);
			if (o == System.TimeZoneInfo.Utc)
				Assert.Inconclusive();

			Assert.AreEqual("Central Standard Time", o.Id);
		}

		[TestMethod]
		public async Task TestUserPage() {
			var u = await R.UserPage.ExecuteAsync(C, "libertyernie");
			Assert.AreEqual("https://a.facdn.net/1424255659/libertyernie.gif", u.avatar.AbsoluteUri);
		}

		[TestMethod]
		public async Task TestWhoami() {
			string s = await R.Whoami.ExecuteAsync(C);
			Assert.AreEqual("lizard-socks", s);
		}

		[TestMethod]
		public async Task TestGallery() {
			var g = await R.Gallery.ExecuteAsync(C, "https://www.furaffinity.net/gallery/libertyernie/");
			Assert.AreEqual(3, g.submissions.Count());
			Assert.AreEqual("https://www.furaffinity.net/view/6796835", g.submissions.First().href.AbsoluteUri);
			Assert.AreEqual("https://t.facdn.net/6796835@200-1320452560.jpg", g.submissions.First().thumbnail.AbsoluteUri);
			Assert.AreEqual("monster", g.submissions.First().title);
		}

		[TestMethod]
		public async Task TestViewSubmission1() {
			var s = await R.ViewSubmission.ExecuteAsync(C, 6214327);
			Assert.AreEqual(System.DateTimeKind.Unspecified, s.date.Kind);
			Assert.IsTrue(s.date > new System.DateTime(2011, 7, 29, 20, 0, 0));
			Assert.IsTrue(s.date < new System.DateTime(2011, 7, 29, 21, 0, 0));
			Assert.IsTrue(s.description.Contains("because transparency"));
			Assert.AreEqual("https://d.facdn.net/art/libertyernie/1311990148/1311990148.libertyernie_vlooper.jpg", s.download.AbsoluteUri);
			Assert.AreEqual("https://d.facdn.net/art/libertyernie/1311990148/1311990148.libertyernie_vlooper.jpg", s.full.AbsoluteUri);
			Assert.IsTrue(s.keywords.Contains("raccoon"));
			Assert.AreEqual(Models.Rating.General, s.rating);
			Assert.AreEqual("https://t.facdn.net/6214327@400-1311990148.jpg", s.thumbnail.AbsoluteUri);
			Assert.AreEqual("Tim & Looper ref", s.title);
			Assert.AreEqual("libertyernie", s.name);
		}

		[TestMethod]
		public async Task TestViewSubmission2() {
			var s = await R.ViewSubmission.ExecuteAsync(C, 31633304);
			Assert.AreEqual(System.DateTimeKind.Unspecified, s.date.Kind);
			Assert.IsTrue(s.date > new System.DateTime(2019, 5, 22, 21, 0, 0));
			Assert.IsTrue(s.date < new System.DateTime(2019, 5, 22, 22, 0, 0));
			Assert.IsTrue(s.description.Contains("populated by bugs"));
			Assert.AreEqual("https://d.facdn.net/art/lizard-socks/1558577083/1558577083.lizard-socks_file.png", s.download.AbsoluteUri);
			Assert.AreEqual("https://d.facdn.net/art/lizard-socks/1558577083/1558577083.lizard-socks_file.png", s.full.AbsoluteUri);
			Assert.IsTrue(s.keywords.Contains("bug"));
			Assert.AreEqual(Models.Rating.General, s.rating);
			Assert.AreEqual("https://t.facdn.net/31633304@400-1558577083.jpg", s.thumbnail.AbsoluteUri);
			Assert.AreEqual("Bug Cap", s.title);
			Assert.AreEqual("lizard-socks", s.name);
		}
	}
}
