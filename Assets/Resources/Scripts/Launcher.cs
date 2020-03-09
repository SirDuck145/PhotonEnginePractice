using UnityEngine;
using Photon.Pun;
using Photon.Realtime;


namespace Com.Lunatic.MyGame
{
    public class Launcher : MonoBehaviourPunCallbacks
    {

        #region Private Serializable Fields

        [Tooltip("The maximum number of players per room. When it is full, a new room will be created")]
        [SerializeField]
        private byte maxPlayersPerRoom = 2;

        [Tooltip("The ui Panel to let the user enter name, connect and play")]
        [SerializeField]
        private GameObject controlPanel;


        [Tooltip("The UI Label to inform the user that the connection is in progress")]
        [SerializeField]
        private GameObject progressLabel;


        #endregion


        #region Private Fields


        /// <summary>
        /// This is the client's version number, it allows us to seperate the players
        /// </summary>
        string gameVersion = "0.1";

        /// <summary>
        /// Keep track of the current process, since connection is async
        /// we need to know when the user is attempting to connect
        /// </summary>
        bool isConnecting;


        #endregion


        #region MonoBehavior CallBacks


        /// <summary>
        /// Monobehavior method called on the gameobject by unity during early initialization phase
        /// </summary>
        private void Awake()
        {
            // *Critical*
            // this makes sure we can use PhotonNetwork.loadLevel() on the master client and all clients in that same room sync their level
            PhotonNetwork.AutomaticallySyncScene = true;
        }

        private void Start()
        {
            progressLabel.SetActive(false);
            controlPanel.SetActive(true);
        }



        #endregion


        #region Public Methods

        /// <summary>
        /// Start the connection process.
        /// - if already connected, attemp to join a random room
        /// - if not yet connected, connect this application instance to photon cloud network
        /// </summary>
        public void Connect ()
        {
            isConnecting = PhotonNetwork.ConnectUsingSettings(); //What exactly does this func do?
            progressLabel.SetActive(true);
            controlPanel.SetActive(false);

            if (PhotonNetwork.IsConnected)
            {
                // *Critical*, Attempt joining a random room
                PhotonNetwork.JoinRandomRoom();
            }
            else
            {
                PhotonNetwork.ConnectUsingSettings();
                PhotonNetwork.GameVersion = gameVersion;
            }
        }


        #endregion


        #region MonoBehaviorPunCallbacks Callbacks


        public override void OnConnectedToMaster()
        {
            Debug.Log("Pun basics tut/launcher: OnConnectedToMaster() was called by PUN");
            // #Critical: We try to join a random room, if we can't we will be called back with OnJoinRandomFailed()
            if (isConnecting)
            {
                PhotonNetwork.JoinRandomRoom();
                isConnecting = false;
            }
        }


        public override void OnDisconnected(DisconnectCause cause)
        {
            progressLabel.SetActive(false);
            controlPanel.SetActive(true);

    
            Debug.LogWarningFormat("PUN Basics Tut/Launcher: OnDisconnected() was called by PUN with reason {0}", cause);
        }

        public override void OnJoinRandomFailed(short returnCode, string message)
        {
            Debug.Log("PUN Basics Tutorial/Launcher:OnJoinRandomFailed() was called by PUN. No random room available, so we create one.\nCalling: PhotonNetwork.CreateRoom");

            // #Critical: we failed to join a random room, maybe none exist or are full.
            PhotonNetwork.CreateRoom(null, new RoomOptions {MaxPlayers = maxPlayersPerRoom });
        }

        public override void OnJoinedRoom()
        {
            Debug.Log("PUN Basics Tutorial/Launcher: OnJoinedRoom() called by PUN. Now this client is in a room.");

            //#Critical: we only load if we are the first player, else we rely on the auto sync
            if (PhotonNetwork.CurrentRoom.PlayerCount == 1)
            {
                Debug.Log("We load the room for one player");

                // #Critical
                // Load the Room Level --> This will be a lobby soon
                PhotonNetwork.LoadLevel("GameRoom");
            }
        }
        #endregion

    }

}