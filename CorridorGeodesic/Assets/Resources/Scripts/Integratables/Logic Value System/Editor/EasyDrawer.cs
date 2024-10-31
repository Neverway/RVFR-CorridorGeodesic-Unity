using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;
using Debug = UnityEngine.Debug;

public abstract class EasyDrawer : PropertyDrawer
{
    public float propertyHeight;
    public Rect area;
    public Properties property;
    public GUIContent label;


    private List<string> alreadyDisplayedErrors = new();
    private static bool propertiesWereModified = true;

    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
    {
        this.property = new Properties(property);
        this.label = label;
        this.area = position;

        DrawerObject.currentlyDrawing = new();

        EditorGUI.BeginProperty(position, label, property);
        try
        {
            DrawerObject contents = OnGUIEasyDrawer(new VerticalGroup());

            position.height = propertyHeight = contents.GetHeight();

            contents.Draw(position);

            OnBeforeFinishGUI(position);
        }
        catch (Exception e)
        {
            if (e is not ExitGUIException && !(string.IsNullOrEmpty(e.StackTrace) || alreadyDisplayedErrors.Contains(e.StackTrace)))
            {
                UnityEngine.Object unityObject = property.serializedObject.targetObject;
                try
                {
                    string errorMessage =
                        $"<size=10><color=yellow>Error drawing property drawer. Context for next error:</color></size>" +
                        $"   Type: {property.GetUnderlyingType().SelectedName(false, true)}" +
                        $"  |  Field: {property.displayName}  " +
                        $"  |  Object: {unityObject}" +
                        $"\n{DrawerObject.CurrentDrawerChainNames()}";

                alreadyDisplayedErrors.Add(e.StackTrace);

                Debug.LogError(errorMessage, unityObject);
                }
                catch
                {
                    Debug.Log(property.GetUnderlyingField());
                    Debug.LogWarning($"Had error trying to display extra context information for property drawer error???", unityObject);
                }

                Debug.LogException(e, unityObject);
            }
        }
        EditorGUI.EndProperty();
        OnAfterGUI();

        if (propertiesWereModified)
        {
            property.serializedObject.ApplyModifiedProperties();
            EditorUtility.SetDirty(property.serializedObject.targetObject);
            propertiesWereModified = false;
        }
    }
    public override float GetPropertyHeight(SerializedProperty property, GUIContent label) =>
        Mathf.Max(propertyHeight, EditorGUIUtility.singleLineHeight);

    public abstract DrawerObject OnGUIEasyDrawer(VerticalGroup contents);
    public virtual void OnBeforeFinishGUI(Rect position) { }
    public virtual void DebugError(Exception e) { Debug.LogError(e); }
    public abstract void OnAfterGUI();

    public static bool SetModified { set { propertiesWereModified |= value; } }

    public struct Properties
    {
        private SerializedProperty property;
        private Type cachedType;
        private FieldInfo cachedFieldInfo;
        private Dictionary<string, Properties> properties;

        public Properties(SerializedProperty property)
        {
            properties = new Dictionary<string, Properties>();
            this.property = property;
            this.cachedType = null;
            this.cachedFieldInfo = null;
        }

        public SerializedProperty Property => property;
        public AnimationCurve AnimationCurve
        {
            get { return property.animationCurveValue; }
            set { property.animationCurveValue = value; }
        }
        #region Quick Access To Property Values
        public bool Bool
        {
            get { return property.boolValue; }
            set { property.boolValue = value; }
        }
        public string String
        {
            get { return property.stringValue; }
            set { property.stringValue = value; }
        }
        public float Float
        {
            get { return property.floatValue; }
            set { property.floatValue = value; }
        }
        public int Int
        {
            get { return property.intValue; }
            set { property.intValue = value; }
        }
        public Vector3 Vector3
        {
            get { return property.vector3Value; }
            set { property.vector3Value = value; }
        }
        public Vector3Int Vector3Int
        {
            get { return property.vector3IntValue; }
            set { property.vector3IntValue = value; }
        }
        public Vector2 Vector2
        {
            get { return property.vector2Value; }
            set { property.vector2Value = value; }
        }
        public Vector2Int Vector2Int
        {
            get { return property.vector2IntValue; }
            set { property.vector2IntValue = value; }
        }
        #endregion

