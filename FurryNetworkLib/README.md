# FurryNetworkLib

This is a C# (.NET Standard 2.0) library for uploading to and downloading from Furry Network.

This library was developed as part of [CrosspostSharp](https://github.com/libertyernie/CrosspostSharp).

Example (C#):

	FurryNetworkClient client = await FurryNetworkClient.LoginAsync(email, password);
	Character character = await client.ChangeCharacterAsync(charName);

	var artwork = await client.UploadArtwork(
		charName,
		File.ReadAllBytes("file.jpg"),
		"image/jpeg",
		"file.jpg");
	await _client.UpdateArtwork(artwork.Id, new FurryNetworkClient.UpdateArtworkParameters {
		Community_tags_allowed = true,
		Description = "description goes here",
		Publish = true,
		Rating = 2,
		Status = "draft",
		Tags = new[] { "tag1", "tag2" },
		Title = "Test"
	});