using System;
using System.Diagnostics;
using System.IO;
using System.Reflection;

namespace MPTagThat.Core
{
  /// <summary>
  /// Contains information about the calling method.
  /// </summary>
  public class CallingMethod
  {
    #region CONSTRUCTORS
    #region DEFAULT
    /// <summary>
    /// Gets the calling method.
    /// </summary>
    public CallingMethod()
      : this(null)
    {
    }
    #endregion DEFAULT

    #region IGNORE
    /// <summary>
    /// Gets the calling method, ignoring calls from the specified type.
    /// </summary>
    /// <param name="ignoreType">All calls made from this type will be ignored.
    /// Use this when wrapping this class in another class. OK if null.</param>
    public CallingMethod(Type ignoreType)
    {
      this.m_IgnoreType = ignoreType;
      this.Initialize();
    }
    #endregion IGNORE
    #endregion CONSTRUCTORS

    #region FILE
    #region NAME
    private string m_FileName;
    /// <summary>
    /// Gets the name of the file that contained the method.
    /// </summary>
    public string FileName
    {
      get
      {
        return this.m_FileName;
      }
    }
    #endregion NAME

    #region PATH
    private string m_FilePath;
    /// <summary>
    /// Gets the path of the file that contained the method.
    /// </summary>
    public string FilePath
    {
      get
      {
        return this.m_FilePath;
      }
    }
    #endregion PATH
    #endregion FILE

    #region IGNORE
    private Type m_IgnoreType;
    /// <summary>
    /// Gets the type that will be ignored.
    /// </summary>
    public Type IgnoreType
    {
      get
      {
        return this.m_IgnoreType;
      }
    }
    #endregion IGNORE

    #region INITIALIZE
    /// <summary>
    /// Initializes the calling method information.
    /// </summary>
    private void Initialize()
    {
      #region METHOD BASE
      MethodBase method = null;
      string ignoreName = this.m_IgnoreType == null ?
          null : this.m_IgnoreType.Name;
      #endregion METHOD BASE

      #region STACK TRACE
      StackFrame stackFrame = null;
      StackTrace stackTrace = new StackTrace(true);
      for (int i = 0; i < stackTrace.FrameCount; i++)
      {
        StackFrame sf = stackTrace.GetFrame(i);
        method = sf.GetMethod();
        string typeName = method.ReflectedType.Name;
        if (String.Compare(typeName, "CallingMethod") != 0 &&
            (ignoreName == null ||
            String.Compare(typeName, ignoreName) != 0))
        {
          stackFrame = sf;
          break;
        }
      }
      #endregion STACK TRACE

      #region METHOD
      method = stackFrame.GetMethod();
      this.m_Method = method;
      string methodString = method.ToString();
      #endregion METHOD

      #region SIGNATURE
      string returnName = null;
      string methodSignature = methodString;

      int splitIndex = methodString.IndexOf(' ');
      if (splitIndex > 0)
      {
        returnName = methodString.Substring(0, splitIndex);
        methodSignature = methodString.Substring(splitIndex + 1,
            methodString.Length - splitIndex - 1);
      }
      this.m_ReturnName = returnName;
      this.m_MethodSignature = methodSignature;
      #endregion SIGNATURE

      #region TYPE
      this.m_Type = method.ReflectedType;
      this.m_TypeName = this.m_Type.Name;
      this.m_TypeNameFull = this.m_Type.FullName;
      #endregion TYPE

      #region METHOD
      this.m_MethodName = method.Name;
      this.m_MethodNameFull = String.Concat(
          this.m_TypeNameFull, ".", this.m_MethodName);
      #endregion METHOD

      #region FILE
      this.m_LineNumber = stackFrame.GetFileLineNumber();

      string fileLine = null;
      this.m_FilePath = stackFrame.GetFileName();
      if (!String.IsNullOrEmpty(this.m_FilePath))
      {
        this.m_FileName = Path.GetFileName(this.m_FilePath);
        fileLine = String.Format("File={0}, Line={1}",
            this.m_FileName, this.m_LineNumber);
      }
      #endregion FILE

      #region FULL SIGNATURE
      this.m_MethodSignatureFull = String.Format("{0} {1}.{2}",
          returnName, this.m_TypeNameFull, this.m_MethodSignature);
      this.m_Text = String.Format("{0} [{1}]",
          this.m_MethodSignatureFull, fileLine);
      #endregion FULL SIGNATURE
    }
    #endregion INITIALIZE

