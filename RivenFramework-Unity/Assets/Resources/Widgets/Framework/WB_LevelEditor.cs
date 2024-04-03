//===================== (Neverway 2024) Written by Liz M. =====================
//
// Purpose: This script serves as the main controller for the level editor in
//  Cartographer. It manages various aspects such as tool modes, cursor
//  positioning, hotbar management, layer visibility, and user input processing
//  to facilitate the creation and modification of game levels.
//
// Notes:
// - The script includes functionality for creating new maps, saving and loading
//      maps, testing maps, placing and erasing tiles/assets, inspecting assets,
//      picking tiles/assets, toggling inventory visibility, and adjusting hotbar
//      and zoom settings.
// - It also features UI elements such as buttons, text fields, and sprites for
//      intuitive user interaction.
// - The script leverages various components from the Unity game engine such as
//      Tilemaps, TextMeshPro, UI elements, Cameras, and EventSystem.
// - Additionally, it incorporates custom components and functionalities specific
//      to the Neverway game, such as unique asset IDs and runtime data inspection.
//
//=============================================================================


using System;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Tilemaps;
using UnityEngine.UI;

public class WB_LevelEditor : MonoBehaviour
{
    //=-----------------=
    // Public Variables
    //=-----------------=


    //=-----------------=
    // Private Variables
    //=-----------------=
    // Start/Stop test
    private Pawn editorPlayer;
    private bool isTesting;

    // Tool mode
    private string currentTool = "paint";
    private int currentPaintMode = 3;
    public bool isShapePainting;
    private Vector3 placeStartPos;
    private Vector3 placeEndPos;
    private Vector3 eraseStartPos;
    private Vector3 eraseEndPos;

    // Tool cursor
    private Vector3 cursorPos;
    private float cursorOffset;
    private Vector2 viewZoomRange = new Vector2(2, 32);

    // Hotbar
    private string[] hotBarTileID = { "", "", "", "", "", "", "", "", "", "" };
    private int currentHotBarIndex;

    // Layers
    private Tilemap currentTilemap;
    private int currentLayer;
    private int currentSublayer;
    private bool[] IsLayerVisible = new[] { true, true, true };
    private int sublayerCount;

    // Inventory
    private bool inventoryOpen;


    //=-----------------=
    // Reference Variables
    //=-----------------=
    private LevelManager levelManager;
    private Camera viewCamera;
    private WB_LevelEditor_Inspector inspector;

    // Serialized Fields
    [SerializeField] private GameMode testingGamemode;
    [SerializeField] private TMP_Text topbarTitle;
    //[SerializeField] private TMP_Text sidebarTitle; // No longer needed since sidebar is now seperated
    [SerializeField] private Button[] fileButtons, viewButtons, helpButtons, filebarButtons, toolbarButtons, hotbarButtons, layerButtons, sublayerButtons, layerVisibilityButtons;
    [SerializeField] private Button clearHotbarButton, toggleInventoryButton;
    [SerializeField] private GameObject tileCursor, inspectionIndicator, shapePainterIndicator, shapePainterIndicatorPrefab;
    [SerializeField] private Sprite[] paintToolSprites, visibilitySprites, lightIndicatorSprites;
    [SerializeField] private GameObject inventory;
    [SerializeField] private GameObject[] layerGroups;
    [SerializeField] private Image shapePainterLightIndicator, networkOnlineLightIndicator;
    [SerializeField] private Image[] layerLightIndicator;
    [SerializeField] private TMP_Text[] coordinateIndicators;
    

    //=-----------------=
    // Mono Functions
    //=-----------------=
    /// <summary>
    /// Initializes the editor components and event listeners.
    /// </summary>
    private void Start()
    {
        // Initialize components
        InitializeComponents();

        // Initialize event listeners for file, help, filebar, toolbar, hotbar, layer, sublayer, and layer visibility buttons
        InitializeEventListeners();

        // Initialize editor player if not already set
        InitializeEditorPlayer();
        
        // Destroy the initial HUD that's created by the Level Blueprint
        Destroy(GameInstance.GetWidget("WB_HUD"));
    }

    /// <summary>
    /// Updates the editor UI elements and processes user input.
    /// </summary>
    private void Update()
    {
        levelManager = FindObjectOfType<LevelManager>();
        // Update UI elements
        UpdateUIElements();

        // Process user input
        ProcessUserInput();
    }
    

    //=-----------------=
    // Internal Functions
    //=-----------------=
    /// <summary>
    /// Initializes the editor components.
    /// </summary>
    private void InitializeComponents()
    {
        levelManager = FindObjectOfType<LevelManager>();
        viewCamera = FindObjectOfType<Camera>(false);
        inspector = FindObjectOfType<WB_LevelEditor_Inspector>(true);
        currentTilemap = levelManager.tilemaps[0];
        sublayerCount = sublayerButtons.Length;
    }

