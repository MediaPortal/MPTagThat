using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace MyLyrics
{
  [Serializable]
  public class LyricsDatabase : IDictionary<string, LyricsItem>, ISerializable
  {
    private readonly DateTime created;

    private readonly Dictionary<string, LyricsItem> db;
    private DateTime lastModified;

    public LyricsDatabase()
    {
      created = DateTime.Now;
      lastModified = DateTime.Now;
      db = new Dictionary<string, LyricsItem>();
    }

    #region Serialization methods

    protected LyricsDatabase(SerializationInfo info, StreamingContext context)
    {
      Dictionary<string, LyricsItem> dbTemp = new Dictionary<string, LyricsItem>();
      db = (Dictionary<string, LyricsItem>)info.GetValue("db", dbTemp.GetType());
      created = info.GetDateTime("created");
      lastModified = info.GetDateTime("lastModified");
    }

    public virtual void GetObjectData(SerializationInfo info, StreamingContext context)
    {
      info.AddValue("db", db);
      info.AddValue("created", created);
      info.AddValue("lastModified", lastModified);
    }

    #endregion

    #region IDictionary<string,LyricsItem> Members

    public void Add(string key, LyricsItem value)
    {
      db.Add(key, value);
    }

    public bool ContainsKey(string key)
    {
      return db.ContainsKey(key);
    }

    public ICollection<string> Keys
    {
      get { return db.Keys; }
    }

    public bool Remove(string key)
    {
      return db.Remove(key);
    }

    public bool TryGetValue(string key, out LyricsItem value)
    {
      return db.TryGetValue(key, out value);
    }

    public ICollection<LyricsItem> Values
    {
      get { return db.Values; }
    }

    public LyricsItem this[string key]
    {
      get { return db[key]; }
      set { db[key] = value; }
    }

    public void Add(KeyValuePair<string, LyricsItem> item)
    {
      db.Add(item.Key, item.Value);
    }

    public void Clear()
    {
      db.Clear();
    }

    public bool Contains(KeyValuePair<string, LyricsItem> item)
    {
      return db.ContainsKey(item.Key);
    }

    public void CopyTo(KeyValuePair<string, LyricsItem>[] array, int arrayIndex)
    {
      throw new Exception("The method or operation is not implemented.");
    }

    public int Count
    {
      get { return db.Count; }
    }

    public bool IsReadOnly
    {
      get { return false; }
    }

    public bool Remove(KeyValuePair<string, LyricsItem> item)
    {
      return db.Remove(item.Key);
    }

    public IEnumerator<KeyValuePair<string, LyricsItem>> GetEnumerator()
    {
      foreach (KeyValuePair<string, LyricsItem> kvp in db)
      {
        yield return kvp;
      }
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
      foreach (KeyValuePair<string, LyricsItem> kvp in db)
      {
        yield return kvp;
      }
    }

    #endregion

    public void SetLastModified()
    {
      lastModified = DateTime.Now;
    }
  }
}