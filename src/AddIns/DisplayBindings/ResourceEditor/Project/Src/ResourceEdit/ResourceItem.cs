﻿// Copyright (c) AlphaSierraPapa for the SharpDevelop Team (for details please see \doc\copyright.txt)
// This code is distributed under the GNU LGPL (for details please see \doc\license.txt)

using System;
using System.Drawing;
using System.Resources;
using System.Windows.Forms;

namespace ResourceEditor
{
	public class ResourceItem
	{
		public ResourceItem(string name, object resourceValue)
		{
			this.Name = name;
			this.ResourceValue = resourceValue;
		}
		
		public ResourceItem(string name, object resourceValue, string comment)
		{
			this.Name = name;
			this.ResourceValue = resourceValue;
			this.Comment = comment;
		}
		
		public string Name { get; set; }
		
		public object ResourceValue { get; set; }
		
		public string Comment { get; set; }
		
		public int ImageIndex
		{
			get {
				if (this.ResourceValue == null) {
					return -1;
				}
				switch(this.ResourceValue.GetType().ToString()) {
					case "System.String":
						return 0;
					case "System.Drawing.Bitmap":
						return 1;
					case "System.Drawing.Icon":
						return 2;
					case "System.Windows.Forms.Cursor":
						return 3;
					case "System.Byte[]":
						return 4;
					default:
						return 5;
				}
			}
		}
		
		public override string ToString()
		{
			if (ResourceValue == null) {
				return "(Nothing/null)";
			}
			
			string type = ResourceValue.GetType().FullName;
			string tmp = String.Empty;
			
			switch (type) {
				case "System.String":
					tmp = ResourceValue.ToString();
					break;
				case "System.Byte[]":
					tmp = "[Size = " + ((byte[])ResourceValue).Length + "]";
					break;
				case "System.Drawing.Bitmap":
					Bitmap bmp = ResourceValue as Bitmap;
					tmp = "[Width = " + bmp.Size.Width + ", Height = " + bmp.Size.Height + "]";
					break;
				case "System.Drawing.Icon":
					Icon icon = ResourceValue as Icon;
					tmp = "[Width = " + icon.Size.Width + ", Height = " + icon.Size.Height + "]";
					break;
				case "System.Windows.Forms.Cursor":
					Cursor c = ResourceValue as Cursor;
					tmp = "[Width = " + c.Size.Width + ", Height = " + c.Size.Height + "]";
					break;
				case "System.Boolean":
					tmp = ResourceValue.ToString();
					break;
				default:
					tmp = ResourceValue.ToString();
					break;
			}
			return tmp;
		}
		
		public ResXDataNode ToResXDataNode(Func<Type, string> typeNameConverter = null)
		{
			var node = new ResXDataNode(Name, ResourceValue, typeNameConverter) {
				Comment = Comment
			};
			return node;
		}
	}
}
