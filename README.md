# No Money Tools
A collection of systems and tools designed to aid in the development of video games with the Unity Game Engine.
While this repo is largely designed to be modular (use what you need), certain systems will be designed to work with other systems.

**Please note, this repo is about to undergo major updates over the next few months.**

## Audio System
This is currently the system we use to handle Audio.
### AudioManager.cs
This manages all the sounds that an object can make. It's recommended that any object that creates more than one sound has an Audio Manager.
### Sound.cs
This is any noise and is used by the Audio Manager.

## Core System
These are our core components we don't leave home without.
### Singleton.cs
All of your Game Manager classes and other Singletons will inherit from this class.
### LevelLoader.cs
Use this to transition between Scenes.

## Object Pooling
Object Pooling is useful for managing many spawnable Game Objects and puts minimal strain on your game's processing speed by instantiating all of the objects up front into Pools.
### ObjectPooler.cs
Attach this component to a Game Object to begin generating and using Object Pools. By default this inherits from the Singleton class.
### ISpawnable.cs
Any object that can be used by the Object Pooler must have a component that implements this interface.

## TMP Animations
This is a collection of scripts to use in conjunction with Text Mesh Pro to add effects to your text.

## Tween Animations
We love DoTweenPro. It's a very affordable Tweening library that we find make creating simple animations easier.

### TweenAnimation.cs
This is a class we created to aid in your creation of good looking Tweens and Sequences. Here are the steps to use it:

 1. Create a c# class that will be your animation, and have it inherit from TweenAnimation.cs
 2. Serialize one of our nested Parameter classes, TweenFloatParameters or TweenVecParameters
 3. Create a function to be called that will generate your animation.
 4. We recommend that in your Game Manager class you create an event to Kill Tweens that will be called when switching to new Scenes and subscribe TweenAnimation.KillCurrent to it.
