namespace PixivUploader

open System.IO

type ViewingRestriction =
| AllAges = 0
| R18 = 1
| R18G = 2

type SexualContent =
| None = 0
| Implicit = 1

type PrivacySettings =
| Public = 0
| MyPixivOnly = 1
| Private = 2

type INewSubmission =
    abstract member Data: Stream
    abstract member Title: string
    abstract member Description: string
    abstract member Tag: seq<string> // space separated
    abstract member ViewingRestriction: ViewingRestriction
    abstract member ImplicitSexualContent: SexualContent // "" or implicit
    abstract member Minors: bool // minors
    abstract member Furry: bool
    abstract member BL: bool // m/m
    abstract member GL: bool // f/f
    abstract member PrivacySettings: PrivacySettings
    abstract member OriginalWork: bool // original work
