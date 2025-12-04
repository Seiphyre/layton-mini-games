\# One-Stop Shop Clone — Architecture Overview









\## 1. Game Overview



\- \*\*Codename\*\*: One-Stop Shop Clone  

\- \*\*Inspiration\*\*: “One-Stop Shop” minigame from \*Professor Layton and the Miracle Mask\*  

\- \*\*Genre\*\*: Puzzle / Logic  

\- \*\*Platforms\*\*: Mobile (Android \& iOS)  

\- \*\*Engine\*\*: Unity 2022.3 LTS  

\- \*\*View\*\*: Top-down grid-based board  

\- \*\*Mode\*\*: Single-player  



\### Summary



The player interacts with:

\- A \*\*grid-based board\*\* (size varies per level, can contain holes),

\- A \*\*list of pieces\*\* to drag onto the board,

\- A \*\*starting piece\*\* already placed on the board,

\- \*\*Walls\*\* between tiles that block adjacency.



The objective is to place all pieces on the board so that a \*\*single, deterministic chain\*\* can be formed from the starting piece, traversing all placed pieces while following adjacency rules.







---







\## 2. Core Features



\### 1. Board System



  - Grid size varies per level.

  - Tiles can be active or inactive (holes allowed → donut-shaped layouts).

  - \*\*Walls between tiles\*\* restrict adjacency.

  - Board configuration is stored in a \*\*ScriptableObject\*\*.



\### 2. Pieces System



\- Pieces occupy multiple tiles (\*\*Tetris-like shapes\*\*).

\- Each piece has:

  - a \*\*type\*\* (category)

  - a \*\*color\*\*

  - a \*\*shape\*\* pattern across tiles.



\### 3. UI / Drag \& Drop System



\- Drag pieces from UI list → onto board.

\- Pieces can be removed (back to UI list) or repositioned.

\- The starting piece is \*\*immutable\*\* and cannot be moved.

\- System designed for \*\*touch interaction\*\* (mobile-friendly).



\- List UI using a \*\*custom ListPresenter / UIList component\*\*.

\- Drag feedback (valid/invalid placement).

\- Mobile UX with intuitive cancel/reposition gestures.

\- Clean and scalable UI architecture suitable for future games.





\### 4. Chain Validation System



Starting from the initial piece:

\- Build a chain by selecting the \*\*only valid adjacent piece\*\*.

\- Valid adjacency requires:

  - The neighbor is not separated by a wall,

  - AND (same color OR same type).

\- Validation rules:

  - If at any step \*two possible\* next pieces exist → failure.

  - If \*no possible\* next piece → failure.

  - If exactly \*one choice\* at all steps → success.

  - The chain must visit \*\*all pieces\*\*.



\### 5. Level System



Levels stored in ScriptableObjects contain:

\- Board size \& tile activation (holes),

\- Walls between tiles,

\- Starting piece placement,

\- List of available pieces (shape + type + color).



\### 6. Level Editor



\- Custom Unity Editor tool to:

  - Edit tile activation,

  - Create walls,

  - Place starting piece,

  - Configure pieces.

\- Current tool exists; will be improved over time.



\### 7. Reusable Toolkit



Goal: build \*\*modular, reusable components\*\* for future Unity projects:

\- Generic grid/board visualizer,

\- Flexible drag \& drop components,

\- UI Toolkit-like components (UIList, Presenters),

\- Modular validation/graph logic.







---







\## 3. Tech Choices



\- \*\*Unity Version\*\*: 2022.3 LTS  

\- \*\*Input System\*\*: (to be determined — Old or New Input System)  

\- \*\*Scene Management\*\*: Single main game scene + ScriptableObject-driven level loading  

\- \*\*UI\*\*: uGUI for now, with custom reusable UI components  

\- \*\*Physics\*\*: No physics; logic runs purely on grid adjacency  

\- \*\*Data Format\*\*:

&nbsp; - ScriptableObjects for board data, piece shapes, level definitions  

\- \*\*Architecture Patterns\*\*:

&nbsp; - Clear separation of:

&nbsp;   - \*\*Data\*\* (ScriptableObjects),

&nbsp;   - \*\*Logic\*\* (systems/services),

&nbsp;   - \*\*Presentation\*\* (UI \& board renderers)

