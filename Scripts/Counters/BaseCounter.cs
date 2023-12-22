using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class BaseCounter : MonoBehaviour, IKithcenObjectParent {

    public static event EventHandler OnAnyObjectPlaced;

    [SerializeField] private Transform counterTopPoint;

    private KitchenObject kitchenObject;

    public virtual void Interact(Player player) { }
    public virtual void InteractAlternate(Player player) { }

    public Transform GetFollowPoint() { return counterTopPoint; }

    public void SetKitcherObject(KitchenObject kitchenObject) { 
        this.kitchenObject = kitchenObject;
        
        if (kitchenObject != null) {
            OnAnyObjectPlaced?.Invoke(this, EventArgs.Empty);
        }
    }

    public KitchenObject GetKitchenObject() { return kitchenObject; }

    public void ClearKitchenObject() { kitchenObject = null; }

    public bool HasKitchenObject() { return kitchenObject != null; }

    public static void ResetSataticData() {
        OnAnyObjectPlaced = null;
    }
}
