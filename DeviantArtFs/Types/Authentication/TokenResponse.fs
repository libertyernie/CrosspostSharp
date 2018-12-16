namespace DeviantArtFs.Types.Authentication

type TokenResponsed = {
    expires_in: int
    status: string
    access_token: string
    token_type: string
    refresh_token: string
    scope: string
}