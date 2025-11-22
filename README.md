# SimpleComponentCopy
### A Simple Component Copier For The Unity Editor

Used to copy Component objects from one GameObject to another in the Unity Editor. Essentially a copy paste for components. Is functional now, happy for any contributions or feedback.

![image](https://github.com/jr-uk/SimpleComponentCopy/assets/10715164/69206d7b-384f-4bf5-8fbc-baa3bdbbf79b)

Note: By default it only creates components of the same type. Enable **Include serialized values** in the tool window if you also want the field/property data copied.

### Recent Changes
- Added per-component toggles so you can cherry-pick what gets copied.
- Added the **Include serialized values** toggle to optionally bring over configured data.
- Skips unsafe-to-clone components such as `Transform`, `MeshFilter`, and `MeshRenderer` automatically.

### Setup
Open the package manager and click the plus sign and enter the URL below to add the project

https://github.com/jr-uk/SimpleComponentCopy.git

![image](https://github.com/jr-uk/SimpleComponentCopy/assets/10715164/97d43568-1591-4377-9c76-3a9788b4ab73)

Alternatively, clone the repo to a folder within your project asset folder and when the editor detects this up it will be loaded.

### Usage

Access the window for this tool from within the editor at the top of the window.

Windows > SimpleComponentCopy

![image](https://github.com/jr-uk/SimpleComponentCopy/assets/10715164/b7e2b595-5c90-477e-b74c-54790b0b20c6)

Developed in editor version 'Unity 2022.3.16f1'