    /// <summary>
    /// Initializes event listeners for buttons.
    /// </summary>
    private void InitializeEventListeners()
    {
        
        // File buttons
        fileButtons[0].onClick.AddListener(NewMap);
        if (!levelManager) levelManager = FindObjectOfType<LevelManager>();
        fileButtons[1].onClick.AddListener(() => { levelManager.ModifyLevelFile("Load"); });
        fileButtons[2].onClick.AddListener(SaveCurrentMap);
        if (!levelManager) levelManager = FindObjectOfType<LevelManager>();
        fileButtons[3].onClick.AddListener(() => { levelManager.ModifyLevelFile("Save"); });
        fileButtons[4].onClick.AddListener(Application.Quit);

        // Help buttons
        foreach (var helpButton in helpButtons)
        {
            helpButton.onClick.AddListener(() => { Application.OpenURL("https://neverway.github.io/404.html"); });
        }

        // Filebar buttons
        filebarButtons[0].onClick.AddListener(() => { levelManager.ModifyLevelFile("Load"); });
        filebarButtons[1].onClick.AddListener(SaveCurrentMap);
        filebarButtons[2].onClick.AddListener(() => { StartOrStopTest(true); });
        filebarButtons[3].onClick.AddListener(() => { StartOrStopTest(false); });

        // Toolbar buttons
        toolbarButtons[0].onClick.AddListener(() => { currentTool = "paint"; });
        toolbarButtons[1].onClick.AddListener(() => { currentTool = "inspect"; });

        // Hotbar buttons
        for (int i = 0; i < hotbarButtons.Length; i++)
        {
            int index = i; // Capturing the variable in the lambda expression
            hotbarButtons[i].onClick.AddListener(() => { currentHotBarIndex = index; });
        }
        clearHotbarButton.onClick.AddListener(() => { Array.Clear(hotBarTileID, 0, hotBarTileID.Length); });
        toggleInventoryButton.onClick.AddListener(ToggleInventory);

        // Layer buttons
        for (var i = 0; i < layerButtons.Length; i++)
        {
            int index = i; // Capturing the variable in the lambda expression
            layerButtons[i].onClick.AddListener(() => { currentLayer = index; });
        }

        // Sublayer buttons
        for (int i = 0; i < sublayerButtons.Length; i++)
        {
            int index = i; // Capturing the variable in the lambda expression
            sublayerButtons[i].onClick.AddListener(() => { currentSublayer = index; });
        }

        // Layer visibility buttons
        for (int i = 0; i < layerVisibilityButtons.Length; i++)
        {
            int index = i; // Capturing the variable in the lambda expression
            layerVisibilityButtons[i].onClick.AddListener(() => { IsLayerVisible[index] = !IsLayerVisible[index]; });
        }
    }

    /// <summary>
    /// Initializes the editor player if not already set.
    /// </summary>
    private void InitializeEditorPlayer()
    {
        if (!editorPlayer)
        {
            editorPlayer = FindObjectOfType<GameInstance>().localPlayerCharacter;
        }
    }

    /// <summary>
    /// Updates the UI elements in the editor.
    /// </summary>
    private void UpdateUIElements()
    {
        UpdateBarTitles();
        UpdateToolImages();
        UpdateHotBarImages();
        UpdateCurrentPaintMode();
        UpdateLayerSelection();
        UpdateLayerVisibility();
        UpdateLightIndicatorImages();
    }
    
    
    /// <summary>
    /// Resets the editor to create a new map.
    /// </summary>
    private void NewMap()
    {
        // Clear the file path
        levelManager.filePath = "";

        // Clear all tiles in tilemaps
        foreach (var tilemap in levelManager.tilemaps)
        {
            tilemap.ClearAllTiles();
        }

        // Clear all assets in the assets root
        for (int i = 0; i < levelManager.assetsRoot.transform.childCount; i++)
        {
            Destroy(levelManager.assetsRoot.transform.GetChild(i).gameObject);
        }
    }

    /// <summary>
    /// Saves the current map either by overwriting the existing file or prompting for a new file.
    /// </summary>
    private void SaveCurrentMap()
    {
        if (!string.IsNullOrEmpty(levelManager.filePath))
        {
            levelManager.SaveLevel(levelManager.filePath);
        }
        else
        {
            levelManager.ModifyLevelFile("Save");
        }
    }

    /// <summary>
    /// Starts or stops testing the current map.
    /// </summary>
    /// <param name="_start">True to start testing, false to stop testing.</param>
    private void StartOrStopTest(bool _start)
    {
        // Initialize editor player if not already found
        if (!editorPlayer)
        {
            editorPlayer = FindObjectOfType<GameInstance>().localPlayerCharacter;
        }

        // Update testing state and player visibility
        isTesting = _start;
        editorPlayer.gameObject.SetActive(!_start);
        filebarButtons[2].gameObject.SetActive(!_start);
        filebarButtons[3].gameObject.SetActive(_start);

        // Create or destroy player character based on testing state
        if (_start)
        {
            SaveCurrentMap();
            FindObjectOfType<GameInstance>().CreateNewPlayerCharacter(testingGamemode, true, true);
            transform.GetChild(0).gameObject.SetActive(false);
            FindObjectOfType<GameInstance>().UI_ShowHUD();
        }
        else
        {
            if (!string.IsNullOrEmpty(levelManager.filePath))
            {
                levelManager.LoadLevel(levelManager.filePath);
            }
            Destroy(FindObjectOfType<GameInstance>().localPlayerCharacter.gameObject);
            FindObjectOfType<GameInstance>().localPlayerCharacter = editorPlayer;
            editorPlayer.gameObject.SetActive(true);
            transform.GetChild(0).gameObject.SetActive(true);
            Destroy(GameInstance.GetWidget("WB_HUD"));
        }
    }

    

