﻿// Copyright (c) AlphaSierraPapa for the SharpDevelop Team (for details please see \doc\copyright.txt)
// This code is distributed under the GNU LGPL (for details please see \doc\license.txt)

using System;
using ICSharpCode.SharpDevelop;
using ICSharpCode.SharpDevelop.Gui;
using ICSharpCode.SharpDevelop.Project;

namespace ICSharpCode.PackageManagement
{
	public class ThreadSafeProjectBrowserUpdater : ProjectBrowserUpdater
	{
		public ThreadSafeProjectBrowserUpdater()
			: base(GetProjectBrowserControl())
		{
		}
		
		static ProjectBrowserControl GetProjectBrowserControl()
		{
			return SD.MainThread.InvokeIfRequired(() => ProjectBrowserPad.Instance.ProjectBrowserControl);
		}
		
		protected override void ProjectItemAdded(object sender, ProjectItemEventArgs e)
		{
			SD.MainThread.InvokeIfRequired(() => base.ProjectItemAdded(sender, e));
		}
	}
}
