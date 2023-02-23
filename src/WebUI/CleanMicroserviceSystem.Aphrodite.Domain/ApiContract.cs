namespace CleanMicroserviceSystem.Aphrodite.Domain
{
    public static class ApiContract
    {
        #region HttpClientNames

        public const string AphroditeHttpClientName = "AphroditeClient";
        public const string GatewayHttpClientName = "UranusClient";
        #endregion

        #region MicroserviceNames

        public const string GatewayUriPrefix = "Ocelot";

        public const string ThemisServiceName = "Themis";
        #endregion
    }
}