    /// <summary>
    /// Updates the titles in the top bar and sidebar.
    /// </summary>
    private void UpdateBarTitles()
    {
        // Update top-bar title
        string topBarTitleText = GetTopBarTitleText();
        topbarTitle.text = topBarTitleText;

        // Update sidebar title
        //string sidebarTitleText = GetSidebarTitleText();
        //sidebarTitle.text = sidebarTitleText;
    }

    /// <summary>
    /// Retrieves the text for the top bar title.
    /// </summary>
    /// <returns>The text for the top bar title.</returns>
    private string GetTopBarTitleText()
    {
        if (string.IsNullOrEmpty(levelManager.filePath))
            return $"New Map - {Application.version}";
    
        string fileName = GetFileNameFromPath(levelManager.filePath);
        return $"{fileName} - {Application.version}";
    }

    /// <summary>
    /// Extracts the file name from the given file path.
    /// </summary>
    /// <param name="_filePath">The file path.</param>
    /// <returns>The extracted file name.</returns>
    private string GetFileNameFromPath(string _filePath)
    {
        int lastIndex = _filePath.LastIndexOfAny(new char[] { '/', '\\' });
        return _filePath.Substring(lastIndex + 1);
    }

    /*/// <summary>
    /// Retrieves the text for the sidebar title based on the current tool.
    /// </summary>
    /// <returns>The text for the sidebar title.</returns>
    private string GetSidebarTitleText()
    {
        return currentTool == "paint" ? "Layers" : "Inspector";
    }*/


    /// <summary>
    /// Updates the images of the little lightbulb indicators on the hotbar
    /// </summary>
    private void UpdateLightIndicatorImages()
    {
        if (isShapePainting) shapePainterLightIndicator.sprite = lightIndicatorSprites[1];
        else shapePainterLightIndicator.sprite = lightIndicatorSprites[0];

        for (int i = 0; i < layerLightIndicator.Length; i++)
        {
            layerLightIndicator[i].sprite = lightIndicatorSprites[0];
        }

        layerLightIndicator[currentLayer].sprite = lightIndicatorSprites[1];
    }
    
    /// <summary>
    /// Updates the images and properties of tools in the toolbar based on the current selected tool.
    /// </summary>
    private void UpdateToolImages()
    {
        // Retrieve images for paint and inspect tools from toolbar buttons
        var paintImage = toolbarButtons[0].transform.GetChild(0).GetComponent<Image>();
        var inspectImage = toolbarButtons[1].transform.GetChild(0).GetComponent<Image>();

        // Set tool icon colors based on the current selected tool
        SetToolIconColors(paintImage, inspectImage);

        // Update paint image sprite based on the current paint mode
        paintImage.sprite = paintToolSprites[currentPaintMode];
        
        // Hide the inspection indicator when not in inspect mode
        if (currentTool != "inspect") inspectionIndicator.SetActive(false);
    }

    /// <summary>
    /// Sets the tool icon colors based on the current selected tool.
    /// </summary>
    private void SetToolIconColors(Image _paintImage, Image _inspectImage)
    {
        _paintImage.color = currentTool == "paint" ? new Color(1, 1, 1, 1) : new Color(1, 1, 1, 0.25f);
        _inspectImage.color = currentTool == "inspect" ? new Color(1, 1, 1, 1) : new Color(1, 1, 1, 0.25f);

        // Set cursor color and offset based on the current selected tool and tile type
        var cursorColor = GetCursorColor();
        var cursorOffset = GetCursorOffset();

        tileCursor.GetComponent<SpriteRenderer>().color = cursorColor;
        if (GetCursorOffset() > 0)
        {
            tileCursor.transform.position = new Vector3((float)MathF.Floor(cursorPos.x / 1) * 1 + cursorOffset, (float)MathF.Floor(cursorPos.y / 1) * 1 + cursorOffset, 0);
        }
        else
        {
            tileCursor.transform.position = new Vector3((float)MathF.Round(cursorPos.x / 1) * 1 + cursorOffset, (float)MathF.Round(cursorPos.y / 1) * 1 + cursorOffset, 0);
        }

        coordinateIndicators[0].text = $"X: {tileCursor.transform.position.x}";
        coordinateIndicators[1].text = $"Y: {tileCursor.transform.position.y}";
        coordinateIndicators[2].text = $"Z: {tileCursor.transform.position.z}";
    }

