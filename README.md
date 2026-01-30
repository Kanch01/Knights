# Knights (Unity / C#)

A top-down action survival game where you play as a knight defending your city from an invading kingdom. Defeat **20 enemies** to win — and once you’ve won, you can keep playing to survive as long as you can.

> **Demo:** https://northwestern.hosted.panopto.com/Panopto/Pages/Viewer.aspx?id=00b931d9-39ae-4ecb-99ac-b3e200589f80

---

## Gameplay Summary

- You are a **knight** defending your small village from invading lance weilding knights.
- **Win condition:** defeat **20 enemies** (you can continue playing after winning).
- **Lose condition:** you have **5 lives**; when you reach **0**, the game ends and the player disappears.
- Enemies **spawn close to the player** and will **attack when in range**.
- As more enemies spawn, some may require **multiple hits** to defeat.

---

## Controls

| Action | Key |
|---|---|
| Move | **W / A / S / D** |
| Attack | **Right Enter / Return** |
| Dash | **Right Shift** |

### Dash Mechanics
- While dashing, the player is **invincible** and can **phase through enemies**.
- **Invincibility ends immediately** when the dash ends.

---

## Objects in the World

### Core Gameplay Objects
- **Player (Knight)**
- **Enemies**

### Environment / Level Objects
- **Cliffs** (solid obstacles)
- **Stairs** (used to climb onto higher ground)

### Decoration (Collidable)
- **Houses**
- **Trees**
- **Bushes**
- **Sheep**

---

## Collision & Interaction Rules

- The player **collides with**:
  - **Cliffs**
  - **Decoration** (houses, trees, bushes, sheep)
- The player can use **stairs** to move onto **higher ground**.
- The player can defeat enemies via **attacks**.
- The player can dash through enemies while invincible.

---

## Enemy Behavior

- Enemies **spawn near the player**.
- Enemies have a **medium attack range**.
- When the player is **within range**, enemies will **attack**.
- Enemies may have **more than 1 health**, requiring multiple hits to kill.

---

## Scoring & Feedback

- **+1 score** for each enemy defeated.
- **Enemy death sound** plays when an enemy dies.
- When the player gets hit:
  - **Lose 1 life**
  - A **hit sound** plays
  - The player briefly **turns red**

---

## End States

The game does **not** immediately exit when an end condition is met:

- **Win:** score reaches **20**
  - You are considered victorious, but gameplay can continue.
- **Lose:** lives reach **0**
  - The player disappears when the game is over.

---

## Tech Stack

- **Engine:** Unity
- **Language:** C#

---

## How to Run

Clone the repo and play it directly on Unity.

