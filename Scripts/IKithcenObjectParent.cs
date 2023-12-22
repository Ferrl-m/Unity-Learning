using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IKithcenObjectParent {
    public Transform GetFollowPoint();

    public void SetKitcherObject(KitchenObject kitchenObject);

    public KitchenObject GetKitchenObject();

    public void ClearKitchenObject();

    public bool HasKitchenObject();
}
