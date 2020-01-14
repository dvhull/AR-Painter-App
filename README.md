# AR-Painter-App
 Simple Augmented Reality drawing App made with Unity's AR foundation framework.
 
 # Demo
 For a quick demo visit https://dvhull.github.io/personal-website/projects.html and scroll down to the fourth project.
 
 # How it works
While the user holds down their finger every frame a raycast from the fingers position on the screen is casted on to a invisible plane directly in front of the phone/camera. The raycasts collision positions are returned and used to update the the line until the finger is lifted up. 

![Diagram2](https://user-images.githubusercontent.com/56657018/72352964-9bdca600-36b9-11ea-92b0-d2cc481d5b43.png)

