using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Midir
{
    public class CameraHandler : MonoBehaviour
    {
        private InputHandler inputHandler;
        private PlayerManager playerManager;

        public Transform targetTransform, cameraTransform, cameraPivotTransform, nearestLockOnTarget, currentLockOnTarget;
        public Transform leftLockTarget, rightLockTarget;
        private Transform myTransform;
        private Vector3 cameraTransformPosition, cameraFollowVelocity = Vector3.zero;
        public LayerMask ignoreLayers, enviromentLayer;

        public static CameraHandler singleton;

        public float lookSpeed = 0.1f, followSpeed = 0.1f, pivotSpeed = 0.03f;

        private float targetPosition, defaultPosition, lookAngle, pivotAngle;
        public float minimumPivot = -35, maximumPivot = 35, cameraSphereRadius = 0.2f, cameraCollisionOffSet = 0.2f, minimumCollsionOffSet = 0.2f;
        public float maximumLockOnDistance = 30, lockedPivotPosition = 2.25f, unlockedPivotPosition = 1.65f;

        List<CharacterManager> availableTargets = new List<CharacterManager>();

        private void Awake()
        {
            singleton = this;
            myTransform = transform;
            defaultPosition = cameraTransform.localPosition.z;
            ignoreLayers = ~(1 << 9 | 1 << 10);
            targetTransform = FindObjectOfType<PlayerManager>().transform;
            inputHandler = FindObjectOfType<InputHandler>();
            playerManager = FindObjectOfType<PlayerManager>();
        }

        private void Start()
        {
            enviromentLayer = LayerMask.NameToLayer("Environment");
        }

        private void Update()
        {
            if (currentLockOnTarget == null)
            {
                inputHandler.lockOnFlag = false;
                ClearLockOnTargets();
            }
                
        }

        public void FollowTarget(float delta)
        {
            Vector3 targetPosition = Vector3.SmoothDamp(myTransform.position, targetTransform.position, ref cameraFollowVelocity, delta / followSpeed);
            myTransform.position = targetPosition;

            HandleCameraCollision(delta);
        }

        public void HandleCameraRotation(float delta, float mouseXInput, float mouseYInput)
        {
            if (inputHandler.lockOnFlag == false && currentLockOnTarget == null)
            {
                lookAngle += (mouseXInput * lookSpeed) / delta;
                pivotAngle -= (mouseYInput * pivotSpeed) / delta;
                pivotAngle = Mathf.Clamp(pivotAngle, minimumPivot, maximumPivot);

                Vector3 rotation = Vector3.zero;
                rotation.y = lookAngle;
                Quaternion targetRotation = Quaternion.Euler(rotation);
                myTransform.rotation = targetRotation;

                rotation = Vector3.zero;
                rotation.x = pivotAngle;

                targetRotation = Quaternion.Euler(rotation);
                cameraPivotTransform.localRotation = targetRotation;
            }
            else
            {
                Vector3 dir = currentLockOnTarget.position - transform.position;
                dir.Normalize();
                dir.y = 0;

                Quaternion targetRotation = Quaternion.LookRotation(dir);
                transform.rotation = targetRotation;

                dir = currentLockOnTarget.position - cameraPivotTransform.position;
                dir.Normalize();

                targetRotation = Quaternion.LookRotation(dir);
                Vector3 eulerAngles = targetRotation.eulerAngles;
                eulerAngles.y = 0;
                cameraPivotTransform.localEulerAngles = eulerAngles;
            }
        }

        private void HandleCameraCollision(float delta)
        {
            targetPosition = defaultPosition;
            Vector3 direction = cameraTransform.position - cameraPivotTransform.position;
            direction.Normalize();

            if (Physics.SphereCast(cameraPivotTransform.position, cameraSphereRadius, direction, out RaycastHit hit, Mathf.Abs(targetPosition), ignoreLayers))
            {
                if (!hit.collider.CompareTag("Player"))
                {
                    float dis = Vector3.Distance(cameraPivotTransform.position, hit.point);
                    targetPosition = -(dis - cameraCollisionOffSet);
                }
            }

            if (Mathf.Abs(targetPosition) < minimumCollsionOffSet)
            {
                targetPosition = -minimumCollsionOffSet;
            }

            cameraTransformPosition.z = Mathf.Lerp(cameraTransform.localPosition.z, targetPosition, delta / 0.2f);
            cameraTransform.localPosition = cameraTransformPosition;
        }

        public void HandleLockOn()
        {
            float shortestDistance = Mathf.Infinity;
            float shortestDistanceOfLeftTarget = Mathf.Infinity;
            float shortestDistanceOfRightTarget = Mathf.Infinity;

            Collider[] colliders = Physics.OverlapSphere(targetTransform.position, 26);

            for (int i = 0; i < colliders.Length; i++)
            {
                CharacterManager character = colliders[i].GetComponent<CharacterManager>();

                if (character != null)
                {
                    Vector3 lockTargetDirection = character.transform.position - targetTransform.position;
                    float distanceFromTarget = Vector3.Distance(targetTransform.position, character.transform.position);
                    float viewableAngle = Vector3.Angle(lockTargetDirection, cameraTransform.forward);

                    if (character.transform.root != targetTransform.transform.root && viewableAngle > -50 && viewableAngle < 50 && distanceFromTarget <= maximumLockOnDistance)
                    {
                        if (Physics.Linecast(playerManager.lockOnTransform.position, character.lockOnTransform.position, out RaycastHit hit))
                        {
                            Debug.DrawLine(playerManager.lockOnTransform.position, character.lockOnTransform.position);

                            if (hit.transform.gameObject.layer != enviromentLayer)
                            {
                                availableTargets.Add(character);
                            }
                        }
                    }
                }
            }

            for (int j = 0; j < availableTargets.Count; j++)
            {
                float distanceFromTarget = Vector3.Distance(targetTransform.position, availableTargets[j].transform.position);

                if (distanceFromTarget < shortestDistance)
                {
                    shortestDistance = distanceFromTarget;
                    nearestLockOnTarget = availableTargets[j].lockOnTransform;
                }

                if (inputHandler.lockOnFlag)
                {
                    Vector3 relativeEnemyPosition = currentLockOnTarget.InverseTransformPoint(availableTargets[j].transform.position);
                    float distanceFromLeftTarget = currentLockOnTarget.transform.position.x - availableTargets[j].transform.position.x;
                    float distanceFromRightTarget = currentLockOnTarget.transform.position.x + availableTargets[j].transform.position.x;

                    if (relativeEnemyPosition.x > 0.00 && distanceFromLeftTarget < shortestDistanceOfLeftTarget)
                    {
                        shortestDistanceOfLeftTarget = distanceFromLeftTarget;
                        leftLockTarget = availableTargets[j].lockOnTransform;
                    }

                    if (relativeEnemyPosition.x < 0.00 && distanceFromRightTarget < shortestDistanceOfRightTarget)
                    {
                        shortestDistanceOfRightTarget = distanceFromRightTarget;
                        rightLockTarget = availableTargets[j].lockOnTransform;
                    }
                }
            }
        }

        public void ClearLockOnTargets()
        {
            availableTargets.Clear();
            nearestLockOnTarget = null;
            currentLockOnTarget = null;
        }

        public void SetCameraHeight()
        {
            Vector3 velocity = Vector3.zero;

            Vector3 newUnlockedPosition = new Vector3(0, unlockedPivotPosition, -1);

            cameraPivotTransform.transform.localPosition = Vector3.SmoothDamp(cameraPivotTransform.transform.localPosition, newUnlockedPosition, ref velocity, Time.deltaTime);
        }
    }
}