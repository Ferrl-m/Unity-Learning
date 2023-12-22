using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class StoveCounter : BaseCounter, IHasProgress {

    public event EventHandler<IHasProgress.OnProgressChangeddEventArgs> OnProgressChanged;

    public event EventHandler<OnStateChangedEventArgs> OnStateChanged;
    public class OnStateChangedEventArgs : EventArgs {
        public State state;
    }
    public enum State {
        Idle,
        Frying,
        Fried,
        Burned,
    }

    [SerializeField] private FryingRecipeSO[] fryingRecipeSOArray;
    [SerializeField] private BurningRecipeSO[] burningRecipeSOArray;

    private float fryingTimer;
    private float burningTimer;
    private FryingRecipeSO fryingRecipeSO;
    private BurningRecipeSO burningRecipeSO;
    private State state;

    private void Start() {
        state = State.Idle;
    }

    private void Update() {
        if(HasKitchenObject()) {
            switch(state) {
                case State.Idle:
                    break;
                case State.Frying:
                    fryingTimer += Time.deltaTime;
                    OnProgressChanged?.Invoke(this, new IHasProgress.OnProgressChangeddEventArgs {
                        progressNormalized = fryingTimer / fryingRecipeSO.fryingTimerMax
                    });

                    if(fryingTimer > fryingRecipeSO.fryingTimerMax) {
                        GetKitchenObject().DestroySelf();

                        KitchenObject.SpawnKitchenObject(fryingRecipeSO.output, this);
                        state = State.Fried;
                        fryingTimer = 0f;
                        burningTimer = 0f;

                        burningRecipeSO = GetFryinggBurningRecipeSOWithInput(GetKitchenObject().GetKitchenObjectSO());

                        ChangeStageEvent();
                    }
                    break;
                case State.Fried:
                    burningTimer += Time.deltaTime;
                    OnProgressChanged?.Invoke(this, new IHasProgress.OnProgressChangeddEventArgs {
                        progressNormalized = burningTimer / burningRecipeSO.burningTimerMax
                    });

                    if(burningTimer > burningRecipeSO.burningTimerMax) {
                        GetKitchenObject().DestroySelf();

                        KitchenObject.SpawnKitchenObject(burningRecipeSO.output, this);
                        state = State.Burned;

                        ChangeStageEvent();

                        OnProgressChanged?.Invoke(this, new IHasProgress.OnProgressChangeddEventArgs {
                            progressNormalized = 1f
                        });
                    }
                    break;
                case State.Burned:
                    break;
            }
        }
    }

    public override void Interact(Player player) {
        if(!HasKitchenObject()) {
            if(player.HasKitchenObject() && HasRecipeWithInput(player.GetKitchenObject().GetKitchenObjectSO())) {
                player.GetKitchenObject().SetKitchenObjectparent(this);

                fryingRecipeSO = GetFryinggRecipeSOWithInput(GetKitchenObject().GetKitchenObjectSO());
                
                state = State.Frying;

                ChangeStageEvent();

                OnProgressChanged?.Invoke(this, new IHasProgress.OnProgressChangeddEventArgs {
                    progressNormalized = fryingTimer / fryingRecipeSO.fryingTimerMax
                });
            }
        } else if(!player.HasKitchenObject()) {
            GetKitchenObject().SetKitchenObjectparent(player);
            ResetStage();

        } else if(player.GetKitchenObject().TryGetPlate(out PlateKitchenObject plateKitchenObject)) {
            if(plateKitchenObject.TryAddIngredient(GetKitchenObject().GetKitchenObjectSO())) {
                GetKitchenObject().DestroySelf();
                ResetStage();
            }
        }
    }

    private KitchenObjectSO GetOutputForInput(KitchenObjectSO inputKitchenObjectSO) {
        FryingRecipeSO fryingRecipeSO = GetFryinggRecipeSOWithInput(inputKitchenObjectSO);

        if(fryingRecipeSO != null) {
            return fryingRecipeSO.output;
        } else return null;
    }

    private bool HasRecipeWithInput(KitchenObjectSO inputKitchenObjectSO) {
        return GetFryinggRecipeSOWithInput(inputKitchenObjectSO) != null;
    }

    private FryingRecipeSO GetFryinggRecipeSOWithInput(KitchenObjectSO inputKitchenObjectSO) {
        foreach(FryingRecipeSO fryingRecipeSO in fryingRecipeSOArray) {
            if(fryingRecipeSO.input == inputKitchenObjectSO) {
                return fryingRecipeSO;
            }
        }

        return null;
    }

    private BurningRecipeSO GetFryinggBurningRecipeSOWithInput(KitchenObjectSO inputKitchenObjectSO) {
        foreach(BurningRecipeSO burningRecipeSO in burningRecipeSOArray) {
            if(burningRecipeSO.input == inputKitchenObjectSO) {
                return burningRecipeSO;
            }
        }

        return null;
    }

    private void ChangeStageEvent() {
        OnStateChanged?.Invoke(this, new OnStateChangedEventArgs {
            state = state
        });
    }

    private void ResetStage() {
        state = State.Idle;

        ChangeStageEvent();

        OnProgressChanged?.Invoke(this, new IHasProgress.OnProgressChangeddEventArgs {
            progressNormalized = 1f
        });
    }

    public bool IsFried() { return state == State.Fried; }
}
