# LDJAM 2025 Project
Unity 6.2 game project for Ludum Dare Jam.

---

## ⚙️ Setup
- **Unity Version**: 6.2  
- Clone repo  
- Open with Unity Hub  

---

## 📜 Rules
- ❌ No game code before jam start  
- ✅ Allowed: repo setup, docs, planning, pseudocode, wireframes  
- Feature branches: `feature/<system>-<name>`  
- `main` branch protected, `dev` used for integration  

---

## 🎮 Coding Conventions (C# / Unity)
**File & Folder Structure**
- `Scripts/Systems/<SystemName>` → e.g. `Scripts/Systems/Dialogue/DialogueManager.cs`  
- `Scripts/Managers/` → game-wide managers (`GameManager`, `AudioManager`)  
- `Prefabs/`, `Resources/`, `Scenes/` for assets  

**Naming**
- Classes → `PascalCase` (e.g. `DialogueManager`)  
- Methods → `PascalCase` (e.g. `StartDialogue()`)  
- Variables → `camelCase` (e.g. `playerGold`)  
- Constants/Enums → `ALL_CAPS` or `PascalCase` enums  
- Booleans → use `is`, `has`, `can` (`isActive`, `hasQuest`)  
- Interfaces → prefix with `I` (`IInteractable`)  

**Unity Specific**
- `Awake()` → internal init  
- `Start()` → references from other objects  
- `FixedUpdate()` → physics only  
- Prefer `private` + `[SerializeField]` over `public`  

---

## 💾 Git Workflow

**Branching**
- Always branch from `dev`  
- Format: `feature/<system>-<task>`  
  - Example: `feature/dialogue-typewriter`  
  - Example: `feature/trading-inventoryUI`

**Commit Messages**
- Format: `[System] Action in imperative mood`  
  - `[Dialogue] Add typewriter effect prototype`  
  - `[Trading] Implement basic buy/sell loop`  
  - `[Map] Fix town scene transition`  

**Pull Requests**
- PRs go into `dev`, not `main`  
- At least 1 reviewer approval required  
- Keep PRs small and focused  

**Commit Frequency**
- Commit often (every 30–60 min of progress)  
- Push at least once per work session  
- Avoid giant “end of day” commits  

**Do Not**
- Don’t commit broken code to `dev`  
- Don’t commit `.csproj`, `.sln`, or user settings  
- Don’t use vague messages like “fix stuff”  

---

## 👥 Team Quick Reference
- **Marco Pinho** → Time & Flags / Integration  
- **Arwen** → Dialogue  
- **Solomon** → Trading  
- **Roberto** → Map & Travel  
- **Josephine** → NPC Data  
- **Soumyajit** → Cutscenes  
- **miKaLj05on** → Audio & UI polish  