        public Properties this[params string[] fields]
        {
            get
            {
                Properties toReturn = this;
                foreach(string field in fields)
                {
                    if (!toReturn.properties.ContainsKey(field))
                        toReturn.properties.Add(field, new Properties(property.FindPropertyRelative(field)));

                    toReturn = toReturn.properties[field];
                }
                return toReturn;
            }
        }

        public static implicit operator SerializedProperty(Properties myself) => myself.property;

        public Type FieldType => (cachedType == null) ? cachedType = property.GetUnderlyingType() : cachedType;
        public FieldInfo FieldInfo => (cachedFieldInfo == null) ? cachedFieldInfo = property.GetFieldInfo() : cachedFieldInfo;
        public bool IsPropertyInArrayOrList() => property.propertyPath.Contains("Array.data");

        public T GetExpensive<T>() => (T)property.boxedValue;
        public void SetExpensive(object value) => property.boxedValue = value;

    }
    public class UniqueDrawerValue<T>
    {
        private Dictionary<string, T> separatedValues = new Dictionary<string, T>();

        public T this[Properties prop]
        {
            get
            {
                if (!separatedValues.ContainsKey(prop.Property.propertyPath))
                    separatedValues.Add(prop.Property.propertyPath, default);

                return separatedValues[prop.Property.propertyPath];
            }
            set
            {
                if (!separatedValues.ContainsKey(prop.Property.propertyPath))
                    separatedValues.Add(prop.Property.propertyPath, value);
                else
                    separatedValues[prop.Property.propertyPath] = value;
            }
        }
    }
    

    public abstract class DrawerObject
    {
        public static List<DrawerObject> currentlyDrawing = new();

        public GUIContent content = new GUIContent("");
        public GUIStyle style = new GUIStyle(EditorStyles.label);

        protected float height = EditorGUIUtility.singleLineHeight;
        public virtual VerticalGroup ToVerticalGroup(params DrawerObject[] objectsToAdd)
        {
            DrawerObject[] all = new DrawerObject[objectsToAdd.Length + 1];
            all[0] = this;
            for (int i = 0; i < objectsToAdd.Length; i++)
                all[i + 1] = objectsToAdd[i];

            return new VerticalGroup(all);
        }
        public void Draw(Rect area)
        {
            currentlyDrawing.Add(this);
            OnDraw(area);
            currentlyDrawing.Remove(this);
        }
        protected abstract void OnDraw(Rect area);
        public float GetHeight()
        {
            float myHeight = height;
            currentlyDrawing.Add(this);
            myHeight = OnGetHeight();
            currentlyDrawing.Remove(this);

            return myHeight;
        }
        protected virtual float OnGetHeight() => height;

        public override string ToString()
        {
            string className = GetType().CSharpName(true);
            className = className.Substring(className.IndexOf('.') + 1, className.Length - className.IndexOf('.') - 1);
            return className + (string.IsNullOrEmpty(content.text) ? "" : $"({content.text})");
        }

        public static string CurrentDrawerChainNames()
        {
            string totalString = "";

            foreach (DrawerObject obj in currentlyDrawing)
                totalString += (totalString == "" ? "" : " -> ") + obj.ToString();

            return totalString;
        }
    }
    
    public abstract class DrawerObjectWithStyle : DrawerObject
    {
        public GUIStyle style = new GUIStyle(EditorStyles.label);

        public DrawerObjectWithStyle AlignUpperLeft()
        {
            style.alignment = TextAnchor.UpperLeft;
            return this;
        }
        public DrawerObjectWithStyle AlignTrueCenter()
        {
            style.alignment = TextAnchor.MiddleCenter;
            return this;
        }

