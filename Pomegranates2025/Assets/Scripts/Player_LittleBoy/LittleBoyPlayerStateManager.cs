using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.UI;

public class LittleBoyPlayerStateManager : MonoBehaviour
{
    // Base State + Chil State Reference
    // For Player we have: IDLE + MOVEMENT + JUMPING + ACTION-TALKING + ACTION-WATER
    [Header("Player Settings")]
    [SerializeField] private LittleBoyPlayerBody playerBody;

    [Header("Bucket Settings")]
    [SerializeField] private GameObject bucketWater;
    [SerializeField] private Animator bucketAnimator;
    public GameObject fillCanvas;
    public Image progressBar;

    [Header("Level / State Settings")]
    [SerializeField] private GameObject doorway;
    [SerializeField] private GameObject dreadObject;
    [SerializeField] private Volume ppv;

    [Header("Cutscene Manager")]
    public CutsceneManager cutsceneManager;

    // Change this to bucket and no bucket
    LittleBoyPlayerBaseState currentState;
    public LittleBoyPlayerAdrenalineState adrenalineState = new LittleBoyPlayerAdrenalineState();
    public LittleBoyPlayerDreadState dreadState = new LittleBoyPlayerDreadState();

    // State Management Flow
    // Adrenaline -> Bucket Drain
    // If bucket drain is interrupted before done -> Adrenaline
    // If bucket drain is finished -> dread

    private Animator animator;
    private bool rayHit;

    /*
    public PlayerInsideState insideState = new PlayerInsideState();
    public PlayerOutsideState outsideState = new PlayerOutsideState();
    */

    // TODO: Replace the above states with new ones! We should be looking at the following
    // 1) Player Enter: The little boy, with his full bucket of water, starts off with a lot of adrenaline. FOV and decaying move speed are present
    // 2) Player Dread: The little boy, now with a present vinette we can use the unity URP post process volume, is a lot slower. Dread creeps in.
    // 3) Player Exit: The little boy dumps his water with a long interaction -- afterwards he is fast and light again but again this is decaying and we go back to 2) as he approaches the entrance



    // TODO: LOOK INTO CYCLE PARAMETER, NOT FOR RIGHT NOW
    // if setWaterPumpingInteract == true && filledWater == false

    // have a float that over fillAmount = time.deltatime * waterfillfrequency fills up from 0 -> 100
    // during water fill have animation for bucket fill progressively. Fill amount animation = fillAmount (speed of animation is tied to fillAmount)
    // When water at max, stop water fill animation and set filledWater (new bool for player) to true   

    private bool waterBucketDrained;

    void Awake()
    {
        animator = GetComponent<Animator>();
        waterBucketDrained = false;
        rayHit = false;
    }

    void Start()
    {
        currentState = adrenalineState;
        adrenalineState.EnterState(this);
    }

    void Update()
    {
        currentState.UpdateState(this);
    }

    // Switch States
    public void SwitchState(LittleBoyPlayerBaseState state)
    {
        currentState = state;
        state.EnterState(this);
    }

    // Dread State
    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("PlayerStateTrigger"))
        {
            SwitchState(dreadState);
            Destroy(other.gameObject);
        }

        if (other.gameObject.CompareTag("CutsceneAdvanceTrigger"))
        {
            cutsceneManager.PlayNext();
            Destroy(other.gameObject);
        }
    }


    public Animator GetBucketAnimator()
    {
        return bucketAnimator;
    }
    public GameObject GetBucketWater()
    {
        return bucketWater;
    }

    public bool GetBucketState()
    {
        return waterBucketDrained;
    }

    public void SetBucketState(bool drain)
    {
        waterBucketDrained = drain;

        if (waterBucketDrained == true)
        {
            doorway.SetActive(false);
        }
    }


    public bool GetRayHit()
    {
        return rayHit;
    }

    public void SetRayHit(bool hit)
    {
        rayHit = hit;
    }

    public LittleBoyPlayerBody GetPlayerBody()
    {
        return playerBody;
    }

    public GameObject GetDreadObject()
    {
        return dreadObject;
    }

    public Volume GetPPV()
    {
        return ppv;
    }

}

