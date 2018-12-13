namespace PillowfortFs

open System

type PillowfortMedia = {
    url: string
    id: int
    post_id: int
    created_at: DateTimeOffset
    updated_at: DateTimeOffset
    media_type: string
    row: string
    col: string
}

type PillowfortPost = {
    user_id: int
    original_post_id: Nullable<int>
    title: string
    content: string
    privacy: string
    nsfw: bool
    id: int
    post_type: string
    created_at: DateTimeOffset
    updated_at: DateTimeOffset
    community_id: Nullable<int>
    rebloggable: bool
    deleted_at: Nullable<DateTimeOffset>
    commentable: bool
    last_activity: DateTimeOffset
    comments_count: int
    likes_count: int
    reblogs_count: int
    tags: seq<string>
    username: string
    avatar_url: string
    media: seq<PillowfortMedia>
    time_elapsed: string
    timestamp: string;
    last_activity_elapsed: string
    mine: bool
    liked: bool
}

// https://www.pillowfort.io/{username}/json/?p=1
type PillowfortPostsResponse = {
    posts: seq<PillowfortPost>
    total_count: int
}