        public DrawerObjectWithStyle AlignUpper()
        {
            if (style.alignment == TextAnchor.MiddleLeft || style.alignment == TextAnchor.LowerLeft)
                style.alignment = TextAnchor.UpperLeft;
            else if (style.alignment == TextAnchor.MiddleCenter || style.alignment == TextAnchor.LowerCenter)
                style.alignment = TextAnchor.UpperCenter;
            else if (style.alignment == TextAnchor.MiddleRight || style.alignment == TextAnchor.LowerRight)
                style.alignment = TextAnchor.UpperRight;
            else
                style.alignment = TextAnchor.UpperCenter;

            return this;
        }
        public DrawerObjectWithStyle AlignMiddle()
        {
            if (style.alignment == TextAnchor.UpperLeft || style.alignment == TextAnchor.LowerLeft)
                style.alignment = TextAnchor.MiddleLeft;
            else if (style.alignment == TextAnchor.UpperCenter || style.alignment == TextAnchor.LowerCenter)
                style.alignment = TextAnchor.MiddleCenter;
            else if (style.alignment == TextAnchor.UpperRight || style.alignment == TextAnchor.LowerRight)
                style.alignment = TextAnchor.MiddleRight;
            else
                style.alignment = TextAnchor.MiddleCenter;

            return this;
        }
        public DrawerObjectWithStyle AlignLower()
        {
            if (style.alignment == TextAnchor.UpperLeft || style.alignment == TextAnchor.MiddleLeft)
                style.alignment = TextAnchor.LowerLeft;
            else if (style.alignment == TextAnchor.UpperCenter || style.alignment == TextAnchor.MiddleCenter)
                style.alignment = TextAnchor.LowerCenter;
            else if (style.alignment == TextAnchor.UpperRight || style.alignment == TextAnchor.MiddleRight)
                style.alignment = TextAnchor.LowerRight;
            else
                style.alignment = TextAnchor.LowerCenter;

            return this;
        }
        public DrawerObjectWithStyle AlignLeft()
        {
            if (style.alignment == TextAnchor.UpperCenter || style.alignment == TextAnchor.UpperRight)
                style.alignment = TextAnchor.UpperLeft;
            else if (style.alignment == TextAnchor.MiddleCenter || style.alignment == TextAnchor.MiddleRight)
                style.alignment = TextAnchor.MiddleLeft;
            else if (style.alignment == TextAnchor.LowerCenter || style.alignment == TextAnchor.LowerRight)
                style.alignment = TextAnchor.LowerLeft;
            else
                style.alignment = TextAnchor.MiddleLeft;

            return this;
        }
        public DrawerObjectWithStyle AlignCenter()
        {
            if (style.alignment == TextAnchor.UpperLeft || style.alignment == TextAnchor.UpperRight)
                style.alignment = TextAnchor.UpperCenter;
            else if (style.alignment == TextAnchor.MiddleLeft || style.alignment == TextAnchor.MiddleRight)
                style.alignment = TextAnchor.MiddleCenter;
            else if (style.alignment == TextAnchor.LowerLeft || style.alignment == TextAnchor.LowerRight)
                style.alignment = TextAnchor.LowerCenter;
            else
                style.alignment = TextAnchor.MiddleCenter;

            return this;
        }
        public DrawerObjectWithStyle AlignRight()
        {
            if (style.alignment == TextAnchor.UpperLeft || style.alignment == TextAnchor.UpperCenter)
                style.alignment = TextAnchor.UpperRight;
            else if (style.alignment == TextAnchor.MiddleLeft || style.alignment == TextAnchor.MiddleCenter)
                style.alignment = TextAnchor.MiddleRight;
            else if (style.alignment == TextAnchor.LowerLeft || style.alignment == TextAnchor.LowerCenter)
                style.alignment = TextAnchor.LowerRight;
            else
                style.alignment = TextAnchor.MiddleRight;

            return this;
        }

        public DrawerObjectWithStyle NotBoldOrItalic()
        {
            style.fontStyle = FontStyle.Normal;
            return this;
        }
        public DrawerObjectWithStyle Bold()
        {
            if (style.fontStyle == FontStyle.Italic)
                style.fontStyle = FontStyle.BoldAndItalic;
            else
                style.fontStyle = FontStyle.Bold;
            return this;
        }
        public DrawerObjectWithStyle Italic()
        {
            style.fontStyle &= FontStyle.Italic;
            return this;
        }
        public DrawerObjectWithStyle UseRichText(bool useRichText = true)
        {
            style.richText = useRichText;
            return this;
        }
        public DrawerObjectWithStyle Small()
        {
            style.fontSize = 10;
            return this;
        }
        public DrawerObjectWithStyle Small2()
        {
            style.fontSize = 8;
            return this;
        }
        public DrawerObjectWithStyle Big()
        {
            style.fontSize = 14;
            return this;
        }
        public DrawerObjectWithStyle Big2()
        {
            style.fontSize = 18;
            return this;
        }
    }
    public abstract class DrawerObjectWithGUIContent : DrawerObject
    {
        public DrawerObjectWithGUIContent Label(string label)
        {
            content.text = label;
            return this;
        }
        public DrawerObjectWithGUIContent HideLabel()
        {
            content.text = "";
            return this;
        }
        public DrawerObjectWithGUIContent Tooltip(string tooltip)
        {
            content.tooltip = tooltip;
            return this;
        }
        public DrawerObjectWithGUIContent Image(Texture image)
        {
            content.image = image;
            return this;
        }
    }

