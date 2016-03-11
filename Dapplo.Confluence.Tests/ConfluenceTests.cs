﻿/*
	Dapplo - building blocks for desktop applications
	Copyright (C) 2015-2016 Dapplo

	For more information see: http://dapplo.net/
	Dapplo repositories are hosted on GitHub: https://github.com/dapplo

	This file is part of Dapplo.Confluence

	Dapplo.Confluence is free software: you can redistribute it and/or modify
	it under the terms of the GNU General Public License as published by
	the Free Software Foundation, either version 3 of the License, or
	(at your option) any later version.

	Dapplo.Confluence is distributed in the hope that it will be useful,
	but WITHOUT ANY WARRANTY; without even the implied warranty of
	MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
	GNU General Public License for more details.

	You should have received a copy of the GNU General Public License
	along with Dapplo.Confluence. If not, see <http://www.gnu.org/licenses/>.
 */

using Xunit;
using System;
using System.Threading.Tasks;
using Xunit.Abstractions;
using Dapplo.LogFacade;

namespace Dapplo.Confluence.Tests
{
	public class ConfluenceTests
	{
		// Test against a well known Confluence
		private static readonly Uri TestConfluenceUri = new Uri("https://confluence.cip4.org/");

		private readonly ConfluenceApi _confluenceApi;

		public ConfluenceTests(ITestOutputHelper testOutputHelper)
		{
			XUnitLogger.RegisterLogger(testOutputHelper, LogLevel.Verbose);
			_confluenceApi = new ConfluenceApi(TestConfluenceUri);
			//_confluenceApi.SetBasicAuthentication("<username>", "<password>");
		}

		[Fact]
		public async Task TestSearch()
		{
			var searchResult = await _confluenceApi.SearchAsync("text ~ \"CIP4\"");
			Assert.NotNull(searchResult);
			Assert.True(searchResult.Results.Count > 0);

			foreach (var content in searchResult.Results)
			{
				Assert.NotNull(content.Type);
			}
		}
	}
}
