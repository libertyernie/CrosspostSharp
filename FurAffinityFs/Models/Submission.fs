namespace FurAffinityFs.Models

type Submission = {
    data: byte[]
    contentType: string
    title: string
    message: string
    keywords: seq<string>
    cat: Category
    scrap: bool
    atype: Type
    species: Species
    gender: Gender
    rating: Rating
    lock_comments: bool
}