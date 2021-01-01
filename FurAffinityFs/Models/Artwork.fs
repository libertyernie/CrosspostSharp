namespace FurAffinityFs.Models

type Artwork = {
    data: byte[]
    contentType: string
    title: string
    message: string
    keywords: string seq
    cat: Category
    scrap: bool
    atype: Type
    species: Species
    gender: Gender
    rating: Rating
    lock_comments: bool
}