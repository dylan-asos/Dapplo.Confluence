// Copyright (c) Dapplo and contributors. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System.Runtime.Serialization;

namespace Dapplo.Confluence.Entities;

/// <summary>
///     Defines where the page will be copied to, and can be one of the following types.
/// </summary>
public enum CopyDestinations
{
    /// <summary>
    /// Some default value
    /// </summary>
    [EnumMember(Value = "none")] None,
    /// <summary>
    /// Page will be copied as a child of the specified parent page
    /// </summary>
    [EnumMember(Value = "parent_page")] ParentPage,
    /// <summary>
    /// Page will be copied to the specified space as a root page on the space
    /// </summary>
    [EnumMember(Value = "space")] Space,
    /// <summary>
    /// Page will be copied and replace the specified page
    /// </summary>
    [EnumMember(Value = "existing_page")] ExistingPage
}