    public class EmptySpace : DrawerObject
    {
        public EmptySpace() { }
        public EmptySpace(float height) 
        {
            this.height = height;
        }
        protected override void OnDraw(Rect area) { }
    }
    public class HorizontalGroup : DrawerObject
    {
        public DrawerObject[] objects;
        public HorizontalGroup(params DrawerObject[] objects)
        {
            this.objects = objects;
        }
        public HorizontalGroup Add(params DrawerObject[] objectsToAdd)
        {
            List<DrawerObject> newObjects = new List<DrawerObject>();

            newObjects.AddRange(objects);
            newObjects.AddRange(objectsToAdd);

            objects = newObjects.ToArray();
            return this;
        }

        protected override void OnDraw(Rect area)
        {
            try
            {
                Rect[] dividedAreas = DivideRectHorizontally(area, objects.Length);

                for(int i = 0; i < objects.Length; i++)
                {
                        objects[i].Draw(dividedAreas[i]);
                }
            }
            catch (Exception e) { Debug.LogError(e); }
        }
        protected override float OnGetHeight()
        {
            height = 0;
            foreach (DrawerObject obj in objects)
                height = Mathf.Max(height, obj.GetHeight());
            return height;
        }
        public static Rect[] DivideRectHorizontally(Rect position, int divisions)
        {
            Rect[] dividedRects = new Rect[divisions];
            position.width /= divisions;

            for (int i = 0; i < divisions; i++)
            {
                dividedRects[i] = position;
                position.x += position.width;
            }
            return dividedRects;
        }
    }
    public class SizedHorizontalGroup : DrawerObject
    {
        public DrawerObject unsizedObject;
        public List<DrawerObjectWithWidth> left = new List<DrawerObjectWithWidth>();
        public List<DrawerObjectWithWidth> right = new List<DrawerObjectWithWidth>();
        public struct DrawerObjectWithWidth
        {
            public int flatWidth;
            public float percentageWidth;
            public bool useFlatWidth;
            public DrawerObject obj;
            public DrawerObjectWithWidth(DrawerObject obj, int widthAmount)
            {
                this.flatWidth = widthAmount;
                this.percentageWidth = 0f;
                this.useFlatWidth = true;
                this.obj = obj;
            }
            public DrawerObjectWithWidth(DrawerObject obj, float widthFactor)
            {
                this.flatWidth = 0;
                this.percentageWidth = widthFactor;
                this.useFlatWidth = false;
                this.obj = obj;
            }
            public float GetWidth(float fullWidth)
            {
                if (useFlatWidth)
                    return flatWidth;

                return percentageWidth * flatWidth;
            }
        }
        
        public SizedHorizontalGroup(DrawerObject unsizedObject)
        {
            this.unsizedObject = unsizedObject;
        }
        
        public SizedHorizontalGroup AddOnLeft(DrawerObject obj, int widthPixels)
            => AddOnLeft(new DrawerObjectWithWidth(obj, widthPixels));
        public SizedHorizontalGroup AddOnLeft(DrawerObject obj, float widthFactor)
            => AddOnLeft(new DrawerObjectWithWidth(obj, widthFactor));
        private SizedHorizontalGroup AddOnLeft(DrawerObjectWithWidth obj) { left.Add(obj); return this; }
        public SizedHorizontalGroup AddOnRight(DrawerObject obj, int widthPixels)
            => AddOnRight(new DrawerObjectWithWidth(obj, widthPixels));
        public SizedHorizontalGroup AddOnRight(DrawerObject obj, float widthFactor)
            => AddOnRight(new DrawerObjectWithWidth(obj, widthFactor));
        private SizedHorizontalGroup AddOnRight(DrawerObjectWithWidth obj) { right.Add(obj); return this; }

