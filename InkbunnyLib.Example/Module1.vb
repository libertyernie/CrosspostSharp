Imports System.IO
Imports Newtonsoft.Json

Module Module1

    Sub Main()
        'PostAndDelete().GetAwaiter().GetResult()
        SearchAndGetDetails().GetAwaiter().GetResult()
    End Sub

    Async Function GetClient() As Task(Of InkbunnyClient)
        Dim ib As InkbunnyClient = Nothing
        Do
            Try
                Console.Write("Enter username: ")
                Dim username = Console.ReadLine()
                Console.Write("Enter password: ")
                Dim password = Console.ReadLine()

                Return Await InkbunnyClient.Create(username, password)
            Catch e As Exception
                Console.Error.WriteLine(e.Message)
            End Try
        Loop
    End Function

    Async Function SearchAndGetDetails() As Task
        Dim ib = Await GetClient()

        Try
            Dim searchResults = Await ib.SearchByUserId(Nothing, 50)

            For Each s In searchResults.submissions
                If (s.scraps) Then
                    Console.WriteLine("  scraps")
                End If
            Next

            Dim details = Await ib.GetSubmissions(searchResults.submissions.Select(Function(s) s.submission_id))
        Catch e As Exception
            Console.Error.WriteLine(e.Message)
            Console.Error.WriteLine(e.StackTrace)
        End Try

        Await ib.Logout()
    End Function

    Async Function PostAndDelete() As Task
        Dim imageData = File.ReadAllBytes("C:\Windows\Web\Wallpaper\Theme1\img3.jpg")

        Dim ib = Await GetClient()

        Console.WriteLine(Await ib.GetUsername)

        Dim submission_id = Await ib.Upload({imageData})

        Dim resp = Await ib.EditSubmission(submission_id,
                    title:="API test",
                    desc:="I'm just testing upload with the Inkbunny API.",
                    type:=SubmissionType.Sketch,
                    scraps:=True,
                    notifyWatchersWhenPublic:=False)

        Console.WriteLine("Submission ID: " & resp.submission_id)

        Dim details = Await ib.GetSubmission(resp.submission_id, show_description:=True)
        Console.WriteLine("Title: " & details.title)
        Console.WriteLine("Description: " & details.description)

        Console.WriteLine("Press enter to edit this submission.")
        Console.ReadLine()

        resp = Await ib.EditSubmission(submission_id,
                    desc:="I'm just testing editing with the Inkbunny API.")

        Console.WriteLine("Submission ID: " & resp.submission_id)
        Console.WriteLine("Press enter to delete this submission.")
        Console.ReadLine()

        Await ib.DeleteSubmission(resp.submission_id)

        Console.WriteLine("Submission deleted.")

        Await ib.Logout()
    End Function

End Module
