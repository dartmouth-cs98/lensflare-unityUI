# LensFlare
Development Repo for the LensFlare Project.
Contributors: Ellis Guo, Armin Mahban, Jeremy Mittleman, Nick Moolenijzer, and Richard Shen

LensFlare is an application for the HoloLens that imagines the many possibilities for augmented reality as a part of daily life. How can AR technology improve or enhance our ordinary experiences? Through object recognition, spacial mapping and clever implementation of world space holograms, LensFlare sets the lofty goal of integrating holographic technologies into mainstream society. 

![picture alt](http://i.imgur.com/gdKcxva.jpg)
![picture alt](http://i.imgur.com/pQevLhI.jpg)

## Architecture:
UI: The UI for LensFlare will consist of both world space canvases and screen overlay canvases. The former will be used in conjunction with the spacial mapping data to attach holograms to real-world objects. The latter will part of our MVP and provide us with a framework to quickly display and test our scene analysis and data parsing. For these displays we will be using the Unity Engine and C# language. 

## Code:
As of Oct 18th: The most up-to-date code in development currently resides on the `photographer` branch. The posted code base currently includes basic scripts implementing the functionalities of being able to capture and save an image with the HoloLens, being able to run actions from vocal keyword-based commands, and being able to detect and translate text within a given image. Each of these functionalities is currently implemented as a distinct script. We are currently working on integrating these functionalities together and as such the structure of the current back-end code base is likely to change in the near future.

## Setup:
In order to build and run LensFlare, the folloing prerequisites are required:
* PC running Windows 10
* Microsoft HoloLens
* Visual Studio 2015 Update 3
* Unity HoloLens Technical Preview
For up-to-date step-by-step instructions please vist <https://developer.microsoft.com/en-us/windows/holographic/install_the_tools>

## Deployment:
The build process for LensFlare is reasonably intricate. Microsoft offers excellent documentation at <https://developer.microsoft.com/en-us/windows/holographic/holograms_100>. Please follow chapters 4 and 5 for detailed build instructions. 

## Contributor responsibilities/information Week of Oct 12 - 18:
[*Ellis Guo*] Mostly working speech recognition and picture taking / file access. Bug fixes in the build process

[*Armin Mahban*] Work on screen-overlay canvas. Bug fixes in the build process. README udpates

[*Jeremy Mittleman*] Work on screen-overlay canvas. Research into canvas animation.

[*Nick Moolenijzer*] JSON parsing

[*Richard Shen*] Coroutine fixes and improvements. Bug fixes in the build process. 


## Contributor responsibilities/information Week of Oct 5 - 11:
[*Ellis Guo*]

[*Armin Mahban*] Mockups and presentation for class

[*Jeremy Mittleman*] As of October 4th, still playing with (already wasted a huge amount of time with) different Windows Dev Environments (Bootcamping v VM) and completing Unity tutorials (https://github.com/Jmittleman17/Roll-A-Ball)

[*Nick Moolenijzer*] As of October 18th, working on data structures underlying JSON requests/responses for the Google Vision API.

[*Richard Shen*] As of Oct. 4th: developed and unit-tested the code for making API calls to Google Vision and Google Translate. Learned about working with the Microsoft Visual Studio and Unity development environments, as well as with C#. Faced several issues with developing for both environments, particularly when working with libraries for making REST requests. 

## TODO Week of Oct 18 - Oct 26:

* First completed iteration of Static Space Canvas

* Begin Space Canvas Animations

* Bug fixes for image access and subsequent Cloud Vision Api Call 

* Formatted results of Cloud Vision Api Call

## Update as of Nov 12:
* Static canvas and active icon complete, fine tuning placement and sizing. 
* Landmark recognition working and populating the canvas with a title and brief description of the landmark, retrieved from Wikipedia.
* Translation needs work: clustering of the text we retrieve from Cloud Vision proving difficult, will require more work in the winter. Scoring of clusters completed based on position in frame and size of the text. 

## Acknowledgements
