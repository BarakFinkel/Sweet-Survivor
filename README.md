# Sweet-Survivor
 
This is a survivor-style mobile game currently in development, designed to deliver fast-paced action and strategic gameplay.  
In this game, you take on the role of a brave Marshmallow fighting to survive against relentless waves of enemy flames trying to melt you!

Each stage challenges you to:  

 🔥 Level Up – Upgrade your stats as you gain experience.  
 🛒 Gear Up – Purchase new weapons from the shop between waves.  
 💀 Survive – Endure the fiery assault and make it to the end!  

The development process heavily incorporates OOP principles and design patterns such as:

- **State Machine** – Conceptually implemented via the GameManager to manage game states efficiently.
- **Singleton Pattern** – Used for global access to core systems.
- **Inheritance** – Ensuring clean, maintainable, and scalable code.

Like all survivor games, this project involves handling large numbers of enemies, projectiles, and various game objects, creating significant challenges in performance optimization and resource management.
To tackle these challenges, the game utilizes several key development tools:

- Managers – Responsible for various gameplay systems, such as player stats, leveling, enemy waves, and more.
- Object Pooling – Efficiently manages enemies, projectiles, enemy drops, and other frequently instantiated objects to enhance performance.
  Also includes custom pooling scripts like the DropManager.cs and DropPool.cs for dynamic pooling of all drop types and easier design via the inspector.

A playable build is available on Itch.io: Link Here

Course credit:
- [Unity 2D Game - Kawaii Survivor - The Coolest Roguelike Ever](https://www.udemy.com/course/unity-2d-game-kawaii-survivor-the-coolest-roguelike-ever)
