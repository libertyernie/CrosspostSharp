Imports System.IO

Module Module1

    Sub Main()
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
            Dim searchResults = Await ib.Search(New Mode1SearchParameters(), 10)
            For Each s In searchResults.submissions
                If s.hidden Then
                    Console.WriteLine("(Hidden - skipping)")
                    Continue For
                End If
                Console.WriteLine(s.title)
            Next

            Console.WriteLine("----------")

            searchResults = Await ib.NextPage(searchResults, 10)
            For Each s In searchResults.submissions
                If s.hidden Then
                    Console.WriteLine("(Hidden - skipping)")
                    Continue For
                End If
                Console.WriteLine(s.title)
            Next

            Console.WriteLine("----------")

            Dim details = Await ib.GetSubmissions(searchResults.submissions.Select(Function(s) s.submission_id), show_description:=True)
            For Each s In details.submissions
                If s.hidden Then
                    Console.WriteLine("(Hidden - skipping)")
                    Continue For
                End If

                Dim ratings = s.ratings.OrderByDescending(Function(r) r.content_tag_id)
                Dim id = ratings.FirstOrDefault()?.content_tag_id
                If id IsNot Nothing Then
                    Console.Write($"({id}) ")
                End If

                Dim desc = s.description.Replace(vbCr, "").Split(vbLf)(0)
                If desc.Length > 20 Then
                    desc = desc.Substring(0, 19) & "..."
                End If
                Console.WriteLine(s.title)
                If (s.scraps) Then
                    Console.WriteLine("  (Scrap)")
                End If
                If (s.guest_block) Then
                    Console.WriteLine("  (Not visible to guests)")
                End If
                Console.WriteLine("  " & desc)
            Next
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
