using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEngine.UI;

namespace Base.UIPackage
{
#if UNITY_EDITOR
    public class UIPackageEditorScripting
    {
        [MenuItem("GameObject/UIManagement/UISystem", false, 10)]
        public static void CreateUISystem()
        {
            Object playerPrefab = AssetDatabase.LoadAssetAtPath("Assets/Damini - UI Package/UI Package/Prefab/UI System.prefab", typeof(GameObject));
            //Instantiate prefab if it exists
            if (playerPrefab != null)
            {
                PrefabUtility.InstantiatePrefab(playerPrefab);
            }
        }


        [MenuItem("GameObject/UIManagement/UIScreens/UIPanel", false, 10)]
        public static void CreateUIPanel()
        {
            Object playerPrefab = AssetDatabase.LoadAssetAtPath("Assets/Damini - UI Package/UI Package/Prefab/UIPanel - Name.prefab", typeof(GameObject));
            //Instantiate prefab if it exists
            if (playerPrefab != null)
            {
                PrefabUtility.InstantiatePrefab(playerPrefab, Selection.activeGameObject.transform);
            }
        }

        [MenuItem("GameObject/UIManagement/UIScreens/UIPopup/UIEmptyPopup", false, 10)]
        public static void CreateUIEmptyPopup()
        {
            Object playerPrefab = AssetDatabase.LoadAssetAtPath("Assets/Damini - UI Package/UI Package/Prefab/Popup - Empty.prefab", typeof(GameObject));
            //Instantiate prefab if it exists
            if (playerPrefab != null)
            {
                PrefabUtility.InstantiatePrefab(playerPrefab, Selection.activeGameObject.transform).name = "UI Popup - Name"; ;
            }
        }

        [MenuItem("GameObject/UIManagement/UIScreens/UIPopup/UIInformativePopup", false, 10)]
        public static void CreateUIInformativePopup()
        {
            Object playerPrefab = AssetDatabase.LoadAssetAtPath("Assets/Damini - UI Package/UI Package/Prefab/Popup - Informative.prefab", typeof(GameObject));
            //Instantiate prefab if it exists
            if (playerPrefab != null)
            {
                PrefabUtility.InstantiatePrefab(playerPrefab, Selection.activeGameObject.transform).name = "UI Informative Popup - Name";
            }
        }

        [MenuItem("GameObject/UIManagement/UIScreens/UIPopup/UIAlertPopup", false, 10)]
        public static void CreateUIAlertPopup()
        {
            Object playerPrefab = AssetDatabase.LoadAssetAtPath("Assets/Damini - UI Package/UI Package/Prefab/Popup - Alert Variant.prefab", typeof(GameObject));
            //Instantiate prefab if it exists
            if (playerPrefab != null)
            {
                PrefabUtility.InstantiatePrefab(playerPrefab, Selection.activeGameObject.transform).name = "UI Alert Popup - Name";
            }
        }

        [MenuItem("GameObject/UIManagement/UIScreens/UIPopup/UIAffirmativePopup", false, 10)]
        public static void CreateUIAffirmativePopup()
        {
            Object playerPrefab = AssetDatabase.LoadAssetAtPath("Assets/Damini - UI Package/UI Package/Prefab/Popup - Affirmative Variant.prefab", typeof(GameObject));
            //Instantiate prefab if it exists
            if (playerPrefab != null)
            {
                PrefabUtility.InstantiatePrefab(playerPrefab, Selection.activeGameObject.transform).name = "UI Affirmative Popup - Name"; ;
            }
        }
        
        [MenuItem("GameObject/UIManagement/UIComponent/UIPanelNavigator", false, 10)]
        public static void CreateUIButtonNavigation()
        {
            GameObject go = new GameObject("Button - (UIPanelNavigator - Name)");
            go.transform.SetParent(Selection.activeGameObject.transform);
            go.AddComponent(typeof(UIPanelNavigator));
        }


        [MenuItem("GameObject/UIManagement/UIComponent/UIPopupNavigator", false, 10)]
        public static void CreateUIButtonOpenPopup()
        {
            GameObject go = new GameObject("Button - (UIPopupNavigator - Name)");
            go.transform.SetParent(Selection.activeGameObject.transform);
            go.AddComponent(typeof(UIPopupNavigator));
        }
    }
#endif
}