        protected override void OnDraw(Rect area)
        {
            try
            {
                //Get full flat widths
                float fullWidth = area.width;
                Rect subArea = area;

                foreach(DrawerObjectWithWidth obj in left)
                {
                    float objWidth = obj.GetWidth(fullWidth);
                    subArea.width = objWidth;
                    area.width -= objWidth;
                    area.x += objWidth;

                        obj.obj.Draw(subArea);
                }
                foreach (DrawerObjectWithWidth obj in right)
                {
                    float objWidth = obj.GetWidth(fullWidth);
                    subArea.width = objWidth;
                    area.width -= objWidth;
                    subArea.x = area.x + area.width;

                        obj.obj.Draw(subArea);
                }
                unsizedObject.Draw(area);
            }
            catch (Exception e) { Debug.LogError(e); }
        }
        protected override float OnGetHeight()
        {
            height = unsizedObject.GetHeight();
            foreach (DrawerObjectWithWidth obj in left)
                height = Mathf.Max(height, obj.obj.GetHeight());
            foreach (DrawerObjectWithWidth obj in right)
                height = Mathf.Max(height, obj.obj.GetHeight());
            return height;
        }
    }
    public class VerticalGroup : DrawerObject
    {
        public float padding = 2f;
        public float indentAmount = 0f;

        public DrawerObject[] objects;
        public VerticalGroup(params DrawerObject[] objects)
        {
            this.objects = objects;
        }
        public VerticalGroup Add(params DrawerObject[] objectsToAdd)
        {
            List<DrawerObject> newObjects = new List<DrawerObject>();

            newObjects.AddRange(objects);
            newObjects.AddRange(objectsToAdd);

            objects = newObjects.ToArray();
            return this;
        }

        public VerticalGroup AddIndent() => AddIndent(EditorGUIUtility.singleLineHeight);
        public VerticalGroup AddIndent(float amount)
        {
            indentAmount += amount;
            return this;
        }

        protected override void OnDraw(Rect area)
        {
            area.width -= indentAmount;
            area.x += indentAmount;

            foreach (DrawerObject obj in objects)
            {
                area.height = obj.GetHeight();
                obj.Draw(area);
                area.y += obj.GetHeight() + padding;
            }
        }
        protected override float OnGetHeight()
        {
            height = 0;
            foreach (DrawerObject obj in objects)
                height += obj.GetHeight() + padding;

            height -= padding;
            height = Mathf.Max(height, 0);
            return height;
        }
    }

    public class Disable : DrawerObject
    {
        public DrawerObject contents;
        public bool disabled = true;
        public Disable(DrawerObject contents) 
        {
            this.contents = contents;
            this.disabled = true;
        }
        public Disable(DrawerObject contents, bool doDisable)
        {
            this.contents = contents;
            this.disabled = doDisable;
        }
        protected override void OnDraw(Rect area)
        {
            bool wasEnabled = GUI.enabled;
            try
            {
                GUI.enabled = !disabled;
                contents.Draw(area);
            }
            catch (Exception e) { Debug.LogError(e); }

            GUI.enabled = wasEnabled;
        }

        protected override float OnGetHeight() => contents.GetHeight();
    }
    public class Hide : DrawerObject
    {
        public DrawerObject contents;
        public Func<bool> doHide;
        public Hide(DrawerObject contents, Func<bool> doHide)
        {
            this.contents = contents;
            this.doHide = doHide;
        }
        protected override void OnDraw(Rect area)
        {
            if (!doHide.Invoke())
                contents.Draw(area);
        }

        protected override float OnGetHeight() => (doHide.Invoke() ? 0 : contents.GetHeight());
    }

    public class Title : Label
    {
        public Title(string label) : base(label) 
        {
            AlignCenter();
            AlignUpper();
            Big();
            Bold();
        }

