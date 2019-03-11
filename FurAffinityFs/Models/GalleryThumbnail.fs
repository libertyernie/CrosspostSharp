namespace FurAffinityFs.Models

open System

type GalleryThumbnail = {
    sid: int
    title: string
    href: Uri
    thumbnail: Uri
}