using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using UnityEditor;
using UnityEditor.Experimental.SceneManagement;
using UnityEditor.IMGUI.Controls;
using UnityEngine;


public class FinderData
{
	public string path;
	public List<string> componentPath = new List<string>();
}
public class ComponentFieldInfo
{
	public FieldInfo fieldInfo;
	public MethodInfo method;
}
public class ComponentFinder
{
	static string path = Application.dataPath.Substring(0, Application.dataPath.Length - "Assets".Length);
	private const string PREFAB_FILE_SEARCH_PATTERN = "t:Prefab";
	private static List<FinderData> Find(Type type, string findSctiptType, string findPath = "", Func<Component, bool> func = null)
	{
		if (string.IsNullOrEmpty(findPath))
			findPath = Application.dataPath;
		if (type == null) return null;
		List<FinderData> output = new List<FinderData>();
		string[] list = Directory.GetFiles(findPath, "*.*", SearchOption.AllDirectories).Where(s => s.ToLower().EndsWith(".prefab")).ToArray();
		try
		{
			for (int i = 0; i < list.Length; ++i)
			{
				string file = list[i];
				file = file.Replace("\\", "/");
				if (file.Contains(Application.dataPath) == true)
					file = file.Replace(Application.dataPath, "Assets");
				EditorUtility.DisplayProgressBar($"{findSctiptType} Find Prefabs", file.Substring(file.LastIndexOf("/") + 1), i / (float)list.Length);
				GameObject obj = AssetDatabase.LoadAssetAtPath<GameObject>(file) as GameObject;
				if (obj == null) continue;
				Component[] componentList = obj.GetComponentsInChildren(type, true);
				if (componentList == null) continue;
				if (componentList.Length <= 0) continue;

				FinderData data = new FinderData();
				data.path = file;
				foreach (Component component in componentList)
				{
					if (func == null || func.Invoke(component))
						data.componentPath.Add(GetComponentPath(obj.transform, component.transform));
				}
				if (data.componentPath.Count > 0)
					output.Add(data);
			}
		}
		finally
		{
			EditorUtility.ClearProgressBar();
		}
		return output;
	}
	public static string GetComponentPath(Transform root, Transform component)
	{
		string Path = "";
		Transform current = component.transform;
		while (current != root.transform)
		{
			Path = Path.Insert(0, current.name);
			if (string.IsNullOrEmpty(Path) == false && current.parent != root.transform)
				Path = Path.Insert(0, "/");
			current = current.parent;
		}
		return Path;
	}
}

public class ComponentFinderWindow : EditorWindow
{
	List<FinderData> editorFinderDatas = new List<FinderData>();
	string ComponentName;
	Type ComponentType;
	List<ComponentFieldInfo> ComponentFields = new List<ComponentFieldInfo>();

	// Unity Editor Window GUI
	Vector2 m_scrollPos = Vector2.zero;
	string[] ComponentFieldNames = null;


	TreeViewState state = new TreeViewState();
	ComponentFinderTreeView treeView = null;
	GUIContent FilterMemberNameGUIPopup = new GUIContent("Filter될 변수이름", "해당 변수 타입으로 변환할수 있는 static bool TryParse함수가 필요");
	GUIContent FilterMemberValueGUI = new GUIContent("Filter용 값", "static bool TryParse를 사용하여 각각의 변수 타입으로 전환될 예정");
	int filterPopupIndex = 0;
	string filterValue;
	bool isFilter = false;


	GUIContent SelectMemberNameGUIPopup = new GUIContent("변수이름", "해당 변수 타입으로 변환할수 있는 static bool TryParse함수가 필요");
	GUIContent SelectMemberValueGUI = new GUIContent("일괄적용값", "static bool TryParse를 사용하여 각각의 변수 타입으로 전환될 예정");
	int selectPopupIndex = 0;
	string changeValue;