        protected override void OnDraw(Rect area)
        {
            base.Draw(area);
            Color dividerColor = new Color(0.5f, 0.5f, 0.5f);
            area.y += area.height;
            area.y -= 2f;
            area.height = 2f;
            EditorGUI.DrawRect(area, dividerColor);
        }
        protected override float OnGetHeight() => base.GetHeight();
    }
    public class Divider : DrawerObject
    {
        public float spaceAbove = 3f;
        public float spaceBelow = 3f;
        protected override void OnDraw(Rect area)
        {
            Color dividerColor = new Color(0.5f, 0.5f, 0.5f);
            area.height -= spaceAbove + spaceBelow;
            area.y += spaceAbove;
            EditorGUI.DrawRect(area, dividerColor);
        }
        protected override float OnGetHeight() => spaceAbove + 2f + spaceBelow;
    }
    public class Boxed : DrawerObject
    {
        public DrawerObject contents;
        private float spacing = 5f;
        public int boldness = 3;
        public Boxed(DrawerObject contents, float spacing = 5f)
        {
            this.contents = contents;
            this.spacing = spacing;
            
        }
        public Boxed Boldness(int boldness)
        {
            this.boldness = boldness;
            return this;
        }

        protected override void OnDraw(Rect area)
        {
            for (int i = 0; i < boldness; i++)
                EditorGUI.HelpBox(area, "", MessageType.None);

            area.y += spacing;
            area.x += spacing;
            area.width -= spacing + spacing;
            area.height -= spacing + spacing;

            contents.Draw(area);
        }

        protected override float OnGetHeight()
        {
            return contents.GetHeight() + (spacing + spacing);
        }
    }
    public class Label : DrawerObjectWithStyle
    {
        public string label;
        public Label(string label)
        {
            style = new GUIStyle(EditorStyles.label);
            this.label = label;
        }
        protected override void OnDraw(Rect area)
        {
            EditorGUI.LabelField(area, label, style);
        }
    }
    public class Button : DrawerObjectWithStyle
    {
        public string text;
        public Action onPress;
        public Button(string text, Action onPress)
        {
            style = new GUIStyle(EditorStyles.miniButton);
            this.text = text;
            this.onPress = onPress;
        }
        protected override void OnDraw(Rect area)
        {
            if (GUI.Button(area, text, style))
                onPress?.Invoke();
        }
    }

    public class FittedLabel : DrawerObjectWithStyle
    {
        public GUIContent label;
        public DrawerObject afterLabel;
        public float maxWidthFactor = 0.5f;
        public float padding = EditorGUIUtility.singleLineHeight;
        public FittedLabel(string label, DrawerObject afterLabel)
        {
            style = new GUIStyle(EditorStyles.label);
            this.label = new GUIContent(label);
            this.afterLabel = afterLabel;
        }
        public FittedLabel MaxWidthFactor(float maxWidthFactor)
        {
            this.maxWidthFactor = Mathf.Clamp(maxWidthFactor, 0f, 1f);
            return this;
        }
        public FittedLabel Padding(float padding)
        {
            this.padding = padding;
            return this;
        }
        protected override void OnDraw(Rect area)
        {
            Vector2 size = style.CalcSize(label);
            float requiredWidth = Mathf.Min(size.x + padding, area.width * maxWidthFactor);

            Rect labelArea = area;
            labelArea.width = requiredWidth;
            EditorGUI.LabelField(labelArea, label, style);

            Rect fieldsArea = area;
            fieldsArea.width -= requiredWidth;
            fieldsArea.x += requiredWidth;
            afterLabel.Draw(fieldsArea);
        }
        protected override float OnGetHeight() => afterLabel.GetHeight();
    }
    public class Property : DrawerObjectWithGUIContent
    {
        public SerializedProperty property;
        bool usePropertyLabel = true;
        bool includeChildren = false;
        public Property(SerializedProperty property)
        {
            if (property == null)
                throw new NullReferenceException();

            content = new GUIContent(GUIContent.none);
            this.property = property;
        }
        public new Property Label(string label)
        {
            content.text = label;
            usePropertyLabel = false;
            return this;
        }
        public new Property HideLabel()
        {
            usePropertyLabel = false;
            return this;
        }
        public Property IncludeChildren()
        {
            includeChildren = true;
            return this;
        }

        protected override void OnDraw(Rect area)
        {
            if (usePropertyLabel)
                SetModified = EditorGUI.PropertyField(area, property, includeChildren);
            else
                SetModified = EditorGUI.PropertyField(area, property, content, includeChildren);
        }

