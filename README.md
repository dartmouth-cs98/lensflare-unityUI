# LensFlare
Development Repo for the LensFlare Project.
Contributors: Ellis Guo, Armin Mahban, Jeremy Mittleman, Nick Moolenijzer, and Richard Shen

Lensflare is an application for the Microsoft HoloLens that imagines the many possibilities for augmented reality as a part of daily life. How can AR technology improve or enhance our ordinary experiences? Through providing the user the ability to annotate any space of their choosing in any manner of their choosing, Lensflare allows anyone to make their own holographic playground, work station, art exhibit, or whatever else they might think of. With Lensflare, the world is your sandbox.

TODO: add screenshots

## Architecture:
UI: On the HoloLens, the UI for Lensflare consists primarily of interactive gems through which users can attach text, images, videos, or even holograms to any point in their chosen space. When sharing a space with others, these gems would be the primary mode by which the users participate in the experience intended by the space designer. When actually designing your own space, the UI also includes voice assistance and holographic canvases for guiding the user through the space creation process.  
Lensflare also includes a backend web application for pairing HoloLens devices and associating media with gems in a user's spaces. This web application consists of a few simple and straightforward pages providing all the necessary steps for fully annotating as many spaces as a user might wish. 
The HoloLens aspect of the Lensflare application is written using the Unity Engine and C# language. The web application aspect is written in JavaScript with Node and Express, and utilizes Heroku for the backend server, MongoDB for the database, and Amazon S3 for remote storage.

## App flow

To set up your first space, follow the following instructions:

1. Sign up or log on at our [Website](www.lensflare.space) 
2. Create a space with a name of your choosing
3. Create a device and associate it with the space made in Step 2
4. Open the QR code for the device
5. Open the Lensflare app on your Hololens
6. When prompted, look at QR code and say "Scan"
7. Say "Lensflare Create Scene" to start placing gems
8. Tap or say "select" to add gems, say "delete" while highlighting a gem to delete it, tap or select an existing gem to move it, tap again to place
9. When ready, say "done" and wait for the data to upload
10. Add text, photos, videos, or meshes to each gem on lensflare.space
11. Watch as the data updates in real time!

## Code:
As of Mar 8th: The `master` branch of the repo lensflare-unityUI contains the most up-to-date code in development for the HoloLens aspect of the Lensflare application. The most up-to-date code in development for the web application aspect of the Lensflare application resides on the `master` branch of the repo lensflare-server. The posted code base on this repo includes 

## Setup:
In order to build and run LensFlare, the folloing prerequisites are required:
* PC running Windows 10
* Microsoft HoloLens
* Visual Studio 2015 Update 3
* Unity HoloLens Technical Preview
For up-to-date step-by-step instructions please vist <https://developer.microsoft.com/en-us/windows/holographic/install_the_tools>

## Deployment:
The build process for LensFlare is reasonably intricate. Microsoft offers excellent documentation at <https://developer.microsoft.com/en-us/windows/holographic/holograms_100>. Please follow chapters 4 and 5 for detailed build instructions. 

## Contributor responsibilities/information:
[*Ellis Guo*] Camera usage, world anchor processing + world anchor sharing (~ 1/2), gem placement/movement, world anchor + image upload/download flow, icon info update flow (hololens side), bug fixes, etc.

[*Armin Mahban*] Media gem model/animations/web loading, instruction canvas design and implmentation, front end design, poster, world anchor processing (< 1/2), bug fixes. 

[*Jeremy Mittleman*] 

[*Nick Moolenijzer*] 

[*Richard Shen*] QR coding, voice assistant, (part of) S3 uploading and communication with backend, other more niche features, bug fixes.

## Acknowledgements

Thanks Tim!
