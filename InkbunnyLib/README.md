# InkbunnyLib

This is a C# (.NET Framework) library that interfaces with the Inkbunny API.

This library was developed as part of [CrosspostSharp](https://github.com/libertyernie/CrosspostSharp).

Supported endpoints:

* login
* upload
* editsubmission
* submissions
* search (paging is supported)
* logout

Dependencies:
* Newtonsoft.Json

Example (C#):

	var client = await InkbunnyLib.InkbunnyClient.CreateAsync(f.Username, f.Password);
	string sid = client.Sid;
	int userId = client.UserId;

	var client = new InkbunnyClient(sid, userId);
	long submission_id = await _client.UploadAsync(files: new byte[][] {
		File.ReadAllBytes("file.jpg")
	});

	var o = await _client.EditSubmissionAsync(
		submission_id: submission_id,
		title: "Title",
		desc: "The description",
		convert_html_entities: true,
		type: InkbunnySubmissionType.Picture,
		scraps: true,
		isPublic: true,
		notifyWatchersWhenPublic: true,
		keywords: new [] { "tag1", "tag2" },
		tag: new InkbunnyRatingTag[] { }
	);