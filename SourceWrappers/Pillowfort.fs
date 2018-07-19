namespace SourceWrappers

open PillowfortFs

type PillowfortSourceWrapper(client: PillowfortClient) =
    inherit SourceWrapper<int>()

    override this.Name = "Pillowfort"
    override this.SuggestedBatchSize = 1

    override this.Fetch cursor take = async {
        return {
            Posts = Seq.empty
            Next = 1
            HasMore = false
        }
    }

    override this.Whoami = client.AsyncWhoami

    override this.GetUserIcon size = client.AsyncGetAvatar