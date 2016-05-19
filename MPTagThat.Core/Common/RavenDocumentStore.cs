using System;
using System.Collections.Concurrent;
using Raven.Client;
using Raven.Client.Document;
using Raven.Client.Embedded;

namespace MPTagThat.Core.Common
{
	public class RavenDocumentStore
	{
		private readonly static ConcurrentDictionary<string, Lazy<IDocumentStore>> Stores = 
						new ConcurrentDictionary<string, Lazy<IDocumentStore>>();

		public static IDocumentStore GetDocumentStoreFor(string databaseName)
		{
			return Stores.GetOrAdd(databaseName, CreateDocumentStore).Value;
		}

		private static Lazy<IDocumentStore> CreateDocumentStore(string databaseName)
		{
			return new Lazy<IDocumentStore>(() =>
			{
				var docStore = new EmbeddableDocumentStore()
				{
					UseEmbeddedHttpServer = true,
					DefaultDatabase = databaseName,
          RunInMemory = false,
				};
				docStore.Initialize();

        docStore.Configuration.MaxPageSize = 50;
        docStore.Conventions.MaxNumberOfRequestsPerSession = 1000000;
				docStore.Conventions.AllowMultipuleAsyncOperations = true;
			  
				return docStore;
			});
		}
	}
}
