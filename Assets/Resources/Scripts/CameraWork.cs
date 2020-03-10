using System.Collections;
using UnityEngine;


namespace Com.LunacyIncorporated.Landslaught
{
    /// <summary>
    /// Camera work, follows a target
    /// </summary>
    public class CameraWork : MonoBehaviour
    {
        #region Private Fields


        [Tooltip("The distance in the local x-z plane to the target")]
        [SerializeField]
        private float distance = 3.0f;

        [Tooltip("The height we want the camera to be above the target")]
        [SerializeField]
        private float height = 2.0f;

        [Tooltip("The smooth time lag for the height of the camera")]
        [SerializeField]
        private float heightSmoothingLag = 0.3f;

        [Tooltip("Allow the camera to be offset vertically from the target, seeing more ground etc")]
        [SerializeField]
        private Vector3 centerOffset = Vector3.zero;

        [Tooltip("Set this as false if a component being instantiated by Photon Network, and manually call OnStartFollowing() when needed")]
        [SerializeField]
        private bool followOnStart = false;

        //Cached transform of the target
        Transform cameraTransform;

        // Maintain a flag internally to reconnect if the target is lost or camera switched
        bool isFollowing;

        // Represents the current velocity
        private float heightVelocity;

        // Represents the position to reach using SmoothDamp()
        private float targetHeight = 100000.0f;


        #endregion


        #region MonoBehavior Callbacks

        void Start()
        {
            if (followOnStart)
            {
                OnStartFollowing();
            }
        }

        /// <summary>
        /// MonoBehavior function called after all update functions have been called
        /// </summary>
        void LateUpdate()
        {
            // The transform target may not destory on level load,
            // So we need to cover edge cases where the main camera is different everytime we load a scene, and reconnect when that happens
            if (cameraTransform == null && isFollowing)
            {
                OnStartFollowing();
            }

            // Only follow is explicitly declared
            if (isFollowing)
            {
                Apply();
            }
        }
        #endregion

        #region Public Methods


        public void OnStartFollowing()
        {
            cameraTransform = Camera.main.transform;
            isFollowing = true;
            // We dont smooth anything, go straight to the right camera shot
            Cut();
        }
        #endregion

        #region Private Methods


        void Cut()
        {
            float oldHeightSmoothing = heightSmoothingLag;
            heightSmoothingLag = 0.001f;
            Apply();
            heightSmoothingLag = oldHeightSmoothing;
        }


        void Apply()
        {
            Vector3 targetCenter = transform.position + centerOffset;
            //Calculate the current and target rotation angles
            float originalTargetAngle = transform.eulerAngles.y;
            float currentAngle = cameraTransform.eulerAngles.y;
            // Adjust the real target angle when the camera is locked
            float targetAngle = originalTargetAngle;
            currentAngle = targetAngle;
            targetHeight = targetCenter.y + height;


            // Damp the height
            float currentHeight = cameraTransform.position.y;
            currentHeight = Mathf.SmoothDamp(currentHeight, targetHeight, ref heightVelocity, heightSmoothingLag);
            // Convert the angle into a rotation, by which we reposition the camera
            Quaternion currentRotation = Quaternion.Euler(0, currentAngle, 0);
            // Set the position of the x-z plane to:
            // distance meters behind the target
            cameraTransform.position = targetCenter;
            cameraTransform.position += currentRotation * Vector3.back * distance;
            // Set the height of the camera
            cameraTransform.position = new Vector3(cameraTransform.position.x, currentHeight, cameraTransform.position.z);
            //Always look at the target
            SetUpRotation(targetCenter);

        }

        void SetUpRotation(Vector3 centerPos)
        {
            Vector3 cameraPos = cameraTransform.position;
            Vector3 offsetToCenter = centerPos - cameraPos;
            // Generate base rotation only around y-axis
            Quaternion yRotation = Quaternion.LookRotation(new Vector3(offsetToCenter.x, 0, offsetToCenter.z));
            Vector3 relativeOffset = Vector3.forward * distance + Vector3.down * height;
            cameraTransform.rotation = yRotation * Quaternion.LookRotation(relativeOffset);
        }
        #endregion
    }
}