    /// <summary>
    /// Gets the cursor color based on the current selected tool and tile type.
    /// </summary>
    private Color GetCursorColor()
    {
        if (currentTool == "paint")
        {
            if (levelManager.GetTileFromMemory(hotBarTileID[currentHotBarIndex]) && !hotBarTileID[currentHotBarIndex].Contains("Collision"))
                return new Color(0.17f, 0.65f, 0.27f, 1f);
            else if (levelManager.GetAssetFromMemory(hotBarTileID[currentHotBarIndex]))
                return new Color(0.25f, 0.41f, 0.72f, 1f);
            else if (levelManager.GetTileFromMemory(hotBarTileID[currentHotBarIndex]) && hotBarTileID[currentHotBarIndex].Contains("Collision"))
                return new Color(0.55f, 0.22f, 0.22f, 1f);
            else
                return new Color(0.5f, 0.5f, 0.5f, 0.5f); // Indicates a missing/null paint mode
        }
        else // currentTool == "inspect"
        {
            return new Color(0.25f, 0.41f, 0.72f, 1f); // Default cursor color for inspect tool
        }
    }

    /// <summary>
    /// Gets the cursor offset based on the current selected tool and tile type.
    /// </summary>
    private float GetCursorOffset()
    {
        if (currentTool == "paint")
        {
            return levelManager.GetTileFromMemory(hotBarTileID[currentHotBarIndex]) ? 0.5f : 0f;
        }
        return 0f; // Default cursor offset
    }


    /// <summary>
    /// Updates the images and selection indicators in the hotbar.
    /// </summary>
    private void UpdateHotBarImages()
    {
        for (var i = 0; i < hotbarButtons.Length; i++)
        {
            // Set sprite for each hotBar tile
            Image hotBarPreview = hotbarButtons[i].transform.GetChild(0).GetComponent<Image>();
            if (levelManager.GetTileFromMemory(hotBarTileID[i]))
            {
                hotBarPreview.enabled = true;
                hotBarPreview.sprite = levelManager.GetTileFromMemory(hotBarTileID[i]).sprite;
            }
            else if (levelManager.GetAssetFromMemory(hotBarTileID[i]))
            {
                hotBarPreview.enabled = true;
                hotBarPreview.sprite = levelManager.GetAssetFromMemory(hotBarTileID[i]).GetComponent<SpriteRenderer>().sprite;
            }
            else
            {
                hotBarPreview.enabled = false;
            }

            // Show hotBar selection indicator
            var hotBarSelection = hotbarButtons[i].transform.GetChild(1).gameObject;
            hotBarSelection.SetActive(i == currentHotBarIndex);
        }
    }
    
    /// <summary>
    /// Updates the current paint mode according to the currently selected hotbar item
    /// </summary>
    private void UpdateCurrentPaintMode()
    {
        if (levelManager.GetTileFromMemory(hotBarTileID[currentHotBarIndex]))
        {
            if (hotBarTileID[currentHotBarIndex].ToLower().Contains("collision")) currentPaintMode = 2;
            else currentPaintMode = 0;
        }
        else if (levelManager.GetAssetFromMemory(hotBarTileID[currentHotBarIndex]))
        {
            currentPaintMode = 1;
        }
        else
        {
            currentPaintMode = 3;
        }
        
    }
    
    /// <summary>
    /// Updates the layer selection menu and visibility.
    /// </summary>
    private void UpdateLayerSelection()
    {
        // Show/hide inspector based on tool mode
        inspector.gameObject.SetActive(currentTool == "inspect");

        // Hide the menu if not in paint tool mode
        if (currentTool != "paint")
        {
            layerButtons[0].transform.parent.parent.gameObject.SetActive(false);
            return;
        }

        // Show the menu if in paint tool mode
        layerButtons[0].transform.parent.parent.gameObject.SetActive(true);

        // Set the layer text colors
        foreach (var button in layerButtons)
        {
            button.transform.GetChild(0).GetComponent<TMP_Text>().color = new Color(1, 1, 1, 0.15f);
        }
        layerButtons[currentLayer].transform.GetChild(0).GetComponent<TMP_Text>().color = new Color(1, 1, 1, 1);

        // Set the sublayer text and active state
        foreach (var button in sublayerButtons)
        {
            button.transform.GetChild(0).GetComponent<TMP_Text>().color = new Color(1, 1, 1, 0.15f);
        }
        sublayerButtons[currentSublayer].transform.GetChild(0).GetComponent<TMP_Text>().color = new Color(1, 1, 1, 1);

        // Set the paint mode
        switch (currentPaintMode)
        {
            case 0:
                // Select the ground layer for each tile map
                currentTilemap = levelManager.tilemaps[currentLayer * sublayerCount + currentSublayer];
                break;
            case 2:
                // Select the collision layer for each tile map
                currentTilemap = levelManager.tilemaps[currentLayer * sublayerCount + (sublayerCount - 1)];
                break;
        }
    }

