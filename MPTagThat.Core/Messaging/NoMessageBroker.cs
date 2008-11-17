using System;
using System.Collections.Generic;

namespace MPTagThat.Core
{
  class NoMessageBroker : IMessageBroker
  {
    #region IMessageBroker Members

    public IMessageQueue GetOrCreate(string queueName)
    {
      Queue q = new Queue();
      return q;
    }

    public IList<string> Queues
    {
      get
      {
        List<string> queueNames = new List<string>();
        return queueNames;
      }
    }

    #endregion
  }
}
