using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class DeliveryManager : MonoBehaviour {

    public static DeliveryManager Instance { get; private set; }

    public event EventHandler OnRecipeSpawn;
    public event EventHandler OnRecipeCompleted;
    public event EventHandler OnRecipeSuccess;
    public event EventHandler OnRecipeFail;

    [SerializeField] private RecipeListSO recipeListSO;

    private List<RecipeSO> waitingRecipeSOList;
    private float spawnRecipeTimer;
    private float spawnRecipeTimerMax = 4f;
    private int waitingRecipesMax = 4;
    private int successfullRecipesAmount;

    private void Awake() {
        Instance = this;
        waitingRecipeSOList = new List<RecipeSO>();
    }
    private void Update() {
        if (GameManager.Instance.IsGamePlaying()) {
            spawnRecipeTimer -= Time.deltaTime;
            if(spawnRecipeTimer < 0f) {
                spawnRecipeTimer = spawnRecipeTimerMax;

                if(waitingRecipeSOList.Count < waitingRecipesMax) {
                    RecipeSO waitingRecipeSO = recipeListSO.recipeSOList[UnityEngine.Random.Range(0, recipeListSO.recipeSOList.Count)];
                    waitingRecipeSOList.Add(waitingRecipeSO);
                    OnRecipeSpawn?.Invoke(this, EventArgs.Empty);
                }
            }
        }
    }

    public void DeliverRecipe(PlateKitchenObject plateKitchenObject) {
        for(int i = 0; i < waitingRecipeSOList.Count; i++) {
            RecipeSO waitingRecipeSo = waitingRecipeSOList[i];
            List<KitchenObjectSO> plateKitchenObjectSOList = plateKitchenObject.GetKitchenObjectSOList();

            bool validIngridients = true;

            if(waitingRecipeSo.kitchenObjectSOList.Count == plateKitchenObjectSOList.Count) {
                foreach(KitchenObjectSO plateKitchenObjectSO in plateKitchenObjectSOList) {
                    if(!waitingRecipeSo.kitchenObjectSOList.Contains(plateKitchenObjectSO)) {
                        validIngridients = false;
                        break;
                    }
                }
            } else validIngridients = false;

            if(validIngridients) {
                Debug.Log("Player delivered " + waitingRecipeSo.name);
                waitingRecipeSOList.RemoveAt(i);
                OnRecipeCompleted?.Invoke(this, EventArgs.Empty);
                OnRecipeSuccess?.Invoke(this, EventArgs.Empty);
                successfullRecipesAmount++;
                return;
            }
        }
        OnRecipeFail?.Invoke(this, EventArgs.Empty);
    }

    public List<RecipeSO> GetWaitingRecipeSOLsit() { return waitingRecipeSOList; }

    public int GetSuccessfullRecipesAmount() { return successfullRecipesAmount; }
}
