namespace BeatTogether.MasterServer.Kernel.Configuration
{
    public class MasterServerConfiguration
    {
        public string EndPoint { get; set; } = "0.0.0.0:2328";
        public int SessionTimeToLive { get; set; } = 180;
        public string QuickPlayRegistrationPassword { get; set; } = "IWantToBeAQuickPlayServer";
    }
}
