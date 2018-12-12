namespace FurAffinityFs

type FurAffinitySubmission = {
    data: byte[]
    contentType: string
    title: string
    message: string
    keywords: seq<string>
    cat: FurAffinityCategory
    scrap: bool
    atype: FurAffinityType
    species: FurAffinitySpecies
    gender: FurAffinityGender
    rating: FurAffinityRating
    lock_comments: bool
}