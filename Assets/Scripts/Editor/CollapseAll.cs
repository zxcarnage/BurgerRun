﻿using System.Collections.Generic;
using System.Reflection;
using UnityEditor;
using UnityEditor.IMGUI.Controls;
using UnityEditorInternal;
using UnityEngine.SceneManagement;

public static class CollapseAll
{
	private const BindingFlags INSTANCE_FLAGS = BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance;
	private const BindingFlags STATIC_FLAGS = BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Static;

	[MenuItem( "Assets/Collapse All", priority = 1000 )]
	private static void CollapseFolders()
	{
		EditorWindow projectWindow = typeof( EditorWindow ).Assembly.GetType( "UnityEditor.ProjectBrowser" ).GetField( "s_LastInteractedProjectBrowser", STATIC_FLAGS ).GetValue( null ) as EditorWindow;
		if( projectWindow )
		{
			object assetTree = projectWindow.GetType().GetField( "m_AssetTree", INSTANCE_FLAGS ).GetValue( projectWindow );
			if( assetTree != null )
				CollapseTreeViewController( projectWindow, assetTree, (TreeViewState) projectWindow.GetType().GetField( "m_AssetTreeState", INSTANCE_FLAGS ).GetValue( projectWindow ) );

			object folderTree = projectWindow.GetType().GetField( "m_FolderTree", INSTANCE_FLAGS ).GetValue( projectWindow );
			if( folderTree != null )
			{
				object treeViewDataSource = folderTree.GetType().GetProperty( "data", INSTANCE_FLAGS ).GetValue( folderTree, null );
				int searchFiltersRootInstanceID = (int) typeof( EditorWindow ).Assembly.GetType( "UnityEditor.SavedSearchFilters" ).GetMethod( "GetRootInstanceID", STATIC_FLAGS ).Invoke( null, null );
				bool isSearchFilterRootExpanded = (bool) treeViewDataSource.GetType().GetMethod( "IsExpanded", INSTANCE_FLAGS, null, new System.Type[] { typeof( int ) }, null ).Invoke( treeViewDataSource, new object[] { searchFiltersRootInstanceID } );

				CollapseTreeViewController( projectWindow, folderTree, (TreeViewState) projectWindow.GetType().GetField( "m_FolderTreeState", INSTANCE_FLAGS ).GetValue( projectWindow ), isSearchFilterRootExpanded ? new int[1] { searchFiltersRootInstanceID } : null );
				
				InternalEditorUtility.expandedProjectWindowItems = (int[]) treeViewDataSource.GetType().GetMethod( "GetExpandedIDs", INSTANCE_FLAGS ).Invoke( treeViewDataSource, null );

				TreeViewItem rootItem = (TreeViewItem) treeViewDataSource.GetType().GetField( "m_RootItem", INSTANCE_FLAGS ).GetValue( treeViewDataSource );
				if( rootItem.hasChildren )
				{
					foreach( TreeViewItem item in rootItem.children )
						EditorPrefs.SetBool( "ProjectBrowser" + item.displayName, (bool) treeViewDataSource.GetType().GetMethod( "IsExpanded", INSTANCE_FLAGS, null, new System.Type[] { typeof( int ) }, null ).Invoke( treeViewDataSource, new object[] { item.id } ) );
				}
			}
		}
	}

	[MenuItem( "GameObject/Collapse All", priority = 40 )]
	private static void CollapseGameObjects( MenuCommand command )
	{
		if( command.context )
		{
			EditorApplication.update -= CallCollapseGameObjectsOnce;
			EditorApplication.update += CallCollapseGameObjectsOnce;

			return;
		}

		EditorWindow hierarchyWindow = typeof( EditorWindow ).Assembly.GetType( "UnityEditor.SceneHierarchyWindow" ).GetField( "s_LastInteractedHierarchy", STATIC_FLAGS ).GetValue( null ) as EditorWindow;
		if( hierarchyWindow )
		{
#if UNITY_2018_3_OR_NEWER
			object hierarchyTreeOwner = hierarchyWindow.GetType().GetField( "m_SceneHierarchy", INSTANCE_FLAGS ).GetValue( hierarchyWindow );
#else
			object hierarchyTreeOwner = hierarchyWindow;
#endif
			object hierarchyTree = hierarchyTreeOwner.GetType().GetField( "m_TreeView", INSTANCE_FLAGS ).GetValue( hierarchyTreeOwner );
			if( hierarchyTree != null )
			{
				List<int> expandedSceneIDs = new List<int>( 4 );
				foreach( string expandedSceneName in (IEnumerable<string>) hierarchyTreeOwner.GetType().GetMethod( "GetExpandedSceneNames", INSTANCE_FLAGS ).Invoke( hierarchyTreeOwner, null ) )
				{
					Scene scene = SceneManager.GetSceneByName( expandedSceneName );
					if( scene.IsValid() )
						expandedSceneIDs.Add( scene.GetHashCode() );
				}

				CollapseTreeViewController( hierarchyWindow, hierarchyTree, (TreeViewState) hierarchyTreeOwner.GetType().GetField( "m_TreeViewState", INSTANCE_FLAGS ).GetValue( hierarchyTreeOwner ), expandedSceneIDs );
			}
		}
	}

	private static void CallCollapseGameObjectsOnce()
	{
		EditorApplication.update -= CallCollapseGameObjectsOnce;
		CollapseGameObjects( new MenuCommand( null ) );
	}

	private static void CollapseTreeViewController( EditorWindow editorWindow, object treeViewController, TreeViewState treeViewState, IList<int> additionalInstanceIDsToExpand = null )
	{
		object treeViewDataSource = treeViewController.GetType().GetProperty( "data", INSTANCE_FLAGS ).GetValue( treeViewController, null );
		List<int> treeViewSelectedIDs = new List<int>( treeViewState.selectedIDs );
		int[] additionalInstanceIDsToExpandArray;
		if( additionalInstanceIDsToExpand != null && additionalInstanceIDsToExpand.Count > 0 )
		{
			treeViewSelectedIDs.AddRange( additionalInstanceIDsToExpand );

			additionalInstanceIDsToExpandArray = new int[additionalInstanceIDsToExpand.Count];
			additionalInstanceIDsToExpand.CopyTo( additionalInstanceIDsToExpandArray, 0 );
		}
		else
			additionalInstanceIDsToExpandArray = new int[0];

		treeViewDataSource.GetType().GetMethod( "SetExpandedIDs", INSTANCE_FLAGS ).Invoke( treeViewDataSource, new object[] { additionalInstanceIDsToExpandArray } );
#if UNITY_2019_1_OR_NEWER
		treeViewDataSource.GetType().GetMethod( "RevealItems", INSTANCE_FLAGS ).Invoke( treeViewDataSource, new object[] { treeViewSelectedIDs.ToArray() } );
#else
		foreach( int treeViewSelectedID in treeViewSelectedIDs )
			treeViewDataSource.GetType().GetMethod( "RevealItem", INSTANCE_FLAGS ).Invoke( treeViewDataSource, new object[] { treeViewSelectedID } );
#endif

		editorWindow.Repaint();
	}

	[MenuItem( "CONTEXT/Component/Collapse All", priority = 1400 )]
	private static void CollapseComponents( MenuCommand command )
	{
		ActiveEditorTracker tracker = ActiveEditorTracker.sharedTracker;
		for( int i = 0, length = tracker.activeEditors.Length; i < length; i++ )
			tracker.SetVisible( i, 0 );

		EditorWindow.focusedWindow.Repaint();
	}
}