&nbsp; - Modular components, composable behaviors, decoupled systems.



---



\## 4. Project Vision



\### What is done



\- UI System

&nbsp; - Complete ListPresenter. Generic list controller that binds a collection of data (List<T>) to a set of UI items (ListItem).

&nbsp; - Complete ListItem.

&nbsp; - Complete Draggable.

&nbsp; - Complete Dropzone.

&nbsp; - Complete Pagination.

\- Editor

&nbsp; - Basic editor as an Editor Component linked to BoardData class. 



\### Short-term goals



\- Board System (Data + Runtime + View minimal)

&nbsp; - Complete BoardData class.

&nbsp; - Complete BoardController class

&nbsp; - Create \& Complete Board class.

&nbsp; - Adapter BoardView pour afficher correctement la grille (avec trous).

&nbsp; - Complete Tile class.

&nbsp; - Complete Wall class.



\- Piece System (Data + Runtime minimal)

&nbsp; - Définir PieceData clairement (type + color + shape).

&nbsp; - Représentation interne des shapes (from ShapeData).



\- UI System (Generic)

&nbsp; - Stabiliser ListPresenter, ListItem, Draggable, DropZone.



\- Piece Placement Logic

&nbsp; - Contrôle de validité de placement sur le board.

&nbsp; - Déplacement / retour en liste.



\- Chain Validation System

&nbsp; - Implémenter et brancher le validateur.



\- Level Loader

&nbsp; - Connecter tout ensemble dans la MainScene.



\- Editor Improvements

&nbsp; - Rendre l’édition de niveaux confortable.



---



\## 4. Data \& Flow Between System



Game Flow (simplifié) :



\- LevelLoader:



  - Reads BoardData + PieceData list.

  - Builds Board object.

  - Initializes BoardView.

  - Creates Piece objects.

  - Populates ListPresenter for UI.



\- Drag \& Drop:



  - Player drags a PieceView from list to board.

  - Drop on board triggers game-specific logic:

    - Ask BoardSystem + PieceSystem:

      - “Is this placement valid (fits in shape, no overlap, on active tiles) ?”

    - If yes → place piece, update Piece state.

    - If no → revert (snap back to list).



\- Validation:



  - When all pieces are placed (or when player presses a button “Check”),

    → ChainValidator.Validate(...) is called.

  - Displays success or failure via UI.



---



\## 6. Current Project Structure



\- Data/

&nbsp; - BoardData/

&nbsp;   - BoardData.cs

&nbsp;   - BoardData.asset

&nbsp; - ColorData/

&nbsp;   - ColorInfo.cs

&nbsp;   - ColorInfo.asset

&nbsp; - TypeData/

&nbsp;   - TypeInfo.cs

&nbsp;   - TypeInfo.asset

&nbsp; - ShapeData/

&nbsp;   - ShapeInfo.cs

&nbsp;   - ShapeInfo.asset

&nbsp; - ShopItemData/

&nbsp;   - ShopItemData.cs

&nbsp;   - ShopItemData.asset



\- Scenes/

&nbsp; - MainScene.scene



\- Scripts/

&nbsp; - BoardSystem/

&nbsp;   - Editor/

&nbsp;     - BoardDataEditor.cs

      - BoardViewEditor.cs

    - BoardView.cs

    - Tile.cs

    - TileType.cs

    - Wall.cs

    - WallDirection.cs

&nbsp; - ChainValidationSystem/

&nbsp; - LevelSystem/

&nbsp; - UISystem/

&nbsp;   - UIElement.cs

&nbsp;   - DataPresenter.cs

&nbsp;   - InventoryItem.cs

&nbsp;   - ShopItem.cs

&nbsp;   - Draggable.cs

&nbsp;   - DropZone.cs

&nbsp;   - ShopItemDropZone.cs

&nbsp;   - ListItem.cs

&nbsp;   - ListPresenter.cs

&nbsp;   - ShopItemList.cs

&nbsp;   - Pagination.cs

&nbsp;   - PaginationControls.cs

&nbsp;   - ImageCollider.cs

&nbsp; - Utils/

&nbsp;   - ComponentUtils.cs

&nbsp;   - ObjectUtils.cs

&nbsp;   - ValidationUtils.cs