    /// <summary>
    /// Updates the visibility of layers based on the IsLayerVisible array.
    /// </summary>
    private void UpdateLayerVisibility()
    {
        for (int i = 0; i < IsLayerVisible.Length; i++)
        {
            switch (IsLayerVisible[i])
            {
                case true:
                    layerVisibilityButtons[i].transform.GetChild(0).GetComponent<Image>().sprite = visibilitySprites[0];
                    layerGroups[i].gameObject.SetActive(true);
                    break;
                case false:
                    layerVisibilityButtons[i].transform.GetChild(0).GetComponent<Image>().sprite = visibilitySprites[1];
                    layerGroups[i].gameObject.SetActive(false);
                    break;
            }
        }
    }

    /// <summary>
    /// Processes user input for various actions in the editor.
    /// </summary>
    private void ProcessUserInput()
    {
        EnsureViewCameraAssigned();
        UpdateCursorPosition();

        if (isTesting)
            return;

        ProcessMouseActions();
        ProcessKeyboardActions();
        ProcessScrollWheelActions();
    }

    /// <summary>
    /// Ensures that the view camera is assigned.
    /// </summary>
    private void EnsureViewCameraAssigned()
    {
        if (viewCamera)
            return;

        Debug.Log("The view camera could not be found! Attempting to re-assign view camera...");
        viewCamera = FindObjectOfType<Camera>();
    }

    /// <summary>
    /// Updates the cursor position.
    /// </summary>
    private void UpdateCursorPosition()
    {
        if (!viewCamera)
        {
            return;
        }
        cursorPos = viewCamera.ScreenToWorldPoint(Input.mousePosition);
    }

    /// <summary>
    /// Processes mouse actions such as placing, inspecting, erasing, and picking.
    /// </summary>
    private void ProcessMouseActions()
    {
        if (EventSystem.current.IsPointerOverGameObject() || !IsLayerVisible[currentLayer])
            return;

        if (currentTool == "paint")
        {
            if (!isShapePainting)
            {
                if (Input.GetMouseButton(0) && currentPaintMode != 1)
                    Place();
                else if (Input.GetMouseButton(1) && currentPaintMode != 1)
                    Erase();
                if (Input.GetMouseButtonDown(0) && currentPaintMode == 1)
                    Place();
                else if (Input.GetMouseButtonDown(1) && currentPaintMode == 1)
                    Erase();
                else if (Input.GetMouseButtonDown(2))
                    Pick();
            }
            else HandleShapePainting();
        }
        else if (Input.GetMouseButton(0))
            Inspect();
    }

    private void HandleShapePainting()
    {
        if (Input.GetMouseButtonDown(0))
        {
            placeStartPos = cursorPos;
        }
        else if (Input.GetMouseButtonUp(0))
        {
            placeEndPos = cursorPos;
            PaintShape(placeStartPos, placeEndPos);
            HideBoxIndicator();
        }
        else if (Input.GetMouseButtonDown(1))
        {
            eraseStartPos = cursorPos;
        }
        else if (Input.GetMouseButtonUp(1))
        {
            eraseEndPos = cursorPos;
            PaintShape(eraseStartPos, eraseEndPos, true);
            HideBoxIndicator();
        }
        else if (Input.GetMouseButton(2))
            Pick();
        if (Input.GetMouseButton(0))
        {
            // Update box indicator
            UpdateBoxIndicator(placeStartPos, cursorPos, new Color(0f,1f,0.2f,0.15f));
        }
        if (Input.GetMouseButton(1))
        {
            // Update box indicator
            UpdateBoxIndicator(eraseStartPos, cursorPos, new Color(1f,0.2f,0f,0.15f));
        }
    }
    private void UpdateBoxIndicator(Vector3 startPos, Vector3 endPos, Color boxColor)
    {
        // If box indicator doesn't exist, create one
        if (shapePainterIndicator == null)
        {
            shapePainterIndicator = Instantiate(shapePainterIndicatorPrefab);
        }

        // Calculate the center position of the box
        Vector3 center = (startPos + endPos) * 0.5f;

        // Calculate the size of the box
        float width = Mathf.Abs(endPos.x - startPos.x);
        float height = Mathf.Abs(endPos.y - startPos.y);

        // Set the position and size of the box indicator
        shapePainterIndicator.transform.position = new Vector3(center.x, center.y, 0);
        shapePainterIndicator.transform.localScale = new Vector3(width, height, 1f);
        shapePainterIndicator.GetComponent<SpriteRenderer>().color = boxColor;
    }

    // Call this method when shape painting ends to hide the box indicator
    private void HideBoxIndicator()
    {
        if (shapePainterIndicator != null)
        {
            Destroy(shapePainterIndicator);
            shapePainterIndicator = null;
        }
    }

