using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using UnityEditor;
using UnityEditor.ShortcutManagement;
using UnityEngine;


public class EditorIcons : EditorWindow
{
    #region EditorGUIUtility Icons
    public static Texture ScriptableObjectIcon => GetEditorGUIUtilityIcon("ScriptableObject Icon");
    public static Texture ZEROKEY => AssetDatabase.LoadAssetAtPath("Assets/Packages/Xelu_Prompts/Keyboard & Mouse/Dark/0_Key_Dark.png", typeof(Texture2D)) as Texture;
    public static Texture Popup => GetEditorGUIUtilityIcon("_Popup");
    public static Texture Help => GetEditorGUIUtilityIcon("_Help");
    public static Texture Clipboard => GetEditorGUIUtilityIcon("Clipboard");
    public static Texture SocialNetworks_UDNOpen => GetEditorGUIUtilityIcon("SocialNetworks.UDNOpen");
    public static Texture SocialNetworks_Tweet => GetEditorGUIUtilityIcon("SocialNetworks.Tweet");
    public static Texture SocialNetworks_FacebookShare => GetEditorGUIUtilityIcon("SocialNetworks.FacebookShare");
    public static Texture SocialNetworks_LinkedInShare => GetEditorGUIUtilityIcon("SocialNetworks.LinkedInShare");
    public static Texture SocialNetworks_UDNLogo => GetEditorGUIUtilityIcon("SocialNetworks.UDNLogo");
    public static Texture AnimationVisibilityToggleOff => GetEditorGUIUtilityIcon("animationvisibilitytoggleoff");
    public static Texture AnimationVisibilityToggleOn => GetEditorGUIUtilityIcon("animationvisibilitytoggleon");
    public static Texture TreeIcon => GetEditorGUIUtilityIcon("tree_icon");
    public static Texture TreeIconLeaf => GetEditorGUIUtilityIcon("tree_icon_leaf");
    public static Texture TreeIconFrond => GetEditorGUIUtilityIcon("tree_icon_frond");
    public static Texture TreeIconBranch => GetEditorGUIUtilityIcon("tree_icon_branch");
    public static Texture TreeIconBranchFrond => GetEditorGUIUtilityIcon("tree_icon_branch_frond");
    public static Texture EditIcon_sml => GetEditorGUIUtilityIcon("editicon.sml");
    public static Texture TreeEditor_Refresh => GetEditorGUIUtilityIcon("TreeEditor.Refresh");
    public static Texture TreeEditor_Duplicate => GetEditorGUIUtilityIcon("TreeEditor.Duplicate");
    public static Texture TreeEditor_Trash => GetEditorGUIUtilityIcon("TreeEditor.Trash");
    public static Texture TreeEditor_AddBranches => GetEditorGUIUtilityIcon("TreeEditor.AddBranches");
    public static Texture TreeEditor_AddLeaves => GetEditorGUIUtilityIcon("TreeEditor.AddLeaves");
    public static Texture PreAudioPlayOn => GetEditorGUIUtilityIcon("preAudioPlayOn");
    public static Texture PreAudioPlayOff => GetEditorGUIUtilityIcon("preAudioPlayOff");
    public static Texture AvatarInspector_RightFingersIk => GetEditorGUIUtilityIcon("AvatarInspector/RightFingersIk");
    public static Texture AvatarInspector_LeftFingersIk => GetEditorGUIUtilityIcon("AvatarInspector/LeftFingersIk");
    public static Texture AvatarInspector_RightFeetIk => GetEditorGUIUtilityIcon("AvatarInspector/RightFeetIk");
    public static Texture AvatarInspector_LeftFeetIk => GetEditorGUIUtilityIcon("AvatarInspector/LeftFeetIk");
    public static Texture AvatarInspector_RightFingers => GetEditorGUIUtilityIcon("AvatarInspector/RightFingers");
    public static Texture AvatarInspector_LeftFingers => GetEditorGUIUtilityIcon("AvatarInspector/LeftFingers");
    public static Texture AvatarInspector_RightArm => GetEditorGUIUtilityIcon("AvatarInspector/RightArm");
    public static Texture AvatarInspector_LeftArm => GetEditorGUIUtilityIcon("AvatarInspector/LeftArm");
    public static Texture AvatarInspector_RightLeg => GetEditorGUIUtilityIcon("AvatarInspector/RightLeg");
    public static Texture AvatarInspector_LeftLeg => GetEditorGUIUtilityIcon("AvatarInspector/LeftLeg");
    public static Texture AvatarInspector_Head => GetEditorGUIUtilityIcon("AvatarInspector/Head");
    public static Texture AvatarInspector_Torso => GetEditorGUIUtilityIcon("AvatarInspector/Torso");
    public static Texture AvatarInspector_MaskEditor_Root => GetEditorGUIUtilityIcon("AvatarInspector/MaskEditor_Root");
    public static Texture AvatarInspector_BodyPartPicker => GetEditorGUIUtilityIcon("AvatarInspector/BodyPartPicker");
    public static Texture AvatarInspector_BodySIlhouette => GetEditorGUIUtilityIcon("AvatarInspector/BodySIlhouette");
    public static Texture Mirror => GetEditorGUIUtilityIcon("Mirror");
    public static Texture SpeedScale => GetEditorGUIUtilityIcon("SpeedScale");
    public static Texture ToolbarMinus => GetEditorGUIUtilityIcon("Toolbar Minus");
    public static Texture ToolbarPlusMore => GetEditorGUIUtilityIcon("Toolbar Plus More");
    public static Texture ToolbarPlus => GetEditorGUIUtilityIcon("Toolbar Plus");
    public static Texture AnimatorControllerIcon => GetEditorGUIUtilityIcon("AnimatorController Icon");
    public static Texture TextAssetIcon => GetEditorGUIUtilityIcon("TextAsset Icon");
    public static Texture ShaderIcon => GetEditorGUIUtilityIcon("Shader Icon");
    public static Texture BooScriptIcon => GetEditorGUIUtilityIcon("boo Script Icon");
    public static Texture CsScriptIcon => GetEditorGUIUtilityIcon("cs Script Icon");
    public static Texture JsScriptIcon => GetEditorGUIUtilityIcon("js Script Icon");
    public static Texture PrefabIcon => GetEditorGUIUtilityIcon("Prefab Icon");
    public static Texture Profiler_NextFrame => GetEditorGUIUtilityIcon("Profiler.NextFrame");
    public static Texture Profiler_PrevFrame => GetEditorGUIUtilityIcon("Profiler.PrevFrame");
    public static Texture SvIconNone => GetEditorGUIUtilityIcon("sv_icon_none");
    public static Texture ColorPicker_CycleSlider => GetEditorGUIUtilityIcon("ColorPicker.CycleSlider");
    public static Texture ColorPicker_CycleColor => GetEditorGUIUtilityIcon("ColorPicker.CycleColor");
    public static Texture EyeDropper_Large => GetEditorGUIUtilityIcon("EyeDropper.Large");
    public static Texture ClothInspector_PaintValue => GetEditorGUIUtilityIcon("ClothInspector.PaintValue");
    public static Texture ClothInspector_ViewValue => GetEditorGUIUtilityIcon("ClothInspector.ViewValue");
    public static Texture ClothInspector_SettingsTool => GetEditorGUIUtilityIcon("ClothInspector.SettingsTool");
    public static Texture ClothInspector_PaintTool => GetEditorGUIUtilityIcon("ClothInspector.PaintTool");
    public static Texture ClothInspector_SelectTool => GetEditorGUIUtilityIcon("ClothInspector.SelectTool");
    public static Texture WelcomeScreen_AssetStoreLogo => GetEditorGUIUtilityIcon("WelcomeScreen.AssetStoreLogo");
    public static Texture AboutWindow_MainHeader => GetEditorGUIUtilityIcon("AboutWindow.MainHeader");
    public static Texture UnityLogo => GetEditorGUIUtilityIcon("UnityLogo");
    public static Texture AgeiaLogo => GetEditorGUIUtilityIcon("AgeiaLogo");
    public static Texture MonoLogo => GetEditorGUIUtilityIcon("MonoLogo");
    //public static Texture PlayButtonProfileAnim => GetEditorGUIUtilityIcon("PlayButtonProfile Anim");
    //public static Texture StepButtonAnim => GetEditorGUIUtilityIcon("StepButton Anim");
    //public static Texture PauseButtonAnim => GetEditorGUIUtilityIcon("PauseButton Anim");
    //public static Texture PlayButtonAnim => GetEditorGUIUtilityIcon("PlayButton Anim");
    public static Texture PlayButtonProfileOn => GetEditorGUIUtilityIcon("PlayButtonProfile On");
    public static Texture StepButtonOn => GetEditorGUIUtilityIcon("StepButton On");
    public static Texture PauseButtonOn => GetEditorGUIUtilityIcon("PauseButton On");
    public static Texture PlayButtonOn => GetEditorGUIUtilityIcon("PlayButton On");
    public static Texture PlayButtonProfile => GetEditorGUIUtilityIcon("PlayButtonProfile");
    public static Texture StepButton => GetEditorGUIUtilityIcon("StepButton");
    public static Texture PauseButton => GetEditorGUIUtilityIcon("PauseButton");
    public static Texture PlayButton => GetEditorGUIUtilityIcon("PlayButton");
    public static Texture ViewToolOrbitOn => GetEditorGUIUtilityIcon("ViewToolOrbit On");
    public static Texture ViewToolZoomOn => GetEditorGUIUtilityIcon("ViewToolZoom On");
    public static Texture ViewToolMoveOn => GetEditorGUIUtilityIcon("ViewToolMove On");
    public static Texture ViewToolOrbit => GetEditorGUIUtilityIcon("ViewToolOrbit");
    public static Texture ViewToolZoom => GetEditorGUIUtilityIcon("ViewToolZoom");
    public static Texture ViewToolMove => GetEditorGUIUtilityIcon("ViewToolMove");
    public static Texture ScaleToolOn => GetEditorGUIUtilityIcon("ScaleTool On");
    public static Texture RotateToolOn => GetEditorGUIUtilityIcon("RotateTool On");
    public static Texture MoveToolOn => GetEditorGUIUtilityIcon("MoveTool On");
    public static Texture ScaleTool => GetEditorGUIUtilityIcon("ScaleTool");
    public static Texture RotateTool => GetEditorGUIUtilityIcon("RotateTool");
    public static Texture MoveTool => GetEditorGUIUtilityIcon("MoveTool");
    public static Texture IconDropdown => GetEditorGUIUtilityIcon("Icon Dropdown");
    public static Texture AvatarIcon => GetEditorGUIUtilityIcon("Avatar Icon");
    public static Texture AvatarPivot => GetEditorGUIUtilityIcon("AvatarPivot");
    public static Texture AvatarInspector_DotSelection => GetEditorGUIUtilityIcon("AvatarInspector/DotSelection");
    public static Texture AvatarInspector_DotFrameDotted => GetEditorGUIUtilityIcon("AvatarInspector/DotFrameDotted");
    public static Texture AvatarInspector_DotFrame => GetEditorGUIUtilityIcon("AvatarInspector/DotFrame");
    public static Texture AvatarInspector_DotFill => GetEditorGUIUtilityIcon("AvatarInspector/DotFill");
    public static Texture AvatarInspector_RightHandZoom => GetEditorGUIUtilityIcon("AvatarInspector/RightHandZoom");
    public static Texture AvatarInspector_LeftHandZoom => GetEditorGUIUtilityIcon("AvatarInspector/LeftHandZoom");
    public static Texture AvatarInspector_HeadZoom => GetEditorGUIUtilityIcon("AvatarInspector/HeadZoom");
    public static Texture AvatarInspector_RightHandZoomSilhouette => GetEditorGUIUtilityIcon("AvatarInspector/RightHandZoomSilhouette");
    public static Texture AvatarInspector_LeftHandZoomSilhouette => GetEditorGUIUtilityIcon("AvatarInspector/LeftHandZoomSilhouette");
    public static Texture AvatarInspector_HeadZoomSilhouette => GetEditorGUIUtilityIcon("AvatarInspector/HeadZoomSilhouette");
    public static Texture AvatarInspector_BodySilhouette => GetEditorGUIUtilityIcon("AvatarInspector/BodySilhouette");
    public static Texture Animation_AddKeyframe => GetEditorGUIUtilityIcon("Animation.AddKeyframe");
    public static Texture Animation_NextKey => GetEditorGUIUtilityIcon("Animation.NextKey");
    public static Texture Animation_PrevKey => GetEditorGUIUtilityIcon("Animation.PrevKey");
    public static Texture LightMeter_RedLight => GetEditorGUIUtilityIcon("lightMeter/redLight");
    public static Texture LightMeter_OrangeLight => GetEditorGUIUtilityIcon("lightMeter/orangeLight");
    public static Texture LightMeter_LightRim => GetEditorGUIUtilityIcon("lightMeter/lightRim");
    public static Texture LightMeter_GreenLight => GetEditorGUIUtilityIcon("lightMeter/greenLight");
    public static Texture Animation_AddEvent => GetEditorGUIUtilityIcon("Animation.AddEvent");
    public static Texture SceneviewAudio => GetEditorGUIUtilityIcon("SceneviewAudio");
    public static Texture SceneviewLighting => GetEditorGUIUtilityIcon("SceneviewLighting");
    public static Texture MeshRendererIcon => GetEditorGUIUtilityIcon("MeshRenderer Icon");
    public static Texture TerrainIcon => GetEditorGUIUtilityIcon("Terrain Icon");
    public static Texture BuildSettings_SelectedIcon => GetEditorGUIUtilityIcon("BuildSettings.SelectedIcon");
    public static Texture Animation_Record => GetEditorGUIUtilityIcon("Animation.Record");
    public static Texture Animation_Play => GetEditorGUIUtilityIcon("Animation.Play");
    public static Texture PreTextureRGB => GetEditorGUIUtilityIcon("PreTextureRGB");
    public static Texture PreTextureAlpha => GetEditorGUIUtilityIcon("PreTextureAlpha");
    public static Texture PreTextureMipMapHigh => GetEditorGUIUtilityIcon("PreTextureMipMapHigh");
    public static Texture PreTextureMipMapLow => GetEditorGUIUtilityIcon("PreTextureMipMapLow");
    public static Texture TerrainInspector_TerrainToolSettings => GetEditorGUIUtilityIcon("TerrainInspector.TerrainToolSettings");
    public static Texture TerrainInspector_TerrainToolPlants => GetEditorGUIUtilityIcon("TerrainInspector.TerrainToolPlants");
    public static Texture TerrainInspector_TerrainToolTrees => GetEditorGUIUtilityIcon("TerrainInspector.TerrainToolTrees");
    public static Texture TerrainInspector_TerrainToolSplat => GetEditorGUIUtilityIcon("TerrainInspector.TerrainToolSplat");
    public static Texture TerrainInspector_TerrainToolSmoothHeight => GetEditorGUIUtilityIcon("TerrainInspector.TerrainToolSmoothHeight");
    public static Texture TerrainInspector_TerrainToolSetHeight => GetEditorGUIUtilityIcon("TerrainInspector.TerrainToolSetHeight");
    public static Texture TerrainInspector_TerrainToolRaise => GetEditorGUIUtilityIcon("TerrainInspector.TerrainToolRaise");
    public static Texture SettingsIcon => GetEditorGUIUtilityIcon("SettingsIcon");
    public static Texture PreMatLight1 => GetEditorGUIUtilityIcon("PreMatLight1");
    public static Texture PreMatLight0 => GetEditorGUIUtilityIcon("PreMatLight0");
    public static Texture PreMatTorus => GetEditorGUIUtilityIcon("PreMatTorus");
    public static Texture PreMatCylinder => GetEditorGUIUtilityIcon("PreMatCylinder");
    public static Texture PreMatCube => GetEditorGUIUtilityIcon("PreMatCube");
    public static Texture PreMatSphere => GetEditorGUIUtilityIcon("PreMatSphere");
    public static Texture CameraIcon => GetEditorGUIUtilityIcon("Camera Icon");
    public static Texture Animation_EventMarker => GetEditorGUIUtilityIcon("Animation.EventMarker");
    public static Texture ASBadgeNew => GetEditorGUIUtilityIcon("AS Badge New");
    //public static Texture ASBadgeMove => GetEditorGUIUtilityIcon("AS Badge Move");
    public static Texture ASBadgeDelete => GetEditorGUIUtilityIcon("AS Badge Delete");
    public static Texture WaitSpin00 => GetEditorGUIUtilityIcon("WaitSpin00");
    public static Texture WaitSpin01 => GetEditorGUIUtilityIcon("WaitSpin01");
    public static Texture WaitSpin02 => GetEditorGUIUtilityIcon("WaitSpin02");
    public static Texture WaitSpin03 => GetEditorGUIUtilityIcon("WaitSpin03");
    public static Texture WaitSpin04 => GetEditorGUIUtilityIcon("WaitSpin04");
    public static Texture WaitSpin05 => GetEditorGUIUtilityIcon("WaitSpin05");
    public static Texture WaitSpin06 => GetEditorGUIUtilityIcon("WaitSpin06");
    public static Texture WaitSpin07 => GetEditorGUIUtilityIcon("WaitSpin07");
    public static Texture WaitSpin08 => GetEditorGUIUtilityIcon("WaitSpin08");
    public static Texture WaitSpin09 => GetEditorGUIUtilityIcon("WaitSpin09");
    public static Texture WaitSpin10 => GetEditorGUIUtilityIcon("WaitSpin10");
    public static Texture WaitSpin11 => GetEditorGUIUtilityIcon("WaitSpin11");
    //public static Texture WelcomeScreen_UnityAnswersLogo => GetEditorGUIUtilityIcon("WelcomeScreen.UnityAnswersLogo");
    //public static Texture WelcomeScreen_UnityForumLogo => GetEditorGUIUtilityIcon("WelcomeScreen.UnityForumLogo");
    //public static Texture WelcomeScreen_UnityBasicsLogo => GetEditorGUIUtilityIcon("WelcomeScreen.UnityBasicsLogo");
    //public static Texture WelcomeScreen_VideoTutLogo => GetEditorGUIUtilityIcon("WelcomeScreen.VideoTutLogo");
    //public static Texture WelcomeScreen_MainHeader => GetEditorGUIUtilityIcon("WelcomeScreen.MainHeader");
    public static Texture VerticalSplit => GetEditorGUIUtilityIcon("VerticalSplit");
    public static Texture HorizontalSplit => GetEditorGUIUtilityIcon("HorizontalSplit");
    //public static Texture PrefabNormalIcon => GetEditorGUIUtilityIcon("PrefabNormal Icon");
    public static Texture PrefabModelIcon => GetEditorGUIUtilityIcon("PrefabModel Icon");
    public static Texture GameObjectIcon => GetEditorGUIUtilityIcon("GameObject Icon");
    public static Texture PreAudioLoopOn => GetEditorGUIUtilityIcon("preAudioLoopOn");
    public static Texture PreAudioLoopOff => GetEditorGUIUtilityIcon("preAudioLoopOff");
    public static Texture PreAudioAutoPlayOn => GetEditorGUIUtilityIcon("preAudioAutoPlayOn");
    public static Texture PreAudioAutoPlayOff => GetEditorGUIUtilityIcon("preAudioAutoPlayOff");
    public static Texture BuildSettings_Web_Small => GetEditorGUIUtilityIcon("BuildSettings.Web.Small");
    public static Texture BuildSettings_Standalone_Small => GetEditorGUIUtilityIcon("BuildSettings.Standalone.Small");
    public static Texture BuildSettings_IPhone_Small => GetEditorGUIUtilityIcon("BuildSettings.iPhone.Small");
    public static Texture BuildSettings_Android_Small => GetEditorGUIUtilityIcon("BuildSettings.Android.Small");
    //public static Texture BuildSettings_BlackBerry_Small => GetEditorGUIUtilityIcon("BuildSettings.BlackBerry.Small");
    //public static Texture BuildSettings_Tizen_Small => GetEditorGUIUtilityIcon("BuildSettings.Tizen.Small");
    public static Texture BuildSettings_XBox360_Small => GetEditorGUIUtilityIcon("BuildSettings.XBox360.Small");
    public static Texture BuildSettings_XboxOne_Small => GetEditorGUIUtilityIcon("BuildSettings.XboxOne.Small");
    //public static Texture BuildSettings_PS3_Small => GetEditorGUIUtilityIcon("BuildSettings.PS3.Small");
    public static Texture BuildSettings_PSP2_Small => GetEditorGUIUtilityIcon("BuildSettings.PSP2.Small");
    public static Texture BuildSettings_PS4_Small => GetEditorGUIUtilityIcon("BuildSettings.PS4.Small");
    public static Texture BuildSettings_PSM_Small => GetEditorGUIUtilityIcon("BuildSettings.PSM.Small");
    public static Texture BuildSettings_FlashPlayer_Small => GetEditorGUIUtilityIcon("BuildSettings.FlashPlayer.Small");
    public static Texture BuildSettings_Metro_Small => GetEditorGUIUtilityIcon("BuildSettings.Metro.Small");
    public static Texture BuildSettings_WP8_Small => GetEditorGUIUtilityIcon("BuildSettings.WP8.Small");
    //public static Texture BuildSettings_SamsungTV_Small => GetEditorGUIUtilityIcon("BuildSettings.SamsungTV.Small");
    public static Texture BuildSettings_Web => GetEditorGUIUtilityIcon("BuildSettings.Web");
    public static Texture BuildSettings_Standalone => GetEditorGUIUtilityIcon("BuildSettings.Standalone");
    public static Texture BuildSettings_IPhone => GetEditorGUIUtilityIcon("BuildSettings.iPhone");
    public static Texture BuildSettings_Android => GetEditorGUIUtilityIcon("BuildSettings.Android");
    //public static Texture BuildSettings_BlackBerry => GetEditorGUIUtilityIcon("BuildSettings.BlackBerry");
    //public static Texture BuildSettings_Tizen => GetEditorGUIUtilityIcon("BuildSettings.Tizen");
    public static Texture BuildSettings_XBox360 => GetEditorGUIUtilityIcon("BuildSettings.XBox360");
    public static Texture BuildSettings_XboxOne => GetEditorGUIUtilityIcon("BuildSettings.XboxOne");
    //public static Texture BuildSettings_PS3 => GetEditorGUIUtilityIcon("BuildSettings.PS3");
    public static Texture BuildSettings_PSP2 => GetEditorGUIUtilityIcon("BuildSettings.PSP2");
    public static Texture BuildSettings_PS4 => GetEditorGUIUtilityIcon("BuildSettings.PS4");
    public static Texture BuildSettings_PSM => GetEditorGUIUtilityIcon("BuildSettings.PSM");
    public static Texture BuildSettings_FlashPlayer => GetEditorGUIUtilityIcon("BuildSettings.FlashPlayer");
    public static Texture BuildSettings_Metro => GetEditorGUIUtilityIcon("BuildSettings.Metro");
    public static Texture BuildSettings_WP8 => GetEditorGUIUtilityIcon("BuildSettings.WP8");
    //public static Texture BuildSettings_SamsungTV => GetEditorGUIUtilityIcon("BuildSettings.SamsungTV");
    public static Texture TreeEditor_BranchTranslate => GetEditorGUIUtilityIcon("TreeEditor.BranchTranslate");
    public static Texture TreeEditor_BranchRotate => GetEditorGUIUtilityIcon("TreeEditor.BranchRotate");
    public static Texture TreeEditor_BranchFreeHand => GetEditorGUIUtilityIcon("TreeEditor.BranchFreeHand");
    public static Texture TreeEditor_BranchTranslateOn => GetEditorGUIUtilityIcon("TreeEditor.BranchTranslate On");
    public static Texture TreeEditor_BranchRotateOn => GetEditorGUIUtilityIcon("TreeEditor.BranchRotate On");
    public static Texture TreeEditor_BranchFreeHandOn => GetEditorGUIUtilityIcon("TreeEditor.BranchFreeHand On");
    public static Texture TreeEditor_LeafTranslate => GetEditorGUIUtilityIcon("TreeEditor.LeafTranslate");
    public static Texture TreeEditor_LeafRotate => GetEditorGUIUtilityIcon("TreeEditor.LeafRotate");
    public static Texture TreeEditor_LeafTranslateOn => GetEditorGUIUtilityIcon("TreeEditor.LeafTranslate On");
    public static Texture TreeEditor_LeafRotateOn => GetEditorGUIUtilityIcon("TreeEditor.LeafRotate On");
    public static Texture SvIconDot15Pix16Gizmo => GetEditorGUIUtilityIcon("sv_icon_dot15_pix16_gizmo");
    public static Texture SvIconDot1Sml => GetEditorGUIUtilityIcon("sv_icon_dot1_sml");
    public static Texture SvIconDot4Sml => GetEditorGUIUtilityIcon("sv_icon_dot4_sml");
    public static Texture SvIconDot7Sml => GetEditorGUIUtilityIcon("sv_icon_dot7_sml");
    public static Texture SvIconDot5Pix16Gizmo => GetEditorGUIUtilityIcon("sv_icon_dot5_pix16_gizmo");
    public static Texture SvIconDot11Pix16Gizmo => GetEditorGUIUtilityIcon("sv_icon_dot11_pix16_gizmo");
    public static Texture SvIconDot12Sml => GetEditorGUIUtilityIcon("sv_icon_dot12_sml");
    public static Texture SvIconDot15Sml => GetEditorGUIUtilityIcon("sv_icon_dot15_sml");
    public static Texture SvIconDot9Pix16Gizmo => GetEditorGUIUtilityIcon("sv_icon_dot9_pix16_gizmo");
    public static Texture SvIconName6 => GetEditorGUIUtilityIcon("sv_icon_name6");
    public static Texture SvIconName3 => GetEditorGUIUtilityIcon("sv_icon_name3");
    public static Texture SvIconName4 => GetEditorGUIUtilityIcon("sv_icon_name4");
    public static Texture SvIconName0 => GetEditorGUIUtilityIcon("sv_icon_name0");
    public static Texture SvIconName1 => GetEditorGUIUtilityIcon("sv_icon_name1");
    public static Texture SvIconName2 => GetEditorGUIUtilityIcon("sv_icon_name2");
    public static Texture SvIconName5 => GetEditorGUIUtilityIcon("sv_icon_name5");
    public static Texture SvIconName7 => GetEditorGUIUtilityIcon("sv_icon_name7");
    public static Texture SvIconDot1Pix16Gizmo => GetEditorGUIUtilityIcon("sv_icon_dot1_pix16_gizmo");
    public static Texture SvIconDot8Pix16Gizmo => GetEditorGUIUtilityIcon("sv_icon_dot8_pix16_gizmo");
    public static Texture SvIconDot2Pix16Gizmo => GetEditorGUIUtilityIcon("sv_icon_dot2_pix16_gizmo");
    public static Texture SvIconDot6Pix16Gizmo => GetEditorGUIUtilityIcon("sv_icon_dot6_pix16_gizmo");
    public static Texture SvIconDot0Sml => GetEditorGUIUtilityIcon("sv_icon_dot0_sml");
    public static Texture SvIconDot3Sml => GetEditorGUIUtilityIcon("sv_icon_dot3_sml");
    public static Texture SvIconDot6Sml => GetEditorGUIUtilityIcon("sv_icon_dot6_sml");
    public static Texture SvIconDot9Sml => GetEditorGUIUtilityIcon("sv_icon_dot9_sml");
    public static Texture SvIconDot11Sml => GetEditorGUIUtilityIcon("sv_icon_dot11_sml");
    public static Texture SvIconDot14Sml => GetEditorGUIUtilityIcon("sv_icon_dot14_sml");
    public static Texture SvLabel0 => GetEditorGUIUtilityIcon("sv_label_0");
    public static Texture SvLabel1 => GetEditorGUIUtilityIcon("sv_label_1");
    public static Texture SvLabel2 => GetEditorGUIUtilityIcon("sv_label_2");
    public static Texture SvLabel3 => GetEditorGUIUtilityIcon("sv_label_3");
    public static Texture SvLabel5 => GetEditorGUIUtilityIcon("sv_label_5");
    public static Texture SvLabel6 => GetEditorGUIUtilityIcon("sv_label_6");
    public static Texture SvLabel7 => GetEditorGUIUtilityIcon("sv_label_7");
    public static Texture SvIconDot14Pix16Gizmo => GetEditorGUIUtilityIcon("sv_icon_dot14_pix16_gizmo");
    public static Texture SvIconDot7Pix16Gizmo => GetEditorGUIUtilityIcon("sv_icon_dot7_pix16_gizmo");
    public static Texture SvIconDot3Pix16Gizmo => GetEditorGUIUtilityIcon("sv_icon_dot3_pix16_gizmo");
    public static Texture SvIconDot0Pix16Gizmo => GetEditorGUIUtilityIcon("sv_icon_dot0_pix16_gizmo");
    public static Texture SvIconDot2Sml => GetEditorGUIUtilityIcon("sv_icon_dot2_sml");
    public static Texture SvIconDot5Sml => GetEditorGUIUtilityIcon("sv_icon_dot5_sml");
    public static Texture SvIconDot8Sml => GetEditorGUIUtilityIcon("sv_icon_dot8_sml");
    public static Texture SvIconDot10Pix16Gizmo => GetEditorGUIUtilityIcon("sv_icon_dot10_pix16_gizmo");
    public static Texture SvIconDot12Pix16Gizmo => GetEditorGUIUtilityIcon("sv_icon_dot12_pix16_gizmo");
    public static Texture SvIconDot10Sml => GetEditorGUIUtilityIcon("sv_icon_dot10_sml");
    public static Texture SvIconDot13Sml => GetEditorGUIUtilityIcon("sv_icon_dot13_sml");
    public static Texture SvIconDot4Pix16Gizmo => GetEditorGUIUtilityIcon("sv_icon_dot4_pix16_gizmo");
    public static Texture SvLabel4 => GetEditorGUIUtilityIcon("sv_label_4");
    public static Texture SvIconDot13Pix16Gizmo => GetEditorGUIUtilityIcon("sv_icon_dot13_pix16_gizmo");

