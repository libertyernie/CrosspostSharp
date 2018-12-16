namespace DeviantArtFs

open System

type DeviantArtToken = {
    AccessToken: string
    ExpiresAt: DateTimeOffset option
    RefreshToken: string option
}
