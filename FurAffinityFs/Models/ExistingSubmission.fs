namespace FurAffinityFs.Models

open System

type ExistingSubmission = {
    title: string
    href: Uri
    description: string
    name: string
    download: Uri
    full: Uri
    thumbnail: Uri
    date: DateTime
    keywords: seq<string>
    rating: Rating
}