    #endregion

    public static IconPreview[] iconPreviews;
    public static GUIStyle iconPreviewStyle;
    public static Material iconMat; 
    public Vector2 scrollPosition;
    public float iconSize = 30;

    [MenuItem("Tools/Editor Icons Preview")]
    public static void ShowWindow() => GetWindow<EditorIcons>("Editor Icons Preview");

    void OnGUI()
    {
        float windowWidth = EditorGUIUtility.currentViewWidth - EditorGUIUtility.singleLineHeight;
        float currentPosition = 0;

        //EditorGUILayout.LabelField("Preview of all EditorIcons", EditorStyles.boldLabel);
        iconSize = EditorGUILayout.Slider(iconSize, 10, 100);
        currentPosition += EditorGUIUtility.singleLineHeight + 2;

        scrollPosition = GUILayout.BeginScrollView(scrollPosition);
        currentPosition += scrollPosition.y;

        if (iconPreviews == null)
            iconPreviews = GetAllIconPreviews();
        if (iconPreviewStyle == null)
        {
            iconPreviewStyle = new GUIStyle(EditorStyles.iconButton);
            iconPreviewStyle.fixedWidth = 20f;
            iconPreviewStyle.fixedHeight = iconPreviewStyle.fixedWidth;
            
        }
        if (iconMat == null)
        {
            iconMat = new Material(Shader.Find("Unlit/Transparent"));
        }

        //scrollPosition = EditorGUI.be
        float padding = 2f;
        Rect currentTextureArea = new Rect(0, 30, iconSize, iconSize);
        foreach (IconPreview icon in iconPreviews)
        {
            float ratio = (float)icon.texture.width / icon.texture.height;
            currentTextureArea.width = iconSize * ratio;
             
            Rect clippedRect = currentTextureArea;
            clippedRect.y = Mathf.Max(clippedRect.y, currentPosition - iconSize);
            clippedRect.height = Mathf.Min(clippedRect.height, clippedRect.y - (currentPosition - iconSize));
            clippedRect.y += iconSize - clippedRect.height;

            if (clippedRect.height > 0)
            {
                EditorGUI.HelpBox(clippedRect, "", MessageType.None);
                EditorGUI.LabelField(currentTextureArea, new GUIContent() { tooltip = icon.fieldName } , new GUIStyle());
                EditorGUI.DrawPreviewTexture(clippedRect, icon.texture, iconMat);
            }

            currentTextureArea.x += currentTextureArea.width + padding;
            if (currentTextureArea.x + currentTextureArea.width > windowWidth)
            {
                currentTextureArea.x = 0;
                currentTextureArea.y += iconSize + padding;
            }
        }


        GUILayout.Space((currentTextureArea.y - currentPosition) + (iconSize + padding) * (iconSize / 3));

        GUILayout.EndScrollView();

        // Add more buttons for other tools as needed
    }

