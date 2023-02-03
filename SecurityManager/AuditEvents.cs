using System.Reflection;
using System.Resources;

namespace SecurityManager
{
	public enum AuditEventTypes
	{
		ActionSuccessful = 0,
		ActionFailed = 1,

	}

	public class AuditEvents
	{
		private static ResourceManager resourceManager = null;
		private static object resourceLock = new object();

		private static ResourceManager ResourceMgr
		{
			get
			{
				lock (resourceLock)
				{
					if (resourceManager == null)
					{
						resourceManager = new ResourceManager
							(typeof(AuditEventFile).ToString(),
							Assembly.GetExecutingAssembly());
					}
					return resourceManager;
				}
			}
		}
       
        public static string ActionSuccesful
		{
			get
			{
				return ResourceMgr.GetString(AuditEventTypes.ActionSuccessful.ToString());
			}
		}

		public static string ActionFailed
		{
			get
			{
				return ResourceMgr.GetString(AuditEventTypes.ActionFailed.ToString());
			}
		}
		
	}
}
