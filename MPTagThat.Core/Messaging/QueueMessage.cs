#region Copyright (C) 2009-2010 Team MediaPortal

// Copyright (C) 2009-2010 Team MediaPortal
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

#region

using System.Collections.Generic;

#endregion

namespace MPTagThat.Core
{
  /// <summary>
  ///   Message to be used in <see cref = "IMessageQueue" />
  /// </summary>
  public class QueueMessage
  {
    #region Protected fields

    protected IDictionary<string, object> _metaData = new Dictionary<string, object>();
    protected IMessageQueue _queue;

    #endregion

    #region Public properties

    /// <summary>
    ///   Gets or sets the queue this message will be send in.
    /// </summary>
    /// <value>The queue.</value>
    public IMessageQueue MessageQueue
    {
      get { return _queue; }
      set { _queue = value; }
    }

    /// <summary>
    ///   Gets or sets the message data. The message data is a generic dictionary special
    ///   data entries defined by the message queue.
    /// </summary>
    /// <value>The meta data.</value>
    public IDictionary<string, object> MessageData
    {
      get { return _metaData; }
      set { _metaData = value; }
    }

    #endregion
  }
}