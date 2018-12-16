namespace DeviantArtFs

open System

type IDeviantArtToken =
    abstract member AccessToken: string with get
    abstract member ExpiresAt: Nullable<DateTimeOffset> with get
    abstract member RefreshToken: string with get

type DeviantArtTokenWithRefresh =
    {
        AccessToken: string
        ExpiresAt: DateTimeOffset
        RefreshToken: string
    }
    interface IDeviantArtToken with
        member this.AccessToken = this.AccessToken
        member this.ExpiresAt = this.ExpiresAt |> Nullable
        member this.RefreshToken = this.RefreshToken

type DeviantArtTokenWithoutRefresh =
    {
        AccessToken: string
    }
    interface IDeviantArtToken with
        member this.AccessToken = this.AccessToken
        member this.ExpiresAt = Nullable()
        member this.RefreshToken = null