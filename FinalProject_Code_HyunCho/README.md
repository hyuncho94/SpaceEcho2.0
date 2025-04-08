Hi Tiffany this part is for you to make you check easier.

Class-Based (Object-Oriented Design)
Files: All major scripts (e.g., JetpackInput.cs, vomitting.cs, QuestionManager.cs)

Inheritance
Files: JetpackInput.cs, JetpackInputBase.cs

Abstraction
Files: JetpackInputBase.cs

Overloading
Files: JetpackInput.cs

Overriding
Files: JetpackInput.cs, JetpackInputBase.cs

Polymorphism
Files: ThrustController.cs, JetpackInput.cs, JetpackInputBase.cs

File Processing (File I/O)
Files: SessionLogger.cs

Exception Catching (optional)
Files: Not implemented

Java/Processing Libraries (optional)
Files: Not applicable (Unity/C# project)

# Final Project Space Echo: Cleaned and Commented Code

This project is a multiplayer VR application using Unity, Photon, and voice-based interaction.  
Below is a summary of each script file included in this project:

---

### Network & Player Management

- **NetworkManager.cs**  
  Handles connection to the Photon server and room management.

- **NetworkPlayerSpawner.cs**  
  Instantiates the local player prefab when joining a room and cleans up on leave.

- **NetworkPlayer.cs**  
  Syncs local player's VR tracking data (head and hands) to their corresponding rig transforms.

- **NetworkIKPlayer.cs**  
  Maps the OVRCameraRig and sets up VR rig transforms, jetpack inputs, and network speech UI.

---

### VR Tracking & Input

- **VRRig.cs**  
  Maps VR device positions and rotations to rig targets in the scene.

- **JetpackInputBase.cs**  
  Abstract class that defines the structure for any type of jetpack input.

- **JetpackInput.cs**  
  Implements two input modes (Trigger-based or Voice-based) using method overloading. Calculates thrust force and direction.

- **ThrustController.cs**  
  Applies combined thrust forces from left and right jetpack inputs to a Rigidbody.

---

### Voice Interaction & Visual Feedback

- **vomitting.cs**  
  Triggers character-by-character text ejection (vomit-like animation) based on user input or voice.

- **Vomit.cs**  
  Handles physical motion and text rendering of each ejected character.

- **vomitRotate.cs**  
  Continuously rotates the spawned text object for visual emphasis.

- **VoiceManager.cs**  
  Handles Android microphone permission requests for voice input.

---

### Visual UI & Interaction

- **LookAt.cs**  
  Rotates an object to face the player's camera on the horizontal plane.

- **QuestionManager.cs**  
  Manages question prompts and triggers UI display via RPC.

- **QuestionTrigger.cs**  
  Detects when a player enters a zone and calls the question manager.

- **UIManager.cs**  
  Displays or hides question popup UI with updated text.

---
# SessionLogger.cs

This Unity C# script logs the number of words spoken by a user during a session and saves the result to a CSV file.

## Features
- Compatible with Android builds (uses `Application.persistentDataPath`)
- Creates a `word_log.csv` file with the following columns:
  - Timestamp
  - WordCount
- Appends new rows each time `LogWordCount(int wordCount)` is called

## Usage
1. Attach `SessionLogger` to a GameObject in your Unity scene.
2. When you want to log the user's spoken word count (e.g., at the end of a session), call:
   ```csharp
   FindObjectOfType<SessionLogger>().LogWordCount(wordCount);
   ```
3. The log file will be saved to:
   - Android: `/storage/emulated/0/Android/data/<your.package.name>/files/word_log.csv`
   - PC/Mac: `Application.persistentDataPath/word_log.csv`

## Note
This script uses UTF-8 encoding and ensures a header row is created if the file does not already exist.


