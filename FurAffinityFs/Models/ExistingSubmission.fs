namespace FurAffinityFs.Models

open System

type ExistingSubmission = {
    title: string
    href: Uri
    description: string
    name: string
    download: Uri
    full: Uri option
    thumbnail: Uri option
    date: string
    keywords: seq<string>
    rating: Rating
}