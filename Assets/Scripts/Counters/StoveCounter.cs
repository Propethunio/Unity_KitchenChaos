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

    [SerializeField] private FryingRecipeSO[] fryingRecepieSOArray;

    private State state;
    private float fryingTimer;
    private FryingRecipeSO fryingRecepieSO;

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
                        progressNormalized = fryingTimer / fryingRecepieSO.fryingTimerMax
                    });
                    if(fryingTimer >= fryingRecepieSO.fryingTimerMax) {
                        GetKitchenObject().DestrySelf();
                        KitchenObject.SpawnKitchenObject(fryingRecepieSO.output, this);
                        fryingRecepieSO = GetFryingRecepieSOWithInput(GetKitchenObject().GetKitchenObjectSO());
                        if(fryingRecepieSO) {
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
                        progressNormalized = fryingTimer / fryingRecepieSO.fryingTimerMax
                    });
                    if(fryingTimer >= fryingRecepieSO.fryingTimerMax) {
                        GetKitchenObject().DestrySelf();
                        KitchenObject.SpawnKitchenObject(fryingRecepieSO.output, this);
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
                fryingRecepieSO = GetFryingRecepieSOWithInput(GetKitchenObject().GetKitchenObjectSO());
                if(fryingRecepieSO) {
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
        FryingRecipeSO fryingRecepieSO = GetFryingRecepieSOWithInput(inputKitchenObjectSO);
        if(fryingRecepieSO) {
            return fryingRecepieSO.output;
        }
        return null;
    }

    private FryingRecipeSO GetFryingRecepieSOWithInput(KitchenObjectSO inputKitchenObjectSO) {
        foreach(FryingRecipeSO fryingRecepieSO in fryingRecepieSOArray) {
            if(fryingRecepieSO.input == inputKitchenObjectSO) {
                return fryingRecepieSO;
            }
        }
        return null;
    }

    public bool isFried() {
        return state == State.Fried;
    }
}