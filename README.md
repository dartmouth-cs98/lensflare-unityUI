# Lensflare
Development Repo for the Lensflare Project.
Contributors: Ellis Guo, Armin Mahban, Jeremy Mittleman, Nick Moolenijzer, and Richard Shen

Lensflare is an application for the HoloLens that imagines the many possibilities for augmented reality as a part of daily life. How can AR technology improve or enhance our ordinary experiences? Through object recognition, spacial mapping and clever implementation of world space holograms, Lensflare sets the lofty goal of integrating holographic technologies into mainstream society. 

![picture alt](http://i.imgur.com/gdKcxva.jpg)
![picture alt](http://i.imgur.com/pQevLhI.jpg)

## Architecture:
UI: The UI for Lensflare will consist of both world space canvases and screen overlay canvases. The former will be used in conjunction with the spacial mapping data to attach holograms to real-world objects. The latter will be part of our MVP and provide us with a framework to quickly display and test our scene analysis and data parsing. For these displays we will be using the Unity Engine and C# language. 

## Code:
As of Nov 15th: The most up-to-date code in development currently resides on the `master` branch. The posted code base currently implements basic functional text translation, landmark recognition, and general annotation of your immediate surroundings. When built to a HoloLens, these features can be called by commands such as "Lensflare, translate" and "Lensflare, what is this?". Returned data is currently given as text on screen overlay canvases. The current code base represents a basic "MVP" of our proposed application.

## Setup:
In order to build and run Lensflare, the folloing prerequisites are required:
* PC running Windows 10
* Microsoft HoloLens
* Visual Studio 2015 Update 3
* Unity HoloLens Technical Preview
For up-to-date step-by-step instructions please vist <https://developer.microsoft.com/en-us/windows/holographic/install_the_tools>

## Deployment:
The build process for Lensflare is reasonably intricate. Microsoft offers excellent documentation at <https://developer.microsoft.com/en-us/windows/holographic/holograms_100>. Please follow chapters 4 and 5 for detailed build instructions. 

## Contributor responsibilities/information for 16F:
[*Ellis Guo*] Photography and speech recognition. General Visual Studio/Unity build debugging.

[*Armin Mahban*] Screen-overlay canvas and speech recognition. General Visual Studio/Unity build debugging.

[*Jeremy Mittleman*] Screen-overlay canvas. General Unity debugging.

[*Nick Moolenijzer*] JSON parsing. General Visual Studio debugging.

[*Richard Shen*] Google Vision and Wikipedia API calls. General Visual Studio/Unity build debugging.

## TODO for 17W:

* World space canvas

* Text importance algorithm

* Speed optimizations

* Additional informational features

## Update as of Nov 12:
* Static canvas and active icon complete, fine tuning placement and sizing. 
* Landmark recognition working and populating the canvas with a title and brief description of the landmark, retrieved from Wikipedia.
* Translation needs work: clustering of the text we retrieve from Cloud Vision proving difficult, will require more work in the winter. Scoring of clusters completed based on position in frame and size of the text. 

## Acknowledgements
http://www.roadtoholo.com/2016/05/04/1601/text-to-speech-for-hololens/
Google Vision