	GUIContent FindComponentGUI = new GUIContent("검색할 컴포넌트 클래스", "namespace까지 넣은 class이름\n ex) Nsus.ZeroSpace.UI.Component.ZSTranslationForText");
	#region Mouse Right Click menu
	[MenuItem("Assets/ZeroSpace/ComponentFind", true)]
	private static bool SelectComponentMenuValidation()
	{
		MonoScript script = Selection.activeObject as MonoScript;
		return script != null;
	}
	[MenuItem("Assets/ZeroSpace/ComponentFind")]
	static private void SelectComponentMenu()
	{
		MonoScript script = Selection.activeObject as MonoScript;
		if (script == null) return;
		Type type = script.GetClass();
		if (type == null) return;
		ComponentFinderWindow window = GetWindow<ComponentFinderWindow>(false, nameof(ComponentFinderWindow));
		window.DoReset();
		window.Show();

		window.ComponentName = type.FullName;
		window.Find();
	}
	#endregion
	#region Menu 
	[MenuItem("ZeroSpace/Tools/Editor/ComponentFinderWindow")]
	static private void OpenWindow()
	{
		ComponentFinderWindow window = GetWindow<ComponentFinderWindow>(false, nameof(ComponentFinderWindow));
		window.DoReset();
		window.Show();
	}
	#endregion
	public void DoReset(bool isComponentNameRest = true)
	{
		if (isComponentNameRest == true)
		{
			ComponentName = string.Empty;
			ComponentType = null;
			ComponentFields.Clear();
			ComponentFieldNames = null;
		}
		editorFinderDatas.Clear();
		selectPopupIndex = 0;
		changeValue = string.Empty;
		treeView = null;
	}
	void OnEnable()
	{
		DoReset();
	}
	public void Find()
	{
		isFilter = false;
		DoReset(false);

		string finderPath = EditorUtility.OpenFolderPanel("검색 할 폴더", Application.dataPath, "");
		if (string.IsNullOrEmpty(finderPath)) return;
		editorFinderDatas = ComponentFinder.Find(ComponentName, finderPath);
		SetComponentType();
		if (editorFinderDatas == null || editorFinderDatas.Count <= 0)
		{
			EditorUtility.DisplayDialog("정보", "검색된 Component가 없습니다.", "확인");
			DoReset(false);
			return;
		}
		treeView = new ComponentFinderTreeView(editorFinderDatas, state)
		{
			ChangeValue = ChangeTreeViewValue
		};
	}
	private void OnGUI()
	{

		m_scrollPos = EditorGUILayout.BeginScrollView(m_scrollPos);

		EditorGUILayout.BeginVertical();
		ComponentName = EditorGUILayout.TextField(FindComponentGUI, ComponentName);
		if (string.IsNullOrEmpty(ComponentName) == false && GUILayout.Button("검색 폴더 지정")) Find();
		EditorGUILayout.EndVertical();

		EditorGUILayout.BeginHorizontal();

		EditorGUILayout.BeginVertical();
		if (treeView != null) treeView.OnGUI(new Rect(0, 50, position.width / 3 * 2, position.height - 50));

		EditorGUILayout.EndVertical();
		EditorGUILayout.Space(position.width / 3 * 2);

		EditorGUILayout.BeginVertical();
		if (ComponentType != null)
		{
			ComponentFilterOnGUI();
			ComponentFinderHelper.HorizontalLine(Color.grey);
			ComponentOnGUI();
		}
		EditorGUILayout.EndVertical();

		EditorGUILayout.EndHorizontal();
		EditorGUILayout.EndScrollView();
	}
	private void ChangeTreeViewValue(IList<int> SelectedList)
	{
		List<FinderData> list = treeView.GetSelectedItems();
		if (list.Count <= 0) return;
		UnityEngine.Object[] prefabs = new UnityEngine.Object[list.Count];
		for (int i = 0; i < list.Count; ++i)
		{
			prefabs[i] = AssetDatabase.LoadAssetAtPath<UnityEngine.Object>(list[i].path);
		}
		Selection.objects = prefabs;

		FinderData OpenAssetData = list.Find(obj => obj.componentPath.Count > 0);
		if (OpenAssetData == null) return;
		if (AssetDatabase.OpenAsset(prefabs[list.IndexOf(OpenAssetData)]) == false) return;
		PrefabStage prefabStage = PrefabStageUtility.GetCurrentPrefabStage();
		if (prefabStage == null) return;
		GameObject go = prefabStage.prefabContentsRoot;
		if (go == null) return;
		GameObject[] selectedObj = new GameObject[OpenAssetData.componentPath.Count];
		for (int i = 0; i < OpenAssetData.componentPath.Count; ++i)
		{
			selectedObj[i] = go.transform.Find(OpenAssetData.componentPath[i]).gameObject;
		}
		Selection.objects = selectedObj;
	}

