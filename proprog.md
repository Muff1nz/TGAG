
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
In our repository we had made it so that commiting to the master branch was not possible, and Pull Requests would have to be reviewed before they could be merged. To review a pull request we would first make sure that the game would run. We would then look at for any bugs in the implemented feature, and in any systems that could be affected by the system. We would also look at the code to make sure there were no obvious issues with the code that could cause problems in the future. After the creation of some performance benchmarking tools, we would also run these on the PRs to make sure they did not cause significant degradation of the game’s performance.


## Individual: Michael

### Good Code
[Link to code](https://github.com/Hifoz/TGAG/blob/master/Assets/Scripts/Utils/PoissonDiscSampler.cs)

#### Code Quality
The class was written so that it would be easy to use. The sampling process is separated into smaller functions based on logical chunks (generating points, validating points, adding valid points). The code also follows the code conventions we created at the start of the project.

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


### A personal reflection about professionalism in programming (Temp title)
TODO

## Individual: Martin (Temp title)

### A link to, and discussion of, code you consider good (Temp title)

### A link to, and discussion of, code you consider bad (Temp title)

### A link to two pieces of code, a before and after refactoring.  This will include a discussion of why the code was refactored (Temp title)

### A personal reflection about professionalism in programming (Temp title)


