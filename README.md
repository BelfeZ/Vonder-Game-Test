**How to Open Scene**
- "SampleScene" in Assets/Scenes folder

**Object that can interact**
- NPC can interact at first and after got quest item.
- Potion (Appear after first talk NPC)

**Timeline asset location**
- In Assets/Timelines this project have 2 timelines

**How Dialogue UI trigger**
- This project Dialogue data will use from Scenario Data in folder Assets/Scenarios and Trigger & Handle in QuestManager.cs

**How item arrow and Item collect notification trigger**
- Item arrow will appear when player talked to NPC first time after that Quest start will appear with Item arrow that point to the item location with handle when player walk off item the arrow will reappear.

**Used 3rd Party Asset (All asset are from Unity asset store and free.)**
- Casual Game Sounds U6, Classic Footstep SFX, Essential 2D Particle FX, GameInputControllerIconsFree, Layer Lab, LUShvalleySound, Pixel Tiles pack, Potion, Tiny Swords, Wizard - 2D Character

**(7/6/2026)**

6.00pm - 11.00pm | Gathering assets for this test and setup scene, Import package, Setup Player controler
- Finding assets that can use in this test and import cinemachine into project, The playercontroller is came with Character asset.

**(8/6/2026)**

9.00am - 13.00pm | Create animation for NPC and can interact with handle, Create Scenario Data System structure for making dialogue with combination of timeline that can edit later, 
Virtual camera for both timeline and cinemachine and put in scene.
- NPC animation contain Idle, Walk and Talk with handled state and NPC have patrol via pinpoint system and Create Scenario Data that can insert dialogue and timeline and banner between 1 scenario.
- Setting the cinemachine to follow the player with little smooth camera.

13.00pm - 15.00pm | Create Arrow guideline , Banner Notification via instance , Item interaction with quest handle
- Handle the arrow guideline to appear when item is outside screen, Handle banner notification for scenario and item pickup and handle the quest routine so player cannot do other thing than talk and pick up item and send quest.

15.00pm - 18.00pm | Use Arrow guideline to item into project and create timeline with addition VFX and sound effect , Adjust timelines
- Insert the arrow logic in game and Create 2 timelines for starting quest and ending quest, Import some sound effect and put in the Sound manager.
- The timelines contain signal track focus (Start quest) and cenimachine camera focus (End quest)

18.00pm - 20.00pm | Polish some script and UI and Check critical bug
- Polish some UI and some timeline (Add some VFX and make dialogue portrait can play).

20.00pm - 22.00pm | Clean up project
- Delete some 3rd party asset that doesn't use in game.

Total hour spent = 18 hr
