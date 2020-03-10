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

        #region Public Fields


        public static GameManager Instance;

        [Tooltip("The prefab used to represent the player")]
        public GameObject playerPrefab;


        #endregion


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

        #region Private Methods

        private void Start()
        {
            Instance = this;

            if (playerPrefab == null)
            {
                Debug.LogError("<Color=red><a>Missing</a></Color> playerPrefab reference. Please set one up in GameObject 'Game Manager'", this);
            }
            else
            {
                if (PlayerManager.LocalPlayerInstance == null)
                {
                    Debug.LogFormat("We are instantiating localplayer from {0}", SceneManagerHelper.ActiveSceneName);
                    // We are in a room. spawn a character for the local player.
                    PhotonNetwork.Instantiate(this.playerPrefab.name, new Vector3(0f, 5f, 0f), Quaternion.identity, 0);
                }
                else
                {
                    Debug.LogFormat("Ignoring scene load for {0}", SceneManagerHelper.ActiveSceneName);
                }
            }
        }

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
