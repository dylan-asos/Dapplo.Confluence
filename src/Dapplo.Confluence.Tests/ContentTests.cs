// Copyright (c) Dapplo and contributors. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.


using System;
using System.Drawing;
using System.Linq;
using System.Threading.Tasks;
using Dapplo.Confluence.Entities;
using Dapplo.Confluence.Query;
using Dapplo.HttpExtensions;
using Dapplo.HttpExtensions.WinForms.ContentConverter;
using Dapplo.HttpExtensions.Wpf.ContentConverter;
using Dapplo.Log;
using Xunit;
using Xunit.Abstractions;

namespace Dapplo.Confluence.Tests;

/// <summary>
///     Tests
/// </summary>
[CollectionDefinition("Dapplo.Confluence")]
public class ContentTests : ConfluenceIntegrationTests
{

#pragma warning disable IDE0090 // Use 'new(...)'
    private static readonly LogSource Log = new LogSource();
#pragma warning restore IDE0090 // Use 'new(...)'
    public ContentTests(ITestOutputHelper testOutputHelper) : base(testOutputHelper)
    {
        // Add BitmapHttpContentConverter if it was not yet added
        if (HttpExtensionsGlobals.HttpContentConverters.All(x => x.GetType() != typeof(BitmapHttpContentConverter)))
        {
            HttpExtensionsGlobals.HttpContentConverters.Add(BitmapHttpContentConverter.Instance.Value);
        }
        // Add BitmapSourceHttpContentConverter if it was not yet added
        if (HttpExtensionsGlobals.HttpContentConverters.All(x => x.GetType() != typeof(BitmapSourceHttpContentConverter)))
        {
            HttpExtensionsGlobals.HttpContentConverters.Add(BitmapSourceHttpContentConverter.Instance.Value);
        }
    }

    [Fact]
    public async Task Test_IsDefault()
    {
        var query = Where.And(Where.Space.Is("TEST"), Where.Type.IsPage, Where.Title.Contains("Doesn't exist"));
        var searchResults = await ConfluenceTestClient.Content.SearchAsync(query);

        var searchResult = searchResults.FirstOrDefault();

        Assert.True(searchResult == default);
    }

    [Fact]
    public async Task Test_ContentVersion()
    {
        var query = Where.And(Where.Space.Is("TEST"), Where.Type.IsPage, Where.Title.Contains("Test Home"));
        var searchResults = await ConfluenceTestClient.Content.SearchAsync(query);

        var searchResult = searchResults.First();
        Log.Info().WriteLine("Version = {0}", searchResult.Version.Number);
        query = Where.Title.Contains("Test Home");
        searchResults = await ConfluenceTestClient.Content.SearchAsync(query);
        searchResult = searchResults.First();
        Log.Info().WriteLine("Version = {0}", searchResult.Version.Number);

        var content = await ConfluenceTestClient.Content.GetAsync(searchResult, ConfluenceClientConfig.ExpandGetContentWithStorage);
        Log.Info().WriteLine("Version = {0}", content.Version.Number);
    }

    /// <summary>
    ///     Test GetAsync
    /// </summary>
    //[Fact]
#pragma warning disable xUnit1013 // Public method should be marked as test
    public async Task TestGetContent()
#pragma warning restore xUnit1013 // Public method should be marked as test
    {
        var content = await ConfluenceTestClient.Content.GetAsync(950274);
        Assert.NotNull(content);
        Assert.NotNull(content.Version);
        Assert.NotNull(content.Ancestors);
        Assert.True(content.Ancestors.Count > 0);
    }

    /// <summary>
    ///     Test UpdateAsync
    /// </summary>
    [Fact]
    public async Task TestContentUpdate()
    {
        var content = await ConfluenceTestClient.Content.GetAsync(550731777, ConfluenceClientConfig.ExpandGetContentForUpdate);
        Assert.NotNull(content);
        Assert.NotNull(content.Version);
        content.Body.Storage.Value += $"\r\nTesting 1 - 2 -3 {DateTimeOffset.Now}";
        content.Version = new Entities.Version { IsMinorEdit = false, Number = content.Version.Number + 1 };
        await ConfluenceTestClient.Content.UpdateAsync(content);
    }

    /// <summary>
    ///     Test .GetChildren
    /// </summary>
    [Fact]
    public async Task TestGetChildren()
    {
        var results = await ConfluenceTestClient.Content.GetChildrenAsync(550731777, new PagingInformation {Limit = 1});
        Assert.NotNull(results);
        Assert.True(results.HasNext);
        Assert.True(results.Size > 0);
    }

    /// <summary>
    ///     Test GetHistoryAsync
    /// </summary>
    //[Fact]
#pragma warning disable xUnit1013 // Public method should be marked as test
    public async Task TestGetContentHistory()
#pragma warning restore xUnit1013 // Public method should be marked as test
    {
        var history = await ConfluenceTestClient.Content.GetHistoryAsync(950274);
        Assert.NotNull(history);
        Assert.NotNull(history.CreatedBy);
    }