    /// <summary>
    /// Processes keyboard actions such as saving, loading, undoing, redoing, and toggling inventory.
    /// </summary>
    private void ProcessKeyboardActions()
    {
        if (Input.GetKeyDown(KeyCode.LeftShift) && currentTool == "paint") isShapePainting = !isShapePainting;
        if (Input.GetKey(KeyCode.LeftControl) && Input.GetKeyDown(KeyCode.S)) SaveCurrentMap();
        else if (Input.GetKey(KeyCode.LeftControl) && Input.GetKeyDown(KeyCode.O)) levelManager.ModifyLevelFile("Load");
        /*else if (Input.GetKey(KeyCode.LeftControl) && Input.GetKey(KeyCode.LeftShift) && Input.GetKeyDown(KeyCode.Z))
            Redo();
        else if (Input.GetKey(KeyCode.LeftControl) && Input.GetKeyDown(KeyCode.Z))
            Undo();*/
        if (Input.GetKeyDown(KeyCode.Tab) && currentTool == "paint") ToggleInventory();
    }

    /// <summary>
    /// Processes scroll wheel actions for adjusting hotbar and zoom.
    /// </summary>
    private void ProcessScrollWheelActions()
    {
        float scrollWheelInput = Input.GetAxis("Mouse ScrollWheel");
        if (scrollWheelInput > 0 && !inventoryOpen)
            AdjustHotBarAndZoom(-1);
        else if (scrollWheelInput < 0 && !inventoryOpen)
            AdjustHotBarAndZoom(1);
    }

    /// <summary>
    /// Adjusts the hotbar and zoom based on the scroll wheel input.
    /// </summary>
    /// <param name="_direction">The direction of adjustment.</param>
    private void AdjustHotBarAndZoom(int _direction)
    {
        if (Input.GetKey(KeyCode.LeftControl))
        {
            if (_direction < 0 && viewCamera.orthographicSize > viewZoomRange.x)
            {
                viewCamera.orthographicSize--;
                if (!editorPlayer) editorPlayer = FindObjectOfType<GameInstance>().localPlayerCharacter;
                editorPlayer.currentState.movementSpeed--;
            }
            else if (_direction > 0 && viewCamera.orthographicSize < viewZoomRange.y)
            {
                viewCamera.orthographicSize++;
                if (!editorPlayer) editorPlayer = FindObjectOfType<GameInstance>().localPlayerCharacter;
                editorPlayer.currentState.movementSpeed++;
            }
        }
        else
        {
            currentHotBarIndex = (currentHotBarIndex + hotBarTileID.Length - _direction) % hotBarTileID.Length;
        }
    }



    /// <summary>
    /// Handles the placement of tiles or assets based on the current paint mode.
    /// </summary>
    private void Place()
    {
        switch (currentPaintMode)
        {
            // Placing a tile or an asset
            case 0:
            case 2:
                PlaceTile();
                break;
            // Placing an asset
            case 1:
                PlaceAsset();
                break;
        }
    }  
    
    /// <summary>
    /// Paints a shape between the given start and end positions.
    /// </summary>
    /// <param name="start">The starting position of the shape.</param>
    /// <param name="end">The ending position of the shape.</param>
    private void PaintShape(Vector3 start, Vector3 end, bool erasing = false)
    {
        // Determine the minimum and maximum positions to iterate over
        int minX = Mathf.FloorToInt(Mathf.Min(start.x, end.x));
        int maxX = Mathf.FloorToInt(Mathf.Max(start.x, end.x));
        int minY = Mathf.FloorToInt(Mathf.Min(start.y, end.y));
        int maxY = Mathf.FloorToInt(Mathf.Max(start.y, end.y));

        // Paint each grid position within the shape
        for (int x = minX; x <= maxX; x++)
        {
            for (int y = minY; y <= maxY; y++)
            {
                // Place tile or asset at the current grid position
                Vector3Int position = new Vector3Int(x, y, 0);
                if (currentTool == "paint" && IsLayerVisible[currentLayer])
                    if (!erasing)
                    {
                        PlaceTileOrAsset(position);
                    }
                    else
                    {
                        EraseTileOrAsset(position);
                    }
                    
            }
        }
    }
    /// <summary>
    /// Places a tile or asset at the given position.
    /// </summary>
    /// <param name="_position">The position to place the tile or asset.</param>
    private void PlaceTileOrAsset(Vector3Int _position)
    {
        switch (currentPaintMode)
        {
            case 0:
            case 2:
                PlaceTile(_position);
                break;
            case 1:
                PlaceAsset(_position);
                break;
        }
    }
    /// <summary>
    /// Places a tile at the cursor position or a specified position.
    /// </summary>
    /// <param name="_position">Optional parameter: position to place the tile.</param>
    private void PlaceTile(Vector3 _position = default)
    {
        Vector3 positionToUse = _position == default ? cursorPos : _position;

        TileBase tile = levelManager.GetTileFromMemory(hotBarTileID[currentHotBarIndex]);
        Vector3Int position = new Vector3Int((int)MathF.Floor(positionToUse.x), (int)MathF.Floor(positionToUse.y), 0);
        currentTilemap.SetTile(position, tile);
    }