	private void ComponentFilterOnGUI()
	{
		filterPopupIndex = EditorGUILayout.Popup(FilterMemberNameGUIPopup, filterPopupIndex, ComponentFieldNames);
		filterValue = EditorGUILayout.TextField(FilterMemberValueGUI, filterValue);
		if (string.IsNullOrEmpty(filterValue)) return;
		if (isFilter)
		{
			if (GUILayout.Button("초기화"))
			{
				isFilter = !isFilter;
				treeView.SetData(editorFinderDatas);
			}
		}
		else
		{
			if (GUILayout.Button("적용"))
			{
				isFilter = !isFilter;
				ComponentFieldInfo info = ComponentFields[filterPopupIndex];
				List<FinderData> list = editorFinderDatas;
				List<FinderData> ApplyList = new List<FinderData>();
				foreach (FinderData findItem in list)
				{
					if (findItem.componentPath.Count <= 0) continue;
					try
					{
						GameObject obj = AssetDatabase.LoadAssetAtPath<GameObject>(findItem.path) as GameObject;

						foreach (string componenetPath in findItem.componentPath)
						{
							Transform findTransform = obj.transform.Find(componenetPath);
							Component component = findTransform.gameObject.GetComponent(ComponentName);
							object Value = info.fieldInfo.GetValue(component);
							if (Value.ToString().ToLower().Contains(filterValue.ToLower()) == true)
							{
								FinderData applyItem = ApplyList.Find(item => item.path == findItem.path);
								if (applyItem == null) ApplyList.Add(applyItem = new FinderData() { path = findItem.path });
								applyItem.componentPath.Add(componenetPath);

							}
						}
					}
					finally
					{
					}
				}

				treeView.SetData(ApplyList);
			}
		}
	}

	private void ComponentOnGUI()
	{
		selectPopupIndex = EditorGUILayout.Popup(SelectMemberNameGUIPopup, selectPopupIndex, ComponentFieldNames);
		changeValue = EditorGUILayout.TextField(SelectMemberValueGUI, changeValue);

		if (GUILayout.Button("일괄 적용"))
		{
			ComponentFieldInfo info = ComponentFields[selectPopupIndex];
			object[] parameters = new object[] { changeValue, null };
			if (info.method == null)
				parameters[1] = changeValue;
			else if ((bool)info.method.Invoke(null, parameters) == false)
			{
				EditorUtility.DisplayDialog("에러", "TryParse이 실패했습니다.", "확인");
				return;
			}
			List<FinderData> list = treeView.GetSelectedItems();
			if (list == null || list.Count <= 0) list = treeView.Data;
			foreach (FinderData item in list)
			{
				if (item.componentPath.Count <= 0) continue;
				GameObject obj = null;
				try
				{
					obj = PrefabUtility.LoadPrefabContents(item.path);

					foreach (string componenetPath in item.componentPath)
					{
						Transform findTransform = obj.transform.Find(componenetPath);
						Component component = findTransform.gameObject.GetComponent(ComponentName);
						info.fieldInfo.SetValue(component, parameters[1]);
					}
					PrefabUtility.SaveAsPrefabAsset(obj, item.path);
				}
				finally
				{
					PrefabUtility.UnloadPrefabContents(obj);
				}
			}
			EditorUtility.DisplayDialog("정보", "적용완료", "확인");
		}
	}
	private bool SetComponentType()
	{
		ComponentType = ComponentFinderHelper.GetType(ComponentName);
		if (ComponentType == null) return false;
		ComponentFields.Clear();
		List<string> FieldNames = new List<string>();
		foreach (FieldInfo info in ComponentType.GetFields())
		{
			if (info.IsDefined(typeof(SerializeField), true) == false) continue;
			Type[] getMethodTypes = new Type[] { typeof(string), info.GetType() };
			MethodInfo method = info.GetType().GetMethod("TryParse", getMethodTypes);
			if (method == null && info.FieldType != typeof(string)) continue;
			ComponentFields.Add(new ComponentFieldInfo()
			{
				fieldInfo = info,
				method = method
			});
			FieldNames.Add(info.Name);

		}
		ComponentFieldNames = FieldNames.ToArray();
		return true;
	}
}