        protected override float OnGetHeight()
        {
            if (usePropertyLabel)
                return EditorGUI.GetPropertyHeight(property, includeChildren);

            return EditorGUI.GetPropertyHeight(property, content, includeChildren);
        }
    }

    public class Dropdown : DrawerObject
    {
        protected override void OnDraw(Rect area)
        {
            throw new NotImplementedException();
        }
    }

    public class PolymorphicSelector : DrawerObjectWithStyle
    {
        DrawerObject contents;
        SerializedProperty property;
        Type baseType;
        public PolymorphicSelector(Properties property)
        {
            style = EditorStyles.label;
            this.property = property.Property;
            this.baseType = property.FieldType;

            this.contents = GetDrawerObjects();
        }
        protected DrawerObject GetDrawerObjects()
        {
            VerticalGroup contents = new VerticalGroup();

            // Dropdown to change the type of the SerializeReference field
            if (property.propertyType == SerializedPropertyType.ManagedReference)
            {
                Type currentType = property.managedReferenceValue?.GetType();
                if (currentType != null)
                {
                    contents.Add(new Label($"Current Type: {currentType.Name}"));
                }
                else
                {
                    contents.Add(new Label("No type found???"));
                }

                List<Type> derivedTypes = baseType.GetAllDerivedTypes();

                foreach (Type type in derivedTypes)
                {
                    contents.Add(new Button(type.HumanName(true), () => { SetType(type); }));
                }

                contents.Add(new Divider());
            }
            
            contents.Add(new Property(property).IncludeChildren());

            return new Boxed(contents).Boldness(1);
        }

        private void SetType(Type type)
        {
            object newInstance = Activator.CreateInstance(type);
            property.managedReferenceValue = newInstance;
            property.serializedObject.ApplyModifiedProperties();
        }

        protected override void OnDraw(Rect area)
        {
            contents.Draw(area);
        }
        protected override float OnGetHeight()
        {
            return contents.GetHeight();
        }
    }
    public class SelectComponentFromGameObject : DrawerObject
    {
        public GameObject obj;
        public SerializedProperty component;
        public SelectComponentFromGameObject(GameObject obj, SerializedProperty component)
        {
            this.obj = obj;
            this.component = component;
        }
        protected override void OnDraw(Rect area)
        {
            if (obj == null)
            {
                new Label("<Needs Game Object>").Draw(area);
                return;
            }
                
            Component[] componentsToSelect = obj.GetComponents(typeof(Component));
            string[] componentNames = ComponentsToNames(componentsToSelect);

            int selectedIndex = -1;
            try 
            {
                if (component.objectReferenceValue != null && component.objectReferenceValue is Component)
                    selectedIndex = Mathf.Max(0, System.Array.IndexOf(componentsToSelect, (Component)component.objectReferenceValue));
            } 
            catch { }

            selectedIndex = EditorGUI.Popup(area, selectedIndex, componentNames);

            if (selectedIndex >= 0)
                component.objectReferenceValue = componentsToSelect[selectedIndex];
        }

        private string[] ComponentsToNames(Component[] components)
        {
            string[] names = new string[components.Length];
            for(int i = 0; i < components.Length; i++)
            {
                string componentName = components[i].GetType().Name;
                int duplicatesBefore = 0;
                int duplicatesAfter = -1;
                for (int e = 0; e < components.Length; e++)
                {
                    if (componentName == components[e].GetType().Name)
                    {
                        if (e < i)
                            duplicatesBefore++;
                        else
                            duplicatesAfter++;
                    }
                }
                names[i] = componentName;

                if (duplicatesBefore + duplicatesAfter > 0)
                    names[i] += $" ({duplicatesBefore + 1})";
            }
            return names;
        }
    }
    public class SelectFieldDropdown : DrawerObjectWithStyle
    {
        public object toGetFieldsFrom;
        public SerializedProperty stringProperty;
        public Func<FieldInfo, bool> filter;
        public SelectFieldDropdown(object toGetFieldsFrom, SerializedProperty stringProperty)
        {
            this.style = EditorStyles.popup;
            this.toGetFieldsFrom = toGetFieldsFrom;
            this.stringProperty = stringProperty;
            this.filter = (f => true);
        }
        public SelectFieldDropdown Filter(Func<FieldInfo, bool> filter)
        {
            this.filter = filter;
            return this;
        }
        public SelectFieldDropdown FilterByType(Type type)
        {
            this.filter = (f => type.IsAssignableFrom(f.FieldType));
            return this;
        }
        protected override void OnDraw(Rect area)
        {
            if (toGetFieldsFrom == null)
            {
                new Label("<Missing object>").Draw(area);
                return;
            }

            string[] fieldNames = toGetFieldsFrom.GetType()
                .GetFields(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance)
                .Where(filter)
                .Select(f => f.Name).ToArray();

            List<string> fieldNamesWithDefault = new List<string>();
            fieldNamesWithDefault.Add("[Select Field]");

            if (fieldNames.Length > 0)
            {
                int selectedIndex = Mathf.Max(0, Array.IndexOf(fieldNames, stringProperty.stringValue));
                selectedIndex = EditorGUI.Popup(area, selectedIndex, fieldNames, style);

                if (selectedIndex >= 0)
                {
                    stringProperty.stringValue = fieldNames[selectedIndex];
                }
            }
            else
            {
                new Label("<No fields found>").Draw(area);
            }
        }
    }
}

