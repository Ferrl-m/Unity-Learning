using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KitchenObject : MonoBehaviour
{
    [SerializeField] private KitchenObjectSO kitchenObjectSO;

    private IKithcenObjectParent kitchenObjectParent;

    public void SetKitchenObjectparent(IKithcenObjectParent kitchenObjectParent) {
        if (kitchenObjectParent.HasKitchenObject()) {
            Debug.LogError("kitchenObjectParent already has an object!");
            return;
        }
        if (this.kitchenObjectParent != null) {
            this.kitchenObjectParent.ClearKitchenObject();
        }

        this.kitchenObjectParent = kitchenObjectParent;
        this.kitchenObjectParent.SetKitcherObject(this);
        transform.parent = kitchenObjectParent.GetFollowPoint();
        transform.localPosition = Vector3.zero;
    }

    public IKithcenObjectParent GetKitchenObjectParent() { return kitchenObjectParent; }

    public KitchenObjectSO GetKitchenObjectSO() {
        return kitchenObjectSO; 
    }

    public void DestroySelf() {
        kitchenObjectParent.ClearKitchenObject();
        Destroy(gameObject);
    }

    public bool TryGetPlate(out PlateKitchenObject plateKitchenObject) {
        if(this is PlateKitchenObject) {
            plateKitchenObject = this as PlateKitchenObject;
            return true;
        } else {
            plateKitchenObject = null;
            return false;
        }
    }

    public static void SpawnKitchenObject(KitchenObjectSO kitchenObjectSO, IKithcenObjectParent kithcenObjectParent) {
        Transform kitchenObjectTransform = Instantiate(kitchenObjectSO.prefab);
        kitchenObjectTransform.GetComponent<KitchenObject>().SetKitchenObjectparent(kithcenObjectParent);
    }
}
