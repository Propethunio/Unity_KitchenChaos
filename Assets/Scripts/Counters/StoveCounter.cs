using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StoveCounter : CounterMaster, IHasProgress {

    public event EventHandler<IHasProgress.OnProgressChangedEventArgs> OnProgressChanged;
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

    [SerializeField] private FryingRecipeSO[] fryingRecipesSOArray;

    private State state;
    private float fryingTimer;
    private FryingRecipeSO fryingRecipeSO;

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
                    OnProgressChanged?.Invoke(this, new IHasProgress.OnProgressChangedEventArgs {
                        progressNormalized = fryingTimer / fryingRecipeSO.fryingTimerMax
                    });
                    if(fryingTimer >= fryingRecipeSO.fryingTimerMax) {
                        GetKitchenObject().DestrySelf();
                        KitchenObject.SpawnKitchenObject(fryingRecipeSO.output, this);
                        fryingRecipeSO = GetFryingRecipeSOWithInput(GetKitchenObject().GetKitchenObjectSO());
                        if(fryingRecipeSO) {
                            fryingTimer = 0f;
                            state = State.Fried;
                            OnStateChanged?.Invoke(this, new OnStateChangedEventArgs {
                                state = state
                            });
                        } else {
                            state = State.Burned;
                            OnStateChanged?.Invoke(this, new OnStateChangedEventArgs {
                                state = state
                            });
                            OnProgressChanged?.Invoke(this, new IHasProgress.OnProgressChangedEventArgs {
                                progressNormalized = 0f
                            });
                        }
                    }
                    break;
                case State.Fried:
                    fryingTimer += Time.deltaTime;
                    OnProgressChanged?.Invoke(this, new IHasProgress.OnProgressChangedEventArgs {
                        progressNormalized = fryingTimer / fryingRecipeSO.fryingTimerMax
                    });
                    if(fryingTimer >= fryingRecipeSO.fryingTimerMax) {
                        GetKitchenObject().DestrySelf();
                        KitchenObject.SpawnKitchenObject(fryingRecipeSO.output, this);
                        state = State.Burned;
                        OnStateChanged?.Invoke(this, new OnStateChangedEventArgs {
                            state = state
                        });
                        OnProgressChanged?.Invoke(this, new IHasProgress.OnProgressChangedEventArgs {
                            progressNormalized = 0f
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
            if(player.HasKitchenObject()) {
                player.GetKitchenObject().SetKitchenObjectParent(this);
                fryingRecipeSO = GetFryingRecipeSOWithInput(GetKitchenObject().GetKitchenObjectSO());
                if(fryingRecipeSO) {
                    state = State.Frying;
                    OnStateChanged?.Invoke(this, new OnStateChangedEventArgs {
                        state = state
                    });
                }
            }
        } else {
            if(!player.HasKitchenObject()) {
                GetKitchenObject().SetKitchenObjectParent(player);
                fryingTimer = 0f;
                state = State.Idle;
                OnStateChanged?.Invoke(this, new OnStateChangedEventArgs {
                    state = state
                });
                OnProgressChanged?.Invoke(this, new IHasProgress.OnProgressChangedEventArgs {
                    progressNormalized = 0f
                });
            } else {
                if(player.GetKitchenObject().TryGetPlate(out PlateKitchenObject plateKitchenObject)) {
                    if(plateKitchenObject.TryAddIngredient(GetKitchenObject().GetKitchenObjectSO())) {
                        GetKitchenObject().DestrySelf();
                        fryingTimer = 0f;
                        state = State.Idle;
                        OnStateChanged?.Invoke(this, new OnStateChangedEventArgs {
                            state = state
                        });
                        OnProgressChanged?.Invoke(this, new IHasProgress.OnProgressChangedEventArgs {
                            progressNormalized = 0f
                        });
                    }
                } else {
                    if(GetKitchenObject().TryGetPlate(out plateKitchenObject)) {
                        if(plateKitchenObject.TryAddIngredient(player.GetKitchenObject().GetKitchenObjectSO())) {
                            player.GetKitchenObject().DestrySelf();
                        }
                    }
                }
            }
        }
    }

    private KitchenObjectSO GetOutputForInput(KitchenObjectSO inputKitchenObjectSO) {
        FryingRecipeSO fryingRecipeSO = GetFryingRecipeSOWithInput(inputKitchenObjectSO);
        if(fryingRecipeSO) {
            return fryingRecipeSO.output;
        }
        return null;
    }

    private FryingRecipeSO GetFryingRecipeSOWithInput(KitchenObjectSO inputKitchenObjectSO) {
        foreach(FryingRecipeSO fryingRecipeSO in fryingRecipesSOArray) {
            if(fryingRecipeSO.input == inputKitchenObjectSO) {
                return fryingRecipeSO;
            }
        }
        return null;
    }

    public KitchenObjectSO GetInputForOutput(KitchenObjectSO outputKitchenObjectSO) {
        FryingRecipeSO fryingRecipeSO = GetFryingRecipeSOWithOutput(outputKitchenObjectSO);
        if(fryingRecipeSO) {
            return fryingRecipeSO.input;
        }
        return null;
    }

    private FryingRecipeSO GetFryingRecipeSOWithOutput(KitchenObjectSO outputKitchenObjectSO) {
        foreach(FryingRecipeSO fryingRecipeSO in fryingRecipesSOArray) {
            if(fryingRecipeSO.output == outputKitchenObjectSO) {
                return fryingRecipeSO;
            }
        }
        return null;
    }

    public bool isFried() {
        return state == State.Fried;
    }
}