public static class EasyDrawerExtensions
{
    public static BindingFlags InstanceFields = BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.Instance;

    public static Type GetFieldTypeDirect(this SerializedProperty property)
    {
        Type parentType = property.serializedObject.targetObject.GetType();
        FieldInfo fieldInfo = parentType.GetField(property.propertyPath, InstanceFields);
        return fieldInfo?.FieldType;
    }
    public static FieldInfo GetFieldInfoDirect(this SerializedProperty property)
    {
        Type parentType = property.serializedObject.targetObject.GetType();
        return parentType.GetField(property.propertyPath, InstanceFields);
    }

    public static Type GetFieldType(this SerializedProperty property)
    {
        Type parentType = property.serializedObject.targetObject.GetType();
        string[] path = property.propertyPath.Split('.'); // Split path for nested fields

        for (int i = 0; i < path.Length; i++)
        {
            FieldInfo fieldInfo = parentType.GetField(path[i], InstanceFields);
            if (fieldInfo == null)
                return null; // Field not found, exit with null

            parentType = fieldInfo.FieldType;

            // Handle array or list elements (e.g., "myArray.data[0]")
            if (parentType.IsArray)
            {
                parentType = parentType.GetElementType(); // Get array element type
            }
            else if (typeof(System.Collections.IList).IsAssignableFrom(parentType))
            {
                parentType = parentType.Generic(0); // Get list element type
            }
        }
        return parentType;
    }
    public static FieldInfo GetFieldInfo(this SerializedProperty property)
    {
        Type parentType = property.serializedObject.targetObject.GetType();
        string[] path = property.propertyPath.Split('.'); // Split path for nested fields
        FieldInfo fieldInfo = null;
        for (int i = 0; i < path.Length; i++)
        {
            fieldInfo = parentType.GetField(path[i], InstanceFields);
            if (fieldInfo == null)
                return null; // Field not found, exit with null

            parentType = fieldInfo.FieldType;

            // Handle array or list elements (e.g., "myArray.data[0]")
            if (parentType.IsArray)
            {
                parentType = parentType.GetElementType(); // Get array element type
            }
            else if (typeof(System.Collections.IList).IsAssignableFrom(parentType))
            {
                parentType = parentType.Generic(0); // Get list element type
            }
        }
        return fieldInfo;
    }

    // Helper to get all derived types of a given base type
    public static List<Type> GetAllDerivedTypes(this Type baseType)
    {
        return AppDomain.CurrentDomain.GetAssemblies()
            .SelectMany(assembly => assembly.GetTypes())
            .Where(t => t.IsSubclassOf(baseType) && !t.IsAbstract)
            .ToList();
    }

    public static Type Generic(this Type type, int index)
    {
        return type.GetGenericArguments()[index];
    }

    ///// <summary>
    ///// Extension method to check if a layer is in a layermask
    ///// </summary>
    ///// <param name="mask"></param>
    ///// <param name="layer"></param>
    ///// <returns></returns>
    //public static bool Contains(this LayerMask mask, int layer)
    //{
    //    return mask == (mask | (1 << layer));
    //}
}