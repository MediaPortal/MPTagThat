#region Copyright (C) 2009-2011 Team MediaPortal
// Copyright (C) 2009-2011 Team MediaPortal
// http://www.team-mediaportal.com
// 
// MPTagThat is free software: you can redistribute it and/or modify
// it under the terms of the GNU General Public License as published by
// the Free Software Foundation, either version 2 of the License, or
// (at your option) any later version.
// 
// MPTagThat is distributed in the hope that it will be useful,
// but WITHOUT ANY WARRANTY; without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE. See the
// GNU General Public License for more details.
// 
// You should have received a copy of the GNU General Public License
// along with MPTagThat. If not, see <http://www.gnu.org/licenses/>.
#endregion

using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Reflection;
using System.Reflection.Emit;
using System.Threading;

namespace MPTagThat.Core.AlbumInfo.AlbumSites
{
	public static class AlbumSiteFactory
	{
		#region strings

		private const string NoPaymentprocessorHasBeenRegisteredWithTheIdentifier = "No PaymentProcessor has been registered with the identifier: ";
		private const string IdentifierCanNotBeNullOrEmpty = "identifier can not be null or empty";
		private const string Createinstance = "CreateInstance";

		#endregion strings

		#region reflection

		private static readonly Type ClassType = typeof(AbstractAlbumSite);
		private static readonly Type[] ConstructorArgs = { typeof(string), typeof(string), typeof(WaitHandle), typeof(int) };

		private static readonly Dictionary<string, Type> ClassRegistry = new Dictionary<string, Type>();
		private static readonly Dictionary<string, ConstructorDelegate> ClassConstructors = new Dictionary<string, ConstructorDelegate>();

		private delegate AbstractAlbumSite ConstructorDelegate(string artist, string album, WaitHandle mEventStopSiteSearches, int timeLimit);

		#endregion reflection

		#region constructors

		static AlbumSiteFactory()
		{
			var albumSites = from b in Assembly.GetExecutingAssembly().GetTypes()
											 where b.IsSubclassOf(ClassType)
											 select b;

			foreach (var type in albumSites)
			{
				ClassRegistry.Add(type.Name, type);
			}
		}

		#endregion constructors

		#region public methods

		/// <summary>
		/// Gets the list of lyrics search sites
		/// </summary>
		/// <returns>List of lyrics search sites</returns>
		public static List<string> AlbumSitesNames()
		{
			return ClassRegistry.Keys.Where(identifier => CreateDummySite(identifier).SiteActive()).ToList();
		}


		/// <summary>
		/// Create a Lyrics search site object by name
		/// </summary>
		/// <param name="identifier">Lyrics site name</param>
		/// <param name="artist">Artist</param>
		/// <param name="album">Album</param>
		/// <param name="mEventStopSiteSearches">Stop event</param>
		/// <param name="timeLimit">Time limit</param>
		/// <returns>Lyrics site object (implements ILyricSite)</returns>
		public static AbstractAlbumSite Create(string identifier, string artist, string album, WaitHandle mEventStopSiteSearches, int timeLimit)
		{
			if (String.IsNullOrEmpty(identifier))
			{
				throw new ArgumentException(IdentifierCanNotBeNullOrEmpty, identifier);
			}
			if (!ClassRegistry.ContainsKey(identifier))
			{
				throw new ArgumentException(NoPaymentprocessorHasBeenRegisteredWithTheIdentifier + identifier);
			}
			return Create(ClassRegistry[identifier], artist, album, mEventStopSiteSearches, timeLimit);
		}

		#endregion

		#region Private methods

		/// <summary>
		/// Create site
		/// </summary>
		/// <param name="type">site identifier</param>
		/// <param name="artist">artist</param>
		/// <param name="album">album</param>
		/// <param name="mEventStopSiteSearches">stop event</param>
		/// <param name="timeLimit">time limit</param>
		/// <returns></returns>
		private static AbstractAlbumSite Create(Type type, string artist, string album, WaitHandle mEventStopSiteSearches, int timeLimit)
		{
			ConstructorDelegate del;

			if (ClassConstructors.TryGetValue(type.Name, out del))
			{
				return del(artist, album, mEventStopSiteSearches, timeLimit);
			}

			try
			{
				var dynamicMethod = new DynamicMethod(Createinstance, type, ConstructorArgs, ClassType);
				var ilGenerator = dynamicMethod.GetILGenerator();

				var constructorInfo = type.GetConstructor(ConstructorArgs);
				if (constructorInfo == null)
				{
					throw new NoNullAllowedException("constructorInfo");
				}

				ilGenerator.Emit(OpCodes.Ldarg_0);
				ilGenerator.Emit(OpCodes.Ldarg_1);
				ilGenerator.Emit(OpCodes.Ldarg_2);
				ilGenerator.Emit(OpCodes.Ldarg_3);
				ilGenerator.Emit(OpCodes.Newobj, constructorInfo);
				ilGenerator.Emit(OpCodes.Ret);

				del = (ConstructorDelegate)dynamicMethod.CreateDelegate(typeof(ConstructorDelegate));
				ClassConstructors.Add(type.Name, del);
				return del(artist, album, mEventStopSiteSearches, timeLimit);
			}
			catch (NullReferenceException)
			{
				return null;
			}
		}

		/// <summary>
		/// Create a dummy site from identifier
		/// </summary>
		/// <param name="identifier">site identifier</param>
		/// <returns>site (without any data)</returns>
		private static AbstractAlbumSite CreateDummySite(string identifier)
		{
			return Create(ClassRegistry[identifier], "", "", null, 0);
		}

		#endregion private methods
	}
}
