namespace CleanMicroserviceSystem.Authentication.Domain
{
    public static class ConfigurationContract
    {
        #region Headers

        public const string AuthenticationSchemeHeaderName = "authentication_scheme";
        public const string ClientAuthenticationSchemeHeaderValue = "client";
        #endregion

        #region Claims


        #endregion

        #region Clients

        public const string TethysClient = "Tethys";
        #endregion

        #region Api
        public const string ReadScope = "Read";
        public const string WriteScope = "Write";

        public const string ThemisAPIResource = "ThemisAPI";
        public const string ThemisAPIReadScope = $"{ThemisAPIResource}.{ReadScope}";
        public const string ThemisAPIWriteScope = $"{ThemisAPIResource}.{WriteScope}";
        #endregion
    }
}