    #region LINE NUMBER
    private int m_LineNumber;
    /// <summary>
    /// Gets the line number in the file that called the method.
    /// </summary>
    public int LineNumber
    {
      get
      {
        return this.m_LineNumber;
      }
    }
    #endregion LINE NUMBER

    #region METHOD
    #region NAME
    #region FULL
    private string m_MethodNameFull;
    /// <summary>
    /// Gets the full name of this method, with namespace.
    /// </summary>
    public string MethodNameFull
    {
      get
      {
        return this.m_MethodNameFull;
      }
    }
    #endregion FULL

    #region METHOD
    private MethodBase m_Method;
    /// <summary>
    /// Gets the calling method.
    /// </summary>
    public MethodBase Method
    {
      get
      {
        return this.m_Method;
      }
    }
    #endregion METHOD

    #region NORMAL
    private string m_MethodName;
    /// <summary>
    /// Gets the name of this method.
    /// </summary>
    public string MethodName
    {
      get
      {
        return this.m_MethodName;
      }
    }
    #endregion NORMAL
    #endregion NAME

    #region SIGNATURE
    #region FULL
    private string m_MethodSignatureFull;
    /// <summary>
    /// Gets the complete method signature
    /// with return type, full method name, and arguments.
    /// </summary>
    public string MethodSignatureFull
    {
      get
      {
        return this.m_MethodSignatureFull;
      }
    }
    #endregion FULL

    #region NORMAL
    private string m_MethodSignature;
    /// <summary>
    /// Gets the method name and arguments.
    /// </summary>
    public string MethodSignature
    {
      get
      {
        return this.m_MethodSignature;
      }
    }
    #endregion NORMAL
    #endregion SIGNATURE
    #endregion METHOD

    #region NAMESPACE
    /// <summary>
    /// Gets the namespace containing the object containing this method.
    /// </summary>
    public string Namespace
    {
      get
      {
        Type type = this.Type;
        return type == null ?
            null : type.Namespace;
      }
    }
    #endregion NAMESPACE

    #region RETURN
    private string m_ReturnName;
    /// <summary>
    /// Gets the name of the return type.
    /// </summary>
    public string ReturnName
    {
      get
      {
        return this.m_ReturnName;
      }
    }
    #endregion RETURN

    #region TEXT
    private string m_Text;
    /// <summary>
    /// Gets the full method signature, file and line number.
    /// </summary>
    public string Text
    {
      get
      {
        return this.m_Text;
      }
    }
    #endregion TEXT

    #region TO STRING
    /// <summary>
    /// Gets the full method signature, file and line number.
    /// </summary>
    public override string ToString()
    {
      return this.Text;
    }
    #endregion TO STRING

    #region TYPE
    #region FULL
    private string m_TypeNameFull;
    /// <summary>
    /// Gets the full name of the type that contains this method,
    /// including the namespace.
    /// </summary>
    public string TypeNameFull
    {
      get
      {
        return this.m_TypeNameFull;
      }
    }
    #endregion FULL

    #region NORMAL
    private string m_TypeName;
    /// <summary>
    /// Gets the name of the type that contains this method,
    /// not including the namespace.
    /// </summary>
    public string TypeName
    {
      get
      {
        return this.m_TypeName;
      }
    }
    #endregion NORMAL

    #region TYPE
    private Type m_Type;
    /// <summary>
    /// Gets the type that contains this method.
    /// </summary>
    public Type Type
    {
      get
      {
        return this.m_Type;
      }
    }
    #endregion TYPE
    #endregion TYPE
  }
}