    /// <summary>
    /// Places an asset at the cursor position or a specified position.
    /// </summary>
    /// <param name="_position">Optional parameter: position to place the asset.</param>
    private void PlaceAsset(Vector3 _position = default)
    {
        Vector3 positionToUse = _position == default ? cursorPos : _position;

        // Destroy any asset already in the selected position
        for (int i = 0; i < levelManager.assetsRoot.transform.childCount; i++)
        {
            Transform child = levelManager.assetsRoot.transform.GetChild(i);
            if (child.position == new Vector3(MathF.Round(positionToUse.x), MathF.Round(positionToUse.y), child.position.z))
            {
                Destroy(child.gameObject);
                break;
            }
        }

        // Place the current asset at the selected position
        GameObject asset = levelManager.GetAssetFromMemory(hotBarTileID[currentHotBarIndex]);
        float assetZ = asset.transform.position.z + -currentLayer;
        Vector3 assetPosition = new Vector3(MathF.Round(positionToUse.x), MathF.Round(positionToUse.y), assetZ);
        GameObject assetRef = Instantiate(asset, assetPosition, Quaternion.identity, levelManager.assetsRoot.transform);
        assetRef.name = assetRef.name.Replace("(Clone)", "").Trim();
        //if (assetRef.GetComponent<Asset_UniqueInstanceId>()) assetRef.GetComponent<Asset_UniqueInstanceId>().Id = GetNextAvailableId();
        
        if(!assetRef.GetComponent<Object_DepthAssigner>()) return;

        assetRef.GetComponent<Object_DepthAssigner>().depthLayer = currentLayer;

/* OLD DEPTH ASSIGNMENT
        var assetSortingLayer = assetRef.GetComponent<SpriteRenderer>().sortingLayerName;
        
        // Assign the object to the correct height layer
        switch (currentLayer)
        {
            case 0:
                // Set collision layer for all collider on object
                for (int i = 0; i < assetRef.GetComponentsInChildren<Collider2D>().Length; i++)
                {
                    asset.GetComponentsInChildren<Collider2D>()[i].gameObject.layer = 6;
                }
                // Set Z depth for lighting
                assetRef.transform.position = new Vector3(assetRef.transform.position.x, assetRef.transform.position.y, 0);
                // Set draw layer
                if (assetSortingLayer == "Trigger" || assetSortingLayer == "NoDraw") break;
                assetRef.GetComponent<SpriteRenderer>().sortingLayerName = "Depth Layer 1";
                break;
            case 1:
                // Set collision layer for all collider on object
                for (int i = 0; i < assetRef.GetComponentsInChildren<Collider2D>().Length; i++)
                {
                    asset.GetComponentsInChildren<Collider2D>()[i].gameObject.layer = 7;
                }
                // Set Z depth for lighting
                assetRef.transform.position = new Vector3(assetRef.transform.position.x, assetRef.transform.position.y, -1);
                // Set draw layer
                if (assetSortingLayer == "Trigger" || assetSortingLayer == "NoDraw") break;
                assetRef.GetComponent<SpriteRenderer>().sortingLayerName = "Depth Layer 2";
                break;
            case 2:
                // Set collision layer for all colliders on object
                for (int i = 0; i < assetRef.GetComponentsInChildren<Collider2D>().Length; i++)
                {
                    asset.GetComponentsInChildren<Collider2D>()[i].gameObject.layer = 8;
                }
                // Set Z depth for lighting
                assetRef.transform.position = new Vector3(assetRef.transform.position.x, assetRef.transform.position.y, -2);
                // Set draw layer
                if (assetSortingLayer == "Trigger" || assetSortingLayer == "NoDraw") break;
                assetRef.GetComponent<SpriteRenderer>().sortingLayerName = "Depth Layer 3";
                break;
        }*/
    }

    /// <summary>
    /// Clears the inspection panel and inspects the closest asset to the cursor position, considering nearby positions.
    /// </summary>
    private void Inspect()
    {
        //inspector.Clear();
    
        // Define a tolerance radius for nearby positions
        float toleranceRadius = 0.9f;
    
        // Initialize variables to keep track of the closest asset
        Transform closestAsset = null;
        float closestDistance = float.MaxValue;

        // Check for asset at cursor position or nearby positions
        foreach (Transform child in levelManager.assetsRoot.transform)
        {
            // Calculate the distance between the cursor position and the asset's position
            float distance = Vector2.Distance(new Vector2(cursorPos.x, cursorPos.y), new Vector2(child.position.x, child.position.y));

            // Check if the asset is within the tolerance radius
            if (distance <= toleranceRadius)
            {
                // If the asset is closer than the current closest asset, update the closest asset
                if (distance < closestDistance)
                {
                    closestAsset = child;
                    closestDistance = distance;
                }
            }
        }
    
        // If a closest asset is found, inspect it
        if (closestAsset != null)
        {
            inspectionIndicator.SetActive(true);
            inspectionIndicator.transform.position = closestAsset.position;
            Object_RuntimeDataInspector data = closestAsset.gameObject.GetComponent<Object_RuntimeDataInspector>();
            if (data != null)
            {
                inspector.InitializeInspector(data);
            }
        }
        else
        {
            // If no asset is found nearby, hide the inspection indicator
            inspectionIndicator.SetActive(false);
            inspector.Clear();
        }
    }

