using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using TMPro;

public class PlayerController : MonoBehaviour
{
    [SerializeField] float rotationPower = 3f;
    [SerializeField] float walkSpeed = 1f;
    [SerializeField] float sprintSpeed = 3f;
    [SerializeField] Transform followTransform;
    [SerializeField] Animator animator;
    [SerializeField] Rigidbody rigidbody;
    [SerializeField] private TMP_Text _pickupOverlay;
    [SerializeField] private GameObject _inventoryOverlay;
    [SerializeField] private GameObject _itemsList;
    [SerializeField] private GameObject _itemDisplay;
    [SerializeField] private ItemDetails _itemDetails;

    Vector2 moveInput;
    Vector2 lookInput;
    float sprintInput;
    private bool _gamePaused;
    private Inventory InventoryInstance;

    private void Start()
    {
        InventoryInstance = new Inventory();
        Cursor.lockState = CursorLockMode.Locked;
    }

    private void Update()
    {
        OpenCloseInventory();
        if (!_gamePaused)
        {
            PickUpCheck();
        }
    }

    private void FixedUpdate()
    {
        if (!animator.GetBool("Pickup") && !_gamePaused)
        {
            Movement();
        }
    }

    private void Movement()
    {
        moveInput = new Vector2(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"));
        lookInput = new Vector2(Input.GetAxis("Mouse X"), -Input.GetAxis("Mouse Y"));
        if (InventoryInstance.CurrentWeight <= InventoryInstance.MaximumWeight)
        {
            sprintInput = Input.GetAxis("Sprint");
        }

        UpdateFollowTargetRotation();

        float speed = 0;

        speed = Mathf.Lerp(walkSpeed, sprintSpeed, sprintInput);
        Vector3 movement = (transform.forward * moveInput.y * speed) + (transform.right * moveInput.x * speed);
        rigidbody.velocity = new Vector3(movement.x, rigidbody.velocity.y, movement.z);

        animator.SetFloat("MovementSpeed", moveInput.y * (speed / walkSpeed));

        //only rotate the player when moving, allows user to look at the player when idle
        if (moveInput.magnitude > 0.01f)
        {
            //Set the player rotation based on the look transform
            transform.rotation = Quaternion.Euler(0, followTransform.eulerAngles.y, 0);
            //reset the y rotation of the look transform
            followTransform.localEulerAngles = new Vector3(followTransform.localEulerAngles.x, 0, 0);
        }
    }

    private void UpdateFollowTargetRotation()
    {
        //Update follow target rotation
        followTransform.rotation *= Quaternion.AngleAxis(lookInput.x * rotationPower, Vector3.up);
        followTransform.rotation *= Quaternion.AngleAxis(lookInput.y * rotationPower, Vector3.right);

        var angles = followTransform.localEulerAngles;
        angles.z = 0;
        var angle = angles.x;

        //Clamp the Up/Down rotation
        if (angle > 180 && angle < 340)
        {
            angles.x = 340;
        }
        else if (angle < 180 && angle > 40)
        {
            angles.x = 40;
        }
        followTransform.localEulerAngles = angles;
    }

    private void PickUpCheck()
    {
        RaycastHit hit;  
        if (Physics.Raycast(transform.position, followTransform.forward, out hit, 3f) && hit.collider.gameObject.GetComponent<Item>())
        {
            _pickupOverlay.gameObject.SetActive(true);
            _pickupOverlay.text = hit.collider.gameObject.GetComponent<Item>().Properties.ItemName + " (E to Pickup)";

            if (Input.GetKeyDown(KeyCode.E) && InventoryInstance.CurrentWeight <= InventoryInstance.MaximumWeight)
            {
                animator.SetBool("Pickup", true);
                StartCoroutine(PickUpItem(hit.collider.gameObject.GetComponent<Item>()));
            }
        }
        else
        {
            _pickupOverlay.gameObject.SetActive(false);
        }
    }

    IEnumerator PickUpItem(Item item)
    {
        if (item != null)
        {
            yield return new WaitForSeconds(1.2f);
            InventoryInstance.InventoryChanged(item, true);
            GameObject itemDisplay = Instantiate(_itemDisplay, Vector3.zero, Quaternion.identity);
            itemDisplay.transform.parent = _itemsList.transform;
            itemDisplay.GetComponent<ItemDisplay>().MyItme = item;
            itemDisplay.GetComponent<ItemDisplay>().DetailsDisplay = _itemDetails;
            if (InventoryInstance.inventory.Count < 2)
            {
                _itemDetails.UpdateItemDetailsOverlay(item.Properties.ItemName, item.Properties.Weight, item.Properties.Value, item.Properties.Description, item.Properties.Category.ToString());
            }
            Destroy(item.gameObject);
            animator.SetBool("Pickup", false);
        }
    }

    private void OpenCloseInventory()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            _gamePaused = !_gamePaused;
            _inventoryOverlay.SetActive(!_inventoryOverlay.activeSelf);
            Cursor.lockState = _gamePaused ? CursorLockMode.None : CursorLockMode.Locked;
        }
    }
}