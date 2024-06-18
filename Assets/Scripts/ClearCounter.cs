using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClearCounter : BaseCounter {


    public override void Interact(Player player) {
        if (!HasKitchenObject() && player.HasKitchenObject()) {
            //player has kitchen object and counter is empty
            player.GetKitchenObject().SetKitchenObjectParent(this);

        }
        else if (!player.HasKitchenObject() && HasKitchenObject()) {
            //player has no kitchen object and counter has object
            GetKitchenObject().SetKitchenObjectParent(player);
        }
    }

}