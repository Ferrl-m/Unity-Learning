using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class CuttingCounter : BaseCounter, IHasProgress {

    [SerializeField] private CuttingRecipeSO[] cuttingRecipeSOArray;

    public event EventHandler<IHasProgress.OnProgressChangeddEventArgs> OnProgressChanged;

    public static event EventHandler OnAnyCut;
    public event EventHandler OnCut;

    private int cuttingProgress;
    public override void Interact(Player player) {
        if(!HasKitchenObject()) {
            if(player.HasKitchenObject() && HasRecipeWithInput(player.GetKitchenObject().GetKitchenObjectSO())) {
                player.GetKitchenObject().SetKitchenObjectparent(this);
                CuttingRecipeSO cuttingRecipeSO = GetCuttingRecipeSOWithInput(GetKitchenObject().GetKitchenObjectSO());

                cuttingProgress = 0;

                ShowProgressBar(cuttingRecipeSO);
            }
        } else if(!player.HasKitchenObject()) {
            GetKitchenObject().SetKitchenObjectparent(player);
        } else if(player.GetKitchenObject().TryGetPlate(out PlateKitchenObject plateKitchenObject)) {
            if(plateKitchenObject.TryAddIngredient(GetKitchenObject().GetKitchenObjectSO())) {
                GetKitchenObject().DestroySelf();
            }
        }
        }

    public override void InteractAlternate(Player player) {
        if (HasKitchenObject() && HasRecipeWithInput(GetKitchenObject().GetKitchenObjectSO())) {
            KitchenObjectSO inputKitchenObjectSO = GetKitchenObject().GetKitchenObjectSO();

            cuttingProgress++;
            CuttingRecipeSO cuttingRecipeSO = GetCuttingRecipeSOWithInput(inputKitchenObjectSO);
            ShowProgressBar(cuttingRecipeSO);

            OnCut?.Invoke(this, EventArgs.Empty);
            OnAnyCut?.Invoke(this, EventArgs.Empty);

            if(cuttingProgress >= cuttingRecipeSO.cuttingProgressMax) {
                KitchenObjectSO outputKitchenObjectSO = GetOutputForInput(GetKitchenObject().GetKitchenObjectSO());
                GetKitchenObject().DestroySelf();

                KitchenObject.SpawnKitchenObject(outputKitchenObjectSO, this);
            }
            
        }
    }

    private KitchenObjectSO GetOutputForInput(KitchenObjectSO inputKitchenObjectSO) {
        CuttingRecipeSO cuttingRecipeSO = GetCuttingRecipeSOWithInput(inputKitchenObjectSO);

        if(cuttingRecipeSO != null) {
            return cuttingRecipeSO.output;
        } else return null;
    }

    private bool HasRecipeWithInput(KitchenObjectSO inputKitchenObjectSO) {
        return GetCuttingRecipeSOWithInput(inputKitchenObjectSO) != null; 
    }

    private CuttingRecipeSO GetCuttingRecipeSOWithInput(KitchenObjectSO inputKitchenObjectSO) {
        foreach(CuttingRecipeSO cuttingRecipeSO in cuttingRecipeSOArray) {
            if(cuttingRecipeSO.input == inputKitchenObjectSO) {
                return cuttingRecipeSO;
            }
        }

        return null;
    }

    private void ShowProgressBar(CuttingRecipeSO cuttingRecipeSO) {
        OnProgressChanged?.Invoke(this, new IHasProgress.OnProgressChangeddEventArgs {
            progressNormalized = (float)cuttingProgress / cuttingRecipeSO.cuttingProgressMax
        });
    }

    new public static void ResetSataticData() {
        OnAnyCut = null;
    }
}
