using UnityEngine;
using UnityEngine.Playables;

public class PlayerScript : MonoBehaviour
{
    private Rigidbody rb;
    private CurrentHealth CurrentHealth;
    private RaycastHit rayHit;
    Animator anim;

    private bool isCrouched = false;
    private bool aiming = false;
    private string currentState;
    private bool isMoving = false;

    private float timeSinceLastShot;

    public InventoryManager inventoryManager;
    public PlayerData playerData;
    public GameObject playerBody;
    public Transform groundCheck;
    public Transform gunPos;
    public CapsuleCollider upperBodyCol;
    public RuntimeAnimatorController defaultAnimController;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
        CurrentHealth = GetComponent<CurrentHealth>();
        anim = GetComponentInChildren<Animator>();

        CurrentHealth.currentHealth = playerData.maxHealth;
    }

    // Update is called once per frame
    void Update()
    {
        InputManager();

        IsGrounded();

        GeneralProcceses();

        MakeGun();
    }

    private void MakeGun()
    {
        if (inventoryManager.GetSelectedItem() != null && inventoryManager.GetSelectedItem().type.ToString() == "weapon" && gunPos.childCount == 0 || 
            inventoryManager.GetSelectedItem() != null && inventoryManager.GetSelectedItem().type.ToString() == "consumable" && gunPos.childCount == 0)
        {
            var obj = Instantiate(inventoryManager.GetSelectedItem().model, gunPos.position, gunPos.rotation);
            obj.name = inventoryManager.GetSelectedItem().name;
            obj.localScale = new Vector3(0.25f, 0.25f, 0.25f);
            obj.parent = gunPos;

            anim.runtimeAnimatorController = inventoryManager.GetSelectedItem().animOverrides;
        }
        else if (inventoryManager.GetSelectedItem() != null && gunPos.GetChild(0) != null)
        { 
            if (inventoryManager.GetSelectedItem().name != gunPos.GetChild(0).name.Replace("(Clone)", ""))
            {
                Destroy(gunPos.GetChild(0).gameObject);
                anim.runtimeAnimatorController = defaultAnimController;
            }
        }
        else if (inventoryManager.GetSelectedItem() == null)
        {
            if (gunPos.childCount > 0)
            {
                Destroy(gunPos.GetChild(0).gameObject);
                anim.runtimeAnimatorController = defaultAnimController;
            }
        }
    }

    private void Move(float moveDir)
    {
        rb.velocity = new Vector3(moveDir * playerData.speed, rb.velocity.y, rayHit.normal.z);
        isMoving = true;

        if (moveDir < 0)
        {
            transform.rotation = Quaternion.Euler(rb.rotation.x, 0f, rb.rotation.z);
        } else
        {
            transform.rotation = Quaternion.Euler(rb.rotation.x, 180f, rb.rotation.z);
        }

        if (IsGrounded())
        {
            if(isMoving == true)
            {
                if (moveDir >= 1 || moveDir <= -1)
                {
                    PlayAnimation("running");
                }
                else
                {
                    if (Aim())
                    {
                        PlayAnimation("aim walking");
                    }
                    else if (Crouch())
                    {
                        PlayAnimation("crouch walking");
                    }
                    else if (!Aim() && !Crouch())
                    {
                        PlayAnimation("walking");
                    }
                }
            } else
            {
                PlayAnimation("idle breathing");
            }
        }
    }

    private void Jump()
    {
        if (IsGrounded())
        {
            rb.AddForce(new Vector3(0, 60, 0) * playerData.jumpHeight * 0.1f, ForceMode.Impulse);
        }
    }

    private bool IsGrounded()
    {
        Physics.Raycast(transform.position, -transform.up, out rayHit);

        if (Vector3.Distance(rayHit.point, groundCheck.position) <= 0.1 && rayHit.collider.gameObject.layer == 3)
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    private bool Aim()
    {
        if (inventoryManager.GetSelectedItem() != null) {
            if (inventoryManager.GetSelectedItem().isRanged == true && inventoryManager.GetSelectedItem().type.ToString() == "weapon")
            {
                if (aiming == true)
                {
                    if (isMoving == false)
                    {
                        PlayAnimation("aim idle");
                        gunPos.transform.LookAt();
                    }

                    return true;
                }
                else
                {
                    return false;
                }
            }
            else
            {
                return true;
            }
        } else
        {
            return false;
        }
    }

    private void Use()
    {
        if (inventoryManager.GetSelectedItem() != null && Aim())
        {
            if (inventoryManager.GetSelectedItem().type.ToString() == "weapon")
            {
                var gun = gunPos.GetChild(0).gameObject;
                ItemData gunData = inventoryManager.GetSelectedItem();
                
                if (timeSinceLastShot > gunData.fireRate)
                {
                    RaycastHit gunRayHit;

                    if (Physics.Raycast(gun.transform.position, gun.transform.forward, out gunRayHit, gunData.distance, LayerMask.GetMask("Enemy")) || 
                        Physics.Raycast(gun.transform.position, gun.transform.forward, out gunRayHit, gunData.distance, LayerMask.GetMask("Human")))
                    {
                        if (gunRayHit.collider.gameObject != null)
                        {
                            gunRayHit.collider.gameObject.GetComponentInParent<CurrentHealth>().currentHealth -= gunData.damage;

                            float forceDir;
                            Vector3 enemyLocationRelativeToSelf = transform.position - gunRayHit.point;
                            if (enemyLocationRelativeToSelf.x < 0)
                            {
                                forceDir = 1;
                            }
                            else
                            {
                                forceDir = -1;
                            }
                            gunRayHit.collider.gameObject.GetComponentInParent<Rigidbody>().AddForce(gunData.knockBack * 70 * forceDir, 20, 0);
                        }
                    }
                }
            }
            else if (inventoryManager.GetSelectedItem().type.ToString() == "consumable")
            {
                CurrentHealth.currentHealth += inventoryManager.GetSelectedItem().severity * (int)inventoryManager.GetSelectedItem().effect;

                var inventorySlots = inventoryManager.inventorySlots;
                InventorySlotScript slot = inventorySlots[inventoryManager.selectedSlot];
                ItemScript itemInSlot = slot.GetComponentInChildren<ItemScript>();
                if (itemInSlot != null)
                {
                    itemInSlot.count--;
                    itemInSlot.RefreshCount();
                } else
                {
                    
                }
            }
            else
            {
                Debug.LogError("item of wrong type being used!");
            }
        }
    }

    private bool Crouch()
    {
        if (isCrouched == true && IsGrounded())
        {
            if (isMoving == false)
            {
                PlayAnimation("crouch idle");
            }

            upperBodyCol.enabled = false;
            isCrouched = true;
            return true;
        } else
        {
            return false;
        }
    }

    private void PlayAnimation(string newState)
    {
        if (currentState == newState)
        {
            //isPlayingAnim = false;
            return;
        }
        anim.Play(newState);
        currentState = newState;
    }

    private void InputManager()
    {
        if (Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.A))
        {
            if (Input.GetKey(KeyCode.LeftShift) && isCrouched == false && !Aim())
            {
                if (Input.GetKey(KeyCode.A))
                {
                    Move(1);
                }

                if (Input.GetKey(KeyCode.D))
                {
                    Move(-1);
                }
            }
            else
            {
                if (Input.GetKey(KeyCode.A))
                {
                    Move(0.75f);
                }

                if (Input.GetKey(KeyCode.D))
                {
                    Move(-0.75f);
                }
            }
        }
        else
        {
            rb.velocity = Vector3.Lerp(rb.velocity, new Vector3(0, rb.velocity.y, rb.velocity.z), Time.deltaTime * 10);
            isMoving = false;
        }

        if (Input.GetKeyDown(KeyCode.Space) || Input.GetKeyDown(KeyCode.W))
        {
            Jump();
        }

        if (Input.GetKeyDown(KeyCode.Mouse0))
        {
            Use();
        }

        if (Input.GetKey(KeyCode.Mouse1))
        {
            aiming = true;
            Aim();
        }
        else
        {
            aiming = false;
            Aim();
        }

        if (Input.GetKey(KeyCode.RightShift))
        {
            isCrouched = true;
            Crouch();
        } else
        {
            isCrouched = false;
            Crouch();
        }
    }

    private void GeneralProcceses()
    {
        timeSinceLastShot += Time.deltaTime;

        Mathf.Clamp(rb.velocity.x, playerData.speed, 0);
        Mathf.Clamp(rb.velocity.y, playerData.speed, 0);
        Mathf.Clamp(transform.rotation.z, 0, 0);

        if (!IsGrounded())
        {
            if (rb.velocity.y < -0.2)
            {
                PlayAnimation("falling down");
            }
            else if (rb.velocity.y > 0.2)
            {
                PlayAnimation("falling");
            }
        }

        if (isMoving == false && IsGrounded() && !Aim() && !Crouch())
        {
            PlayAnimation("idle breathing");
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "scene swich")
        {
            var sceneManager = GameObject.FindGameObjectsWithTag("scene manager")[0].GetComponent<EnterableThings>();
            sceneManager.SwichScene(other);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.tag == "scene swich")
        {
            var sceneManager = GameObject.FindGameObjectsWithTag("scene manager")[0].GetComponent<EnterableThings>();
            sceneManager.isButtonVisible = false;
        }
    }
}
