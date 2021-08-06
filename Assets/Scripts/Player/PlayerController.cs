using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Default
{
    public class PlayerController : MonoBehaviour, EntityStatsObserver
    {
        [Space(20)]
        public float mouseSensitivityX = 2f;
        public float mouseSensitivityY = 2f;
        [Space(10)]
        public float speedNormal = 5f;
        public float speedSneaking = 2f;
        public float speedSprinting = 8f;
        public float jumpForce = 1f;
        float speedCurrent = 0f;
        public float gravity = 10f;
        public float airControl = 1f;
        Vector3 moveDirection = Vector3.zero;
        [Space(10)]
        public float heightNormal = 1.8f;
        public float heightSneaking = 1.4f;
        public float camOffsetHeight = 0.2f;
        public float camOffsetX = 0f;
        public float camOffsetY = 0f;
        public float fovNormal = 60f;
        public float fovSprinting = 80f;
        [Space(10), SerializeField]
        private bool isFrozen = false;
        [HideInInspector] public bool canControl = true;
        public bool isSneaking = false;
        public bool isSprinting = false;
        [Space(10)]
        public Animator crossAnimator;
        public Animator bloodyScreen;
        [Space(10)]
        public AudioSource fxSource;
        public AudioClip hitFx;
        public AudioClip noFx;

        public CharacterController charController;
        public PlayerStats stats;
        public Transform eyeHeightTransform;
        public Camera cam;

        public Transform itemTransform;
        private List<ItemHoldable> items = new List<ItemHoldable>();
        private ItemHoldable currentItem = null;

        private LayerMask myLayerMask;

        void Awake()
        {
            myLayerMask = 1 << gameObject.layer;
        }

        void Start()
        {
            if (stats != null)
                stats.entityStatsObservers.Add(this);

            eyeHeightTransform.localPosition = new Vector3(0f, heightNormal - camOffsetHeight, 0f);

            speedCurrent = speedNormal;
        }

        void Update()
        {
            //Apply Camera Effects
            CameraEffects();

            //Do nothing when Player isn't allowed to move
            if (isFrozen || PauseManager.isPaused().Value)
                return;

            MoveData inputs;
            if (canControl)
            {
                //Get Inputs
                inputs = new MoveData()
                {
                    xRot = Input.GetAxis("Mouse Y") * mouseSensitivityY,
                    yRot = Input.GetAxis("Mouse X") * mouseSensitivityX,
                    axisHorizontal = InputSettings.usingMouse ? Input.GetAxisRaw("Horizontal") : Input.GetAxis("Horizontal"),
                    axisVertical = InputSettings.usingMouse ? Input.GetAxisRaw("Vertical") : Input.GetAxis("Vertical"),
                    axisSneak = Input.GetAxisRaw("Sneak"),
                    axisSprint = Input.GetAxisRaw("Sprint"),
                    axisJump = Input.GetAxisRaw("Jump"),
                    axisPrimary = Input.GetAxisRaw("Primary"),
                    axisSecondary = Input.GetAxisRaw("Secondary")
                };
            }
            else
            {
                inputs = new MoveData();
            }

            if (currentItem != null)
                currentItem.UseItem(inputs);

            Rotate(inputs.xRot, inputs.yRot);
            Move(inputs);
        }

        void Rotate(float xRot, float yRot)
        {
            Quaternion characterTargetRot = transform.localRotation;
            Quaternion cameraTargetRot = eyeHeightTransform.localRotation;

            characterTargetRot *= Quaternion.Euler(0f, yRot, 0f);
            cameraTargetRot *= Quaternion.Euler(-xRot, 0f, 0f);

            cameraTargetRot = ClampRotationAroundXAxis(cameraTargetRot);

            transform.localRotation = characterTargetRot;
            eyeHeightTransform.localRotation = cameraTargetRot;
        }

        void Move(MoveData inputs)
        {
            //Changes camera height and speed while sneaking
            if (inputs.axisSneak > 0)
            {
                if (!isSneaking)
                    StartCoroutine(Sneak(true));

                if (isSprinting)
                    StartCoroutine(Sprint(false));
            }
            else
            {
                if (isSneaking)
                    StartCoroutine(Sneak(false));

                if (!isSneaking)
                {
                    //Changes camera FOV and speed while sprinting
                    if (inputs.axisSprint > 0 && inputs.axisVertical > 0)
                    {
                        if (!isSprinting)
                            StartCoroutine(Sprint(true));
                    }
                    else
                    {
                        if (isSprinting)
                            StartCoroutine(Sprint(false));
                    }
                }
            }
            speedCurrent = isSneaking ? speedSneaking : isSprinting ? speedSprinting : speedNormal;

            //Normalize input and add speed
            Vector2 input = new Vector2(inputs.axisHorizontal, inputs.axisVertical);
            input.Normalize();
            input *= speedCurrent;

            //Jump and Gravity
            if (charController.isGrounded)
            {
                moveDirection = transform.forward * input.y + transform.right * input.x;
                if (inputs.axisJump > 0)
                    moveDirection.y = jumpForce;
            }
            else
            {
                input *= airControl;
                moveDirection = transform.forward * input.y + transform.right * input.x + transform.up * moveDirection.y;
            }

            moveDirection.y -= gravity * (Time.deltaTime / 2);

            Vector3 oldPos = transform.position;
            charController.Move(moveDirection * Time.deltaTime);
            Vector3 newPos = transform.position;

            moveDirection.y -= gravity * (Time.deltaTime / 2);
        }

        void CameraEffects()
        {
            if (camOffsetX != 0f || camOffsetY != 0f)
                eyeHeightTransform.localPosition = new Vector3(camOffsetX, camOffsetY, 0f);
        }

        IEnumerator Sneak(bool willSneak)
        {
            //First, check if he can unsneak
            if (!willSneak)
            {
                if (Physics.OverlapCapsule(transform.position + charController.radius * Vector3.up,
                        transform.position + (heightNormal - charController.radius) * Vector3.up,
                        charController.radius * 0.99f).Length > 1)
                    yield break;
            }

            isSneaking = willSneak;
            charController.height = willSneak ? heightSneaking : heightNormal;
            charController.center = Vector3.up * (charController.height / 2f);

            Vector3 oldCamPos = eyeHeightTransform.localPosition;
            float newHeight = willSneak ? heightSneaking - camOffsetHeight : heightNormal - camOffsetHeight;

            for (float i = 0; i < 1 && (isSneaking == willSneak); i += 0.2f)
            {
                eyeHeightTransform.localPosition = Vector3.Lerp(oldCamPos, new Vector3(0f, newHeight, 0f), i);
                yield return new WaitForSeconds(1f / 60f);
            }
            if (isSneaking == willSneak)
                eyeHeightTransform.localPosition = new Vector3(0f, newHeight, 0f);
        }

        IEnumerator Sprint(bool willSprint)
        {
            isSprinting = willSprint;

            float oldFov = willSprint ? fovNormal : fovSprinting;
            float newFov = willSprint ? fovSprinting : fovNormal;

            for (float i = 0; i < 1 && (isSprinting == willSprint); i = i + 0.2f)
            {
                cam.fieldOfView = Mathf.Lerp(oldFov, newFov, i);
                yield return new WaitForSeconds(1f / 60f);
            }
            if (isSprinting == willSprint)
                cam.fieldOfView = newFov;
        }

        public bool CanMove() { return !isFrozen; }

        public void SetCanMove(bool canMove)
        {
            isFrozen = !canMove;
            charController.detectCollisions = canMove;
            crossAnimator.SetBool("Activated", canMove);

            if (currentItem != null)
            {
                if (canMove)
                    currentItem.OnEquip();
                else
                    currentItem.OnUnequip();
            }
        }

        public void TeleportPlayer(Transform newPosition, bool cameraPerspective = false, Vector3 offset = new Vector3())
        {
            if (isSprinting)
                StartCoroutine(Sprint(false));
            float heightOffset = 0f;
            if (isSneaking && cameraPerspective)
            {
                isSneaking = false;
                charController.height = heightNormal;
                heightOffset = heightNormal - camOffsetHeight - eyeHeightTransform.localPosition.y;
                eyeHeightTransform.localPosition = new Vector3(0f, heightNormal - camOffsetHeight, 0f);
            }

            Vector3 positionNew = newPosition.position + offset;
            if (cameraPerspective)
                positionNew -= (isSneaking ? heightSneaking - camOffsetHeight : heightNormal - camOffsetHeight) * Vector3.up;

            bool oldCCState = charController.enabled;
            charController.enabled = false;

            transform.position = positionNew;
            transform.rotation = Quaternion.Euler(0f, newPosition.rotation.eulerAngles.y, 0f);
            eyeHeightTransform.localRotation = Quaternion.Euler(newPosition.rotation.eulerAngles.x, 0f, 0f);

            charController.enabled = oldCCState;
        }

        public IEnumerator MoveRotatePlayer(Transform newPosition, float seconds = 2f, bool cameraPerspective = false, Vector3 offset = new Vector3())
        {
            if (isSprinting)
                StartCoroutine(Sprint(false));
            float heightOffset = 0f;
            if (isSneaking && cameraPerspective)
            {
                isSneaking = false;
                charController.height = heightNormal;
                heightOffset = heightNormal - camOffsetHeight - eyeHeightTransform.localPosition.y;
                eyeHeightTransform.localPosition = new Vector3(0f, heightNormal - camOffsetHeight, 0f);
            }

            Vector3 positionNew = newPosition.position + offset;
            if (cameraPerspective)
                positionNew -= (isSneaking ? heightSneaking - camOffsetHeight : heightNormal - camOffsetHeight) * Vector3.up;

            var mov = StartCoroutine(MovePlayer(positionNew, seconds, cameraPerspective));
            var rot = StartCoroutine(RotatePlayer(newPosition.rotation, seconds));

            yield return mov;
            yield return rot;
        }

        public IEnumerator MovePlayer(Vector3 newPosition, float seconds = 2f, bool ignoreSneak = false)
        {
            if (isSprinting)
                StartCoroutine(Sprint(false));
            if (isSneaking && !ignoreSneak)
                StartCoroutine(Sneak(false));

            Vector3 positionOld = transform.position;

            float rate = 1f / seconds;
            float fSmooth;
            for (float f = 0f; f <= 1f; f += rate * Time.deltaTime)
            {
                fSmooth = Mathf.SmoothStep(0f, 1f, f);
                transform.position = Vector3.Lerp(positionOld, newPosition, fSmooth);

                yield return null;
            }

            transform.position = newPosition;
        }

        public IEnumerator RotatePlayer(Quaternion newRotation, float seconds = 2f)
        {
            if (isSprinting)
                StartCoroutine(Sprint(false));

            Quaternion rotationPlayerOld = transform.rotation;
            Quaternion rotationCameraOld = eyeHeightTransform.localRotation;

            Quaternion rotationPlayerNew = Quaternion.Euler(0f, newRotation.eulerAngles.y, 0f);
            Quaternion rotationCameraNew = Quaternion.Euler(newRotation.eulerAngles.x, 0f, 0f);

            float rate = 1f / seconds;
            float fSmooth;
            for (float f = 0f; f <= 1f; f += rate * Time.deltaTime)
            {
                fSmooth = Mathf.SmoothStep(0f, 1f, f);
                eyeHeightTransform.localRotation = Quaternion.Lerp(rotationCameraOld, rotationCameraNew, fSmooth);
                transform.rotation = Quaternion.Lerp(rotationPlayerOld, rotationPlayerNew, fSmooth);

                yield return null;
            }

            eyeHeightTransform.localRotation = rotationCameraNew;
            transform.rotation = rotationPlayerNew;
        }

        public IEnumerator LookAt(Vector3 lookAtPos, float seconds = 2f)
        {
            yield return RotatePlayer(Quaternion.LookRotation(lookAtPos - eyeHeightTransform.position), seconds);
        }

        public void SetRotationLerp(Vector3 a, Vector3 b, float t)
        {
            Quaternion a1 = Quaternion.Euler(0f, a.y, 0f);
            Quaternion a2 = Quaternion.Euler(a.x, 0f, 0f);
            Quaternion b1 = Quaternion.Euler(0f, b.y, 0f);
            Quaternion b2 = Quaternion.Euler(b.x, 0f, 0f);

            transform.rotation = Quaternion.Lerp(a1, b1, t);
            eyeHeightTransform.localRotation = Quaternion.Lerp(a2, b2, t);
        }

        public Vector3 GetRotation()
        {
            return new Vector3(eyeHeightTransform.localEulerAngles.x, transform.eulerAngles.y, 0f);
        }

        Quaternion ClampRotationAroundXAxis(Quaternion q)
        {
            q.x /= q.w;
            q.y /= q.w;
            q.z /= q.w;
            q.w = 1.0f;

            float angleX = 2.0f * Mathf.Rad2Deg * Mathf.Atan(q.x);
            angleX = Mathf.Clamp(angleX, -90, 90);
            q.x = Mathf.Tan(0.5f * Mathf.Deg2Rad * angleX);

            return q;
        }

        void EntityStatsObserver.ChangedHp(int value)
        {
            if (stats.hp <= 0)
            {
                //TODO: Dead
                fxSource.pitch = 0.2f;
                fxSource.PlayOneShot(hitFx);
                bloodyScreen.SetTrigger("Dead");
            }
            else if (value < 0)
            {
                fxSource.pitch = 1f;
                fxSource.PlayOneShot(hitFx);
                bloodyScreen.SetTrigger("Hit");
            }
        }

        public void ShakeMp()
        {
            stats.ShakeMp();
            fxSource.pitch = 1f;
            fxSource.PlayOneShot(noFx);
        }

        public void AddHoldableItem(ItemHoldable item, bool directlyEquip)
        {
            item.transform.parent = itemTransform;
            item.transform.localPosition = item.positionWhenHeld.position;
            item.transform.localRotation = Quaternion.Euler(item.positionWhenHeld.rotation);
            item.transform.localScale = item.positionWhenHeld.scale;

            items.Add(item);

            if (directlyEquip)
            {
                if (currentItem != null)
                    currentItem.OnUnequip();
                currentItem = item;
                currentItem.OnEquip();
            }
        }

        public void RemoveItem(ItemHoldable item)
        {
            item.OnUnequip();
            if (currentItem == item)
                currentItem = null;
            items.Remove(item);
        }
    }
}
