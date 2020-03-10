using System.Collections;
using UnityEngine;

using Photon.Pun;
using Photon.Pun.Demo.PunBasics;

namespace Com.LunacyIncorporated.Landslaught
{
    public class PlayerManager : MonoBehaviourPunCallbacks, IPunObservable
    {
        #region Public Fields


        [Tooltip("The current Health of our player")]
        public float Health = 1f;

        [Tooltip("The local player instance. Use this to know if the local player is represented in the scene")]
        public static GameObject LocalPlayerInstance;


        #endregion


        #region Private Fields


        [Tooltip("The beams GameObject to control")]
        [SerializeField]
        private GameObject beams;
        [Tooltip("The beam collider itself")]
        [SerializeField]
        private Collider beamCollider;
        //True when the user is firing
        bool IsFiring;


        #endregion

        #region IPunObservable implementation

        public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
        {
            if (stream.IsWriting)
            {
                // We own this player: send the others our data
                stream.SendNext(IsFiring);
                stream.SendNext(Health);
            }
            else
            {
                // Network player, recieve data
                this.IsFiring = (bool)stream.ReceiveNext();
                this.Health = (float)stream.ReceiveNext();
            }
        }
        #endregion

        #region MonoBehavior Callback

        /// <summary>
        /// MonoBehavior method called on the gameobject by unity during early Init phase.
        /// </summary>
        void Awake()
        {
            if (photonView.IsMine)
            {
                PlayerManager.LocalPlayerInstance = this.gameObject;
            }

            // #Critical
            // We flag as dont destory on load so that the instance survives level sync giving a seamless experience when levels load
            DontDestroyOnLoad(this.gameObject);

            if (beams == null)
            {
                Debug.LogError("<Color=Red><a>Missing<a></Color> Beams Reference", this);
            }
            else
            {
                beams.SetActive(false);
            }
        }

        void Start()
        { 
            CameraWork _cameraWork = gameObject.GetComponent<CameraWork>();

            if (_cameraWork != null)
            {
                if (photonView.IsMine)
                {
                    _cameraWork.OnStartFollowing();
                }
            }
            else
            {
                Debug.LogError("<Color=Red><a>Missing</a></Color> CameraWork component on a playerPrefab", this);
            }
        }

        /// <summary>
        /// MonoBehavior method called on GameObject by Unity every frame.
        /// </summary>
        void Update()
        {
            if (photonView.IsMine)
            {
                ProcessInputs();
            }

            if (beams != null && IsFiring != beams.activeInHierarchy)
            {
                beams.SetActive(IsFiring);
            }

            if (Health <= 0f)
            {
                //We can just assume that the GameManager will be connected to the GameObject, so we are able to just access
                GameManager.Instance.LeaveRoom();
            }
        }

        /// <summary>
        /// Called when the collider "other" enters the trigger
        /// Reduce the active users health if the collider is a beam
        /// Does not deal with the fact that the user can shoot themself (If in a flip)
        /// </summary>
        /// <param name="other"></param>
        void OnTriggerEnter(Collider other)
        {
            if (!photonView.IsMine)
            {
                return;
            }

            if (!other.name.Contains("Beam"))
            {
                return;
            }

            
            if (beamCollider != other)
            {
                Health -= 0.1f;
            }
        }

        /// <summary>
        /// This method is called once per frame for every collider "other" that is touching the trigger
        /// Continue to decrease the players health while the beam touches them
        /// </summary>
        /// <param name="other"></param>
        void OnTriggerStay(Collider other)
        {
            if (!photonView.IsMine)
            {
                return;
            }

            if (!other.name.Contains("Beam"))
            {
                return;
            }

            //Slowly affect health
            Health -= 0.1f * Time.deltaTime;
        }


        #endregion

        #region Custom Methods

        void ProcessInputs()
        {
            if (Input.GetButtonDown("Fire1"))
            {
                if (!IsFiring)
                {
                    IsFiring = true;
                }
            }
            if (Input.GetButtonUp("Fire1"))
            {
                if (IsFiring)
                {
                    IsFiring = false;
                }
            }
        }
        #endregion
    }

}