
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
We documented our work through issues, pull request, smart commits, comments in the code and written meeting logs. 


### Use of Code Reviews
In our repository we had made it so that commiting to master was not possible, and Pull Requests would have to be reviewed before they could be merged. To review a pull request we would first make sure that the game would run. We would then look at for any bugs in the implemented feature, and in any systems that could be affected by the system. We would also look at the code to make sure there were no obvious issues with the code that could cause problems in the future. After the creation of some performance benchmarking tools, we would also run these on the PRs to make sure they did not cause significant degradation of the game’s performance. 



### What We Would Have Done Differently
(Taken more time to think about benchmarks to prevent changes in them)

## Individual: Michael

### Good Code
[Link to code](https://github.com/Hifoz/TGAG/blob/master/Assets/Scripts/Utils/PoissonDiscSampler.cs)

#### Code Quality
The class was written so that it would be easy to use. The sampling process is separated into smaller functions based on logical chunks (generating points, validating points, adding valid points). The code also follows the code conventions we created at the start of the project and is written so that there should be no need for any comments inline in the functions.

#### Quality of comments
All the functions are commented properly according to the agreed on standard. Even if a parameter name is self-describing, there is a small explanation of what it is for. For parameters with less obvious names I wrote a bit more on them, so that it would be easily understandable, but I still tried to keep the descriptions of each parameter somewhat short.

### Bad Code
[Link to code(NaiveMeshDataGenerator.GenerateCubeFace(...))](https://github.com/Hifoz/TGAG/blob/f109a9fbffa9a72b2cd4d8168ae7eeef66c7ca11/Assets/Scripts/WorldGen/MeshGen/NaiveMeshDataGenerator.cs#L114)

#### Code Quality
The code contains a switch in where all branches do the same thing with different numbers, which means there is a lot of repetition, breaking with the DRY principles. It also causes the texture coordinate mapping to be more complex than it needs to (see [NaiveMeshDataGenerator.applyTextureCoordinates(...)](https://github.com/Hifoz/TGAG/blob/f109a9fbffa9a72b2cd4d8168ae7eeef66c7ca11/Assets/Scripts/WorldGen/MeshGen/NaiveMeshDataGenerator.cs#L265)) because the order of the vertices are different for the different face directions.


#### Refactoring the Code
[Pre-refactor](https://github.com/Hifoz/TGAG/blob/f109a9fbffa9a72b2cd4d8168ae7eeef66c7ca11/Assets/Scripts/WorldGen/MeshGen/NaiveMeshDataGenerator.cs#L114)  
[Post-refactor](https://github.com/Hifoz/TGAG/blob/8032ff7d059cfbd9c36a7dd144337ee270f99781/Assets/Scripts/WorldGen/MeshGen/NaiveMeshDataGenerator.cs#L114)  
[Diff](https://github.com/Hifoz/TGAG/pull/114/files)  

The new solution removes the switch statement in favour of using the direction to figure out how to place the vertices. It is more elegant than the old solution and takes up much less space.

The refactor to NaiveMeshDataGenerator.GenerateCubeFace(...) means that I also had to change [NaiveMeshDataGenerator.applyTextureCoordinates(...)](https://github.com/Hifoz/TGAG/blob/8032ff7d059cfbd9c36a7dd144337ee270f99781/Assets/Scripts/WorldGen/MeshGen/NaiveMeshDataGenerator.cs#L236), because the order of the vertices were different. Because the vertices are added in the same order now for all face directions, I could simplify how we add the texture coordinates.


### A personal reflection about professionalism in programming
For me, professional programming is about not programming so that only I can understand and work with the code, but so that other people understand, use and maintain/extend the code. It's also about finding the sweet spot between high-performing code and easily understood and maintainable code. I believe writing good documentation is important for this, because even if someone can read and understand what a part of the code is doing, they also need to understand its place in the bigger picture. Documentation is also important because no matter what, there will be code that is hard to understand, if only because there is no good way of doing it without sacrificing performance.

The code and documentation is only one part of professional programming, another part is the process. If one sits down and starts working on something without planning, it is likely that the project will just end up as a mess. If multiple people start working on a project together without a plan or proper communication, then conflicting ideas might lead to both bad code, and conflict in the team.



## Individual: Martin (Temp title)

### Good code example
Even though this code is used as my example of good code, it has some issues. I did not stick to one of our coding conventions when writing it for instance, we were supposed to explicitly declare the protection level, even if not needed to by the compiler. I did not explicitly declare the member variables as private in this class. 

[BoneKeyFrames](https://github.com/Hifoz/TGAG/blob/master/Assets/Scripts/Animals/BoneKeyFrames.cs) is a class for animating bones in an animal with forward kinematics.

One good thing about the class is that it throws informative exceptions when something is wrong. BoneKeyFrames contains a set amount of frames for the animation, and the setters will throw an exception if you try to set an amount of key frames that differs from what you specified in the constructor. The exceptions thrown are generic System.Exception exceptions with a message, it could be improved by using a custom exception. The use of exceptions in this way makes the class more robust.

Another good thing is the solution for timing other logic with the animation. Instead of having to time the animation and sync your logic with it in that way, the BoneKeyFrames offers KeyFrameTriggers. A KeyFrameTrigger is a delegate function which is called whenever the BoneKeyFrame switches frames. You can specify one for each frame, or a selection of frames. This was used to sync walking sounds with the animations. 

It also allows for interpolation between two BoneKeyFrames that operate on the same bone. The two BoneKeyFrames do not need to have the same number of key frames or timing, the only requirement is that they operate on the same bone. This gives us a versatile tool for transitioning between different animations. 

The quality of the comments in the class is also good in my opinion, the coding standard was followed and each function has an XML style comment with relevant information. I could have specified in the function comment if the functions threw any exceptions though in retrospect.

### Bad code example
The [AnimalState](https://github.com/Hifoz/TGAG/blob/master/Assets/Scripts/Animals/AnimalState.cs) is a class containing information about the state of the animal.

The AnimalState class is bad because it breaks with object oriented design by not hiding the underlying implementation. All of its members are public, and the class contains zero methods. It was originally introduced as a simple container class for some key data concerning animals, it was needed to share data between the Animal and the AnimalBrain. This quick hack was never refactored as it should have been, and we started building additional functionality on top of it, making the refactor more work. 

The main issue with not following object oriented design in my mind is that it causes object functionality to be implemented outside the object. This prevents the logical grouping of methods and attributes. The code is easier to maintain if for instance; all the AnimalState logic is also handeled in the AnimalState class. Right now that logic is primarily handled by the Animal class. The Animal class does not handle the animal state in a logical way either. The AnimalState is calculated in the doGravity() function, and it should be handled in its own calculateState() function at least. I did not intend for the code to end up this way, it mostly evolved into it over time. I did not initially have a concept of state, I just calculated if the animal was grounded or not before doing gravity, which is why state calculations happen in doGravity(). 

[Bone](https://github.com/Hifoz/TGAG/blob/master/Assets/Scripts/Animals/AnimalSkeleton.cs) is the class for one of the animation bones in an animal. (Class is defined at the top of AnimalSkeleton) 

Another similar example to AnimalState is the Bone class declared at the beginning of AnimalSkeleton.cs. It contains 3 members, the Transform of the bone and the lower/upper rotation limits. This class is bad for the same reasons as AnimalState, it causes the implementation of the Bone class to happen outside the Bone same as with the AnimalState. The Bone class should have an applyRotation(Quaternion rotation) function, which enforces the rotation constraints. This functionality is instead implemented in the Animal class, in the various places rotations are applied (such as in CCD). 

These various bits of code mentioned here are things that i kind of implemented and forgot about. The lacking quality of the code became apparent as I were going back to old code when writing the bachelor thesis. Which made me think that maybe dedicating some time to just looking over and reviewing old code might be helpful. You will look at the code with a new perspective after some time has passed and your understanding of the code base is different. 

### Refactored code example
[Pre-AnimalSkeleton](https://github.com/Hifoz/TGAG/blob/ad9fc592706c0670775d5aa66091d2015da44a38/Assets/Scripts/WorldGen/AnimalSkeleton.cs)  
[Post-AnimalSkeleton](https://github.com/Hifoz/TGAG/blob/8032ff7d059cfbd9c36a7dd144337ee270f99781/Assets/Scripts/WorldGen/MeshGen/NaiveMeshDataGenerator.cs#L114)  
[Diff-AnimalSkeleton](https://github.com/Hifoz/TGAG/commit/4cce840e0357055cb02eb6f63e643bd55d8ee71e#diff-e2e4fd7d70a051652bcd8de3408a38fd)  

AnimalSkeleton is the class responsible for defining and generating the body of an animal. The post version of AnimalSkeleton linked to here is not the final version of animal skeleton, but the version from the refactor pull request. The the final version the AnimalSkeleton is an abstract superclass, that specific animal types inherit from. At this point in development only one animal type was implemented. 

The AnimalSkeleton generates a body for the animal based on a set of parameters such as legCount, legLength and so on. In the initial implementation these parameters were all individually declared as member variables of AnimalSkeleton, this is what the refactor addressed.

The issues with declaring every parameter for the AnimalSkeleton as their own member attributes is that it gives us no logical grouping of the parameters, and it prevents us from being able to iterate over them. It is also not very scalable and adaptable, which is something we need it to be in order to support multiple different types of animals, all consisting of different sets of parameters. Initializing a set of let’s say 15 individual parameters using random number generators is also tedious and error prone due to the possibility of forgetting to initialize some parameters.

The refactor solved these issues by introducing the concept of BodyParameters. A BodyParameter is an enum, which is used as a key to a MixedDicitonary. After the refactor the parameters of the animal are stored in a single MixedDictionary<BodyParameters>, and the possible ranges for each BodyParameter is stored in a second MixedDictionary<BodyParameters>. This gives us a logical grouping of the parameters by keeping all of them in a single dictionary. Every class that (in successive PR’s) inherits from AnimalSkeleton can populate the dictionaries with the BodyParameters relevant to the specific animal, instead of inheriting a whole range of “loose” parameter attributes or defining parameter attributes itself. Defining attributes is done by populating the bodyParametersRange dictionary with the ranges for each body parameter. The AnimalSkeleton will then iterate through bodyParametersRange to automatically generate the final bodyParameters dictionary. This makes it so that we don’t have to manually specify which BodyParameters we want and manually do their initialization. 

The downside of the refactor is that it makes the code more verbose, as we have to access a dictionary with an enum when we want to get a BodyParameter. There is also a slight performance hit to doing dictionary access as opposed to directly accessing a member attribute. I feel like these downsides are outweighed by having logical grouping of parameters and automatic initialization of parameter values.  



### Personal reflection about professionalism in programming:
To me professionalism in programming is in part about taking the time to do the “less fun” stuff such as documentation and commenting. If you document your code and process it will be easier for co-workers and other developers to continue you work. It will also be easier for you to go back to old code that you have written yourself if you documented what you did and how the code works. These are things that can be easy to ignore if you are working by yourself, where it might be easy to get carried away and only write code. 

I think professionalism is also about how you write code, putting an emphasis on writing code for maintainability and readability. This can mean making design decisions that will have a negative impact on performance for the sake of making the code readable. This is a difficult choice to make coming from game development, since there is such a high focus on performance when it comes to games. 

Professionalism is also about using a proper development process. Before i started studying and early on in the studies i would never use any process, i would just sit down and write code. This was fine in the beginning, because all of the projects were small and i was working alone. When it comes to bigger projects involving multiple people, this stops working. There is a need for overview of the development, and proper delegation of work amongst the team members. Using a method such as scrum or kanban makes a big difference here. I also found after learning these methods that they are helpful even when working alone. Making a sprint board with various cards for bugs or new features is helpful in giving an overview even when alone. It acts as a method of dividing and conquering the implementation. 

Professionalism is also about trying to give up the “not invented here syndrome” i think. Swallowing your pride and using good solutions developed by other people can be a productive thing to do. There is a large amount of good open source systems and libraries out there, which can speed up development by reducing the amount of work needed to be done.

There is more to professionalism then just developing software as well. As a developer you have some responsibility in regards to your employer and society. This can cause conflicts of interest, when your employers interest seems to conflict with the good of society. Let’s say for instance that the employer is asking you to willingly break the new GDPR regulations for monetary gain. Your options in this scenario do not seem good, on one hand you could whistleblow, which is in the interest of society.  However this could lead to you losing your job, and potentially the jobs of your colleagues. So you would have to choose between causing damage on a local or national scale. It is hard to say which decision is right here, and you would have to base it on your personal values i think. 
