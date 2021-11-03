using UnityEngine;

namespace Scripts.Config
{
	public class ConfigurationLoaderBehaviour : MonoBehaviour
	{
		private void Start()
		{
			Configuration.Instance = Configuration.Load();
		}
	}
}
