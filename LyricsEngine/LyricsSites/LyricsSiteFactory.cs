using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Reflection;
using System.Reflection.Emit;
using System.Threading;

namespace LyricsEngine.LyricsSites
{
    public static class LyricsSiteFactory
    {
        #region strings

        private const string NoPaymentprocessorHasBeenRegisteredWithTheIdentifier = "No PaymentProcessor has been registered with the identifier: ";
        private const string IdentifierCanNotBeNullOrEmpty = "identifier can not be null or empty";
        private const string Createinstance = "CreateInstance";

        #endregion strings

        #region reflection

        private static readonly Type ClassType = typeof (AbstractSite);
        private static readonly Type[] ConstructorArgs = {typeof (string), typeof (string), typeof(WaitHandle), typeof (int)};

        private static readonly Dictionary<string, Type> ClassRegistry = new Dictionary<string, Type>();
        private static readonly Dictionary<string, ConstructorDelegate> ClassConstructors = new Dictionary<string, ConstructorDelegate>();

        private delegate AbstractSite ConstructorDelegate(string artist, string title, WaitHandle mEventStopSiteSearches, int timeLimit);

        #endregion reflection

        #region constructors

        static LyricsSiteFactory()
        {
            var lyricSites = from b in Assembly.GetExecutingAssembly().GetTypes()
                                    where b.IsSubclassOf(ClassType)
                                    select b;

            foreach (var type in lyricSites)
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
        public static List<string> LyricsSitesNames()
        {
            return ClassRegistry.Keys.Where(identifier => CreateDummySite(identifier).SiteActive()).ToList();
        }

        /// <summary>
        /// Return all LRC sites
        /// </summary>
        /// <returns>List of LRC sites</returns>
        public static List<string> LrcLyricsSiteNames()
        {
            return LyricsSitesNames().Where(identifier => CreateDummySite(identifier).GetLyricType() == LyricType.Lrc).ToList();
        }


        /// <summary>
        /// Returns list of sites according to specific speed
        /// </summary>
        /// <param name="speed">site speed</param>
        /// <returns>List of sites with a speed at least as requested</returns>
        public static List<string> LyricsSitesBySpeed(SiteSpeed speed)
        {
            return LyricsSitesNames().Where(identifier => CreateDummySite(identifier).GetSiteSpeed() <= speed).ToList();
        }


        /// <summary>
        /// Create a Lyrics search site object by name
        /// </summary>
        /// <param name="identifier">Lyrics site name</param>
        /// <param name="artist">Artist</param>
        /// <param name="title">Title</param>
        /// <param name="mEventStopSiteSearches">Stop event</param>
        /// <param name="timeLimit">Time limit</param>
        /// <returns>Lyrics site object (implements ILyricSite)</returns>
        public static AbstractSite Create(string identifier, string artist, string title, WaitHandle mEventStopSiteSearches, int timeLimit)
        {
            if (String.IsNullOrEmpty(identifier))
            {
                throw new ArgumentException(IdentifierCanNotBeNullOrEmpty, identifier);
            }
            if (!ClassRegistry.ContainsKey(identifier))
            {
                throw new ArgumentException(NoPaymentprocessorHasBeenRegisteredWithTheIdentifier + identifier);
            }
            return Create(ClassRegistry[identifier], artist, title, mEventStopSiteSearches, timeLimit);
        }

        public static string GetBaseUrlFromSiteName(string site)
        {
            var abstractSite = CreateDummySite(site);
            return abstractSite != null ? abstractSite.BaseUrl : string.Empty;
        }

        #endregion public methods

        #region private methods

        /// <summary>
        /// Create site
        /// </summary>
        /// <param name="type">site identifier</param>
        /// <param name="artist">artist</param>
        /// <param name="title">title</param>
        /// <param name="mEventStopSiteSearches">stop event</param>
        /// <param name="timeLimit">time limit</param>
        /// <returns></returns>
        private static AbstractSite Create(Type type, string artist, string title, WaitHandle mEventStopSiteSearches, int timeLimit)
        {
            ConstructorDelegate del;

            if (ClassConstructors.TryGetValue(type.Name, out del))
            {
                return del(artist, title, mEventStopSiteSearches, timeLimit);
            }

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

            del = (ConstructorDelegate) dynamicMethod.CreateDelegate(typeof (ConstructorDelegate));
            ClassConstructors.Add(type.Name, del);
            return del(artist, title, mEventStopSiteSearches, timeLimit);
        }

        /// <summary>
        /// Create a dummy site from identifier
        /// </summary>
        /// <param name="identifier">site identifier</param>
        /// <returns>site (without any data)</returns>
        private static AbstractSite CreateDummySite(string identifier)
        {
            return Create(ClassRegistry[identifier], "", "", null, 0);
        }

        #endregion private methods
    }
}
