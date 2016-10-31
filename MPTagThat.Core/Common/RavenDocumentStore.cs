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

    #region Public Methods

    public static IDocumentStore GetDocumentStoreFor(string databaseName)
		{
			return Stores.GetOrAdd(databaseName, CreateDocumentStore).Value;
		}

	  public static void RemoveStore(string databasename)
	  {
	   Lazy<IDocumentStore> store = null;
	    Stores.TryRemove(databasename, out store);
	  }

    #endregion

    #region Private Methods

    private static Lazy<IDocumentStore> CreateDocumentStore(string databaseName)
		{
			return new Lazy<IDocumentStore>(() =>
			{
				var docStore = new EmbeddableDocumentStore()
				{
					UseEmbeddedHttpServer = false,
          DataDirectory = $"~\\Databases\\{databaseName}",
          RunInMemory = false,
				};
				docStore.Initialize();

        docStore.Conventions.MaxNumberOfRequestsPerSession = 1000000;
				docStore.Conventions.AllowMultipuleAsyncOperations = true;
			  
				return docStore;
			});
		}

    #endregion
  }
}
