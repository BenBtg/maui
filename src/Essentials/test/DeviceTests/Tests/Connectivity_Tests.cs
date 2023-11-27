using System.Linq;
using System.Threading.Tasks;
using Microsoft.Maui.ApplicationModel;
using Microsoft.Maui.Networking;
using Xunit;

namespace Microsoft.Maui.Essentials.DeviceTests
{
	[Category("Connectivity")]
	public class Connectivity_Tests
	{
		[Fact]
		public void Network_Access() =>
			Assert.Equal(NetworkAccess.Internet, Connectivity.NetworkAccess);

		[Fact]
		public void Connection_Profiles() =>
			Assert.True(Connectivity.ConnectionProfiles.Count() > 0);

		[Fact]
		public void Distict_Connection_Profiles()
		{
			var profiles = Connectivity.ConnectionProfiles;
			Assert.Equal(profiles.Count(), profiles.Distinct().Count());
		}

		[Fact]
		public async Task Test()
		{
			var current = Connectivity.Current.NetworkAccess;

			var thread = await Task.Run(() => Connectivity.Current.NetworkAccess);

			Assert.Equal(current, thread);
		}

		[Fact]
		public async Task Network_Access_MainThread_To_BackgroundThread()
		{
			NetworkAccess mainThreadResult = NetworkAccess.Unknown;
			NetworkAccess backgroundThreadResult = NetworkAccess.Unknown;

			MainThread.BeginInvokeOnMainThread(() =>
			{
				mainThreadResult = Connectivity.Current.NetworkAccess;
			});

			await Task.Run(() =>
			{
				backgroundThreadResult = Connectivity.Current.NetworkAccess;
			});

			Assert.Equal(NetworkAccess.Internet, mainThreadResult);
			Assert.Equal(mainThreadResult, backgroundThreadResult);
		}

		[Fact]
		public async Task Network_Access_Background_To_ThreadMainThread()
		{
			NetworkAccess mainThreadResult = NetworkAccess.Unknown;
			NetworkAccess backgroundThreadResult = NetworkAccess.Unknown;

			await Task.Run(() =>
			{
				backgroundThreadResult = Connectivity.Current.NetworkAccess;
			});

			MainThread.BeginInvokeOnMainThread(() =>
			{
				mainThreadResult = Connectivity.Current.NetworkAccess;
			});

			Assert.Equal(NetworkAccess.Internet, backgroundThreadResult);
			Assert.Equal(mainThreadResult, backgroundThreadResult);
		}
	}
}
