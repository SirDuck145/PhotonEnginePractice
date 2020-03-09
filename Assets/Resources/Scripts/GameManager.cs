using System.Collections;
using System;

using UnityEngine;
using UnityEngine.SceneManagement;

using Photon.Pun;
using Photon.Realtime;


namespace Com.LunacyIncorporated.Landslaught
{
    public class GameManager : MonoBehaviourPunCallbacks
    {
        #region Photon Callbacks

        public override void OnPlayerEnteredRoom(Player newPlayer)
        {
            Debug.LogFormat("OnPlayerEnteredRoom() {0}", newPlayer.NickName); // not seen if you're the player connecting

            if (PhotonNetwork.IsMasterClient)
            {
                Debug.LogFormat("OnPlayerEnteredRoom IsMasterClient {0}", PhotonNetwork.IsMasterClient); // called before OnPlayerLeftRoom


                LoadArena();
            }
        }

        public override void OnPlayerLeftRoom(Player otherPlayer)
        {
            Debug.LogFormat("OnPlayerLeftRoom() {0}", otherPlayer.NickName);

            if (PhotonNetwork.IsMasterClient)
            {
                Debug.LogFormat("OnPlayerLeftRoom IsMasterClient {0}", PhotonNetwork.IsMasterClient);

                LeaveRoom();
            }
        }


        public override void OnLeftRoom()
        {
            SceneManager.LoadScene(0);
        }



        #endregion


        #region Public Methods


        public void LeaveRoom()
        {
            PhotonNetwork.LeaveRoom();
        }


        #endregion

        #region Public Methods

        void LoadArena()
        {
            if (!PhotonNetwork.IsMasterClient)
            {
                Debug.LogError("PhotonNetwork: Trying to load a level, but we are not the master client");
            }

            if (PhotonNetwork.CurrentRoom.PlayerCount == 2)
            {
                Debug.Log("PhotonNetwork: Loading the battle arena");
                PhotonNetwork.LoadLevel("GameRoom");
            }
            else
            {
                //This will eventually load a lobby room
                if (PhotonNetwork.CurrentRoom.Name != "Launcher")
                {
                    PhotonNetwork.LoadLevel("GameRoom");
                }
            }

        }
        #endregion

    }

}
