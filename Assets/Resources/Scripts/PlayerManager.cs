using System.Collections;
using UnityEngine;

using Photon.Pun;

namespace Com.LunacyIncorporated.Landslaught
{
    public class PlayerManager : MonoBehaviourPunCallbacks
    {
        #region Public Fields


        [Tooltip("The current Health of our player")]
        public float Health = 1f;


        #endregion


        #region Private Fields


        [Tooltip("The beams GameObject to control")]
        [SerializeField]
        private GameObject beams;
        //True when the user is firing
        bool IsFiring;


        #endregion

        #region MonoBehavior Callback
        
        /// <summary>
        /// MonoBehavior method called on the gameobject by unity during early Init phase.
        /// </summary>
        void Awake()
        {
            if (beams == null)
            {
                Debug.LogError("<Color=Red><a>Missing<a></Color> Beams Reference", this);
            }
            else
            {
                beams.SetActive(false);
            }
        }

        /// <summary>
        /// MonoBehavior method called on GameObject by Unity every frame.
        /// </summary>
        void Update()
        {


            ProcessInputs();

            if (beams != null && IsFiring != beams.activeInHierarchy)
            {
                beams.SetActive(IsFiring);
            }
        }

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

            Health -= 0.1f;
        }

        private void OnTriggerStay(Collider other)
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