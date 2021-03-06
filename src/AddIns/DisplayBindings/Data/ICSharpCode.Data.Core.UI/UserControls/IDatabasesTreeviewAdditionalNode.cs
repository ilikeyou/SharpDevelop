﻿// Copyright (c) AlphaSierraPapa for the SharpDevelop Team (for details please see \doc\copyright.txt)
// This code is distributed under the GNU LGPL (for details please see \doc\license.txt)

#region Usings

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections;

#endregion

namespace ICSharpCode.Data.Core.UI.UserControls
{
    public interface IDatabasesTreeViewAdditionalNode
    {
        string Name { get; }
        IEnumerable Items { get; }
        string DataTemplate { get; }
    }
}
