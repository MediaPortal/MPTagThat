using System;

namespace LyricsEngine
{
  public interface ILyricForm
  {
    Object[] UpdateString { set; }
    Object[] UpdateStatus { set; }
    Object[] LyricFound { set; }
    Object[] LyricNotFound { set; }
    Object[] ThreadFinished { set; }
    string ThreadException { set; }
  }
}