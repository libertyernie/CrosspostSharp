Imports System.IO

Module Module1

    Sub Main()
        AsyncMain().GetAwaiter().GetResult()
    End Sub

    Async Function AsyncMain() As Task
        Dim imageData = File.ReadAllBytes("C:\Windows\Web\Wallpaper\Theme1\img3.jpg")

        Console.Write("Enter username: ")
        Dim username = Console.ReadLine()
        Console.Write("Enter password: ")
        Dim password = Console.ReadLine()

        Dim ib = Await InkbunnyClient.Create(username, password)

        Console.WriteLine(Await ib.GetUsername)

        Dim submission_id = Await ib.Upload({imageData})

        Dim resp = Await ib.EditSubmission(submission_id,
                    title:="API test",
                    desc:="I'm just testing upload with the Inkbunny API.",
                    type:=SubmissionType.Sketch,
                    scraps:=True,
                    notifyWatchersWhenPublic:=False)

        Console.WriteLine("Submission ID: " & resp.submission_id)

        Dim details = Await ib.GetSubmissions({resp.submission_id}, show_description:=True)
        Console.WriteLine("Title: " & details.submissions(0).title)
        Console.WriteLine("Description: " & details.submissions(0).description)

        Console.WriteLine("Press enter to edit this submission.")
        Console.ReadLine()

        resp = Await ib.EditSubmission(submission_id,
                    desc:="I'm just testing editing with the Inkbunny API.")

        Console.WriteLine("Submission ID: " & resp.submission_id)
        Console.WriteLine("Press enter to delete this submission.")
        Console.ReadLine()

        Await ib.DeleteSubmission(resp.submission_id)

        Console.WriteLine("Submission deleted.")
    End Function

End Module
