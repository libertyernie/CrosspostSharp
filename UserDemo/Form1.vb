Imports System.Net
Imports SourceWrappers

Public Class Form1
    Private Pager As IPagedWrapperConsumer = Nothing
    Private ImageLinks As New Dictionary(Of Object, String)

    Private Sub UpdateThumbnail(picBox As PictureBox, picLabel As Label, post As IPostBase)
        picLabel.Text = ""
        picBox.Image = Nothing
        ImageLinks.Remove(picBox)
        If post IsNot Nothing Then
            picLabel.Text = post.Title
            If TypeOf post Is IRemotePhotoPost Then
                picBox.ImageLocation = CType(post, IRemotePhotoPost).ThumbnailURL
            End If
            ImageLinks(picBox) = post.ViewURL
        End If
    End Sub

    Private Sub UpdateThumbnails(posts As IEnumerable(Of IPostBase))
        Dim list = posts.Take(3).ToList()
        Dim post1 = list.FirstOrDefault()
        Dim post2 = list.Skip(1).FirstOrDefault()
        Dim post3 = list.Skip(2).FirstOrDefault()

        UpdateThumbnail(PictureBox1, PicLabel1, post1)
        UpdateThumbnail(PictureBox2, PicLabel2, post2)
        UpdateThumbnail(PictureBox3, PicLabel3, post3)
    End Sub

    Private Async Sub Button1_Click(sender As Object, e As EventArgs) Handles Button1.Click
        If SiteField.SelectedItem = "FurAffinity" Then
            Pager = New AsyncSeqWrapperPagedConsumer(New FurAffinityUserSourceWrapper(ScreennameField.Text, False), 3)
        Else
            Pager = Nothing
        End If

        Dim posts = Await Pager.FirstAsync()
        UpdateThumbnails(posts)
    End Sub

    Private Async Sub BackButton_Click(sender As Object, e As EventArgs) Handles BackButton.Click
        Dim posts = Await Pager.PrevAsync()
        UpdateThumbnails(posts)
    End Sub

    Private Async Sub NextButton_Click(sender As Object, e As EventArgs) Handles NextButton.Click
        Dim posts = Await Pager.NextAsync()
        UpdateThumbnails(posts)
    End Sub

    Private Sub PictureBox1_Click(sender As Object, e As EventArgs) Handles PictureBox1.Click, PictureBox2.Click, PictureBox3.Click
        If ImageLinks.ContainsKey(sender) Then
            Process.Start(ImageLinks(sender))
        End If
    End Sub
End Class
