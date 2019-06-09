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

type NewSubmission = {
    data: byte[]
    title: string
    comment: string
    tag: seq<string> // space separated
    x_restrict_sexual: ViewingRestriction
    sexual: SexualContent // "" or implicit
    lo: bool // minors
    furry: bool
    bl: bool // m/m
    gl: bool // f/f
    restrict: PrivacySettings
    original: bool // original work
}