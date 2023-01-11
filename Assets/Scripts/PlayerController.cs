using System.Threading.Tasks;
using System.Runtime.CompilerServices;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.InteropServices.WindowsRuntime;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEditor;
using NPCDependencies;
using TaskSystem;

public class PlayerController : MonoBehaviour, IDialogueReceiver, ITaskReceiver
{
    public GameObject cameraHolder;
    public CharacterController characterController;
    [HideInInspector] public InGameUI ui;
    [SerializeField] Transform groundCheck;
    [SerializeField] LayerMask groundMask;
    [Space, Header("Player Statistics")]
    [SerializeField] protected float walkSpeed = 2.85f;
    [SerializeField] protected float sprintSpeed = 5.85f;
    [SerializeField] protected float crouchSpeed = 1.95f;
    [SerializeField] protected float jumpForce = 1f;
    [SerializeField] protected float mouseSensitivity = 100f;
    //[ReadOnly(true)] public List<Dialogue> queuedDialogues = new();
    // Start is called before the first frame update
    void Awake()
    {
        PlayerAttributes.player = this;
        PlayerInteraction.player = this;
        PlayerTasks.player = this;
        PlayerTasks.player = this;
        PlayerStates.player = this;
        PlayerMovement.player = this;
        PlayerCamera.player = this;
        PlayerMovement.SetNoclip(false);
        ui = FindObjectOfType<InGameUI>();
        InGameUI.DialogueUI.SetUIActive(false);
    }
    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
    }
    // Update is called once per frame
    void Update()
    {
        PlayerStates.UpdateStates();
        PlayerMovement.UpdateGenericMovement();
        PlayerMovement.UpdateMovement();
        PlayerCamera.UpdateComponent();
        PlayerInteraction.UpdateLogic();
        PlayerTasks.UpdateComponent();

        if (Input.GetKeyDown(KeyCode.RightAlt))
        {
            if (PlayerMovement.noclip) PlayerMovement.SetNoclip(false);
            else PlayerMovement.SetNoclip(true);
        }
    }
    public void ResetPlayerAttributes()
    {
        PlayerAttributes.SetWalkSpeed(2.85f);
        PlayerAttributes.SetSprintSpeed(5.85f);
        PlayerAttributes.SetCrouchSpeed(1.95f);
        PlayerAttributes.SetJumpForce(1f);
        PlayerAttributes.SetMouseSensitivity(100f);
    }
    public static class PlayerAttributes
    {
        public static PlayerController player;
        public static float g_walkSpeed { get { return player.walkSpeed; } }
        public static float g_sprintSpeed { get { return player.sprintSpeed; } }
        public static float g_crouchSpeed { get { return player.crouchSpeed; } }
        public static float g_jumpForce { get { return player.jumpForce; } }
        public static float g_mouseSensitivity { get { return player.mouseSensitivity; } }
        public static float g_gravity = -9.81f, g_groundDist = 0.5f;
        public static void SetWalkSpeed(float amount) { player.walkSpeed = amount; }
        public static void SetSprintSpeed(float amount) { player.sprintSpeed = amount; }
        public static void SetCrouchSpeed(float amount) { player.crouchSpeed = amount; }
        public static void SetJumpForce(float amount) { player.jumpForce = amount; }
        public static void SetMouseSensitivity(float amount) { player.mouseSensitivity = amount; }
    }
    public static class PlayerStates
    {
        public static PlayerController player;
        static bool enabled = true;
        public static void SetComponentActive(bool boolean)
        {
            enabled = boolean;
        }
        public static bool isWalking = false, isSprinting = false, isJumping = false, isCrouching = false, onGround = false;
        public static void UpdateStates()
        {
            if (!enabled) return;
            PlayerStates.onGround = Physics.CheckSphere(player.groundCheck.position, PlayerAttributes.g_groundDist, player.groundMask);
            if (Mathf.Abs(Input.GetAxis("Horizontal")) > 0f || Mathf.Abs(Input.GetAxis("Vertical")) > 0f)
            {
                isWalking = true;
            }
            else
            {
                isWalking = false;
            }
            if (Input.GetKeyDown(KeyCode.C))
            {
                if (isCrouching)
                {
                    isCrouching = false;
                }
                else
                {
                    isCrouching = true;
                }
            }
            if (isWalking && !isCrouching && Input.GetKey(KeyCode.LeftShift) && (Mathf.Abs(Input.GetAxis("Horizontal")) >= 1f || Mathf.Abs(Input.GetAxis("Vertical")) >= 1f))
            {
                isSprinting = true;
            }
            else
            {
                isSprinting = false;
            }
            if (Input.GetKeyDown(KeyCode.Space) && onGround)
            {
                isCrouching = false;
                isJumping = true;
            }
            else
            {
                isJumping = false;
            }
        }
    }
    public static class PlayerMovement
    {
        public static PlayerController player;
        static bool enabled = true;
        static bool gravityEnabled = true;
        public static void SetGravity(bool boolean)
        {
            gravityEnabled = boolean;
        }
        public static void SetComponentActive(bool boolean)
        {
            enabled = boolean;
        }
        static float speedValve
        {
            get
            {
                //Debug.Log($"isWalking: {PlayerStates.isWalking}    isSprinting: {PlayerStates.isSprinting}    isCrouching: {PlayerStates.isCrouching}");
                if (PlayerStates.isCrouching) return PlayerAttributes.g_crouchSpeed;
                if (PlayerStates.isSprinting) return PlayerAttributes.g_sprintSpeed;
                if (PlayerStates.isWalking) return PlayerAttributes.g_walkSpeed;
                return 0f;
            }
        }
        public static Vector3 MovementInput;
        public static Vector3 Velocity;
        public static bool noclip = false;
        static Vector3 t_pos, t_localPos;
        static Quaternion t_rot, t_localRot;
        public static void UpdateMovement()
        {
            float vertical = (!noclip ? Input.GetAxis("Vertical") : Input.GetAxisRaw("Vertical")), horizontal = (!noclip ? Input.GetAxis("Horizontal") : Input.GetAxisRaw("Horizontal"));
            MovementInput = player.transform.right * horizontal + player.transform.forward * vertical;
            if (noclip)
            {
                player.cameraHolder.transform.position += MovementInput * Time.deltaTime * speedValve;
            }
            if (!enabled) return;
            if (!noclip)
            {
                player.characterController.Move(MovementInput * Time.deltaTime * speedValve);
                if (PlayerStates.isJumping)
                {
                    Velocity.y = Mathf.Sqrt(PlayerAttributes.g_jumpForce * -2f * PlayerAttributes.g_gravity);
                }
            }
        }
        public static void UpdateGenericMovement()
        {
            if (!gravityEnabled) return;
            Velocity.y += PlayerAttributes.g_gravity * Time.deltaTime;
            if (PlayerStates.onGround && Velocity.y < 0) Velocity.y = -4f;
            player.characterController.Move(Velocity * Time.deltaTime);
        }
        public static void SetNoclip(bool boolean)
        {
            if (boolean)
            {
                t_pos = player.cameraHolder.transform.position;
                t_localPos = player.cameraHolder.transform.localPosition;
                t_rot = player.cameraHolder.transform.rotation;
                t_localRot = player.cameraHolder.transform.localRotation;
            }
            else
            {
                player.cameraHolder.transform.position = t_pos;
                player.cameraHolder.transform.rotation = t_rot;
                player.cameraHolder.transform.localPosition = t_localPos;
                player.cameraHolder.transform.localRotation = t_localRot;
            }
            noclip = boolean;
        }
    }
    public static class PlayerCamera
    {
        public static PlayerController player;
        static bool enabled = true;
        static float x, y;
        public static void SetComponentActive(bool boolean)
        {
            enabled = boolean;
        }
        public static void UpdateComponent()
        {
            if (!enabled) return;
            x = PlayerAttributes.g_mouseSensitivity * Input.GetAxisRaw("Mouse X") * Time.deltaTime;
            //else x += PlayerAttributes.g_mouseSensitivity * Input.GetAxisRaw("Mouse X") * Time.deltaTime;
            y += PlayerAttributes.g_mouseSensitivity * Input.GetAxisRaw("Mouse Y") * Time.deltaTime;
            y = Mathf.Clamp(y, -80, 80);
            player.cameraHolder.transform.localRotation = Quaternion.Euler(-y, !PlayerMovement.noclip ? 0f : (player.cameraHolder.transform.localRotation.y + x), !PlayerMovement.noclip ? 0f : player.cameraHolder.transform.localRotation.z);
            if (!PlayerMovement.noclip) player.transform.Rotate(Vector3.up * x);
            else player.cameraHolder.transform.Rotate(Vector3.up * x);
        }
    }
    public static class PlayerInteraction
    {
        public static PlayerController player;
        static bool enabled = true;
        public static bool inDialogue = false;
        static float isInvoked = 0;//1000 milisec = 1 sec
        static List<List<Dialogue>> dialogueBuffer = new();
        static List<Dialogue> ongoingDialogue = new();
        static Dialogue processing;
        public static List<NPCScript> NPCBuffer = new();
        public static NPCScript ongoingDialogueNPC;
        public static void SetComponentActive(bool boolean)
        {
            enabled = boolean;
        }
        public static void UpdateLogic()
        {
            if (!enabled) return;
            if (Input.GetKeyDown(KeyCode.F))
            {
                CheckNPCInteraction();
            }
            if (inDialogue)
            {
                //Debug.Log($"ongoingDialogueCount: {ongoingDialogue.Count}, dialogueBufferCount: {dialogueBuffer.Count}, isInvoked: {isInvoked}");
                isInvoked -= Time.deltaTime;
                if (isInvoked <= 0f)
                {
                    if (ongoingDialogue.Count >= 1)
                    {
                        processing = ongoingDialogue[0];
                        isInvoked = processing.waitDuration;
                        InGameUI.DialogueUI.SetUIData(processing, ongoingDialogueNPC.properties);
                        ongoingDialogue.Remove(processing);//When Timer is up, switch to next Dialogue in current List of Dialogues.
                        Debug.Log("Retreiving Dialogue from Ongoing");
                    }
                    else
                    {
                        if (dialogueBuffer.Count >= 1)
                        {
                            ongoingDialogue = dialogueBuffer[0];
                            ongoingDialogueNPC = NPCBuffer[0];
                            NPCBuffer.Remove(ongoingDialogueNPC);
                            dialogueBuffer.Remove(ongoingDialogue);
                            Debug.Log("Retreiving Dialogue from buffer");
                        }
                        else
                        {
                            ongoingDialogue.Clear();
                            NPCBuffer.Clear();
                            ongoingDialogueNPC = new();
                            InGameUI.DialogueUI.SetUIActive(false);
                            Debug.Log("Finishing off dialogue");
                            inDialogue = false;
                        }
                    }
                }
                else
                {   //Processing current Dialogue
                    OnProcessDialogueChoice(processing.events, processing.choices.Count);
                }
                //Debug.Log("In Dialogue...");
            }
        }
        static void CheckNPCInteraction()
        {
            RaycastHit hit;
            if (Physics.Raycast(player.cameraHolder.transform.position, player.cameraHolder.transform.forward, out hit, 3f))
            {
                NPCScript temp = hit.collider.GetComponent<NPCScript>();
                temp?.SendDialogue(player);
            }
        }
        public static bool QueueDialogue(List<Dialogue> dialogue)
        {
            bool success = dialogueBuffer.Contains(dialogue);
            if (!success) dialogueBuffer.Add(dialogue);
            Debug.Log("QueueDialogue()");
            return !success;
        }
        public static bool ProcessDialogue(List<Dialogue> dialogue)
        {
            bool success = true;
            ongoingDialogue = dialogue;
            if (ongoingDialogue.Equals(dialogue)) success = true;
            else success = false;
            Debug.Log($"ProcessDialogue(), ongoingDialogueCount: {ongoingDialogue.Count}");
            return success;
        }
        static void OnProcessDialogueChoice(List<UnityEvent> events, int maxNum)
        {
            if (maxNum >= 1)
            {
                for (int i = 1; i <= maxNum; i++)
                {
                    if (Input.GetKeyDown(i.ToString()))
                    {
                        InGameUI.DialogueUI.choicesCache[i - 1].Selected();
                        events[i - 1].Invoke();
                        isInvoked = 0;
                        Debug.Log($"Choosing Choice {i}");
                        player.OnRepliedDialogue();
                    }
                }
            }
            else
            {
                if (Input.GetKeyDown("1"))
                {
                    InGameUI.DialogueUI.choicesCache[0].Selected();
                    Debug.Log($"Choosing Choice 1");
                    isInvoked = 0;
                    player.OnRepliedDialogue();
                }
            }
            //Debug.Log("OnProcessDialogueChoice() is Running.");
        }
    }
    public static class PlayerTasks
    {
        public static PlayerController player;
        static bool enabled = true;
        static List<TaskInfo> taskInfos;
        public static void SetComponentActive(bool boolean)
        {
            enabled = boolean;
        }
        public static void UpdateComponent()
        {
            if (!enabled) return;
            if (Input.GetKeyDown(KeyCode.F))
            {
                CheckInteraction();
            }
            //if()
        }
        static void CheckAccomplishment(TaskInfo info){
            if(info.subtasks.Count >= 1){
                foreach(SubtaskInfo tp in info.subtasks){
                    if(tp.current >= tp.limit){
                        
                    }
                }
            }
        }
        static void CheckInteraction(){
            RaycastHit hit;
            if (Physics.Raycast(player.cameraHolder.transform.position, player.cameraHolder.transform.forward, out hit, 3f))
            {
                ITaskableObject temp = hit.collider.GetComponent<ITaskableObject>();
                temp?.ConsumeObject(player);
            }
        }
    }
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(groundCheck.position, PlayerAttributes.g_groundDist);
    }

    #region Interface Ports
    public void OnReceivedDialogue()
    {
        //throw new NotImplementedException();
    }

    public void OnRepliedDialogue()
    {
        ////throw new NotImplementedException();
    }

    public void ReceiveDialogue(Dialogue dialogue, bool invokeDialogue = true, NPCScript npc = null)
    {
        List<Dialogue> ne = new();
        ne.Add(dialogue);
        ReceiveDialogue(ne, npc);
        ////throw new NotImplementedException();
    }

    public void ReceiveDialogue(List<Dialogue> dialogues, bool invokeDialogue = true, NPCScript npc = null)
    {
        Debug.Log("Received First Hand Dialogue");
        if (PlayerInteraction.NPCBuffer.Contains(npc) || (PlayerInteraction.ongoingDialogueNPC == npc && PlayerInteraction.ongoingDialogueNPC != null)) return;
        InGameUI.DialogueUI.SetUIActive(true);
        //queuedDialogues.AddRange(dialogues);
        if (invokeDialogue)
        {
            InvokeDialogue(dialogues, npc);
        }
        ////throw new NotImplementedException();
    }

    public void InvokeDialogue(List<Dialogue> dialogues, NPCScript npc = null)
    {
        OnReceivedDialogue();
        if (PlayerInteraction.inDialogue)
        {
            PlayerInteraction.QueueDialogue(dialogues);
            PlayerInteraction.NPCBuffer.Add(npc);
        }
        else
        {
            PlayerInteraction.ProcessDialogue(dialogues);
            PlayerInteraction.ongoingDialogueNPC = npc;
            PlayerInteraction.inDialogue = true;
        }
        //throw new NotImplementedException();
    }

    public void OnReceiveTask()
    {

    }

    public void OnFinishTask()
    {

    }
    public void ReceiveTask()
    {

    }
    #endregion
}

[CustomEditor(typeof(PlayerController))]
public class PlayerControllerEditor : Editor
{
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();
        PlayerController player = (PlayerController)target;
        if (GUILayout.Button("Reset Player Attributes"))
        {
            player.ResetPlayerAttributes();
        }
    }
}