    [Shortcut("DEBUG STRINGS", typeof(SceneView), KeyCode.J, ShortcutModifiers.Alt)]
    public static void GeneratingString()
    { 
        string startPrintingNamesAtName = "TreeEditor.BranchRotate";
        string fields = "Mirror\r\nSpeedScale\r\nToolbar Minus\r\nToolbar Plus More\r\n" +
            "Toolbar Plus\r\nAnimatorController Icon\r\nTextAsset Icon\r\nShader Icon\r\n" +
            "boo Script Icon\r\ncs Script Icon\r\njs Script Icon\r\nPrefab Icon\r\nProfiler.NextFrame" +
            "\r\nProfiler.PrevFrame\r\nsv_icon_none\r\nColorPicker.CycleSlider\r\nColorPicker.CycleColor" +
            "\r\nEyeDropper.Large\r\nClothInspector.PaintValue\r\nClothInspector.ViewValue\r\nClothInspector.SettingsTool" +
            "\r\nClothInspector.PaintTool\r\nClothInspector.SelectTool\r\nWelcomeScreen.AssetStoreLogo\r\n" +
            "WelcomeScreen.AssetStoreLogo\r\nAboutWindow.MainHeader\r\nUnityLogo\r\nAgeiaLogo\r\nMonoLogo\r\nToolbar Minus" +
            "\r\nPlayButtonProfile Anim\r\nStepButton Anim\r\nPauseButton Anim\r\nPlayButton Anim\r\nPlayButtonProfile On" +
            "\r\nStepButton On\r\nPauseButton On\r\nPlayButton On\r\nPlayButtonProfile\r\nStepButton\r\nPauseButton\r\nPlayButton" +
            "\r\nViewToolOrbit On\r\nViewToolZoom On\r\nViewToolMove On\r\nViewToolOrbit On\r\nViewToolOrbit\r\nViewToolZoom" +
            "\r\nViewToolMove\r\nViewToolOrbit\r\nScaleTool On\r\nRotateTool On\r\nMoveTool On\r\nScaleTool\r\nRotateTool\r\n" +
            "MoveTool\r\nPauseButton\r\nPlayButton\r\nToolbar Minus\r\nToolbar Plus\r\nUnityLogo\r\nIcon Dropdown\r\n" +
            "Avatar Icon\r\nAvatarPivot\r\nSpeedScale\r\nAvatarInspector/DotSelection\r\nAvatarInspector/DotFrameDotted" +
            "\r\nAvatarInspector/DotFrame\r\nAvatarInspector/DotFill\r\nAvatarInspector/RightHandZoom\r\n" +
            "AvatarInspector/LeftHandZoom\r\nAvatarInspector/HeadZoom\r\nAvatarInspector/RightLeg\r\nAvatarInspector/LeftLeg" +
            "\r\nAvatarInspector/RightFingers\r\nAvatarInspector/RightArm\r\nAvatarInspector/LeftFingers\r\nAvatarInspector/LeftArm" +
            "\r\nAvatarInspector/Head\r\nAvatarInspector/Torso\r\nAvatarInspector/RightHandZoomSilhouette\r\n" +
            "AvatarInspector/LeftHandZoomSilhouette\r\nAvatarInspector/HeadZoomSilhouette\r\nAvatarInspector/BodySilhouette" +
            "\r\nAnimation.AddKeyframe\r\nAnimation.NextKey\r\nAnimation.PrevKey\r\nlightMeter/redLight\r\nlightMeter/orangeLight" +
            "\r\nlightMeter/lightRim\r\nlightMeter/greenLight\r\nAnimation.AddEvent\r\nSceneviewAudio\r\nSceneviewLighting\r\n" +
            "MeshRenderer Icon\r\nTerrain Icon\r\nsv_icon_none\r\nBuildSettings.SelectedIcon\r\nAnimation.AddEvent\r\n" +
            "Animation.AddKeyframe\r\nAnimation.NextKey\r\nAnimation.PrevKey\r\nAnimation.Record\r\nAnimation.Play\r\nPreTextureRGB" +
            "\r\nPreTextureAlpha\r\nPreTextureMipMapHigh\r\nPreTextureMipMapLow\r\nTerrainInspector.TerrainToolSettings\r\n" +
            "TerrainInspector.TerrainToolPlants\r\nTerrainInspector.TerrainToolTrees\r\nTerrainInspector.TerrainToolSplat\r\n" +
            "TerrainInspector.TerrainToolSmoothHeight\r\nTerrainInspector.TerrainToolSetHeight\r\nTerrainInspector.TerrainToolRaise" +
            "\r\nSettingsIcon\r\nPauseButton\r\nPlayButton\r\nPreMatLight1\r\nPreMatLight0\r\nPreMatTorus\r\nPreMatCylinder" +
            "\r\nPreMatCube\r\nPreMatSphere\r\nPreMatLight1\r\nPreMatLight0\r\nCamera Icon\r\nToolbar Minus\r\nToolbar Plus\r\n" +
            "Animation.EventMarker\r\nAS Badge New\r\nAS Badge Move\r\nAS Badge Delete\r\nWaitSpin00\r\nWaitSpin01\r\nWaitSpin02" +
            "\r\nWaitSpin03\r\nWaitSpin04\r\nWaitSpin05\r\nWaitSpin06\r\nWaitSpin07\r\nWaitSpin08\r\nWaitSpin09\r\nWaitSpin10" +
            "\r\nWaitSpin11\r\nWelcomeScreen.AssetStoreLogo\r\nWelcomeScreen.UnityAnswersLogo\r\nWelcomeScreen.UnityForumLogo" +
            "\r\nWelcomeScreen.UnityBasicsLogo\r\nWelcomeScreen.VideoTutLogo\r\nWelcomeScreen.MainHeader\r\nUnityLogo\r\n" +
            "preAudioPlayOn\r\npreAudioPlayOff\r\nAnimation.EventMarker\r\nPreMatLight1\r\nPreMatLight0\r\nPreMatTorus\r\n" +
            "PreMatCylinder\r\nPreMatCube\r\nPreMatSphere\r\nTreeEditor.Duplicate\r\nToolbar Minus\r\nToolbar Plus\r\n" +
            "PreTextureMipMapHigh\r\nPreTextureMipMapLow\r\nPreTextureRGB\r\nPreTextureAlpha\r\nVerticalSplit\r\nHorizontalSplit" +
            "\r\nIcon Dropdown\r\nPrefabNormal Icon\r\nPrefabModel Icon\r\nPrefabNormal Icon\r\nPrefab Icon\r\nGameObject Icon" +
            "\r\npreAudioLoopOn\r\npreAudioLoopOff\r\npreAudioPlayOn\r\npreAudioPlayOff\r\npreAudioAutoPlayOn\r\npreAudioAutoPlayOff" +
            "\r\nBuildSettings.Web.Small\r\nBuildSettings.Standalone.Small\r\nBuildSettings.iPhone.Small\r\n" +
            "BuildSettings.Android.Small\r\nBuildSettings.BlackBerry.Small\r\nBuildSettings.Tizen.Small\r\n" +
            "BuildSettings.XBox360.Small\r\nBuildSettings.XboxOne.Small\r\nBuildSettings.PS3.Small\r\nBuildSettings.PSP2.Small" +
            "\r\nBuildSettings.PS4.Small\r\nBuildSettings.PSM.Small\r\nBuildSettings.FlashPlayer.Small\r\nBuildSettings.Metro.Small" +
            "\r\nBuildSettings.WP8.Small\r\nBuildSettings.SamsungTV.Small\r\nBuildSettings.Web\r\nBuildSettings.Standalone\r\n" +
            "BuildSettings.iPhone\r\nBuildSettings.Android\r\nBuildSettings.BlackBerry\r\nBuildSettings.Tizen\r\nBuildSettings.XBox360" +
            "\r\nBuildSettings.XboxOne\r\nBuildSettings.PS3\r\nBuildSettings.PSP2\r\nBuildSettings.PS4\r\nBuildSettings.PSM\r\n" +
            "BuildSettings.FlashPlayer\r\nBuildSettings.Metro\r\nBuildSettings.WP8\r\nBuildSettings.SamsungTV\r\n" +
            "TreeEditor.BranchTranslate\r\nTreeEditor.BranchRotate\r\nTreeEditor.BranchFreeHand\r\nTreeEditor.BranchTranslate On" +
            "\r\nTreeEditor.BranchRotate On\r\nTreeEditor.BranchFreeHand On\r\nTreeEditor.LeafTranslate\r\nTreeEditor.LeafRotate" +
            "\r\nTreeEditor.LeafTranslate On\r\nTreeEditor.LeafRotate On\r\nsv_icon_dot15_pix16_gizmo\r\nsv_icon_dot1_sml\r\n" +
            "sv_icon_dot4_sml\r\nsv_icon_dot7_sml\r\nsv_icon_dot5_pix16_gizmo\r\nsv_icon_dot11_pix16_gizmo\r\nsv_icon_dot12_sml" +
            "\r\nsv_icon_dot15_sml\r\nsv_icon_dot9_pix16_gizmo\r\nsv_icon_name6\r\nsv_icon_name3\r\nsv_icon_name4\r\nsv_icon_name0" +
            "\r\nsv_icon_name1\r\nsv_icon_name2\r\nsv_icon_name5\r\nsv_icon_name7\r\nsv_icon_dot1_pix16_gizmo\r\nsv_icon_dot8_pix16_gizmo" +
            "\r\nsv_icon_dot2_pix16_gizmo\r\nsv_icon_dot6_pix16_gizmo\r\nsv_icon_dot0_sml\r\nsv_icon_dot3_sml\r\nsv_icon_dot6_sml\r\n" +
            "sv_icon_dot9_sml\r\nsv_icon_dot11_sml\r\nsv_icon_dot14_sml\r\nsv_label_0\r\nsv_label_1\r\nsv_label_2\r\nsv_label_3\r\n" +
            "sv_label_5\r\nsv_label_6\r\nsv_label_7\r\nsv_icon_none\r\nsv_icon_dot14_pix16_gizmo\r\nsv_icon_dot7_pix16_gizmo\r\n" +
            "sv_icon_dot3_pix16_gizmo\r\nsv_icon_dot0_pix16_gizmo\r\nsv_icon_dot2_sml\r\nsv_icon_dot5_sml\r\nsv_icon_dot8_sml\r\n" +
            "sv_icon_dot10_pix16_gizmo\r\nsv_icon_dot12_pix16_gizmo\r\nsv_icon_dot10_sml\r\nsv_icon_dot13_sml\r\nsv_icon_dot4_pix16_gizmo" +
            "\r\nsv_label_4\r\nsv_icon_dot13_pix16_gizmo";

        string generatedCode = "";

        string[] separatedNames = fields.Split("\r\n");
        List<string> alreadyUsedNames = new List<string>();


        foreach(string name in separatedNames)
        {
            if (alreadyUsedNames.Contains(name))
                continue;
            alreadyUsedNames.Add(name);

            if (!alreadyUsedNames.Contains(startPrintingNamesAtName))
                continue;

            string stringName = name;
            string fieldName = "";
            bool capitalizeNextChar = true;
            for (int i = 0; i < name.Length; i++)
            {
                char c = name[i];

                if (c == '_')
                {
                    capitalizeNextChar = true;
                    continue;
                }

                if (c == '.' || c == '/')
                    c = '_';

                if (c == ' ')
                {
                    capitalizeNextChar = true;
                    continue;
                }

                if (capitalizeNextChar)
                {
                    fieldName += c.ToString().ToUpper();
                    capitalizeNextChar = false;
                }
                else
                    fieldName += c;

                if (!char.IsLetter(c))
                {
                    capitalizeNextChar = true;
                }
            }

            generatedCode += $"public static Texture {fieldName} => GetEditorGUIUtilityIcon(\"{name}\");\n";
        }
        Debug.Log(generatedCode);
    }

    public IconPreview[] GetAllIconPreviews()
    {
        return typeof(EditorIcons).GetProperties(BindingFlags.Static | BindingFlags.Public)
                   .Where(prop => prop.PropertyType == typeof(Texture))
                   .Select(prop => new IconPreview(prop.GetValue(this) as Texture, prop.Name))
                   .ToArray();
    }


    private static Texture GetEditorGUIUtilityIcon(string name) => EditorGUIUtility.IconContent(name).image;

    public struct IconPreview
    {
        public Texture texture;
        public string fieldName;
        public IconPreview(Texture texture, string fieldName)
        {
            this.texture = texture;
            this.fieldName = fieldName;
        }

    }
}
