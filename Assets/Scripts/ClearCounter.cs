using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClearCounter : MonoBehaviour {

    [SerializeField] private KitchenObjectSO kitchenObjectSO;
    [SerializeField] private Transform counterTopPoint;
    [SerializeField] private ClearCounter secondClearCounter;
    public bool testing;
    private KitchenObject kitchenObject;

    private void Update() {
        if(testing && Input.GetKeyDown(KeyCode.W)) {
            if (kitchenObject != null) {
                kitchenObject.SetClearCounter(secondClearCounter);
                Debug.Log(kitchenObject.GetClearCounter());
            }
        }
    }
    public void Interact() {
        if (kitchenObject == null) {
            Transform kitchenObjectTransform = Instantiate(kitchenObjectSO.prefab, counterTopPoint);
            kitchenObjectTransform.localPosition = Vector3.zero;

            kitchenObject = kitchenObjectTransform.GetComponent<KitchenObject>();
            kitchenObject.SetClearCounter(this);

        }
    }


}