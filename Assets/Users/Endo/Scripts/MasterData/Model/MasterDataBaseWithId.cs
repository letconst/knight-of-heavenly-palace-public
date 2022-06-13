using UnityEngine;

public abstract class MasterDataBaseWithId : MasterDataBase
{
    [SerializeField]
    private string id;

    public string Id => id;
}
