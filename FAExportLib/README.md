# FAExportLib

This is a C# (.NET Standard) library that interfaces with FAExport at https://faexport.boothale.net to allow access to FurAffinity submissions.

If you want to act as a certain user (which lets you post journals and allows limiting what mature/adult submissions you see), create a FAUserClient object with the "a" and "b" cookies from FurAffinity. You can also just create a FAClient object, where all users and submissions will be accessible.

Supported endpoints:

* GET /user/{name}
* GET /user/{name}/{folder}
* GET /user/{name}/journals
* GET /search
* GET /submission/{id}
* POST /journal

Dependencies:
* Newtonsoft.Json

This library was developed as part of [CrosspostSharp](https://github.com/libertyernie/CrosspostSharp).

If you're developing a WinForms app and need a login dialog, take a look at [FAWinFormsLogin](https://github.com/libertyernie/FAWinFormsLogin).

Example (C#):

	FAClient client = new FAClient(a, b);
	IEnumerable<FAFolderSubmission> submissions = await client.GetSubmissionsAsync("your_username", FAFolder.gallery);
	FASubmission with_details = client.GetSubmissionAsync(submissions.First().Id);