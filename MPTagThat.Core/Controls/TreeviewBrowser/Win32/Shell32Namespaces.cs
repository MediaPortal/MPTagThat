using System;

namespace Raccoom.Win32
{
	/// <summary>
	/// Summary description for Shell32Namespaces.
	/// </summary>
	public class Shell32Namespaces
	{
		#region fields
		private Shell32.Shell _shell;
		#endregion

		#region public interface
		public Shell32.Folder GetDesktop()
		{
			return Shell.NameSpace(Shell32.ShellSpecialFolderConstants.ssfDESKTOP);			
		}
		public Shell32.FolderItems GetEntries(Shell32.ShellSpecialFolderConstants shellFolder)
		{
			
			Shell32.Folder shell32Folder = Shell.NameSpace(shellFolder);
			return shell32Folder.Items();
		}
		#endregion				

		#region internal interface
		internal Shell32.Shell Shell
		{
			get
			{
				// create on demand
				if(_shell==null) _shell = new Shell32.ShellClass();				
				return _shell;
			}
		}
		#endregion		
	}
}
