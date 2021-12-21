// Copyright (c) Dapplo and contributors. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.


using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace Dapplo.Confluence.Entities;

/// <summary>
///     Defines where the page is copied to
/// </summary>
[JsonObject]
public class CopyPageRequestDestination
{
    /// <summary>
    ///     If set to true, attachments are copied to the destination page.
    /// </summary>
    [JsonProperty("type")]
    [JsonConverter(typeof(StringEnumConverter))]
    public CopyDestinations DestinationType { get; set; }

    /// <summary>
    /// The space key for space type, and content id for parent_page and existing_page
    /// </summary>
    [JsonProperty("value")]
    public string Value { get; set; }
}