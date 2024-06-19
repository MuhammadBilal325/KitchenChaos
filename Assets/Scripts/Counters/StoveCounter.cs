using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static CuttingCounter;

public class StoveCounter : BaseCounter, IHasProgress {

    public enum State {
        Idle,
        Frying,
        Fried,
        Burned
    }
    public event EventHandler<IHasProgress.OnProgressChangedEventArgs> OnProgressChanged;
    public event EventHandler<OnStateChangedEventArgs> OnStateChanged;
    public class OnStateChangedEventArgs : EventArgs {
        public State state;
    }
    [SerializeField] private FryingRecipeSO[] fryingRecipeSOArray;
    [SerializeField] private BurningRecipeSO[] burningRecipeSOArray;
    private State state;
    private float fryingTimer;
    private float burningTimer;
    private FryingRecipeSO fryingRecipeSO;
    private BurningRecipeSO burningRecipeSO;

    private void Start() {
        state = State.Idle;
  
    }
    private void Update() {
        switch (state) {
            case State.Idle:
                break;
            case State.Frying:
                fryingTimer += Time.deltaTime;
                OnProgressChanged?.Invoke(this, new IHasProgress.OnProgressChangedEventArgs { progressNormalized = fryingTimer/fryingRecipeSO.fryingTimer });
                if (fryingTimer >= fryingRecipeSO.fryingTimer) {
                    GetKitchenObject().DestroySelf();
                    KitchenObject.SpawnKitchenObject(fryingRecipeSO.output, this);
                    state = State.Fried;
                    burningTimer = 0;
                    burningRecipeSO = GetBurningRecipeSOWithInput(GetKitchenObject().GetKitchenObjectSO());
                    OnStateChanged?.Invoke(this, new OnStateChangedEventArgs{ state = state });
                 }
                break;
            case State.Fried:
                burningTimer += Time.deltaTime;
                OnProgressChanged?.Invoke(this, new IHasProgress.OnProgressChangedEventArgs { progressNormalized = burningTimer / burningRecipeSO.burningTimer });
                if (burningTimer >= burningRecipeSO.burningTimer) {
                    GetKitchenObject().DestroySelf();
                    KitchenObject.SpawnKitchenObject(burningRecipeSO.output, this);
                    state = State.Burned;
                    OnProgressChanged?.Invoke(this, new IHasProgress.OnProgressChangedEventArgs { progressNormalized = 0f });
                    OnStateChanged?.Invoke(this, new OnStateChangedEventArgs { state = state });
                }
                break;
            case State.Burned:
                break;
        }
        
    }
    public override void Interact(Player player) {
        if (!HasKitchenObject() && player.HasKitchenObject()) {
            //player has kitchen object and counter is empty
            if (GetFryingOutputForInput(player.GetKitchenObject().GetKitchenObjectSO()) != null) {
                player.GetKitchenObject().SetKitchenObjectParent(this);
                fryingRecipeSO= GetFryingRecipeSOWithInput(GetKitchenObject().GetKitchenObjectSO());
                state= State.Frying;
                fryingTimer = 0f;
            OnProgressChanged?.Invoke(this, new IHasProgress.OnProgressChangedEventArgs { progressNormalized = 0 });
               }
        }
        else if (HasKitchenObject()) {
            if (!player.HasKitchenObject()) {
                //player has no kitchen object and counter has object
                GetKitchenObject().SetKitchenObjectParent(player);
                state = State.Idle;
                OnProgressChanged?.Invoke(this, new IHasProgress.OnProgressChangedEventArgs { progressNormalized = 0f });
            }
            else if (player.GetKitchenObject().TryGetPlate(out PlateKitchenObject plateKitchenObject)) {
                //Player is holding a plate
                bool ingredientAdded = plateKitchenObject.TryAddIngredient(GetKitchenObject().GetKitchenObjectSO());
                if (ingredientAdded) {
                    GetKitchenObject().DestroySelf();
                    state = State.Idle;
                    OnProgressChanged?.Invoke(this, new IHasProgress.OnProgressChangedEventArgs { progressNormalized = 0f });
                }
            }
        }
        OnStateChanged?.Invoke(this, new OnStateChangedEventArgs { state = state });
    }

    private  KitchenObjectSO GetFryingOutputForInput(KitchenObjectSO inputKitchenObjectSO) {
        foreach (FryingRecipeSO fryingRecipeSO in fryingRecipeSOArray) {
            if (fryingRecipeSO.input == inputKitchenObjectSO) {
                return fryingRecipeSO.output;
            }
        }
        return null;
    }
    private FryingRecipeSO GetFryingRecipeSOWithInput(KitchenObjectSO inputKitchenObjectSO) {
        foreach (FryingRecipeSO fryingRecipeSO in fryingRecipeSOArray) {
            if (fryingRecipeSO.input == inputKitchenObjectSO) {
                return fryingRecipeSO;
            }
        }
        return null;
    }
    private KitchenObjectSO GetBurningOutputForInput(KitchenObjectSO inputKitchenObjectSO) {
        foreach (BurningRecipeSO burningRecipeSO in burningRecipeSOArray) {
            if (burningRecipeSO.input == inputKitchenObjectSO) {
                return burningRecipeSO.output;
            }
        }
        return null;
    }
    private BurningRecipeSO GetBurningRecipeSOWithInput(KitchenObjectSO inputKitchenObjectSO) {
        foreach (BurningRecipeSO burningRecipeSO in burningRecipeSOArray) {
            if (burningRecipeSO.input == inputKitchenObjectSO) {
                return burningRecipeSO;
            }
        }
        return null;
    }
}
