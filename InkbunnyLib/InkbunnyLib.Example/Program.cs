using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InkbunnyLib.Example {
    class Program {
        static void Main(string[] args) {
            SearchAndGetDetails().GetAwaiter().GetResult();
        }

        static async Task<InkbunnyClient> GetClient() {
            while (true) {
                try {
                    Console.Write("Enter username: ");
                    string username = Console.ReadLine();
                    Console.Write("Enter password: ");
                    string password = Console.ReadLine();
                    
                    return await InkbunnyClient.CreateAsync(username, password);
                } catch (Exception e) {
                    Console.Error.WriteLine(e.Message);
                }
            }
        }

        static async Task SearchAndGetDetails() {
            var ib = await GetClient();

            try {
                var searchResults = await ib.SearchAsync(submissions_per_page: 10);
                foreach (var s in searchResults.submissions) {
                    if (s.hidden) {
                        Console.WriteLine("(Hidden - skipping)");
                        continue;
                    }
                    Console.WriteLine(s.title);
                }

                Console.WriteLine("----------");

                searchResults = await ib.SearchAsync(searchResults.rid, searchResults.page + 1, 10);
                foreach (var s in searchResults.submissions) {
                    if (s.hidden) {
                        Console.WriteLine("(Hidden - skipping)");
                        continue;
                    }
                    Console.WriteLine(s.title);
                }

                Console.WriteLine("----------");

                var details = await ib.GetSubmissionsAsync(searchResults.submissions.Select(s => s.submission_id), show_description: true);
                foreach (var s in details.submissions) {
                    if (s.hidden) {
                        Console.WriteLine("(Hidden - skipping)");
                        continue;
                    }

                    var ratings = s.ratings.OrderByDescending(r => r.content_tag_id);
                    var id = ratings.FirstOrDefault()?.content_tag_id;
                    if (id != null) {
                        Console.Write($"({id}) ");
                    }

                    string descFirstLine = s.description.Replace("\r", "").Split('\n')[0];
                    if (descFirstLine.Length > 20) {
                        descFirstLine = descFirstLine.Substring(0, 20) + "...";
                    }

                    Console.WriteLine(s.title);
                    if (s.scraps) Console.WriteLine("  (Scrap");
                    if (s.guest_block) Console.WriteLine("  (Not visible to guests)");
                    Console.WriteLine("  " + descFirstLine);
                }

                await ib.LogoutAsync();
            } catch (Exception e) {
                Console.Error.WriteLine(e.Message);
                Console.Error.WriteLine(e.StackTrace);
            }
        }

        static async Task PostAndDelete() {
            byte[] imageData = File.ReadAllBytes(@"C:\Windows\Web\Wallpaper\Theme1\img3.jpg");

            var ib = await GetClient();

            Console.WriteLine(await ib.GetUsernameAsync());

            long submission_id = await ib.UploadAsync(new[] { imageData });

            var resp = await ib.EditSubmissionAsync(submission_id,
                    title: "API test",
                    desc: "I'm just testing upload with the Inkbunny API.",
                    type: InkbunnySubmissionType.Sketch,
                    scraps: true,
                    notifyWatchersWhenPublic: false);

            Console.WriteLine("Submission ID: " + resp.submission_id);

            var details = await ib.GetSubmissionAsync(resp.submission_id, show_description: true);
            Console.WriteLine("Title: " + details.title);
            Console.WriteLine("Description: " + details.description);

            Console.WriteLine("Press enter to edit this submission.");
            Console.ReadLine();

            resp = await ib.EditSubmissionAsync(submission_id,
                        desc: "I'm just testing editing with the Inkbunny API.");

            Console.WriteLine("Submission ID: " + resp.submission_id);
            Console.WriteLine("Press enter to delete this submission.");
            Console.ReadLine();

            await ib.DeleteSubmissionAsync(resp.submission_id);

            Console.WriteLine("Submission deleted.");

            await ib.LogoutAsync();
        }
    }
}
