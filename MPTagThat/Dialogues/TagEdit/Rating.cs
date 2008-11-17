using System;
using System.Collections.Generic;
using System.Text;

namespace MPTagThat.TagEdit
{
  public class Rating
  {
    #region Variables
    private string _ratingUser;
    private string _rating;
    private string _ratingPlayCounter;

    public Rating()
    {

    }

    public Rating(string user, string rating, string playcount)
    {
      _ratingUser = user;
      _rating = rating;
      _ratingPlayCounter = playcount;
    }

    public string User
    {
      get { return _ratingUser; }
      set { _ratingUser = value; }
    }

    public string RatingValue
    {
      get { return _rating; }
      set { _rating = value; }

    }

    public string PlayCounter
    {
      get { return _ratingPlayCounter; }
      set { _ratingPlayCounter = value; }
    }
    #endregion
  }
}
