namespace FurAffinityFs.Models

type GalleryPage = {
    submissions: seq<GalleryThumbnail>
    next_page_href: string option
}