# Multiplayer Lobby + WebGL JS Interop Test Task

## Overview

This project implements a simple multiplayer lobby system using Mirror networking, combined with a shared gameplay element and WebGL browser integration via JavaScript interop.

---

## Setup & Run Instructions

### Unity Version

* Unity **6000.3.10f**

### Running Locally (Editor)

1. Open the project in Unity
2. Open the Lobby scene
3. Press **Play**
4. Use **Host** to start server + client
5. Connect additional clients via build or editor instances

---

### Running WebGL Build

1. Build project for WebGL
2. Serve using local server:

```bash
npx http-server -g
```

3. Open in browser:

```text
http://localhost:8080
```

or go to site itch.io: https://devsmanticore.itch.io/shared-counter
run game

---

### Multiplayer Testing

* Run **Host** in one instance
* Open WebGL build in browser as client
* Connect using same address

---

## Architecture Overview

### Lobby System

* Custom NetworkManager (`NetworkManagerLobby`)
* Players join lobby and are represented by `NetworkRoomPlayerLobby`
* Each player has:

  * Display name (SyncVar)
  * Ready state (SyncVar)

### Ready Flow

* Players toggle ready via Command
* Server validates all players
* Game starts only when minimum 2 players are ready

---

### Gameplay Layer

* After game start, a shared object (`SharedCounterGameplay`) is spawned
* Server-authoritative model:

  * Clients send Commands
  * Server updates state
  * SyncVar propagates to all clients

---

### Networking

* Built using Mirror
* Server-authoritative architecture
* Sync via:

  * SyncVars (player state, counter value)
  * Commands (client → server)
  * RPC (server → clients)

---

### WebGL JS Interop

Implemented via `.jslib` plugin:

#### Features:

* Save/load player name via `localStorage`
* Read player name from URL query (`?name=Alex`)
* Copy join link to clipboard
* Browser alert on player join

#### Bridge:

* C# uses `[DllImport("__Internal")]`
* JS functions defined in `browser.jslib`

---

## Technical Decisions

### Networking (Mirror)

* Chosen for simplicity and open-source flexibility
* Server-authoritative approach ensures consistency
* Avoided client authority for shared state

---

### Transport

* SimpleWebTransport used (WebSocket-based)
* Works in WebGL environments

---

### State Synchronization

* SyncVars for low-frequency state (name, ready, counter)
* Commands for user actions
* RPC for broadcast events (player join)

---

### JS Interop

* Implemented using native `.jslib` instead of Unity WebGL templates
* Allows direct access to browser APIs
* Added fallback for clipboard API for compatibility

---

## Known Issues / Improvements

* No scene transition (Lobby → Game scene)
* No reconnect logic 
* Clipboard API requires HTTPS  (working using my local server but don't worked on itch.io)
* UI is minimal and not production-ready
* Limited error handling in networking

### Possible Improvements

* Add proper game state machine
* Scene management via NetworkRoomManager
* Better UI/UX
* Add integration tests
* Optimize WebGL build size

---

## Time Log

| Phase          | Time |
| -------------- | ---- |
| Project Setup  | 0.4h |
| Mirror Lobby   | 1.5h |
| Shared Element | 0.5h |
| JS Interop     | 1.5h |
| WebGL + Docs   | 0.5h |
| **Total**      | ~4.4h|