using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClearCounter : BaseCounter {

   
    public override void Interact(Player player) {
        if (!HasKitchenObject()) {
            if (player.HasKitchenObject()) {
                //player has kitchen object and counter is empty
                player.GetKitchenObject().SetKitchenObjectParent(this);

            }
        }
        else {
            if (!player.HasKitchenObject()) {
                //player has no kitchen object and counter has object
                GetKitchenObject().SetKitchenObjectParent(player);
            }
            else {
                //Player is carrying someting
                if (player.GetKitchenObject().TryGetPlate(out PlateKitchenObject plateKitchenObject)) {
                    //Player is holding a plate
                    bool ingredientAdded = plateKitchenObject.TryAddIngredient(GetKitchenObject().GetKitchenObjectSO());
                    if (ingredientAdded)
                        GetKitchenObject().DestroySelf();
                }
                else {
                    //Player is holding something other than plate
                    if (GetKitchenObject().TryGetPlate(out plateKitchenObject)) {
                        //Counter is holding a Plate
                        if (plateKitchenObject.TryAddIngredient(player.GetKitchenObject().GetKitchenObjectSO())){
                            player.GetKitchenObject().DestroySelf();
                        }
                    }
                }
            }
        }
    }
}