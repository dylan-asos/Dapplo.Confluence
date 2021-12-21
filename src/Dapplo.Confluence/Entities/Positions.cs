// Copyright (c) Dapplo and contributors. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.


using System.Runtime.Serialization;

namespace Dapplo.Confluence.Entities;

/// <summary>
/// Positions which are used to describe where content is moved to
/// </summary>
public enum Positions
{
	/// <summary>
	/// Move the page under the same parent as the target, before the target in the list of children
	/// </summary>
	[EnumMember(Value = "before")] Before,
	/// <summary>
	/// Move the page under the same parent as the target, after the target in the list of children
	/// </summary>
	[EnumMember(Value = "after")] After,
	/// <summary>
	/// Move the page to be a child of the target
	/// </summary>
	[EnumMember(Value = "append")] Append
}