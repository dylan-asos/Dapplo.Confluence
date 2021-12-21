// Copyright (c) Dapplo and contributors. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.


using Newtonsoft.Json;

namespace Dapplo.Confluence.Entities;

/// <summary>
///     Information on how to copy content
/// </summary>
[JsonObject]
public class CopyContent
{
    /// <summary>
    ///     If set to true, attachments are copied to the destination page.
    /// </summary>
    [JsonProperty("copyAttachments")]
    public bool CopyAttachments { get; set; }

    /// <summary>
    ///     If set to true, page permissions are copied to the destination page.
    /// </summary>
    [JsonProperty("copyPermissions")]
    public bool CopyPermissions { get; set; }

    /// <summary>
    ///     If set to true, content properties are copied to the destination page.
    /// </summary>
    [JsonProperty("copyProperties")]
    public bool CopyProperties { get; set; }

    /// <summary>
    ///     If set to true, labels are copied to the destination page.
    /// </summary>
    [JsonProperty("copyLabels")]
    public bool CopyLabels { get; set; }

    /// <summary>
    ///     If set to true, custom contents are copied to the destination page.
    /// </summary>
    [JsonProperty("copyCustomContents")]
    public bool CopyCustomContents { get; set; }

    /// <summary>
    ///     Defines where the page will be copied to
    /// </summary>
    [JsonProperty("destination")]
    public CopyPageRequestDestination Destination { get; set; }

    /// <summary>
    ///     If defined, this will replace the title of the destination page.
    /// </summary>
    [JsonProperty("pageTitle", DefaultValueHandling = DefaultValueHandling.Ignore)]
    public string PageTitle { get; set; }

    /// <summary>
    /// If defined, this will replace the body of the destination page.
    /// </summary>
    [JsonProperty("body", DefaultValueHandling = DefaultValueHandling.Ignore)]
    public Content Body { get; set; }
}