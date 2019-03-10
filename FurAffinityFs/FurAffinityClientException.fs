namespace FurAffinityFs

open System

type FurAffinityClientException(message: string) =
    inherit ApplicationException(message)