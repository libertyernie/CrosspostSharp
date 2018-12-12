# WeasylLib

This is a C# (.NET Standard) library that interfaces with the Weasyl API.

Supported endpoints:

* whoami
* useravatar
* submissions/(submitid)/view
* characters/{charid}/view
* users/(login_name)/gallery

The library can also scrape character submission IDs (charids) for a given user.

Dependencies:
* Newtonsoft.Json

Example (C#):

	var client = new WeasylClient(apiKey);
	WeasylUser user = client.WhoamiAsync();

	IEnumerable<Folder> folders = client.GetFoldersAsync();
	Uri newSubmission = client.UploadVisualAsync(
		data: File.ReadAllBytes("file.jpg"),
		title: "Test",
		subtype: SubmissionType.Sketch,
		folderid: folders.First().FolderId,
		rating: Rating.General,
		content: "Description goes here",
		tags: new[] { "tag1", "tag2" }
	);