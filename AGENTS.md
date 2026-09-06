# Virtual Dissection Hall - Codex Instructions



## Project

This is the Virtual Dissection Hall (VDH), a Unity VR project for Meta Quest 3.



The current project is functional.

Preserving existing functionality is the highest priority.



## Critical safety rules



Do not make large architectural changes without explicit permission.



Before modifying code:



1. Inspect the relevant files.

2. Identify dependencies.

3. Explain the proposed modification.

4. Modify the smallest possible number of files.



Do not rename or delete:



- GameObjects

- scenes

- prefabs

- layers

- tags

- serialized fields

- materials

- shaders



unless explicitly requested.



Do not modify the following without explicit permission:



- Packages/manifest.json

- ProjectSettings/

- Assets/Scenes/

- Assets/Settings/

- prefabs

- shader files



## Git workflow



The stable branch is:



main



Development work must happen on:



codex-dev



Never merge automatically into main.



Never switch to main unless explicitly requested.



## VDH platform



- Unity

- C#

- Meta Quest 3

- Meta XR / OVR

- XR interactions



## VR interaction



The right controller is the primary controller for anatomical interactions.



The left controller must not interact with anatomical information pins.



Important layer:



PinesInformativos



## Anatomical systems



The project contains anatomical systems including:



- Circulatory

- Skeletal

- Muscular

- Digestive

- Nervous

- Respiratory



## Anatomical pins



Pins open world-space anatomical information panels.



Expected behavior:



1. Right controller selects a pin.

2. Corresponding panel opens.

3. Previous panel closes.

4. Only one information panel should normally be active.



## Dissection window



The project has two interaction modes:



### Head-Following

The dissection window follows the user's head.



### Spatially Locked

The dissection window remains fixed in world space.



Do not change HF or SL behavior unless explicitly requested.



## Shader



The project uses a custom dissection shader.



Do not replace or substantially rewrite shaders without explicit permission.



## Safe workflow



For each requested change:



1. Explain what files you intend to inspect.

2. Inspect the current implementation.

3. Explain the root cause or architecture.

4. Propose the smallest safe change.

5. Ask for permission before changing multiple systems.

6. Modify only necessary files.

7. Report every modified file.

8. Explain how to test the change in Unity.

9. Do not commit unless explicitly requested.

10. Do not push unless explicitly requested.


