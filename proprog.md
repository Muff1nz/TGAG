
## From submission @confluence

### A list of the name of the students in the team  
Michael Bråten  
Martin Bjerknes  

### A list of links to any other repos connected to the project  
Original repo that this is a fork of: https://github.com/Hifoz/TGAG  
Benchmark data visualisation repo: https://github.com/Muff1nz/TGAG_PythonScripts  
KinoFog, the open source fog effect we used: https://github.com/keijiro/KinoFog   
Mesh Generator Prototype: https://github.com/Hifoz/voxelmesh  
Noise function and ChunkVoxelDataGenerator prototype: https://github.com/Muff1nz/ProcGen-Prototype   


## Group Discussion

### Strengths and Weaknesses of Languages Used in the Project
C#, shaderlab, hlsl. C# (multiple inheritance, lack of pointers, so if you want a reference to a simple value, you must wrap it in a class first. Memory management, GC (good or bad)). Shaderlab, high level, easier then GLSL, less control, multi compile. 

#### C#
The main language in our project is C#. One issue we encountered with C# was the lack of multiple inheritance when implementing the animals. We wanted the animal behaviour and animal functionality to have their own inheritance trees, we would then implement the animals through multiple inheritance. Since this was not possible we instead implemented behaviour through composition. The lack of multiple inheritance is not an issue that can not be overcome, you can work around it using alternative solutions as we did. 

C# is a somewhat high level language, compared to languages such as C and C++ in that it does the memory management for us. There are two sides to the automatic memory management. On one hand it is convenient that we do not have to worry about it, we can just allocate memory without giving it much thought. This can lead to an increase in productivity by lowering the difficulty of implementing something. A negative aspect of the automatic memory management is that it gives us less control and comes with some overhead. 

A feature we miss in C# that is present in C and C++ is pointers. If you want to maintain a reference to simple types in C# you must first wrap them in a class. Wheres you could just create a pointer to solve the same issue in C or C++. C# has classes and structs, where classes are reference types and structs are value types. This gives us some control of whether or not we want a value or a reference, however it is not as much control as C/C++ pointers give. 


#### ShaderLab/HLSL

We also used ShaderLab and HLSL for our shaders. ShaderLab gives us some options for how much complexity we want when implementing a shader, we can choose between a surface shader or vertex/fragment shader for instance. We used vertex/fragment shaders because it was the most similar to GLSL that we were familiar with from before, and it gives us more control when writing special effects such as water reflections and texture generation.

ShaderLab and Unity provides us with a set of helpful macros for doing things such as shadows. This is helpful for productivity as it lets us implement shadow mapping with two lines of code. The macro also makes it so that the shadow implementation is integrated into Unity. Meaning that things such as graphics options effect the shadow quality. The downside of this is that we lose control of the implementation. When trying to do special effects such as wind moving the leaves of trees the shadow implementation does not account for the change in vertex position, making the shadows render incorrectly. If we implemented the shadows without the macro, we could account for the special effect on the leaves, but this would also increase development time. 

### Process and Communication Systems
For communication and process control we used discord and trello. Discord was used for general purpose communication, it allows us to create servers that we can use for written and verbal communication. We can also share images and other files, this lets us communicate things such as bugs or code with images. Discord also allows for screen sharing. All of these features gives us a large range of options for communicating our ideas.   

We made sure to always be available on discord when we were awake, so that we could reach each other on demand. We had an agreement to conduct our sprint reviews in the discord voice chat every wednesday. 

Trello was used as our sprint board, to give us an overview and delegate the various pieces of development. We would make cards for various pieces of work to be done, this could be things such as a new feature or a bug fix. During every sprint review we would each select which cards we would work on during the next sprint. Without trello or a tool like it we would have trouble keeping tabs on what the other team member is working on. Something we could have looked into is integration between trello and GitHub, so that smart commits and pull request could be associated with the trello cards. 

### Use of Version Control Systems, Ticket Tracking, Branching, Version Control
git, github. Before the bachelor we did not use feature branches, now we did. We would check out old commits when inspecting the effects of new changes.  

For version control we used Git, hosted on GitHub. We used the issue board on GitHub to keep track of any bugs we found while testing. When working on an issue, we also added it to the Trello board. When we were implementing new features, or fixing bugs we would work in feature branches. Using feature branches was not something we had been doing before  this project. Previously we would have one branch per person. Using feature branches made it so that we could see from only the branch name what was being worked on, and one person could work on multiple issues as well (unlike one branch per person, where we would have to wait for our code to be reviewed before we could start working on something else in the branch). 