    [Fact]
    public async Task TestCreateContent()
    {
        var query = Where.And(Where.Space.Is("TEST"), Where.Type.IsPage, Where.Title.Contains("Testing 1 2 3"));
        var searchResults = await ConfluenceTestClient.Content.SearchAsync(query);
        var oldPage = searchResults.Results.FirstOrDefault();
        if (oldPage != null)
        {
            await ConfluenceTestClient.Content.DeleteAsync(oldPage);
        }
        await Task.Delay(1000);
        var page = await ConfluenceTestClient.Content.CreateAsync(ContentTypes.Page, "Testing 1 2 3", "TEST", "<p>This is a test</p>");
        Assert.NotNull(page);
        Assert.True(page.Id > 0);
        await Task.Delay(1000);
        await ConfluenceTestClient.Content.DeleteAsync(page);
    }

    //[Fact]
#pragma warning disable xUnit1013 // Public method should be marked as test
    public async Task TestDeleteContent()
#pragma warning restore xUnit1013 // Public method should be marked as test
    {
        await ConfluenceTestClient.Content.DeleteAsync(30375945);
    }

    [Fact]
    public async Task TestSearch()
    {
        ConfluenceClientConfig.ExpandSearch = new[] { "version", "space", "space.icon", "space.description", "space.homepage", "history.lastUpdated" };

        var searchResult = await ConfluenceTestClient.Content.SearchAsync(Where.And(Where.Type.IsPage, Where.Text.Contains("Test Home")), pagingInformation: new PagingInformation {Limit = 20});
        Assert.Equal(ContentTypes.Page, searchResult.First().Type);
        var uri = ConfluenceTestClient.CreateWebUiUri(searchResult.FirstOrDefault()?.Links);
        Assert.NotNull(uri);
    }

    [Fact]
    public async Task TestSearchAttachment()
    {
        ConfluenceClientConfig.ExpandSearch = new[] { "version", "space", "space.icon", "space.description", "space.homepage", "history.lastUpdated" };

        var query = Where.And(Where.Type.IsAttachment, Where.Text.Contains("404"));

        var searchResult = await ConfluenceTestClient.Content.SearchAsync(query, pagingInformation: new PagingInformation { Limit = 1 });
        var attachment = searchResult.First();
        Assert.Equal(ContentTypes.Attachment, attachment.Type);
        Assert.NotNull(ConfluenceTestClient.Attachment.CreateDownloadUri(attachment.Links));
        // I know the attachment is a bitmap, this should work
        var bitmap = await ConfluenceTestClient.Attachment.GetContentAsync<Bitmap>(attachment);
        Assert.True(bitmap.Width > 0);
    }

    [Fact]
    public async Task TestSearchLabels()
    {
        var searchResult = await ConfluenceTestClient.Content.SearchAsync(Where.And(Where.Type.IsPage, Where.Text.Contains("Test Home")), pagingInformation: new PagingInformation { Limit = 1 });
        var contentId = searchResult.First().Id;

        var labels = new[] { "test1", "test2" };
        await ConfluenceTestClient.Content.AddLabelsAsync(contentId, labels.Select(s => new Label { Name = s }));

        ConfluenceClientConfig.ExpandSearch = new[] { "version", "space", "space.icon", "space.description", "space.homepage", "history.lastUpdated", "metadata.labels" };

        searchResult = await ConfluenceTestClient.Content.SearchAsync(Where.And(Where.Type.IsPage, Where.Text.Contains("Test Home")), pagingInformation: new PagingInformation { Limit = 1 });
        var labelEntities = searchResult.First().Metadata.Labels.Results;

        Assert.NotEmpty(labelEntities);

        // Delete all
        foreach (var label in labelEntities)
        {
            await ConfluenceTestClient.Content.DeleteLabelAsync(contentId, label.Name);
        }
    }

    [Fact]
    public async Task TestLabels()
    {
        var searchResult = await ConfluenceTestClient.Content.SearchAsync(Where.And(Where.Type.IsPage, Where.Text.Contains("Test Home")), pagingInformation: new PagingInformation { Limit = 1 });
        var contentId = searchResult.First().Id;

        var labels = new[] { "test1", "test2" };
        await ConfluenceTestClient.Content.AddLabelsAsync(contentId, labels.Select(s => new Label { Name = s }));
        var labelsForContent = await ConfluenceTestClient.Content.GetLabelsAsync(contentId);
        Assert.Equal(labels.Length, labelsForContent.Count(label => labels.Contains(label.Name)));

        // Delete all
        foreach (var label in labelsForContent)
        {
            await ConfluenceTestClient.Content.DeleteLabelAsync(contentId, label.Name);
        }
    }
}