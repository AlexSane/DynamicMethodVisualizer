// 
// Copyright (c) 2005-2013 TargetProcess. All rights reserved.
// TargetProcess proprietary/confidential. Use is subject to license terms. Redistribution of this file is strictly forbidden.
// 

using System;
using System.Collections.Generic;
using System.Text;
using System.Reflection;
using ClrTest.Reflection;
using System.Reflection.Emit;

namespace ReflectionDemos.ILParser
{
	public static class ILReaderFactory
	{
		public static IILReader GetReader(object target)
		{
			if (target is MethodBase)
			{
				return GetReader((MethodBase) target);
			}
			else if (target is Delegate)
			{
				var @delegate = (Delegate) target;
				return GetReader(@delegate.Method);
			}
			return null;
		}

		public static IILReader GetReader(MethodBase method)
		{
			if (method == null)
			{
				return null;
			}

			if (method is DynamicMethod)
			{
				return new DynamicILReader(method as DynamicMethod);
			}
			else
			{
				FieldInfo fieldInfo = method.GetType().GetField("m_owner", BindingFlags.NonPublic | BindingFlags.Instance);
				if (fieldInfo != null)
				{
					var value = fieldInfo.GetValue(method);
					return GetReader(value);
				}
				else
				{
					return new StaticILReader(method);
				}
			}
		}
	}
}