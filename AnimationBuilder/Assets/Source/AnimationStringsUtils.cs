public static class AnimationStrings
{
    public const string FILE_CONTENTS_HEADER = 
        "%YAML 1.1\n% TAG !u! tag:unity3d.com,2011:\n" +
        "--- !u!74 &7400000\nAnimationClip:\n" +
        "  m_ObjectHideFlags: 0\n" +
        "  m_CorrespondingSourceObject: {fileID: 0}\n" +
        "  m_PrefabInternal: {fileID: 0}\n" +
        "  m_Name: ";

    public const string POST_ANIM_NAME_CONTENTS =
        "  serializedVersion: 6\n" +
        "  m_Legacy: 0\n" +
        "  m_Compressed: 0\n" +
        "  m_UseHighQualityCurve: 1\n" +
        "  m_RotationCurves: []\n" +
        "    m_CompressedRotationCurves: []\n" +
        "    m_EulerCurves:\n" +
        "  - curve:\n" +
        "      serializedVersion: 2\n" +
        "      m_Curve:";

    public const string CURVE_TEMPLATE =
        "      - serializedVersion: 3\n" +
        "        time: 0\n" +
        "        value: {x: {0}, y: {1}, z: {2}}\n" +
        "        inSlope: {x: 0, y: 0, z: 0}\n" +
        "        outSlope: {x: 0, y: 0, z: 0}\n" +
        "        tangentMode: 0\n" +
        "        weightedMode: 0\n" +
        "        inWeight: {x: 0.33333334, y: 0.33333334, z: 0.33333334}\n" +
        "        outWeight: {x: 0.33333334, y: 0.33333334, z: 0.33333334}";

    public const string CURVE_POS_SEP =
        "      m_PreInfinity: 2\n" +
        "      m_PostInfinity: 2\n" +
        "      m_RotationOrder: 4\n" +
        "    path: \n" +
        "  m_PositionCurves:\n" +
        "  - curve:\n" +
        "      serializedVersion: 2\n" +
        "      m_Curve:";

    public const string STOP_TIME_PREFIX =
        "      m_PreInfinity: 2\n" +
        "      m_PostInfinity: 2\n" +
        "      m_RotationOrder: 4\n" +
        "    path: \n" +
        "  m_ScaleCurves: []\n" +
        "    m_FloatCurves: []\n" +
        "    m_PPtrCurves: []\n" +
        "    m_SampleRate: 60\n" +
        "  m_WrapMode: 0\n" +
        "  m_Bounds:\n" +
        "    m_Center: {x: 0, y: 0, z: 0}\n" +
        "    m_Extent: {x: 0, y: 0, z: 0}\n" +
        "  m_ClipBindingConstant:\n" +
        "    genericBindings:\n" +
        "    - serializedVersion: 2\n" +
        "      path: 0\n" +
        "      attribute: 1\n" +
        "      script: {fileID: 0}\n" +
        "      typeID: 4\n" +
        "      customType: 0\n" +
        "      isPPtrCurve: 0\n" +
        "    - serializedVersion: 2\n" +
        "      path: 0\n" +
        "      attribute: 4\n" +
        "      script: {fileID: 0}\n" +
        "      typeID: 4\n" +
        "      customType: 4\n" +
        "      isPPtrCurve: 0\n" +
        "    pptrCurveMapping: []\n" +
        "  m_AnimationClipSettings:\n" +
        "    serializedVersion: 2\n" +
        "    m_AdditiveReferencePoseClip: {fileID: 0}\n" +
        "    m_AdditiveReferencePoseTime: 0\n" +
        "    m_StartTime: 0\n" +
        "    m_StopTime: ";

    public const string EDITOR_CURVES_PREFIX =
        "    m_OrientationOffsetY: 0\n" +
        "    m_Level: 0\n" +
        "    m_CycleOffset: 0\n" +
        "    m_HasAdditiveReferencePose: 0\n" +
        "    m_LoopTime: 1\n" +
        "    m_LoopBlend: 0\n" +
        "    m_LoopBlendOrientation: 0\n" +
        "    m_LoopBlendPositionY: 0\n" +
        "    m_LoopBlendPositionXZ: 0\n" +
        "    m_KeepOriginalOrientation: 0\n" +
        "    m_KeepOriginalPositionY: 1\n" +
        "    m_KeepOriginalPositionXZ: 0\n" +
        "    m_HeightFromFeet: 0\n" +
        "    m_Mirror: 0\n" +
        "  m_EditorCurves:\n" +
        "  - curve:\n" +
        "      serializedVersion: 2\n" +
        "      m_Curve:";

    public const string EDITOR_CURVE_TEMPLATE =
        "      - serializedVersion: 3\n" +
        "        time: {0}\n" +
        "        value: 0\n" +
        "        inSlope: 0\n" +
        "        outSlope: 0\n" +
        "        tangentMode: 136\n" +
        "        weightedMode: 0\n" +
        "        inWeight: 0.33333334\n" +
        "        outWeight: 0.33333334";
}