public class ComponentFinderTreeView : TreeView
{
	public List<FinderData> Data => editorFinderDatas;
	public Action<IList<int>> ChangeValue = null;

	private List<FinderData> editorFinderDatas;
	public ComponentFinderTreeView(List<FinderData> editorFinderDatas, TreeViewState state) : base(state) => SetData(editorFinderDatas);

	public ComponentFinderTreeView(TreeViewState state) : base(state) => Reload();
	public void SetData(List<FinderData> data)
	{
		this.editorFinderDatas = data;
		Reload();
	}
	protected override void SelectionChanged(IList<int> selectedIds)
	{
		base.SelectionChanged(selectedIds);
		ChangeValue?.Invoke(selectedIds);

	}

	protected override TreeViewItem BuildRoot()
	{
		TreeViewItem root = new TreeViewItem() { id = 0, depth = -1 };
		if (editorFinderDatas == null || editorFinderDatas.Count <= 0)
		{
			root.AddChild(new TreeViewItem() { depth = 0, id = 0, displayName = "Empty" });
			return root;
		}
		int Index = 1;
		foreach (FinderData finderData in editorFinderDatas)
		{
			TreeViewItem child = new TreeViewItem() { id = Index++, depth = 0, displayName = ComponentFinderHelper.ShortName(finderData.path) };
			foreach (string path in finderData.componentPath)
			{
				TreeViewItem pathChild = new TreeViewItem() { id = Index++, depth = 1, displayName = path };
				child.AddChild(pathChild);
			}
			root.AddChild(child);
		}
		return root;
	}

	public List<FinderData> GetSelectedItems()
	{
		IList<TreeViewItem> list = FindRows(GetSelection());

		List<FinderData> output = new List<FinderData>();
		foreach (TreeViewItem item in list)
		{
			TreeViewItem root = item.depth == 0 ? item : item.parent;
			FinderData findData = editorFinderDatas.Find(obj => ComponentFinderHelper.ShortName(obj.path) == root.displayName);
			FinderData data = output.Find(obj => obj.path == findData.path);
			if (data == null) output.Add(data = new FinderData() { path = findData.path });
			if (item.depth == 1) data.componentPath.Add(item.displayName);
			else
			{
				item.children.ForEach(path => data.componentPath.Add(path.displayName));
			}
		}
		return output;
	}
}


public static class ComponentFinderHelper
{
	public static string ShortName(string name) => name.Substring(name.LastIndexOf("/") + 1);
	public static Type GetType(string TypeName)
	{

		foreach (Assembly a in AppDomain.CurrentDomain.GetAssemblies())
		{
			if (a.GetType(TypeName) != null) return a.GetType(TypeName);
			foreach (Type type in a.GetTypes())
			{
				if (type.Name.ToLower().Contains(TypeName.ToLower())) return type;
			}
		}
		return null;
	}
	private static GUIStyle horizontalLine = new GUIStyle()
	{
		normal = new GUIStyleState()
		{
			background = EditorGUIUtility.whiteTexture,
		},
		margin = new RectOffset(0, 0, 4, 4),
		fixedHeight = 1
	};
	public static void HorizontalLine(Color color)
	{
		var c = GUI.color;
		GUI.color = color;
		GUILayout.Box(GUIContent.none, horizontalLine);
		GUI.color = c;
	}
}