### Programming and commenting style guide
https://docs.google.com/document/d/19iqSOXVe7pn26eSybdBrjFK4kj44F9KbRVmF8MOfbSE/edit?usp=sharing


### Use of Libraries and Integration of Libraries
Outside of the Unity API and the .NET API we didn’t use any libraries. We did however use KinoFog, a Unity package that is used to create a fade-to-skybox world fog.

### Professionalism in Your Approach to Software Development
The fact that we document our work, write detailed PR’s, do sprint reviews and meeting logs. 



### Use of Code Reviews
In our repository we had made it so that commiting to master was not possible, and Pull Requests would have to be reviewed before they could be merged. To review a pull request we would first make sure that the game would run. We would then look at for any bugs in the implemented feature, and in any systems that could be affected by the system. We would also look at the code to make sure there were no obvious issues with the code that could cause problems in the future. After the creation of some performance benchmarking tools, we would also run these on the PRs to make sure they did not cause significant degradation of the game’s performance. 


## Individual: Michael

### Good Code
[Link to code](https://github.com/Hifoz/TGAG/blob/master/Assets/Scripts/Utils/PoissonDiscSampler.cs)

#### Code Quality
The class was written so that it would be easy to use. The sampling process is separated into smaller functions based on logical chunks (generating points, validating points, adding valid points). The code also follows the code conventions we created at the start of the project and is written so that there should be no need for any comments inline in the functions.

#### Quality of comments
All the functions are commented properly according to the agreed on standard. Even if a parameter name is self-describing, there is a small explanation of what it is for. For paramteres with less obvoius names I wrote a bit more on them, so that it would be easily understandable, but I still tried to keep the descriptions of each parameter somewhat short.

### Bad Code
[Link to code(NaiveMeshDataGenerator.GenerateCubeFace(...))](https://github.com/Hifoz/TGAG/blob/f109a9fbffa9a72b2cd4d8168ae7eeef66c7ca11/Assets/Scripts/WorldGen/MeshGen/NaiveMeshDataGenerator.cs#L114)

#### Code Quality
The code contains a switch in where all branches do the same thing with different numbers, which means there is a lot of repitition, breaking with the DRY principles. It also causes the texture coordinate mapping to be more complex than it needs to (see [NaiveMeshDataGenerator.applyTextureCoordinates(...)](https://github.com/Hifoz/TGAG/blob/f109a9fbffa9a72b2cd4d8168ae7eeef66c7ca11/Assets/Scripts/WorldGen/MeshGen/NaiveMeshDataGenerator.cs#L265)) because the order of the vertices are different for the different face directions.


#### Refactoring the Code
[Pre-refactor](https://github.com/Hifoz/TGAG/blob/f109a9fbffa9a72b2cd4d8168ae7eeef66c7ca11/Assets/Scripts/WorldGen/MeshGen/NaiveMeshDataGenerator.cs#L114)  
[Post-refactor](https://github.com/Hifoz/TGAG/blob/8032ff7d059cfbd9c36a7dd144337ee270f99781/Assets/Scripts/WorldGen/MeshGen/NaiveMeshDataGenerator.cs#L114)  
[Diff](https://github.com/Hifoz/TGAG/pull/114/files)  

The new solution removes the switch statement in favour of using the direction to figure out how to place the vertices. It is more elegant than the old solution and takes up much less space.

The refactor to NaiveMeshDataGenerator.GenerateCubeFace(...) means that I also had to change [NaiveMeshDataGenerator.applyTextureCoordinates(...)](https://github.com/Hifoz/TGAG/blob/8032ff7d059cfbd9c36a7dd144337ee270f99781/Assets/Scripts/WorldGen/MeshGen/NaiveMeshDataGenerator.cs#L236), because the order of the vertices were different. Because the vertices are added in the same order now for all face directions, I could simplify how we add the texture coordinates.


### A personal reflection about professionalism in programming
TODO



## Individual: Martin (Temp title)

### Good code example
[BoneKeyFrames](https://github.com/Hifoz/TGAG/blob/master/Assets/Scripts/Animals/BoneKeyFrames.cs)
Even though this code is used as my example of good code, it has some issues. I did not stick to one of our coding conventions when writing it for instance, we were supposed to explicitly declare the protection level, even if not needed to by the compiler. I did not explicitly declare the member variables as private in this class. 

BoneKeyFrames is a class for animating bones in an animal with forward kinematics.

One good thing about the class is that it throws informative exceptions when something is wrong. BoneKeyFrames contains a set amount of frames for the animation, and the setters will throw an exception if you try to set an amount of key frames that differs from what you specified in the constructor. The exceptions thrown are generic System.Exception exceptions with a message, it could be improved by using a custom exception. The use of exceptions in this way makes the class more robust.

Another good thing is the solution for timing other logic with the animation. Instead of having to time the animation and sync your logic with it in that way, the BoneKeyFrames offers KeyFrameTriggers. A KeyFrameTrigger is a delegate function which is called whenever the BoneKeyFrame switches frames. You can specify one for each frame, or a selection of frames. This was used to sync walking sounds with the animations. 

It also allows for interpolation between two BoneKeyFrames that operate on the same bone. The two BoneKeyFrames do not need to have the same number of key frames or timing, the only requirement is that they operate on the same bone. This gives us a versatile tool for transitioning between different animations. 

The quality of the comments in the class is also good in my opinion, the coding standard was followed and each function has an XML style comment with relevant information. I could have specified in the function comment if the functions threw any exceptions though in retrospect.

### Bad code example
The [AnimalState](https://github.com/Hifoz/TGAG/blob/master/Assets/Scripts/Animals/AnimalState.cs) is a class containing information about the state of the animal.

The AnimalState class is bad because it breaks with object oriented design by not hiding the underlying implementation. All of its members are public, and the class contains zero methods. It was originally introduced as a simple container class for some key data concerning animals, it was needed to share data between the Animal and the AnimalBrain. This quick hack was never refactored as it should have been, and we started building additional functionality on top of it, making the refactor more work. 

The main issue with not following object oriented design in my mind is that it causes object functionality to be implemented outside the object. This prevents the logical grouping of methods and attributes. The code is easier to maintain if for instance; all the AnimalState logic is also handeled in the AnimalState class. Right now that logic is primarily handled by the Animal class. The Animal class does not handle the animal state in a logical way either. The AnimalState is calculated in the doGravity() function, and it should be handled in its own calculateState() function at least. We did not intend for the code to end up this way, it mostly evolved into it over time. We did not initially have a concept of state, we just calculated if the animal was grounded or not before doing gravity, which is why state calculations happen in doGravity(). 

[Bone](https://github.com/Hifoz/TGAG/blob/master/Assets/Scripts/Animals/AnimalSkeleton.cs) is the class for one of the animation bones in an animal. 

Another similar example to AnimalState is the Bone class declared at the beginning of AnimalSkeleton.cs. It contains 3 members, the Transform of the bone and the lower/upper rotation limits. This class is bad for the same reasons as AnimalState, it causes the implementation of the Bone class to happen outside the Bone same as with the AnimalState. The Bone class should have an applyRotation(Quaternion rotation) function, which enforces the rotation constraints. This functionality is instead implemented in the Animal class, in the various places rotations are applied (such as in CCD). 

These various bits of code mentioned here are things that we kind of implemented and forgot about. The lacking quality of the code became apparent as we were going back to old code when writing the bachelor thesis. Which made me think that maybe dedicating some time to just looking over and reviewing old code might be helpful. You will look at the code with a new perspective after some time has passed and your understanding of the code base is different. 

### Refactored code example
[Pre-AnimalSkeleton](https://github.com/Hifoz/TGAG/blob/ad9fc592706c0670775d5aa66091d2015da44a38/Assets/Scripts/WorldGen/AnimalSkeleton.cs)  
[Post-AnimalSkeleton](https://github.com/Hifoz/TGAG/blob/8032ff7d059cfbd9c36a7dd144337ee270f99781/Assets/Scripts/WorldGen/MeshGen/NaiveMeshDataGenerator.cs#L114)  
[Diff-AnimalSkeleton](https://github.com/Hifoz/TGAG/commit/4cce840e0357055cb02eb6f63e643bd55d8ee71e#diff-e2e4fd7d70a051652bcd8de3408a38fd)  

The post version of AnimalSkeleton linked to here is not the final version of animal skeleton, but the version from the refactor pull request. 


### A personal reflection about professionalism in programming (Temp title)
TODO