    /// <summary>
    /// Sets the current hot bar tile to the one at the cursor position.
    /// </summary>
    private void Pick()
    {
        // Define a tolerance radius for nearby positions
        float toleranceRadius = 0.9f;
    
        // Initialize variables to keep track of the closest asset
        Transform closestAsset = null;
        float closestDistance = float.MaxValue;

        // Check for asset at cursor position or nearby positions
        foreach (Transform child in levelManager.assetsRoot.transform)
        {
            // Calculate the distance between the cursor position and the asset's position
            float distance = Vector2.Distance(new Vector2(cursorPos.x, cursorPos.y), new Vector2(child.position.x, child.position.y));

            // Check if the asset is within the tolerance radius
            if (distance <= toleranceRadius)
            {
                // If the asset is closer than the current closest asset, update the closest asset
                if (distance < closestDistance)
                {
                    closestAsset = child;
                    closestDistance = distance;
                }
            }
        }
    
        // If a closest asset is found, inspect it
        if (closestAsset != null)
        {
            SetCurrentHotBarTile(closestAsset.name);
        }
        else
        {
            // If no asset is found nearby, 
            // Check for tile at cursor position
            Vector3Int position = new Vector3Int((int)MathF.Floor(cursorPos.x), (int)MathF.Floor(cursorPos.y), 0);
            TileBase tile = currentTilemap.GetTile(position);
            if (tile != null)
            {
                SetCurrentHotBarTile(tile.name);
            }
        }
    }

    /// <summary>
    /// Places a tile or asset at the given position.
    /// </summary>
    /// <param name="_position">The position to place the tile or asset.</param>
    private void EraseTileOrAsset(Vector3Int _position)
    {
        switch (currentPaintMode)
        {
            case 0:
            case 2:
                EraseTile(_position);
                break;
            case 1:
                EraseAsset(_position);
                break;
        }
    }
    
    /// <summary>
    /// Erases either a tile or an asset based on the current paint mode.
    /// </summary>
    private void Erase()
    {
        switch (currentPaintMode)
        {
            // Erasing a tile or an asset
            case 0:
            case 2:
                EraseTile();
                break;
            // Erasing an asset
            case 1:
                EraseAsset();
                break;
        }
    }

    /// <summary>
    /// Erases a tile at the cursor position or a specified position.
    /// </summary>
    /// <param name="_position">Optional parameter: position to erase the tile.</param>
    private void EraseTile(Vector3 _position = default)
    {
        Vector3 positionToUse = _position == default ? cursorPos : _position;

        Vector3Int position = new Vector3Int((int)MathF.Floor(positionToUse.x), (int)MathF.Floor(positionToUse.y), 0);
        currentTilemap.SetTile(position, null);
    }

    /// <summary>
    /// Erases the closest asset to the cursor position or a specified position.
    /// </summary>
    /// <param name="_position">Optional parameter: position to erase the asset.</param>
    private void EraseAsset(Vector3 _position = default)
    {
        Vector3 positionToUse = _position == default ? cursorPos : _position;
    
        // Define a tolerance radius for nearby positions
        float toleranceRadius = 0.9f;
    
        // Initialize variables to keep track of the closest asset
        Transform closestAsset = null;
        float closestDistance = float.MaxValue;

        // Find the closest asset to the specified position
        for (int i = 0; i < levelManager.assetsRoot.transform.childCount; i++)
        {
            Transform child = levelManager.assetsRoot.transform.GetChild(i);
        
            // Calculate the distance between the specified position and the asset's position
            float distance = Vector2.Distance(new Vector2(positionToUse.x, positionToUse.y), new Vector2(child.position.x, child.position.y));

            // Check if the asset is within the tolerance radius
            if (distance <= toleranceRadius)
            {
                // If the asset is closer than the current closest asset, update the closest asset
                if (distance < closestDistance)
                {
                    closestAsset = child;
                    closestDistance = distance;
                }
            }
        }

        // If a closest asset is found, erase it
        if (closestAsset != null)
        {
            Destroy(closestAsset.gameObject);
        }
    }

    
    /// <summary>
    /// Toggles the visibility of the inventory UI.
    /// </summary>
    private void ToggleInventory()
    {
        inventoryOpen = !inventoryOpen;
        inventory.SetActive(inventoryOpen);
    }
    

    //=-----------------=
    // External Functions
    //=-----------------=
    /// <summary>
    /// Sets the tile ID of the current hot bar index.
    /// </summary>
    /// <param name="_tileID">The ID of the tile to set.</param>
    public void SetCurrentHotBarTile(string _tileID)
    {
        hotBarTileID[currentHotBarIndex] = _tileID;
    }
}
