using System;
using System.IO;
using System.ComponentModel;
using System.Collections.Generic;
using System.Xml;
using ICSharpCode.Core;
using ICSharpCode.SharpDevelop.Gui;

namespace ICSharpCode.SharpDevelop.Project
{
	public enum ItemType {
		Unknown,
		
		// ReferenceProjectItem
		Reference,
		ProjectReference,
		COMReference,
		
		WebReferenceUrl,
		
		// FileProjectItem
		Compile,
		EmbeddedResource,
		None,
		Content,
		Folder,
		WebReferences,
		
		BootstrapperFile
	}
	
	public abstract class ProjectItem : LocalizedObject, IDisposable
	{
		string        include;
		PropertyGroup properties = new PropertyGroup();
		IProject      project    = null;
		
		[Browsable(false)]
		public abstract ItemType ItemType {
			get;
		}
		
		[Browsable(false)]
		public IProject Project {
			get {
				return project;
			}
			set {
				project = value;
				fileNameCache = null;
			}
		}
		
		[Browsable(false)]
		public string Include {
			get {
				return include;
			}
			set {
				include = value;
				fileNameCache = null;
			}
		}
		
		[Browsable(false)]
		public PropertyGroup Properties {
			get {
				return properties;
			}
		}
		
		string fileNameCache;
		
		[Browsable(false)]
		public virtual string FileName {
			get {
				if (fileNameCache == null)
					fileNameCache = Path.Combine(project.Directory, include);
				return fileNameCache;
			}
			set {
				fileNameCache = null;
				Include = FileUtility.GetRelativePath(project.Directory, value);
			}
		}
		
		[Browsable(false)]
		public virtual string Tag {
			get {
				return ItemType.ToString();
			}
		}
		
		public ProjectItem(IProject project)
		{
			this.project = project;
		}
		
		#region System.IDisposable interface implementation
		public virtual void Dispose()
		{
		}
		#endregion
		public override string ToString()
		{
			return String.Format("[ProjectItem: ItemType={0}, Include={1}, Properties={2}]",
			                     ItemType,
			                     Include,
			                     Properties);
		}
		
		static ProjectItem ReadItem(XmlTextReader reader, IProject project, string itemType)
		{
			ProjectItem newItem = ProjectItemFactory.CreateProjectItem(project, itemType);
			newItem.Include  = reader.GetAttribute("Include");
			if (!reader.IsEmptyElement) {
				PropertyGroup.ReadProperties(reader, newItem.Properties, itemType);
			}
			return newItem;
		}
		
		
		internal void WriteItem(XmlTextWriter writer)
		{
			writer.WriteStartElement(Tag);
			writer.WriteAttributeString("Include", Include);
			Properties.WriteProperties(writer);
			writer.WriteEndElement();
		}
		
		internal static void ReadItemGroup(XmlTextReader reader, IProject project, List<ProjectItem> items)
		{
			if (reader.IsEmptyElement) {
				return;
			}
			while (reader.Read()) {
				switch (reader.NodeType) {
					 case XmlNodeType.EndElement:
						if (reader.LocalName == "ItemGroup") {
							return;
						}
						break;
					case XmlNodeType.Element:
						items.Add(ReadItem(reader, project, reader.LocalName));
						break;
				}
			}
		}
		
		internal static void WriteItemGroup(XmlTextWriter writer, List<ProjectItem> items)
		{
			writer.WriteStartElement("ItemGroup");
			foreach (ProjectItem item in items) {
				item.WriteItem(writer);
			}
			writer.WriteEndElement();
		}
		
		public override void InformSetValue(LocalizedPropertyDescriptor localizedPropertyDescriptor, object component, object value)
		{
			Project.Save();
		}
	}
}
