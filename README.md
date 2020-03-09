# PhotonEnginePractice
In this project I am following along with the Photon Engine tutorial using PUN.<br/>

### Lobby Implementation
My current lobby implementation differs slightly from their own.<br/>
I intend on creating a game with just two players, so I decided to forgo<br/>
implementing multiple rooms that fit the naming schema they suggest.<br/>

The user is met with a simple launch screen<br/>
![The user login and connect page](Screenshots/BasicMenu.png)
This launch screen remembers the users last entered username.<br/>
Just below the user name, there is a play button which loads the user into a room using PUN.

### Player Implementation
At this point in the tutorial I am creating and working on the player prefab<br/>
This prefab is not instanced and put into a room yet by PUN.<br/>
At the current moment it only exists in a test room, where I am creating the movement and other mechanics<br/>
![The player prefab running](Screenshots/Running.png)
The player is also able to jump, given that they are already running<br/>
![The player begins the jump](Screenshots/Jump1)
![The player mid jump](Screenshots/Jump2)
![The player lands the jump](Screenshots/Jump3)
