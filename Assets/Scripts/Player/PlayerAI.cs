using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class PlayerAI : MonoBehaviour {

    [SerializeField] private DeliveryManager deliveryManager;

    private NavMeshAgent agent;
    private Vector3 lastPosition;
    private CounterMaster target;
    private CounterMaster savedTarget;
    private ClearCounter counterWithPlate;
    private RecipeSO waitingRecipe;
    private KitchenObjectSO neededRecipeIngredient;
    private KitchenObjectSO neededBaseIngredient;
    private KitchenObject ingredient;
    private KitchenObject plate;
    private List<ClearCounter> clearCountersList;
    private List<ContainerCounter> containerCountersList;
    private List<CuttingCounter> cuttingCounterList;
    private List<StoveCounter> stoveCountersList;
    private List<PlatesCounter> platesCountersList;
    private List<DeliveryCounter> deliveryCountersList;
    private List<TrashCounter> trashCountersList;
    private RecipeType recipeType;
    private enum RecipeType {
        None,
        Cut,
        Fry,
    }
    private float waitTimer;
    private int ingredientPlace;
    private bool recipeReady;
    private bool changePlatePlace;


    private void Start() {
        if(Player.Instance.isAI) {
            agent = GetComponent<NavMeshAgent>();
            lastPosition = transform.position;
            clearCountersList = new List<ClearCounter>();
            containerCountersList = new List<ContainerCounter>();
            cuttingCounterList = new List<CuttingCounter>();
            stoveCountersList = new List<StoveCounter>();
            platesCountersList = new List<PlatesCounter>();
            deliveryCountersList = new List<DeliveryCounter>();
            trashCountersList = new List<TrashCounter>();
            clearCountersList.AddRange(FindObjectsOfType<ClearCounter>());
            containerCountersList.AddRange(FindObjectsOfType<ContainerCounter>());
            cuttingCounterList.AddRange(FindObjectsOfType<CuttingCounter>());
            stoveCountersList.AddRange(FindObjectsOfType<StoveCounter>());
            platesCountersList.AddRange(FindObjectsOfType<PlatesCounter>());
            deliveryCountersList.AddRange(FindObjectsOfType<DeliveryCounter>());
            trashCountersList.AddRange(FindObjectsOfType<TrashCounter>());
        } else {
            this.enabled = false;
        }
    }

    private void Update() {
        SetIsWalking();
        if(waitTimer <= 0f) {
            if(!waitingRecipe) {
                CheckForRecipe();
            } else {
                WorkOnRecipe();
            }
        } else {
            waitTimer -= Time.deltaTime;
        }
    }

    private void WorkOnRecipe() {
        if(!plate) {
            GetPlate();
        } else if(Player.Instance.GetKitchenObject() == plate && !recipeReady && target) {
            PutPlateOnClearCounter();
        } else if(Player.Instance.GetKitchenObject() == plate && recipeReady) {
            DeliverRecipe();
        } else if(ingredient) {
            WorkWithIngredient();
        } else if(changePlatePlace) {
            ChangePlatePlace();
        } else if(neededBaseIngredient) {
            GetBaseIngredient();
        } else {
            CheckForNextIngredient();
        }
    }

    private void ChangePlatePlace() {
        if(!target) {
            target = counterWithPlate;
        } else if(!Player.Instance.isWalking && Vector3.Distance(transform.position, target.transform.position) < 1.5f) {
            target.Interact(Player.Instance);
            changePlatePlace = false;
            target = null;
            waitTimer = .4f;
        }
    }

    private void DeliverRecipe() {
        if(!target) {
            SetTargetDeliveryCounter();
        } else if(!Player.Instance.isWalking && Vector3.Distance(transform.position, target.transform.position) < 1.5f) {
            target.Interact(Player.Instance);
            target = null;
            waitingRecipe = null;
            recipeReady = false;
            plate = null;
            counterWithPlate = null;
            ingredientPlace = 0;
            waitTimer = 1f;
        }
    }

    private void GrabPlateForDelivery() {
        if(!target) {
            target = counterWithPlate;
        } else if(!Player.Instance.isWalking && Vector3.Distance(transform.position, target.transform.position) < 1.5f) {
            target.Interact(Player.Instance);
            target = null;
            recipeReady = true;
            waitTimer = .4f;
        }
    }

    private void WorkWithIngredient() {
        if(ingredient.GetKitchenObjectSO() == neededRecipeIngredient) {
            PutIngredientOnPlate();
        } else {
            switch(recipeType) {
                case RecipeType.Fry:
                    if(!target) {
                        SetTargetStoveCounter();
                    } else {
                        if(!Player.Instance.isWalking && Vector3.Distance(transform.position, target.transform.position) < 1.5f) {
                            if(Player.Instance.HasKitchenObject()) {
                                target.Interact(Player.Instance);
                                waitTimer = .4f;
                            } else if(target.GetKitchenObject().GetKitchenObjectSO() == neededRecipeIngredient) {
                                ingredient = target.GetKitchenObject();
                                target.Interact(Player.Instance);
                                target = null;
                                waitTimer = .6f;
                            }
                        }
                    }
                    break;
                case RecipeType.Cut:
                    if(!target) {
                        SetTargetCuttingCounter();
                    } else {
                        if(!Player.Instance.isWalking && Vector3.Distance(transform.position, target.transform.position) < 1.5f) {
                            if(Player.Instance.HasKitchenObject()) {
                                target.Interact(Player.Instance);
                                waitTimer = .4f;
                            } else {
                                target.InteractAlternate(Player.Instance);
                                ingredient = target.GetKitchenObject();
                                waitTimer = .6f;
                            }
                        }
                    }
                    break;
            }
        }
    }

    private void PutIngredientOnPlate() {
        if(!Player.Instance.HasKitchenObject()) {
            target.Interact(Player.Instance);
            target = null;
            waitTimer = .2f;
        } else {
            if(!target) {
                target = counterWithPlate;
                agent.SetDestination(target.transform.position);
            } else if(!Player.Instance.isWalking && Vector3.Distance(transform.position, target.transform.position) < 1.5f) {
                target.Interact(Player.Instance);
                target = null;
                ingredient = null;
                neededRecipeIngredient = null;
                neededBaseIngredient = null;
                waitTimer = .4f;
            }
        }
    }

    private void GetBaseIngredient() {
        if(!target) {
            SetTargetContainerCounter();
        } else {
            if(!Player.Instance.isWalking && Vector3.Distance(transform.position, target.transform.position) < 1.5f) {
                target.Interact(Player.Instance);
                ingredient = Player.Instance.GetKitchenObject();
                target = null;
            }
        }
    }

    private void PutPlateOnClearCounter() {
        if(!(target.GetType() == typeof(ClearCounter))) {
            savedTarget = target;
            SetCleanCounter();
            agent.SetDestination(target.transform.position);
        } else {
            if(!Player.Instance.isWalking && Vector3.Distance(transform.position, target.transform.position) < 1.5f) {
                if(Player.Instance.HasKitchenObject()) {
                    target.Interact(Player.Instance);
                    counterWithPlate = target as ClearCounter;
                    target = savedTarget;
                    agent.SetDestination(target.transform.position);
                }
            }
        }
    }

    private void GetPlate() {
        if(!target) {
            SetTargetPlatesCounter();
        } else {
            if(!Player.Instance.isWalking && Vector3.Distance(transform.position, target.transform.position) < 1.5f) {
                target.Interact(Player.Instance);
                plate = Player.Instance.GetKitchenObject();
                target = null;
            }
        }
    }

    private void CheckForNextIngredient() {
        if(waitingRecipe.kitchenObjectSOList.Count > ingredientPlace) {
            neededRecipeIngredient = waitingRecipe.kitchenObjectSOList[ingredientPlace];
            ingredientPlace++;
        }
        if(neededRecipeIngredient) {
            RecipeType currRecipeType = recipeType;
            SetNeededBaseIngredient();
            if(ingredientPlace > 1 && currRecipeType != recipeType) {
                changePlatePlace = true;
            }
        } else {
            GrabPlateForDelivery();
        }
    }

    private void SetNeededBaseIngredient() {
        switch(neededRecipeIngredient.outputOfRecipeType) {
            case KitchenObjectSO.OutputOfRecipeType.None:
                neededBaseIngredient = neededRecipeIngredient;
                recipeType = RecipeType.None;
                break;
            case KitchenObjectSO.OutputOfRecipeType.Fry:
                neededBaseIngredient = stoveCountersList[0].GetInputForOutput(neededRecipeIngredient);
                recipeType = RecipeType.Fry;
                break;
            case KitchenObjectSO.OutputOfRecipeType.Cut:
                neededBaseIngredient = cuttingCounterList[0].GetInputForOutput(neededRecipeIngredient);
                recipeType = RecipeType.Cut;
                break;
        }
    }

    private void SetCleanCounter() {
        switch(recipeType) {
            case RecipeType.None:
                ClearCounter currClearCounter = null;
                foreach(ClearCounter clearCounter in clearCountersList) {
                    if(!currClearCounter) {
                        currClearCounter = clearCounter;
                    } else if(Vector3.Distance(clearCounter.transform.position, target.transform.position) < Vector3.Distance(currClearCounter.transform.position, target.transform.position)) {
                        currClearCounter = clearCounter;
                    }
                }
                target = currClearCounter;
                break;
            case RecipeType.Fry:
                StoveCounter currStoveCounter = null;
                foreach(StoveCounter stoveCounter in stoveCountersList) {
                    if(!currStoveCounter) {
                        currStoveCounter = stoveCounter;
                    } else if(Vector3.Distance(transform.position, stoveCounter.transform.position) < Vector3.Distance(transform.position, currStoveCounter.transform.position)) {
                        currStoveCounter = stoveCounter;
                    }
                }
                currClearCounter = null;
                foreach(ClearCounter clearCounter in clearCountersList) {
                    if(!currClearCounter) {
                        currClearCounter = clearCounter;
                    } else if(Vector3.Distance(clearCounter.transform.position, currStoveCounter.transform.position) < Vector3.Distance(currClearCounter.transform.position, currStoveCounter.transform.position)) {
                        currClearCounter = clearCounter;
                    }
                }
                target = currClearCounter;
                break;
            case RecipeType.Cut:
                CuttingCounter currCuttingCounter = null;
                foreach(CuttingCounter cuttingCounter in cuttingCounterList) {
                    if(!currCuttingCounter) {
                        currCuttingCounter = cuttingCounter;
                    } else if(Vector3.Distance(target.transform.position, cuttingCounter.transform.position) < Vector3.Distance(target.transform.position, currCuttingCounter.transform.position)) {
                        currCuttingCounter = cuttingCounter;
                    }
                }
                currClearCounter = null;
                foreach(ClearCounter clearCounter in clearCountersList) {
                    if(!currClearCounter) {
                        currClearCounter = clearCounter;
                    } else if(Vector3.Distance(clearCounter.transform.position, currCuttingCounter.transform.position) < Vector3.Distance(currClearCounter.transform.position, currCuttingCounter.transform.position)) {
                        currClearCounter = clearCounter;
                    }
                }
                target = currClearCounter;
                break;
        }
    }

    private void SetTargetStoveCounter() {
        foreach(StoveCounter stoveCounter in stoveCountersList) {
            if(!target) {
                target = stoveCounter;
            } else if(Vector3.Distance(transform.position, stoveCounter.transform.position) < Vector3.Distance(transform.position, target.transform.position)) {
                target = stoveCounter;
            }
        }
        agent.SetDestination(target.transform.position);
    }

    private void SetTargetDeliveryCounter() {
        foreach(DeliveryCounter deliveryCounter in deliveryCountersList) {
            if(!target) {
                target = deliveryCounter;
            } else if(Vector3.Distance(transform.position, deliveryCounter.transform.position) < Vector3.Distance(transform.position, target.transform.position)) {
                target = deliveryCounter;
            }
        }
        agent.SetDestination(target.transform.position);
    }

    private void SetTargetPlatesCounter() {
        foreach(PlatesCounter platesCounter in platesCountersList) {
            if(!target) {
                target = platesCounter;
            } else if(Vector3.Distance(transform.position, platesCounter.transform.position) < Vector3.Distance(transform.position, target.transform.position)) {
                target = platesCounter;
            }
        }
        agent.SetDestination(target.transform.position);
    }

    private void SetTargetCuttingCounter() {
        foreach(CuttingCounter cuttingCounter in cuttingCounterList) {
            if(!target) {
                target = cuttingCounter;
            } else if(Vector3.Distance(transform.position, cuttingCounter.transform.position) < Vector3.Distance(transform.position, target.transform.position)) {
                target = cuttingCounter;
            }
        }
        agent.SetDestination(target.transform.position);
    }

    private void SetTargetContainerCounter() {
        foreach(ContainerCounter containerCounter in containerCountersList) {
            if(containerCounter.kitchenObjectSO == neededBaseIngredient) {
                target = containerCounter;
                if(Vector3.Distance(transform.position, target.transform.position) > 0) {
                    agent.SetDestination(target.transform.position);
                }
                break;
            }
        }
    }

    private void CheckForRecipe() {
        if(deliveryManager.GetWaitingRecipeSOList().Count > 0) {
            waitingRecipe = deliveryManager.GetWaitingRecipeSOList()[0];
        }
    }

    private void SetIsWalking() {
        if(lastPosition != transform.position) {
            lastPosition = transform.position;
            Player.Instance.isWalking = true;
        } else {
            Player.Instance.isWalking = false;
        }
    }
}