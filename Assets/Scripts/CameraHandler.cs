using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Midir
{
    public class CameraHandler : MonoBehaviour
    {
        public Transform targetTransform, cameraTransform, cameraPivotTransform;
        private Transform myTransform;
        private Vector3 cameraTransformPosition, cameraFollowVelocity = Vector3.zero;
        public LayerMask ignoreLayers;

        public static CameraHandler singleton;

        public float lookSpeed = 0.1f, followSpeed = 0.1f, pivotSpeed = 0.03f;

        private float targetPosition, defaultPosition, lookAngle, pivotAngle;
        public float minimumPivot = -35, maximumPivot = 35, cameraSphereRadius = 0.2f, cameraCollisionOffSet = 0.2f, minimumCollsionOffSet = 0.2f;

        private void Awake()
        {
            singleton = this;
            myTransform = transform;
            defaultPosition = cameraTransform.localPosition.z;
            ignoreLayers = ~(1 << 8 | 1 << 9 | 1 << 10);
            targetTransform = FindObjectOfType<PlayerManager>().transform;
        }

        public void FollowTarget(float delta)
        {
            Vector3 targetPosition = Vector3.SmoothDamp(myTransform.position, targetTransform.position, ref cameraFollowVelocity, delta / followSpeed);
            myTransform.position = targetPosition;

            HandleCameraCollision(delta);
        }

        public void HandleCameraRotation(float delta, float mouseXInput, float mouseYInput)
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

        private void HandleCameraCollision(float delta)
        {
            targetPosition = defaultPosition;
            RaycastHit hit;
            Vector3 direction = cameraTransform.position - cameraPivotTransform.position;
            direction.Normalize();

            if (Physics.SphereCast(cameraPivotTransform.position, cameraSphereRadius, direction, out hit, Mathf.Abs(targetPosition), ignoreLayers))
            {
                float dis = Vector3.Distance(cameraPivotTransform.position, hit.point);
                targetPosition = -(dis - cameraCollisionOffSet);
            }

            if (Mathf.Abs(targetPosition) < minimumCollsionOffSet)
            {
                targetPosition = -minimumCollsionOffSet;
            }

            cameraTransformPosition.z = Mathf.Lerp(cameraTransform.localPosition.z, targetPosition, delta / 0.2f);
            cameraTransform.localPosition = cameraTransformPosition;
        }
    }
}