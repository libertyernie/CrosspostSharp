namespace PixivUploader

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

type IPixivUploadParameters =
    abstract member title: string
    abstract member comment: string
    abstract member tag: seq<string> // space separated
    abstract member x_restrict_sexual: ViewingRestriction
    abstract member sexual: SexualContent // "" or implicit
    abstract member lo: bool // minors
    abstract member furry: bool
    abstract member bl: bool // m/m
    abstract member gl: bool // f/f
    abstract member restrict: PrivacySettings
    abstract member original: bool // original work