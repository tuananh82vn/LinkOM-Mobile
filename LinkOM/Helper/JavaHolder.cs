﻿using System;

namespace LinkOM
{
	public class JavaHolder : Java.Lang.Object
	{
		public readonly object Instance;

		public JavaHolder(object instance)
		{
			Instance = instance;
		}
	} 
}

