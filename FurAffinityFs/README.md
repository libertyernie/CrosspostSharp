# FurAffinityFs

This is an F# (.NET Standard 2.0) library for uploading images to FurAffinity.

This library was developed as part of [CrosspostSharp](https://github.com/libertyernie/CrosspostSharp).

If you're developing a WinForms app and need a login dialog, take a look at [FAWinFormsLogin](https://github.com/libertyernie/FAWinFormsLogin).

Example (C#):

	var client = new FurAffinityClient(a, b);
	string username = await client.WhoamiAsync();
	await client.SubmitPostAsync(new FurAffinitySubmission(
		data: System.IO.File.ReadAllBytes("file.jpg"),
		contentType: "image/jpeg",
		title: "Test",
		message: "Description goes here",
		keywords: new[] { "tag1", "tag2" },
		cat: FurAffinityCategory.Artwork_Digital,
		scrap: true,
		atype: FurAffinityType.General_Furry_Art,
		species: FurAffinitySpecies.Lizard,
		gender: FurAffinityGender.Male,
		rating: FurAffinityRating.General,
		lock_comments: false););
