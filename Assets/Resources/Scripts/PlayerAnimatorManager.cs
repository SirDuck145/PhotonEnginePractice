using System.Collections;
using UnityEngine;

using Photon.Pun;


namespace Com.LunacyIncorporated.Landslaught
{
    public class PlayerAnimatorManager : MonoBehaviourPun
    {

        #region Private Fields

        [SerializeField]
        private float directionDampTime = 0.01f;


        #endregion


        #region MonoBehavior Callbacks

        private Animator animator;

        //Use this to init
        private void Start()
        {
            animator = GetComponent<Animator>();
            if (!animator)
            {
                Debug.LogError("PlayerAnimatorManager is Missing Animator component");
            }
        }


        //Update is called once per frame
        private void Update()
        {
            //If the view is not the players, and they are connected, do nothing
            if (photonView.IsMine == false && PhotonNetwork.IsConnected == true)
            {
                return;
            }


            //Handles no animator
            if (!animator)
            {
                return;
            }

            AnimatorStateInfo stateInfo = animator.GetCurrentAnimatorStateInfo(0);
            //Only allow jumping if the player is running
            if (stateInfo.IsName("Base Layer.Run"))
            {
                if (Input.GetButtonDown("Fire2"))
                {
                    animator.SetTrigger("Jump");
                }
            }
            float h = Input.GetAxis("Horizontal");
            float v = Input.GetAxis("Vertical");
            if (v < 0)
            {
                v = 0;
            }
            animator.SetFloat("Speed", h * h + v * v);
            animator.SetFloat("Direction", h, directionDampTime, Time.deltaTime);

        }


        #endregion
    }
}
