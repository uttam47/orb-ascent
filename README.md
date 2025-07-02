# Orb Ascent

**Orb Ascent** is a minimalist puzzle-platformer built in Unity, where the core mechanic revolves around guiding small orbs through cleverly designed levels toward a teleportation point. The game emphasizes spatial reasoning, environmental interaction, and momentum-based puzzles, all wrapped in a serene, atmospheric presentation.

[![Get it on Google Play](https://play.google.com/intl/en_us/badges/static/images/badges/en_badge_web_generic.png)](https://play.google.com/store/apps/details?id=com.analyticalapproach.dominance)
<!-- Replace with actual Play Store URL -->

---

## 🎯 Features

- ⚙️ Custom physics-based orb mechanics  
- 🧠 Level designs focused on momentum, timing, and sequence  
- 🎨 Handcrafted UI with clean visuals and minimal distractions  
- 🎮 Mobile-friendly input system with tactile feedback  
- 🔄 Efficient scene management and data-driven level progression  

---

## 🧱 Architecture

- **Entity-Driven Design** using Unity’s component system  
- **State Machine** for orb behaviors and game flow  
- **Decoupled UI and Game Logic** for easier iteration and scaling  
- **Modular Level Loader** for quick prototyping and content addition  
- Clean separation between **Game Logic**, **Presentation**, and **Input Handling**

---

## 📂 Project Structure

```plaintext
Assets/
├── Scripts/
│   ├── Core/           # Base game loop, input handlers, global services
│   ├── Game/           # Orb, level elements, interaction logic
│   ├── UI/             # Screens, popups, transitions
│   └── Utilities/      # Extensions, helpers, constants
├── Art/                # Visual assets and animations
├── Audio/              # Sound effects and background music
└── Scenes/             # Level and UI scenes
```

## 📈 Future Goals
-📘 Tutorial System
A dedicated in-game tutorial system to onboard new players and introduce key mechanics gradually.

-🧱 Expanded Level Set
The current architecture includes internal level design tools, enabling faster and more structured creation of additional handcrafted levels.

-🌌 Procedural Level Generation (Long-Term)
Exploration of a replayable, procedurally-generated level mode to add variety and challenge beyond handcrafted content.
