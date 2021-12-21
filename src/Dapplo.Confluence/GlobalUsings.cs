// Copyright (c) Dapplo and contributors. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.


global using System;
global using System.Threading;
global using System.Threading.Tasks;
global using System.Net;
global using System.Collections.Generic;
global using System.Linq;
global using System.Text;

#if NET471 || NET461
global using System.Net.Cache;
#endif
global using Dapplo.Log;
global using Dapplo.Confluence.Entities;
global using Dapplo.HttpExtensions;
global using Dapplo.HttpExtensions.JsonNet;
global using Dapplo.Confluence.Internals;
global using Dapplo.Confluence.Query;
global using Dapplo.HttpExtensions.Extensions;