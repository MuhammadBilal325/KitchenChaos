using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CuttingCounter : BaseCounter,IHasProgress {

    public static event EventHandler OnAnyCut;

    new public static void ResetStaticData() {
        OnAnyCut = null;
    }

    public event EventHandler<IHasProgress.OnProgressChangedEventArgs> OnProgressChanged;
    public event EventHandler OnCut;
    [SerializeField] private CuttingRecipeSO[] cuttingRecipeSOArray;
    private int cuttingProgress;
    public override void Interact(Player player) {
        if (!HasKitchenObject() && player.HasKitchenObject()) {
            //player has kitchen object and counter is empty
            if (GetOutputForInput(player.GetKitchenObject().GetKitchenObjectSO()) != null) {
                player.GetKitchenObject().SetKitchenObjectParent(this);
                cuttingProgress = 0;
                OnProgressChanged?.Invoke(this, new IHasProgress.OnProgressChangedEventArgs { progressNormalized = 0 });
            }
        }
        else if (HasKitchenObject()) {
            if (!player.HasKitchenObject()) {

                //player has no kitchen object and counter has object
                GetKitchenObject().SetKitchenObjectParent(player);
            }
            else if (player.GetKitchenObject().TryGetPlate(out PlateKitchenObject plateKitchenObject)) {
                //Player is holding a plate
                bool ingredientAdded = plateKitchenObject.TryAddIngredient(GetKitchenObject().GetKitchenObjectSO());
                if (ingredientAdded)
                    GetKitchenObject().DestroySelf();
            }
        }
    }

    public override void InteractAlternate(Player player) {
        if (HasKitchenObject()) {
            KitchenObjectSO outputKitchenObjectSO = GetOutputForInput(GetKitchenObject().GetKitchenObjectSO());
            if (outputKitchenObjectSO == null) {
                return;
            }
            OnCut?.Invoke(this, EventArgs.Empty);
            OnAnyCut?.Invoke(this,EventArgs.Empty);
            cuttingProgress++;
            CuttingRecipeSO cuttingRecipeSO = GetCuttingRecipeSOWithInput(GetKitchenObject().GetKitchenObjectSO());
            OnProgressChanged?.Invoke(this, new IHasProgress.OnProgressChangedEventArgs { progressNormalized = (float)cuttingProgress / cuttingRecipeSO.cuttingProgressMax });
            if (cuttingProgress >= cuttingRecipeSO.cuttingProgressMax) {
                GetKitchenObject().DestroySelf();
                KitchenObject.SpawnKitchenObject(outputKitchenObjectSO, this);
                cuttingProgress = 0;
                OnProgressChanged?.Invoke(this, new IHasProgress.OnProgressChangedEventArgs { progressNormalized = 0 });
            }
        }
    }
    private KitchenObjectSO GetOutputForInput(KitchenObjectSO inputKitchenObjectSO) {
        foreach (CuttingRecipeSO cuttingRecipeSO in cuttingRecipeSOArray) {
            if (cuttingRecipeSO.input == inputKitchenObjectSO) {
                return cuttingRecipeSO.output;
            }
        }
        return null;
    }
    private CuttingRecipeSO GetCuttingRecipeSOWithInput(KitchenObjectSO inputKitchenObjectSO) {
        foreach (CuttingRecipeSO cuttingRecipeSO in cuttingRecipeSOArray) {
            if (cuttingRecipeSO.input == inputKitchenObjectSO) {
                return cuttingRecipeSO;
            }
        }
        return null;
    }
}

