using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;
using static UnityEngine.Rendering.DebugUI.MessageBox;

[CustomPropertyDrawer(typeof(EasyDrawerTestClass))]
public class EasyDrawer : PropertyDrawer
{
    public int propertyDrawerLines = 0;
    public const float SPACING = 2f;
    public float propertyHeight = 0f;

    public Dictionary<string, SerializedProperty> properties;
    public SerializedProperty property_self;

    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
    {
        properties = new Dictionary<string, SerializedProperty>();
        this.property_self = property;

        try
        {
            EditorGUI.BeginProperty(position, label, property);
            OnGUIEasyDrawer(position);
            EditorGUI.EndProperty();
        }
        catch (Exception e) 
        {
            Debug.LogError($"Error Drawing PropertyDrawer: {e}");
        }
    }

    public virtual void OnGUIEasyDrawer(Rect area)
    {
        string someStringField = nameof(EasyDrawerTestClass.someStringField);
        AddProperty(someStringField);
        string someTransform = nameof(EasyDrawerTestClass.someTransform);
        AddProperty(someTransform);
        string fieldReference = nameof(EasyDrawerTestClass.fieldReference);
        AddProperty(fieldReference);

        DrawerObject mainContents = new HorizontalGroup(
            new Label("Hello World"),
            new Property(properties[fieldReference]).HideLabel(),
            new Property(properties[someStringField]).HideLabel()
            );

        mainContents = new VerticalGroup(new Label("Test Prefix"), new Boxed(mainContents)).AddIndent();
        mainContents = new Boxed(mainContents);
        mainContents = new Boxed(mainContents);
        mainContents = new Boxed(mainContents);
        mainContents = new Boxed(mainContents);
        mainContents = new Boxed(mainContents);
        mainContents = new Boxed(mainContents);
        mainContents = new Boxed(mainContents);

        area.height = mainContents.GetHeight();
        mainContents.Draw(area);
        propertyHeight = mainContents.GetHeight();

        //VerticalGroup contents = new VerticalGroup();
        //
        //contents.Add(new Label("Testing Label"));
        //contents.Add(new HorizontalGroup(
        //    new Label("Field: "), 
        //    new Property(properties[someStringField])
        //    ));
        //
        //area.height = contents.height;
        //contents.Draw(area);
    }

    public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
    {
        return Mathf.Max(propertyHeight, EditorGUIUtility.singleLineHeight);
    }

    protected void AddProperty(string field)
    {
        if(!properties.ContainsKey(field))
            properties.Add(field, property_self.FindPropertyRelative(field));
    }
    protected void DRAW_Property(Rect position, string field)
    {
        AddProperty(field);
        EditorGUI.PropertyField(position, properties[field]);
    }
    protected void DRAW_Label(Rect position, string text, GUIStyle style = null)
    {
        if (style == null)
            EditorGUI.LabelField(position, text);
        else
            EditorGUI.LabelField(position, text, style);
    }

    protected Type GetFieldType(SerializedProperty property)
    {
        // Use reflection to get the type of the field
        Type targetType = property.serializedObject.targetObject.GetType();
        FieldInfo field = targetType.GetField(property.propertyPath);
        return field != null ? field.FieldType : null;
    }
    protected bool IsListType(Type type)
    {
        // Check if the type is a List<T>
        return type.IsGenericType && type.GetGenericTypeDefinition() == typeof(List<>);
    }

    public Rect StartLineCounting(Rect position)
    {
        propertyHeight = 0f;
        propertyDrawerLines = 1;
        position.height = EditorGUIUtility.singleLineHeight;
        return position;
    }
    public void EndLineCounting()
    {
        propertyDrawerLines--;
    }

    public Rect AdvanceLine(Rect position)
    {
        propertyDrawerLines++;
        position.y += EditorGUIUtility.singleLineHeight + SPACING;
        return position;
    }
    public float LineCountToHeight(int lines) =>
        EditorGUIUtility.singleLineHeight * lines + (2 * (lines - 1));

    public Rect Indent(Rect position)
    {
        //Make sure indentAmount is reasonable
        float indentAmount = Mathf.Min(EditorGUIUtility.singleLineHeight, position.width * 0.1f);

        position.x += indentAmount;
        position.width -= indentAmount;

        return position;
    }
    public Rect[] DivideRectHorizontally(Rect position, int divisions)
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


    public abstract class DrawerObject
    {
        protected float height = EditorGUIUtility.singleLineHeight;
        public virtual float GetHeight() => height;
        public abstract void Draw(Rect area);
    }
    
    public abstract class DrawerObjectWithStyle : DrawerObject
    {
        public GUIStyle style;
        public DrawerObjectWithStyle AlignLeft()
        {
            style.alignment = TextAnchor.MiddleLeft;
            return this;
        }
        public DrawerObjectWithStyle AlignCenter()
        {
            style.alignment = TextAnchor.MiddleCenter;
            return this;
        }
        public DrawerObjectWithStyle AlignRight()
        {
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
    }
    
    public abstract class DrawerObjectWithGUIContent : DrawerObject
    {
        public GUIContent content = GUIContent.none;

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
            newObjects.AddRange(newObjects);
            objects = newObjects.ToArray();
            return this;
        }

        public override void Draw(Rect area)
        {
            Rect[] dividedAreas = DivideRectHorizontally(area, objects.Length);

            for(int i = 0; i < objects.Length; i++)
                objects[i].Draw(dividedAreas[i]);
        }
        public override float GetHeight()
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
            newObjects.AddRange(newObjects);
            objects = newObjects.ToArray();
            return this;
        }

        public VerticalGroup AddIndent() => AddIndent(EditorGUIUtility.singleLineHeight);
        public VerticalGroup AddIndent(float amount)
        {
            indentAmount += amount;
            return this;
        }

        public override void Draw(Rect area)
        {
            foreach (DrawerObject obj in objects)
            {
                area.height = obj.GetHeight();
                obj.Draw(area);
                area.y += obj.GetHeight() + padding;
            }
        }
        public override float GetHeight()
        {
            height = 0;
            foreach (DrawerObject obj in objects)
                height += obj.GetHeight() + padding;

            height -= padding;
            height = Mathf.Max(height, 0);
            return height;
        }
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

        public override void Draw(Rect area)
        {
            for (int i = 0; i < boldness; i++)
                EditorGUI.HelpBox(area, "", MessageType.None);

            area.y += spacing;
            area.x += spacing;
            area.width -= spacing + spacing;
            area.height -= spacing + spacing;

            contents.Draw(area);
        }

        public override float GetHeight()
        {
            return contents.GetHeight() + (spacing + spacing);
        }
    }


    public class Label : DrawerObjectWithStyle
    {
        public string label;
        public Label(string label)
        {
            style = EditorStyles.label;
            this.label = label;
        }
        public override void Draw(Rect area) => EditorGUI.LabelField(area, label, style);
    }
    public class Property : DrawerObjectWithGUIContent
    {
        public SerializedProperty property;
        bool usePropertyLabel = false;
        bool includeChildren = false;
        public Property(SerializedProperty property)
        {
            this.property = property;
        }
        public Property UsePropertyLabel()
        {
            usePropertyLabel = true;
            return this;
        }
        public Property IncludeChildren()
        {
            includeChildren = true;
            return this;
        }

        public override void Draw(Rect area)
        {
            if (usePropertyLabel)
                EditorGUI.PropertyField(area, property);
            else
                EditorGUI.PropertyField(area, property, content, includeChildren);
        }

        public override float GetHeight()
        {
            return EditorGUI.GetPropertyHeight(property, includeChildren);
        }
    }
}