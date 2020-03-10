# PhotonEnginePractice
In this project I am following along with the Photon Engine tutorial using PUN.<br/>

### Lobby Implementation
My current lobby implementation differs slightly from their own.<br/>
I intend on creating a game with just two players, so I decided to forgo<br/>
implementing multiple rooms that fit the naming schema they suggest.<br/>

The user is met with a simple launch screen<br/>
![The user login and connect page](Screenshots/BasicMenu.png)
This launch screen remembers the users last entered username.<br/>
Just below the user name, there is a play button which loads the user into the play room using PUN.

### Player Implementation
The player is instantiated from a prefab<br/>
This prefab is instanced and put into a room by Pun allowing me to synchronize the player across all clients.<br/>
![The player prefab running](Screenshots/Running.png)
I disliked how the tutorial implemented their lasers, you were able to hit yourself<br/>
I fixed this issue by just checking if the collision happened to be with its own collider.</br>
The player is also able to jump, given that they are already running<br/>
![The player begins the jump](Screenshots/Jump1.png)
![The player mid jump](Screenshots/Jump2.png)
![The player lands the jump](Screenshots/Jump3.png)
