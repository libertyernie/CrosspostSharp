namespace FurAffinityFs

type IFurAffinityCredentials =
    abstract member A: string
    abstract member B: string

type FurAffinityCredentials = {
    a: string
    b: string
} with
    interface IFurAffinityCredentials with
        member this.A = this.a
        member this.B = this.b