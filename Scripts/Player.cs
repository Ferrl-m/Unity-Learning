using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Player : MonoBehaviour, IKithcenObjectParent {
    public static Player Instance { get; private set; }
    
    public event EventHandler<OnSelectedCounterChangedEventArgs> OnSelectedCounterChanged;
    public class OnSelectedCounterChangedEventArgs : EventArgs {
        public BaseCounter selectedCounter;
    }

    public event EventHandler OnPlayerGrab;

    [SerializeField] private float moveSpeed = 7f; 
    [SerializeField] private float rotateSpeed = 10f;
    [SerializeField] private Transform kitchenObjectHoldPoint;
    [SerializeField] private GameInput gameInput;
    [SerializeField] private LayerMask layerMask;

    private float playerRadius = .6f;
    private float playerHeight = 2f;

    private float moveDistance;
    private bool isWalking;
    private Vector3 lastDirection;
    private BaseCounter selectedCounter;
    private KitchenObject kitchenObject;

    private void Awake() {
        if(Instance != null) {
            Debug.LogError("There is more than 1 Player instance");
        }
        Instance = this;
    }

    private void Start() {
        gameInput.OnInteractAction += GameInput_OnInteractAction;
        gameInput.OnInteractAlternateAction += GameInput_OnInteractAlternateAction;
    }

    private void GameInput_OnInteractAlternateAction(object sender, EventArgs e) {
        if(!GameManager.Instance.IsGamePlaying()) return;

        if(selectedCounter != null) {
            selectedCounter.InteractAlternate(this);
        }
    }

    private void GameInput_OnInteractAction(object sender, System.EventArgs e) {
        if(!GameManager.Instance.IsGamePlaying()) return;

        if(selectedCounter != null) {
            selectedCounter.Interact(this);
        }
    }

    // Update with 2 methods
    private void Update() {
        HandleMovement();
        HandleInteractions();
    }

    private bool CanMoveTo(Vector3 moveDir) {
        return !Physics.CapsuleCast(transform.position, transform.position + Vector3.up * playerHeight, playerRadius, moveDir, moveDistance);
    }

    public bool IsWalking() {
        return isWalking;
    }

    // Movement method
    private void HandleMovement() {
        Vector3 moveDir = GetMoveDirection();

        moveDistance = moveSpeed * Time.deltaTime;
        bool canMove = CanMoveTo(moveDir);

        transform.forward = Vector3.Slerp(transform.forward, moveDir, Time.deltaTime * rotateSpeed);
        
        if(!canMove) {
            Vector3 moveX = new Vector3(moveDir.x, 0, 0).normalized;
            canMove = CanMoveTo(moveX);

            if(canMove) {
                moveDir = moveX;
            } else {
                Vector3 moveZ = new Vector3(0, 0, moveDir.z).normalized;
                canMove = CanMoveTo(moveZ);

                if(canMove) {
                    moveDir = moveZ;
                }
            }
        }

        if (canMove) {
            transform.position += moveDir * moveDistance;
        }

        isWalking = moveDir != Vector3.zero; 

    }

    // Interaction method
    private void HandleInteractions() {
        Vector3 moveDir = GetMoveDirection();

        if (moveDir != Vector3.zero) {
            lastDirection = moveDir;
        }

        float interactDistance = 2f;
        if(Physics.Raycast(transform.position, lastDirection, out RaycastHit raycastHit, interactDistance, layerMask)) {
            if(raycastHit.transform.TryGetComponent(out BaseCounter baseCounter)) {
                if(baseCounter != selectedCounter) {
                    SetSelectedCounter(baseCounter);
                }
            } else SetSelectedCounter(null);
        } else SetSelectedCounter(null);
    }

    private Vector3 GetMoveDirection() {
        Vector2 inputVector = gameInput.GetMovementVectorNormalized();
        
        return new Vector3(inputVector.x, 0, inputVector.y);
    }

    // TODO refactor not to Invoke every Update
    private void SetSelectedCounter(BaseCounter selectedCounter) {
        this.selectedCounter = selectedCounter;

        OnSelectedCounterChanged?.Invoke(this, new OnSelectedCounterChangedEventArgs {
            selectedCounter = selectedCounter // OnSelected...counter = Player.counter
        });
    }

    public Transform GetFollowPoint() { return kitchenObjectHoldPoint; }

    public void SetKitcherObject(KitchenObject kitchenObject) {
        OnPlayerGrab?.Invoke(this, EventArgs.Empty);
        this.kitchenObject = kitchenObject;
    }

    public KitchenObject GetKitchenObject() { return kitchenObject; }

    public void ClearKitchenObject() { kitchenObject = null; }

    public bool HasKitchenObject() { return kitchenObject != null; }
}
